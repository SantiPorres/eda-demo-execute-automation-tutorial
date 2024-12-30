FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../EDA_Inventory/EDA_Inventory.csproj", "../EDA_Inventory/"]
RUN dotnet restore "../EDA_Inventory/EDA_Inventory.csproj"
COPY . .
WORKDIR "/src/../EDA_Inventory"
RUN dotnet build "EDA_Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EDA_Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EDA_Inventory.dll"]
