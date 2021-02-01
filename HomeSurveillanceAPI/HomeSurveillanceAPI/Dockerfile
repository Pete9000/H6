#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HomeSurveillanceAPI/HomeSurveillanceAPI.csproj", "HomeSurveillanceAPI/"]
RUN dotnet restore "HomeSurveillanceAPI/HomeSurveillanceAPI.csproj"
COPY . .
WORKDIR "/src/HomeSurveillanceAPI"
RUN dotnet build "HomeSurveillanceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeSurveillanceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeSurveillanceAPI.dll"]