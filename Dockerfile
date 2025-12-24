# Sử dụng hình ảnh SDK để build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy file dự án vào thư mục hiện tại của Docker
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ code còn lại và build
COPY . ./
RUN dotnet publish -c Release -o out

# Sử dụng hình ảnh Runtime để chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Cấu hình cổng cho Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MyFirstBackend.dll"]