using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankService
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=AccountBank")
        {
        }

        public DbSet<Account> accounts { get; set; }
        public DbSet<TransactionHistory> transactionHistories { get; set; }

    }
}