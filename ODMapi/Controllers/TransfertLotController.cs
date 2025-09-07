using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using System;
using System.Data;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/transfertlot")]
    public class TransfertLotController : ControllerBase
    {
        private readonly TransfertLotRepository _repository;

        public TransfertLotController(TransfertLotRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var transferts = _repository.GetAllTransferts();
            return Ok(transferts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var transfert = _repository.GetTransfertById(id);
            if (transfert == null)
            {
                return NotFound();
            }
            return Ok(transfert);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TransfertLot transfert)
        {
            try
            {
                _repository.AddTransfert(transfert);
                return CreatedAtAction(nameof(GetById), new { id = transfert.ID }, transfert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TransfertLot transfert)
        {
            if (transfert == null || transfert.ID != id)
            {
                return BadRequest("Invalid transfert data or ID mismatch.");
            }

            try
            {
                _repository.UpdateTransfert(transfert);
                return NoContent();
            }
            catch (DBConcurrencyException)
            {
                return Conflict(new { message = "Concurrency conflict: Transfert was modified by another user." });
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