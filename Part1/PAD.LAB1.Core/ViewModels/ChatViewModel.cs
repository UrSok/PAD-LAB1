using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PAD.LAB1.Client.Storage;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Core.Services.Client;
using PAD.LAB1.Core.Utils;
using PAD.LAB1.Shared.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel<LocalUserData>
    {

        private MvxObservableCollection<MessageModel> messages;
        public MvxObservableCollection<MessageModel> Messages
        {
            get { return messages; }
            set
            {
                SetProperty(ref messages, value);
            }
        }

        private readonly IMvxNavigationService mvxNavigationService;

        public ChatViewModel(IMvxNavigationService mvxNavigationService)
        {
            Messages = new MvxObservableCollection<MessageModel>();
            this.mvxNavigationService = mvxNavigationService;

            SendMessageCommand = new MvxCommand(SendMessage);
        }

        public IMvxCommand SendMessageCommand { get; set; }
        private void SendMessage()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();

            var message = new MessageModel
            {
                MemberColor = localUserData.MemberColor,
                MemberName = localUserData.MemberName,
                MessageDate = DateTime.Now,
                MessageText = MessageText,
            };

            clientService.SendMessage(message, localUserData.RoomCode);

            MessageText = string.Empty;
        }


        private LocalUserData localUserData;
        public LocalUserData LocalUserData
        {
            get { return localUserData; }
            set
            {
                SetProperty(ref localUserData, value);
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

        public bool IsSendAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(MessageText);
            }
        }

        public void EvaluateChatUIStorage()
        {
            var clientService = Mvx.IoCProvider.Resolve<IClientService>();
            while (true)
            {
                if (!clientService.IsConnected)
                {
                    mvxNavigationService.Navigate<ConnectionLostViewModel>();
                    break;
                }

                while (!ClientUIStorage.IsEmpty)
                {
                    var uiModel = ClientUIStorage.GetNext();


                    if (uiModel != null)
                    {
                        switch (uiModel.Command)
                        {
                            case ClientUICommand.NewMember:
                                {
                                    localUserData.MembersCount = uiModel.Room.MembersCount;
                                    Messages.Add(new MessageModel
                                    {
                                        MessageDate = DateTime.Now,
                                        MemberName = Constants.SystemName,
                                        MessageText = $"{ uiModel.Member.Name } joined the room!",
                                    });
                                }
                                break;
                            case ClientUICommand.MemberLeft:
                                {
                                    localUserData.MembersCount = uiModel.Room.MembersCount;
                                    Messages.Add(new MessageModel
                                    {
                                        MessageDate = DateTime.Now,
                                        MemberName = Constants.SystemName,
                                        MessageText = $"{ uiModel.Member.Name } left the room!",
                                    });
                                }
                                break;
                            case ClientUICommand.NewMessage:
                                {
                                    Messages.Add(new MessageModel
                                    {
                                        MemberColor = uiModel.Member.Color,
                                        MemberName = uiModel.Member.Name,
                                        MessageDate = uiModel.Message.DateTime,
                                        MessageText = uiModel.Message.Text,
                                    });
                                }
                                break;
                            default:
                                throw new Exception("No such command!");
                        }
                    }
                }
            }
        }

        public override void Prepare(LocalUserData parameter)
        {
            LocalUserData = parameter;

            Messages.Add(new MessageModel
            {
                MessageDate = DateTime.Now,
                MemberName = Constants.SystemName,
                MessageText = $"Welcome! Use this code to invite others: { LocalUserData.RoomCode }.",
            });

            Task.Factory.StartNew(EvaluateChatUIStorage, TaskCreationOptions.LongRunning);
        }
    }
}
