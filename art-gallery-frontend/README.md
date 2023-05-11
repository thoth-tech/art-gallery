# Frontend Installation Instructions

## Getting Started

1. Fork this repository, and clone the forked repository to your local machine by running
   `git clone https://github.com/<your-username>/art-gallery-frontend.git`
2. Navitage to the cloned repository's root directory: `cd <repository-name`>

## Using Docker

### Prerequesites

Download and install Docker Desktop: <https://www.docker.com/products/docker-desktop/>

### Building and Running the Containers

1. Build and run the container with the following command (ensure you have Docker Desktop open when
   doing so): - `docker-compose -f docker-compose-dev.yml up --build`

2. To re-run the container without rebuilding the images use:
   `docker-compose -f docker-compose-dev.yml up`. Once changes have been made to the app, the
   container will need to be rebuilt using `docker-compose -f docker-compose-dev.yml build` or
   `docker-compose -f docker-compose-dev.yml up --build`.

3. The frontend will be available at `http://localhost`

4. To stop and remove the container, run `docker-compose -f docker-compose-dev.yml down`
