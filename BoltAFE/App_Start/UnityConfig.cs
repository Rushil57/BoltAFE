using BoltAFE.Domain.Admin;
using BoltAFE.Domain.AFE;
using BoltAFE.Domain.Login;
using BoltAFE.Domain.UserMaster;
using BoltAFE.Repositories.Admin;
using BoltAFE.Repositories.AFE;
using BoltAFE.Repositories.Login;
using BoltAFE.Repositories.UserMaster;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BoltAFE
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<ILoginRepository, LoginRepository>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IUserMasterRepository, UserMasterRepository>();
            container.RegisterType<IAFERepository, AFERepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}