using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web.Http;
using API.Controllers;
using API.DependencyResolution;
using API.Infraestructure.Category;
using API.Infraestructure.Product;
using API.Models;
using MediatR;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings.Resolvers;


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
                
                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

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
            kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();

            kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
            kernel.Bind(scan => scan.From(Assembly.GetExecutingAssembly()).SelectAllClasses().InNamespaceOf(typeof (ListCategories.QueryHandler)).BindAllInterfaces());
            kernel.Bind(scan => scan.From(Assembly.GetExecutingAssembly()).SelectAllClasses().InNamespaceOf(typeof (ListProducts.QueryHandler)).BindAllInterfaces());

            kernel.Bind<ApiContext>().ToSelf().InRequestScope();

            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));
            
        }
    }
}
