namespace odm_api.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string TelephoneFixe { get; set; }
        public string TelephoneMobile { get; set; }
        public string Fax { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public string CreationUtilisateur { get; set; }
        public string ModificationUtilisateur { get; set; }
    }
}