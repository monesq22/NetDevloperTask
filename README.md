# BusinessCard Project

## Overview
This repository contains the full stack implementation of the BusinessCard project, consisting of a .NET backend and an Angular frontend. The application supports importing/exporting business card data (CSV/XML), viewing, and filtering business cards.

## Features
- Import and Export business cards (CSV, XML)
- Create, read, update, and delete business cards
- Frontend using Angular 15 with a modern UI
- Backend using .NET 8 Web API
- Database with SQL Server for storing business card information

## Project Structure
- `backend`: Contains the .NET Core Web API.
- `frontend`: Contains the Angular frontend app.

## Prerequisites

### Backend (for .NET API)
- .NET SDK 8.0 or higher
- SQL Server

### Frontend (for Angular)
- Angular CLI (version 15.x or higher)

## Setup Instructions

### Backend Setup
1. Navigate to the `backend` folder:
Restore dependencies
In the terminal or package manager console (PMC), navigate to the project root directory and run:
** dotnet restore
Set up the database :
In the terminal or package manager console (PMC), navigate to the project root directory and run:
** dotnet ef migrations add your-db
** dotnet ef database update
Update the connection string in appsettings.json:

"ConnectionStrings": {
   "DefaultConnection": "Server=your-server;Database=your-db;User Id=your-username;Password=your-password;"
}

##Frontend Setup

Install dependencies:
In the terminal
npm install
Update the backend API URL in BusinessCardService.ts file:
  private apiUrl = 'https://localhost:7155/api/BusinessCard';  // Correct API URL

### SQL Server Database Restoration
To restore the database, run the following command in SQL Server Management Studio:
"script.sql"

