using WalletsSytems.Domain;
using WalletSystems.Responses.Dtos;

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