#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG APP_VERSION="6.13.0"
WORKDIR /src
COPY ["StoreHub.Web.csproj", "."]
RUN dotnet restore "StoreHub.Web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./StoreHub.Web.csproj" -c Release -o /app/build /p:Version=$APP_VERSION

FROM build AS publish
RUN dotnet publish "StoreHub.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:Version=$APP_VERSION

FROM base AS final
WORKDIR /app
ENV STOREHUB_INSTANCE_URL=blah
ENV STOREHUB_INSTANCE_API_KEY=blah
ENV STOREHUB_SUMOLOGIC_URL=blah
ENV STOREHUB_ENVIRONMENT="Production"
ENV STOREHUB_OPEN_FEATURE_CLIENT_ID="blah"
ENV STOREHUB_CONNECTION_STRING=blah
ENV ASPNETCORE_URLS="http://+:8080"
ENV ASPNETCORE_ENVIRONMENT="Production"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreHub.Web.dll"]