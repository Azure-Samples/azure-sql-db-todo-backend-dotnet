---
page_type: sample
languages:
- tsql
- sql
- aspx-csharp
- csharp
- json
products:
- azure
- dotnet
- aspnet
- aspnet-core
- azure-app-service
- azure-sql-database
description: "TodoMVC Backend Implementation with Azure WebApps, Net Core and Azure SQL"
urlFragment: azure-sql-db-todo-backend-dotnet
---

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

# TodoMVC Backend Implementation with Azure WebApps, .Net Core and Azure SQL

![License](https://img.shields.io/badge/license-MIT-green.svg)

Implementation of the [Todo Backend API](http://www.todobackend.com/index.html) using Azure WebApps, .Net Core and Azure SQL. 

## Unit Test

As per Todo Backend API specifications, unit test can be run via this link, once you have deployed it on Azure:

[Todo Backend Specs Unit Test](https://todobackend.com/specs/index.html)

Make sure to [configure CORS correctly](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-rest-api#add-cors-functionality) for your Web App.

## Live Client

You can also test this implementation right using the live [ToDoMVC](http://todomvc.com/) test app:

[Todo Full-Stack Example](https://todobackend.com/client/index.html)

using the deployed solution as the backend server. For example: https://dm-tdb-02.azurewebsites.net/todo.

## Deployment

### Create Database

Create an Azure SQL database where to host the to-do data. If you need help to create an Azure SQL database, take a look here: [Running the samples](https://github.com/yorek/azure-sql-db-samples#running-the-samples). 

If you are completely new to Azure SQL, here's a full playlist that will help you: [Azure SQL for beginners](https://www.youtube.com/playlist?list=PLlrxD0HtieHi5c9-i_Dnxw9vxBY-TqaeN).

### Manual Deployment

WIP

### Automated Deployment

WIP

## Contributing 

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

More details in the full [Contributing](./CONTRIBUTING.md) page.
