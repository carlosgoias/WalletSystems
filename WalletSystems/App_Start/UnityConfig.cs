using System.Web.Http;
using Unity;
using Unity.WebApi;
using WalletSystems.Repository;
using WalletSystems.Repository.Interfaces;
using WalletSystems.Services;
using WalletSystems.Services.Interfaces;

namespace WalletSystems
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterSingleton<IWalletRepository, WalletRepository>();
            container.RegisterSingleton<IWalletService, WalletService>();


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}