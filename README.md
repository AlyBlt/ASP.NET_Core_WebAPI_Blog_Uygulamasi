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
| Kimlik Doğrulama    | JWT (JSON Web Token)     |
| API Dökümantasyonu  | Swagger / OpenAPI        |
| Logging             | Serilog                  |
| Validasyon          | FluentValidation         |
| Test                | xUnit _(planlanıyor)_    |
| Versiyon Kontrolü   | Git & GitHub             |
| Global Hata Yöntemi | Exception Handling       |

---

## ✨ Uygulama Özellikleri

- Blog gönderilerini listeleme, detay görüntüleme
- Yeni gönderi oluşturma, düzenleme, silme
- RESTful mimari yapısı
- Entity Framework Core ile veritabanı işlemleri
- Katmanlı mimari planlaması (Controller / Service / Repository)
- Swagger ile API test arayüzü
- JWT Authentication
- Validasyon ve hata yönetimi

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

## 🔐 Rol Tabanlı Yetkilendirme

| Rol   | Yetki                                                                 |
|-------|-----------------------------------------------------------------------|
| User  | Blogları listeleyebilir ve detaylarını görebilir.                     |
| Author| Blog yazısı oluşturabilir, güncelleyebilir, silebilir.                |
| Admin | Kullanıcıların rollerini değiştirebilir.                              |

> 🛑 **Register işlemi sadece `User` rolü ile kayıt olmayı destekler.**  
> ✅ **Admin**, kullanıcıların rollerini `"Admin"`, `"Author"` veya `"User"` olarak güncelleyebilir.

## 📡 API Endpointleri (Örnek)

| HTTP Metodu | Rota                          | Açıklama                                  |
|-------------|-------------------------------|-------------------------------------------|
| POST        | /api/user/register            | Yeni kullanıcı kaydı _(sadece User)_      |
| POST        | /api/user/login               | Giriş ve token alımı                      |
| PUT         | /api/user/update-role/{id}    | Admin tarafından rol güncelleme           |
| GET         | /api/posts                    | Tüm gönderileri getir                     |
| GET         | /api/posts/{id}               | ID ile gönderi getir                      |
| POST        | /api/posts                    | Yeni gönderi oluştur _(sadece Author)_    |
| PUT         | /api/posts/{id}               | Gönderiyi güncelle _(sadece Author)_      |
| DELETE      | /api/posts/{id}               | Gönderiyi sil _(sadece Author)_           |


## 📁 Proje Yapısı

```text
├── Controllers       # API uç noktalarını barındırır
├── Data              # Veritabanı context ve seed işlemleri
├── DTOs              # Veri transfer nesneleri (Request/Response)
├── Helpers           # Yardımcı sınıflar ve sabitler
├── Mappings          # AutoMapper konfigürasyonları
├── Middlewares       # Özel hata yakalama gibi middleware'ler
├── Migrations        # EF Core migration dosyaları
├── Models            # Veritabanı entity sınıfları
├── Repositories      # Veri erişim işlemleri (interface + implementation)
├── Services          # İş mantığı katmanı
└── Validators        # FluentValidation sınıfları
```

## ✅ Yol Haritası

- [x] CRUD işlemleri  
- [x] Katmanlı mimariye geçiş  
- [x] JWT ile kimlik doğrulama  
- [x] Swagger entegrasyonu  
- [x] Logging (Serilog)  
- [x] FluentValidation ile input doğrulama  
- [x] Global Exception Handling  
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