#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/QiProcureDemo.Migrator/QiProcureDemo.Migrator.csproj", "src/QiProcureDemo.Migrator/"]
COPY ["src/QiProcureDemo.EntityFrameworkCore/QiProcureDemo.EntityFrameworkCore.csproj", "src/QiProcureDemo.EntityFrameworkCore/"]
COPY ["src/QiProcureDemo.Core/QiProcureDemo.Core.csproj", "src/QiProcureDemo.Core/"]
COPY ["src/QiProcureDemo.Core.Shared/QiProcureDemo.Core.Shared.csproj", "src/QiProcureDemo.Core.Shared/"]
RUN dotnet restore "src/QiProcureDemo.Migrator/QiProcureDemo.Migrator.csproj"
COPY . .
WORKDIR "/src/src/QiProcureDemo.Migrator"
RUN dotnet build "QiProcureDemo.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QiProcureDemo.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QiProcureDemo.Migrator.dll"]
