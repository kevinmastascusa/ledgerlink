using LedgerLink.Core.DTOs;
using LedgerLink.Core.Extensions;
using LedgerLink.Core.Models;
using LedgerLink.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LedgerLink.API.Controllers
{
    /// <summary>
    /// Controller for managing transaction-related operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Gets a transaction by its ID
        /// </summary>
        /// <param name="id">The ID of the transaction to retrieve</param>
        /// <returns>The transaction if found, NotFound if not found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                if (transaction == null)
                    return NotFound();

                return Ok(transaction.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all transactions for a specific account
        /// </summary>
        /// <param name="accountId">The ID of the account</param>
        /// <returns>A list of transactions for the account</returns>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByAccountId(int accountId)
        {
            try
            {
                var transactions = await _transactionService.GetByAccountIdAsync(accountId);
                return Ok(transactions.Select(t => t.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all transactions for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of transactions for the user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByUserId(int userId)
        {
            try
            {
                var transactions = await _transactionService.GetByUserIdAsync(userId);
                return Ok(transactions.Select(t => t.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("account/{accountId}/date-range")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByDateRange(
            int accountId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var transactions = await _transactionService.GetByDateRangeAsync(accountId, startDate, endDate);
                return Ok(transactions.Select(t => t.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <param name="createTransactionDto">The transaction data to create</param>
        /// <returns>The created transaction</returns>
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto createTransactionDto)
        {
            try
            {
                var transaction = await _transactionService.CreateAsync(createTransactionDto);
                return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction.ToDto());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing transaction
        /// </summary>
        /// <param name="id">The ID of the transaction to update</param>
        /// <param name="updateTransactionDto">The updated transaction data</param>
        /// <returns>The updated transaction if successful, NotFound if not found</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDto>> Update(int id, UpdateTransactionDto updateTransactionDto)
        {
            try
            {
                var transaction = await _transactionService.UpdateAsync(id, updateTransactionDto);
                if (transaction == null)
                    return NotFound();

                return Ok(transaction.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        /// <param name="id">The ID of the transaction to delete</param>
        /// <returns>NoContent if successful, NotFound if not found</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _transactionService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the total amount of transactions for a specific account
        /// </summary>
        /// <param name="accountId">The ID of the account</param>
        /// <returns>The total amount of transactions</returns>
        [HttpGet("account/{accountId}/total")]
        public async Task<ActionResult<decimal>> GetTotalByAccountId(int accountId)
        {
            try
            {
                var total = await _transactionService.GetTotalByAccountIdAsync(accountId);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 