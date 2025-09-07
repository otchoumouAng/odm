using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/lot")]
    public class LotController : ControllerBase
    {
        private readonly LotRepository _repository;

        public LotController(LotRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lots = _repository.GetAllLots();
            return Ok(lots);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var lot = _repository.GetLotById(id);
            return lot != null ? Ok(lot) : NotFound();
        }
    }
}