[assembly: WebActivator.PreApplicationStartMethod(typeof(StaticVoid.Blog.Site.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(StaticVoid.Blog.Site.NinjectWebCommon), "Stop")]

namespace StaticVoid.Blog.Site
{
	using System;
	using System.Web;
	using System.Web.Mvc;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Web.Mvc;
	using Ninject.Web.Mvc.FilterBindingSyntax;
	using Ninject.Web.Common;
	using StaticVoid.Blog.Data;
	using StaticVoid.Blog.Site.Security;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
			var kernel = new StandardKernel();
			kernel.Load(new PersistanceModule());
			kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
			kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
			kernel.Bind<OpenIdMembershipService>().ToSelf().InTransientScope();
			kernel.BindFilter<MyAuthorizeFilter>(FilterScope.Action, 0).WhenActionMethodHas<AuthorAuthorizeAttribute>();
			kernel.BindFilter<MyAuthorizeFilter>(FilterScope.Controller, 0).WhenControllerHas<AuthorAuthorizeAttribute>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
