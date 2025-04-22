FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Business ./Fanex.BetBuilder.Business
COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service.csproj

WORKDIR /src/Fanex.BetBuilder.Service

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n -r linux-x64

COPY ./Fanex.BetBuilder.Service/Fanex.BetBuilder.Service .

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-nap-mail-core

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.BetBuilder.Service.dll"]
