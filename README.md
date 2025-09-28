# WEB API .NET Developer Project
## Project Overview
The primary purpose of this project is to demonstrate the ability to integrate multiple external APIs, design a reliable backend system, and manage data storage efficiently.

## Features
- Fetches real-time cryptocurrency data from multiple sources.
- Stores all fetched data in a relational database.
- Provides a Web API to retrieve and manipulate stored data.
- Easily extendable to include more cryptocurrencies or blockchain networks.

## Technology Stack
- **Backend:** .NET 8.0
- **Database:** SQLite
- **API Integration:** REST API
- **Deployment:** Docker, Linux VM
  
## Installation
1. Clone the repository:  
   ```bash
      https://github.com/nmokariya25/blockchain_app.git
2. Navigate to the project directory:
   ```bash
     cd src
3. Build Project using dotnet command
   ```bash
     dotnet clean
     dotnet restore
     dotnet build

## Usage
1. Run actual application
   ```bash
      dotnet run

2. Run Unit Test cases of the project
   ```bash
      cd..
      dotnet test .\tests\MyBlockChain.Tests.Functional\
      dotnet test .\tests\MyBlockChain.Tests.Integration\
      dotnet test .\tests\MyBlockChain.Tests.Unit\

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for bug fixes, feature requests, or improvements.

   
