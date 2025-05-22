using System;
using System.Collections.Generic;

namespace WalletsSytems.Domain
{
    public class Wallet
    {
        public Wallet()
        {
            ProcessedTransactions = new HashSet<string>();
        }
        public Guid Id { get; set; }
        public decimal Balance { get; set; }

        public HashSet<string> ProcessedTransactions { get; }

    }
}
