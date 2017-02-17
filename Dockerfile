FROM microsoft/aspnetcore
WORKDIR /app
COPY ./bin/Release/netcoreapp1.1/publish/ .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "bjoernerlweinde_core.dll"]


