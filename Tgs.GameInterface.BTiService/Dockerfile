#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
RUN dotnet nuget remove source 'Microsoft Visual Studio Offline Packages' || true
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.BTiService/Tgs.GameInterface.BTiService.csproj", "Tgs.GameInterface.BTiService/"]
COPY ["Tgs.GameInterface.BTiService.Core/Tgs.GameInterface.BTiService.Core.csproj", "Tgs.GameInterface.BTiService.Core/"]
RUN dotnet restore "Tgs.GameInterface.BTiService/Tgs.GameInterface.BTiService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.BTiService"
RUN dotnet build "Tgs.GameInterface.BTiService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.BTiService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.BTiService.dll"]