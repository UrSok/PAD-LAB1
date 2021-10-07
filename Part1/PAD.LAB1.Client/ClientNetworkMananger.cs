using PAD.LAB1.Client.Utils;
using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models.Payload;
using PAD.LAB1.Shared.Utils;
using System;
using System.Net;
using System.Net.Sockets;

namespace PAD.LAB1.Client
{
    public class ClientNetworkMananger
    {
        protected Socket socket; // client socket
        protected readonly ConnectionInfo connectionInfo; // conexiunea de la broker
        public bool IsConnected => socket.Connected;

        public ClientNetworkMananger()
        {
            connectionInfo = new ConnectionInfo();
        }

        public void Connect(string brokerIpAddress, int brokerPort) // facem conexiunea catre broker
        {
            var address = new IPEndPoint(IPAddress.Parse(brokerIpAddress), brokerPort); // cream o adresa

            if (socket != null) 
            {
                socket.Close();
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(address, OnConnect, null);
        }

        protected void OnConnect(IAsyncResult asyncResult)
        {
            try
            {
                connectionInfo.Socket = socket; // salvam conexiunea broker-ului in connectioninfo
                socket.EndConnect(asyncResult);
                StartReceive();
            }
            catch
            {
                socket.Close();
            }
        }

        protected void OnReceive(IAsyncResult asyncResult) //
        {
            try
            {
                var receivedBytesLength = socket.EndReceive(asyncResult, out SocketError response);

                if (response == SocketError.Success)
                {
                    var payloadBytes = new byte[receivedBytesLength];
                    // facem o copie la bitii primiti intro variabila local si trimitem catre PayloadHandler
                    Array.Copy(connectionInfo.Buffer, payloadBytes, receivedBytesLength);
                    PayloadHandler.Handle(payloadBytes);
                    // curatim buferul, care a fost primit de la Broker
                    Array.Clear(connectionInfo.Buffer, 0, ConnectionInfo.BufferSize);
                    // este facut asa, pentru a evita cazurile, cand mesajul nou primit e mai scurt decat cel precedent, ce ar putea cauza date eronate
                    StartReceive();
                }
            }
            catch
            {
                socket.Close();
            }
        }

        protected void StartReceive() // ptu a primi mesaje de la broker
        {
            socket.BeginReceive(connectionInfo.Buffer, 0, ConnectionInfo.BufferSize,
                SocketFlags.None, OnReceive, null);
        }

        public void Send(Payload payload)// trimitem un payload si se face convertirea in biti si se transmite la broker
        {
            try
            {
                var payloadBytes = payload.GetBytes();
                socket.Send(payloadBytes);
            }
            catch
            {
                socket.Close();
            }
        }
    }
}
