using System;
using System.Threading.Tasks;
using LedgerLink.Core.DTOs;
using LedgerLink.Core.Exceptions;
using LedgerLink.Core.Interfaces;
using LedgerLink.Core.Models;
using AutoMapper;

namespace LedgerLink.Core.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(Guid id, int userId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                throw new NotFoundException($"Transaction with ID {id} not found");

            if (transaction.Account?.UserId != userId)
                throw new UnauthorizedException("You are not authorized to access this transaction");

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
} 