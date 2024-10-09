FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OctoGoat/OctoGoat.csproj", "OctoGoat/"]
RUN dotnet restore "OctoGoat/OctoGoat.csproj"
COPY . .
WORKDIR "/src/OctoGoat"
RUN dotnet build "OctoGoat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OctoGoat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "OctoGoat.dll"]

COPY entrypoint.sh .
RUN apt-get update
RUN apt-get install -y curl


#RUN mkdir /etc/crontabs
#RUN touch /etc/crontabs/root
RUN chmod +x entrypoint.sh
#RUN echo -e "\n* * * * * curl -v -X POST -H \"CRON: \$HOSTNAME\" localhost:8080/populate?aaa=89w37f4yj5c9q8w3y4560n9f78c23y4cf6098234yd6o9x87y3456" >> /etc/crontabs/root
ENTRYPOINT ["./entrypoint.sh"]
