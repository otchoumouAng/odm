using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using odm_api.Models.DTOs;
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
        public IActionResult Create([FromBody] MouvementStockDto dto)
        {
            try
            {
                // Validation côté serveur pour la création
                if (string.IsNullOrEmpty(dto.CreationUtilisateur))
                {
                    return BadRequest("CreationUtilisateur est requis pour la création.");
                }
                if (!string.IsNullOrEmpty(dto.ModificationUtilisateur) || dto.RowVersionKey != null)
                {
                    return BadRequest("Les champs de modification ne doivent pas être inclus pour la création.");
                }

                var mouvement = new MouvementStock
                {
                    MagasinID = dto.MagasinID,
                    CampagneID = dto.CampagneID,
                    ExportateurID = dto.ExportateurID,
                    MouvementTypeID = dto.MouvementTypeID,
                    ObjetEnStockID = dto.ObjetEnStockID,
                    ObjetEnStockType = dto.ObjetEnStockType,
                    EmplacementID = dto.EmplacementID,
                    SiteID = dto.SiteID,
                    Reference1 = dto.Reference1,
                    Reference2 = dto.Reference2,
                    DateMouvement = dto.DateMouvement,
                    Sens = dto.Sens,
                    Quantite = dto.Quantite,
                    PoidsBrut = dto.PoidsBrut,
                    TareSacs = dto.TareSacs,
                    TarePalettes = dto.TarePalettes,
                    PoidsNetLivre = dto.PoidsNetLivre,
                    RetentionPoids = dto.RetentionPoids,
                    PoidsNetAccepte = dto.PoidsNetAccepte,
                    Statut = dto.Statut,
                    Commentaire = dto.Commentaire,
                    CreationUtilisateur = dto.CreationUtilisateur,
                    ProduitID = dto.ProduitID,
                    CertificationID = dto.CertificationID
                };

                _repository.AddMouvement(mouvement);
                return CreatedAtAction(nameof(GetById), new { id = mouvement.ID }, mouvement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MouvementStockDto dto)
        {
            try
            {
                // Validation côté serveur pour la mise à jour
                if (string.IsNullOrEmpty(dto.ModificationUtilisateur))
                {
                    return BadRequest("ModificationUtilisateur est requis pour la mise à jour.");
                }
                if (dto.RowVersionKey == null)
                {
                    return BadRequest("RowVersionKey est requis pour les opérations de mise à jour.");
                }
                if (!string.IsNullOrEmpty(dto.CreationUtilisateur))
                {
                    return BadRequest("Les champs de création ne doivent pas être inclus pour la mise à jour.");
                }

                // Récupérer l'objet existant pour préserver les propriétés de recherche
                var existingMouvement = _repository.GetMouvementById(id);
                if (existingMouvement == null)
                {
                    return NotFound();
                }

                // Mettre à jour les propriétés mutables à partir du DTO
                existingMouvement.MagasinID = dto.MagasinID;
                existingMouvement.CampagneID = dto.CampagneID;
                existingMouvement.ExportateurID = dto.ExportateurID;
                existingMouvement.MouvementTypeID = dto.MouvementTypeID;
                existingMouvement.EmplacementID = dto.EmplacementID;
                existingMouvement.DateMouvement = dto.DateMouvement;
                existingMouvement.Sens = dto.Sens;
                existingMouvement.Quantite = dto.Quantite;
                existingMouvement.PoidsBrut = dto.PoidsBrut;
                existingMouvement.TareSacs = dto.TareSacs;
                existingMouvement.TarePalettes = dto.TarePalettes;
                existingMouvement.PoidsNetLivre = dto.PoidsNetLivre;
                existingMouvement.RetentionPoids = dto.RetentionPoids;
                existingMouvement.PoidsNetAccepte = dto.PoidsNetAccepte;
                existingMouvement.Commentaire = dto.Commentaire;
                existingMouvement.ModificationUtilisateur = dto.ModificationUtilisateur;
                existingMouvement.RowVersionKey = dto.RowVersionKey;
                
                _repository.UpdateMouvement(existingMouvement);
                return NoContent();
            }
            catch (DBConcurrencyException)
            {
                return Conflict(new { message = "Conflit de concurrence : le mouvement a été modifié par un autre utilisateur." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur: " + ex.Message });
            }
        }
    }
}
