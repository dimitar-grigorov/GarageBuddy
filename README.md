# Garage Buddy - Open Source Garage Management System

[![All Contributors](https://img.shields.io/github/contributors/dimitar-grigorov/GarageBuddy)](https://github.com/dimitar-grigorov/GarageBuddy/graphs/contributors)
![GitHub last commit](https://img.shields.io/github/last-commit/dimitar-grigorov/GarageBuddy.svg)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/dimitar-grigorov/GarageBuddy)
[![License](https://img.shields.io/github/license/dimitar-grigorov/GarageBuddy.svg)](LICENSE)

**Garage Buddy** is a comprehensive, open source garage management system built on the [ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-6.0) platform. Designed to cater to the needs of auto mechanics and workshops, Garage Buddy simplifies the process of managing service records and parts for every customer's car. Everything can be easily tracked and organized within the application.

## Key Features

- **Cloud-Based Solution**: Garage Buddy is a fully-featured, cloud-based application, eliminating the need for complex installations. Mechanics can access the system by simply navigating to the login page using their web browser and entering their login details. This ensures hassle-free accessibility from anywhere with an internet connection.

- **Scalability**: With its cloud-based nature, Garage Buddy offers easy scalability to suit garages or workshops of any size. Whether you run a small repair shop or a large-scale auto service center, the system adapts to your requirements, making it a perfect fit for your business.

- **User Management**: Garage Buddy supports multiple user accounts, allowing mechanics and employees to have their own personalized access. Adding additional users is a breeze, making it convenient for your solution to grow as your business expands.

- **Service Tracking**: Keep a detailed log of every service performed on a customer's vehicle. From routine maintenance to complex repairs, Garage Buddy helps you maintain a complete history of each car, ensuring better customer service.

- **File Attachment**: Mechanics can attach files to service records, including pictures of repairs before and after completion. This feature enhances documentation and allows customers to see visual evidence of the work done. Additionally, warranties for parts used in repairs can be tracked, ensuring quick access to warranty details when needed.

## Getting Started

To get started with Garage Buddy, follow the instructions below:

1. Clone the repository: `git clone https://github.com/dimitar-grigorov/GarageBuddy`
2. Install the required dependencies
- Visual Studio 2022
- Client side libraries are automatically installed by LibMan (included in Visual Studio).
- SQL Server 2019
3. Configure the database settings to fit your environment.

### Configure Database Settings

Garage Buddy uses different than the default configuration file (`appsettings.json`). It looks like this:

```json
{
  "DatabaseSettings": {
    "DbProvider": "mssql",
    "DefaultConnection": "Server=.;Database=GarageBuddy;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
Also the same format can be used in the User Secrets.

4. Apply the database migrations: `Update-Database`
- In Visual Studio, open the Package Manager Console and select the `Data\GarageBuddy.Data` project as the Default project.
- Execute the `Update-Database` command.
5. On the first run the application will seed the database. The first registered user will be an administrator.

For more detailed instructions on installation and setup, please refer to the [Installation Guide](https://github.com/dimitar-grigorov/GarageBuddy/blob/main/docs/INSTALLATION.md).

## Technologies used
- ASP.NET Core 6
- ASP.NET Core Identity
- Entity Framework Core

## Third-Party
 - Based on [ASP.NET Core MVC template](https://github.com/NikolayIT/ASP.NET-Core-Template) by [Nikolay Kostov](https://github.com/NikolayIT)
 - Theme [Mazer Dashboard](https://github.com/zuramai/mazer) by [Zuramai](https://github.com/zuramai)
 - Custom [npm package](https://www.npmjs.com/package/@grigorov-it/mazer) build on top of the Mazer theme.
 
## Contributing

Garage Buddy is an open source project, and we welcome contributions from the community. If you find any bugs, have feature suggestions, or want to help improve the system, please feel free to submit issues and pull requests.

## License

Garage Buddy is released under the [MIT License](https://github.com/dimitar-grigorov/GarageBuddy/blob/main/LICENSE).
