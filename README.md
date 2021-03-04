# **RestSample API**

Rest API created to showcase basic .Net Core Functionality

## **Public API**

## Products -> api/products

- [GET] GetAll
  - Called upon entering page. Returns all products
- [GET] GetAllFiltered -> api/products/filter?
  - Filters the products by the following parameters:
  - Size: comma separated string -> size=small,medium
  - Highlight: comma separated string -> highlight=word,hat
  - MaxPrice: decimal -> maxPrice=5 or maxPrice=5.5
  - MinPrice: decimal -> minPrice=2 or minPrice=1.5
- [GET] Metadata -> api/metadata
  - Returns the following metadata
  - MinPrice
  - MaxPrice
  - AllSizes -> Lists all unique available sizes
  - CommonWords -> Takes 5-15th most commonly word
  - Takes no params, but can be modified with Query params easily
- Upon calling GetAll and GetAllFiltered metadata will also be returned. Only metadata for filtered items will be shown.

## **Architecture**

Application is split into four projects:

- RestSample.App
- RestSample.Core
- RestSample.Data
- RestSample.Test

## RestSample.App

Main portion of this assignment.

Dtos folder contains simple helper classes. In real application, an AutoMapper and full DTOs should be created for each model, however for demo application purposes the base Products Model was used, with simple input, output, and metadata builder.

Client folder contains base MockyHttpClient, which currently has support for getting all data from MockyApi, but it can be expanded with CRUD later. Each call to Mocky is cached for default duration pulled from Mocky settings. In case something goes wrong during the call to Mocky API an empty list will be returned, which should later be changed with custom User Friendly exception handling so user can see what went wrong. Mocky options are pulled from appsettings.json into a MockyOptionsClass, so they can be used anywhere in the application, and they are easily configurable.

Services folder contains base service interface which defines some methods each service should probably have. Product service inherits it and implements multiple additional functions, such as GetAllFiltered and GetAllMetadata. ProductService calls our MockyHttpClient, gets data from it, then builds metadata and/or filters it.

## RestSample.Core

Simple domain layer. Store&#39;s exceptions, helpers and Mocky support. Base Mocky Client from App layer should be moved here.

## RestSample.Data

Models are stored here. There is just a single model that handles Products currently. In case something had to be saved to a database it would be added here. Also, modular database support, such as EF Code First can go here.

WordHelper is a static class that can be used for getting unique words in a list of lists, get a list of most used words, and highlight requested words in a sentence.

## RestSample.Test

Unit testing project created for testing specific parts of the applications.

One testing project was created for an entire solution, so it should contain subfolders for each project in the solution.

Test example was created by using RestSample.Data.Helpers.WordHelper static class, so the Test class should be placed to folders Core/Helpers and named WordHelperUnitTest.cs.

Tests should follow the naming convention: FunctionForTesting\_FunctionUseCase\_ExpectedResult.