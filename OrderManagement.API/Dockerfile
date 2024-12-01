# Fase di build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ENV ASPNETCORE_URLS=http://+:80

# Installare curl
RUN apt-get update && apt-get install -y curl && apt-get clean

# Copia i file csproj di tutti i progetti
COPY OrderManagement.Application/OrderManagement.Application.csproj OrderManagement.Application/
COPY OrderManagement.Common/OrderManagement.Common.csproj OrderManagement.Common/
COPY OrderManagement.Core/OrderManagement.Core.csproj OrderManagement.Core/
COPY OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj OrderManagement.Infrastructure/
COPY OrderManagement.API/OrderManagement.API.csproj OrderManagement.API/

# Ripristina le dipendenze
RUN dotnet restore OrderManagement.API/OrderManagement.API.csproj

# Copia tutto il codice sorgente
COPY OrderManagement.Application/ OrderManagement.Application/
COPY OrderManagement.Common/ OrderManagement.Common/
COPY OrderManagement.Core/ OrderManagement.Core/
COPY OrderManagement.Infrastructure/ OrderManagement.Infrastructure/
COPY OrderManagement.API/ OrderManagement.API/

# Compila e pubblica l'applicazione in modalità Release
WORKDIR /src/OrderManagement.API
RUN dotnet publish -c Release -o /app/publish

# Fase di runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrderManagement.API.dll"]
