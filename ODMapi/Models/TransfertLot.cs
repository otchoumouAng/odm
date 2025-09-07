using System;

namespace odm_api.Models
{
    public class TransfertLot
    {
        public Guid Id { get; set; }
        public string CampagneId { get; set; }
        public int SiteId { get; set; }
        public Guid LotId { get; set; }
        public string? NumeroExpedition { get; set; }
        public string? NumBordereauExpedition { get; set; }
        public int? MagasinExpeditionId { get; set; }
        public int? NombreSacsExpedition { get; set; }
        public int? NombrePaletteExpedition { get; set; }
        public decimal? TareSacsExpedition { get; set; }
        public decimal? TarePaletteExpedition { get; set; }
        public decimal? PoidsBrutExpedition { get; set; }
        public decimal? PoidsNetExpedition { get; set; }
        public string? ImmTracteurExpedition { get; set; }
        public string? ImmRemorqueExpedition { get; set; }
        public DateTime? DateExpedition { get; set; }
        public string? CommentaireExpedition { get; set; }
        public string? NumBordereauReception { get; set; }
        public int? MagasinReceptionId { get; set; }
        public int? NombreSacsReception { get; set; }
        public int? NombrePaletteReception { get; set; }
        public decimal? PoidsNetReception { get; set; }
        public decimal? PoidsBrutReception { get; set; }
        public decimal? TareSacsReception { get; set; }
        public decimal? TarePaletteReception { get; set; }
        public string? ImmTracteurReception { get; set; }
        public string? ImmRemorqueReception { get; set; }
        public string? CommentaireReception { get; set; }
        public DateTime? DateReception { get; set; }
        public string? Statut { get; set; }
        public bool? Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? MagReceptionTheo { get; set; }

        // Joined properties from SP for read operations
        public string? SiteNom { get; set; }
        public string? MagasinExpeditionNom { get; set; }
        public string? MagasinReceptionNom { get; set; }
        public string? NumeroLot { get; set; }
        public int? ExportateurId { get; set; }
        public string? ExportateurNom { get; set; }
        public string? MagReceptionTheoNom { get; set; }
    }
}
