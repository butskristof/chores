FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj files and restore nuget packages as distinct layers
# to speed up build where only the source code changes
COPY *.sln ./
# all csproj files are copied into the /app root folder
# we look for csproj files three layers deep, make sure to update if
# the solution structure requires deeper lookup
COPY **/*.csproj ./
COPY **/**/**/*.csproj ./
COPY **/**/**/**/*.csproj ./
# all projects in the solution are listed, which prints their path 
# these paths are used to move the csproj files from the root to 
# their correct location 
RUN dotnet sln list | grep ".csproj" \
    | while read -r line; do \
    mkdir -p $(dirname $line); \
    mv $(basename $line) $(dirname $line); \
    done;
RUN dotnet restore

# copy rest of the source code and build
COPY src/. ./src/
WORKDIR /app/src
# publish the API project with Release configuration
RUN dotnet publish -c Release -o /app --no-restore 3-presentation/Api

# create a final stage/image which does not contain the SDK, 
# only the ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# copy the published output from the build stage
COPY --from=build /app ./
# define entrypoint for the application
ENTRYPOINT ["dotnet", "Chores.Api.dll"]

