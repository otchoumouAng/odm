using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using System;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/mouvementstock")]
    public class MouvementStockController : ControllerBase
    {
        private readonly MouvementStockRepository _repository;

        public MouvementStockController(MouvementStockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var mouvements = _repository.GetAllMouvementsStock();
            return Ok(mouvements);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var mouvement = _repository.GetMouvementStockById(id);
            return mouvement != null ? Ok(mouvement) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] MouvementStock mouvementStock)
        {
            try
            {
                if (mouvementStock == null)
                {
                    return BadRequest("MouvementStock object is null");
                }
                var createdMouvement = _repository.AddMouvementStock(mouvementStock);
                return CreatedAtAction(nameof(GetById), new { id = createdMouvement.Id }, createdMouvement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] MouvementStock mouvementStock)
        {
            try
            {
                if (mouvementStock == null || mouvementStock.Id != id)
                {
                    return BadRequest("Invalid data or ID mismatch.");
                }

                // The repository's Update method will handle the concurrency and existence check.
                // For an extra layer of safety, we could check for existence here first.
                var existingMouvement = _repository.GetMouvementStockById(id);
                if (existingMouvement == null)
                {
                    return NotFound();
                }

                _repository.UpdateMouvementStock(mouvementStock);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Specific error handling can be added here, e.g., for concurrency exceptions
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
