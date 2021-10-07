using AutoMapper;
using GrpcAgent;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Core.Services;
using PAD.LAB1.Shared.Models;
using System.Collections.Generic;

namespace PAD.LAB1.Core.ViewModels
{
    public class ConnectionViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        public IMvxCommand ContinueCommand { get; set; }

        public ConnectionViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;
            ContinueCommand = new MvxCommand(Continue);

            IsUIAvailable = true;
        }

        private void Continue()
        {
            var setUpType = GetSetUpType();
            if (!IsContinueButtonAvailable) return;

            if (setUpType == SetUpType.CreateRoom)
            {
                CreateRoom();
            }
            else if (setUpType == SetUpType.JoinRoom)
            {
                JoinRoom();
            }
        }

        private async void CreateRoom()
        {
            var chatService = Mvx.IoCProvider.Resolve<IChatService>();

            IsUIAvailable = false;
            try
            {
                var result = await chatService.Client.CreateRoomAsync(new CreateRoomRequest { MemberName = Name });

                if (result.ReplyInfo.Status == ReplyStatus.Failed)
                {
                    FailedReason = result.ReplyInfo.Reason;
                    IsUIAvailable = true;
                    return;
                }

                var userData = new UserData
                {
                    MemberId = result.Member.Id,
                    MemberColor = result.Member.HexColor,
                    MemberName = result.Member.Name,
                    RoomCode = result.Room.Code,
                    MembersCount = result.Room.MembersCount,
                };
                await mvxNavigationService.Navigate<ChatViewModel, UserData>(userData);
            }
            catch
            {
                FailedReason = "Broker Is Offline";
            }
            IsUIAvailable = true;
        }

        private async void JoinRoom()
        {
            var chatService = Mvx.IoCProvider.Resolve<IChatService>();
            var mapper = Mvx.IoCProvider.Resolve<IMapper>();

            IsUIAvailable = false;
            try
            {
                var result = await chatService.Client.JoinRoomAsync(new JoinRoomRequest { MemberName = Name, RoomCode = roomCode });

                if (result.ReplyInfo.Status == ReplyStatus.Failed)
                {
                    FailedReason = result.ReplyInfo.Reason;
                    IsUIAvailable = true;
                    return;
                }

                var userData = new UserData
                {
                    MemberId = result.Member.Id,
                    MemberColor = result.Member.HexColor,
                    MemberName = result.Member.Name,
                    RoomCode = result.Room.Code,
                    MembersCount = result.Room.MembersCount,
                };

                foreach (var messageToConvert in result.MessagesInfo)
                {
                    userData.MessageInfosHistory.Add(new MessageInfoModel(
                        mapper.Map<MemberModel>(messageToConvert.Member),
                        mapper.Map<MessageModel>(messageToConvert.Message)
                        ));
                }
                await mvxNavigationService.Navigate<ChatViewModel, UserData>(userData);
            }
            catch
            {
                FailedReason = "Broker Is Offline";
            }
            IsUIAvailable = true;
        }


        private string failedReason;
        public string FailedReason
        {
            get => failedReason;
            set
            {
                SetProperty(ref failedReason, value);
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

        private bool isUIAvailable;
        public bool IsUIAvailable
        {
            get => isUIAvailable;
            set
            {
                SetProperty(ref isUIAvailable, value);
                RaisePropertyChanged(nameof(IsContinueButtonAvailable));
            }
        }

        public bool IsContinueButtonAvailable
        {
            get
            {
                bool available = IsUIAvailable;
                
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

                if (connectType == SetUpType.JoinRoom)
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

        public SetUpType GetSetUpType()
        {

            return !string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(RoomCode)
                ? SetUpType.CreateRoom
                : !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(RoomCode) ? SetUpType.JoinRoom : SetUpType.None;
        }

    }
}
