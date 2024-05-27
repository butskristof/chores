FROM node:lts-iron AS build-spa
ARG SPA_PATH=./app
WORKDIR /app
RUN chown node:node ./
USER node

COPY ${SPA_PATH}/package*.json ./
RUN npm ci
COPY ${SPA_PATH}/. .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-bff
ARG BFF_PATH=./bff
WORKDIR /src

COPY ${BFF_PATH}/Chores.Bff.csproj ./
RUN dotnet restore
COPY ${BFF_PATH}/. .
COPY --from=build-spa /app/dist ./wwwroot
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-bff /app/publish .
ENTRYPOINT ["dotnet", "Chores.Bff.dll"]
