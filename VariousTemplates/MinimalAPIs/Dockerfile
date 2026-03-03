FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MinimalAPIs/MinimalAPIs.csproj", "MinimalAPIs/"]
RUN dotnet restore "MinimalAPIs/MinimalAPIs.csproj"
COPY . .
WORKDIR "/src/MinimalAPIs"
RUN dotnet build "MinimalAPIs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalAPIs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalAPIs.dll"]