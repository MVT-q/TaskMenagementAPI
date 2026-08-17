# Task Management API

## Description

API for managing projects and their tasks.

This application was built to practice backend development, working with Entity Framework Core queries and general backend development concepts.

## Features

- User registration and authentication
- JWT authentication
- CRUD operations for projects and tasks
- Adding users to projects
- Assigning project members to tasks
- Changing task status by the assigned user
- Task filtering, search, sorting and pagination
- `INotificationService` for action notifications 

## Technologies

- C#
- ASP.NET
- SQL Server
- Entity Framework Core
- JWT

## Authentication

After registration, use the login endpoint to enter the username and password of the created account. Then copy the returned JWT token and paste it into `Authorize`.

## Endpoints

| Method | Endpoint             | Description |
| ------ | -------------------- | ----------- |
| `POST` | `/api/Auth/login`    | Login       |
| `POST` | `/api/Auth/register` | Register    |

---

| Method   | Endpoint                                     | Description                |
| -------- | -------------------------------------------- | -------------------------- |
| `GET`    | `/api/projects/{projectId}`                  | Get project by ID          |
| `PUT`    | `/api/projects/{projectId}`                  | Update project             |
| `DELETE` | `/api/projects/{projectId}`                  | Delete project             |
| `POST`   | `/api/projects`                              | Create project             |
| `GET`    | `/api/projects`                              | Get user's projects        |
| `GET`    | `/api/projects/{projectId}/members`          | Get project members        |
| `POST`   | `/api/projects/{projectId}/members`          | Add member to project      |
| `GET`    | `/api/projects/{projectId}/members/{userId}` | Get project member         |
| `DELETE` | `/api/projects/{projectId}/members/{userId}` | Remove member from project |
| `PATCH`  | `/api/projects/{projectId}/members/{userId}` | Change member role         |

---

| Method   | Endpoint                                        | Description             |
| -------- | ----------------------------------------------- | ----------------------- |
| `GET`    | `/projects/{projectId}/tasks/{taskId}`          | Get task by ID          |
| `PUT`    | `/projects/{projectId}/tasks/{taskId}`          | Update task             |
| `DELETE` | `/projects/{projectId}/tasks/{taskId}`          | Delete task             |
| `POST`   | `/projects/{projectId}/tasks`                   | Create task             |
| `GET`    | `/projects/{projectId}/tasks`                   | Get project tasks       |
| `PATCH`  | `/projects/{projectId}/tasks/{taskId}/status`   | Update task status      |
| `PATCH`  | `/projects/{projectId}/tasks/{taskId}/priority` | Update task priority    |
| `PATCH`  | `/projects/{projectId}/tasks/{taskId}/due-date` | Update task due date    |
| `PATCH`  | `/projects/{projectId}/tasks/{taskId}/assignee` | Assign or unassign task |


## Query Parameters

The `GET /projects/{projectId}/tasks` endpoint supports the following query parameters:

| Parameter    | Description								 |
| ------------ | ------------------------------------------- |
| `search`     | Searches tasks by title					 |
| `status`     | Filters tasks by status					 |
| `priority`   | Filters tasks by priority					 |
| `sortBy`     | Sorts tasks by status, priority or due date |
| `descending` | Changes sorting direction					 |
| `page`       | Page number								 |
| `pageSize`   | Number of tasks per page (1–100)            |

---

Example:

`GET /projects/1/tasks?search=API&priority=High&sortBy=Priority&descending=true&page=1&pageSize=20`

## How to Run

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Configure the required JWT secret using User Secrets.
4. Apply the database migrations.
5. Build and run the application.
6. Register an account and log in.
7. Copy the returned JWT token and paste it into Swagger's `Authorize` dialog.

## Configuration

The application uses `appsettings.json` for general configuration.

JWT configuration contains the following settings:

- Issuer
- Audience

The JWT signing key is stored using ASP.NET Core User Secrets and is not stored in `appsettings.json`.

The application also uses a SQL Server connection string configured under `ConnectionStrings:DefaultConnection`.

For local development, configure the required JWT secret using User Secrets before running the application.

## Author

Yehor Radykop