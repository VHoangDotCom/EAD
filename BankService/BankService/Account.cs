using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService
{
    public class Account
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public double Balancer { get; set; }
        public string AccountNumber { get; set; }
        public int Status { get; set; }
    }
}