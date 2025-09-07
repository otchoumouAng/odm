using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using System;

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
            var transferts = _repository.GetAllTransfertLots();
            return Ok(transferts);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var transfert = _repository.GetTransfertLotById(id);
            return transfert != null ? Ok(transfert) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] TransfertLot transfertLot)
        {
            try
            {
                if (transfertLot == null)
                {
                    return BadRequest("TransfertLot object is null");
                }
                var createdTransfert = _repository.AddTransfertLot(transfertLot);
                return CreatedAtAction(nameof(GetById), new { id = createdTransfert.Id }, createdTransfert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] TransfertLot transfertLot)
        {
            try
            {
                if (transfertLot == null || transfertLot.Id != id)
                {
                    return BadRequest("Invalid data or ID mismatch.");
                }

                var existingTransfert = _repository.GetTransfertLotById(id);
                if (existingTransfert == null)
                {
                    return NotFound();
                }

                _repository.UpdateTransfertLot(transfertLot);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
