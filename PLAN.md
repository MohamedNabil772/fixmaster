# FixMaster: Multi-Agent Execution Plan

## 1. Executive Summary
FixMaster is a platform connecting service providers ("Masters") with clients for repair and custom services. The system facilitates a bidding process, real-time tracking, secure payments, and administrative oversight. This plan outlines the transition from architecture design to full-scale implementation using a specialized multi-agent team, managed via a formal GitHub-based code review workflow.

---

## 2. Technical Stack & Standards
- **Backend**: .NET Core 8+ Microservices
  - **Architecture**: Clean Architecture + CQRS (MediatR)
  - **Patterns**: Factory, Specification, Repository, DRY
  - **Communication**: RabbitMQ/Azure Service Bus (Asynchronous), REST/gRPC (Synchronous)
- **Frontend**: Vue.js 3 (Composition API, Pinia)
- **Mobile**: Flutter or React Native (Cross-platform for Masters/Clients)
- **Database**: PostgreSQL (Relational data)
- **Logging**: Seq (Centralized logging) with Correlation ID middleware for distributed tracing.
- **Payments**: Integration with Online Gateways & Electronic Wallets.
- **Source Control**: GitHub (Branching strategy: GitHub Flow)
- **Code Review**: Mandatory PR reviews by the Team Leader for architectural compliance.

---

## 3. Agent Roles & Responsibilities

| Role | Key Focus Areas |
| :--- | :--- |
| **Senior UI/UX Designer** | User journeys, Bidding UI, Admin Dashboard, Mobile Prototypes. |
| **Senior Backend Developer** | Microservices, API Gateway, Seq Logging integration, GitHub PR creation. |
| **Senior Frontend Developer** | Vue.js Web Portal, Shared Component Library, Mobile App Logic, GitHub PR creation. |
| **Senior Tester** | TDD/BDD strategy, Load testing, Automated PR verification. |
| **Team Leader** | Repository management, PR Reviews, Sprint orchestration, Architectural compliance. |

---

## 4. Phased Execution Roadmap

### Phase 1: Foundation & Infrastructure (Team Lead + Backend)
- Setup Microservice template with Clean Architecture folders.
- Configure PostgreSQL containers and Seq logging instance.
- Implement `CorrelationIdMiddleware` across all services.
- Establish API Gateway (Ocelot or YARP).

### Phase 2: Design System & User Journeys (UI/UX + Frontend)
- Design the "Bidding War" interface for Masters.
- Design the "Service Progress" tracker for Clients.
- Define Admin personnel chat UI (Mockups only).
- Create Vue.js base project with Tailwind/SCSS.

### Phase 3: Core Service Development (Backend + Tester)
- **Identity Service**: JWT-based Auth for Clients, Masters, and Admins.
- **Request/Bidding Service**: Implementation of CQRS commands for `PostRequest`, `PlaceBid`, `AcceptBid`.
- **Specification Pattern**: Implement for filtering requests by category/location.

### Phase 4: Integration & Payments (Backend + Frontend)
- Integrate Payment Gateway (Stripe/PayPal/Local Providers).
- Implement Electronic Wallet logic (Balance, Transactions).
- **Verification Logic**: Photo/OTP-based service completion.

### Phase 5: Admin & Feedback (Full Team)
- Admin Dashboard for request monitoring.
- Feedback loop & Rating system implementation.
- Chat UI implementation (Admin-Client restricted).

### Phase 6: Mobile Application (Frontend/Mobile + Tester)
- Development of the mobile app for on-the-go bidding and notifications.
- Push notification service for alerts.

### Phase 7: Deployment & CI/CD (Team Lead + Tester)
- Setup Kubernetes/Docker Swarm for production-grade orchestration.
- Implement CI/CD pipelines for automated testing and deployment.
- Configure Prometheus/Grafana for real-time monitoring.

### Phase 8: Scaling & Security Hardening (Backend + Frontend)
- Implement Redis caching for high-traffic bidding data.
- Security hardening (Rate limiting, WAF, Database encryption).
- Performance optimization and load balancing.

---

## 5. Architectural Mandates
1. **CQRS**: All write operations must use Commands; all reads must use Queries.
2. **Logging**: Every log entry must include a `CorrelationId`. No exception.
3. **Clean Architecture**: Domain layer must have zero dependencies. Application layer contains business logic.
4. **DRY**: Shared logic (e.g., logging, validation) must reside in a `Common` NuGet/Library.
5. **Chat**: Implement the UI and API stubs for chat, but keep the `ChatService` disabled in the current production configuration.
6. **Code Review**: No code is merged into the main branch without an approved Pull Request (PR) from the Team Leader.
7. **Git Workflow**: Agents must create descriptive feature branches and link PRs to tasks.

---

## 6. Verification Strategy (Senior Tester)
- **Unit Tests**: XUnit/FluentAssertions for Domain logic.
- **Integration Tests**: TestContainers for PostgreSQL.
- **E2E Tests**: Playwright for Vue.js flows.
- **Performance**: Stress test the bidding broadcast mechanism.
- **Security**: Automated OWASP ZAP scans in the CI pipeline.

---

## 7. Next Steps
1. **Approval**: User reviews and approves the extended PLAN.md.
2. **Initialization**: Agents are assigned to Phase 1 tasks.
3. **First Sprint**: Setup of Identity and Bidding Microservices.
4. **DevOps Setup**: Initialize CI/CD pipelines early in the process.
