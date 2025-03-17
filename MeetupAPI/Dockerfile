FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MeetupAPI.csproj", "."]
RUN dotnet restore "MeetupAPI.csproj"
COPY . .
RUN dotnet build "MeetupAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeetupAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeetupAPI.dll"]