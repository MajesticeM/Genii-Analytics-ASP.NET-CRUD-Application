Project Overview

This project is a small inventory and invoicing management system built as part of a practical assessment.
The goal of the system is to demonstrate full-stack development skills, clean architecture, and structured project management using GitHub.

The application allows users to:

Manage products and stock levels
Create and manage invoices
Track product sales
Generate reports
Receive restock notifications

The project was intentionally managed using GitHub Issues, Kanban board, feature branches, and pull requests to simulate a real-world team environment.


Features

Authentication & Authorization
Secure login system
Role-based access control
Three roles:
Admin
User
Manager

User Management (Admin)
Create users
Edit users
Assign roles
Activate/deactivate users

Product Management
Create, edit, and manage products
Track:
Product name
Cost per item
Quantity in stock
Restock threshold

Invoice Management
Create invoices
Edit invoices
Add multiple products to invoices
Automatic total calculation (client-side, no server calls)
Track:
Invoice date
User who created the invoice
Total amount
Manager Dashboard & Reporting
Managers have access to:

All invoices (view and edit)
Reports including:
Number of items sold per product
Total number of products
Total products sold
Products in stock
Stock vs sold comparison

Restock Notification
Managers receive a single consolidated notification
Displays all products below restock threshold
Triggered dynamically from product stock levels

Tech Stack
Backend: ASP.NET MVC (.NET Framework 4.7.2+)
Language: C#
Frontend: HTML, CSS, JavaScript (jQuery)
ORM: Entity Framework 6 (Code First)
Database: SQL Server
Querying: LINQ

Architecture & Structure
The application follows a clean and maintainable structure:

Controllers → Handle HTTP requests
Models → Domain entities
ViewModels → UI-specific data structures
Data → DbContext and database configuration
Migrations → Code-first migrations
Views → Razor views
Scripts → JavaScript logic (invoice calculations)

Roles & Permissions
Role	Permissions
Admin	Full user management, product management
User	Create and manage own invoices
Manager	View/edit all invoices, access reports, view restock alerts
⚙️ Setup Instructions
1. Clone the Repository
git clone https://github.com/your-username/your-repo.git
2. Open Solution
Open in Visual Studio 2022
Ensure .NET Framework 4.7.2+ is installed
3. Configure Database
Update your connection string in:

Web.config
4. Run Migrations
Open Package Manager Console:

Enable-Migrations
Add-Migration InitialCreate
Update-Database
5. Run the Application
Press F5 or click Run
Navigate to the login page

Key Technical Decisions
Client-Side Invoice Calculation

Invoice totals are calculated using JavaScript (client-side) to:

Improve responsiveness
Avoid unnecessary server calls
Meet assessment requirement
Entity Framework Code-First
Database schema is generated from models
Enables easy migrations and version control
Role-Based Authorization
Implemented using [Authorize] attributes
Ensures proper access control across the system
Restock Notification Design
Implemented as a single aggregated alert
Avoids notification spam
Provides clear manager visibility

Project Management Approach
This project was managed using:

GitHub Issues (task breakdown)
GitHub Kanban Board (progress tracking)
Feature Branching Strategy
Pull Requests with self-review
Incremental commits over time

Kanban Workflow
Backlog → Ready → In Progress → In Review → Done

Branching Strategy
main → stable branch
feature/* → feature development
docs/* → documentation updates

Example Features Implemented via Issues
Authentication & Roles
User CRUD
Product CRUD
Invoice Management
Reporting Dashboard
Restock Notification

Known Limitations / Future Improvements
The following enhancements were intentionally scoped out and tracked as issues:
Export reports to CSV/PDF
Add audit logging for invoice changes
Improve UI/UX responsiveness
Add automated unit/integration tests
Advanced reporting filters
API documentation expansion

API Notes (Basic)
Basic controller actions follow standard MVC patterns:

GET /Products
POST /Invoices/Create
GET /Reports

Further API documentation can be extended in future iterations.

Testing Notes
Manual testing performed for:
Invoice calculations
Role-based access restrictions
CRUD operations
Reporting accuracy

Future work includes adding automated test coverage.

Assumptions
Invoice totals are trusted from client input but validated server-side
Stock levels are manually managed (no automatic deduction logic implemented)
Restock threshold is configurable per product


Author
Mashudu Ralephata
Senior Full Stack Developer

Submission
This repository was created as part of a practical assessment and submitted for evaluation.
