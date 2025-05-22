using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;

namespace WalletSystems.Repository.Interfaces
{
    public interface IWalletRepository
    {
        Wallet Create();
        Wallet Get(Guid id);
        Task<bool> DepositAsync(Guid id, decimal amount);
        Task<bool> WithdrawAsync(Guid id, decimal amount);        
    }
}
