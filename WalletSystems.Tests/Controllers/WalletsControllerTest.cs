
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WalletsSytems.Domain;
using WalletSystems.Controllers;
using WalletSystems.Core;
using WalletSystems.Dtos.Requests;
using WalletSystems.Services.Interfaces;

namespace WalletSystems.Tests.Controllers
{
    [TestClass]
    public class WalletsControllerTests
    {
        private Mock<IWalletService> _walletServiceMock;
        private WalletsController _controller;
        private Guid _walletId;
        private string _transactionId;

        [TestInitialize]
        public void Setup()
        {
            _walletServiceMock = new Mock<IWalletService>();
            _controller = new WalletsController(_walletServiceMock.Object);
            _walletId = Guid.NewGuid();
            _transactionId = Guid.NewGuid().ToString();
        }

        [TestMethod]
        public void Create_ShouldReturnWallet()
        {
            var wallet = new Wallet { Id = _walletId, Balance = 0 };
            _walletServiceMock.Setup(s => s.Create()).Returns(wallet);

            var result = _controller.Create();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Get_WalletFound_ShouldReturnOk()
        {
            var wallet = new Wallet { Id = _walletId, Balance = 100 };
            _walletServiceMock.Setup(s => s.Get(_walletId)).Returns(wallet);

            var result = _controller.Get(_walletId) as OkNegotiatedContentResult<Wallet>;
            Assert.IsNotNull(result);
            Assert.AreEqual(wallet.Id, result.Content.Id);
        }

        [TestMethod]
        public void Get_WalletNotFound_ShouldReturnNotFound()
        {
            _walletServiceMock.Setup(s => s.Get(_walletId)).Returns((Wallet)null);

            var result = _controller.Get(_walletId);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Deposit_Success_ShouldReturnOk()
        {
            _walletServiceMock.Setup(s => s.DepositAsync(_walletId, 100, _transactionId))
                .ReturnsAsync(Result.Success());

            var result = await _controller.Deposit(_walletId, new TransactionRequestDto { Amount = 100, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Deposit_AlreadyProcessed_ShouldReturnBadRequest()
        {
            _walletServiceMock.Setup(s => s.DepositAsync(_walletId, 100, _transactionId))
                .ReturnsAsync(Result.Fail(WalletExceptionTypes.TransactionAlreadyProcessed));

            var result = await _controller.Deposit(_walletId, new TransactionRequestDto { Amount = 100, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task Deposit_WalletNotFound_ShouldReturnNotFound()
        {
            _walletServiceMock.Setup(s => s.DepositAsync(_walletId, 100, _transactionId))
                .ReturnsAsync(Result.Fail(WalletExceptionTypes.WalletNotFound));

            var result = await _controller.Deposit(_walletId, new TransactionRequestDto { Amount = 100, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Withdraw_Success_ShouldReturnOk()
        {
            _walletServiceMock.Setup(s => s.WithdrawAsync(_walletId, 50, _transactionId))
                .ReturnsAsync(Result.Success());

            var result = await _controller.Withdraw(_walletId, new TransactionRequestDto { Amount = 50, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Withdraw_InsufficientFunds_ShouldReturnBadRequest()
        {
            _walletServiceMock.Setup(s => s.WithdrawAsync(_walletId, 999, _transactionId))
                .ReturnsAsync(Result.Fail(WalletExceptionTypes.InsufficientFunds));

            var result = await _controller.Withdraw(_walletId, new TransactionRequestDto { Amount = 999, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task Withdraw_WalletNotFound_ShouldReturnNotFound()
        {
            _walletServiceMock.Setup(s => s.WithdrawAsync(_walletId, 100, _transactionId))
                .ReturnsAsync(Result.Fail(WalletExceptionTypes.WalletNotFound));

            var result = await _controller.Withdraw(_walletId, new TransactionRequestDto { Amount = 100, TransactionId = _transactionId });
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
