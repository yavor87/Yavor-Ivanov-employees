# Employees

One Paragraph of project description goes here

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Development Prerequisites

- Microsoft Visual Studio 2017
- Nuget and Nuget Command Line Tool

### Build from command line

- Clone the repository
- CD to the Solution folder (*src/Employees*)
- Execute Command ```nuget.exe restore```
- Execute Any of the following commands:
    - Debug:      
        ```msbuild Employees.sln /property:Configuration="Debug"```

    - Release: 
        ```msbuild Employees.sln /property:Configuration="Release"```

## Running the tests

- Execute Command ```nuget.exe restore```
- Build the test project
    ```msbuild Employees.Test/Employees.Test.csproj /property:Configuration="Debug"```
- Run the tests
    ```.\packages\xunit.runner.console.2.4.0\tools\net461\xunit.console.exe .\Employees.Test\bin\Debug\Employees.Test.dll```
## Authors

* **Yavor Ivanov** - *Initial work*

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
