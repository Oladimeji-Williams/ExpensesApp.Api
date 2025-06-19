using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesApp.API.Models;
using ExpensesApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Data.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExpensesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {
            var transaction = transactionService.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet()]
        public IActionResult GetTransactions()
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nameIdentifierClaim))
            {
                return BadRequest("Could not get the user id");
            }
            if (!int.TryParse(nameIdentifierClaim, out var userId))
            {
                return BadRequest();
            }
            var allTransaction = transactionService.GetTransactions(userId);
            return Ok(allTransaction);
        }
        [HttpPost()]
        public IActionResult CreateTransaction([FromBody] PostTransactionDto postTransactionDto)
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nameIdentifierClaim))
            {
                return BadRequest("Could not get the user id");
            }
            if (!int.TryParse(nameIdentifierClaim, out var userId))
            {
                return BadRequest();
            }
            var newPostTransaction = transactionService.CreateTransaction(postTransactionDto, userId);
            return Ok(newPostTransaction);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] PutTransactionDto putTransactionDto)
        {
            var updatedTransaction = transactionService.UpdateTransaction(id, putTransactionDto);
            if (updatedTransaction == null)
            {
                return NotFound();
            }
            return Ok(updatedTransaction);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            transactionService.DeleteTransaction(id);
            return Ok();
        }
    }
}