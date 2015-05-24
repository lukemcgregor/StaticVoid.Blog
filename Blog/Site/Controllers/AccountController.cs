using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using StaticVoid.Repository;
using StaticVoid.Blog.Site.Security;
using StaticVoid.Blog.Data;
using StaticVoid.Blog.Site.Services;
using System.Collections.Generic;
using StaticVoid.Blog.Site.Gravitar;

namespace StaticVoid.Blog.Site.Controllers
{
	[Authorize]
	public class AccountController : BlogBaseController
	{
		//private readonly OpenIdMembershipService _openIdMembership;
		private readonly IBlogContext _context;
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<Invitation> _invitationRepository;
		private readonly IAttacher<User> _userAttacher;
		private readonly IAttacher<Securable> _securableAttacher;
		private readonly ISecurityHelper _securityHelper;

		public AccountController(
			//OpenIdMembershipService membershipService,
			IBlogContext context,
			IRepository<User> userRepository,
			IRepository<Data.Blog> blogRepo,
			IRepository<Invitation> invitationRepository,
			IAttacher<User> userAttacher,
			IAttacher<Securable> securableAttacher,
			ISecurityHelper securityHelper,
			IHttpContextService httpContext)
			: base(blogRepo, httpContext)
		{
			_context = context;
			_userRepository = userRepository;
			_invitationRepository = invitationRepository;
			_userAttacher = userAttacher;
			_securableAttacher = securableAttacher;
			_securityHelper = securityHelper;
		}


		public ActionResult Register(string token)
		{
			var invitation = _invitationRepository.GetActiveByToken(token, i => i.Securable, i => i.Securable.Members);

			if (invitation == null)
			{
				throw new HttpException(400, "Invalid token");
			}

			var user = _securityHelper.CurrentUser;

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

			return RedirectToAction("Index", "Dashboard", new { Area = "Manage" });
		}


		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (Request.IsAuthenticated)
			{
				return Redirect(returnUrl);
			}
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[AllowAnonymous]
		public ActionResult ExternalLogin(string provider, string returnUrl = "/")
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string notifyClientId)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToLocal(returnUrl);
			}

			var ctx = Request.GetOwinContext();
			var result = ctx.Authentication.AuthenticateAsync("ExternalCookie").Result;

			// Sign in the user with this external login provider if the user already has a login
			ProviderLogin providerLogin = _context.ProviderLogins.Include(p=>p.User).GetByProviderAndKey(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey);
			if (providerLogin != null)
			{
				var changes = false;

				if (String.IsNullOrWhiteSpace(providerLogin.User.FirstName))
				{
					changes = true;
					providerLogin.User.FirstName = result.Identity.FindFirstValue(ClaimTypes.GivenName);
					providerLogin.User.LastName = result.Identity.FindFirstValue(ClaimTypes.Surname);
				}

				if (String.IsNullOrWhiteSpace(providerLogin.User.LastName))
				{
					changes = true;
					providerLogin.User.FirstName = result.Identity.FindFirstValue(ClaimTypes.GivenName);
					providerLogin.User.LastName = result.Identity.FindFirstValue(ClaimTypes.Surname);
				}

				if (String.IsNullOrWhiteSpace(providerLogin.User.Email))
				{
					changes = true;
					providerLogin.User.FirstName = result.Identity.FindFirstValue(ClaimTypes.Email);
				}

				if (changes)
				{
					_context.SaveChanges();
				}

				SignIn(providerLogin.User, loginInfo.Login.LoginProvider, isPersistent: false);
			}
			else
			{
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();

				var user = new User()
				{
					CreatedVia = info.Login.LoginProvider,
					Email = result.Identity.FindFirstValue(ClaimTypes.Email),
					FirstName = result.Identity.FindFirstValue(ClaimTypes.GivenName),
					LastName = result.Identity.FindFirstValue(ClaimTypes.Surname)
				};
				_context.ProviderLogins.Add(new ProviderLogin
				{
					User = user,
					Provider = info.Login.LoginProvider.ParseProviderType(),
					ProviderKey = info.Login.ProviderKey
				});
				_context.SaveChanges();

				SignIn(user, info.Login.LoginProvider, isPersistent: true);
			}
			return Redirect(returnUrl);
		}
		
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();

			return RedirectToAction("Index", "Post");
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void SignIn(User user, string loginType, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

			ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName()));
			identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		public enum ManageMessageId
		{
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Default");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}
