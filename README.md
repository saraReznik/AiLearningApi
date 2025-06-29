# AI Driven Learning Platform (Mini MVP)

This repository contains a mini AI-driven learning platform, serving as a Minimum Viable Product (MVP). It includes a .NET Core Web API backend and a React.js frontend, demonstrating full-stack development skills, API integration, and modular code organization.

## Table of Contents

- [Project Objective](#project-objective)
- [Features](#features)
- [Technical Stack](#technical-stack)
  - [Backend](#backend)
  - [Frontend](#frontend)
- [Project Structure](#project-structure)
- [Setup and Installation](#setup-and-installation)
  - [Prerequisites](#prerequisites)
  - [1. Clone the Repository (with Submodules)](#1-clone-the-repository-with-submodules)
  - [2. Backend Setup (.NET API)](#2-backend-setup-net-api)
  - [3. Frontend Setup (React App)](#3-frontend-setup-react-app)
- [Running the Application](#running-the-application)
  - [Running the Backend API](#running-the-backend-api)
  - [Running the Frontend React App](#running-the-frontend-react-app)
- [API Endpoints (Swagger UI)](#api-endpoints-swagger-ui)
- [Assumptions and Design Choices](#assumptions-and-design-choices)
- [Contact](#contact)

---

## Project Objective

The main objective of this project is to create a mini learning platform where users can select learning topics (by category and sub-category), send prompts to an AI (simulated), receive generated lessons, and view their learning history. An admin dashboard is also included for user and prompt management.

## Features

**User Features:**
- User Registration
- User Login (Authentication via Email and Phone)
- Selecting learning Categories and Sub-Categories
- Submitting a prompt to the AI to receive a lesson-like response
- Viewing personal learning history (all past prompts and responses)

**Admin Features:**
- View a list of all registered users
- View the prompt history for any selected user

## Technical Stack

### Backend

- **Language/Framework:** .NET Core Web API (C#)
- **Database:** (Assumed: SQL-based, e.g., PostgreSQL/MySQL, managed via Entity Framework Core Migrations)
- **API Integration:** OpenAI GPT API (simulated/mocked responses are assumed if not fully integrated)
- **Authentication:** JWT (JSON Web Tokens) for API security

### Frontend

- **Framework:** React.js
- **State Management:** React Hooks (useState, useEffect) and Context API (for Auth)
- **Routing:** React Router DOM
- **API Communication:** Fetch API (directly, or Axios if re-implemented)
- **UI Framework:** Material-UI (MUI) for a professional and modern look.

## Project Structure

The repository is structured as a monorepo, containing both frontend and backend applications:

.
├── backend/                  # .NET Core Web API project (managed as a Git Submodule)
│   ├── Server.sln            # Solution file
│   ├── Server/               # Main ASP.NET Core project
│   │   ├── Controllers/      # API controllers (Auth, User, Category, Prompt)
│   │   ├── Properties/       # launchSettings.json for running the app
│   │   └── Startup.cs        # Application startup and configuration (CORS, Auth)
│   └── BL/                   # Business Logic Layer
│   └── Dal/                  # Data Access Layer
│   └── ... (other backend files)
└── frontend/                 # React.js application
├── public/               # Static assets
├── src/
│   ├── api/              # API service configurations (axiosInstance.js)
│   ├── components/       # Reusable UI components (Auth, Dashboard, Admin)
│   ├── context/          # React Contexts (AuthContext.jsx)
│   ├── pages/            # Main application pages (Login, Register, Dashboard, Admin)
│   └── App.jsx           # Main application component and routing
├── package.json          # Frontend dependencies
└── ... (other frontend files)


## Setup and Installation

Follow these steps to set up and run the project locally.

### Prerequisites

-   [.NET SDK](https://dotnet.microsoft.com/download) (Version compatible with your backend, e.g., .NET 6.0 or 7.0 SDK)
-   [Node.js](https://nodejs.org/en/download/) (LTS version recommended) & npm (Node Package Manager, comes with Node.js)
-   A running database instance (e.g., PostgreSQL, MySQL, MongoDB) and configured `ConnectionStrings` in `appsettings.json` of your backend project.
-   [Git](https://git-scm.com/downloads)

### 1. Clone the Repository (with Submodules)

Since the backend is included as a Git submodule, you need to clone the repository with the `--recursive` flag to get both the main project and the backend code:

```bash
git clone --recursive YOUR_MAIN_REPO_URL # Replace with the URL of THIS repository
cd YOUR_MAIN_REPO_FOLDER_NAME          # Navigate into the cloned project folder
(If you already cloned without --recursive, run git submodule update --init --recursive inside the cloned folder)

2. Backend Setup (.NET API)
Navigate to the backend project folder:

Bash

cd backend/Server # Adjust "backend/Server" if your backend project path is different
(This is the folder containing Server.csproj and Startup.cs).

Restore NuGet packages:

Bash

dotnet restore
Configure Database Connection:

Open appsettings.json (and appsettings.Development.json) inside the Server project folder.

Update the DefaultConnection string under ConnectionStrings to point to your local database instance.

Example (PostgreSQL): "DefaultConnection": "Host=localhost;Port=5432;Database=ai_learning_db;Username=your_user;Password=your_password;"

Apply Database Migrations (if using Entity Framework Core):

Ensure your database is running.

Run migrations to create the database schema:

Bash

dotnet ef database update
(If you encounter issues here, ensure your DbContext and migrations are correctly set up in the Dal layer of your backend.)

Configure JWT Settings:

In appsettings.json, locate the "Jwt" section.

Ensure Key, Issuer, and Audience are properly set.

Example:

JSON

"Jwt": {
  "Key": "THIS_IS_A_VERY_STRONG_SECRET_KEY_FOR_JWT_AUTHENTICATION_AND_SHOULD_BE_KEPT_CONFIDENTIAL_AND_LONG_ENOUGH",
  "Issuer": "https://localhost:7120",
  "Audience": "https://localhost:5173"
}
(The Key should be a long, random string. The Issuer should be your backend URL, and Audience your frontend URL.)

Trust HTTPS Development Certificate:

If running HTTPS, ensure your .NET development certificate is trusted:

Bash

dotnet dev-certs https --trust
3. Frontend Setup (React App)
Navigate to the frontend project folder:

Bash

cd ../frontend # Adjust this path if your frontend project is not directly inside "frontend"
(This is the folder containing package.json).

Install npm dependencies:

Bash

npm install
Verify API Base URL:

Open frontend/src/api/axiosInstance.js.

Ensure API_BASE_URL matches your backend's HTTPS URL (e.g., https://localhost:7120/api).

Running the Application
Running the Backend API
Navigate to the backend project folder (backend/Server or your specific backend path):

Bash

cd backend/Server
Run the API:

Bash

dotnet run
The API will typically run on https://localhost:7120 (or http://localhost:5292 if configured for HTTP, check Properties/launchSettings.json). Keep this terminal window open.

Running the Frontend React App
Navigate to the frontend project folder (frontend or your specific frontend path):

Bash

cd frontend
Run the React app:

Bash

npm run dev
The React app will typically open in your browser at http://localhost:5173 (Vite) or http://localhost:3000 (Create React App).

API Endpoints (Swagger UI)
You can explore the backend API endpoints using Swagger UI:

Once the backend is running, open your browser and navigate to: https://localhost:7120/swagger (adjust port if needed).

This provides a detailed interactive documentation of all available API endpoints.

Assumptions and Design Choices
Authentication Flow: The login process currently verifies Email and Phone only, without a password for simplicity in this MVP. For a production system, password hashing and secure verification would be mandatory.

User Model (BLUser): The BLUser model used for registration does not contain a password field. It's assumed that password management (hashing, storage) is handled implicitly by the _userService.Create method in the backend's BL layer, or that passwords are not stored/used for this specific MVP.

Submodules: The monorepo setup uses Git submodules for the backend. This requires extra steps for cloning (--recursive) and updating (git submodule update --init --recursive). This allows independent development and versioning of the backend.

Error Handling: Basic error handling is implemented on both frontend and backend. For a production environment, more robust and user-friendly error messages, as well as logging, would be necessary.

UI/UX: Material-UI (MUI) is used to provide a professional and consistent design across the application. Minimal custom CSS is applied.

Contact
For any questions or issues, please contact [s0548426472@gmail.com].
