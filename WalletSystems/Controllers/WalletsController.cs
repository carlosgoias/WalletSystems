using System;
using System.Threading.Tasks;
using System.Web.Http;
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
            return Ok(wallet);
        }

        [HttpPost]
        [Route("wallets/{id}/deposit")]
        public async Task<IHttpActionResult> Deposit(Guid id, [FromBody] decimal amount)
        {
            var result = await _walletService.DepositAsync(id, amount);
            if (result) return Ok();
            return NotFound();
        }

        [HttpPost]
        [Route("wallets/{id}/withdraw")]
        public async Task<IHttpActionResult> Withdraw(Guid id, [FromBody] decimal amount)
        {
            var result = await _walletService.WithdrawAsync(id, amount);
            if (result) return Ok();
            return BadRequest("Insufficient funds or wallet not found");
        }
    }
}