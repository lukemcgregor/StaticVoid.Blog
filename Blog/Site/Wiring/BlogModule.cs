
using StaticVoid.Blog.Data;
using StaticVoid.Mockable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using StaticVoid.Blog.Site.Security;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Ninject.Web.Common;
using StaticVoid.Blog.Email;
using StaticVoid.Blog.Site.Services;
using StaticVoid.Blog.Site.Areas.Manage.Models;
using StaticVoid.Repository;

namespace StaticVoid.Blog.Site.Wiring
{
    public class BlogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>().To<BlogContext>().InRequestScope();
            Bind<IProvideDateTime>().To<DateTimeProvider>();
            Bind<OpenIdMembershipService>().ToSelf().InTransientScope();
            Bind<IRepositoryDataSource<TemporaryUploadedBlogBackup>>().To<InMemoryRepositoryDataSource<TemporaryUploadedBlogBackup>>().InSingletonScope();
            Bind<ISecurityHelper>().To<SecurityHelper>();
            Bind<ISendEmail>().To<EmailSender>();
            Bind<IInvitationService>().To<InvitationService>();
            Bind<IHttpContextService>().To<HttpContextService>();
            Kernel.BindFilter<CurrentBlogAuthorAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<CurrentBlogAuthorAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAuthorAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<CurrentBlogAuthorAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAdminAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<CurrentBlogAdminAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAdminAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<CurrentBlogAdminAuthorizeAttribute>();
            Kernel.BindFilter<PlatformAdminAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<PlatformAdminAuthorizeAttribute>();
            Kernel.BindFilter<PlatformAdminAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<PlatformAdminAuthorizeAttribute>();
        }
    }
}