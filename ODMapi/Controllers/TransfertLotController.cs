using Microsoft.AspNetCore.Mvc;
using odm_api.Models;
using odm_api.Repositories;
using odm_api.Models.DTOs;
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
        public IActionResult Create([FromBody] TransfertLotDto dto)
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

                var transfert = new TransfertLot
                {
                    CampagneID = dto.CampagneID,
                    SiteID = dto.SiteID,
                    LotID = dto.LotID,
                    NumeroLot = dto.NumeroLot,
                    NumBordereauExpedition = dto.NumBordereauExpedition,
                    MagasinExpeditionID = dto.MagasinExpeditionID,
                    NombreSacsExpedition = dto.NombreSacsExpedition,
                    NombrePaletteExpedition = dto.NombrePaletteExpedition,
                    TareSacsExpedition = dto.TareSacsExpedition,
                    TarePaletteExpedition = dto.TarePaletteExpedition,
                    PoidsBrutExpedition = dto.PoidsBrutExpedition,
                    PoidsNetExpedition = dto.PoidsNetExpedition,
                    ImmTracteurExpedition = dto.ImmTracteurExpedition,
                    ImmRemorqueExpedition = dto.ImmRemorqueExpedition,
                    DateExpedition = dto.DateExpedition,
                    CommentaireExpedition = dto.CommentaireExpedition,
                    Statut = dto.Statut,
                    MagReceptionTheoID = dto.MagReceptionTheoID,
                    CreationUtilisateur = dto.CreationUtilisateur
                };

                _repository.AddTransfert(transfert);
                return CreatedAtAction(nameof(GetById), new { id = transfert.ID }, transfert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur interne du serveur: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TransfertLotDto dto)
        {
            if (string.IsNullOrEmpty(dto.ModificationUtilisateur))
            {
                return BadRequest("ModificationUtilisateur est requis pour la mise à jour.");
            }
            if (dto.RowVersionKey == null)
            {
                return BadRequest("RowVersionKey est requis pour la mise à jour.");
            }
            if (!string.IsNullOrEmpty(dto.CreationUtilisateur))
            {
                return BadRequest("Les champs de création ne doivent pas être inclus pour la mise à jour.");
            }

            try
            {
                var existingTransfert = _repository.GetTransfertById(id);
                if (existingTransfert == null)
                {
                    return NotFound();
                }

                existingTransfert.CampagneID = dto.CampagneID;
                existingTransfert.SiteID = dto.SiteID;
                existingTransfert.LotID = dto.LotID;
                existingTransfert.NumeroLot = dto.NumeroLot;
                existingTransfert.NumBordereauExpedition = dto.NumBordereauExpedition;
                existingTransfert.MagasinExpeditionID = dto.MagasinExpeditionID;
                existingTransfert.NombreSacsExpedition = dto.NombreSacsExpedition;
                existingTransfert.NombrePaletteExpedition = dto.NombrePaletteExpedition;
                existingTransfert.TareSacsExpedition = dto.TareSacsExpedition;
                existingTransfert.TarePaletteExpedition = dto.TarePaletteExpedition;
                existingTransfert.PoidsBrutExpedition = dto.PoidsBrutExpedition;
                existingTransfert.PoidsNetExpedition = dto.PoidsNetExpedition;
                existingTransfert.ImmTracteurExpedition = dto.ImmTracteurExpedition;
                existingTransfert.ImmRemorqueExpedition = dto.ImmRemorqueExpedition;
                existingTransfert.DateExpedition = dto.DateExpedition;
                existingTransfert.CommentaireExpedition = dto.CommentaireExpedition;
                existingTransfert.Statut = dto.Statut;
                existingTransfert.MagReceptionTheoID = dto.MagReceptionTheoID;
                existingTransfert.ModificationUtilisateur = dto.ModificationUtilisateur;
                existingTransfert.RowVersionKey = dto.RowVersionKey;

                _repository.UpdateTransfert(existingTransfert);
                return NoContent();
            }
            catch (DBConcurrencyException)
            {
                return Conflict(new { message = "Conflit de concurrence: le transfert a été modifié par un autre utilisateur." });
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
