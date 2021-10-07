using PAD.LAB1.Broker.Storage;
using PAD.LAB1.Broker.Utils;
using PAD.LAB1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker
{
//BrokerNetworkManager reprezinta file-ul principal al broker-ului
    public class BrokerNetworkManager
    {
        private const int ConnectionsLimit = 10;

        private Socket socket;

        public BrokerNetworkManager()
        {

        }

        // Functia pentru pornirea Broker-ului
        public void Start(string brokerIpAddress, int brokerPort)
        {
            var address = new IPEndPoint(IPAddress.Parse(brokerIpAddress), brokerPort); // initializam adresa
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // initiazam socket-ul pe protocolul TCP

            socket.Bind(address); // asignam adresa care a fost creata la socketul creat
            socket.Listen(ConnectionsLimit); //numarul maxim de conexiuni
            AcceptConnections(); // functie aparte unde incepe sa primeasca conexiuni

            var payloadRouter = new PayloadRouter();
            // initializam un fir nou de executie, care va rula in background
            Task.Factory.StartNew(payloadRouter.Route, TaskCreationOptions.LongRunning);
        }

        private void AcceptConnections() //incepe sa primeasca conexiuni
        {
            socket.BeginAccept(OnAccept, null); 
        }

        private void StartReceive(ConnectionInfo connectionInfo)
        {
            connectionInfo.Socket.BeginReceive(connectionInfo.Buffer, 0, ConnectionInfo.BufferSize,
                SocketFlags.None, OnReceive, connectionInfo); // ultimul parametru este object state
        }

        private void OnAccept(IAsyncResult asyncResult)
        {
            // ConnectionInfo - cu el salvam conexiunile care le primim, le atribuim un id random
            // salvam socketul clientului, si folosim un buffer prin care o sa primim datele
            var connectionInfo = new ConnectionInfo(); // cream un obiect pentru a salva clientul
            try
            {
                connectionInfo.Socket = socket.EndAccept(asyncResult); // salvam socket-ul client-ului in obiectul de mai sus
                ConnectionStorage.Add(connectionInfo); 
                StartReceive(connectionInfo);
                BrokerUIStorage.EnqueNewConnection(connectionInfo.Id); 
            }
            catch (Exception ex)
            {
                BrokerUIStorage.EnqueError(ex);
            }
            finally
            {
                AcceptConnections();
            }
        }

        private void OnReceive(IAsyncResult asyncResult)
        {
            var connectionInfo = asyncResult.AsyncState as ConnectionInfo; // extragem parametrul object state 
            try
            {
                var receivedBytesLength = connectionInfo.Socket.EndReceive(asyncResult, out SocketError response);

                if (response == SocketError.Success)
                {
                    var payloadBytes = new byte[receivedBytesLength];
                    // facem o copie la bitii primiti intr-o variabila local si trimitem catre PayloadHandler
                    Array.Copy(connectionInfo.Buffer, payloadBytes, receivedBytesLength);
                    PayloadHandler.Handle(payloadBytes, connectionInfo.Id);
                    // curatim buferul, care a fost primit de la Client
                    Array.Clear(connectionInfo.Buffer, 0, ConnectionInfo.BufferSize);
                    // este facut asa, pentru a evita cazurile, cand mesajul nou primit e mai scurt decat cel precedent, ce ar putea cauza date eronate
                }
            }
            catch (Exception ex)
            {
                BrokerUIStorage.EnqueError(ex);
            }
            finally
            {
                try
                {
                    StartReceive(connectionInfo);
                }
                catch
                {
                    MemberLostConnection(connectionInfo); // o folosim cand se pierde conexinunea, ptu a sterge membrii din lista
                }
            }
        }

        public static void MemberLostConnection(ConnectionInfo connectionInfo) 
        {
            var connectionInfoId = connectionInfo.Id; // salvam id-ul la conexiune trimis mai sus
            connectionInfo.Socket.Close(); // inchidem socket-ul care-l avem in conexiune
            ConnectionStorage.Remove(connectionInfo.Id); // stergem din lista conexiunea

            PayloadHandler.HandleMemberLostConnection(connectionInfoId);
        }
    }
}
