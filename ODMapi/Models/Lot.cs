namespace odm_api.Models
{
    public class Lot
    {
        public Guid Id { get; set; }
        public string CampagneId { get; set; }
        public int? SiteId { get; set; }
        public int? ProduitId { get; set; }
        public int ExportateurId { get; set; }
        public Guid? ProductionId { get; set; }
        public int? TypeLotId { get; set; }
        public int? MagasinId { get; set; }
        public int? CertificationId { get; set; }
        public Guid? EmbarquementId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateProduction { get; set; }
        public string? NumeroLot { get; set; }
        public int? NombreSacs { get; set; }
        public int? NombrePalette { get; set; }
        public decimal? PoidsBrut { get; set; }
        public decimal? TareSacs { get; set; }
        public decimal? TarePalettes { get; set; }
        public decimal? PoidsNet { get; set; }
        public bool EstQueue { get; set; }
        public bool EstManuel { get; set; }
        public bool? EstReusine { get; set; }
        public string? Statut { get; set; }
        public bool Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public bool? EstFictif { get; set; }
        public int? NombreSacsFictif { get; set; }
        public int? TypeChargementId { get; set; }
        public int? TypeElementEnStock { get; set; }
        public int? EstCertifie { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? DateApprobation { get; set; }
        public int? StockMagasinId { get; set; }
        public int? StockNombreSacs { get; set; }
        public int? StockNombrePalette { get; set; }
        public decimal? StockPoidsBrut { get; set; }
        public decimal? StockTareSacs { get; set; }
        public decimal? StockTarePalettes { get; set; }
        public decimal? StockPoidsNet { get; set; }
        public int? MagasinDestTheoriqueId { get; set; }
        public DateTime? Datereusinage { get; set; }
        public int? Nombresacsreusine { get; set; }
        public double? Poidsbrutreusinage { get; set; }
        public double? Poidsreusinage { get; set; }
        public double? Tarepalettereusinage { get; set; }
        public int? Id_Lot_Import { get; set; }
        public int? GradeLotId { get; set; }
        public int? TypeEmballageId { get; set; }

        // Joined properties from SP
        public string? ExportateurNom { get; set; }
        public string? NumeroProduction { get; set; }
        public string? TypeLotDesignation { get; set; }
        public string? CertificationDesignation { get; set; }
        public string? EstQueueText { get; set; }
        public string? EstManuelText { get; set; }
        public string? EstReusineText { get; set; }
    }
}
