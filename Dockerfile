FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ./Dimer/*.csproj ./
RUN dotnet restore

COPY ./Dimer/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY ./Dimer/appsettings.*.json ./
COPY ./Dimer/appsettings.json ./
ENTRYPOINT ["dotnet", "Dimer.dll"]