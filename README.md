# MazeWalker - scraper of TVMaze

## Technology

This project has been created using Azure Functions. You can run it locally using Azure Storage Emulator (or Azurite) and Azure Function Core Tools.
Within the project there are two functions - one will scrape data from TVMaze on schedule and the other will serve the API exposing scraped data  

## Prerequisites

1. CosmosDB account
2. Principal that executes the functions has to have Contributor or DocumentDB Account Contributor role assigned. If you created the cosmos account you're using you likely already have necessary role assignment.

## Running locally

1. save your database  credentials to user secrets:
```c#
dotnet user-secrets set "SubscriptionId" "<id of the Azure subscription>"
dotnet user-secrets set "CosmosAccountName" "<name of the account"
dotnet user-secrets set "CosmosDatabaseName" "<name of the database. it will be created>"
dotnet user-secrets set "ResourceGroupName" "<name of the resource group where the account is created>"
```
2. Run the function from Visual Studio / VS Core / Rider run menu. You can also use Azure Functions Core Tools directly
