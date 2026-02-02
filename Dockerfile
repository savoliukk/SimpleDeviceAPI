FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SimpleDeviceAPI.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrations
WORKDIR /src
COPY SimpleDeviceAPI.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet tool install --tool-path /tools dotnet-ef
ENV PATH="/tools:${PATH}"

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "SimpleDeviceAPI.dll"]
