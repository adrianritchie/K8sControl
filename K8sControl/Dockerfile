# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY K8sControl/*.csproj ./K8sControl/
RUN dotnet restore

# copy everything else and build app
COPY K8sControl/. ./K8sControl/
WORKDIR /source/K8sControl
#RUN dotnet publish -c release -o /app --no-restore
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "K8sControl.dll"]
