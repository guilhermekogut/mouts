# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)

## Setup and Running the Project

### Prerequesites

•	.NET 8 SDK

•	Docker Desktop (for running containers)

•	Visual Studio 2022 or another compatible IDE

### Running with Docker Compose

1.	**Select the Docker Compose Profile**

In Visual Studio, set the docker-compose project as the startup project.
Alternatively, from the command line, you can run:

```powershell
docker-compose up --build
```

2.	**Expose Database Ports (Required)**

To run database migrations from the command line, you must expose the database service port to your host.
Edit the docker-compose.override.yml file and add a ports mapping to the database service. For example:

```yaml
services:
  database:
    ports:
      - "5432:5432" # Expose PostgreSQL port
```
Adjust the port numbers as needed for your database engine.

3.	**Add Connection String to usersecrets.json (Required)**

Ensure your usersecrets.json contains the connection string for the database, using the internal Docker network hostname (e.g., ambev.developerevaluation.database).
Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=internal.host.name;Port=5432;Database=ambev;Username=<userName>;Password=<password>"
  }
}
```
> **Note:** The hostname must match the service name defined in your docker-compose.yml.

4.	**Ensure Containers Are Running**

Before running any database migration commands, make sure all containers (especially the database) are up and healthy.
You can check the status with:
````powershell
docker-compose ps
````

### Database Migrations

Before running the application, ensure the database schema is up to date.

1.	**Set the API Project as Startup Project**

In Visual Studio, right-click the Ambev.DeveloperEvaluation.WebApi project and select "Set as Startup Project".

2.	** Run Database Update**

Use the following command in the root directory (or adjust the path as needed):

```powershell
dotnet ef database update --connection "Host=127.0.0.1;Port=5432;Database=developer_evaluation;Username=<userName>;Password=<password>;Trust Server Certificate=true;" --startup-project Ambev.DeveloperEvaluation.WebApi
```
> **Note** The --startup-project parameter must point to the API project so that the correct configuration is used.
Important: The database container must be running and the port exposed as described above.

### Running Unit Tests

To execute the unit tests, run the following command from the solution root:
```powershell
dotnet run tests --project .\tests\Ambev.DeveloperEvaluation.Unit\
```
> **Note** Ensure that you run this command from the solution root directory, where the `tests` folder is located.
This will build and run unit tests.
