//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ProyectoEdificios.Data.Contexts;
//using ProyectoEdificios.Data.Models;

//namespace ProyectoEdificios.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EdificiosController : ControllerBase
//    {
//        private readonly ProyectoEdificiosDbContext _context;

//        public EdificiosController(ProyectoEdificiosDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Edificios
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Edificio>>> GetEdificios()
//        {
//            return await _context.Edificios.ToListAsync();
//        }

//        // GET: api/Edificios/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Edificio>> GetEdificio(string id)
//        {
//            var edificio = await _context.Edificios.FindAsync(id);

//            if (edificio == null)
//            {
//                return NotFound();
//            }

//            return edificio;
//        }

//        // PUT: api/Edificios/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutEdificio(string id, Edificio edificio)
//        {
//            if (id != edificio.Name)
//            {
//                return BadRequest();
//            }

//            _context.Entry(edificio).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!EdificioExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Edificios
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<IEnumerable<Edificio>>> PostEdificios([FromBody] IEnumerable<Edificio> edificios)
//        {
//            var lista = edificios?.ToList();

//            if (lista == null || !lista.Any())
//                return BadRequest("Debe enviar al menos un edificio.");

//            var primerEdificio = lista.First();

//            await _context.Edificios.Where(x => x.ProjectId == primerEdificio.ProjectId)
//                .ExecuteDeleteAsync();

//            _context.Edificios.AddRange(lista);

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                var duplicated = lista.FirstOrDefault(e => EdificioExists(e.Name));
//                if (duplicated != null)
//                    return Conflict($"El edificio '{duplicated.Name}' ya existe.");

//                throw;
//            }

//            return Ok(lista);
//        }

//        // DELETE: api/Edificios/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEdificio(string id)
//        {
//            var edificio = await _context.Edificios.FindAsync(id);
//            if (edificio == null)
//            {
//                return NotFound();
//            }

//            _context.Edificios.Remove(edificio);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool EdificioExists(string id)
//        {
//            return _context.Edificios.Any(e => e.Name == id);
//        }
//    }
//}
