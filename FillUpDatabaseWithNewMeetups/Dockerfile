FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FillUpDatabaseWithNewMeetups/FillUpDatabaseWithNewMeetups.csproj", "FillUpDatabaseWithNewMeetups/"]
RUN dotnet restore "FillUpDatabaseWithNewMeetups/FillUpDatabaseWithNewMeetups.csproj"
COPY . .
WORKDIR "/src/FillUpDatabaseWithNewMeetups"
RUN dotnet build "FillUpDatabaseWithNewMeetups.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FillUpDatabaseWithNewMeetups.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FillUpDatabaseWithNewMeetups.dll"]
