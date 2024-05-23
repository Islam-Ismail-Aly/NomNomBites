# NomNomBites


## Overview
**NomNomBites** is a comprehensive food ordering application designed to streamline the process of ordering food online and managing the system through an admin dashboard. This project leverages modern technologies and architectural patterns to ensure a robust, scalable, and maintainable codebase.


## Features
- **Food Ordering System**
  - Browse and order food items
  - Manage cart and checkout process
- **Customer Management**
  - Maintain detailed records of customers
- **Admin Dashboard**
  - Manage food items, categories, and orders
  - View metrics and statistics through charts and counts
  - DataTables jQuery plugin for enhanced table functionality (searching, pagination)
- **Authentication and Authorization**
  - Implemented using Microsoft Identity
- **Data Mapping and Validation**
  - AutoMapper for object-to-object mapping
- **Design Patterns**
  - Repository Pattern for data access
  - Unit of Work for managing transactions
  - MVC (Model-View-Controller) pattern for organizing the application
- **Front-end Integration**
  - jQuery for DOM manipulation and AJAX calls


## Technologies Used
- **Backend**: .NET Core, MS Identity, AutoMapper
- **Frontend**: HTML, Bootstrap, jQuery, DataTables jQuery
- **Database**: SQL Server
- **Architecture**: N-Tier Architecture, Code First Approach

## Project Structure

```plaintext
NomNomBites/
│   ├── Core/
│   │   └── NomNomBites.Core/
│   │       ├── Controllers
│   │       ├── Models
│   │       ├── Repositories
│   │       ├── Services
│   │       └── ...
│   ├── Infrastructure/
│   │   └── NomNomBites.Infrastructure/
│   │       ├── Data
│   │       ├── Migrations
│   │       ├── Repositories
│   │       └── ...
│   ├── Web/
│   │   └── NomNomBites.Web/
│   │       ├── wwwroot
│   │       ├── Controllers
│   │       ├── Views
│   │       ├── wwwroot
│   │       └── ...
