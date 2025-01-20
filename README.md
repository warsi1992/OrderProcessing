## OrderProcessing
## This document provides instructions to set up and run the Order Processing System API project.

# Prerequisites

## Ensure you have the following installed on your machine:

## .NET Framework (version used in the project, e.g., .NET 6.0/7.0)

## SQL Server (for database management)

## Visual Studio (or any compatible IDE)

## Git (if using version control)

## Project Setup Instructions

# 1. Clone the Repository

If using Git, clone the project repository:

  git clone 
  cd OrderProcessingSystem

# 2. Configure Database Connection

Open appsettings.json and update the ConnectionStrings section with your SQL Server instance details:

  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=OrderProcessingDatabase;Integrated Security=True;Encrypt=False;"
  }

Replace YOUR_SERVER_NAME with your local SQL Server instance.

# 3. Restore Dependencies

Navigate to the project folder and run the following command to restore NuGet packages:

  ### dotnet restore

# 4. Apply Database Migrations (Entity Framework Core - Code First Approach)

## This project follows the Entity Framework Core Code First approach.
# To create the database schema, run:

  ### dotnet ef database update

# To add a new migration, use:

  ### dotnet ef migrations add InitialCreate

# Ensure Entity Framework Core tools are installed by running:

  ### dotnet tool install --global dotnet-ef

# 5. Build the Project

## Compile the project using the following command:

 ### dotnet build

# 6. Run the Application

## To start the API, use:

  ### dotnet run

# 7. Test the API

Use tools like Postman or Swagger UI to test API endpoints:
# 8. Logging

Logging is configured via Serilog. Logs are written to the console with the configured logging level in appsettings.json:

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }

Adjust logging levels as needed.

# 9. Deployment

To publish the project, use the following command:

 ### dotnet publish -c Release -o ./publish

Deploy the output folder publish to your preferred hosting environment.

# 10. Troubleshooting

Ensure SQL Server is running and accessible.

Check for any missing NuGet packages using dotnet restore.

Review logs(C:/Logs) for any runtime errors.

## How to run the api
### 1. Add New customer just pass name and email in requestbody.
### 2. Add as many product you want to add.
### 3. Create Order by providing customer id to it and selecting roduct by id ([1,2,etc]) it will update customer table as well..

Following these steps, you should be able to set up and run the Order Processing System API successfully.
