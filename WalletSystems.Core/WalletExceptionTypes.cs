namespace WalletSystems.Core
{
    public enum WalletExceptionTypes
    {
        None = 0,
        WalletNotFound = 1,
        InsufficientFunds = 2,
        TransactionAlreadyProcessed = 3,
        InvalidTransactionId = 4,
        InvalidAmount = 5,
        WalletAlreadyExists = 6,
        TransactionFailed = 7
    }
}
