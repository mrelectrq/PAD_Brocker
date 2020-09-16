using System;
using System.Collections.Generic;
using System.Text;
using TransactionBroker.Interfaces;
using TransactionBroker.Routing;

namespace TransactionBroker
{
    public class ApiManager
    {
        public IRouting GetRooting()
        {
            return new RequestHandler();
        }
    }
}
