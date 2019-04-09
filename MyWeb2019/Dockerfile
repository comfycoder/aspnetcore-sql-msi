FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MyWeb2019/MyWeb2019.csproj", "MyWeb2019/"]
RUN dotnet restore "MyWeb2019/MyWeb2019.csproj"
COPY . .
WORKDIR "/src/MyWeb2019"
RUN dotnet build "MyWeb2019.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyWeb2019.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWeb2019.dll"]