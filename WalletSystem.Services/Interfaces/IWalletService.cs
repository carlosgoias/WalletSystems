using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;
using WalletSystems.Core;

namespace WalletSystems.Services.Interfaces
{
    public interface IWalletService
    {
        Wallet Create();
        Wallet Get(Guid id);
        Task<Result> DepositAsync(Guid id, decimal amount, string transactionId);
        Task<Result> WithdrawAsync(Guid id, decimal amount, string transactionId);
    }
}
