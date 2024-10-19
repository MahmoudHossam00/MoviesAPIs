# Movies Encyclopedia System APIs

This project provides a simple Movies Encyclopedia Management System API that allows users to add and manage various movies and categories.

## Features

- **Authentication**: Users can register and log in to access different permissions.
- **Moderation**: Admins can assign different roles to users and edit existing movies.

## Project Structure

The project follows a layered architecture and includes the following components:

- **Models**: Manages entities such as `ApplicationUsers`, `Categories`, and `Movies`.
- **Controllers**: Handles user requests, including posting, commenting, replying, liking, and moderation actions (e.g., assigning permissions and suspending users).
- **Services**: Contains business logic for handling various operations.
- **Helpers**: Manages AutoMapper and configuration classes for JWT and other requirements.
- **Assets**: Used for testing the API with different types of inputs.
- **DTO (Data Transfer Objects)**: Handles different models used for user requests.

## Database Tables

The system utilizes the following tables:

- **Categories**: Stores names of different categories.
- **Movies**: Stores movie data.
- **AspNetUsers**: Stores application user information and data.
- **AspNetRoles**: Stores roles and permission information.

## Technologies Used

- **C#**
- **ASP.NET Core**
- **Entity Framework Core**

## Required Tools and Technologies

This project requires the following tools and technologies:

- **Visual Studio 2022**: The recommended Integrated Development Environment (IDE) for this project, providing full support for .NET 8.0 and additional tools like scaffolding and debugging.
- **.NET 8.0 SDK**: The project is built using the .NET 8.0 framework, which must be installed for development and running the application.
- **SQL Server**: A relational database system used to store the forum data, including user information, posts, and comments.

### NuGet Packages

- **Microsoft.AspNetCore.Authentication.JwtBearer**: Manages user authentication via token-based security.
- **Microsoft.AspNetCore.Identity**: Handles user management and role-based access control.
- **Microsoft.EntityFrameworkCore**: Provides Object-Relational Mapping (ORM) to interact with the SQL Server database.
- **Swashbuckle.AspNetCore**: Generates interactive API documentation using Swagger.
