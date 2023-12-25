#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln .

COPY ["GameLeaderBoard/GameLeaderBoard.csproj", "GameLeaderBoard/"]
RUN dotnet restore "GameLeaderBoard/GameLeaderBoard.csproj"
COPY . .

WORKDIR /src/GameLeaderBoard
RUN dotnet build

FROM build AS publish
WORKDIR /src/GameLeaderBoard
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GameLeaderBoard.dll"]