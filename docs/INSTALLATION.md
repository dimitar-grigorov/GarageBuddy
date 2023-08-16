
# GarageBuddy Installation Guide

This guide will walk you through the installation process for setting up GarageBuddy, an ASP.NET project, along with the necessary SQL Server setup using Docker. Please follow the steps below to ensure a successful installation.

## Prerequisites

Before you begin, make sure you have the following prerequisites installed on your system:

-   [Docker](https://www.docker.com/get-started)
-   [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet)

## Step 1: Run SQL Server Container

In this step, we will set up a SQL Server container using Docker.

1.  Open a terminal or command prompt.
    
2.  Run the following Docker command to start the SQL Server container:

```bash
docker run --name garage-buddy-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SUPER_SECRET_PASSWORD" -p 1433:1433 -v sqlvol-garage:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-latest
```
Replace `SUPER_SECRET_PASSWORD` with the desired password for the `sa` user.

## Step 2: Configure User Secrets

In this step, we will configure user secrets to store the database connection string securely.

1.  Navigate to your GarageBuddy project directory.
    
2.  Open a terminal within the project directory.
    
3.  Run the following command to set the user secrets:
```bash
dotnet user-secrets set "DatabaseSettings:DbProvider" "mssql"
dotnet user-secrets set "DatabaseSettings:DefaultConnection" "Server=.;Database=GarageBuddy;Trusted_Connection=False;TrustServerCertificate=True;User Id=sa;Password=SUPER_SECRET_PASSWORD"
```
Alternatively in Visual Studio add this in User Secrets.
```json
{
  "DatabaseSettings": {
    "DbProvider": "mssql",
    "DefaultConnection": "Server=.;Database=GarageBuddy;Trusted_Connection=False;TrustServerCertificate=True;User Id=sa;Password=SUPER_SECRET_PASSWORD"
  }
}
```
Make sure to replace `SUPER_SECRET_PASSWORD` with the password you set in Step 1.

## Step 3: Build and Run GarageBuddy (optional)

In this step, we will build and run the GarageBuddy ASP.NET project.

1.  Open a terminal within the project directory.
    
2.  Run the following commands to restore dependencies, build, and run the project:
```bash
dotnet  tool install -g Microsoft.Web.LibraryManager.Cli
libman install
dotnet restore
dotnet build
dotnet run
```
