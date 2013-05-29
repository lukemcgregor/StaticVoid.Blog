﻿
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

namespace StaticVoid.Blog.Site.Wiring
{
    public class BlogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>().To<BlogContext>().InRequestScope();
            Bind<IProvideDateTime>().To<DateTimeProvider>();
            Bind<IVisitLoggerService>().To<VisitLoggerService>();
            Bind<OpenIdMembershipService>().ToSelf().InTransientScope();
            Bind<ISecurityHelper>().To<SecurityHelper>();
            Bind<ISendEmail>().To<EmailSender>();
            Kernel.BindFilter<CurrentBlogAuthorAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<CurrentBlogAuthorAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAuthorAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<CurrentBlogAuthorAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAdminAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<CurrentBlogAdminAuthorizeAttribute>();
            Kernel.BindFilter<CurrentBlogAdminAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<CurrentBlogAdminAuthorizeAttribute>();
            Kernel.BindFilter<PlatformAdminAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<PlatformAdminAuthorizeAttribute>();
            Kernel.BindFilter<PlatformAdminAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<PlatformAdminAuthorizeAttribute>();
        }
    }
}