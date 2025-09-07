namespace odm_api.Models
{
    public class Exportateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string TelephoneFixe { get; set; }
        public string TelephoneMobile { get; set; }
        public string Fax { get; set; }
        public bool Desactive { get; set; }
        public string? PrefixeFacture { get; set; }
        public string? Prefixe { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
    }
}
