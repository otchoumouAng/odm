using System;

namespace odm_api.Models
{
    public class Lot
    {
        public Guid ID { get; set; }
        public string CampagneID { get; set; }
        public int ExportateurID { get; set; }
        public string ExportateurNom { get; set; }
        public Guid? ProductionID { get; set; }
        public string NumeroProduction { get; set; }
        public int? TypeLotID { get; set; }
        public string TypeLotDesignation { get; set; }
        public int? CertificationID { get; set; }
        public string CertificationDesignation { get; set; }
        public DateTime DateLot { get; set; }
        public DateTime? DateProduction { get; set; }
        public string NumeroLot { get; set; }
        public int? NombreSacs { get; set; }
        public decimal? PoidsBrut { get; set; }
        public decimal? TareSacs { get; set; }
        public decimal? TarePalettes { get; set; }
        public decimal? PoidsNet { get; set; }
        public bool EstQueue { get; set; }
        public bool EstManuel { get; set; }
        public bool? EstReusine { get; set; }
        public string Statut { get; set; }
        public bool Desactive { get; set; }
        public string CreationUtilisateur { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUtilisateur { get; set; }
        public DateTime? ModificationDate { get; set; }
        public byte[]? RowVersionKey { get; set; }
        public string EstQueueText { get; set; }
        public string EstManuelText { get; set; }
        public string EstReusineText { get; set; }
        public bool? EstFictif { get; set; }
    }
}