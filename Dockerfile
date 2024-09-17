FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /src

COPY ["TaskManagerAPI.sln", "."]
COPY ["TaskManagerAPI/TaskManagerAPI.Api.csproj", "TaskManagerAPI/"]
COPY ["TaskManagerAPI.Application/TaskManagerAPI.Application.csproj", "TaskManagerAPI.Application/"]
COPY ["TaskManagerAPI.Contracts/TaskManagerAPI.Contracts.csproj", "TaskManagerAPI.Contracts/"]
COPY ["TaskManagerAPI.Domain/TaskManagerAPI.Domain.csproj", "TaskManagerAPI.Domain/"]
COPY ["TaskManagerAPI.Infrastructure/TaskManagerAPI.Infrastructure.csproj", "TaskManagerAPI.Infrastructure/"]

EXPOSE 5009
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5009

RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.Api.csproj"

WORKDIR "/src/TaskManagerAPI"
COPY . .

RUN dotnet build "TaskManagerAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagerAPI.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerAPI.Api.dll"]
