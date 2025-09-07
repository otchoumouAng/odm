using System;

namespace odm_api.Models
{
    public class MouvementStock
    {
        public Guid Id { get; set; }
        public int MagasinId { get; set; }
        public string CampagneId { get; set; }
        public int? ExportateurId { get; set; }
        public int MouvementTypeId { get; set; }
        public int? CertificationId { get; set; }
        public int? EmballageTypeId { get; set; }
        public Guid? ObjetEnStockId { get; set; }
        public int? ObjetEnStockType { get; set; }
        public int? EmplacementId { get; set; }
        public int SiteId { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public DateTime Date { get; set; }
        public short Sens { get; set; }
        public int Quantite { get; set; }
        public decimal PoidsBrut { get; set; }
        public decimal TareSacs { get; set; }
        public decimal TarePalettes { get; set; }
        public decimal PoidsNetLivre { get; set; }
        public decimal Retention { get; set; }
        public decimal PoidsNetAccepte { get; set; }
        public string Statut { get; set; }
        public string? Commentaire { get; set; }
        public bool? Desactive { get; set; }
        public string? Approbateur { get; set; }
        public DateTime? DateApprobation { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? ProduitId { get; set; }

        // Joined properties from SP for read operations
        public string? MagasinNom { get; set; }
        public string? MouvementTypeDesignation { get; set; }
        public string? ExportateurNom { get; set; }
        public string? CertificationDesignation { get; set; }
        public string? SacTypeDesignation { get; set; }
        public string? EmplacementDesignation { get; set; }
        public string? Reference3 { get; set; }
        public string? SiteNom { get; set; }
    }
}
