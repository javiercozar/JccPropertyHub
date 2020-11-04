FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

#COPY ["*.sln",""]
#COPY ["JccPropertyHub.Domain.Core/JccPropertyHub.Domain.Core.csproj", "JccPropertyHub.Domain.Core/JccPropertyHub.Domain.Core.csproj"]
#COPY ["JccPropertyHub.Domain.Infraestructure/JccPropertyHub.Domain.Infraestructure.csproj", "JccPropertyHub.Domain.Infraestructure/JccPropertyHub.Domain.Infraestructure.csproj"]
#COPY ["JccProperty.Domain.HotelJccConnector/JccProperty.Domain.HotelJccConnector.csproj", "JccPropertyHub.Domain.HotelJccConnector/JccProperty.Domain.HotelJccConnector.csproj"]
#COPY ["JccProperty.Domain.HotelLegsConnector/JccProperty.Domain.HotelLegsConnector.csproj","JccPropertyHub.Domain.HotelLegsConnector/JccProperty.Domain.HotelLegsConnector.csproj"]
#COPY ["JccPropertyHub.WebApi/JccPropertyHub.WebApi.csproj" ,"JccPropertyHub.WebApi/JccPropertyHub.WebApi.csproj"]
#RUN dotnet restore JccPropertyHub.Domain.Core/JccPropertyHub.Domain.Core.csproj
#RUN dotnet restore JccPropertyHub.Domain.Infraestructure/JccPropertyHub.Domain.Infraestructure.csproj
#RUN dotnet restore JccPropertyHub.Domain.HotelJccConnector/JccProperty.Domain.HotelJccConnector.csproj
#RUN dotnet restore JccPropertyHub.Domain.HotelLegsConnector/JccProperty.Domain.HotelLegsConnector.csproj
#RUN dotnet restore JccPropertyHub.WebApi/JccPropertyHub.WebApi.csproj

COPY . .
RUN dotnet restore 
 
#COPY . ./
RUN dotnet publish JccPropertyHub.WebApi/JccPropertyHub.WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
EXPOSE 80
EXPOSE 443
WORKDIR /app
ENV ASPNETCORE_URLS="http://+"
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "JccPropertyHub.WebApi.dll"]