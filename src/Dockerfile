FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG CONFIGURATION=Debug
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c ${CONFIGURATION} -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
EXPOSE 8080
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Habits.gRPC.dll"]