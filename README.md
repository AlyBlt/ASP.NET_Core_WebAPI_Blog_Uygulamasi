# 📝 ASP.NET Core Web API Blog Uygulaması

## 📌 Proje Açıklaması

Bu depo, .NET 8 ile geliştirilmiş bir ASP.NET Core Web API blog uygulamasıdır. Projenin temel amacı, modern bir backend uygulamasının geliştirilmesinden ziyade; Dockerize edilmesi, Nginx Reverse Proxy ile yapılandırılması ve GitHub Actions tabanlı CI/CD süreçleriyle bulut ortamına (Vultr) taşınmasını deneyimlemek ve sergilemektir.

---

## 🛠️ Tech stack

- .NET 8 (ASP.NET Core Web API)  
- Entity Framework Core (SQL Server)  
- JWT (Microsoft.IdentityModel.Tokens)  
- FluentValidation, Serilog  
- AutoMapper
- Swagger (opsiyonel)  
- xUnit (unit testler mevcut)  
- Docker / Docker Compose  
- GitHub Actions (CI/CD)  
- Nginx (reverse proxy) 

---

## ✨ Öne çıkan özellikler
- Makaleleri listeleme, detay görüntüleme, oluşturma, güncelleme, silme  
- Kategori / Tag / Comment CRUD işlemleri
- Role-based authorization (Admin / Author / User)  
- EF Core migrations + otomatik DB seeding (Program.cs içinde)  
- Statik frontend örnekleri: `wwwroot/index.html`, `wwwroot/articles.html`, `wwwroot/article.html`  
- Docker Compose geliştirme konfigürasyonu (`docker-compose.yml`)  
- **CI/CD:** Test -> Image Build -> Push -> Deploy (GitHub Actions)

---

## 🚀 Hızlı Başlatma — Local (Docker)

### 1️⃣ Depoyu klonlayın:

```bash
git clone https://github.com/AlyBlt/ASP.NET_Core_WebAPI_Blog_Uygulamasi.git
cd ASP.NET_Core_WebAPI_Blog_Uygulamasi

```

### 2️⃣ `.env` oluşturun (örnek):

DB_NAME, DB_USER, DB_PASSWORD, JWT_SECRET gibi çevresel değişkenleri içeren .env dosyanızın ana dizinde olduğundan emin olun.

### 3️⃣ Docker image build ve up:

```docker compose up -d --build```

> ⚙️ **Port Yapılandırma Notu:** Proje, canlı sunucu (CI/CD) ve Nginx entegrasyonu gereksinimleri nedeniyle varsayılan olarak **11011** portu üzerinden sunulmaktadır.
> 
> * **Varsayılan Erişim:** `http://localhost:11011/swagger`
> * **Port Değişimi:** Eğer yerelinizde geleneksel `5000` portunu kullanmak isterseniz, `docker-compose.yml` dosyasındaki `ports` satırını `- "5000:8080"` şeklinde güncelleyerek komutu yeniden çalıştırabilirsiniz.

> **Erişim Detayları (Varsayılan)**

- Frontend: `http://localhost:11011/index.html`
- Health check: `http://localhost:11011/health`
- Swagger UI (ShowSwagger=true): `http://localhost:11011/swagger`

---

## 🔐 Rol Tabanlı Yetkilendirme

| Rol   | Yetki                                                                 |
|-------|-----------------------------------------------------------------------|
| User  | Blogları listeleyebilir ve detaylarını görebilir.                     |
| Author| Blog yazısı oluşturabilir, güncelleyebilir, silebilir.                |
| Admin | Tüm CRUD yetkileri + Kullanıcı rollerini yönetme.                     |

> 🛑 **Register işlemi sadece `User` rolü ile kayıt olmayı destekler.**  
> ✅ **Admin**, kullanıcıların rollerini `"Admin"`, `"Author"` veya `"User"` olarak güncelleyebilir.

---

## ℹ️ CI/CD ve Prod Deploy (özet)
- GitHub Actions workflow: commit → `dotnet test` → Docker image build → push → (opsiyonel) SSH ile sunucuya deploy.
- Örnek akış:
  - Build image: `docker build -t myregistry/blog-api:${{ github.sha }} -f src/Blog.Api/Dockerfile .`
  - Push: registry (Docker Hub / ACR / ghcr.io)
  - Sunucuda: `docker compose pull && docker compose up -d`
- Secrets: `DOCKERHUB_USER`, `DOCKERHUB_TOKEN`, `SERVER_HOST`, `SERVER_USER`, `SERVER_SSH_KEY` vb.

---

## 📁 Proje Yapısı

```text
.
├── .github
│   └── workflows
│       └── deploy.yml          # GitHub Actions CI/CD yapılandırması
├── BLOG
|   |── Solution Items 	        # örneğin: Dockerfile, .dockerignore, docker-compose.yml, .env)
|   |── tests
│   ├── src
│   │   ├── Blog.Api            # Sunum Katmanı (Web API/wwwroot)
│   │   │── Blog.Infrastructure # Veri Erişim Katmanı (EF Core)
│   │   ├── Blog.Application    # İş Mantığı (Business Logic)
│   │   └── Blog.Domain         # Veri Modelleri (Entities)
│   └── Blog.sln                # Visual Studio Çözüm Dosyası
└── README.md                   # Proje Dokümantasyonu
```

---

## ✅ Yol Haritası

- [x] JWT ile kimlik doğrulama  
- [x] AutoMapper ile DTO - entity dönüşümleri
- [x] Logging (Serilog)  
- [x] FluentValidation ile input doğrulama  
- [x] Global Exception Handling  
- [x] xUnit ile test senaryoları  
- [x] Docker & Docker Compose Entegrasyonu 
- [x] GitHub Actions CI/CD Pipeline
- [x] Nginx Reverse Proxy yapılandırması 

---

## 🤝 Katkıda Bulunmak

Bu proje gelişim sürecinde olup, her türlü geri bildirim ve katkıya açıktır.  
Forklayabilir, issue açabilir ya da pull request gönderebilirsiniz.

## 👩‍💻 Geliştirici

**Aliye Bulut, PhD**  
Backend Developer 
📫 [LinkedIn Profilim](https://www.linkedin.com/in/aliye-bulut-phd-867453357/)  
📂 [GitHub Sayfam](https://github.com/AlyBlt)