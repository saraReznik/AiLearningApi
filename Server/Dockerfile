# Dockerfile (ממוקם ב-backend/Server)
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build 

WORKDIR /src/backend

COPY ["backend/Server/Server.sln", "Server/"]
COPY ["backend/Server/", "Server/"]
COPY ["backend/BL/", "BL/"]
COPY ["backend/Dal/", "Dal/"]

WORKDIR /src/backend/Server

RUN dotnet restore "Server.sln"

RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS final 

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 7120

ENTRYPOINT ["dotnet", "Server.dll"]