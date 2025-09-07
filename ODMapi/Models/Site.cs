namespace odm_api.Models
{
    public class Site
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        public int? ProvenanceID { get; set; }
        public string ProvenanceNom { get; set; }
        public int? DestinationID { get; set; }
        public string DestinationNom { get; set; }
        public bool Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUtilisateur { get; set; }
        public DateTime ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public int? PrefixeSite { get; set; }
    }
}