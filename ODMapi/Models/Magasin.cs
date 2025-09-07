namespace odm_api.Models
{
    public class Magasin
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public int StockTypeId { get; set; }
        public string? StockTypeDesignation { get; set; }
        public string Localisation { get; set; }
        public bool EstExterne { get; set; }
        public bool EstTransit { get; set; }
        public bool Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? SiteId { get; set; }
        public string? SiteNom { get; set; }
        public bool? EstMagasinParDefaut { get; set; }
        public bool? VisibleInStockDashboard { get; set; }
        public bool? Visible { get; set; }
        public bool? PontBascule { get; set; }
    }
}
