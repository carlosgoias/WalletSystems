using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;
using WalletSystems.Core;
using WalletSystems.Repository.Interfaces;
using WalletSystems.Services.Interfaces;

namespace WalletSystems.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public Wallet Create()
        {
            return _walletRepository.Create();
        }

        public async Task<Result> DepositAsync(Guid id, decimal amount, string transactionId)
        {
            if (amount <= 0)
                return Result.Fail(WalletExceptionTypes.InvalidAmount);

            var wallet = _walletRepository.Get(id);

            if (wallet is null)
                return Result.Fail(WalletExceptionTypes.WalletNotFound);

            if (wallet.ProcessedTransactions.Contains(transactionId))
                return Result.Fail(WalletExceptionTypes.TransactionAlreadyProcessed);

            var result = await _walletRepository.DepositAsync(id, amount, transactionId);
            if (!result)
            {
                return Result.Fail(WalletExceptionTypes.TransactionFailed);
            }
            return Result.Success();
        }

        public Wallet Get(Guid id)
        {
            return _walletRepository.Get(id);
        }

        public async Task<Result> WithdrawAsync(Guid id, decimal amount, string transactionId)
        {
            if (amount <= 0)
                return Result.Fail(WalletExceptionTypes.InvalidAmount);

            var wallet = _walletRepository.Get(id);
            if (wallet is null)
                return Result.Fail(WalletExceptionTypes.WalletNotFound);

            if (wallet.ProcessedTransactions.Contains(transactionId))
                return Result.Fail(WalletExceptionTypes.TransactionAlreadyProcessed);

            if (wallet.Balance < amount)
            {
                return Result.Fail(WalletExceptionTypes.InsufficientFunds);
            }

            var result = await _walletRepository.WithdrawAsync(id, amount, transactionId);
            if (!result)
            {
                return Result.Fail(WalletExceptionTypes.TransactionFailed);
            }
            return Result.Success();
        }
    }
}
