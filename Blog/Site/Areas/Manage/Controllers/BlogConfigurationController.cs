using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Controllers;
using StaticVoid.Blog.Site.Services;

namespace StaticVoid.Blog.Site.Areas.Manage.Controllers
{
    [CurrentBlogAuthorAuthorize]
    public class BlogConfigurationController : ManageBaseController
    {
        private readonly IRepository<Data.Blog> _blogRepo;
        private readonly IRepository<Data.Securable> _securableRepository;
        private readonly IRepository<Data.User> _userRepository;
        private readonly ISecurityHelper _securityHelper;

        public BlogConfigurationController(
            IRepository<Data.Blog> blogRepo, 
            IRepository<Securable> securableRepository, 
            IRepository<Data.User> userRepository, 
            ISecurityHelper securityHelper,
            IHttpContextService httpContext) : base(blogRepo, httpContext,securityHelper,userRepository,securableRepository)
        {
            _blogRepo = blogRepo;
            _securableRepository = securableRepository;
            _userRepository = userRepository;
            _securityHelper = securityHelper;
        }

        public ActionResult Edit(int? blogId)
        {
            Data.Blog blog = null;
            if(blogId.HasValue)
            {
                blog = _blogRepo.GetBy(b => b.Id == blogId.Value);

                if(!_userRepository.GetCurrentUser(_securityHelper).IsAdminOfBlog(blog,_securableRepository))
                {
                    throw new HttpException(403, "Not Authorized");
                }
            }
            else
            {
                blog = CurrentBlog;
            }
            return PartialView("EditModal", new BlogConfigModel
                {
                    AnalyticsKey = blog.AnalyticsKey,
                    AuthoritiveUrl = blog.AuthoritiveUrl,
                    Description = blog.Description,
                    DisqusShortname = blog.DisqusShortname,
                    Name = blog.Name,
                    Twitter = blog.Twitter,
                    BlogStyleId = blog.BlogTemplateId
                });
        }

        [HttpPost]
        public ActionResult Edit(int? blogId, BlogConfigModel model)
        {
            if (ModelState.IsValid)
            {
                Data.Blog blog = null;
                if (blogId.HasValue)
                {
                    blog = _blogRepo.GetBy(b => b.Id == blogId.Value);

                    if (!_userRepository.GetCurrentUser(_securityHelper).IsAdminOfBlog(blog, _securableRepository))
                    {
                        throw new HttpException(403, "Not Authorized");
                    }
                }
                else
                {
                    blog = CurrentBlog;
                }

                blog.AnalyticsKey = model.AnalyticsKey;
                blog.AuthoritiveUrl = model.AuthoritiveUrl;
                blog.Description = model.Description;
                blog.DisqusShortname = model.DisqusShortname;
                blog.Name = model.Name;
                blog.Twitter = model.Twitter;

                _blogRepo.Update(blog);

                return Json(new { success = true });
            }

            return PartialView("EditModal", model);
        }

    }
}
