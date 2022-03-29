using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSRVC_Core.Modelo;

namespace WebSRVC_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenesPersonasController : ControllerBase
    {
        private readonly ModelAppDbContext _context;

        public ImagenesPersonasController(ModelAppDbContext context)
        {
            _context = context;
        }


        [HttpGet(Name ="Obtener Imagenes de una persona")]
        [Route("fotos/{ID_Persona}")]
        public IQueryable<ImagenesPersona> GetImagenesPersonaID(int ID_Persona)
        {
            var i = from ip in _context.ImagenesPersona where ip.PersonaId == ID_Persona select ip;
            return i;
            //return db.ImagenesPersona;
        }


        // GET: api/ImagenesPersonas
        [HttpGet]
        public IEnumerable<ImagenesPersona> GetImagenesPersona()
        {
            return _context.ImagenesPersona;
        }

        // GET: api/ImagenesPersonas/5
        [HttpGet(Name = "Obtener Relacion Imagenes- Persona")]
        [Route("relacion/{id}")]
        public async Task<IActionResult> GetImagenesPersona([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagenesPersona = await _context.ImagenesPersona.FindAsync(id);

            if (imagenesPersona == null)
            {
                return NotFound();
            }

            return Ok(imagenesPersona);
        }

        // PUT: api/ImagenesPersonas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagenesPersona([FromRoute] int id, [FromBody] ImagenesPersona imagenesPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != imagenesPersona.Id)
            {
                return BadRequest();
            }

            _context.Entry(imagenesPersona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenesPersonaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ImagenesPersonas
        [HttpPost]
        public async Task<IActionResult> PostImagenesPersona([FromBody] ImagenesPersona imagenesPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                imagenesPersona.Fecha = DateTime.Now;

                Identificacion.Detector detector = new Identificacion.Detector( );
                List<string> rostros = detector.ObtenerRostros_base64IM(imagenesPersona.Fotocompleta,false);
                if (rostros.Count>0)
                {
                    imagenesPersona.Foto = rostros.FirstOrDefault();
                    _context.ImagenesPersona.Add(imagenesPersona);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetImagenesPersona", new { id = imagenesPersona.Id }, imagenesPersona);
                }
                else
                {
                    return NotFound("Foto");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
            
        }

        //// POST: api/ImagenesPersonas
        //[HttpPost]
        //[Route("identificar/")]
        //public async Task<IActionResult> Identificar([FromBody] ImagenesPersona imagenesPersona)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {

        //        var pers = from p in _context.ImagenesPersona where p.PersonaId == imagenesPersona.PersonaId select p;

        //        Identificacion.IMetodoReconocimiento identificador = new Identificacion.Eigen();
                

                
        //        if (rostros.Count > 0)
        //        {
        //            imagenesPersona.Foto = rostros.FirstOrDefault();
        //            _context.ImagenesPersona.Add(imagenesPersona);
        //            await _context.SaveChangesAsync();

        //            return CreatedAtAction("GetImagenesPersona", new { id = imagenesPersona.Id }, imagenesPersona);
        //        }
        //        else
        //        {
        //            return NotFound("Foto");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }


        //}


        // DELETE: api/ImagenesPersonas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagenesPersona([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagenesPersona = await _context.ImagenesPersona.FindAsync(id);
            if (imagenesPersona == null)
            {
                return NotFound();
            }

            _context.ImagenesPersona.Remove(imagenesPersona);
            await _context.SaveChangesAsync();

            return Ok(imagenesPersona);
        }

        private bool ImagenesPersonaExists(int id)
        {
            return _context.ImagenesPersona.Any(e => e.Id == id);
        }
    }
}