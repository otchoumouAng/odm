using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;

namespace odm_api.Repositories
{
    public class LotRepository
    {
        private readonly DatabaseService _dbService;

        public LotRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<Lot> GetAllLots()
        {
            var lots = new List<Lot>();

            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_Lot_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CampagneID", "-1");
                    cmd.Parameters.AddWithValue("@ExportateurID", -1);
                    cmd.Parameters.AddWithValue("@TypeLotID", -1);
                    cmd.Parameters.AddWithValue("@CertificationID", -1);
                    cmd.Parameters.AddWithValue("@dateDebut", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateFin", DBNull.Value);
                    cmd.Parameters.AddWithValue("@statut", "-1");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lots.Add(new Lot
                            {
                                ID = (Guid)reader["ID"],
                                CampagneID = reader["CampagneID"]?.ToString(),
                                ExportateurID = (int)reader["ExportateurID"],
                                ExportateurNom = reader["ExportateurNom"]?.ToString(),
                                ProductionID = reader["ProductionID"] as Guid?,
                                NumeroProduction = reader["NumeroProduction"]?.ToString(),
                                TypeLotID = reader["TypeLotID"] as int?,
                                TypeLotDesignation = reader["TypeLotDesignation"]?.ToString(),
                                CertificationID = reader["CertificationID"] as int?,
                                CertificationDesignation = reader["CertificationDesignation"]?.ToString(),
                                DateLot = (DateTime)reader["DateLot"],
                                DateProduction = reader["DateProduction"] as DateTime?,
                                NumeroLot = reader["NumeroLot"]?.ToString(),
                                NombreSacs = reader["NombreSacs"] as int?,
                                PoidsBrut = reader["PoidsBrut"] as decimal?,
                                TareSacs = reader["TareSacs"] as decimal?,
                                TarePalettes = reader["TarePalettes"] as decimal?,
                                PoidsNet = reader["PoidsNet"] as decimal?,
                                EstQueue = (bool)reader["EstQueue"],
                                EstManuel = (bool)reader["EstManuel"],
                                EstReusine = reader["EstReusine"] as bool?,
                                Statut = reader["Statut"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                EstQueueText = reader["EstQueueText"]?.ToString(),
                                EstManuelText = reader["EstManuelText"]?.ToString(),
                                EstReusineText = reader["EstReusineText"]?.ToString(),
                                EstFictif = reader["EstFictif"] as bool?
                            });
                        }
                    }
                }
            }
            return lots;
        }

        public Lot GetLotById(Guid id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_Lot_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Lot
                            {
                                ID = (Guid)reader["ID"],
                                CampagneID = reader["CampagneID"]?.ToString(),
                                ExportateurID = (int)reader["ExportateurID"],
                                ExportateurNom = reader["ExportateurNom"]?.ToString(),
                                ProductionID = reader["ProductionID"] as Guid?,
                                NumeroProduction = reader["NumeroProduction"]?.ToString(),
                                TypeLotID = reader["TypeLotID"] as int?,
                                TypeLotDesignation = reader["TypeLotDesignation"]?.ToString(),
                                CertificationID = reader["CertificationID"] as int?,
                                CertificationDesignation = reader["CertificationDesignation"]?.ToString(),
                                DateLot = (DateTime)reader["DateLot"],
                                DateProduction = reader["DateProduction"] as DateTime?,
                                NumeroLot = reader["NumeroLot"]?.ToString(),
                                NombreSacs = reader["NombreSacs"] as int?,
                                PoidsBrut = reader["PoidsBrut"] as decimal?,
                                TareSacs = reader["TareSacs"] as decimal?,
                                TarePalettes = reader["TarePalettes"] as decimal?,
                                PoidsNet = reader["PoidsNet"] as decimal?,
                                EstQueue = (bool)reader["EstQueue"],
                                EstManuel = (bool)reader["EstManuel"],
                                EstReusine = reader["EstReusine"] as bool?,
                                Statut = reader["Statut"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                EstFictif = reader["EstFictif"] as bool?
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}