# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["test/Hangfire.JobManagement.Test.SampleServer/Hangfire.JobManagement.Test.SampleServer.csproj", "test/Hangfire.JobManagement.Test.SampleServer/"]

RUN dotnet restore "./test/Hangfire.JobManagement.Test.SampleServer/Hangfire.JobManagement.Test.SampleServer.csproj"
COPY . .

WORKDIR "/src/test/Hangfire.JobManagement.Test.SampleServer"

RUN dotnet build "./Hangfire.JobManagement.Test.SampleServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hangfire.JobManagement.Test.SampleServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hangfire.JobManagement.Test.SampleServer.dll"]