# **Haber Çekme ve Elasticsearch Entegrasyonu**

Bu proje, [Sozcu](https://www.sozcu.com.tr/) web sitesinden haber başlıklarını dinamik olarak çekmek, bu verileri Elasticsearch'e kaydetmek ve Razor Pages tabanlı bir arayüz üzerinden haberleri arayıp görüntülemek için tasarlanmıştır.

---

## **Özellikler**
- **Web Scraping (Veri Çekme):** 
  - Selenium kullanılarak Sozcu.com.tr web sitesinden haber başlıkları ve URL'ler çekilir.
- **Elasticsearch Entegrasyonu:**
  - Çekilen haberler Elasticsearch'e kaydedilir.
  - Elasticsearch'te hızlı ve verimli arama yapılır.
- **Arama Özellikleri:**
  - Tüm haberleri getirme.
  - `Wildcard` ve `Bool` sorguları ile esnek arama.
  - Elasticsearch'te bulunan haberlerin tekrar kaydedilmesini önleme.
- **Kullanıcı Arayüzü:**
  - Razor Pages tabanlı bir web uygulaması ile haberleri listeleme ve arama.
- **Veri Yönetimi:**
  - Haberlerin Elasticsearch'teki verileri Kibana üzerinden görselleştirme ve yönetme.

---

## **Kullanılan Teknolojiler**
### **Backend**
- **ASP.NET Core:** RESTful API ile verilerin yönetimi.
- **Elasticsearch:** Veri depolama ve sorgulama için arama motoru.
- **Nest:** Elasticsearch ile entegrasyon için .NET kütüphanesi.
- **Selenium:** Web scraping işlemleri için.

### **Frontend**
- **ASP.NET Razor Pages:** Haberlerin kullanıcı dostu bir arayüzde görüntülenmesi ve arama yapılması.

### **Konteynerleştirme**
- **Docker:** Elasticsearch ve Kibana servislerini çalıştırmak için kullanılır.
- **Kibana:** Elasticsearch verilerini görselleştirmek ve yönetmek için.

---

## **2. Gereksinimler**
- **Docker** ve **Docker Compose** kurulu olmalıdır.
- **.NET 7 SDK** veya daha yeni bir sürüm yüklü olmalıdır.
- **ChromeDriver** yüklenmelidir (Selenium tarafından web scraping için kullanılır).

---

## **3. Elasticsearch ve Kibana'yı Çalıştırma**
Bu proje, Elasticsearch ve Kibana'yı çalıştırmak için Docker kullanır. Sağlanan `docker-compose.yml` dosyasını kullanarak servisleri çalıştırabilirsiniz.

### **Konteynerleri Başlatma**
```bash
docker-compose up -d


### **Konteynerleri Doğrulama**
- **Elasticsearch**: [http://localhost:9200](http://localhost:9200)
- **Kibana**: [http://localhost:5601](http://localhost:5601)


## **4. Uygulamayı Yapılandırma**
- **`appsettings.json`** dosyasını düzenleyin veya doğrudan `ElasticsearchService` içinde bağlantı URL'sini ayarlayın:

```json
{
  "ElasticsearchUrl": "http://localhost:9200"
}



## **5. Uygulamayı Çalıştırma**

### **ASP.NET Core API'yi Başlatma**
Aşağıdaki komutu kullanarak API'yi başlatın:
```bash
dotnet run --project WebAPI


### **Razor Pages Frontend'i Başlatma**
Aşağıdaki komutu kullanarak Razor Pages arayüzünü çalıştırın:
```bash
dotnet run --project Presentation

### **Frontend Kullanıcı Arayüzü**
Tarayıcınızdan şu adrese giderek arayüze ulaşabilirsiniz: [https://localhost:5001](https://localhost:5001)

## **Kullanım**

### **1. Haber Çekme**
- Razor Pages arayüzünde yer alan **"Veri Çek"** butonuna tıklayarak Sozcu.com.tr web sitesinden haberleri çekebilirsiniz.

### **2. Haberleri Görüntüleme**
- Çekilen haberler veya Elasticsearch'ten alınan veriler ana sayfada listelenir.

### **3. Haberlerde Arama Yapma**
- Arama çubuğunu kullanarak başlığa veya kaynağa göre haberleri arayabilirsiniz.

### **4. Verileri Kibana Üzerinden Yönetme**
- Kibana'yı açarak (`http://localhost:5601`) indekslenmiş verileri görselleştirebilir ve yönetebilirsiniz:
  - **Stack Management > Index Patterns** yolunu takip edin.
  - **news** için bir indeks deseni (index pattern) ekleyin.


## **API Endpoints**

### **1. Scrape News**
- **Endpoint:** `GET /api/articles/getscraping`
- Açıklama: Web sitesinden haberleri çeker ve Elasticsearch'e indeksler.

### **2. Get All Articles**
- **Endpoint:** `GET /api/articles/getallarticles`
- Açıklama: Elasticsearch'teki tüm haberleri getirir.

### **3. Search Articles**
- **Endpoint:** `GET /api/articles/getarticlesbyquery?query={query}`
- Açıklama: Gönderilen sorguya (query) göre haberleri arar.

---

## **Klasör Yapısı**

```bash
Project
├── Entity                  # Domain modelleri (ör. Article)
├── Infrastructure          # Elasticsearch ve Web Scraping için servisler
│   ├── Interfaces          # Servisler için arayüzler
│   └── Services            # Servislerin implementasyonları
├── WebAPI                  # Backend RESTful API
└── Presentation            # Razor Pages tabanlı frontend


## **Bağımlılıklar**

### **NuGet Paketleri**
- **Nest**: Elasticsearch için .NET istemcisi.
- **Selenium.WebDriver**: ChromeDriver kullanarak web scraping yapmak için.
- **HtmlAgilityPack**: Web scraping işlemleri için HTML'yi ayrıştırma (parsing) kütüphanesi.

### **Bağımlılıkları Yükleme**
Aşağıdaki komutu kullanarak projedeki tüm bağımlılıkları yükleyebilirsiniz:
```bash
dotnet restore
