using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcAgent;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Core.Services;
using PAD.LAB1.Core.Utils;
using PAD.LAB1.Shared.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel<UserData>
    {
        public ThreadSafeObservableCollection<MessageInfoModel> Messages {  get; set; }

        private readonly IMvxNavigationService mvxNavigationService;

        public IMvxCommand SendMessageCommand { get; set; }

        public ChatViewModel(IMvxNavigationService mvxNavigationService)
        {
            Messages = new ThreadSafeObservableCollection<MessageInfoModel>();
            this.mvxNavigationService = mvxNavigationService;

            SendMessageCommand = new MvxCommand(SendMessage);
            IsUIAvailable = true;
        }

        private UserData userData;
        public UserData UserData
        {
            get { return userData; }
            set
            {
                SetProperty(ref userData, value);
            }
        }

        private async void SendMessage()
        {
            var chatService = Mvx.IoCProvider.Resolve<IChatService>();
            if (!IsSendAvailable) return;

            var messageRequest = new MessageRequest
            {
                MemberId = UserData.MemberId,
                RoomCode = UserData.RoomCode,
                MessageText = MessageText
            };

            MessageState = "Sending";
            IsUIAvailable = false;
            try
            {
                var result = await chatService.Client.SendMessageAsync(messageRequest);

                if (result.ReplyInfo.Status == ReplyStatus.Failed)
                {
                    MessageState = "Failed";
                    IsUIAvailable = true;
                    return;
                }
            }
            catch
            {
                IsUIAvailable = false;
                MessageState = "Broker Offline";
                return;
            }
            IsUIAvailable = true;

            MessageState = "Sent";
            MessageText = string.Empty;
            await Task.Delay(500);
            messageState = string.Empty;
            
        }

        private string messageState;
        public string MessageState
        {
            get => messageState;
            set
            {
                SetProperty(ref messageState, value);
            }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                SetProperty(ref messageText, value);
                RaisePropertyChanged(nameof(IsSendAvailable));
            }
        }

        private bool isUIAvailable;
        public bool IsUIAvailable
        {
            get => isUIAvailable;
            set
            {
                SetProperty(ref isUIAvailable, value);
                RaisePropertyChanged(nameof(IsSendAvailable));
            }
        }

        public bool IsSendAvailable
        {
            get
            {
                return IsUIAvailable && !string.IsNullOrEmpty(MessageText);
            }
        }

        public override void Prepare(UserData parameter)
        {
            var mapper = Mvx.IoCProvider.Resolve<IMapper>();
            UserData = parameter;
            if (UserData.MessageInfosHistory.Any())
            {
                Messages.AddRange(UserData.MessageInfosHistory);
            }

            Task.Factory.StartNew(RoomStream, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(ChatStream, TaskCreationOptions.LongRunning);
        }

        public async Task RoomStream()
        {
            var chatService = Mvx.IoCProvider.Resolve<IChatService>();

            using (var call = chatService.Client.RoomStream(new StreamRequest { RoomCode = UserData.RoomCode, MemberId = UserData.MemberId }))
            {
                while (true)
                {
                    while (await call.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                    {
                        var result = call.ResponseStream.Current;

                        if (result.ReplyInfo.Status == ReplyStatus.Failed)
                        {
                            return;
                        }

                        UserData.MembersCount = result.MembersCount;
                    }
                }
            }
        }
        public async Task ChatStream()
        {
            var chatService = Mvx.IoCProvider.Resolve<IChatService>();
            var mapper = Mvx.IoCProvider.Resolve<IMapper>();

            using (var call = chatService.Client.ChatStream(new StreamRequest { RoomCode = UserData.RoomCode, MemberId = UserData.MemberId }))
            {
                while (true)
                {
                    while (await call.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                    {
                        var result = call.ResponseStream.Current;

                        if (result.ReplyInfo.Status == ReplyStatus.Failed)
                        {
                            return;
                        }

                        Messages.Add(new MessageInfoModel(
                            mapper.Map<MemberModel>(result.MessageInfo.Member),
                            mapper.Map<MessageModel>(result.MessageInfo.Message)
                        ));
                    }
                }
            }
        }
    }
}
