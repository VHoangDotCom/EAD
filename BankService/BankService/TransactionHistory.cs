using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService
{
    public class TransactionHistory
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string SenderAccountNumber { get; set; } // (string FK from Account): ai gửi đến
        public string ReceiverAccountNumber { get; set; } // (string FK from Account): ai nhận tiền
        public int Type { get; set; } // withdraw (1), deposit (2), transfer (3)
    }
}