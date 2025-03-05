# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
# Core
COPY --link ./Habits.Core/Habits.Core.csproj ./Habits.Core/.
RUN dotnet restore ./Habits.Core -a $TARGETARCH

# Application
COPY --link ./Habits.Application/Habits.Application.csproj ./Habits.Application/.
RUN dotnet restore ./Habits.Application -a $TARGETARCH

# Infrastructure
COPY --link ./Habits.Infrastructure/Habits.Infrastructure.csproj ./Habits.Infrastructure/.
RUN dotnet restore ./Habits.Infrastructure -a $TARGETARCH

# WebApi
COPY --link ./Habits.WebApi/Habits.WebApi.csproj ./Habits.WebApi/.
RUN dotnet restore ./Habits.WebApi -a $TARGETARCH

# Copy source code and publish app
COPY --link . .
RUN dotnet publish ./Habits.WebApi -a $TARGETARCH --no-restore -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
EXPOSE 8080
WORKDIR /app
COPY --link --from=build /app .
USER $APP_UID
ENTRYPOINT ["./Habits.WebApi"]