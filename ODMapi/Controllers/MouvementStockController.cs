using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using System;
using System.Data;

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
            var mouvements = _repository.GetAllMouvements();
            return Ok(mouvements);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var mouvement = _repository.GetMouvementById(id);
            if (mouvement == null)
            {
                return NotFound();
            }
            return Ok(mouvement);
        }

        [HttpPost]
        public IActionResult Create([FromBody] MouvementStock mouvement)
        {
            try
            {
                _repository.AddMouvement(mouvement);
                return CreatedAtAction(nameof(GetById), new { id = mouvement.ID }, mouvement);
            }
            catch (Exception ex)
            {
                // You'll need to define specific exceptions for different error types
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MouvementStock mouvement)
        {
            if (mouvement == null || mouvement.ID != id)
            {
                return BadRequest("Invalid mouvement data or ID mismatch.");
            }

            try
            {
                _repository.UpdateMouvement(mouvement);
                return NoContent();
            }
            catch (DBConcurrencyException)
            {
                return Conflict(new { message = "Concurrency conflict: Mouvement was modified by another user." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
    }
}