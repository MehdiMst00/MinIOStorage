# MinIO S3 Storage ([min.io](https://min.io/))

## Screenshots
<p>
  <a href="https://raw.githubusercontent.com/MehdiMst00/MinIOStorage/main/screenshots/1.png">
    <img src="https://raw.githubusercontent.com/MehdiMst00/MinIOStorage/main/screenshots/1.png" />
  </a>
  <a href="https://raw.githubusercontent.com/MehdiMst00/MinIOStorage/main/screenshots/2.png">
    <img src="https://raw.githubusercontent.com/MehdiMst00/MinIOStorage/main/screenshots/2.png" />
  </a>
</p>

## Run
1. Install and start docker
2. Build and run using `docker-compose`
```powershell
docker-compose up -d
```

## MinIO Console
1. launch `http://localhost:9001/login` url in browser
2. default username: `admin` - password: `admin123` (you can see in docker compose file)
3. Create `Access Keys` in menu and set it in docker compose override and rerun `docker-compose up -d` for apply changes

## Swagger
https://localhost:8081/swagger/index.html

## Links
[docker desktop](https://docs.docker.com/desktop/)

[min.io](https://min.io)

[minio-dotnet](https://github.com/minio/minio-dotnet)

[minio nuget](https://www.nuget.org/packages/Minio)
