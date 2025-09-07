using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/magasin")]
    public class MagasinController : ControllerBase
    {
        private readonly MagasinRepository _repository;

        public MagasinController(MagasinRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var magasins = _repository.GetAllMagasins();
            return Ok(magasins);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var magasin = _repository.GetMagasinById(id);
            return magasin != null ? Ok(magasin) : NotFound();
        }
    }
}