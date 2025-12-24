FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Chỉ copy file project và restore (Không dùng thư mục con)
COPY MyFirstBackend.csproj ./
RUN dotnet restore

# Copy toàn bộ file còn lại vào thư mục gốc của Docker
COPY . ./

# Build trực tiếp file csproj thay vì file sln để tránh lỗi đường dẫn cũ
RUN dotnet publish MyFirstBackend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MyFirstBackend.dll"]