# Docker Installation Instructions - Written by Chloe Hulme

## Getting Started

1. Fork this repository, and clone the forked repository to your local machine by running `git clone https://github.com/<your-username>/art-gallery-dev.git`
2. Navitage to the cloned repository's root directory: `cd <repository-name`>

### Prerequesites

Download and install Docker Desktop: <https://www.docker.com/products/docker-desktop/>

### Building and Running the Containers

1. Build and run the containers with the following command (ensure you have Docker Desktop open
when doing so):
    - `docker-compose up --build`

2. To re-run the container without rebuilding the images use: `docker-compose up`. Once changes have
been made to the app, the container will need to be rebuilt using `docker-compose build` or
`docker-compose up --build`.

3. The frontend will be available at `http://localhost`

4. The backend will be available at `http://localhost:7194`, the database will be available on `port 5433`

5. To stop and remove the container, run `docker-compose down`
