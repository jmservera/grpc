#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
# libdl.so workaround as per https://github.com/grpc/grpc/issues/24153
RUN apt-get update && apt-get install -y libc-dev && apt-get clean
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GreeterClient/GreeterClient.csproj", "GreeterClient/"]
COPY ["Greeter/Greeter.csproj", "Greeter/"]
RUN dotnet restore "GreeterClient/GreeterClient.csproj"
COPY . .
WORKDIR "/src/GreeterClient"
RUN dotnet build "GreeterClient.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/GreeterClient"
RUN dotnet publish "GreeterClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreeterClient.dll"]
