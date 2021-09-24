using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBTarjeta.Models.Request
{
    public class TarjetaRequest
    {
        public long Id { get; set; }
        [Required]
        [ExisteNumeroTarjeta(ErrorMessage = "La tarjeta ingresada ya existe")]
        public string NumeroTarjeta { get; set; }
        [Required]
        public string Titular { get; set; }
        [Required]
        public string FechaExpiracion { get; set; }
        [Required]
        public string CVV { get; set; }
    }

    #region Validations
    public class ExisteNumeroTarjeta:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            TarjetaRequest _tarjetaRequest = (TarjetaRequest)validationContext.ObjectInstance;

            if (_tarjetaRequest.Id > 0) return ValidationResult.Success; //es una edicion

            string _numeroTarjeta = (string)value;
            using (var db = new TarjetaCreditoContext())
            {
                var _tarjeta = db.Tarjetas.Where(x => x.NumeroTarjeta.Equals(_numeroTarjeta)).FirstOrDefault();
                if (_tarjeta == null) return ValidationResult.Success; //si no existe tarjeta valida
                else return new ValidationResult("La tarjeta de credito existe");
            }
        }
    }

    #endregion
}
