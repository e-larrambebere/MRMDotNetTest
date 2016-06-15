using System.Web.Http.Dependencies;
using API.App_Start;
using Ninject;

namespace API.DependencyResolution
{
    public class NinjectResolver : NinjectScope,
        System.Web.Http.Dependencies.IDependencyResolver,
        System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(kernel.BeginBlock());
        }
    }
}