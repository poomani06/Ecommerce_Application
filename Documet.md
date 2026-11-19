# E-Commerce Application
## Version 1 – Category Management Implementation

### 1. Overview

In this version, I implemented the Category Management feature of my E-Commerce Application.

The implementation was divided into two commits:

1. **Backend:** Implement Category CRUD API using ASP.NET Core Web API, Entity Framework Core and SQL Server.
2. **Frontend:** Implement Category Management UI using Angular, TypeScript and HttpClient.

The main purpose is to allow users to view, add, update and delete product categories through the Angular application.

### 2. Commit 1 – Backend: Category CRUD API

**Commit message:** `Implement category CRUD API`

#### Technologies used

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Dependency Injection
- DTOs
- Repository Pattern
- Service Layer

#### 2.1 Backend architecture

I followed the layered architecture:

Controller → Service → Repository → DbContext → SQL Server

Each layer has a separate responsibility.

**1. Controller**

File: `CategoriesController.cs`

- Receives HTTP requests from Angular or Swagger.
- Defines API endpoints.
- Calls the service layer.
- Returns appropriate HTTP responses.

**2. Service**

Files: `ICategoryService.cs`, `CategoryService.cs`

- Contains category business logic.
- Validates category information.
- Checks duplicate category names.
- Prevents deleting categories that contain products.
- Converts entities into response DTOs.

**3. Repository**

Files: `ICategoryRepository.cs`, `CategoryRepository.cs`

- Handles database operations.
- Uses Entity Framework Core.
- Retrieves, inserts, updates and deletes category records.
- Checks whether a category name already exists.
- Checks whether a category contains products.

**4. DbContext**

File: `ApplicationDbContext.cs`

- Represents the database session.
- Provides access to database tables through DbSet properties.
- Manages entity relationships using Entity Framework Core.

#### 2.2 DTOs

I used Data Transfer Objects (DTOs) to transfer only the required information between the client and backend.

Three DTOs were created:

- `CreateCategoryDto`: Receives category information when creating a category.
- `UpdateCategoryDto`: Receives updated category information.
- `CategoryResponseDto`: Sends category information back to the client.

**Why DTOs?**

DTOs separate API request and response data from database entity models. They also allow validation rules to be applied to incoming data.

#### 2.3 Category business logic

The service layer handles these validations:

1. Category name is required.
2. Category name is trimmed to remove unnecessary spaces.
3. Duplicate category names are not allowed.
4. A category cannot be deleted if products are associated with it.

For example, if a category called Electronics already exists, creating another category with the same name should be rejected.

If a category has products, deletion is prevented to protect the existing product-category relationship.

#### 2.4 API endpoints

Base URL: `/api/categories`

| HTTP Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/categories` | Get all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create a category |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category |

#### 2.5 Backend request flow

Example: Adding a category.

1. The client sends a POST request to `/api/categories`.
2. The controller receives the request.
3. The request data is validated.
4. The controller calls the category service.
5. The service checks the category name and duplicate records.
6. The service calls the repository.
7. The repository adds the category using Entity Framework Core.
8. `SaveChangesAsync()` saves the data into SQL Server.
9. The API returns the response to the client.

### 3. Commit 2 – Frontend: Category Management UI

**Commit message:** `Implement category management UI`

#### Technologies used

- Angular
- TypeScript
- HTML
- CSS
- Angular FormsModule
- HttpClient
- Angular Services
- RxJS Observables
- Angular Signals and Effects (proposed component improvement)

#### 3.1 Category model

File: `category.model.ts`

I created TypeScript interfaces to define the category data structure.

- `Category`: Represents category information received from the backend.
- `CreateCategory`: Represents the data required to create a category.
- `UpdateCategory`: Represents the data required to update a category.

These interfaces provide type safety when working with category data.

#### 3.2 Category service

File: `category.ts`

The CategoryService communicates with the backend API using Angular HttpClient.

It contains methods for:

- Getting all categories.
- Getting a category by ID.
- Creating a category.
- Updating a category.
- Deleting a category.

The service keeps HTTP communication separate from the component.

The API base URL is configured through the Angular environment configuration.

#### 3.3 Category component

Files: `categories.ts`, `categories.html`, `categories.css`

The component manages the Category Management page.

It handles:

- Loading category information.
- Adding categories.
- Editing existing categories.
- Updating categories.
- Deleting categories.
- Displaying success and error messages.
- Managing loading and saving states.

#### 3.4 Angular features used

**1. Component**

The Categories component controls the category management page and connects the HTML template with the TypeScript logic.

**2. FormsModule and ngModel**

I used template-driven forms to collect category name and description.

Example:

`[(ngModel)]="name"`

This provides two-way data binding between the input field and the component property.

**3. Dependency Injection**

CategoryService is injected into the Categories component through its constructor.

This allows the component to use API methods without directly creating the service object.

**4. HttpClient**

HttpClient sends HTTP requests to the ASP.NET Core backend.

**5. Observables and subscribe()**

HTTP methods return Observables. I used `subscribe()` to handle successful API responses and errors.

**6. Angular control flow**

I used Angular's `@if` and `@for` syntax.

- `@if`: Displays success messages, errors, loading indicators and empty-state messages.
- `@for`: Iterates through categories and displays them in a table.

**7. Loading and saving states**

- `isLoading` controls the category loading message.
- `isSaving` controls the Add/Update button state.
- Success and error messages provide feedback after API operations.

#### 3.5 Frontend request flow

Example: Adding a category.

1. The user enters the category name and description.
2. The user clicks Add Category.
3. The component calls `saveCategory()`.
4. The component checks whether the category name is empty.
5. It creates a request object using the entered information.
6. It calls `CategoryService.createCategory()`.
7. HttpClient sends a POST request to the backend.
8. The backend validates and saves the category.
9. The frontend receives the API response.
10. The component displays a success message and reloads the category list.

### 4. Complete Frontend-to-Backend Flow

Angular UI → Categories Component → CategoryService → HttpClient → ASP.NET Core Controller → CategoryService → CategoryRepository → ApplicationDbContext → SQL Server

The response travels back through the API to Angular.

Angular then updates the displayed category list.

### 5. Example Request

**Create Category**

HTTP Method: POST

Endpoint: `/api/categories`

Request JSON:

```json
{
  "name": "Electronics",
  "description": "Electronic products and accessories"
}
```

Example response:

```json
{
  "categoryId": 1,
  "name": "Electronics",
  "description": "Electronic products and accessories"
}
```

The actual ID and response depend on the database and API response.

### 6. Testing Performed

Backend testing:
- Tested GET all categories using Swagger.
- Tested GET category by ID.
- Tested POST to create a category.
- Tested PUT to update a category.
- Tested DELETE for a category.
- Tested duplicate category names.
- Tested deletion when products are associated with a category.

Frontend testing:
- Opened the Category Management page.
- Checked whether categories load from the backend.
- Added a category.
- Edited an existing category.
- Updated category information.
- Deleted a category.
- Checked success and error messages.
- Checked whether the category list updates after operations.

### 7. Important Design Decisions

**Why use a service layer?**

To keep business logic separate from the controller and make the application easier to maintain.

**Why use a repository layer?**

To separate database operations from business logic.

**Why use async/await?**

To perform database operations asynchronously without blocking the request thread while waiting for database operations.

**Why prevent deleting categories that contain products?**

Products are related to categories through a foreign key. Preventing deletion protects existing relationships and avoids orphaned product records.

**Why use Angular services?**

To keep API communication separate from UI logic and make the service reusable.

### 8. Questions the Teacher May Ask

**Q1. What is CRUD?**

CRUD stands for Create, Read, Update and Delete. These are the four basic operations used to manage category records.

**Q2. What is the purpose of the controller?**

The controller receives HTTP requests, calls the service layer and returns HTTP responses.

**Q3. Why did you use DTOs?**

DTOs transfer only the required data between the frontend and backend. They also separate API data from database entities.

**Q4. What is the Repository Pattern?**

It separates database operations from business logic. In this project, CategoryRepository handles database operations for categories.

**Q5. What is Dependency Injection?**

Dependency Injection is a technique where required objects are provided to a class instead of the class creating them itself.

**Q6. What is Entity Framework Core?**

EF Core is an Object-Relational Mapper (ORM). It allows us to work with database records using C# objects instead of writing SQL for every operation.

**Q7. What is DbContext?**

DbContext represents a session with the database. It tracks entities and performs database operations.

**Q8. What is HttpClient in Angular?**

HttpClient is used to send HTTP requests from Angular to backend APIs.

**Q9. What is an Observable?**

An Observable represents a stream of asynchronous data. Angular HttpClient returns Observables, and we can subscribe to receive responses or errors.

**Q10. What is ngModel?**

`ngModel` provides two-way data binding between a form input and a component property.

**Q11. What is the difference between @if and @for?**

`@if` conditionally displays content. `@for` repeats content for each item in a collection.

**Q12. What happens if we delete a category that contains products?**

The backend prevents the deletion because products are associated with that category. This protects the foreign-key relationship.

**Q13. Why do we reload categories after adding or updating?**

To retrieve the latest data from the backend and display the updated category list.

**Q14. Why should we not write database logic in the controller?**

It mixes responsibilities and makes the controller difficult to maintain. Database operations belong in the repository layer.

**Q15. How does Angular communicate with ASP.NET Core?**

Angular uses HttpClient to send HTTP requests to ASP.NET Core API endpoints. The backend processes the request and returns a response, usually in JSON format.

### 9. Short Presentation to the Teacher

"In my E-Commerce project, I completed the Category Management vertical slice using two commits.

In the backend commit, I implemented Category CRUD operations using ASP.NET Core Web API. I followed Controller, Service, Repository and DbContext architecture. I used DTOs for request and response data, added validation for category names, prevented duplicate categories and prevented deleting categories that contain products.

In the frontend commit, I created the Category Management page using Angular. I created a category model, a CategoryService for HTTP communication and a Categories component for UI operations. I used HttpClient, Observables, FormsModule, ngModel, Angular control-flow syntax and dependency injection.

The Angular component sends requests to the backend API. The backend processes the request through its layers and saves or retrieves data from SQL Server using Entity Framework Core. The response is then displayed in the Angular application.

I tested the CRUD operations and validation through Swagger and the Angular UI."

### 10. Scope of This Version

Completed in this version:
- Backend Category CRUD API.
- Frontend Category Management UI.
- Category validation.
- Duplicate category prevention.
- Protection against deleting categories that contain products.
- Frontend and backend integration.

This version focuses only on Category Management. Product Management, Authentication, Cart and Order Management are separate planned features.

**End of Version 1 – Category Management**