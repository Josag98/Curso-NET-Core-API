using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("Id:int")]
        public async Task<ActionResult<Libro>> GetLibro(int Id)
        {
            return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x=> x.Id == Id);
        }

        [HttpPost]
        public async Task<ActionResult> PostLibro(Libro newLibro)
        {
            bool existeAutor = await context.Autores.AnyAsync(x=>x.Id == newLibro.AutorId);

            if (!existeAutor)
            {
                return BadRequest("El Id del autor del libro proporsionado no existe");
            }

            context.Libros.Add(newLibro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
