# 1. AÞAMA: SDK (DERLEME VE TEST)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm .csproj dosyalarýný hiyerarþiye uygun kopyalýyoruz
COPY ["src/Blog.Api/Blog.Api.csproj", "src/Blog.Api/"]
COPY ["src/Blog.Domain/Blog.Domain.csproj", "src/Blog.Domain/"]
COPY ["src/Blog.Application/Blog.Application.csproj", "src/Blog.Application/"]
COPY ["src/Blog.Infrastructure/Blog.Infrastructure.csproj", "src/Blog.Infrastructure/"]
COPY ["tests/Blog.Tests.Unit/Blog.Tests.Unit.csproj", "tests/Blog.Tests.Unit/"]

# Baðýmlýlýklarý geri yüklüyoruz (Hem API hem TEST için)
RUN dotnet restore "src/Blog.Api/Blog.Api.csproj"
RUN dotnet restore "tests/Blog.Tests.Unit/Blog.Tests.Unit.csproj"

# Tüm kaynak kodu kopyalýyoruz
COPY . .

# --- KRÝTÝK ADIM: TESTLERÝ ÇALIÞTIR ---
# --no-restore yerine sadece Release modunda çalýþtýrýyoruz, 
# NuGet yollarýný karýþtýrmamasý için temiz bir build istiyoruz.
RUN dotnet test "tests/Blog.Tests.Unit/Blog.Tests.Unit.csproj" -c Release

# Uygulamayý publish ediyoruz
WORKDIR "/src/src/Blog.Api"
RUN dotnet publish "Blog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. AÞAMA: RUNTIME (ÇALIÞTIRMA)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Uygulamanýn çalýþacaðý port (Varsayýlan 8080)
EXPOSE 8080

ENTRYPOINT ["dotnet", "Blog.Api.dll"]