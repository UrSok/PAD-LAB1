namespace PAD.LAB1.Broker.Storages
{
// contine toate storage-urile pe care le avem - baza de date
    public interface IRootStorage
    {
        IRoomStorage Rooms { get; }
    }

    public class RootStorage : IRootStorage
    {
        public IRoomStorage Rooms { get; }

        public RootStorage()
        {
            Rooms = new RoomStorage();
        }
    }
}
