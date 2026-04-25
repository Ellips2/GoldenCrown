FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build 
WORKDIR /src 
COPY ["GoldenCrown.API/*.csproj", "./GoldenCrown.API/"] 
COPY ["GoldenCrown.Application/*.csproj", "./GoldenCrown.Application/"] 
COPY ["GoldenCrown.Domain/*.csproj", "./GoldenCrown.Domain/"] 
COPY ["GoldenCrown.Infrastructure/*.csproj", "./GoldenCrown.Infrastructure/"] 
RUN dotnet restore "./GoldenCrown.API/GoldenCrown.API.csproj" 
COPY . .
RUN dotnet build "./GoldenCrown.API/GoldenCrown.API.csproj" -c Release -o /app/build

FROM build AS publish 
RUN dotnet publish "./GoldenCrown.API/GoldenCrown.API.csproj" -c Release -o /app/publish /p:UseAppHost=false 

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final 
WORKDIR /app 
EXPOSE 8080
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "GoldenCrown.API.dll"]
