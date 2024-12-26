/*
Bir market yönetim sistemi geliştirmenizi istiyorum. Fikir olması açısından: Ürunler, müşteriler (bireysel müşteri, kurumsal müşteri), 
ödeme (kredi kartı, nakit, havale), sepet, indirim ( yüzdelik indirim, sabit indirim), sipariş (onaylandı, hazırlanıyor, teslim edildi) 
gibi yapılar olmalıdır. Arayüz kısmı benim için mühim değil. Önemli olan dönem boyunca öğrendiğimiz kodlama yapılarını kullanmış olmanızdır. 
Özellikle kalıtım, polimorfizm, abstract classlar ve try-catch yapılarının kullanılmadığı ödevler kabul edilmeyecektir. 
Ödev son uygulama ödeviniz olacak süresiz paylaşıyorum ve sizin hayal gücünüze bırakıyorum. İyi çalışmalar.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketYonetimSistemi
{
    // Ürün Sınıfı
    public class Urun
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }

        public Urun(int id, string ad, decimal fiyat)
        {
            Id = id;
            Ad = ad;
            Fiyat = fiyat;
        }

        public void UrunBilgisiGoster()
        {
            Console.WriteLine($"Ürün ID: {Id}, Ad: {Ad}, Fiyat: {Fiyat} TL");
        }
    }

    // Müşteri Abstract Sınıfı ve Alt Sınıflar
    public abstract class Musteri
    {
        public int MusteriId { get; set; }
        public string AdSoyad { get; set; }
        public abstract void MusteriBilgisiGoster();
    }

    public class BireyselMusteri : Musteri
    {
        public string TcKimlikNo { get; set; }

        public override void MusteriBilgisiGoster()
        {
            Console.WriteLine($"Bireysel Müşteri - ID: {MusteriId}, Ad: {AdSoyad}, TC: {TcKimlikNo}");
        }
    }

    public class KurumsalMusteri : Musteri
    {
        public string SirketAdi { get; set; }

        public override void MusteriBilgisiGoster()
        {
            Console.WriteLine($"Kurumsal Müşteri - ID: {MusteriId}, Şirket: {SirketAdi}, Ad: {AdSoyad}");
        }
    }

    // Ödeme Abstract Sınıfı ve Alt Sınıflar
    public abstract class Odeme
    {
        public abstract void OdemeYap(decimal tutar);
    }

    public class KrediKartiOdeme : Odeme
    {
        public override void OdemeYap(decimal tutar)
        {
            Console.WriteLine($"Kredi kartıyla {tutar} TL ödendi.");
        }
    }

    public class NakitOdeme : Odeme
    {
        public override void OdemeYap(decimal tutar)
        {
            Console.WriteLine($"Nakit olarak {tutar} TL ödendi.");
        }
    }

    // İndirim Sistemi (Interface Kullanımı)
    public interface IIndirim
    {
        decimal IndirimHesapla(decimal tutar);
    }

    public class YuzdelikIndirim : IIndirim
    {
        private decimal yuzde;

        public YuzdelikIndirim(decimal yuzde)
        {
            this.yuzde = yuzde;
        }

        public decimal IndirimHesapla(decimal tutar)
        {
            return tutar - (tutar * yuzde / 100);
        }
    }

    public class SabitIndirim : IIndirim
    {
        private decimal sabitTutar;

        public SabitIndirim(decimal sabitTutar)
        {
            this.sabitTutar = sabitTutar;
        }

        public decimal IndirimHesapla(decimal tutar)
        {
            return tutar - sabitTutar;
        }
    }

    // Sepet Sınıfı
    public class Sepet
    {
        public List<Urun> Urunler { get; set; } = new List<Urun>();

        public void UrunEkle(Urun urun)
        {
            Urunler.Add(urun);
            Console.WriteLine($"{urun.Ad} sepete eklendi.");
        }

        public decimal ToplamTutar()
        {
            return Urunler.Sum(u => u.Fiyat);
        }
    }

    // Sipariş Sınıfı
    public class Siparis
    {
        public int SiparisId { get; set; }
        public Sepet Sepet { get; set; }
        public string Durum { get; set; } = "Hazırlanıyor"; // Varsayılan durum

        public void SiparisDurumuGuncelle(string yeniDurum)
        {
            Durum = yeniDurum;
            Console.WriteLine($"Sipariş durumu güncellendi: {Durum}");
        }
    }

    // Program
    class MarketYonetimSistemi
    {
        static void Main(string[] args)
        {
            try
            {
                // Ürünler
                Urun urun1 = new Urun(1, "Elma", 10);
                Urun urun2 = new Urun(2, "Armut", 15);

                // Sepet
                Sepet sepet = new Sepet();
                sepet.UrunEkle(urun1);
                sepet.UrunEkle(urun2);

                Console.WriteLine($"Sepet Toplamı: {sepet.ToplamTutar()} TL");

                // İndirim
                IIndirim indirim = new YuzdelikIndirim(10); // %10 indirim
                decimal indirimliTutar = indirim.IndirimHesapla(sepet.ToplamTutar());
                Console.WriteLine($"İndirimli Tutar: {indirimliTutar} TL");

                // Ödeme
                Odeme odeme = new KrediKartiOdeme();
                odeme.OdemeYap(indirimliTutar);

                // Sipariş
                Siparis siparis = new Siparis { SiparisId = 1, Sepet = sepet };
                siparis.SiparisDurumuGuncelle("Onaylandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }
}
