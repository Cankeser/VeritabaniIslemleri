VeriTabaniIslemleri sınıfı, SQL Server veritabanı ile etkileşime geçmek için tasarlanmış, temel veritabanı işlemlerini kolaylaştıran ve düzenli hale getiren bir yardımcı sınıftır. Bu sınıf, veritabanı ile işlem yaparken daha verimli ve güvenli bir yol sunar. SQL sorgularını her seferinde manuel yazmak yerine, sınıfın sağladığı metodlar sayesinde bu işlemler daha basit ve hızlı bir şekilde yapılabilir.

### Sınıfın Metodları

1. **KomutCalistir**  
   Bu metod, SQL sorgularını çalıştırmak için kullanılır. Veri ekleme, güncelleme veya silme işlemleri yapılabilir. SQL komutları, parametreli olarak alınır, böylece güvenlik önlemleri sağlanır (SQL Injection saldırılarına karşı). İşlem başarılıysa, etkilenen satır sayısı döndürülür.

   **Örnek kullanım:**
   ```csharp
   string sorgu = "UPDATE Musteriler SET Ad = @Ad WHERE Id = @Id";
   Dictionary<string, object> parametreler = new Dictionary<string, object>
   {
       { "Ad", "Ahmet" },
       { "Id", 5 }
   };
   int sonuc = KomutCalistir(sorgu, parametreler);
   ```

2. **VeriGetir**  
   Bu metod, veritabanından veri almak için kullanılır. Bir SQL sorgusu (genellikle SELECT) çalıştırılır ve sonuçlar bir `DataTable` olarak döndürülür. Parametre kullanarak filtreleme de yapılabilir.

   **Örnek kullanım:**
   ```csharp
   string sorgu = "SELECT * FROM Musteriler WHERE Sehir = @Sehir";
   Dictionary<string, object> parametreler = new Dictionary<string, object>
   {
       { "Sehir", "İstanbul" }
   };
   DataTable musteriler = VeriGetir(sorgu, parametreler);
   ```

3. **VeriEkle**  
   Belirtilen tabloya yeni bir veri ekler. Dinamik bir `INSERT INTO` komutu oluşturur ve işlemi gerçekleştiren metod, başarılıysa `true`, başarısızsa `false` döndürür.

   **Örnek kullanım:**
   ```csharp
   Dictionary<string, object> yeniMusteri = new Dictionary<string, object>
   {
       { "Ad", "Mehmet" },
       { "Soyad", "Yılmaz" },
       { "Sehir", "Ankara" }
   };
   bool eklendiMi = VeriEkle("Musteriler", yeniMusteri);
   ```

4. **VeriGuncelle**  
   Mevcut bir kaydı güncellemek için kullanılır. Güncellenecek kolonlar ve yeni değerler belirtilir. Ayrıca hangi koşulda güncelleme yapılacağına dair parametreler alınır.

   **Örnek kullanım:**
   ```csharp
   Dictionary<string, object> guncellemeBilgileri = new Dictionary<string, object>
   {
       { "Ad", "Mustafa" }
   };
   Dictionary<string, object> kosulBilgileri = new Dictionary<string, object>
   {
       { "Id", 2 }
   };
   bool guncellendiMi = VeriGuncelle("Musteriler", guncellemeBilgileri, "Id = @Id", kosulBilgileri);
   ```

5. **VeriSil**  
   Belirli bir kaydı silmek için kullanılır. Hangi kaydın silineceği, bir koşul parametresiyle belirtilir. Başarılı bir işlemde `true`, başarısız bir işlemde `false` döndürülür.

   **Örnek kullanım:**
   ```csharp
   Dictionary<string, object> silmeKosulu = new Dictionary<string, object>
   {
       { "Id", 3 }
   };
   bool silindiMi = VeriSil("Musteriler", "Id = @Id", silmeKosulu);
   ```

6. **VeriListele**  
   Bir tablodaki tüm verileri listelemek için kullanılır. Koşul verilir veya tüm kayıtlar döndürülür. Sonuç, bir `DataTable` olarak elde edilir.

   **Örnek kullanım:**
   ```csharp
   DataTable tumMusteriler = VeriListele("Musteriler");
   ```

### Sınıfın Genel Amacı

- **Kod Tekrarını Azaltma:** Her seferinde SQL sorgusu yazmak yerine, metodları kullanarak aynı işlemleri tekrar tekrar yapabiliriz.
- **Güvenlik:** SQL Injection gibi saldırılara karşı güvenli parametreli sorgular kullanılır.
- **Esneklik:** Hangi tabloya işlem yapılacağı, hangi verilerin güncelleneceği gibi durumlar dinamik bir şekilde belirlenebilir.
  
Bu sınıf, veritabanı işlemlerini basitleştirir ve düzenler, özellikle büyük projelerde bu tür yardımcı sınıfların kullanımı, bakım ve yönetimi kolaylaştırır.
