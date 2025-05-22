using System;
using System.Threading.Tasks;
using System.Web.Http;
using WalletSystems.Core;
using WalletSystems.Dtos.Requests;
using WalletSystems.Extensions;
using WalletSystems.Services.Interfaces;

namespace WalletSystems.Controllers
{
    public class WalletsController : ApiController
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        [Route("wallets")]
        public IHttpActionResult Create()
        {
            var wallet = _walletService.Create();
            return Ok(wallet.ToDto());
        }

        [HttpGet]
        [Route("wallets/{id}")]
        public IHttpActionResult Get(Guid id)
        {
            var wallet = _walletService.Get(id);
            if (wallet is null) return NotFound();
            return Ok(wallet.ToDto());
        }

        [HttpPost]
        [Route("wallets/{id}/deposit")]
        public async Task<IHttpActionResult> Deposit(Guid id, [FromBody] TransactionRequestDto transactionRequestDto)
        {
            var result = await _walletService.DepositAsync(id, transactionRequestDto.Amount, transactionRequestDto.TransactionId);
            if (result.Successed) return Ok();

            switch(result.Exception)
            {
                case WalletExceptionTypes.TransactionAlreadyProcessed:
                    return BadRequest("Transaction already processed");
                case WalletExceptionTypes.WalletNotFound:
                    return NotFound();
                case WalletExceptionTypes.InvalidAmount:
                    return BadRequest("Invalid amount");
                default:
                    return InternalServerError(new Exception("Transaction failed"));
            }
        }

        [HttpPost]
        [Route("wallets/{id}/withdraw")]
        public async Task<IHttpActionResult> Withdraw(Guid id, [FromBody] TransactionRequestDto transactionRequestDto)
        {
            var result = await _walletService.WithdrawAsync(id, transactionRequestDto.Amount, transactionRequestDto.TransactionId);
            if (result.Successed) return Ok();
            
            switch(result.Exception)
            {
                case WalletExceptionTypes.TransactionAlreadyProcessed:
                    return BadRequest("Transaction already processed");
                case WalletExceptionTypes.InsufficientFunds:
                    return BadRequest("Insufficient funds");
                case WalletExceptionTypes.WalletNotFound:
                    return NotFound();
                case WalletExceptionTypes.InvalidAmount:
                    return BadRequest("Invalid amount");
                default:
                    return InternalServerError(new Exception("Transaction failed"));
            }
        }
    }
}