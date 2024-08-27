# ------------------------------------------------------------------------------------------------
# Obtener la imagen oficial de docker hub https://hub.docker.com/r/microsoft/dotnet-sdk, 
# y lo renombra como build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Le coloca el nombre a la aplicación
WORKDIR /app2
# Expone los puertos
EXPOSE 8080
EXPOSE 8081
# ------------------------------------------------------------------------------------------------
# Construye la imagen para release
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src2
# ------------------------------------------------------------------------------------------------
# Copia los archivos del proyecto a la ruta del proyecto 
COPY ./*.csproj ./
# Verifica que el archivo del proyecto .csproj tenga todas las depndencias del proyecto
RUN dotnet restore 
# Copia el resto de los archivos del proyecto a la ruta del proyecto
COPY . .
WORKDIR /src2
RUN dotnet build "banobras-bitacoras-persistence.csproj" -c $BUILD_CONFIGURATION -o /app2/build
# ------------------------------------------------------------------------------------------------
# Obtiene lo de build y lo renombra como publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Obtiene todos los artefactos que docker necesita para ejecutar la aplicación, toma las dlls
RUN dotnet publish "banobras-bitacoras-persistence.csproj" -c $BUILD_CONFIGURATION -o /app2/publish
# ------------------------------------------------------------------------------------------------
# Obtiene lo de base y lo renombra como finish
FROM base AS final
WORKDIR /app2
# Toma el build que ahora se llama publish y lo pasa a la carpeta publish para publicar
COPY --from=publish /app2/publish .
ENTRYPOINT ["dotnet", "banobras-bitacoras-persistence.dll"]
# ------------------------------------------------------------------------------------------------

