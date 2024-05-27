FROM node:lts-iron AS build-spa
ARG SPA_PATH=./app
WORKDIR /app
RUN chown node:node ./
USER node

COPY ${SPA_PATH}/package*.json ./
RUN npm ci
COPY ${SPA_PATH}/. .
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG BFF_PATH=./bff
WORKDIR /src
COPY ["${BFF_PATH}/Chores.Bff.csproj", "./"]
RUN dotnet restore "Chores.Bff.csproj"
COPY ${BFF_PATH}/. .
COPY --from=build-spa /app/dist ./wwwroot
WORKDIR "/src/"
RUN dotnet build "Chores.Bff.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Chores.Bff.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chores.Bff.dll"]
