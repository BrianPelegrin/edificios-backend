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
        public IActionResult GetSheets() => Ok( _apartmentsService.GetSheetList() );
        
    }
}
