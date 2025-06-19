using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}