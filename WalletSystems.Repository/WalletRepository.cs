using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using WalletsSytems.Domain;
using WalletSystems.Core;
using WalletSystems.Repository.Interfaces;

namespace WalletSystems.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ConcurrentDictionary<Guid, Wallet> _wallets;
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _walletLocks;
        private readonly ConcurrentDictionary<Guid, CircuitBreakerState> _circuitBreakers;

        public WalletRepository()
        {
            _wallets = new ConcurrentDictionary<Guid, Wallet>();
            _walletLocks = new ConcurrentDictionary<Guid, SemaphoreSlim>();
            _circuitBreakers = new ConcurrentDictionary<Guid, CircuitBreakerState>();
        }

        public Wallet Create()
        {
            var wallet = new Wallet { Id = Guid.NewGuid(), Balance = 0 };
            if(_wallets.TryAdd(wallet.Id, wallet))
            {
                _walletLocks[wallet.Id] = new SemaphoreSlim(1, 1);
                _circuitBreakers[wallet.Id] = new CircuitBreakerState();
                return wallet;
            }
            throw new Exception();
        }

        public async Task<bool> DepositAsync(Guid id, decimal amount)
        {
            if (!TryGetWalletWithState(id, out var wallet, out var semaphore, out var breaker))
                return false;

            if (breaker.IsOpen)
                return false;

            for (int attempt = 0; attempt < 3; attempt++)
            {
                if (await semaphore.WaitAsync(TimeSpan.FromMilliseconds(200)))
                {
                    try
                    {
                        wallet.Balance += amount;
                        breaker.Reset();
                        return true;
                    }
                    catch
                    {
                        breaker.RecordFailure();
                        return false;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }

                await Task.Delay(200);
            }

            breaker.RecordFailure();
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
            if (!TryGetWalletWithState(id, out var wallet, out var semaphore, out var breaker))
                return false;

            if (breaker.IsOpen)
                return false;

            for (int attempt = 0; attempt < 3; attempt++)
            {
                if (await semaphore.WaitAsync(TimeSpan.FromMilliseconds(200)))
                {
                    try
                    {
                        if (wallet.Balance >= amount)
                        {
                            wallet.Balance -= amount;
                            breaker.Reset();
                            return true;
                        }
                        return false;
                    }
                    catch
                    {
                        breaker.RecordFailure();
                        return false;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }

                await Task.Delay(200); 

            }

            breaker.RecordFailure();
            return false;
        }

        private bool TryGetWalletWithState(Guid id, out Wallet wallet, out SemaphoreSlim semaphore, out CircuitBreakerState breaker)
        {
            wallet = null;
            semaphore = null;
            breaker = null;

            return _wallets.TryGetValue(id, out wallet) &&
                   _walletLocks.TryGetValue(id, out semaphore) &&
                   _circuitBreakers.TryGetValue(id, out breaker);
        }
    }
}
