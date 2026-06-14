# FixMaster: Role-Based Access Control (RBAC) Execution Plan

## 1. Overview
This plan outlines the implementation of a full RBAC system to ensure that users (Clients, Masters, and Admins) can only access features and data appropriate to their roles.

---

## 2. Agent Assignments

| Role | Agent | Focus Areas |
| :--- | :--- | :--- |
| **Team Leader** | TeamLeadAgent | Architectural compliance, PR reviews, final verification. |
| **Senior Backend Developer 1** | BackendDev1 | Identity Service: Role seeding, Registration updates, JWT Claims. |
| **Senior Backend Developer 2** | BackendDev2 | API Enforcement: [Authorize(Roles = "...")] implementation across services. |
| **Senior UI/UX Designer** | UIUXAgent | Role-based layout design, Conditional UI component states. |
| **Senior Frontend Developer 1** | FrontendDev1 | Auth Store updates, Router Guards, Role-based navigation logic. |
| **Senior Frontend Developer 2** | FrontendDev2 | Sidebar/Navbar integration, Role-based component visibility. |

---

## 3. Implementation Phases

### Phase 1: Identity & Security Foundation (BackendDev1 & BackendDev2)
1.  **Role Seeding**: Implement a seeder in `Identity.Infrastructure` to ensure `Admin`, `Client`, and `Master` roles exist in the database.
2.  **Registration Update**: Modify `RegisterUserCommand` to:
    *   Accept a role parameter (optional, defaults to `Client`).
    *   Assign the user to the specified role using `UserManager.AddToRoleAsync`.
3.  **JWT Claims**: Update `JwtTokenGenerator` to:
    *   Fetch roles for the user during login/registration.
    *   Add `ClaimTypes.Role` claims for every role the user belongs to.
4.  **Backend Enforcement**:
    *   Add `[Authorize(Roles = "Admin")]` to Admin-only controllers.
    *   Update `BiddingService` to restrict `PostRequest` to `Client` and `PlaceBid` to `Master`.

### Phase 2: Frontend Logic & Security (FrontendDev1 & UIUXAgent)
1.  **Auth Store Update**:
    *   Update `useAuthStore` to extract roles from the JWT or the `AuthResponse`.
    *   Persist the role in `localStorage` for UI consistency.
2.  **Router Guards**:
    *   Modify `src/router/index.ts` to check `meta.roles` on routes.
    *   Redirect users to an "Unauthorized" page or login if they lack the required role for `/admin`.
3.  **UI Design**:
    *   Design the user experience for "Access Denied" states.
    *   Define which sidebar items are visible for each role.

### Phase 3: UI Integration & Polishing (FrontendDev2 & UIUXAgent)
1.  **Conditional Sidebar**:
    *   Update `Sidebar.vue` to use `v-if` directives based on `authStore.user.role`.
    *   Ensure Admin links are strictly hidden for Clients/Masters.
2.  **Branding & Context**:
    *   Update the Navbar to display the user's role badge (e.g., "Admin", "Pro Master").

### Phase 4: Review & Validation (Team Lead)
1.  **PR Review**: Conduct deep architectural review of all changes.
2.  **E2E Testing**:
    *   Verify a new user defaults to "Client".
    *   Verify an "Admin" can access the dashboard while a "Client" cannot.
    *   Ensure JWTs correctly carry role claims across service boundaries.

---

## 4. Technical Mandates
*   **No Hardcoding**: Roles must be managed via the `IdentityRole` system, not hardcoded strings in business logic.
*   **Security First**: Frontend guards are for UX; backend role attributes are the source of truth for security.
*   **Consistency**: Use the same role naming conventions across Backend and Frontend.
