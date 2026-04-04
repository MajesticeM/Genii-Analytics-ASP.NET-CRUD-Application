# Genii Analytics Assessment – Inventory & Invoicing System

## Overview

This project is a simple inventory and invoicing system built as part of a practical assessment.

The goal of the project is to demonstrate:

* Clean code structure
* Proper use of GitHub (issues, commits, pull requests)
* Role-based security
* Full-stack development skills using ASP.NET MVC

The system allows users to manage products and create invoices with multiple items.

---

## Tech Stack

* ASP.NET MVC (.NET Framework 4.7.2)
* C#
* Entity Framework 6 (Code First)
* SQL Server
* LINQ
* JavaScript / jQuery

---

## Features Implemented

### Authentication & Roles

* Secure login system using ASP.NET Identity
* Role-based access control
* Roles:

  * Admin
  * User
  * Manager

### Default Login (Seeded)

```
Email: admin@geniiassessment.local
Password: Admin@12345

NB: Once you login, admin can create a normal user and manager to view functionality of other roles.  
```

---

### User Management (Admin + Manager)

* Create users
* Edit users
* Delete users
* Assign roles
* Activate / deactivate users

---

### Product Management

* Create products
* Edit products
* Delete products
* Track:

  * Name
  * Cost per item
  * Quantity in stock

---

### Invoice Management (User Invoice CRUD)

Users can:

* Create invoices
* Edit their own invoices
* View their own invoices
* Add multiple items per invoice

Invoice includes:

* Invoice date
* User who created it
* Total amount

---

### Client-Side Invoice Calculation

* Line totals update instantly
* Invoice total updates automatically
* No server communication required (as per requirement)

---

## Project Structure

* **Controllers** → Handle requests
* **Models** → Database entities
* **ViewModels** → UI-specific models
* **Views** → Razor pages
* **Data (DbContext)** → Database access
* **Migrations** → Code-first database setup

---

## How to Run the Project

### 1. Clone Repository

```
git clone https://github.com/MajesticeM/Genii-Analytics-ASP.NET-CRUD-Application.git
```

### 2. Open in Visual Studio

* Use Visual Studio 2022
* Ensure .NET Framework 4.7.2+ is installed

---

### 3. Database Setup

This project uses **SQL Server LocalDB / SQL Server Object Explorer**



Run:

```
Update-Database
```

This will:

* Create the database
* Create Identity tables
* Seed roles and admin user

---

### 4. Run the Application

Press:

```
F5
```

Navigate to:

```
/Account/Login
```

Login using admin credentials above.

---

## Roles & Permissions

| Role    | Access                                         |
| ------- | ---------------------------------------------- |
| Admin   | Full system access (users, products, invoices) |
| User    | Create and manage own invoices                 |
| Manager | Manage users + (future: reporting & oversight) |

---

## GitHub Workflow (Important for Assessment)

This project follows a structured workflow:

* Feature branches (`feature/...`)
* GitHub Issues for task tracking
* Pull Requests for merging
* Self-review before merge
* Incremental commits

### Example Branches

* `feature-auth-and-roles`
* `feature-user-management`
* `feature-product-crud`
* `feature-user-invoice-crud`

---

## Kanban Workflow

Backlog → To Do → In Progress → Review → Done

---

## Key Technical Decisions

### 1. Client-Side Invoice Totals

Invoice totals are calculated using JavaScript to:

* Improve performance
* Meet requirement (no server calls)

---

### 2. Entity Framework Code First

* Database created from models
* Easy migrations
* Clean version control

---

### 3. Role-Based Authorization

* `[Authorize(Roles = "...")]` used
* Ensures correct access control

---

## Work Left as Tickets (Important)

To reflect real-world development and project planning, some features were intentionally **not fully implemented** and tracked as GitHub issues.

### Pending Features (Tickets)

* Manager can view and edit all invoices
* Reporting dashboard:

  * Items sold per product
  * Total products
  * Stock vs sold
* Restock notification system
* Automatic stock deduction after invoice creation
* Auto-fill product price on selection
* Export reports (CSV / PDF)
* Audit logging
* UI/UX improvements
* Automated tests
* Extended API documentation

---

## Basic API / Controller Notes

Controllers follow standard MVC patterns:

* `GET /Products`
* `POST /Invoices/Create`
* `GET /Invoices`

Comments are included for key methods to support basic documentation.

---

## Testing

Manual testing completed for:

* Login & role access
* Product CRUD
* Invoice creation and editing
* Client-side calculations
* Cloning and running of the repositiory


