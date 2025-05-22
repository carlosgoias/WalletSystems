using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;

namespace WalletSystems.Services.Interfaces
{
    public interface IWalletService
    {
        Wallet Create();
        Wallet Get(Guid id);
        Task<bool> DepositAsync(Guid id, decimal amount);
        Task<bool> WithdrawAsync(Guid id, decimal amount);
    }
}
