using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/client")]
    public class ClientController : ControllerBase
    {
        private readonly ClientRepository _repository;

        public ClientController(ClientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = _repository.GetAllClients();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _repository.GetClientById(id);
            return client != null ? Ok(client) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Client client)
        {
            try
            {
                _repository.AddClient(client);
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                // Handle specific error cases
                if (ex.Message.Contains("already exists"))
                {
                    return Conflict(new { message = "Client already exists" });
                }
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Client client)
        {
            if (client == null || client.Id != id)
                return BadRequest("Invalid client data or ID mismatch.");

            var existingClient = _repository.GetClientById(id);
            if (existingClient == null)
                return NotFound();

            _repository.UpdateClient(client);
            return NoContent(); // 204
        }
    }
}