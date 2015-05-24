using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StaticVoid.Blog.Site
{
	public partial class Startup
	{
		private static bool IsAjaxRequest(IOwinRequest request)
		{
			IReadableStringCollection query = request.Query;
			if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
			{
				return true;
			}
			IHeaderDictionary headers = request.Headers;
			return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
		}

		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				CookieName = "staticvoid.blog",
				ExpireTimeSpan = new TimeSpan(60, 0, 0, 0),
				SlidingExpiration = true,
				Provider = new CookieAuthenticationProvider
				{
					OnApplyRedirect = ctx =>
					{
						if (!IsAjaxRequest(ctx.Request))
						{
							ctx.Response.Redirect(ctx.RedirectUri);
						}
					}
				}
			});

			// Use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			//if (ConfigurationManager.AppSettings["auth.twitter.id"] != null && ConfigurationManager.AppSettings["auth.twitter.secret"] != null)
			//{
			//	var twitter = new TwitterAuthenticationOptions
			//	{
			//		ConsumerKey = ConfigurationManager.AppSettings["auth.twitter.id"],
			//		ConsumerSecret = ConfigurationManager.AppSettings["auth.twitter.secret"],
			//		Provider = new TwitterAuthenticationProvider
			//		{
			//			OnAuthenticated = ctx =>
			//			{
			//				return Task.Run(() =>
			//				{
			//					if (ctx.ScreenName != null)
			//					{

			//					}
			//				});
			//			}
			//		}
			//	};
			//	app.UseTwitterAuthentication(twitter);
			//}

			//if (ConfigurationManager.AppSettings["auth.microsoft.id"] != null && ConfigurationManager.AppSettings["auth.microsoft.secret"] != null)
			//{
			//	var ms = new MicrosoftAccountAuthenticationOptions()
			//	{
			//		ClientId = ConfigurationManager.AppSettings["auth.microsoft.id"],
			//		ClientSecret = ConfigurationManager.AppSettings["auth.microsoft.secret"],
			//		SignInAsAuthenticationType = "ExternalCookie",
			//		Provider = new MicrosoftAccountAuthenticationProvider
			//		{
			//			OnAuthenticated = ctx =>
			//			{
			//				return Task.Run(() =>
			//				{
			//					if (ctx.User["emails"]["preferred"] != null)
			//					{
			//						ctx.Identity.AddClaim(new Claim(ClaimTypes.Email, ctx.User["emails"]["preferred"].ToString()));
			//					}
			//					if (ctx.User["id"] != null)
			//					{
			//						ctx.Identity.AddClaim(new Claim(SecurityHelper.SmallProfilePictureUrl, String.Format("https://apis.live.net/v5.0/{0}/picture?small", ctx.User["id"])));
			//						ctx.Identity.AddClaim(new Claim(SecurityHelper.LargeProfilePictureUrl, String.Format("https://apis.live.net/v5.0/{0}/picture?large", ctx.User["id"])));
			//					}
			//				});
			//			}
			//		}
			//	};
			//	ms.Scope.Add("wl.basic");
			//	ms.Scope.Add("wl.emails");

			//	app.UseMicrosoftAccountAuthentication(ms);
			//}


			if (ConfigurationManager.AppSettings["auth.google.id"] != null && ConfigurationManager.AppSettings["auth.google.secret"] != null)
			{
				var google = new GoogleOAuth2AuthenticationOptions
				{
					ClientId = ConfigurationManager.AppSettings["auth.google.id"],
					ClientSecret = ConfigurationManager.AppSettings["auth.google.secret"],
					Provider = new GoogleOAuth2AuthenticationProvider
					{
						OnAuthenticated = ctx =>
						{
							return Task.Run(() =>
							{
								//if (ctx.User["image"] != null && ctx.User["image"]["url"] != null)
								//{
								//	ctx.Identity.AddClaim(new Claim(SecurityHelper.SmallProfilePictureUrl, ctx.User["image"]["url"].ToString().Split('?')[0] + "?sz=" + smallImageSize));
								//	ctx.Identity.AddClaim(new Claim(SecurityHelper.LargeProfilePictureUrl, ctx.User["image"]["url"].ToString().Split('?')[0] + "?sz=" + largeImageSize));
								//}
								//else if (ctx.Identity.FindFirstValue(ClaimTypes.Email) != null)
								//{
								//	ctx.Identity.AddClaim(new Claim(SecurityHelper.SmallProfilePictureUrl, ctx.Identity.FindFirstValue(ClaimTypes.Email).GravitarUrlFromEmail() + "?s=" + smallImageSize));
								//	ctx.Identity.AddClaim(new Claim(SecurityHelper.LargeProfilePictureUrl, ctx.Identity.FindFirstValue(ClaimTypes.Email).GravitarUrlFromEmail() + "?s=" + largeImageSize));
								//}
							});
						}
					}
				};

				google.Scope.Add("email");
				google.Scope.Add("profile");

				app.UseGoogleAuthentication(google);
			}
		}
	}
}