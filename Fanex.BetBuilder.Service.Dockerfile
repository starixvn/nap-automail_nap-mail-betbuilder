ARG CI_REGISTRY_IMAGE
FROM $CI_REGISTRY_IMAGE:builder as deps
RUN mkdir -p /src/packages

FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

ENV NUGET_PACKAGES=/src/packages
COPY --from=deps $NUGET_PACKAGES/. $NUGET_PACKAGES/

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Business ./Fanex.BetBuilder.Business
COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj

WORKDIR /src/Fanex.BetBuilder.Service

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n -r linux-x64 

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service .

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-nap-mail-core

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0


WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.BetBuilder.Service.dll"]
