FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ComparadorTipoCambio.csproj", "ComparadorTipoCambio/"]
RUN dotnet restore "ComparadorTipoCambio/ComparadorTipoCambio.csproj"

COPY . /src/ComparadorTipoCambio
WORKDIR /src/ComparadorTipoCambio

RUN dotnet publish "ComparadorTipoCambio.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ComparadorTipoCambio.dll"]


#docker run comparador-tipo-cambio