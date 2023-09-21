FROM hub.nexdev.net/base/dotnet/core/sdk:7.0 AS builder
WORKDIR /src

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Business ./Fanex.BetBuilder.Business
COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.BetBuilder.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/rebuild-baseimage/dotnetcore/aspnet:7.0-nap-mail-core



# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

ENV TZ="America/La_Paz" 

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.BetBuilder.Service.dll"]
