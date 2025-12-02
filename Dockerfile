# ===== Build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src    

# Copia solo el csproj y restaura (mejor cache)
COPY Lot/Lot.csproj Lot/
RUN dotnet restore Lot/Lot.csproj

# Copia el resto del repo
COPY . .

# Publica
RUN dotnet publish Lot/Lot.csproj -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Usuario no root
RUN useradd -m appuser
USER appuser

# Escucha en 0.0.0.0:8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Copia la app publicada
COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "Lot.dll"]
