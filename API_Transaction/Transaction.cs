using API_Transaction.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Transaction
{
    public class Transaction
    {
        public string owner_card_id { get; set; }
        public string recipient_card_id { get; set; }
        public int transaction_summ { get; set; }
        public string ccy { get; set; }
        public string aditional_comment { get; set; }
        public TransactionType transactionType { get; set; }
    }
}
