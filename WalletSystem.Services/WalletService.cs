using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;
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

        public Task<bool> DepositAsync(Guid id, decimal amount)
        {
            return _walletRepository.DepositAsync(id, amount);
        }

        public Wallet Get(Guid id)
        {
            return _walletRepository.Get(id);
        }

        public Task<bool> WithdrawAsync(Guid id, decimal amount)
        {
            return _walletRepository.WithdrawAsync(id, amount);
        }
    }
}
