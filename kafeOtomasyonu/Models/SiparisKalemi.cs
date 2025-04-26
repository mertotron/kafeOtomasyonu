using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafeOtomasyonu.Models
{
    internal class SiparisKalemi
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }

        public decimal ToplamFiyat => Adet * Fiyat;

        public override string ToString()
        {
            return $"{UrunAdi} x {Adet} = {ToplamFiyat:C2}";
        }

    }
}
