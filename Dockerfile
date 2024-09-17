FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5009
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5009

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TaskManagerAPI.sln", "."]
COPY ["TaskManagerAPI/TaskManagerAPI.Api.csproj", "TaskManagerAPI/"]
COPY ["TaskManagerAPI.Application/TaskManagerAPI.Application.csproj", "TaskManagerAPI.Application/"]
COPY ["TaskManagerAPI.Contracts/TaskManagerAPI.Contracts.csproj", "TaskManagerAPI.Contracts/"]
COPY ["TaskManagerAPI.Domain/TaskManagerAPI.Domain.csproj", "TaskManagerAPI.Domain/"]
COPY ["TaskManagerAPI.Infrastructure/TaskManagerAPI.Infrastructure.csproj", "TaskManagerAPI.Infrastructure/"]

RUN dotnet restore "TaskManagerAPI/TaskManagerAPI.Api.csproj"
COPY . .

WORKDIR "/src/TaskManagerAPI"
RUN dotnet build "TaskManagerAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagerAPI.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerAPI.Api.dll", "--server.urls", "http://0.0.0.0:5009"]