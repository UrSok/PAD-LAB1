using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Client.Storage;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Core.Services.Client;
using PAD.LAB1.Shared.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PAD.LAB1.Core.ViewModels
{
    public class ClientSetUpViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        public IMvxCommand ContinueCommand { get; set; }

        public ClientSetUpViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;
            ContinueCommand = new MvxCommand(Continue);
            IsNoSuchRoomVisible = Visibility.Collapsed;
        }

        private void Continue()
        {
            var setUpType = GetSetUpType();

            if (setUpType == SetUpType.CreateRoom)
            {
                CreateRoom();
            }
            else if (setUpType == SetUpType.EnterRoom)
            {
                EnterRoom();
            }
        }

        public void CreateRoom()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();

            if (!clientService.IsConnected)
            {
                mvxNavigationService.Navigate<StartUpViewModel>();
                return;
            }

            clientService.CreateRoom(Name);
            Task.Factory.StartNew(WaitForResponse);
        }

        public void EnterRoom()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();

            if (!clientService.IsConnected)
            {
                mvxNavigationService.Navigate<StartUpViewModel>();
                return;
            }

            clientService.EnterRoom(Name, RoomCode);
            Task.Factory.StartNew(WaitForResponse);
        }

        public void WaitForResponse()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();

            float elapsedSeconds = 0;
            do
            {
                Thread.Sleep(500);
                elapsedSeconds += 0.5f;
            } while (clientService.IsConnected && ClientUIStorage.IsEmpty && elapsedSeconds < 3);

            if (!clientService.IsConnected)
            {
                mvxNavigationService.Navigate<ConnectionLostViewModel>();
                return;
            }

            var clientUIModel = ClientUIStorage.GetNext();

            if (clientUIModel == null)
            {
                IsNoSuchRoomVisible = Visibility.Visible;
            }
            else if (clientUIModel.Command == ClientUICommand.NoSuchRoom)
            {
                IsNoSuchRoomVisible = Visibility.Visible;
            }
            else if (clientUIModel.Command == ClientUICommand.WelcomeToRoom)
            {
                var localUserData = new LocalUserData
                {
                    MemberColor = clientUIModel.Member.Color,
                    MemberName = clientUIModel.Member.Name,
                    RoomCode = clientUIModel.Room.Code,
                    MembersCount = clientUIModel.Room.MembersCount
                };

                mvxNavigationService.Navigate<ChatViewModel, LocalUserData>(localUserData);
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                RaisePropertyChanged(() => IsContinueButtonAvailable);
                RaisePropertyChanged(() => ContinueButtonText);
                RaisePropertyChanged(() => ValidationText);
            }
        }

        private string roomCode;
        public string RoomCode
        {
            get => roomCode;
            set
            {
                SetProperty(ref roomCode, value);
                RaisePropertyChanged(() => IsContinueButtonAvailable);
                RaisePropertyChanged(() => ContinueButtonText);
                RaisePropertyChanged(() => ValidationText);
            }
        }

        public bool IsContinueButtonAvailable
        {
            get
            {
                bool available = true;

                if (GetSetUpType() == SetUpType.None)
                {
                    available = false;
                }

                return available;
            }
        }

        public string ContinueButtonText
        {
            get
            {
                string text = "Continue";

                var connectType = GetSetUpType();

                if (connectType == SetUpType.CreateRoom)
                {
                    text = "Create Room";
                }

                if (connectType == SetUpType.EnterRoom)
                {
                    text = "Join Room";
                }

                return text;
            }
        }

        public string ValidationText
        {
            get
            {
                string text = "";

                if (GetSetUpType() == SetUpType.None)
                {
                    text = "Either fill Name field or both Name and Room Code fields!";
                }

                return text;
            }
        }


        private Visibility isNoSuchRoomVisible;
        public Visibility IsNoSuchRoomVisible
        {
            get => isNoSuchRoomVisible;
            set
            {
                SetProperty(ref isNoSuchRoomVisible, value);
            }
        }

        public SetUpType GetSetUpType()
        {

            return !string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(RoomCode)
                ? SetUpType.CreateRoom
                : !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(RoomCode) ? SetUpType.EnterRoom : SetUpType.None;
        }
    }
}
