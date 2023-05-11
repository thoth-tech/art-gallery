# Indigenous Art Gallery Project

The the following guide provides instructions on installation using Docker.

Project Specs: .NET 6 API with PostgreSQL v15.1 database and Vue v3.2.13 front end.

## Prerequesites

Download and install Docker Desktop: <https://www.docker.com/products/docker-desktop/>

### Getting Started

1. Fork this repository, and clone the forked repository to your local machine by running
   `git clone https://github.com/<your-username>/art-gallery.git`
2. Navitage to the cloned repository's root directory: `cd <repository-name`>

### Building and Running the Containers

1. Build and run the containers with the following command (ensure you have Docker Desktop open when
   doing so): - `docker-compose up --build`

2. To re-run the container without rebuilding the images use: `docker-compose up`. Once changes have
   been made to the app, the container will need to be rebuilt using `docker-compose build` or
   `docker-compose up --build`.

3. The frontend will be available at `http://localhost`

4. The backend will be available at `http://localhost:7194`, the database will be available on
   `port 5433`

5. To stop and remove the container, run `docker-compose down`

## Repo Structure

```plaintext
  art-gallery
  ├── .github
  │   ├── pull_request_template.md
  │   ├── workflows
  │   │   ├── ci.yml
  ├── .vale
  │   ├── thothtech
  │   ├── vale-json.tmpl
  │   ├── vale.tmpl
  ├── art-gallery-backend
  │   ├── .vs
  │   ├── art-gallery-backend
  │   │   ├── Authentication
  │   │   ├── db_dump
  │   │   ├── Middleware
  │   │   ├── Models
  │   │   ├── Persistence
  │   │   ├── Properties
  │   │   ├── wwwroot
  │   │   ├── appsettings.Development.json
  │   │   ├── appsettings.json
  │   │   ├── art-gallery-backend.csproj
  │   │   ├── art-gallery-backend.csproj.user
  │   │   ├── docker-compose-dev.yml
  │   │   ├── Dockerfile
  │   │   ├── Program.cs
  │   │   ├── README.md
  ├── art-gallery-database
  │   ├── db_sql.txt
  ├── art-gallery-frontend
  │   ├── docs
  │   ├── public
  │   ├── src
  │   ├── babel.config.js
  │   ├── docker-compose-dev.yml
  │   ├── Dockerfile
  │   ├── jsconfig.json
  │   ├── package-lock.json
  │   ├── package.json
  │   ├── README.md
  │   ├── vue.config.js
  ├── .editorconfig
  ├── .gitignore
  ├── .markdownlint.yml
  ├── .prettierignore
  ├── .prettierrc
  ├── docker-compose.yml
  ├── package-lock.json
  ├── README.md
  └── CONTRIBUTING.md
```
