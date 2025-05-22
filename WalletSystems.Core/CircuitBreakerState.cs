using System;

namespace WalletSystems.Core
{
    public class CircuitBreakerState
    {
        public int FailureCount { get; private set; }
        public DateTime LastFailureTime { get; private set; }

        public bool IsOpen => FailureCount >= 3 && (DateTime.UtcNow - LastFailureTime) < TimeSpan.FromSeconds(5);

        public void RecordFailure()
        {
            FailureCount++;
            LastFailureTime = DateTime.UtcNow;
        }

        public void Reset()
        {
            FailureCount = 0;
        }
    }
}
