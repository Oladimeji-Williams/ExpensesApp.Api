using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Models;

namespace ExpensesApp.API.Data.Interfaces
{
    public interface ITransactionService
    {
        List<Transaction> GetTransactions(int userId);
        Transaction? GetTransaction(int id);
        Transaction CreateTransaction(PostTransactionDto postTransactionDto, int userId);
        Transaction? UpdateTransaction(int id, PutTransactionDto putTransactionDto);
        void DeleteTransaction(int id);

    }
}