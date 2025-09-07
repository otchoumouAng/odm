using System;

namespace odm_api.Models
{
    public class MouvementStock
    {
        public Guid ID { get; set; }
        public int MagasinID { get; set; }
        public string MagasinNom { get; set; }
        public string CampagneID { get; set; }
        public int? ExportateurID { get; set; }
        public string ExportateurNom { get; set; }
        public int MouvementTypeID { get; set; }
        public string MouvementTypeDesignation { get; set; }
        public int? CertificationID { get; set; }
        public string CertificationDesignation { get; set; }
        public int? SacTypeID { get; set; }
        public string SacTypeDesignation { get; set; }
        public Guid? ObjetEnStockID { get; set; }
        public int? ObjetEnStockType { get; set; }
        public int? EmplacementID { get; set; }
        public string EmplacementDesignation { get; set; }
        public int SiteID { get; set; }
        public string SiteNom { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public DateTime DateMouvement { get; set; }
        public short Sens { get; set; }
        public int Quantite { get; set; }
        public decimal PoidsBrut { get; set; }
        public decimal TareSacs { get; set; }
        public decimal TarePalettes { get; set; }
        public decimal PoidsNetLivre { get; set; }
        public decimal RetentionPoids { get; set; }
        public decimal PoidsNetAccepte { get; set; }
        public string Statut { get; set; }
        public string Commentaire { get; set; }
        public bool? Desactive { get; set; }
        public string ApprobationUtilisateur { get; set; }
        public DateTime? ApprobationDate { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? ProduitID { get; set; }
    }
}