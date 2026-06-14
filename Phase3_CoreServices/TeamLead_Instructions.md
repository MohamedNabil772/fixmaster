# Team Lead Instructions: Phase 3 Core Services Implementation

## Overview
As part of Phase 3, we are focusing on the core authentication and bidding logic. All developers must adhere to the Clean Architecture and CQRS principles as outlined in the `PLAN.md`.

---

## 1. Backend Specifications (CQRS Commands)

### A. Identity Service: `LoginUserCommand`
**Purpose:** Authenticate users and provide a JWT token.
- **Location:** `FixMaster.Identity.Application.Users.Commands.LoginUser`
- **Structure:**
  ```csharp
  public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;
  ```
- **Expected Logic:**
  - Validate user credentials using `UserManager<User>`.
  - If successful, generate a JWT token using `IJwtTokenGenerator`.
  - Return `AuthResponse` containing user details and the token.
  - Throw a custom `UnauthorizedException` (or similar) on failure.

### B. Bidding Service: `SubmitBidCommand`
**Purpose:** Allow Masters to place a bid on an existing service request.
- **Location:** `FixMaster.Bidding.Application.Bids.Commands.SubmitBid`
- **Data Structure:**
  ```csharp
  public record SubmitBidCommand(
      Guid RequestId,
      Guid MasterId,
      decimal Amount,
      string Description) : IRequest<Guid>;
  ```
- **Requirements:**
  - **Entity Creation:** Define a `Bid` entity in `FixMaster.Bidding.Domain.Entities` if it doesn't exist.
    - Fields: `Id`, `RequestId`, `MasterId`, `Amount`, `Description`, `CreatedAt`, `Status` (Pending, Accepted, Rejected).
  - **Validation:** Ensure the `ServiceRequest` exists and is in `Open` status.
  - **Persistence:** Add the bid to the database and return its `Id`.

### C. Bidding Service: `SelectMasterCommand`
**Purpose:** Allow Clients to accept a specific bid, effectively closing the bidding process.
- **Location:** `FixMaster.Bidding.Application.Requests.Commands.SelectMaster`
- **Data Structure:**
  ```csharp
  public record SelectMasterCommand(Guid RequestId, Guid BidId) : IRequest;
  ```
- **Expected Logic:**
  - Validate that the `ServiceRequest` belongs to the current user (Client).
  - Update the `ServiceRequest.Status` to `BiddingClosed` or `InProgress`.
  - Update the selected `Bid.Status` to `Accepted`.
  - Mark all other bids for the same request as `Rejected`.
  - Trigger a domain event or integration event for notifications (Future task).

### D. Bidding Service: `GetBidsByRequestQuery`
**Purpose:** Retrieve bids for a specific service request with privacy rules.
- **Location:** `FixMaster.Bidding.Application.Bids.Queries.GetBidsByRequest`
- **Data Structure:**
  ```csharp
  public record GetBidsByRequestQuery(Guid RequestId) : IRequest<IEnumerable<BidResponse>>;
  ```
- **Mandatory Filtering Rule (HANDLER LOGIC):**
  - The handler MUST retrieve the current User's ID and Role from the identity context (e.g., `ICurrentUserService`).
  - Fetch the `ServiceRequest` to identify the `ClientId` (Owner).
  - **If User is the Request Owner (User.Id == ServiceRequest.ClientId):** Return ALL bids for this request.
  - **If User is a Master:** Return ONLY the bid created by this User (`MasterId == User.Id`).
  - **Bidders must NOT see other bidders' prices.**

---

## 2. Integration Events

### A. `ServiceRequestCreated`
**Purpose:** Notify the system that a new request has been created so providers can be alerted.
- **Location:** `FixMaster.Common.Events`
- **Structure:**
  ```csharp
  public record ServiceRequestCreated(
      Guid RequestId,
      string Title,
      string Description,
      string Category,
      decimal Budget,
      Guid ClientId,
      DateTime CreatedAt);
  ```

---

## 3. Frontend Guidance (Vue 3 + Pinia)

### ApiGateway Interaction
The Frontend must NOT communicate directly with microservices. All requests must go through the `ApiGateway` (YARP).
- **Identity Base URL:** `/api/identity/`
- **Bidding Base URL:** `/api/bidding/`

### Pinia Stores
- **AuthStore:**
  - Implement `login` action calling `POST /api/identity/users/login`.
  - Persist the JWT token in `localStorage` or `sessionStorage`.
  - Include the token in the `Authorization: Bearer <token>` header for all subsequent requests.
- **BiddingStore:**
  - Implement actions for `submitBid` and `selectMaster`.
  - Use reactive state to manage lists of requests and their associated bids.

### Interceptors & Correlation IDs
- Ensure all Axios/Fetch requests include the `X-Correlation-Id` header (automatically generated or passed from previous state) to support distributed tracing in Seq.

---

## 3. Architectural Mandates

1.  **CQRS Strictness:** Use `MediatR` for all commands. Controllers should only contain `_mediator.Send(command)`.
2.  **Domain Integrity:** Do not leak persistence logic into the Application layer. Use Interfaces (e.g., `IBiddingDbContext`) defined in Application and implemented in Infrastructure.
3.  **Validation:** Use `FluentValidation` for all command parameters.
4.  **Logging:** Log the start and completion of every command, ensuring the `CorrelationId` is present in the context.
5.  **Clean Code:** Follow the naming conventions established in existing commands (`RegisterUserCommand`, `CreateRequestCommand`).

---

**Approval Required:** Any deviation from these specifications must be discussed with the Team Lead before implementation.
