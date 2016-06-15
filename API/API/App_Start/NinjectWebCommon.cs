using System.Reflection;
using System.Web.Http;
using API.Controllers;
using API.DependencyResolution;
using API.Models;
using ShortBus;
using ShortBus.Ninject;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(API.App_Start.NinjectWebCommon), "Stop")]

namespace API.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                kernel.Bind<IMediator>().To<Mediator>();

                kernel.Bind(x => x.FromThisAssembly()
                .SelectAllClasses()
                .InheritedFromAny(
                    new[]
                    {
                        typeof(ICommandHandler<>), 
                        typeof(IQueryHandler<,>)
                    })
                .BindDefaultInterfaces());

                kernel.Bind<IDependencyResolver>().ToMethod(x => DependencyResolver.Current);
                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                ShortBus.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Bind<ApiContext>().ToMethod(ctx => new ApiContext());
            kernel.Bind<ApiContext>().ToSelf().InRequestScope();
        }
    }
}
