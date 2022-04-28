using BankService.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private MyDBContext db = new MyDBContext();

        public TransactionHistoryDto Deposit(string userName, string password, double amount)
        {
        
            var account = db.accounts.Where(s => s.FullName.Equals(userName) && s.Password.Equals(password)).FirstOrDefault();
            if (account != null)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        account.Balancer += amount;
                        var transactionHistory = new TransactionHistory()
                        {
                           Id = Guid.NewGuid().ToString(),
                            Amount = amount,
                            SenderAccountNumber = account.AccountNumber,
                            ReceiverAccountNumber = account.AccountNumber,
                            
                            Type = 2,
                        };
                        db.Entry(account).State = EntityState.Modified;
                        db.transactionHistories.Add(transactionHistory);
                        db.SaveChanges();
                        transaction.Commit();
                        return new TransactionHistoryDto()
                        {
                            Id = transactionHistory.Id,
                            Amount = transactionHistory.Amount,
                            SenderAccountNumber = transactionHistory.SenderAccountNumber,
                            ReceiverAccountNumber = transactionHistory.ReceiverAccountNumber,
                        
                            Type = transactionHistory.Type,
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return null;
        }

     
        public AccountDto Login(string userName, string password)
        {
          
            var account = db.accounts.Where(s => s.FullName.Equals(userName) && s.Password.Equals(password)).FirstOrDefault();
            if (account != null)
            {
                var accountDto = new AccountDto();
                accountDto.AccountNumber = account.AccountNumber;
                accountDto.FullName = account.FullName;
                accountDto.Password = account.Password;
                accountDto.Balancer = account.Balancer;
                accountDto.Status = account.Status;
                return accountDto;
            }
            else
            {
                return null;
            }
        }

        public Account SaveAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public TransactionHistoryDto Transfer(string userName, string password, double amount, string receiverAccountNumber)
        {
      
            var account = db.accounts.Where(s => s.FullName.Equals(userName) && s.Password.Equals(password)).FirstOrDefault();
            var accountNumber = db.accounts.Where(s => s.AccountNumber.Equals(receiverAccountNumber)).FirstOrDefault();
            if (account != null)
            {
                if (accountNumber != null)
                {
                    if (account.Balancer < amount)
                    {
                        return null;
                    }
                    else
                    {
                        using (DbContextTransaction transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                account.Balancer -= amount;
                                accountNumber.Balancer += amount;
                                var transactionHistory = new TransactionHistory()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Amount = amount,
                                    SenderAccountNumber = account.AccountNumber,
                                    ReceiverAccountNumber = accountNumber.AccountNumber,
                                    Type = 3,
                                };
                                db.Entry(account).State = EntityState.Modified;
                                db.Entry(accountNumber).State = EntityState.Modified;
                                db.transactionHistories.Add(transactionHistory);
                                db.SaveChanges();
                                transaction.Commit();
                                return new TransactionHistoryDto()
                                {
                                    Id = transactionHistory.Id,
                                    Amount = transactionHistory.Amount,
                                    SenderAccountNumber = transactionHistory.SenderAccountNumber,
                                    ReceiverAccountNumber = transactionHistory.ReceiverAccountNumber,
                                    Type = transactionHistory.Type,
                                };
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
            return null;
        }

        public TransactionHistoryDto WithDraw(string userName, string password, double amount)
        {
           
            var account = db.accounts.Where(s => s.FullName.Equals(userName) && s.Password.Equals(password)).FirstOrDefault();
            if (account != null)
            {
                Debug.WriteLine(account.FullName);
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        account.Balancer -= amount;
                        var transactionHistory = new TransactionHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Amount = amount,
                            SenderAccountNumber = account.AccountNumber,
                            ReceiverAccountNumber = account.AccountNumber,
                            
                           Type = 1,
                        };
                        db.Entry(account).State = EntityState.Modified;
                        db.transactionHistories.Add(transactionHistory);
                        db.SaveChanges();
                        transaction.Commit();
                        return new TransactionHistoryDto()
                        {
                            Id = transactionHistory.Id,
                            Amount = transactionHistory.Amount,
                            SenderAccountNumber = transactionHistory.SenderAccountNumber,
                            ReceiverAccountNumber = transactionHistory.ReceiverAccountNumber,
                            Type = transactionHistory.Type,
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return null;
        }
    }
}
