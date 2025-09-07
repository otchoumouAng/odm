using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/exportateur")]
    public class ExportateurController : ControllerBase
    {
        private readonly ExportateurRepository _repository;

        public ExportateurController(ExportateurRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var exportateurs = _repository.GetAllExportateurs();
            return Ok(exportateurs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var exportateur = _repository.GetExportateurById(id);
            return exportateur != null ? Ok(exportateur) : NotFound();
        }
    }
}