#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HomeSurveillanceApp/HomeSurveillanceApp.csproj", "HomeSurveillanceApp/"]
RUN dotnet restore "HomeSurveillanceApp/HomeSurveillanceApp.csproj"
COPY . .
WORKDIR "/src/HomeSurveillanceApp"
RUN dotnet build "HomeSurveillanceApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeSurveillanceApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeSurveillanceApp.dll"]