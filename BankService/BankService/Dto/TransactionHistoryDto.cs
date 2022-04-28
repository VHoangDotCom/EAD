using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankService.Dto
{
    [DataContract]
    public class TransactionHistoryDto
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string SenderAccountNumber { get; set; }
        [DataMember]
        public string ReceiverAccountNumber { get; set; }
        [DataMember]
        public int Type { get; set; }

    }
}