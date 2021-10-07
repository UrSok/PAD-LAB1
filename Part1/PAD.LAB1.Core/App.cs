using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.Services.Broker;
using PAD.LAB1.Core.Services.Client;
using PAD.LAB1.Core.ViewModels;

namespace PAD.LAB1.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IClientService>(() => new ClientService());
            Mvx.IoCProvider.RegisterSingleton<IBrokerService>(() => new BrokerService());

            RegisterAppStart<StartUpViewModel>();
        }
    }
}
