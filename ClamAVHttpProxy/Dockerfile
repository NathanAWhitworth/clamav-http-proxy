FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV DEBIAN_FRONTEND=noninteractive

RUN apt update && \
    apt install -y curl && \
    rm -rf /var/lib/apt/lists/*

HEALTHCHECK --interval=10s --timeout=10s --retries=3 \
    CMD curl -f http://localhost/healthz || exit 1

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ClamAVHttpProxy.dll"]