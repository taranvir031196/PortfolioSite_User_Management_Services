# PortfolioSite_User_Management_Services

The User Management Service is a RESTful API built in .Net 7.0 that provides functionality for managing users within a portfolio Site. It allows clients to perform operations such as user creation, retrieval, update, and deletion.

## Table of Contents

- [Getting Started](#getting-started)
- [Endpoints](#endpoints)
- [Authentication](#authentication)
- [Error Handling](#error-handling)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

To get started with the .NET 7 User Management Services, follow these steps:

1. **Clone the repository:**
   ```sh
   git clone https://github.com/taranvir031196/PortfolioSite_User_Management_Services

2. **Install the .NET 7 SDK:**

   Follow the instructions on the .NET website to download and install the [.NET 7](https://dotnet.microsoft.com/en-us/download) SDK for your platform.

3. **Restore dependencies:**
      ```sh
      cd user-management-service
      dotnet restore

4. **Configure environment variables:**
    ```sh
     cp appsettings.example.json appsettings.json

5. **Start the service:**
   ```sh
   dotnet run

## Endpoints

  - POST /api/v1/authenticate/roles/add: endpoint for adding a role.
  - POST /api/v1/authenticate/register: endpoint for registering a user on Portfolio platform.
  - POST /api/v1/authenticate/login: endpoint for logging into the Portfolio application.
  - PUT /api/v1/authenticate/updateUser: endpoint for updating a user already added into the system.
  - DELETE /api/v1/authenticate/deleteUser: endpoint for deleting already added user from the system.
   
    For detailed information on request and response formats, refer to the API documentation.
   
## Authentication
   The .NET 7 User Management Service uses JSON Web Tokens (JWT) for authentication. To access protected endpoints, clients must include a valid JWT token in the Authorization header of the request.
   
## Error Handling
   The .NET 7 User Management Service follows standard HTTP status codes and error handling practices. In case of an error, the server will respond with an appropriate status code and error message in the response body.

## Contributing
   Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request on GitHub.

## License
   This project is licensed under the MIT License - see the LICENSE file for details.
