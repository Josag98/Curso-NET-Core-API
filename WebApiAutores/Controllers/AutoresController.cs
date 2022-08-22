using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/Autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet] // api/autores
        [HttpGet("listado")] // api/autores/listado
        [HttpGet("/listado")] // listado
        public async Task<ActionResult<List<Autor>>> GetAutores()
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> GetFirstAutor()
        {
            return await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Autor>> GetAutorForId(int Id)
        {
            var autor =  await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x=>x.Id == Id);
            if(autor == null)
            {
                return NotFound($"No hay un autor con el Id {Id}");

            }
            return autor;
        }

        [HttpGet("{Nombre}")]
        public async Task<ActionResult<Autor>> GetAutorForName(string Nombre)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Nombre.Contains(Nombre));
            if (autor == null)
            {
                return NotFound($"No hay un autor con el Nombre {Nombre}");

            }
            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> PostAutores([FromBody] Autor newAutor)
        {
            context.Add(newAutor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> PutAutores(Autor Autor,int Id)
        {
            if(Autor.Id != Id)
            {
                return BadRequest("El Id del autor no coincide con el Id de la URL");
            }

            var extiste = await context.Autores.AnyAsync(x => x.Id == Id);

            if (!extiste)
            {
                return NotFound();
            }

            context.Update(Autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteAutores(int Id)
        {
           
            var extiste = await context.Autores.AnyAsync(x=>x.Id == Id);

            if (!extiste)
            {
                return NotFound();
            }
            context.Remove(new Autor {Id = Id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
