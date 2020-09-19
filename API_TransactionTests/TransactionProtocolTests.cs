using Microsoft.VisualStudio.TestTools.UnitTesting;
using API_Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionBroker.Storage;
using System.Configuration;

namespace API_Transaction.Tests
{
    [TestClass()]
    public class TransactionProtocolTests
    {
        [TestMethod()]
        public void SerializeTest()
        {

            //string path = ConfigurationManager.AppSettings.Get("storage_path");

            RequestStorage.GetInstance().AddTransaction(
                new TransactionProtocol()
                { Sender_id = "gaedawd", Request_id = "gsdfedead" });
            RequestStorage.GetInstance().AddTransaction(
    new TransactionProtocol()
    { Sender_id = "gaedawd", Request_id = "gsdfedead" });
            RequestStorage.GetInstance().AddTransaction(
    new TransactionProtocol()
    { Sender_id = "gaedawd", Request_id = "gsdfedead" });
            RequestStorage.GetInstance().AddTransaction(
    new TransactionProtocol()
    { Sender_id = "gaedawd", Request_id = "gsdfedead" });

            RequestStorage.LoadStorage();
            var data = RequestStorage.GetInstance();

            throw new NotImplementedException();
        }
    }
}