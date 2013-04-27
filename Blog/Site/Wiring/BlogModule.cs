
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
            Kernel.BindFilter<MyAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<AuthorAuthorizeAttribute>();
            Kernel.BindFilter<MyAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<AuthorAuthorizeAttribute>();
        }
    }
}