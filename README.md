# Buildberry

A .NET 9 Minimal API sandbox project for testing development workflows with BlackBerry Key2.

## Project Overview

Buildberry exposes a clean API for retrieving GitHub Actions workflow runs. The project is designed with a focus on:

- Clean project structure with separation of concerns
- Dependency injection for extensibility
- Service layer abstraction for easy GitHub API integration
- Hardcoded sample data for initial testing

## Getting Started

### Prerequisites

- .NET 9 SDK

### Building and Running

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at `http://localhost:5000`.

## API Endpoints

### GET /api/builds

Returns a list of GitHub Actions workflow runs.

**Response:**
```json
[
  {
    "repository": "buildberry",
    "workflow": "CI",
    "status": "success",
    "updatedAt": "2026-06-09T12:00:00Z"
  }
]
```

## Project Structure

```
Buildberry/
├── Models/
│   └── WorkflowRun.cs
├── Services/
│   ├── IBuildsService.cs
│   └── BuildsService.cs
├── Controllers/
│   └── BuildsController.cs
├── Program.cs
└── Buildberry.csproj
```

## Future Enhancements

- GitHub API integration for real workflow data
- Authentication and authorization
- Filtering and pagination
- Caching layer

*using neovim on blackberry key2
