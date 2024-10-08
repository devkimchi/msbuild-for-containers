# MSBuild for Containers

As a .NET developer, when you build a container image for your app, you can use the `Dockerfile` to define the container image. However, you can also use the `dotnet publish` command to build and publish the container image without a `Dockerfile`. This repository provides sample .NET apps using container images with `Dockerfile` and with `dotnet publish`.

In addition to that, if you want to orchestrate containers Docker Compose is usually the first approach. However, you can also use the .NET Aspire to generate the Docker Compose file from the .NET Aspire manifest JSON file. This repository also provides a sample .NET app using the .NET Aspire to orchestrate containers.

## Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download/dotnet/8.0?WT.mc_id=dotnet-144884-juyoo) with [.NET Aspire workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling?WT.mc_id=dotnet-144884-juyoo)
- [Visual Studio](https://visualstudio.microsoft.com/?WT.mc_id=dotnet-144884-juyoo) or [Visual Studio Code](https://code.visualstudio.com/?WT.mc_id=dotnet-144884-juyoo) + [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- [Aspirate](https://github.com/prom3theu5/aspirational-manifests)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Getting Started

### Run with `Dockerfile`

1. Run `docker init` to create a new Dockerfile for web app.

    ```bash
    pushd ./MSBuildForContainers.WebApp
    
    docker init
    docker build . -t webapp:latest
    docker run -d -p 3000:8080 webapp:latest
    
    popd
    ```

1. Run `docker init` to create a new Dockerfile for API app.

    ```bash
    pushd ./MSBuildForContainers.ApiApp
    
    docker init
    docker build . -t apiapp:latest
    docker run -d -p 5050:8080 apiapp:latest
    
    popd
    ```

1. Open the browser and navigate to `http://localhost:3000` to see the web app running and `http://localhost:5050` to see the API app running.

### Run without `Dockerfile`

1. Run the following `dotnet publish` command to build and publish the web app.

    ```bash
    dotnet publish ./MSBuildForContainers.WebApp \
        -t:PublishContainer \
        --os linux --arch x64

    docker run -d -p 3000:8080 webapp:latest
    ```

1. Run the following `dotnet publish` command to build and publish the web app.

    ```bash
    dotnet publish ./MSBuildForContainers.ApiApp \
        -t:PublishContainer \
        --os linux --arch x64
    
    docker run -d -p 5050:8080 apiapp:latest
    ```

1. If you want to change the base image to Ubuntu Chiseled image, use the following command.

    ```bash
    dotnet publish ./MSBuildForContainers.ApiApp \
        -t:PublishContainer \
        --os linux --arch x64 \
        -p:ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled \
        -p:ContainerRepository=apiapp \
        -p:ContainerImageTag=latest
    
    dotnet publish ./MSBuildForContainers.WebApp \
        -t:PublishContainer \
        --os linux --arch x64 \
        -p:ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled \
        -p:ContainerRepository=webapp \
        -p:ContainerImageTag=latest

    docker run -d -p 3000:8080 webapp:latest
    docker run -d -p 5050:8080 apiapp:latest
    ```

1. Check Docker Desktop to see the container image size and compare it from the previous step.
1. Open the browser and navigate to `http://localhost:3000` to see the web app running and `http://localhost:5050` to see the API app running.

### Run with Docker Compose

1. Update the web app to reference API app by commenting and uncommenting the `Program.cs` file in the `MSBuildForContainers.WebApp` project.
1. Run the following command to rebuild the container image.

    ```bash
    dotnet publish ./MSBuildForContainers.WebApp \
        -t:PublishContainer \
        --os linux --arch x64 \
        -p:ContainerBaseImage=mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled \
        -p:ContainerRepository=webapp \
        -p:ContainerImageTag=latest
    ```

1. Run the following `docker compose` command to run both apps.

    ```bash
    docker compose -f ./docker-compose.yaml up
    ```

1. Open the browser and navigate to `http://localhost:3000` to see the web app running and `http://localhost:5050` to see the API app running.

## Orchestrate with .NET Aspire

1. Switch to the `aspire` branch.

    ```bash
    git switch aspire
    ```

1. Run the following command to see whether the .NET Aspire dashboard is running

    ```bash
    dotnet watch run --project ./MSBuildForContainers.AppHost
    ```

1. Generate .NET Aspire manifest JSON file.

    ```bash
    dotnet run --project ./MSBuildForContainers.AppHost \
        -- \
        --publisher manifest \
        --output-path ../aspire-manifest.json
    ```

1. Generate the Docker Compose file from the .NET Aspire manifest JSON file.

    ```bash
    aspirate generate \
        --project-path ./MSBuildForContainers.AppHost \
        --aspire-manifest ./aspire-manifest.json \
        --output-format compose \
        --disable-secrets --include-dashboard false
    ```

1. Run the Docker Compose file generated by the .NET Aspire.

    ```bash
    docker compose -f ./aspirate-output/docker-compose.yaml up
    ```

1. Open the browser and navigate to `http://localhost:10002` to see the web app running and `http://localhost:10000` to see the API app running.
