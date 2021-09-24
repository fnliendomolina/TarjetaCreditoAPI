using System;
using System.Collections.Generic;

#nullable disable

namespace FBTarjeta.Models
{
    public partial class Tarjeta
    {
        public long Id { get; set; }
        public string Titular { get; set; }
        public string NumeroTarjeta { get; set; }
        public string FechaExpiracion { get; set; }
        public string Cvv { get; set; }
    }
}
