using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TransactionBroker.Interfaces;
using TransactionBroker.Storage;

namespace TransactionBroker
{
    public class BrokerServer
    {
        private Socket _socket;
        private BrokerServer()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        public void Start(string ip, int port)
        {
            try
            {
                var ip_point = new IPEndPoint(IPAddress.Parse(ip), port);
                _socket.Bind(ip_point);
                _socket.Listen(10);
                AcceptConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while starting BrokerServer" + e.Message);
            }
        }

        private void AcceptConnection()
        {
            _socket.BeginAccept(AcceptConnectionCallback, null);
        }

        private void AcceptConnectionCallback(IAsyncResult result)
        {
            try
            {
                ConnectionParam connection = new ConnectionParam();
                connection.Socket = _socket.EndAccept(result);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Context, 0, connection.Address.Length
                    , 0, ReceiveDataCallback, connection);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while accepting data" + e.Message);
            }
            finally
            {
                AcceptConnection();
            }

        }

        private void ReceiveDataCallback(IAsyncResult asyncResult)
        {
            ConnectionParam connectionParam = (ConnectionParam)asyncResult.AsyncState;
            try
            {
                SocketError error;
                int msgSize = connectionParam.Socket.EndReceive(asyncResult, out error);

                if (error == SocketError.Success)
                {
                    byte[] incomingData = new byte[msgSize];
                    Array.Copy(connectionParam.Context, incomingData, msgSize);


                    ApiManager manager = new ApiManager();

                    manager.GetRooting().RouteRequests(connectionParam);

                    //add API logic
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while receiving message : " + e.Message);
            }
            finally
            {
                var status = IsConnected(connectionParam.Socket);
                if (status)
                {
                    connectionParam.Socket.BeginReceive(connectionParam.Context, 0, connectionParam.Address.Length
                         , 0, ReceiveDataCallback, connectionParam);
                }
                else
                {
                    Console.WriteLine("Deconectarea socketului:" + connectionParam.Address);
                    connectionParam.Socket.Close();

                    //Added ConnectionStorage
                    ConnectionStorage.GetInstance().ExcludeConnection(connectionParam);
                }
            }
        }
        private bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
