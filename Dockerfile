FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /app

# copy everything else and build

COPY . ./
WORKDIR /app/HotelManage.Api
RUN dotnet restore
RUN dotnet publish -c Release -o out

# build runtime image

FROM microsoft/dotnet:2.1-aspnetcore-runtime

WORKDIR /app

COPY --from=build-env /app/HotelManage.Api/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "HotelManage.Api.dll"]