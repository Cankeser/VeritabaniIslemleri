

📌 VeritabaniIslemleri Sınıfı Nedir?Bu sınıf, SQL Server veritabanı ile etkileşim kurmak için kullanılan genel amaçlı bir yardımcı sınıftır.İçerisinde ekleme, güncelleme, silme, veri çekme ve listeleme gibi temel işlemleri yapan metodlar bulunur.Böylece her seferinde SQL sorgusu yazmak yerine, bu metodları kullanarak işlemlerimizi daha pratik ve düzenli bir şekilde yapabiliriz.

Sınıf, bağlantı dizesi (connection string) olarak localhost sunucusundaki TeknikServisDb adlı veritabanını kullanır.

🛠️ Sınıfta Bulunan Metotlar

1️⃣ KomutCalistir (SQL Komutlarını Çalıştırma)

📌 Ne İşe Yarar?Bu metod, veritabanına yeni veri eklemek, güncelleme yapmak veya veri silmek gibi işlemler için kullanılır.

📌 Nasıl Çalışır?

Bir SQL sorgusu alır (örneğin: INSERT INTO, UPDATE, DELETE).

Eğer varsa parametreleri alır (veri güvenliğini artırmak için).

SQL komutunu çalıştırır ve kaç satırın etkilendiğini döndürür.

📌 Örnek Kullanım:

string sorgu = "UPDATE Musteriler SET Ad = @Ad WHERE Id = @Id";
Dictionary<string, object> parametreler = new Dictionary<string, object>
{
    { "Ad", "Ahmet" },
    { "Id", 5 }
};
int sonuc = KomutCalistir(sorgu, parametreler);

Bu kod, Id değeri 5 olan müşterinin Ad bilgisini "Ahmet" olarak günceller.

2️⃣ VeriGetir (Veri Çekme - Listeleme)

📌 Ne İşe Yarar?Bu metod, veritabanından veri almak için kullanılır. Örneğin:

Kullanıcı bilgilerini listelemek

Ürünleri getirmek

Sipariş geçmişini göstermek

📌 Nasıl Çalışır?

Bir SQL sorgusu alır (örneğin: SELECT * FROM Musteriler).

Eğer varsa parametreleri alır (filtreleme için).

Sonuçları bir DataTable içine doldurur ve geri döndürür.

📌 Örnek Kullanım:

string sorgu = "SELECT * FROM Musteriler WHERE Sehir = @Sehir";
Dictionary<string, object> parametreler = new Dictionary<string, object>
{
    { "Sehir", "İstanbul" }
};
DataTable musteriler = VeriGetir(sorgu, parametreler);

Bu kod, sadece İstanbul'daki müşterileri listeleyen bir sorgu çalıştırır.

3️⃣ VeriEkle (Yeni Kayıt Ekleme)

📌 Ne İşe Yarar?Belirtilen bir tabloya yeni bir veri eklemek için kullanılır.

📌 Nasıl Çalışır?

Hangi tabloya ekleme yapılacağını alır.

Kolon isimleri ve değerlerini alır.

Dinamik bir INSERT INTO sorgusu oluşturur ve çalıştırır.

Eğer ekleme başarılı olursa true, başarısız olursa false döner.

📌 Örnek Kullanım:

Dictionary<string, object> yeniMusteri = new Dictionary<string, object>
{
    { "Ad", "Mehmet" },
    { "Soyad", "Yılmaz" },
    { "Sehir", "Ankara" }
};
bool eklendiMi = VeriEkle("Musteriler", yeniMusteri);

Bu kod, Musteriler tablosuna yeni bir müşteri ekler.

4️⃣ VeriGuncelle (Var Olan Veriyi Güncelleme)

📌 Ne İşe Yarar?Bir tablodaki mevcut veriyi güncellemek için kullanılır.

📌 Nasıl Çalışır?

Hangi tabloya işlem yapılacağını alır.

Güncellenecek kolon isimleri ve değerlerini alır.

Koşul belirterek hangi kayıtların güncelleneceğini belirtir.

Güncelleme işlemi başarılı olursa true, değilse false döner.

📌 Örnek Kullanım:

Dictionary<string, object> guncellemeBilgileri = new Dictionary<string, object>
{
    { "Ad", "Mustafa" }
};
Dictionary<string, object> kosulBilgileri = new Dictionary<string, object>
{
    { "Id", 2 }
};
bool guncellendiMi = VeriGuncelle("Musteriler", guncellemeBilgileri, "Id = @Id", kosulBilgileri);

Bu kod, Id'si 2 olan müşterinin adını "Mustafa" olarak günceller.

5️⃣ VeriSil (Belirli Verileri Silme)

📌 Ne İşe Yarar?Bir tablodan belirli koşula uyan kayıtları silmek için kullanılır.

📌 Nasıl Çalışır?

Silme işleminin yapılacağı tabloyu alır.

Hangi koşula göre silme işlemi yapılacağını alır.

Silme işlemi başarılı olursa true, değilse false döner.

📌 Örnek Kullanım:

Dictionary<string, object> silmeKosulu = new Dictionary<string, object>
{
    { "Id", 3 }
};
bool silindiMi = VeriSil("Musteriler", "Id = @Id", silmeKosulu);

Bu kod, Id'si 3 olan müşteriyi veritabanından siler.

6️⃣ VeriListele (Tüm Kayıtları Getirme)

📌 Ne İşe Yarar?Belirtilen tablodaki tüm verileri listelemek için kullanılır.

📌 Nasıl Çalışır?

Tablo adı alır.

Koşul ekleyerek belirli kayıtları getirebilir veya tüm kayıtları alabilir.

Sonuçları bir DataTable olarak döndürür.

📌 Örnek Kullanım:

DataTable tumMusteriler = VeriListele("Musteriler");

Bu kod, Musteriler tablosundaki tüm kayıtları getirir.

🎯 Genel Olarak Bu Sınıfın Amacı Nedir?

Kod tekrarını azaltmak: Her seferinde uzun SQL sorguları yazmak yerine, bu metotları kullanarak işlemleri kısa ve anlaşılır hale getirebiliriz.

Güvenliği artırmak: Parametreli sorgular kullanarak SQL Injection gibi güvenlik açıklarını önleyebiliriz.

Esnek bir yapı sunmak: Hangi tabloya işlem yapılacağı, hangi verilerin güncelleneceği gibi şeyleri dinamik olarak belirlememizi sağlar.

