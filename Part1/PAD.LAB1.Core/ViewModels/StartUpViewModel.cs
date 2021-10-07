using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Core.Services.Broker;
using PAD.LAB1.Core.Services.Client;
using System.Threading;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.ViewModels
{
    public class StartUpViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;

        public IMvxCommand StartBrokerCommand { get; set; }
        public IMvxCommand StartClientCommand { get; set; }

        public StartUpViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;

            StartBrokerCommand = new MvxCommand(StartBroker);
            StartClientCommand = new MvxCommand(StartClient);

            IsLocalhost = true;
            StartingProgress = StartingProgress.None;
            IsUIAvailable = true;
            BrokerPort = "9000";
        }

        private void StartBroker()
        {
            var brokerService = Mvx.IoCProvider.Resolve<IBrokerService>();
            StartingProgress = StartingProgress.InProgress;
            IsUIAvailable = false;
            var errorCatched = brokerService.Start(BrokerIp, BrokerPort);
            if (!errorCatched)
            {
                StartingProgress = StartingProgress.Failed;
                IsUIAvailable = true;
                return;
            }
            mvxNavigationService.Navigate<BrokerViewModel, string>(BrokerAddress);
        }

        private void StartClient()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();
            clientService.Connect(BrokerIp, BrokerPort);
            StartingProgress = StartingProgress.InProgress;
            IsUIAvailable = false;
            Task.Factory.StartNew(WaitForClientResult);
        }

        private void WaitForClientResult()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();
            Thread.Sleep(500);
            if (clientService.IsConnected)
            {
                mvxNavigationService.Navigate<ClientSetUpViewModel>();
            }
            else
            {
                StartingProgress = StartingProgress.Failed;
            }
            IsUIAvailable = true;
        }

        public void SetLocalhost()
        {
            if (IsLocalhost)
            {
                BrokerIpPart1 = "127";
                BrokerIpPart2 = "0";
                BrokerIpPart3 = "0";
                BrokerIpPart4 = "1";
            }
            else
            {
                BrokerIpPart1 = "";
                BrokerIpPart2 = "";
                BrokerIpPart3 = "";
                BrokerIpPart4 = "";
            }
        }

        #region BrokerAddressProperties
        private string brokerIpPart1;
        public string BrokerIpPart1
        {
            get { return brokerIpPart1; }
            set
            {
                SetProperty(ref brokerIpPart1, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }

        private string brokerIpPart2;
        public string BrokerIpPart2
        {
            get { return brokerIpPart2; }
            set
            {
                SetProperty(ref brokerIpPart2, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }

        private string brokerIpPart3;
        public string BrokerIpPart3
        {
            get { return brokerIpPart3; }
            set
            {
                SetProperty(ref brokerIpPart3, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }

        private string brokerIpPart4;
        public string BrokerIpPart4
        {
            get { return brokerIpPart4; }
            set
            {
                SetProperty(ref brokerIpPart4, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }

        private string brokerPort;
        public string BrokerPort
        {
            get => brokerPort;
            set
            {
                SetProperty(ref brokerPort, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }

        private string BrokerIp => $"{ BrokerIpPart1 }.{ BrokerIpPart2 }.{ BrokerIpPart3 }.{ BrokerIpPart4 }";
        private string BrokerAddress => $"{ BrokerIpPart1 }.{ BrokerIpPart2 }.{ BrokerIpPart3 }.{ BrokerIpPart4 }:{ BrokerPort }";
        #endregion

        public bool IsIpChangeable
        {
            get
            {
                if (!IsUIAvailable) return false;

                return !IsLocalhost;
            }
        }

        public bool IsStartAvailable
        {
            get
            {
                if (!IsUIAvailable) return false;

                // validate ip, and port numbers
                return !string.IsNullOrEmpty(BrokerIpPart1)
                    && !string.IsNullOrEmpty(BrokerIpPart2)
                    && !string.IsNullOrEmpty(BrokerIpPart3)
                    && !string.IsNullOrEmpty(BrokerIpPart4)
                    && !string.IsNullOrEmpty(BrokerPort);
            }
        }

        private bool isLocalhost;
        public bool IsLocalhost
        {
            get { return isLocalhost; }
            set
            {
                SetProperty(ref isLocalhost, value);
                RaisePropertyChanged(nameof(IsIpChangeable));
                SetLocalhost();
            }
        }

        private StartingProgress startingProgress;
        public StartingProgress StartingProgress
        {
            get { return startingProgress; }
            set
            {
                SetProperty(ref startingProgress, value);
                RaisePropertyChanged(nameof(StartingResult));
                RaisePropertyChanged(nameof(StartingResultColor));
            }
        }

        public string StartingResult
        {
            get
            {
                string text = "";

                if (StartingProgress == StartingProgress.InProgress)
                {
                    text = "Starting in progress!";
                }
                else if (StartingProgress == StartingProgress.Failed)
                {
                    text = "Starting failed!";
                }

                return text;
            }
        }

        public string StartingResultColor
        {
            get
            {
                if (StartingProgress == StartingProgress.InProgress)
                {
                    return "Blue";
                }
                else if (StartingProgress == StartingProgress.Failed)
                {
                    return "OrangeRed";
                }

                return "";
            }
        }

        private bool isUIAvailable;
        public bool IsUIAvailable
        {
            get => isUIAvailable;
            set
            {
                SetProperty(ref isUIAvailable, value);
                RaisePropertyChanged(nameof(IsStartAvailable));
            }
        }
    }
}
