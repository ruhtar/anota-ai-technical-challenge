# Anota Aí - Backend Technical Challenge Mid Level 

## Overview

- This project is a backend API for Anota Aí Code Challange for MidLevel position. This application manages a product catalog in a marketplace application. 
- The API was developed using .NET instead of Node.js (as C# is my main programming language), and RabbitMQ was used for message queuing instead of AWS SQS because it is a technology that I'm currently learning. 
- The system uses MongoDB as the database and AWS S3 for storing the catalog JSON. 
- The primary functionalities include creating, updating, associating, and deleting products and categories, as well as managing and publishing catalog changes.

You can find the challenge repository here: [Anota Aí Backend Challenge](https://github.com/githubanotaai/new-test-backend-nodejs)

## Technologies Used

- **.NET**: Backend framework for developing the API.
- **MongoDB**: Database for storing products and categories.
- **RabbitMQ**: Message queue service for catalog change notifications.
- **AWS S3**: Storage service for catalog JSON files.
- **ASP.NET Core**: Web framework for handling HTTP requests.
- **XUnit**: Integration and unit tests.
- **Moq**: Mock lib used for tests.

## Must have User Stories for the challenge 

1. **Register a Product**
   - As a user, I want to register a product with its owner, so that I can access its data in the future (title, description, price, category, owner ID).

2. **Register a Category**
   - As a user, I want to register a category with its owner, so that I can access its data in the future (title, description, owner ID).

3. **Associate a Product with a Category**
   - As a user, I want to associate a product with a category.

4. **Update Product or Category Data**
   - As a user, I want to update the data of a product or category.

5. **Delete a Product or Category**
   - As a user, I want to delete a product or category from my catalog.

6. **Catalog Search Endpoint**
   - The catalog is represented as a JSON compilation of all available categories and items owned by a user, which does not require fetching information from the database on each search request.

7. **Publish Catalog Changes**
   - Whenever there is a change in the product catalog, publish this change to the "catalog-emit" topic in RabbitMQ.

8. **Consume Catalog Changes**
   - Implement a consumer that listens to catalog changes for a specific owner. When the consumer receives a message, search the database for that owner's catalog, generate the catalog JSON, and publish it to an AWS S3 service bucket.

## API Endpoints

### Product Endpoints

- **GET /api/products/{id}**
  - Gets a product.

- **GET /api/products**
  - Gets all products.

- **POST /api/products**
  - Register a new product.

- **PATCH /api/products/{id}**
  - Update a product's data.

- **DELETE /api/products/{id}**
  - Delete a product.

### Category Endpoints

- **GET /api/categories/{id}**
  - Gets a category.

- **GET /api/categories**
  - Gets all categories.

- **PATCH /api/categories/{id}**
  - Update a category's data.

- **DELETE /api/categories/{id}**
  - Delete a category.

### Catalog Endpoints

- **GET /api/catalog/{ownerId}**
  - Retrieve the catalog for a specific owner.

## Message Queue and Storage

- **RabbitMQ**: Used for publishing catalog change events.
  - Topic: `catalog-queue`
  
- **AWS S3**: Used for storing catalog JSON files.
