using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using StaticVoid.Repository;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Data;

namespace StaticVoid.Blog.Site.Controllers
{
    public class AccountController : BlogBaseController
    {
        private readonly OpenIdMembershipService _openIdMembership;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Invitation> _invitationRepository;
        private readonly IAttacher<User> _userAttacher;
        private readonly IAttacher<Securable> _securableAttacher;
        private readonly ISecurityHelper _securityHelper;

        public AccountController(
            OpenIdMembershipService membershipService, 
            IRepository<User> userRepository, 
            IRepository<Data.Blog> blogRepo, 
            IRepository<Invitation> invitationRepository,
            IAttacher<User> userAttacher,
            IAttacher<Securable> securableAttacher,
            ISecurityHelper securityHelper): base(blogRepo)
        {
            _openIdMembership = membershipService;
			_userRepository = userRepository;
            _securityHelper = securityHelper;
            _invitationRepository = invitationRepository;
            _userAttacher = userAttacher;
            _securableAttacher = securableAttacher;
        }

        public ActionResult Login()
        {
            var user = _openIdMembership.GetUser();
            if (user != null)
            {
				var authenticatedUser = _userRepository.EnsureUser(new User
			    {
					ClaimedIdentifier = user.ClaimedIdentifier,
					Email = user.Email,
					FirstName = user.FullName.Split(' ').First(),
					LastName = user.FullName.Split(' ').Last(),
                    CreatedVia = Request.Url.Authority
				});

                var cookie = _openIdMembership.CreateFormsAuthenticationCookie(user);
                HttpContext.Response.Cookies.Add(cookie);

                return new RedirectResult(Request.Params["ReturnUrl"] ?? "/");
            }
            return View();
        }

        [OpenIdAuthorize]
        public ActionResult Register(string token)
        {
            var invitation = _invitationRepository.GetActiveByToken(token, i=>i.Securable, i=>i.Securable.Members);

            if(invitation == null)
            {
                throw new HttpException(400, "Invalid token");
            }

            var user = _userRepository.GetCurrentUser(_securityHelper);

            var securable = invitation.Securable;

            if (!securable.Members.Any(u => u.Id == user.Id))
            {
                _securableAttacher.EnsureAttached(securable);
                _userAttacher.EnsureAttached(user);

                if (user.Securables == null)
                {
                    user.Securables = new List<Securable>();
                }
                user.Securables.Add(securable);

                _userRepository.Update(user);
            }
            invitation.AssignedToId = user.Id;

            _invitationRepository.Update(invitation);

            return RedirectToAction("Index", "Dashboard", new { Area="Manage" });
        }

        [HttpPost]
        public ActionResult Login(string openid_identifier)
        {
            var response = _openIdMembership.ValidateAtOpenIdProvider(openid_identifier);

            if (response != null)
            {
                return response.RedirectingResponse.AsActionResult();
            }

            return View();
        }

        [OpenIdAuthorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Post");
        }
    }
}
