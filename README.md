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
Run the application and open your browser

## 🚀 What Needs to Be Improved for Production Readiness

| Area | Recommendation |
|------|----------------|
| **Persistence** | Replace in-memory storage with a real database (e.g., SQL Server, PostgreSQL). Use transactions or optimistic concurrency. |
| **Concurrency & Idempotency** | Use distributed locking (e.g., Redis, SQL advisory locks) and persist transaction IDs to ensure true idempotency across nodes and restarts. |
| **Resilience** | Replace custom circuit breaker with a library like [Polly](https://github.com/App-vNext/Polly) to handle retries, fallbacks, and timeouts cleanly. |
| **Security** | Implement JWT/OAuth2 authentication, authorization per wallet, and rate limiting. |
| **Validation** | Add FluentValidation or Data Annotations to validate incoming requests. |
| **Observability** | Integrate Serilog for structured logging, Prometheus/Grafana for metrics, and OpenTelemetry for tracing. |
| **API Design** | Standardize error responses, introduce API versioning (e.g., `/api/v1/wallets`), and document endpoints with examples. |
| **Testing** | Add integration tests with real DB, load tests for concurrency, and mock-based edge case tests. |
| **CI/CD** | Setup GitHub Actions or GitLab CI for continuous testing and deployment. |
| **Deployment** | Use Docker, container orchestration (Kubernetes/ECS), and proper secrets management. |
| **Compliance** | Add audit logs, data encryption, and support for GDPR (e.g., user data export/deletion). |

