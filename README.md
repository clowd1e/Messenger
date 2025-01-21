<div>
  <img align="left" src="https://github.com/clowd1e/Messenger/blob/b8aac0492b821a1680e3edaab180b2d0849a9194/Logo.png" width="40" height="40" />
  <h1>Messenger</h1>
</div>

## About the Project

**Messenger** is a *Real-Time Communication* project that is designed to enhance communication technologies for users. It consists of two parts: *API* and *Client*.

### Built with

The project is built using:

* ![ASP.NET](https://img.shields.io/badge/ASP.NET-blue?logo=dotnet&logoColor=white)
* ![Angular](https://img.shields.io/badge/angular-%23DD0031.svg?style=for-the-badge&logo=angular&logoColor=white)
* ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

## Installation

Clone or download the repository on your local machine.

To clone the repository:
1. Download [Git](https://git-scm.com/downloads).
2. Open git bash.
3. Execute command:
   ```sh
   git clone https://github.com/clowd1e/Messenger.git
   ```

## Running the Application

To install and run the project, you need [Docker](https://www.docker.com/get-started/) installed on your machine.

Before launching the application, ensure the following ports are not occupied:
* `5000` - used by the Web API
* `1433` - used by the database
* `10000` - used by blob storage
* `4200` - used by the client-side application

### 1. Running the API

Navigate to /repo-path/`Api/src` and execute the following command:
 ```sh
 docker compose up -d --build
 ```

### 2. Running the Client

Navigate to /repo-path/`Client` and execute the following command:
 ```sh
 docker compose up -d --build
 ```

## Accessing the Application

After running the project, you can access: 
* The Client at `http://localhost:4200`
* The API at `http://localhost:5000/swagger`

## Bugs And Feedback

If you encounter any bug or issue, please use GitHub Issues.

## LICENSE
This project is licensed under the MIT License. See the [LICENSE](https://github.com/clowd1e/Messenger/blob/b8aac0492b821a1680e3edaab180b2d0849a9194/LICENSE) file for details.
