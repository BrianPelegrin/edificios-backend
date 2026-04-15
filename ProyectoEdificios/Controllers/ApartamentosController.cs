using Microsoft.AspNetCore.Mvc;
using ProyectoEdificios.Services.Projects;

namespace ProyectoEdificios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartamentosController : ControllerBase
    {

        private readonly IProjectApartmentsService _apartmentsService;

        public ApartamentosController(IProjectApartmentsService apartmentsService)
        {
            _apartmentsService = apartmentsService;
        }

        [HttpGet("sheets")]
        public async Task<IActionResult> GetSheets(CancellationToken cancellationToken)
            => Ok(await _apartmentsService.GetSheetListAsync(cancellationToken));
        
    }
}
