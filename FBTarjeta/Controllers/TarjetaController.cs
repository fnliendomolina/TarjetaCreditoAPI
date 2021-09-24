using FBTarjeta.Models;
using FBTarjeta.Models.Request;
using FBTarjeta.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBTarjeta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Respuesta respuesta = new Respuesta();
            using (var db = new TarjetaCreditoContext())
            {
                try
                {
                    respuesta.Exito = 1;
                    respuesta.Data = await db.Tarjetas.ToListAsync();
                }
                catch (Exception ex)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = ex.Message;
                    return BadRequest(respuesta);
                }
            }
            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TarjetaRequest model)
        {
            Respuesta respuesta = new Respuesta();
            using (var db = new TarjetaCreditoContext())
            {
                try
                {
                    await db.Tarjetas.AddAsync(new Tarjeta {
                        Titular = model.Titular,
                        NumeroTarjeta = model.NumeroTarjeta,
                        FechaExpiracion = model.FechaExpiracion,
                        Cvv = model.CVV
                    });
                    await db.SaveChangesAsync();
                    respuesta.Exito = 1;
                }
                catch (Exception ex)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = ex.Message;
                    return BadRequest(respuesta);
                }
                return Ok(respuesta);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(long? Id, TarjetaRequest model)
        {
            Respuesta respuesta = new Respuesta();
            if (Id == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Id no valido";
                return NotFound(respuesta);
            }

            using (var db = new TarjetaCreditoContext())
            {
                try
                {
                    db.Update(new Tarjeta
                    {
                        Cvv = model.CVV,
                        FechaExpiracion = model.FechaExpiracion,
                        Id = (long)Id,
                        NumeroTarjeta = model.NumeroTarjeta,
                        Titular = model.Titular
                    });
                    await db.SaveChangesAsync();
                    respuesta.Exito = 1;
                    return Ok(respuesta);
                }
                catch (Exception ex)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = ex.Message;
                }
            }
            return BadRequest(respuesta);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long? Id)
        {
            Respuesta respuesta = new Respuesta();
            if (Id == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Id no valido";
                return NotFound(respuesta);
            }

            using (var db = new TarjetaCreditoContext())
            {
                try
                {
                    var tarjeta = db.Tarjetas.Where(x => x.Id == Id).FirstOrDefault();
                    if (tarjeta == null)
                    {
                        respuesta.Exito = 0;
                        respuesta.Mensaje = "Tarjeta no encontrada";
                        return NotFound(respuesta);
                    }

                    db.Tarjetas.Remove(tarjeta);
                    await db.SaveChangesAsync();
                    respuesta.Exito = 1;
                    return Ok(respuesta);
                }
                catch (Exception ex)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = ex.Message;
                }
            }
            return BadRequest(respuesta);
        }
    }
}
