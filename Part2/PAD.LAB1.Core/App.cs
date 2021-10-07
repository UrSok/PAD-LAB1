using AutoMapper;
using MvvmCross;
using MvvmCross.ViewModels;
using PAD.LAB1.Broker.Utils;
using PAD.LAB1.Core.Services;
using PAD.LAB1.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IChatService>(() => new ChatService());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SharedMapperProfile());
            });
            Mvx.IoCProvider.RegisterSingleton(() => config.CreateMapper());

            RegisterAppStart<ConnectionViewModel>();
        }
    }
}
