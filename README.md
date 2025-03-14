# KB2025

### Clean Architecture.

- **Domain Layer** ➜ the project that contains the domain layer, including the entities, value objects, and domain services

- **Application Layer** ➜ the project that contains the application layer and implements the application services, DTOs (data transfer objects), and mappers. It should reference the *Domain* project.

- **Infrastructure Layer** ➜ The project contains the infrastructure layer, including the implementation of data access, logging, email, and other communication mechanisms. It should reference the *Application* project.

- **Presentation Layer** ➜ The main project contains the presentation layer and implements the ASP.NET Core web API. It should reference the *Application* and *Infrastructure* projects.

-----------------------------------------------------------------------

[English](https://github.com/Hereigo/KB2025/tree/main/ENGLISH)

[JavaScript](https://github.com/Hereigo/KB2025/tree/main/JS)

[SQL](https://github.com/Hereigo/KB2025/tree/main/SQL)

