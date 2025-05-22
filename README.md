# 🏦 Wallet System – ASP.NET Web API (.NET Framework 4.8)

This project implements a **Wallet Service** for a sports betting platform using ASP.NET Web API 2 (.NET Framework 4.8). It exposes a RESTful interface to manage user wallet operations such as creating wallets, depositing funds, withdrawing funds, and checking balances.

The solution includes:
- ✅ Thread-safe wallet storage using `SemaphoreSlim`
- ✅ Resilient operations with retry + circuit breaker
- ✅ RESTful API endpoints documented via Swagger UI
- ✅ Unit-tested business logic
- ✅ Dependency Injection using Unity


## 🚀 Features

### 🔐 Wallet Operations
- **Create Wallet** – Generates a new wallet with 0 balance.
- **Deposit Funds** – Adds funds to a wallet (thread-safe).
- **Withdraw Funds** – Withdraws funds if balance is sufficient.
- **Get Wallet Balance** – Returns current balance of a wallet.

### ⚙️ Resilience with Circuit Breaker
To handle contention or failure scenarios (e.g. concurrency issues), both `DepositAsync` and `WithdrawAsync` include:

- ✅ **Retry logic**: up to 3 attempts with 200ms delay.
- ✅ **Circuit breaker pattern**:
  - After 3 consecutive failures, the circuit is **opened** for 5 seconds.
  - During this open state, all requests are rejected immediately.
  - After cooldown, the circuit resets and accepts new requests.

### 🧪 Concurrency Handling
Each wallet is protected by its own `SemaphoreSlim` instance to allow concurrent, isolated access. This avoids blocking unrelated wallet operations.

## 📘 Swagger UI (Interactive Docs)

This project includes Swagger UI (via [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)) to test and explore the API.

### ✅ How to use:
Run the application and open your browser at:

