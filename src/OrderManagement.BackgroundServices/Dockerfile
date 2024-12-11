# Fase di build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia i file csproj di tutti i progetti
COPY src/OrderManagement.Application/OrderManagement.Application.csproj src/OrderManagement.Application/
COPY src/OrderManagement.Common/OrderManagement.Common.csproj src/OrderManagement.Common/
COPY src/OrderManagement.Core/OrderManagement.Core.csproj src/OrderManagement.Core/
COPY src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj src/OrderManagement.Infrastructure/
COPY src/OrderManagement.BackgroundServices/OrderManagement.BackgroundServices.csproj src/OrderManagement.BackgroundServices/

# Ripristina le dipendenze
RUN dotnet restore OrderManagement.BackgroundServices/OrderManagement.BackgroundServices.csproj

# Copia tutto il codice sorgente
COPY src/OrderManagement.Application/ src/OrderManagement.Application/
COPY src/OrderManagement.Common/ src/OrderManagement.Common/
COPY src/OrderManagement.Core/ src/OrderManagement.Core/
COPY src/OrderManagement.Infrastructure/ src/OrderManagement.Infrastructure/
COPY src/OrderManagement.BackgroundServices/ src/OrderManagement.BackgroundServices/

# Compila e pubblica l'applicazione in modalità Release
WORKDIR /src/OrderManagement.BackgroundServices
RUN dotnet publish -c Release -o /app/publish

# Fase di runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrderManagement.BackgroundServices.dll"]
