#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Web/GarageBuddy.Web/GarageBuddy.Web.csproj", "Web/GarageBuddy.Web/"]
COPY ["GarageBuddy.Common/GarageBuddy.Common.csproj", "GarageBuddy.Common/"]
COPY ["Data/GarageBuddy.Data.Models/GarageBuddy.Data.Models.csproj", "Data/GarageBuddy.Data.Models/"]
COPY ["Data/GarageBuddy.Data.Common/GarageBuddy.Data.Common.csproj", "Data/GarageBuddy.Data.Common/"]
COPY ["Data/GarageBuddy.Data/GarageBuddy.Data.csproj", "Data/GarageBuddy.Data/"]
COPY ["Services/GarageBuddy.Services.Data/GarageBuddy.Services.Data.csproj", "Services/GarageBuddy.Services.Data/"]
COPY ["Services/GarageBuddy.Services.Data.Models/GarageBuddy.Services.Data.Models.csproj", "Services/GarageBuddy.Services.Data.Models/"]
COPY ["Services/GarageBuddy.Services.Mapping/GarageBuddy.Services.Mapping.csproj", "Services/GarageBuddy.Services.Mapping/"]
COPY ["Services/GarageBuddy.Services.Messaging/GarageBuddy.Services.Messaging.csproj", "Services/GarageBuddy.Services.Messaging/"]
COPY ["Web/GarageBuddy.Web.Infrastructure/GarageBuddy.Web.Infrastructure.csproj", "Web/GarageBuddy.Web.Infrastructure/"]
COPY ["Web/GarageBuddy.Web.ViewModels/GarageBuddy.Web.ViewModels.csproj", "Web/GarageBuddy.Web.ViewModels/"]
RUN dotnet restore "Web/GarageBuddy.Web/GarageBuddy.Web.csproj"
COPY . .
WORKDIR "/src/Web/GarageBuddy.Web"
RUN dotnet build "GarageBuddy.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GarageBuddy.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GarageBuddy.Web.dll"]