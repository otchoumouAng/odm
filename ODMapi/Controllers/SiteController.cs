using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;

namespace odm_api.Controllers
{
    [ApiController]
    [Route("/api/site")]
    public class SiteController : ControllerBase
    {
        private readonly SiteRepository _repository;

        public SiteController(SiteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var sites = _repository.GetAllSites();
            return Ok(sites);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var site = _repository.GetSiteById(id);
            return site != null ? Ok(site) : NotFound();
        }
    }
}