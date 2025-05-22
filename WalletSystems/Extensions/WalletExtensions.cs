using WalletsSytems.Domain;
using WalletSystems.Dtos;

namespace WalletSystems.Extensions
{
    public static class WalletExtensions
    {
        public static WalletDto ToDto(this Wallet wallet)
        {
            return new WalletDto(wallet.Id, wallet.Balance);
        }
    }
}