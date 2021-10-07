using MvvmCross.ViewModels;
using PAD.LAB1.Broker.Models;
using PAD.LAB1.Broker.Storage;
using PAD.LAB1.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.ViewModels
{
    public class BrokerViewModel : MvxViewModel<string>
    {

        private MvxObservableCollection<ConnectionModel> connections;
        public MvxObservableCollection<ConnectionModel> Connections
        {
            get { return connections; }
            set
            {
                SetProperty(ref connections, value);
            }
        }

        private MvxObservableCollection<RoomModel> rooms;
        public MvxObservableCollection<RoomModel> Rooms
        {
            get { return rooms; }
            set
            {
                SetProperty(ref rooms, value);
            }
        }

        public BrokerViewModel()
        {
            Connections = new MvxObservableCollection<ConnectionModel>();
            Rooms = new MvxObservableCollection<RoomModel>();

            Task.Factory.StartNew(EvaluateUICommands, TaskCreationOptions.LongRunning);
        }

        public void EvaluateUICommands()
        {
            while (true)
            {
                if (BrokerUIStorage.IsEmpty) continue;

                var brokerUIModel = BrokerUIStorage.GetNext();

                if (brokerUIModel == null) continue;

                switch (brokerUIModel.UICommand)
                {
                    case UICommand.NewConnection:
                        {
                            Connections.Add(new ConnectionModel
                            {
                                Id = brokerUIModel.ConnectionInfoId
                            });
                        }
                        RaisePropertyChanged(nameof(TotalConnections));
                        break;
                    case UICommand.NewRoom:
                        {
                            Rooms.Add(new RoomModel
                            {
                                Code = brokerUIModel.RoomCode,
                                MembersCount = brokerUIModel.MembersCount
                            });
                            RaisePropertyChanged(nameof(TotalRooms));
                        }
                        break;
                    case UICommand.WelcomeToRoom:
                    case UICommand.MemberLeft:
                    case UICommand.MemberLostConnection:
                        {
                            var room = Rooms.FirstOrDefault(x => x.Code == brokerUIModel.RoomCode);
                            
                            if (room != null)
                            {
                                room.MembersCount = brokerUIModel.MembersCount;
                            }

                            if (brokerUIModel.UICommand != UICommand.WelcomeToRoom)
                            {
                                var connectionToRemove = Connections.FirstOrDefault(x => x.Id == brokerUIModel.ConnectionInfoId);
                                Connections.Remove(connectionToRemove);
                                RaisePropertyChanged(nameof(TotalConnections));
                            }
                        }
                        break;
                    default:
                        throw new Exception("Unknown Command");
                }
            }
        }

        private string brokerAddress;
        public string BrokerAddress
        {
            get { return brokerAddress; }
            set
            {
                SetProperty(ref brokerAddress, value);
            }
        }

        private int payloadsInQueue;
        public int PayloadsInQueue
        {
            get { return payloadsInQueue; }
            set
            {
                SetProperty(ref payloadsInQueue, value);
            }
        }

        public int TotalConnections => Connections.Count;

        public int TotalRooms => Rooms.Count;

        public override void Prepare(string parameter)
        {
            BrokerAddress = parameter;
        }
    }
}
