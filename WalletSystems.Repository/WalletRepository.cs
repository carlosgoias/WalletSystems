using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using WalletsSytems.Domain;
using WalletSystems.Repository.Interfaces;

namespace WalletSystems.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ConcurrentDictionary<Guid, Wallet> _wallets;
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _walletLocks;

        public WalletRepository()
        {
            _wallets = new ConcurrentDictionary<Guid, Wallet>();
            _walletLocks = new ConcurrentDictionary<Guid, SemaphoreSlim>();
        }

        public Wallet Create()
        {
            var wallet = new Wallet { Id = Guid.NewGuid(), Balance = 0 };
            if(_wallets.TryAdd(wallet.Id, wallet))
            {
                _walletLocks[wallet.Id] = new SemaphoreSlim(1, 1);
                return wallet;
            }
            throw new Exception();
        }

        public async Task<bool> DepositAsync(Guid id, decimal amount)
        {
            if (_wallets.TryGetValue(id, out var wallet) &&
                _walletLocks.TryGetValue(id, out var semaphore))
            {
                await semaphore.WaitAsync();
                try
                {
                    wallet.Balance += amount;
                    return true;
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return false;
        }

        public Wallet Get(Guid id)
        {
            if(_wallets.TryGetValue(id, out var wallet))
                return wallet;

            return null;
        }

        public async Task<bool> WithdrawAsync(Guid id, decimal amount)
        {
            if (_wallets.TryGetValue(id, out var wallet) &&
                _walletLocks.TryGetValue(id, out var semaphore))
            {
                await semaphore.WaitAsync();
                try
                {
                    if (wallet.Balance >= amount)
                    {
                        wallet.Balance -= amount;
                        return true;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return false;
        }
    }
}
