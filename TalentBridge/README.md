Full Stack Final Project Documentation
Student Information

Name, Surname: _______________________
Group: _______________________
Date: _______________________
Contact Email: _______________________


1. Project Overview
1.1 Project Name
TalentBridge - Universal HR Management and Recruitment System
1.2 Project Brief Description (2-3 sentences)
TalentBridge is a web-based platform that enables all types of organizations (schools, clinics, private companies, government agencies) to efficiently conduct staff recruitment processes. The system includes vacancy posting, candidate registration, profession-specific online testing with multiple question types, and automatic result evaluation with comprehensive analytics.
1.3 Real Problem the Project Solves
Detailed Problem Description:
Organizations across all industries face significant challenges in the employee recruitment process, which is time-consuming, expensive, and resource-intensive. Traditional methods involve physical interviews and paper-based assessments, limiting the number of candidates that can be processed and reducing the possibility of objective, fair evaluation. The lack of standardized testing, poor candidate tracking, and inability to efficiently manage large applicant pools create bottlenecks in hiring.
Who Does Your Project Help (Target Audience):

HR managers across all industries needing efficient recruitment tools
Job seekers from all professions looking for streamlined application processes
Small and medium-sized businesses without dedicated HR departments
Government agencies requiring transparent and fair hiring processes
Recruiting companies managing multiple clients and positions
Educational institutions hiring teachers and staff
Healthcare facilities recruiting medical professionals

Why This Problem Is Important:
Selecting appropriate staff is critical to the success of any organization. Poor hiring decisions cost organizations both time and money through employee turnover, reduced productivity, and training expenses. An effective HR system saves time (reducing time-to-hire by up to 50%), reduces costs (lowering recruitment expenses by 30-40%), and ensures finding the best candidates through objective, data-driven criteria. Additionally, it provides transparency and accountability in the hiring process, which is especially important for government and public sector organizations.

2. Technology Stack
2.1 Backend Technologies

Framework: ASP.NET Web API (.NET Version: 8.0)
Database:

☑ SQL Server


ORM:

☑ Entity Framework Core


Authentication: JWT (JSON Web Tokens)
Email Service: SendGrid / SMTP
File Storage: Azure Blob Storage / Local File System
Additional Libraries:

AutoMapper (DTO mapping)
FluentValidation (input validation)
Serilog (structured logging)
Swashbuckle (API documentation)
BCrypt.Net (password hashing)



2.2 Frontend Technologies

Primary Framework/Library:

☑ Angular (Version: 17)


CSS Framework:

☑ Bootstrap (Version 5)


Additional Libraries:

Angular Material (UI components)
Chart.js (data visualization and statistics)
ng-bootstrap (Bootstrap components for Angular)
RxJS (reactive programming)
NgRx (state management - optional)
PrimeNG (additional UI components)




3. API Models and Structure
3.1 Core Models
Model 1:
Model Name: User
Properties:
- int Id (Primary Key)
- string Email (unique, required, max 255 chars)
- string Password (hashed, required)
- string? AppleId (nullable)
- string? GoogleId (nullable)
- ROLES Role (enum: USER, HR_MANAGER, ADMIN)
- bool IsVerified (default: false)
- string AuthProvider (default: "Email")
- string FirstName (required, max 50 chars)
- string LastName (required, max 50 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- UserDetails? Details (1:1)
- HRManager? HRManager (1:1)
- List<Application> Applications (1:N)
- List<SavedVacancy> SavedVacancies (1:N)
- List<Notification> Notifications (1:N)
```

**Model 2:**
```
Model Name: Organization<TExactType>
Properties:
- int Id (Primary Key)
- string Name (required, max 200 chars)
- string Address (required)
- string ContactEmail (required, email format)
- string PhoneNumber (required)
- TYPES Type (enum: School, Clinic, Company, Government, NGO)
- TExactType ExactType (generic for specific organization types)
- string? Logo (nullable, file path/URL)
- string? Website (nullable, URL format)
- string Description (required, max 2000 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- List<HRManager> HRManagers (1:N)
- List<Vacancy> Vacancies (1:N)
- List<Test> Tests (1:N)
```

**Model 3:**
```
Model Name: UserDetails
Properties:
- int Id (Primary Key)
- int UserId (Foreign Key → User)
- string PhoneNumber (required)
- string? ProfilePictureUrl (nullable, file path)
- GENDER Gender (enum: Male, Female, Other, PreferNotToSay)
- string? Bio (nullable, max 500 chars)
- string? CVPdfUrl (nullable, file path)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- User User (1:1)
- List<Education> Educations (1:N)
- List<Experience> Experiences (1:N)
- List<string> Skills (JSON array)
```

**Model 4:**
```
Model Name: HRManager
Properties:
- int Id (Primary Key)
- int UserId (Foreign Key → User)
- int OrganizationId (Foreign Key → Organization)
- string Position (required, max 100 chars)
- string? Department (nullable, max 100 chars)
- DateTime HiredDate
- string? Permissions (nullable, JSON serialized)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- User User (1:1)
- Organization Organization (N:1)
- List<Vacancy> Vacancies (1:N as CreatedBy)
- List<Test> Tests (1:N as CreatedBy)
```

**Model 5:**
```
Model Name: Vacancy
Properties:
- int Id (Primary Key)
- int OrganizationId (Foreign Key → Organization)
- int CreatedBy (Foreign Key → HRManager)
- string Title (required, max 200 chars)
- string Description (required, max 5000 chars)
- string Requirements (required, max 2000 chars)
- string Responsibilities (required, max 2000 chars)
- string Profession (required, max 100 chars)
- string Industry (required, max 100 chars)
- EMPLOYMENT_TYPE EmploymentType (enum)
- EXPERIENCE_LEVEL ExperienceLevel (enum)
- decimal? SalaryMin (nullable)
- decimal? SalaryMax (nullable)
- string SalaryCurrency (default: "GEL")
- string Location (required, max 200 chars)
- bool IsRemote (default: false)
- VACANCY_STATUS Status (enum, default: Draft)
- DateTime ApplicationDeadline
- DateTime? PublishedAt (nullable)
- int ViewCount (default: 0)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Organization Organization (N:1)
- HRManager CreatedByHRManager (N:1)
- List<Application> Applications (1:N)
- List<SavedVacancy> SavedByUsers (1:N)
- List<VacancyView> Views (1:N)
```

**Model 6:**
```
Model Name: Application
Properties:
- int Id (Primary Key)
- int VacancyId (Foreign Key → Vacancy)
- int UserId (Foreign Key → User)
- APPLICATION_STATUS Status (enum, default: Pending)
- string? CoverLetter (nullable, max 2000 chars)
- DateTime AppliedAt (default: current timestamp)
- int? ReviewedBy (nullable, Foreign Key → HRManager)
- string? ReviewNotes (nullable, max 1000 chars)
- string? RejectionReason (nullable, max 500 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Vacancy Vacancy (N:1)
- User User (N:1)
- HRManager? ReviewedByHRManager (N:1)
- List<TestAssignment> TestAssignments (1:N)
- List<ApplicationTimeline> Timeline (1:N)
- List<Message> Messages (1:N)
```

**Model 7:**
```
Model Name: Test
Properties:
- int Id (Primary Key)
- int OrganizationId (Foreign Key → Organization)
- int CreatedBy (Foreign Key → HRManager)
- string Title (required, max 200 chars)
- string Description (required, max 1000 chars)
- string Profession (required, max 100 chars)
- string Industry (required, max 100 chars)
- int DurationMinutes (required, range: 5-300)
- decimal PassingScore (required, percentage 0-100)
- decimal TotalPoints (required)
- TEST_DIFFICULTY Difficulty (enum: Easy, Medium, Hard)
- bool IsActive (default: true)
- string? Instructions (nullable, max 2000 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Organization Organization (N:1)
- HRManager CreatedByHRManager (N:1)
- List<Question> Questions (1:N)
- List<TestAssignment> Assignments (1:N)
```

**Model 8:**
```
Model Name: Question
Properties:
- int Id (Primary Key)
- int TestId (Foreign Key → Test)
- string QuestionText (required, max 1000 chars)
- QUESTION_TYPE QuestionType (enum)
- decimal Points (required)
- int OrderNumber (required)
- int? TimeLimitSeconds (nullable)
- bool IsRequired (default: true)
- string? Explanation (nullable, max 500 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Test Test (N:1)
- List<QuestionOption> Options (1:N)
- List<SubmissionAnswer> Answers (1:N)
```

**Model 9:**
```
Model Name: QuestionOption
Properties:
- int Id (Primary Key)
- int QuestionId (Foreign Key → Question)
- string OptionText (required, max 500 chars)
- bool IsCorrect (required)
- int OrderNumber (required)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Question Question (N:1)
```

**Model 10:**
```
Model Name: TestAssignment
Properties:
- int Id (Primary Key)
- int ApplicationId (Foreign Key → Application)
- int TestId (Foreign Key → Test)
- int AssignedBy (Foreign Key → HRManager)
- DateTime AssignedAt (default: current timestamp)
- DateTime ExpiresAt (required)
- DateTime? StartedAt (nullable)
- DateTime? CompletedAt (nullable)
- TEST_ASSIGNMENT_STATUS Status (enum, default: Assigned)
- string AccessToken (required, unique, for secure access)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- Application Application (N:1)
- Test Test (N:1)
- HRManager AssignedByHRManager (N:1)
- TestSubmission? Submission (1:1)
```

**Model 11:**
```
Model Name: TestSubmission
Properties:
- int Id (Primary Key)
- int TestAssignmentId (Foreign Key → TestAssignment, unique)
- int UserId (Foreign Key → User)
- int TestId (Foreign Key → Test)
- DateTime StartTime
- DateTime? EndTime (nullable)
- DateTime? SubmittedAt (nullable)
- decimal? TotalPointsEarned (nullable)
- decimal? PercentageScore (nullable)
- bool? IsPassed (nullable)
- bool IsAutoGraded (default: true)
- bool RequiresManualReview (default: false)
- int? ReviewedBy (nullable, Foreign Key → HRManager)
- DateTime? ReviewedAt (nullable)
- string? Feedback (nullable, max 2000 chars)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- TestAssignment TestAssignment (1:1)
- User User (N:1)
- Test Test (N:1)
- HRManager? ReviewedByHRManager (N:1)
- List<SubmissionAnswer> Answers (1:N)
```

**Model 12:**
```
Model Name: SubmissionAnswer
Properties:
- int Id (Primary Key)
- int TestSubmissionId (Foreign Key → TestSubmission)
- int QuestionId (Foreign Key → Question)
- int? SelectedOptionId (nullable, Foreign Key → QuestionOption)
- string? AnswerText (nullable, max 5000 chars)
- decimal? PointsAwarded (nullable)
- bool? IsCorrect (nullable)
- DateTime AnsweredAt
- int TimeSpentSeconds (required)
- DateTime CreatedAt
- DateTime UpdatedAt
Navigation Properties:
- TestSubmission TestSubmission (N:1)
- Question Question (N:1)
- QuestionOption? SelectedOption (N:1)
```

**Additional Models:**
- Education (user education records)
- Experience (user work experience)
- SavedVacancy (bookmarked vacancies)
- VacancyView (analytics tracking)
- ApplicationTimeline (status history)
- Notification (user notifications)
- Message (internal messaging)
- EmailVerificationToken (email verification)
- PasswordResetToken (password resets)
- RefreshToken (JWT refresh tokens)
- AuditLog (system audit trail)

### 3.2 Model Relationships

**One-to-One:**
- User ↔ UserDetails
- User ↔ HRManager
- TestAssignment ↔ TestSubmission

**One-to-Many:**
- User → Applications (as candidate)
- User → SavedVacancies
- User → Notifications
- User → Messages (as sender/receiver)
- UserDetails → Educations
- UserDetails → Experiences
- Organization → HRManagers
- Organization → Vacancies
- Organization → Tests
- HRManager → Vacancies (as creator)
- HRManager → Tests (as creator)
- Vacancy → Applications
- Vacancy → SavedVacancies
- Vacancy → VacancyViews
- Application → TestAssignments
- Application → ApplicationTimeline entries
- Application → Messages
- Test → Questions
- Test → TestAssignments
- Question → QuestionOptions
- Question → SubmissionAnswers
- TestSubmission → SubmissionAnswers
- Message → Replies (self-referencing)

**Many-to-Many (via junction tables):**
- Users ↔ Vacancies (via SavedVacancy)
- Applications ↔ Tests (via TestAssignment)

### 3.3 API Endpoints (Minimum 10)

| HTTP Method | Endpoint | Description |
|-------------|----------|-------------|
| POST | /api/auth/register | User registration with email/password or OAuth |
| POST | /api/auth/login | User authentication and JWT token generation |
| POST | /api/auth/refresh-token | Refresh expired JWT access token |
| POST | /api/auth/forgot-password | Request password reset email |
| POST | /api/auth/reset-password | Reset password with token |
| POST | /api/auth/verify-email | Verify email address with token |
| GET | /api/users/me | Get current authenticated user profile |
| PUT | /api/users/me | Update current user information |
| GET | /api/users/me/details | Get detailed user profile (education, experience, skills) |
| PUT | /api/users/me/details | Update user details |
| POST | /api/users/me/cv | Upload CV/resume file |
| POST | /api/users/me/profile-picture | Upload profile picture |
| POST | /api/organizations | Create new organization |
| GET | /api/organizations/{id} | Get organization details by ID |
| GET | /api/organizations | List all organizations with pagination and filters |
| PUT | /api/organizations/{id} | Update organization information |
| GET | /api/organizations/{id}/statistics | Get organization recruitment statistics |
| POST | /api/hr-managers | Create HR manager profile (link user to organization) |
| GET | /api/hr-managers/me | Get current user's HR manager profile |
| POST | /api/vacancies | Create new job vacancy |
| GET | /api/vacancies/{id} | Get vacancy details by ID |
| GET | /api/vacancies | List all vacancies with advanced filters |
| GET | /api/vacancies/search | Search vacancies by keywords, location, etc. |
| PUT | /api/vacancies/{id} | Update vacancy information |
| PATCH | /api/vacancies/{id}/status | Change vacancy status (publish, close, etc.) |
| DELETE | /api/vacancies/{id} | Delete vacancy |
| GET | /api/vacancies/{id}/applications | Get all applications for a specific vacancy |
| GET | /api/vacancies/{id}/statistics | Get vacancy statistics (views, applications) |
| POST | /api/applications | Submit application to a vacancy |
| GET | /api/applications/{id} | Get application details |
| GET | /api/applications/my-applications | Get current user's applications |
| PUT | /api/applications/{id} | Update application (cover letter) |
| PATCH | /api/applications/{id}/status | Change application status (HR only) |
| POST | /api/applications/{id}/withdraw | Withdraw application |
| GET | /api/applications/{id}/timeline | Get application status history |
| POST | /api/tests | Create new test |
| GET | /api/tests/{id} | Get test details |
| GET | /api/tests | List tests with filters |
| PUT | /api/tests/{id} | Update test |
| DELETE | /api/tests/{id} | Delete test |
| POST | /api/tests/{testId}/questions | Add question to test |
| GET | /api/tests/{testId}/questions | Get all questions in test |
| PUT | /api/tests/{testId}/questions/{questionId} | Update question |
| DELETE | /api/tests/{testId}/questions/{questionId} | Delete question |
| POST | /api/questions/{questionId}/options | Add answer option to question |
| POST | /api/test-assignments | Assign test to application |
| GET | /api/test-assignments/{id} | Get test assignment details |
| GET | /api/test-assignments/my-assignments | Get current user's test assignments |
| POST | /api/test-submissions/{assignmentId}/start | Start test (create submission) |
| POST | /api/test-submissions/{id}/save-answer | Save/update answer (auto-save) |
| POST | /api/test-submissions/{id}/submit | Submit completed test |
| GET | /api/test-submissions/{id}/results | Get test results |
| POST | /api/test-submissions/{id}/grade | Manual grading (HR only) |
| POST | /api/saved-vacancies | Save/bookmark vacancy |
| GET | /api/saved-vacancies | Get current user's saved vacancies |
| DELETE | /api/saved-vacancies/{id} | Remove saved vacancy |
| GET | /api/notifications | Get current user's notifications |
| PATCH | /api/notifications/{id}/read | Mark notification as read |
| PATCH | /api/notifications/mark-all-read | Mark all notifications as read |
| POST | /api/messages | Send message to another user |
| GET | /api/messages | Get inbox messages |
| GET | /api/messages/conversation/{userId} | Get conversation with specific user |
| PATCH | /api/messages/{id}/read | Mark message as read |
| GET | /api/dashboard/candidate | Get candidate dashboard data |
| GET | /api/dashboard/hr-manager | Get HR manager dashboard data |
| GET | /api/reports/applications | Generate applications report |
| GET | /api/reports/vacancies | Generate vacancies report |
| GET | /api/reports/tests/performance | Generate test performance report |
| POST | /api/reports/export | Export report as CSV/PDF |
| GET | /api/public/vacancies | Public vacancy search (no auth required) |
| GET | /api/public/vacancies/{id} | Public vacancy details (no auth required) |

**Total Endpoints:** 70+ comprehensive REST API endpoints

---

## 4. Functionality

### 4.1 Core Features

1. **Multi-Role Authentication System**
   - Email/password registration and login
   - OAuth integration (Google, Apple)
   - JWT-based authentication with refresh tokens
   - Email verification requirement
   - Password reset functionality
   - Role-based access control (USER, HR_MANAGER, ADMIN)

2. **Comprehensive User Profile Management**
   - Personal information management
   - Education history tracking (multiple degrees)
   - Work experience records (multiple positions)
   - Skills list management (dynamic array)
   - CV/resume upload and storage
   - Profile picture upload and management
   - Bio and contact information

3. **Organization Management System**
   - Flexible organization types (School, Clinic, Company, Government, NGO)
   - Organization branding (logo, description)
   - Multiple HR managers per organization
   - Organization statistics dashboard
   - Industry and type categorization

4. **Advanced Vacancy Management**
   - Create detailed job postings with rich descriptions
   - Define requirements and responsibilities
   - Set employment types (Full-time, Part-time, Contract, Internship, Freelance)
   - Target experience levels (Entry, Junior, Mid, Senior, Lead, Executive)
   - Specify salary ranges with currency
   - Location and remote work options
   - Application deadline management
   - Vacancy status workflow (Draft → Active → Closed)
   - View count tracking for analytics

5. **Application Tracking System**
   - One-click application submission
   - Cover letter support
   - Comprehensive status tracking (Pending, Under Review, Test Assigned, Test Completed, Interviewed, Shortlisted, Accepted, Rejected, Withdrawn)
   - Application timeline/history tracking
   - HR review notes and rejection reasons
   - Application withdrawal capability
   - Link to test assignments

6. **Flexible Online Testing Platform**
   - Create reusable tests for positions
   - Support for 6 question types:
     * Multiple Choice (auto-graded)
     * True/False (auto-graded)
     * Short Answer (manual review)
     * Essay (manual review)
     * Coding challenges (manual/auto)
     * File Upload (manual review)
   - Configurable test duration (5-300 minutes)
   - Set passing score thresholds (percentage-based)
   - Test difficulty levels (Easy, Medium, Hard)
   - Question ordering and time limits per question
   - Test instructions and explanations

7. **Secure Test-Taking Experience**
   - Unique access tokens for each test assignment
   - Time-limited test sessions with countdown
   - Auto-save functionality for answers
   - Track time spent per question
   - Support for multiple answer types
   - Progress tracking during test
   - Submission confirmation

8. **Intelligent Test Grading System**
   - Automatic grading for objective questions (Multiple Choice, True/False)
   - Manual review workflow for subjective answers
   - Points awarded tracking per question
   - Percentage score calculation
   - Pass/fail determination based on threshold
   - HR manager feedback system
   - Detailed submission analytics

9. **Internal Communication System**
   - Direct messaging between users
   - Message threading (reply to messages)
   - Application-specific conversations
   - Read/unread status tracking
   - Inbox and sent messages views
   - Conversation history

10. **Comprehensive Notification System**
    - 10+ notification types:
      * Application received
      * Application status changed
      * Test assigned
      * Test completed
      * Test expiring soon
      * Vacancy expiring
      * New message received
      * System alerts
    - Real-time notifications
    - Deep linking via ActionUrl
    - Mark as read functionality
    - Unread count display

11. **Advanced Search and Discovery**
    - Multi-criteria vacancy search
    - Filters: profession, industry, location, employment type, experience level, salary range, remote work
    - Keyword-based search
    - Sort by: date, salary, popularity
    - Save/bookmark favorite vacancies with personal notes
    - Similar vacancy recommendations

12. **Analytics and Reporting**
    - Candidate dashboard (application statistics, test results)
    - HR dashboard (recruitment metrics, pending applications)
    - Organization analytics (vacancies, applications, time-to-hire)
    - Vacancy performance tracking (views, conversion rates)
    - Test performance analytics (average scores, question difficulty)
    - Recruitment pipeline visualization
    - Export reports as CSV/PDF

13. **File Management System**
    - CV/resume upload (PDF, DOCX)
    - Profile picture upload (JPG, PNG, WebP)
    - Organization logo upload
    - Test answer file uploads
    - File size validation and compression
    - Secure file storage (cloud or local)

14. **Audit and Security**
    - Complete audit trail (AuditLog)
    - Track all sensitive operations
    - IP address and user agent logging
    - Monitor login attempts
    - Application status change history
    - GDPR compliance features

15. **Public Job Board**
    - Public vacancy search (no authentication required)
    - View vacancy details
    - Organization public profiles
    - Anonymous view tracking for analytics

### 4.2 User Roles

**Administrator (ADMIN)**
- **Permissions:**
  * Full system access
  * Manage all organizations
  * View system-wide statistics
  * Access complete audit logs
  * Manage users across all organizations
  * System configuration and settings
  * Monitor system health and performance
  * Resolve disputes and issues
  * Export comprehensive reports

**HR Manager (HR_MANAGER)**
- **Permissions:**
  * All USER permissions
  * Linked to specific organization
  * Create and manage vacancies for their organization
  * Review applications for their vacancies
  * Change application statuses
  * Add review notes and rejection reasons
  * Create and manage tests for their organization
  * Create questions with multiple types and answer options
  * Assign tests to candidates
  * Grade test submissions (auto and manual)
  * Provide detailed feedback on submissions
  * Communicate with candidates via messaging
  * View organization-level analytics
  * Manage organization profile and branding
  * Track vacancy performance metrics
  * Export organization reports

**Registered User / Candidate (USER)**
- **Permissions:**
  * Browse and search public vacancies
  * View detailed vacancy information
  * Apply to vacancies with cover letter
  * Manage comprehensive profile:
    - Personal details (name, email, phone, bio)
    - Education history (multiple degrees)
    - Work experience (multiple positions)
    - Skills list (dynamic array)
    - CV/resume upload
    - Profile picture
  * Take assigned tests with various question types
  * View application status and complete timeline
  * Save/bookmark favorite vacancies with notes
  * Communicate with HR managers via messages
  * View test results and detailed feedback
  * Receive notifications for all relevant events
  * Withdraw applications
  * View personal dashboard with statistics
  * Access application history

**Guest / Visitor (No Authentication)**
- **Permissions:**
  * Browse public vacancy listings
  * Search vacancies with filters
  * View public vacancy details
  * View public organization profiles
  * View list of industries and professions
  * Register for new account
  * No ability to apply, save, or communicate

### 4.3 Authorization Logic

**JWT Token-Based Authentication:**
- Access tokens (short-lived: 15-30 minutes)
- Refresh tokens (long-lived: 7-30 days)
- Token rotation and revocation for security
- Secure token storage and transmission

**Role-Based Access Control (RBAC):**
- Every endpoint protected with `[Authorize]` attribute
- Role-specific endpoints with `[Authorize(Roles = "ADMIN")]`
- Resource ownership validation (users can only modify their own data)
- Organization boundary enforcement (HR managers limited to their organization)

**Security Checks:**
- Email verification required for full access
- Password strength requirements (8+ chars, uppercase, lowercase, number, special char)
- Rate limiting on sensitive endpoints (login, password reset)
- Account lockout after failed login attempts
- Unique test access tokens for secure test-taking
- IP address and user agent tracking for audit

**Permission Hierarchy:**
```
ADMIN (highest privileges)
  ├─ Can access all organizations
  ├─ Can view all users and data
  └─ System-wide management

HR_MANAGER
  ├─ Organization-specific access
  ├─ Can manage own organization's vacancies and tests
  ├─ Can review applications for own vacancies
  └─ Limited to assigned organization

USER (Candidate)
  ├─ Can manage own profile
  ├─ Can apply to vacancies
  ├─ Can take assigned tests
  └─ Read-only access to public data

GUEST
  └─ Read-only access to public vacancies

5. Database Schema
5.1 Tables and Their Purpose

Users | Purpose: Store user accounts with authentication data (email, hashed password, role, OAuth IDs)
UserDetails | Purpose: Store extended user profile information (phone, bio, gender, profile picture, CV)
Education | Purpose: Store user education records (school, degree, years)
Experience | Purpose: Store user work experience records (company, role, years)
Organizations | Purpose: Store organization information (name, address, type, contact info, branding)
HRManagers | Purpose: Link users to organizations as HR managers (position, department, permissions)
Vacancies | Purpose: Store job vacancy postings (title, description, requirements, employment type, status)
Applications | Purpose: Store candidate applications to vacancies (status, cover letter, review notes)
ApplicationTimeline | Purpose: Track application status change history (who changed, when, notes)
SavedVacancies | Purpose: Store user's bookmarked/saved vacancies (user, vacancy, notes)
VacancyViews | Purpose: Track vacancy views for analytics (user, IP address, timestamp)
Tests | Purpose: Store test definitions (title, duration, passing score, difficulty)
Questions | Purpose: Store test questions (text, type, points, time limit)
QuestionOptions | Purpose: Store answer options for multiple choice questions (text, is correct)
TestAssignments | Purpose: Assign tests to applications (access token, expiration, status)
TestSubmissions | Purpose: Store completed test submissions (scores, pass/fail, timestamps)
SubmissionAnswers | Purpose: Store individual question answers (selected option, text, points awarded)
Notifications | Purpose: Store user notifications (type, message, read status, action URL)
Messages | Purpose: Store internal messages between users (subject, body, thread support)
EmailVerificationTokens | Purpose: Store email verification tokens (token, expiration, used status)
PasswordResetTokens | Purpose: Store password reset tokens (token, expiration, used status)
RefreshTokens | Purpose: Store JWT refresh tokens (token, expiration, revoked status)
AuditLogs | Purpose: System-wide audit trail (action, entity, old/new values, IP address)

5.2 Indexes and Optimization
Users Table:

Email (Unique Index) - Fast user lookup by email, ensure uniqueness
Role (Non-clustered Index) - Filter users by role
(IsVerified, CreatedAt) - Find unverified accounts

Applications Table:

(VacancyId, UserId) - Composite Index - Prevent duplicate applications, fast lookup
(Status, AppliedAt) - Composite Index - Filter and sort by status and date
ReviewedBy (Non-clustered Index) - Find applications reviewed by HR manager

Vacancies Table:

(Status, PublishedAt) - Composite Index - Filter active vacancies by publish date
(Profession, Industry) - Composite Index - Multi-criteria search
OrganizationId (Non-clustered Index) - Find organization's vacancies
ApplicationDeadline (Non-clustered Index) - Find expiring vacancies

TestSubmissions Table:

TestAssignmentId (Unique Index) - Ensure one submission per assignment
(UserId, TestId) - Composite Index - Find user's test history
IsPassed (Non-clustered Index) - Filter passed/failed tests
SubmittedAt (Non-clustered Index) - Sort by completion date

Notifications Table:

(UserId, IsRead, CreatedAt) - Composite Index - Efficient unread notifications query
RelatedEntityId (Non-clustered Index) - Link to related entities

Messages Table:
Messages Table:

(SenderId, ReceiverId) - Composite Index - Find conversations between users
(ReceiverId, IsRead) - Composite Index - Efficient unread messages query
ApplicationId (Non-clustered Index) - Find messages related to specific application
ParentMessageId (Non-clustered Index) - Thread navigation
SentAt (Non-clustered Index) - Sort messages chronologically

VacancyViews Table:

VacancyId (Non-clustered Index) - Aggregate views per vacancy
(VacancyId, ViewedAt) - Composite Index - Time-series analytics
UserId (Non-clustered Index) - Track user view history

AuditLogs Table:

(EntityType, EntityId) - Composite Index - Find all changes to specific entity
UserId (Non-clustered Index) - Track user actions
CreatedAt (Non-clustered Index) - Time-based filtering
Action (Non-clustered Index) - Filter by action type

Tests Table:

OrganizationId (Non-clustered Index) - Find organization's tests
(Profession, Industry) - Composite Index - Find relevant tests
IsActive (Non-clustered Index) - Filter active tests

Questions Table:

(TestId, OrderNumber) - Composite Index - Retrieve ordered questions efficiently
TestId (Non-clustered Index) - Find all questions for a test

TestAssignments Table:

ApplicationId (Non-clustered Index) - Find assignments for application
AccessToken (Unique Index) - Secure test access validation
(Status, ExpiresAt) - Composite Index - Find expired/active assignments

RefreshTokens Table:

Token (Unique Index) - Fast token validation
(UserId, IsRevoked) - Composite Index - Find active tokens for user
ExpiresAt (Non-clustered Index) - Cleanup expired tokens

EmailVerificationTokens & PasswordResetTokens:

Token (Unique Index) - Fast token validation
(UserId, IsUsed) - Composite Index - Find unused tokens for user
ExpiresAt (Non-clustered Index) - Cleanup expired tokens

Performance Optimization Strategies:

Eager Loading: Use .Include() for frequently accessed navigation properties
Pagination: All list queries implement pagination (default 10, max 100 items per page)
Async Operations: All database operations use async/await pattern
Query Projections: Use DTOs and .Select() to return only needed fields
Caching: Implement Redis caching for frequently accessed read-only data (industries, professions)
Database Cleanup: Scheduled jobs to delete expired tokens and old audit logs
Connection Pooling: Configure SQL Server connection pooling for optimal performance


6. Additional Functionality
6.1 Integrations
Email Service (Required):

Provider: SendGrid or SMTP
Purpose:

Welcome emails with verification links
Email verification confirmations
Password reset instructions
Application status change notifications
Test assignment notifications
Test result notifications
Deadline reminders
New message alerts


Features:

HTML email templates with organization branding
Personalization with user data
Delivery tracking and analytics
Unsubscribe management
Bounce and spam complaint handling



File Storage Service (Required):

Provider: Azure Blob Storage, AWS S3, or Local File System
Purpose:

Store user profile pictures (max 5MB, JPG/PNG/WebP)
Store user CVs/resumes (max 10MB, PDF/DOCX)
Store organization logos (max 2MB, JPG/PNG/SVG)
Store test answer file uploads (max 25MB, various formats)


Features:

Unique file naming (UUID-based)
CDN integration for fast delivery
Signed URLs for secure private file access
Automatic cleanup of orphaned files
Image optimization and resizing
Virus scanning for uploaded files



OAuth Providers (Optional):

Google OAuth 2.0:

Quick registration and login
Profile data auto-fill (name, email, profile picture)
Secure authentication without password management


Apple Sign In:

iOS/macOS user convenience
Privacy-focused authentication
Email relay option support



PDF Generation (Optional):

Library: iTextSharp or DinkToPdf
Purpose:

Generate test result reports
Create application summary PDFs
Export vacancy details
Generate recruitment analytics reports


Features:

Professional formatting with charts
Organization branding (logo, colors)
Digital signatures for authenticity



Analytics Service (Optional):

Provider: Google Analytics or Application Insights
Purpose:

Track user behavior and engagement
Monitor vacancy view patterns
Analyze application conversion rates
Identify popular positions and industries
System performance monitoring



SMS Service (Future):

Provider: Twilio or AWS SNS
Purpose:

Two-factor authentication (2FA)
Critical notification alerts (test deadlines, interviews)
OTP for password reset



Video Conference Integration (Future):

Provider: Zoom, Microsoft Teams, or Google Meet
Purpose:

Schedule and conduct video interviews
Record interview sessions
Screen sharing for technical assessments




9. Project Unique Characteristics
9.1 How Does Your Project Differ from Similar Existing Solutions?
1. Universal Multi-Industry Platform:

Unlike specialized platforms (LinkedIn for professionals, Indeed for general jobs), TalentBridge is designed to serve ALL industries equally - from schools hiring teachers to hospitals recruiting doctors to tech companies finding developers
Flexible Organization<TExactType> model allows industry-specific customization while maintaining a unified platform
No industry bias - equal features and importance for education, healthcare, IT, government, etc.

2. Comprehensive Testing System:

Goes beyond simple resume screening with 6 distinct question types (MultipleChoice, TrueFalse, ShortAnswer, Essay, Coding, FileUpload)
Intelligent hybrid grading: automatic grading for objective questions + manual review for subjective answers
Reusable test library: Organizations can create test banks and assign them to multiple vacancies
Per-question time limits and detailed performance analytics
Most platforms offer only basic questionnaires or no testing at all

3. Complete Audit Trail and Transparency:

Every application status change tracked in ApplicationTimeline with timestamps and responsible party
Comprehensive AuditLog for system-wide operations (GDPR compliance ready)
VacancyView tracking for recruitment analytics
Candidates can see complete history of their application journey
Organizations get transparency reports for compliance and fairness

4. Georgian Market Focus:

Built specifically for the Georgian job market with local needs in mind
GEL currency support as default
Future: Full Georgian language localization
Understanding of local industries (schools, clinics, government agencies)
Compliance with Georgian labor laws and regulations

5. Advanced Profile Management:

Separate entities for Education and Experience (not just text fields)
Dynamic skills array stored as JSON
Multiple education degrees and work positions supported
CV/resume file storage with version control
Profile completeness tracking for better matching

6. Integrated Communication System:

Built-in messaging with threading (no need for external email back-and-forth)
Application-specific conversations (contextual communication)
10+ notification types with deep linking (ActionUrl for direct navigation)
Real-time notifications for immediate engagement

7. Secure Test-Taking Experience:

Unique access tokens per assignment (prevents sharing)
Test expiration and deadline enforcement
Time tracking per question for behavioral analysis
Auto-save functionality (no lost progress)
Support for file uploads in test answers (portfolios, code samples)

8. Flexible Organization Model:

Generic Organization<TExactType> allows type-specific extensions
Multiple HR managers per organization (role delegation)
Department-level permissions for large organizations
Organization branding and customization (logo, description)

9. Smart Status Workflows:

9 application statuses (Pending, UnderReview, TestAssigned, TestCompleted, Interviewed, ShortListed, Accepted, Rejected, Withdrawn)
Vacancy lifecycle (Draft → Active → Closed/OnHold/Expired)
Test assignment states (Assigned → InProgress → Completed/Expired/Cancelled)
Prevents invalid state transitions

10. Privacy and Security First:

JWT with refresh token rotation (prevents token theft)
Email verification required (reduces fake accounts)
Password reset with secure tokens (time-limited, one-time use)
Role-based access control with resource ownership validation
Optional anonymous vacancy viewing for privacy
GDPR-ready data export and deletion capabilities

11. Analytics-Driven Insights:

Vacancy performance metrics (views, view-to-application ratio)
Test difficulty analysis (question-level statistics)
Time-to-hire tracking
Candidate pipeline visualization
Industry and profession trends
Exportable reports (CSV/PDF)

12. Developer-Friendly Architecture:

Clean folder structure (Models organized by domain)
Comprehensive enum system (type safety)
RESTful API design following best practices
Swagger/OpenAPI documentation
DTO pattern for API responses
Repository pattern for data access

9.2 Innovative Features
1. AI-Powered Candidate Ranking (Future):

Machine learning algorithms analyze candidate profiles against job requirements
Scoring based on education match, experience relevance, skills alignment
Automatic shortlisting of top candidates
Bias reduction through objective data analysis
Continuous learning from hiring outcomes

2. Profession-Specific Test Library (Future):

Pre-built test templates for common positions (Software Developer, Teacher, Nurse, Accountant)
Community-contributed test questions
Industry-standard assessments (coding challenges, teaching scenarios)
Multilingual test support
Regular updates based on industry trends

3. Smart Candidate-Vacancy Matching (Future):

Automatic matching algorithm suggests relevant vacancies to candidates
Considers education, experience, skills, location preferences, salary expectations
Reverse matching: Suggests qualified candidates to HR managers
Email notifications for new matching opportunities
Improves discovery and reduces missed opportunities

4. Video Interview Integration (Future):

Schedule video interviews directly in platform
One-click join with no external accounts needed
Record interviews for review and compliance
Screen sharing for technical assessments
Collaborative interview notes among HR team
Candidate rating system post-interview

5. Blockchain-Based Credential Verification (Future):

Educational credentials verified on blockchain
Work experience validation through employer signatures
Immutable proof of qualifications
Reduces resume fraud
Instant verification for HR managers

6. Adaptive Testing:

Test difficulty adjusts based on candidate performance
More efficient assessment in less time
Better differentiation between candidate skill levels
Reduced test fatigue

7. Collaborative Hiring:

Multiple HR managers can review same application
Voting system for candidate selection
Discussion threads on applications
Consensus-based decision making
Interview panel scheduling and coordination

8. Candidate Pool Management:

Save promising candidates for future positions
Tag candidates with skills and interests
Reach out to past candidates for new vacancies
Build talent pipeline proactively

9. Remote Work First:

IsRemote flag for vacancies
Remote-specific filters in search
Time zone consideration
Virtual onboarding support

10. Customizable Workflows:

Organizations can define custom application stages
Automated actions based on status changes
Custom email templates per organization
Configurable notification preferences

11. Gamification (Future):

Points for profile completion
Badges for skills verification
Leaderboards for popular candidates
Achievements for successful applications
Increases engagement and profile quality

12. Integration Marketplace (Future):

Connect with HR tools (BambooHR, Workday)
Background check service integration
Salary benchmarking tools
Employee onboarding systems
Calendar and scheduling apps
Open API for third-party developers


10. Development Plan
10.1 Development Timeline (Estimated)
Phase 1: Foundation (Weeks 1-3)

Setup project structure (Backend API + Angular Frontend)
Configure database and Entity Framework migrations
Implement authentication system (JWT, registration, login)
Create base models and repositories
Setup email service integration

Phase 2: Core Features (Weeks 4-7)

User profile management (UserDetails, Education, Experience)
Organization and HR Manager setup
Vacancy CRUD operations
Application submission and tracking
Basic search and filtering

Phase 3: Testing System (Weeks 8-10)

Test creation and management
Question types implementation
Test assignment workflow
Test-taking interface with timer
Auto-grading and manual review

Phase 4: Communication & Notifications (Weeks 11-12)

Messaging system with threading
Notification system with multiple types
Email notification integration
Real-time updates (optional: SignalR)

Phase 5: Analytics & Reporting (Weeks 13-14)

Dashboard for candidates and HR managers
Vacancy and application statistics
Test performance analytics
Report generation and export

Phase 6: Polish & Testing (Weeks 15-16)

UI/UX refinement
Performance optimization
Security hardening
Unit and integration testing
Bug fixes and improvements
Documentation completion

10.2 Potential Challenges and Risks
1. Test Security and Cheating Prevention:

Risk: Candidates may attempt to cheat during online tests (screen sharing, external help)
Mitigation: Implement unique access tokens, time limits, question randomization, consider browser lockdown features or proctoring in future versions

2. Scalability for Large Organizations:

Risk: System may slow down with thousands of applications and tests
Mitigation: Implement proper indexing, pagination, caching (Redis), database optimization, consider microservices architecture for future scaling

3. Data Privacy and GDPR Compliance:

Risk: Handling sensitive personal data (CVs, test results) requires strict compliance
Mitigation: Implement data encryption, audit logging, user consent management, data export/deletion features, consult legal experts

4. File Storage Costs:

Risk: Storing thousands of CVs, profile pictures, and test files can be expensive
Mitigation: Implement file size limits, compression, lifecycle policies to archive old files, consider cost-effective storage solutions

5. Email Deliverability:

Risk: Important emails (verification, notifications) may end up in spam folders
Mitigation: Use reputable email service (SendGrid), implement SPF/DKIM/DMARC, avoid spammy content, monitor bounce rates

6. Complex Test Grading Logic:

Risk: Manual grading workflow may create bottlenecks for HR managers
Mitigation: Provide clear UI for grading, allow bulk operations, implement grading templates, consider AI-assisted grading for essays

7. Browser Compatibility:

Risk: Angular application may not work consistently across all browsers
Mitigation: Use Angular's browser support, test on Chrome, Firefox, Safari, Edge, implement polyfills where needed

8. Concurrent Test Taking:

Risk: Multiple candidates taking tests simultaneously may cause race conditions
Mitigation: Use database transactions, optimistic concurrency, proper locking mechanisms

9. Mobile Responsiveness:

Risk: Complex UI (test taking, application forms) may not work well on mobile
Mitigation: Use Bootstrap responsive grid, test on various screen sizes, consider mobile-first design

10. User Adoption:

Risk: Organizations may resist switching from traditional recruitment methods
Mitigation: Provide excellent onboarding, video tutorials, customer support, free trial period, demonstrate ROI (time/cost savings)

11. Search Performance:

Risk: Full-text search on large vacancy/candidate datasets may be slow
Mitigation: Implement Elasticsearch for advanced search, optimize SQL queries, use proper indexes, consider search result caching

12. Real-Time Features:

Risk: Implementing real-time notifications and messaging can be complex
Mitigation: Start with polling, upgrade to SignalR if needed, ensure proper connection management

10.3 Future Development (After Project Completion)
Short-Term (3-6 months):

Mobile Applications: Native iOS and Android apps for better candidate experience
Advanced Search: Elasticsearch integration for instant, relevant search results
Video Interviews: Integrated video calling for remote interviews (Zoom/Teams API)
Enhanced Analytics: Advanced charts, trend analysis, predictive hiring metrics
Calendar Integration: Google Calendar/Outlook sync for interview scheduling
Bulk Operations: HR managers can process multiple applications at once
Email Templates: Customizable email templates per organization
Multi-Language Support: i18n for Georgian, English, and other languages

Mid-Term (6-12 months):

AI Candidate Matching: ML algorithms for automatic candidate-vacancy matching
Automated Screening: AI-powered initial resume screening
Sentiment Analysis: Analyze cover letters and essays for soft skills
Chatbot Assistant: Answer candidate questions, guide through application process
Blockchain Verification: Credential verification on blockchain
Advanced Testing: Adaptive tests, coding environment, video responses
Background Checks: Integration with background check services
Offer Management: Digital offer letters, e-signatures, onboarding workflow
Candidate Portal: Enhanced portal with application tracking, interview prep

Long-Term (12+ months):

Marketplace: Third-party test providers, assessment tools, HR services
Public API: RESTful API for third-party integrations
White-Label Solution: Customizable platform for large enterprises
Predictive Analytics: Predict candidate success, turnover risk, time-to-hire
Gamification: Points, badges, leaderboards for engagement
Social Features: Professional networking, employee referrals
Learning Management: Skill development courses for candidates
Performance Management: Post-hire performance tracking integration
Compliance Tools: EEOC reporting, diversity metrics, audit trails
Global Expansion: Multi-currency, multi-timezone, localization for international markets

Technology Upgrades:

Migrate to microservices architecture for better scalability
Implement event-driven architecture with message queues
Add GraphQL API alongside REST
Implement Progressive Web App (PWA) features
Use machine learning for all predictive features
Real-time collaboration features with WebRTC
Advanced security: Two-factor authentication, biometric login
Performance: Edge computing, global CDN, advanced caching


11. Additional Notes
Project Vision:
TalentBridge aims to become the leading HR management and recruitment platform in Georgia, eventually expanding to neighboring countries and global markets. The platform's universal design makes it suitable for any industry, from small businesses to large government agencies.
Success Criteria:

Technical: System handles 10,000+ concurrent users, API response time < 500ms, 99.9% uptime
Business: 100+ organizations registered in first 6 months, 10,000+ candidates, 1,000+ vacancies posted
User Satisfaction: Net Promoter Score (NPS) > 50, average user rating > 4.5/5

Key Differentiators:

Only Georgian platform serving ALL industries equally
Most comprehensive testing system with 6 question types
Complete transparency with audit trails
Built-in communication eliminating external email
Smart automation reducing HR manager workload by 40%+

Technical Excellence:

Clean architecture with separation of concerns
Comprehensive test coverage (unit, integration, E2E)
Well-documented API with Swagger
Security-first approach (OWASP guidelines)
Performance-optimized with caching and indexing
Scalable infrastructure ready for growth

Social Impact:

Increases employment opportunities through better job matching
Reduces hiring bias through objective testing
Provides equal access to job market for all candidates
Supports small businesses with affordable HR tools
Promotes transparency in government hiring
Helps educational institutions find qualified teachers

Sustainability:

Freemium model: Free for small organizations (1-5 vacancies), paid plans for larger
Tiered pricing based on organization size and features needed
Revenue from premium features (advanced analytics, AI matching, video interviews)
Potential partnerships with job boards and recruitment agencies
Future: Marketplace revenue from third-party integrations

Team & Resources:

Full-stack developer(s) with .NET and Angular expertise
UI/UX designer for intuitive interface design
QA engineer for comprehensive testing
DevOps engineer for deployment and monitoring
Optional: Data scientist for future AI features

Development Environment:

Version Control: Git with GitHub/GitLab
IDE: Visual Studio 2022 (Backend), VS Code (Frontend)
Database Tools: SQL Server Management Studio, Azure Data Studio
API Testing: Postman, Swagger UI
Project Management: Jira, Trello, or Azure DevOps
Documentation: Markdown, Swagger, Wiki

Deployment Strategy:

Development: Local machines with SQL Server LocalDB
Staging: Azure App Service + Azure SQL Database (or AWS equivalents)
Production: Azure App Service with auto-scaling, Azure SQL Database with geo-replication
CI/CD: GitHub Actions or Azure DevOps Pipelines
Monitoring: Application Insights, Azure Monitor, or CloudWatch

Open Source Considerations:

Core platform could be open-sourced for community contributions
Benefit from developer community bug fixes and features
Builds trust and transparency
Consider dual licensing (open source + commercial)

Compliance & Legal:

Terms of Service and Privacy Policy required
GDPR compliance for data protection
Georgian labor law compliance
Accessibility standards (WCAG 2.1)
Regular security audits
Data retention policies

Project Success Indicators:

Clean, maintainable codebase
Comprehensive documentation
Positive user feedback
Working prototype with core features
Successful demo to stakeholders
Foundation for future growth


Project Repository: https://github.com/[username]/talentbridge
API Documentation: https://api.talentbridge.ge/swagger
Live Demo: https://demo.talentbridge.ge
Wiki: https://github.com/[username]/talentbridge/wiki
Contact Information:

Project Email: dev@talentbridge.ge
Support: support@talentbridge.ge
Website: www.talentbridge.ge


Document Version: 1.0
Last Updated: [Current Date]
Document Author: [Student Name]
Project Status: In Development
Estimated Completion: [Date]

Declaration:
I declare that this project documentation represents my original work and understanding of the TalentBridge platform. The technical implementation follows industry best practices and academic standards for full-stack web development.
Student Signature: _______________________
Date: _______________________
