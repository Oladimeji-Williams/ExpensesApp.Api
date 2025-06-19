using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesApp.API.Data.Interfaces;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Models;

namespace ExpensesApp.API.Data.Services
{
    public class TransactionService(AppDbContext appDbContext) : ITransactionService
    {
        public Transaction CreateTransaction(PostTransactionDto postTransactionDto, int userId)
        {
            var newPostTransaction = new Transaction()
            {
                Amount = postTransactionDto.Amount,
                Type = postTransactionDto.Type,
                Category = postTransactionDto.Category,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };
            appDbContext.Transactions.Add(newPostTransaction);
            appDbContext.SaveChanges();
            return newPostTransaction;
        }

        public void DeleteTransaction(int id)
        {
            var transaction = appDbContext.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                appDbContext.Transactions.Remove(transaction);
                appDbContext.SaveChanges();
            }
        }

        public Transaction? GetTransaction(int id)
        {
            var transaction = appDbContext.Transactions.FirstOrDefault(t => t.Id == id);
            return transaction;
        }

        public List<Transaction> GetTransactions(int userId)
        {
            var allTransaction = appDbContext.Transactions.Where(n => n.UserId == userId).ToList();
            return allTransaction;
        }

        public Transaction? UpdateTransaction(int id, PutTransactionDto putTransactionDto)
        {
            var transaction = appDbContext.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transaction.Type = putTransactionDto.Type;
                transaction.Amount = putTransactionDto.Amount;
                transaction.Category = putTransactionDto.Category;
                transaction.UpdatedAt = DateTime.Now;
                appDbContext.Transactions.Update(transaction);
                appDbContext.SaveChanges();
            }
            return transaction;
        }
    }
}