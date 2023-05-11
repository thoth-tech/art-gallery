# Backend Installation Instructions

## Getting Started

1. Fork this repository, and clone the forked repository to your local machine by running
   `git clone https://github.com/<your-username>/art-gallery-backend.git`
2. Navitage to the cloned repository's root directory: `cd <repository-name`>

## Using Docker

### Prerequesites

Download and install Docker Desktop: <https://www.docker.com/products/docker-desktop/>

### Building and Running the Containers

1. In `appsettings.json` alter the following lines:

   - Under `ConnectionStrings`, remove
     `"PostgresSQL": "Host=localhost;Database=art_gallery_db;Username=postgres;Password=PostgreSQL;"`
   - and replace it with
     `"PostgresSQL": "Server=host.docker.internal;Database=art_gallery_db;User Id=postgres;Port=5433;Password=PostgreSQL;IntegratedSecurity=true;Pooling=true;"`

2. Next, build and run the containers with the following command (ensure you have Docker Desktop
   open when doing so): - `docker-compose -f docker-compose-dev.yml up --build`

3. To re-run containers without rebuilding their images use:
   `docker-compose -f docker-compose-dev.yml up`. Once changes have been made to the app, the
   containers will need to be rebuilt using `docker-compose -f docker-compose-dev.yml build` or
   `docker-compose -f docker-compose-dev.yml up --build`.

4. The backend will be available at `http://localhost:7194`, the database will be available on
   `port 5433`

5. To stop and remove the containers, run `docker-compose -f docker-compose-dev.yml down`

## From Source Code

### Setting up PostgreSQL

This project uses Postgres v15.1. A full installation guide can be found at
<https://www.postgresql.org/docs/current/tutorial-start.html>

#### PostgreSQL Installation Steps

- Download Postgres on your desired platform at <https://www.postgresql.org/download/.>
- Follow the installation wizard dialog, when prompted for a password use **‘PostgreSQL’**. Postgres
  does offer an in-house open source administration and development platform called pgAdmin. Other
  alternatives to this include Azure Database Studio and DBeaver.
- Once Installed, a prompt to launch Stack Builder will display, select yes. This will ensure you
  have the adequate drivers, which in our case in under **Categories > Database Drivers > Npgsql
  v3.2.6-3**, or whichever is most current at the time of installation.
- Once Stack Builder has finished its installation process, close the application, and launch
  pgAdmin.
- pgAdmin may prompt for a master password, this can be left blank or set the same as the postgres
  user (i.e., ‘PostgreSQL’) for simplicity.
- On the left-hand side (under Browser) collapse the Server tab, followed by PostgreSQL 15. You will
  be prompted to enter as password, enter **‘PostgreSQL’** (this is the same password we set for the
  postgres user during installation). Right click on Databases and select **Create > Database…**
- Enter the following details: [Database: art_gallery_db, Owner: postgres, Comment: Thoth Tech Art
  Gallery Project]. Then select **“Save”**.
- Next, depending on the operating system, you may be required to link the binary paths for pgAdmin.
  This can be done by selecting **Files > Preference > Paths > Binary Paths** and setting the bottom
  PostgreSQL Binary Path for version 15 to **‘C:\Program Files\PostgreSQL\15\bin’** or your
  installation directory equivalent.
- Next, right click on the newly created database and select Restore.
- In the `art-gallery-backend` repository acquired earlier, there is an `art-gallery-database`
  folder that contains the database schema. Select the schema that best suits your setup
  requirements. For the purpose of this document, we will select the schema that includes sample
  data: `art-gallery-db-sampledata.sql`.
- Select Restore, Right click on the database and select Refresh. Collapse the **art_gallery_db >
  Schemas > Tables**. There should be 7 tables all populated with sample data.
- Congratulations, the database is now setup and running!

### Setting up Visual Studio 2022

Visual Studio 2022 is the recommended IDE for the project. The project was built using .NET6,
however there is a newer version available. A full in-depth guide for installing .NET7 for a variety
of platforms can be found at
<https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net70>

#### VS Installation Steps

- Download Visual Studio 2022 at <https://visualstudio.microsoft.com/vs/community/>.
- Follow the installation prompts and be sure to select the ASP.NET package under Web & Cloud.
- Navigate to the `art-gallery-backend.sln` solution file in the download destination for step one.
  For me this was
  **"D:\Development\DDGCIT-art-gallery\art-gallery-backend\art-gallery-backend\art-gallery-backend.sln"**.
- In `appsettings.json` alter the following lines:
  1. Under `ConnectionStrings`, remove
     `"PostgresSQL": "Server=host.docker.internal;Database=art_gallery_db;User Id=postgres;Port=5433;Password=PostgreSQL;IntegratedSecurity=true;Pooling=true;"`
  2. and replace it with
     `"PostgresSQL": "Host=localhost;Database=art_gallery_db;Username=postgres;Password=PostgreSQL;"`
- Open the solution, then build and the run the application.
- You may get an error depending on the speed of your system during the first build – This will be
  due to packages that have not yet installed, give NuGet restore a few minutes to complete this
  process.
- Congratulations, the backend API is now setup and running!
