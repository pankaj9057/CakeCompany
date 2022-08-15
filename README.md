# CakeCompany
Cacke Company is a sample solution to demonstrate code/architecture refactoring

This Solution will demonstrate the following
1.  Dependancy Injection and Mediator Pattern (Behavioural Pattern)
2.  Clean/Onion Architecture

To acheive and simplify this I have used below Nuget packages
1.  Serilog
2.  Serilog.Enrichers.Thread
3.  Serilog.Extensions.Logging
4.  Serilog.Sinks.Console
5.  Serilog.Sinks.File
6.  MediatR
7.  MediatR.Extensions.Microsoft.DependencyInjection
8.  Microsoft.Extensions.DependencyInjection
9.  Microsoft.Extensions.Logging.Abstractions
10. Microsoft.Extensions.Logging.Console
11. xUnit
12. Moq
13. FluentAssertion
14. Microsoft.NET.Test.Sdk

In the CakeCompany solution I have created six layer which are mentioned below
1.  CakeCompany (console proj)
2.  CakeCompany.Application (For CQRS)
3.  CakeCompany.Core (For Dtos and Interfaces)
4.  CakeCompany.Providers (For Business Logics)
>This can be splitted into two part i.e Providers (Only Business Logic) and Infrastructure (Only DB Operation) as we do not have DB operation so I have not seperated the logics
5.  CakeCompany.Test (For Unit Testing)
>If you have Infrastructure layer then create Integration test project and write all DB related test case to test Infrastructure layer.

Initial Version Can be found [here](https://github.com/pankaj9057/CakeCompany/tree/3f0c71a3a93a09d622cc93f4cd1647b4d53cbd4d)

Ref: [Common web application architectures](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
