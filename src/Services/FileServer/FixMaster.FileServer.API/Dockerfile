FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Services/FileServer/FixMaster.FileServer.API/FixMaster.FileServer.API.csproj", "Services/FileServer/FixMaster.FileServer.API/"]
COPY ["src/Common/FixMaster.Common/FixMaster.Common.csproj", "Common/FixMaster.Common/"]

RUN dotnet restore "Services/FileServer/FixMaster.FileServer.API/FixMaster.FileServer.API.csproj"

COPY src/ .
WORKDIR "/src/Services/FileServer/FixMaster.FileServer.API"
RUN dotnet build "FixMaster.FileServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FixMaster.FileServer.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p uploads
ENTRYPOINT ["dotnet", "FixMaster.FileServer.API.dll"]
