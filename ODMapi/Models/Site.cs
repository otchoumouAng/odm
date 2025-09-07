namespace odm_api.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public bool Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUtilisateur { get; set; }
        public DateTime ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? PrefixeSite { get; set; }

        // Joined properties from SP
        public int? DestinationId { get; set; }
        public string? DestinationNom { get; set; }
        public int? ProvenanceId { get; set; }
        public string? ProvenanceNom { get; set; }
    }
}
