using StaticVoid.Blog.Data;
using StaticVoid.Blog.Email;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [CurrentBlogAdminAuthorize]
    public class PermissionsEditorController : ManageBaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly IRepository<Securable> _securableRepo;
        private readonly IRepository<Invitation> _invitationRepo;
        private readonly ISecurityHelper _securityHelper;
        private readonly IAttacher<Securable> _securableAttacher;
        private readonly IAttacher<Data.Blog> _blogAttacher;
        private readonly IInvitationService _invitationService;

        public PermissionsEditorController(
            IRepository<User> userRepo,
            IRepository<Data.Blog> blogRepo,
            IRepository<Securable> securableRepo,
            IRepository<Invitation> invitationRepo,
            ISecurityHelper securityHelper,
            IInvitationService invitationService,
            IAttacher<Securable> securableAttacher,
            IAttacher<Data.Blog> blogAttacher,
            IHttpContextService httpContext)
            : base(blogRepo, httpContext, securityHelper, userRepo, securableRepo)
        {
            _userRepo = userRepo;
            _blogRepo = blogRepo;
            _securableRepo = securableRepo;
            _invitationRepo = invitationRepo;
            _securityHelper = securityHelper;
            _invitationService = invitationService;
            _blogAttacher = blogAttacher;
            _securableAttacher = securableAttacher;
        }

        public ActionResult EditPermissions(int? blogId = null)
        {
			var currentUser = _securityHelper.CurrentUser;

            var ownedBlogs = _securableRepo.BlogsUserIsAdminOf(_blogRepo, currentUser.Id).Where(b=> !blogId.HasValue || b.Id== blogId.Value);

            var model = new EditPermissionsModel{ PermissionModels = new List<PermissionModel>()};

            foreach(var blog in ownedBlogs)
            {
                //TODO make this not O(n) this only really matters for performance of superusers
                var authors = _securableRepo.GetBy(s => s.Id == blog.AuthorSecurableId, s => s.Members).Members.Select(m => new PermissionMemberModel
                {
                    CanChange = m.Id != currentUser.Id,
                    UserId = m.Id,
                    Name = m.FullName(),
                    Email = m.Email
                }).ToList();

                foreach (var invitation in _invitationRepo.GetActiveBySecurable(blog.AuthorSecurableId))
                {
                    authors.Add(new PermissionMemberModel{
                        Email = invitation.Email,
                        UserId = 0,
                        CanChange = true
                    });
                }

                var admins = _securableRepo.GetBy(s => s.Id == blog.AdminSecurableId, s => s.Members).Members.Select(m => new PermissionMemberModel
                {
                    CanChange = m.Id != currentUser.Id,
                    UserId = m.Id,
                    Name = m.FullName(),
                    Email = m.Email
                }).ToList();

                foreach (var invitation in _invitationRepo.GetActiveBySecurable(blog.AdminSecurableId))
                {
                    authors.Add(new PermissionMemberModel
                    {
                        Email = invitation.Email,
                        UserId = 0,
                        CanChange = true
                    });
                }

                model.PermissionModels.Add(new PermissionModel
                {
                    PermissionName = String.Format("Blog Admins: {0}", blog.Name),
                    SecurableId = blog.AdminSecurableId,
                    Members = admins
                });
                
                model.PermissionModels.Add(new PermissionModel
                {
                    PermissionName = String.Format("Blog Authors: {0}", blog.Name),
                    SecurableId = blog.AuthorSecurableId,
                    Members = authors
                });
            }

            return PartialView("EditPermissionsModal", model);
        }

        [HttpPost]
        public JsonResult Invite(InviteModel model)
        {
			var currentUser = _securityHelper.CurrentUser;
            if (!_securableRepo.BlogsUserIsAdminOf(_blogRepo, currentUser.Id).Any(b => b.AuthorSecurableId == model.SecurableId))
            {
                throw new HttpException(403, "Not Authorised");
            }
            _invitationService.Invite(model.SecurableId, model.Email, currentUser.FullName());

            return Json(true);
        }

        [HttpPost]
        public JsonResult Revoke(RevokeModel model)
        {
			var currentUser = _securityHelper.CurrentUser;
            if (!_securableRepo.BlogsUserIsAdminOf(_blogRepo, currentUser.Id).Any(b => b.AuthorSecurableId == model.SecurableId))
            {
                throw new HttpException(403, "Not Authorised");
            }

            if (model.UserId == 0)
            {
                _invitationRepo.RevokeInvitation(model.Email, model.SecurableId);
            }
            else
            {
                _securableRepo.RemoveMember(_securableAttacher, model.UserId, model.SecurableId);
            }

            return Json(true);
        }

    }
}
