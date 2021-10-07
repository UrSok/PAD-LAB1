using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.ViewModels
{
    public class ConnectionLostViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        public ConnectionLostViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;

            GoToStartCommand = new MvxCommand(GoToStart);
        }

        public IMvxCommand GoToStartCommand { get; set; }

        public void GoToStart()
        {
            mvxNavigationService.Navigate<StartUpViewModel>();
        }
    }
}
