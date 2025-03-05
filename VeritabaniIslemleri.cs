using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace VeritabaniIslemleri
{
    public class VeritabaniIslemleri
    {
        //Database alanı değişecek üzerinde çalışılmak istenen database adı girilecek
        private readonly string baglantiDizesi = "server=localhost;database=TeknikServisDb;Trusted_Connection=true";

        /* --- Genel komut çalıştırma metodu (Ekleme, Güncelleme, Silme) --- 
         - İlk parametre (sorgu): Çalıştırılacak SQL komutu. 
         - İkinci parametre (parametreler): SQL sorgusuna dinamik olarak eklenebilecek parametreleri içeren bir Dictionary.
         - Metot, etkilenen satır sayısını döndürür.  
        */
        public int KomutCalistir(string sorgu, Dictionary<string, object> parametreler = null)
        {
            // Etkilenen satır bilgisini tutmak için int türünde bir değişken tanımlayıp ilk değerini 0 olarak veriyoruz.
            int etkilenenSatir = 0;

            // Veritabanı bağlantısı için SqlConnection nesnesi oluşturuluyor. Bağlantı dizesi (connection string) parametre olarak veriliyor.
            using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
            {
                // Bağlantıyı açıyoruz
                baglanti.Open();

                // Belirtilen SQL sorgusunu çalıştırmak için SqlCommand nesnesi oluşturuluyor. Sorgu ve bağlantı nesnesi parametre olarak veriliyor.
                using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                {
                    // Parametreler sözlüğü üzerinde döngü başlatıyoruz. 
                    foreach (var param in parametreler)
                    {
                        //Parametrelerin her biri (@paramAnahtar, paramDegeri) şeklinde sorguya ekleniyor.
                        komut.Parameters.AddWithValue($"@{param.Key}", param.Value);
                    }

                    // Komut çalıştırılıyor ve etkilenen satır sayısı alınıyor.
                    etkilenenSatir = komut.ExecuteNonQuery();
                }
            }

            // Etkilenen satır sayısını döndürüyoruz.
            return etkilenenSatir;
        }


        /* --- Genel veri getirme, listeleme metodu ---
          - Belirtilen SQL sorgusunu çalıştırarak sonuçları bir DataTable içinde döndüren metot.  
          - İlk parametre (sorgu): Çalıştırılacak SQL sorgusu.  
          - İkinci parametre (parametreler): SQL sorgusuna dinamik olarak eklenen parametreleri içeren Dictionary.  
        */
        public DataTable VeriGetir(string sorgu, Dictionary<string, object> parametreler = null)
        {
            // Sonuçları saklamak için DataTable oluşturuluyor.
            DataTable table = new DataTable();

            // Veritabanı bağlantısı oluşturuluyor.
            using (SqlConnection baglanti = new SqlConnection(baglantiDizesi))
            {
                // SQL sorgusunu çalıştırmak için SqlCommand nesnesi oluşturuluyor.
                using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                {
                    //Parametrenin Null yani boş olma durumu kontrol edelir.
                    if (parametreler != null)
                    {
                        // Parametreler döngüyle ekleniyor.
                        foreach (var param in parametreler)
                        {
                            // Parametre, (@paramAdi,paramDegeri) şeklinde sorguya ekleniyor.
                            komut.Parameters.AddWithValue($"@{param.Key}", param.Value);

                        }

                    }

                    // Verileri almak için SqlDataAdapter oluşturuluyor.
                    using (SqlDataAdapter adaptor = new SqlDataAdapter(komut))
                    {
                        // Sorgu sonucu DataTable içine dolduruluyor.
                        adaptor.Fill(table);

                    }
                }
            }

            // Doldurulmuş DataTable geriye döndürülüyor.
            return table;
        }


        /* Genel Veri Ekleme Metodu, belirli bir tabloya veri eklemek için kullanılır. 
         * Bu metodun amacı, kolon adları ve değerleri dinamik olarak alıp, 
         * SQL INSERT komutunu oluşturmak ve veritabanına eklemektir.*/
        public bool VeriEkle(string tabloAdi, Dictionary<string, object> kolonlar)
        {

            // Kolon isimlerinin virgülle ayrılmış bir string haline getirilmesi.
            // Örneğin: "Ad,Soyad,Yas"
            string kolonlarStr = string.Join(",", kolonlar.Keys);

            // Parametreler için "@KolonAdı" şeklinde bir string oluşturuluyor.
            // Örneğin: "@Ad,@Soyad,@Yas"
            string degerlerStr = string.Join(",", kolonlar.Keys.Select(k => $"@{k}"));

            // Dinamik olarak INSERT INTO sorgusu oluşturuluyor.
            // Örneğin: "INSERT INTO Kullanıcılar(Ad,Soyad,Yas) VALUES(@Ad,@Soyad,@Yas)"
            string eklemeSorgusu = $"INSERT INTO {tabloAdi}({kolonlarStr}) VALUES({degerlerStr})";

            // Sorgu KomutCalistir metodu ile  çalıştırılıyor ve eklenen satır sayısı kontrol ediliyor. 
            // Eğer satır eklenirse true döndürülüyor, aksi takdirde false.
            return KomutCalistir(eklemeSorgusu, kolonlar) > 0;
        }


        /* --- Genel Güncelleme Metodu ---
        - Bu metod, belirtilen tabloya ait verileri güncellemek için kullanılır. 
        - İlk parametre (tabloAdi): Güncellenecek tablonun adı.
        - İkinci parametre (kolonlar): Güncellenecek kolonların adı ve yeni değerleri içeren bir sözlük (Dictionary).
        - Üçüncü parametre (kosul): Güncellenecek kayıtları belirlemek için WHERE koşulunu içeren bir string. 
        - Dördüncü parametre (kosulParametreleri): WHERE koşulunda kullanılacak parametrelerin ve değerlerinin bulunduğu bir sözlük.
        - Metod, başarılı bir güncelleme işlemi gerçekleştirildiyse true döndürür, aksi takdirde false döner.*/
        public bool VeriGuncelle(string tabloAdi, Dictionary<string, object> kolonlar, string kosul, Dictionary<string, object> kosulParametreleri)
        {
            // Kolon adlarını ve parametrelerini birleştirerek SET kısmını oluşturuyoruz.
            // Örneğin: "Ad = @Ad, Soyad = @Soyad"
            string setStr = string.Join(", ", kolonlar.Keys.Select(k => $"{k} = @{k}"));

            // Güncelleme sorgusunun tamamını oluşturuyoruz.
            // "UPDATE {tabloAdi} SET {setStr} WHERE {kosul}" şeklinde dinamik bir SQL sorgusu oluşturuluyor.
            // Bu sorgu, kolonları ve koşulu içeren dinamik bir UPDATE sorgusu olacaktır.
            string sorgu = $"UPDATE {tabloAdi} SET {setStr} WHERE {kosul}";

            // Güncelleme için parametreleri birleştiriyoruz.
            // Kosul parametrelerini kolonlara ekliyoruz (örneğin: WHERE kısmı için kullanılan parametreler).
            foreach (var param in kosulParametreleri)
            {
                kolonlar[param.Key] = param.Value;
            }

            // SQL sorgusunu çalıştırıyoruz ve etkilenen satır sayısını kontrol ediyoruz.
            // Eğer etkilenen satır sayısı 0'dan büyükse true döner, aksi takdirde false döner.
            return KomutCalistir(sorgu, kolonlar) > 0;
        }

        /* 📌 **Genel Silme Metodu**
          - Bu metod, belirtilen tablodan veri silmek için kullanılır. 
          - İlk parametre (tabloAdi): Verilerin silineceği tablonun adı.
          - İkinci parametre (kosul): Silinecek verileri belirlemek için WHERE koşulunu içeren bir string.Örneğin, "Id = @Id" gibi          
        // - Üçüncü parametre (kosulParametreleri): WHERE koşulunda kullanılacak parametrelerin ve değerlerinin bulunduğu bir sözlük.
         Metod, başarılı bir silme işlemi gerçekleştirilirse true döner, aksi takdirde false döner.*/
        public bool VeriSil(string tabloAdi, string kosul, Dictionary<string, object> kosulParametreleri)
        {
            // Dinamik olarak DELETE sorgusu oluşturuluyor.
            // Örneğin: "DELETE FROM Kullanıcılar WHERE Id = @Id"
            string sorgu = $"DELETE FROM {tabloAdi} WHERE {kosul}";

            // SQL sorgusu çalıştırılıyor ve etkilenen satır sayısı kontrol ediliyor.
            // Eğer etkilenen satır sayısı 0'dan büyükse true döner, aksi takdirde false döner.
            return KomutCalistir(sorgu, kosulParametreleri) > 0;
        }
        // 📌 **Genel Listeleme Metodu**
        // Bu metod, belirtilen tablodan verileri listelemek için kullanılır. 
        // - İlk parametre (tabloAdi): Verilerin listeleneceği tablonun adı.
        // - İkinci parametre (kosul): Verilerin filtrelenmesi için WHERE koşulunu içeren bir string. 
        //   Varsayılan olarak "1=1" kullanılır, yani tüm veriler listelenir. (İhtiyaca göre filtreleme yapılabilir.)
        // - Üçüncü parametre (kosulParametreleri): WHERE koşulunda kullanılacak parametrelerin ve değerlerinin bulunduğu bir sözlük. (Varsayılan olarak null'dır.)
        // Bu metod, verileri bir `DataTable` olarak döndürür.
        public DataTable VeriListele(string tabloAdi, string kosul = "1=1", Dictionary<string, object> kosulParametreleri = null)
        {
            // Dinamik olarak SELECT sorgusu oluşturuluyor.
            // Örneğin: "SELECT * FROM Kullanıcılar WHERE Id = @Id"
            string sorgu = $"SELECT * FROM {tabloAdi} WHERE {kosul}";

            // Sorgu çalıştırılıyor ve sonucu bir DataTable olarak döndürülüyor.
            return VeriGetir(sorgu, kosulParametreleri);
        }
    }
}
