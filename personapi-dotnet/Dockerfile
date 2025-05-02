# ---> build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# 1) Copiamos el .csproj y restauramos
COPY *.csproj ./
RUN dotnet restore

# 2) Copiamos todo y publicamos en /app
COPY . ./
RUN dotnet publish -c Release -o /app

# ---> runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# 3) Copiamos los binarios publicados
COPY --from=build-env /app ./

# 4) Configuramos la URL y el puerto
ENV ASPNETCORE_URLS=http://+:5062
EXPOSE 5062

# 5) Punto de entrada
ENTRYPOINT ["dotnet", "personapi-dotnet.dll"]
