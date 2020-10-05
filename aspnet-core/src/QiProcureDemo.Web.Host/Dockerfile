#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/QiProcureDemo.Web.Host/QiProcureDemo.Web.Host.csproj", "src/QiProcureDemo.Web.Host/"]
COPY ["src/QiProcureDemo.Web.Core/QiProcureDemo.Web.Core.csproj", "src/QiProcureDemo.Web.Core/"]
COPY ["src/QiProcureDemo.Application/QiProcureDemo.Application.csproj", "src/QiProcureDemo.Application/"]
COPY ["src/QiProcureDemo.Application.Shared/QiProcureDemo.Application.Shared.csproj", "src/QiProcureDemo.Application.Shared/"]
COPY ["src/QiProcureDemo.Core.Shared/QiProcureDemo.Core.Shared.csproj", "src/QiProcureDemo.Core.Shared/"]
COPY ["src/QiProcureDemo.Core/QiProcureDemo.Core.csproj", "src/QiProcureDemo.Core/"]
COPY ["src/QiProcureDemo.EntityFrameworkCore/QiProcureDemo.EntityFrameworkCore.csproj", "src/QiProcureDemo.EntityFrameworkCore/"]
COPY ["src/QiProcureDemo.GraphQL/QiProcureDemo.GraphQL.csproj", "src/QiProcureDemo.GraphQL/"]
RUN dotnet restore "src/QiProcureDemo.Web.Host/QiProcureDemo.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/QiProcureDemo.Web.Host"
RUN dotnet build "QiProcureDemo.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QiProcureDemo.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QiProcureDemo.Web.Host.dll"]
