using API_Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace TransactionBroker.Storage
{
    public class RequestStorage
    {
        private readonly object _lock = new object();
        private static DateTime TimeStamp = DateTime.Now.AddMinutes(1);
        private static readonly Lazy<RequestStorage> Storage = new Lazy<RequestStorage>
            (() => new RequestStorage());

        private static ConcurrentQueue<TransactionProtocol> Transactions { get; set; }

        private RequestStorage()
        {

            if (Transactions is null)
            {
                GetStorage();
            }
        }

        public static RequestStorage GetInstance()
        {
            return Storage.Value;
        }

        public void AddTransaction(TransactionProtocol transaction)
        {
            Transactions.Enqueue(transaction);
            CheckTimeArhivation();
        }

        public TransactionProtocol GetTransaction()
        {
            TransactionProtocol transact = null;
            var status = false;

            while (status)
            {
                status = Transactions.TryDequeue(out transact);
            }
            return transact;
        }

        private static void GetStorage()
        {
            string path = Config.STORAGE_PATH;
            string file_name = Config.STORAGE_NAME;
            string serializationFile = Path.Combine(path, file_name);
            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new BinaryFormatter();
                try
                {

                    var uploadedProtocols = (TransactionProtocol[])bformatter.Deserialize(stream);
                    Transactions = new ConcurrentQueue<TransactionProtocol>();
                    foreach (var i in uploadedProtocols)
                    {
                        Transactions.Enqueue(i);
                    }

                }
                catch (Exception e)
                {
                    Transactions = new ConcurrentQueue<TransactionProtocol>();
                    Console.WriteLine("Initialization of TransactionStorage - Storage is Empty" + e.Message);
                }


            }
        }

        public static void LoadStorage()
        {
            string path = Config.STORAGE_PATH;
            string file_name = Config.STORAGE_NAME;
            string serializationFile = Path.Combine(path, file_name);
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {

                TransactionProtocol[] toLoad = Transactions.ToArray();

                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, toLoad);
            }
        }

        private void CheckTimeArhivation()
        {
            lock (_lock)
            {
                if (TimeStamp < DateTime.Now)
                {
                    LoadStorage();
                    TimeStamp = DateTime.Now.AddMinutes(1);
                }
            }

        }
    }
}
