# Thoth Tech - Art Gallery Project

Project Title: Dockerisation of the Indigenous Art Gallery Project.

**_Handover Date: 21/5/23_**

## Introduction

This project handover document outlines the key information and deliverables of the project
'Dockerisation of the Indigenous Art Gallery Project' to facilitate a smooth transition of
responsibilities from the current project team to the receiving party. It provides an overview of
the project, its objectives, and important contacts for future reference.

### Project Overview

- Project Description: The research, completion and documentation of the Dockerisation of the
  Indigenous Art Gallery Project. Intended to benefit future developers of the project by creating a
  containerised development environment, and ultimately working towards deployment in a Docker swarm
  when appropriate.
- Project Objectives: To containerise the existing Indigenous Art Gallery Project using Docker, with
  the ultimate aim of having the frontend, backend, and database running in isolated containers,
  communicating via a network bridge.
- Project Duration: T1, 2023 (10 weeks: 6/3/23 - 21/5/23)
- Project Team: Chloe Hulme (Delivery Lead - <chulme@deakin.edu.au>)

### Project Deliverables

The following deliverables have been completed or are currently in progress:

- Fully documented Docker research notes
- Fully operational .NET 6 backend container, using custom image
- Fully operational Vue v3.2.13 frontend container, using custom image
- Fully operational PostgreSQL v15.1 database container, using PostgreSQL latest image
- Fully operational application running in bridged containers
- Development docker-compose files to isolate front and back end containers
- Full project documentation including README.md's
- Full CONTRIBUTING.md guide instructing on how to contribute to the project correctly

### Key Project Documentation

The following project documentation is included in this handover:

- [DockerizationFeasabilityCheck.md](https://github.com/thoth-tech/documentation/blob/main/docs/ArtGallery/Dockerization/DockerizationFeasabilityCheck.md)
- [DockerizationProjectOutline.md](https://github.com/thoth-tech/documentation/blob/main/docs/ArtGallery/Dockerization/DockerizationProjectOutline.md)
- [DockerizationResearchNotes.md](https://github.com/thoth-tech/documentation/blob/main/docs/ArtGallery/Dockerization/DockerizationResearchNotes.md)
- [Frontend README.md](https://github.com/thoth-tech/art-gallery/blob/development/art-gallery-frontend/README.md)
- [Backend README.md](https://github.com/thoth-tech/art-gallery/blob/development/art-gallery-backend/art-gallery-backend/README.md)
- [Full README.md](https://github.com/thoth-tech/art-gallery/blob/development/README.md)
- [CONTRIBUTING.md](https://github.com/thoth-tech/art-gallery/blob/development/CONTRIBUTING.md)

### Project Contacts

For any inquiries or clarifications related to the project, the following contacts can be reached:

- Delivery Lead: Chloe Hulme (<chulme@deakin.edu.au>)
- Product Lead: Daniel Maddern (<maddernd@deakin.edu.au>)

### Outstanding Tasks

The following tasks or issues require attention or completion:

- Creation of VS Code dev containers using the created Docker containers, to further enhance the
  development environment for future members of the Indigenous Art Gallery Project development team.

### Additional Comments

The Indigenous Art Gallery Project is trialing a Ruby/Angular implementation from T2. The outcome of
this trial will determine the lifespan of the current containers.
