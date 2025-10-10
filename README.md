# 📝 ASP.NET Core Web API Blog Uygulaması

## 📌 Proje Açıklaması

Bu proje, ASP.NET Core Web API kullanılarak geliştirilmiş basit bir **Blog Yönetim Sistemi** API'sidir. Kullanıcılar blog gönderilerini görüntüleyebilir; yazarlar ise gönderi oluşturabilir, düzenleyebilir ve silebilir. Proje, temel CRUD işlemleri ve backend geliştirme becerilerimi sergilemek için hazırlanmıştır.

---

## 🛠️ Kullanılan Teknolojiler

| Katman / Araç       | Teknoloji                |
|---------------------|--------------------------|
| Backend Framework   | ASP.NET Core Web API     |
| ORM                 | Entity Framework Core    |
| Veritabanı          | SQL Server               |
| Kimlik Doğrulama    | JWT (JSON Web Token) _(planlanıyor)_ |
| API Dökümantasyonu | Swagger / OpenAPI        |
| Logging             | Serilog _(planlanıyor)_  |
| Validasyon          | FluentValidation _(planlanıyor)_ |
| Test                | xUnit _(planlanıyor)_     |
| Versiyon Kontrolü   | Git & GitHub             |

---

## ✨ Uygulama Özellikleri

- Blog gönderilerini listeleme, detay görüntüleme
- Yeni gönderi oluşturma, düzenleme, silme (planlanan auth ile)
- RESTful mimari yapısı
- Entity Framework Core ile veritabanı işlemleri
- Katmanlı mimari planlaması (Controller / Service / Repository)
- Swagger ile API test arayüzü (planlanıyor)
- JWT Authentication (planlanıyor)
- Validasyon ve hata yönetimi (planlanıyor)

## 🚀 Kurulum ve Çalıştırma

### 1️⃣ Klonla

```bash
git clone https://github.com/AlyBlt/ASP.NET_Core_WebAPI_Blog_Uygulamasi.git
cd ASP.NET_Core_WebAPI_Blog_Uygulamasi

```

### 2️⃣ Projeyi Aç

Visual Studio veya Visual Studio Code ile projeyi aç.

### 3️⃣ Veritabanı Ayarlarını Yap

appsettings.json dosyasındaki Connection String'i kendi bilgisayarına göre düzenle.

### 4️⃣ Migration ve Veritabanı Oluştur (Varsa)

```bash
dotnet ef database update
```

---

### 5️⃣ Uygulamayı Başlat

```bash
dotnet run
```

### 6️⃣ Swagger Arayüzü ile Test Et

Tarayıcıdan aç:
👉 https://localhost:5001/swagger (ya da uygulamanın çalıştığı port)


## 📡 API Endpointleri

| HTTP Metodu | Rota                | Açıklama                      |
|-------------|---------------------|-------------------------------|
| GET         | /api/posts          | Tüm gönderileri getir         |
| GET         | /api/posts/{id}     | ID ile gönderi getir          |
| POST        | /api/posts          | Yeni gönderi oluştur _(auth)_ |
| PUT         | /api/posts/{id}     | Gönderiyi güncelle _(auth)_   |
| DELETE      | /api/posts/{id}     | Gönderiyi sil _(auth)_        |

> 🛑 _Authentication işlemleri ilerleyen aşamada eklenecektir._

## 📁 Proje Yapısı (Planlanan)

```text
├── Controllers
├── Services
├── Repositories
├── Models
├── DTOs
├── Helpers
└── Middleware
```

## ✅ Yol Haritası

- [x] CRUD işlemleri tamamlandı  
- [ ] Katmanlı mimariye geçiş  
- [ ] JWT ile kimlik doğrulama  
- [ ] Swagger entegrasyonu  
- [ ] Logging (Serilog)  
- [ ] FluentValidation ile input doğrulama  
- [ ] Global Exception Handling  
- [ ] xUnit ile test senaryoları  
- [ ] README güncellemeleri  
- [ ] Docker ile yayınlama _(isteğe bağlı)_  


## 🖼️ Örnek Ekran Görüntüleri _(planlanıyor)_

> Projenin çalışır hali tamamlandığında buraya Swagger görüntüsü ve Postman örnekleri eklenecektir.

## 🤝 Katkıda Bulunmak

Bu proje gelişim sürecinde olup, her türlü geri bildirim ve katkıya açıktır.  
Forklayabilir, issue açabilir ya da pull request gönderebilirsiniz.

## 👩‍💻 Geliştirici

**Aliye Bulut, PhD**  
Junior Backend Developer | Biomedical Engineer  
📫 [LinkedIn Profilim](https://www.linkedin.com/in/aliye-bulut-phd-867453357/)  
📂 [GitHub Sayfam](https://github.com/AlyBlt)