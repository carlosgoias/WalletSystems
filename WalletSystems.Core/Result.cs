namespace WalletSystems.Core
{
    public class Result
    {
        public Result(WalletExceptionTypes exception, bool success)
        {
            Exception = exception;
            Successed = success;
        }

        public WalletExceptionTypes Exception { get; }
        public bool Successed { get;  }

        public bool Failed => !Successed; 

        public static Result Success()
        {
            return new Result(WalletExceptionTypes.None, true);
        }

        public static Result Fail(WalletExceptionTypes exception)
        {
            return new Result(exception, false);
        }
    }
}
