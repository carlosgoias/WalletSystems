
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using WalletsSytems.Domain;
using WalletSystems.Core;
using WalletSystems.Repository.Interfaces;
using WalletSystems.Services;

namespace WalletSystems.Tests
{
    [TestClass]
    public class WalletServiceTests
    {
        private Mock<IWalletRepository> _walletRepositoryMock;
        private WalletService _walletService;
        private Guid _walletId;
        private string _transactionId;

        [TestInitialize]
        public void Setup()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _walletService = new WalletService(_walletRepositoryMock.Object);
            _walletId = Guid.NewGuid();
            _transactionId = Guid.NewGuid().ToString();
        }

        [TestMethod]
        public async Task DepositAsync_InvalidAmount_ShouldFail()
        {
            var result = await _walletService.DepositAsync(_walletId, 0, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.InvalidAmount, result.Exception);
        }

        [TestMethod]
        public async Task DepositAsync_WalletNotFound_ShouldFail()
        {
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns((Wallet)null);

            var result = await _walletService.DepositAsync(_walletId, 100, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.WalletNotFound, result.Exception);
        }

        [TestMethod]
        public async Task DepositAsync_DuplicateTransaction_ShouldFail()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100,
            };
            wallet.ProcessedTransactions.Add(_transactionId);
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);

            var result = await _walletService.DepositAsync(_walletId, 100, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.TransactionAlreadyProcessed, result.Exception);
        }

        [TestMethod]
        public async Task DepositAsync_RepositoryFails_ShouldFail()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100
            };
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);
            _walletRepositoryMock.Setup(r => r.DepositAsync(_walletId, 100, _transactionId)).ReturnsAsync(false);

            var result = await _walletService.DepositAsync(_walletId, 100, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.TransactionFailed, result.Exception);
        }

        [TestMethod]
        public async Task DepositAsync_Valid_ShouldSucceed()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100
            };
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);
            _walletRepositoryMock.Setup(r => r.DepositAsync(_walletId, 100, _transactionId)).ReturnsAsync(true);

            var result = await _walletService.DepositAsync(_walletId, 100, _transactionId);
            Assert.IsTrue(result.Successed);
        }

        [TestMethod]
        public async Task WithdrawAsync_InvalidAmount_ShouldFail()
        {
            var result = await _walletService.WithdrawAsync(_walletId, 0, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.InvalidAmount, result.Exception);
        }

        [TestMethod]
        public async Task WithdrawAsync_WalletNotFound_ShouldFail()
        {
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns((Wallet)null);

            var result = await _walletService.WithdrawAsync(_walletId, 50, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.WalletNotFound, result.Exception);
        }

        [TestMethod]
        public async Task WithdrawAsync_DuplicateTransaction_ShouldFail()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100
            };
            wallet.ProcessedTransactions.Add(_transactionId);
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);

            var result = await _walletService.WithdrawAsync(_walletId, 50, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.TransactionAlreadyProcessed, result.Exception);
        }

        [TestMethod]
        public async Task WithdrawAsync_InsufficientFunds_ShouldFail()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 20,
            };
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);

            var result = await _walletService.WithdrawAsync(_walletId, 50, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.InsufficientFunds, result.Exception);
        }

        [TestMethod]
        public async Task WithdrawAsync_RepositoryFails_ShouldFail()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100
            };
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);
            _walletRepositoryMock.Setup(r => r.WithdrawAsync(_walletId, 50, _transactionId)).ReturnsAsync(false);

            var result = await _walletService.WithdrawAsync(_walletId, 50, _transactionId);
            Assert.IsFalse(result.Successed);
            Assert.AreEqual(WalletExceptionTypes.TransactionFailed, result.Exception);
        }

        [TestMethod]
        public async Task WithdrawAsync_Valid_ShouldSucceed()
        {
            var wallet = new Wallet
            {
                Id = _walletId,
                Balance = 100
            };
            _walletRepositoryMock.Setup(r => r.Get(_walletId)).Returns(wallet);
            _walletRepositoryMock.Setup(r => r.WithdrawAsync(_walletId, 50, _transactionId)).ReturnsAsync(true);

            var result = await _walletService.WithdrawAsync(_walletId, 50, _transactionId);
            Assert.IsTrue(result.Successed);
        }
    }
}
