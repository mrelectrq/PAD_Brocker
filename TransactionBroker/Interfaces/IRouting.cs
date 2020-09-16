using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionBroker.Interfaces
{
    public interface IRouting
    {
        void RouteRequests(ConnectionParam connection);
    }
}
