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
    /// Controller for managing account-related operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Gets an account by its ID
        /// </summary>
        /// <param name="id">The ID of the account to retrieve</param>
        /// <returns>The account if found, NotFound if not found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                if (account == null)
                    return NotFound();

                return Ok(account.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all accounts for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of accounts belonging to the user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetByUserId(int userId)
        {
            try
            {
                var accounts = await _accountService.GetByUserIdAsync(userId);
                return Ok(accounts.Select(a => a.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="createAccountDto">The account data to create</param>
        /// <returns>The created account</returns>
        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(CreateAccountDto createAccountDto)
        {
            try
            {
                var account = await _accountService.CreateAsync(createAccountDto);
                return CreatedAtAction(nameof(GetById), new { id = account.Id }, account.ToDto());
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
        /// Updates an existing account
        /// </summary>
        /// <param name="id">The ID of the account to update</param>
        /// <param name="updateAccountDto">The updated account data</param>
        /// <returns>The updated account if successful, NotFound if not found</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> Update(int id, UpdateAccountDto updateAccountDto)
        {
            try
            {
                var account = await _accountService.UpdateAsync(id, updateAccountDto);
                if (account == null)
                    return NotFound();

                return Ok(account.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an account
        /// </summary>
        /// <param name="id">The ID of the account to delete</param>
        /// <returns>NoContent if successful, NotFound if not found</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _accountService.DeleteAsync(id);
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
        /// Gets the current balance of an account
        /// </summary>
        /// <param name="id">The ID of the account</param>
        /// <returns>The current balance of the account</returns>
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalance(int id)
        {
            try
            {
                var balance = await _accountService.GetBalanceAsync(id);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 