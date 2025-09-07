using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace odm_api.Repositories
{
    public class LotRepository
    {
        private readonly DatabaseService _dbService;

        public LotRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private Lot MapReaderToLot(SqlDataReader reader)
        {
            var lot = new Lot
            {
                Id = (Guid)reader["ID"],
                CampagneId = reader["CampagneID"].ToString() ?? string.Empty,
                ExportateurId = (int)reader["ExportateurID"],
                ProductionId = reader["ProductionID"] != DBNull.Value ? (Guid?)reader["ProductionID"] : null,
                TypeLotId = reader["TypeLotID"] != DBNull.Value ? (int?)reader["TypeLotID"] : null,
                CertificationId = reader["CertificationID"] != DBNull.Value ? (int?)reader["CertificationID"] : null,
                Date = (DateTime)reader["DateLot"],
                DateProduction = reader["DateProduction"] != DBNull.Value ? (DateTime?)reader["DateProduction"] : null,
                NumeroLot = reader["NumeroLot"] != DBNull.Value ? reader["NumeroLot"].ToString() : null,
                NombreSacs = reader["NombreSacs"] != DBNull.Value ? (int?)reader["NombreSacs"] : null,
                PoidsBrut = reader["PoidsBrut"] != DBNull.Value ? (decimal?)reader["PoidsBrut"] : null,
                TareSacs = reader["TareSacs"] != DBNull.Value ? (decimal?)reader["TareSacs"] : null,
                TarePalettes = reader["TarePalettes"] != DBNull.Value ? (decimal?)reader["TarePalettes"] : null,
                PoidsNet = reader["PoidsNet"] != DBNull.Value ? (decimal?)reader["PoidsNet"] : null,
                EstQueue = (bool)reader["EstQueue"],
                EstManuel = (bool)reader["EstManuel"],
                EstReusine = reader["EstReusine"] != DBNull.Value ? (bool?)reader["EstReusine"] : null,
                Statut = reader["Statut"] != DBNull.Value ? reader["Statut"].ToString() : null,
                Desactive = (bool)reader["Desactive"],
                CreationUtilisateur = reader["CreationUtilisateur"].ToString() ?? string.Empty,
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"] != DBNull.Value ? reader["ModificationUtilisateur"].ToString() : null,
                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                RowVersionKey = reader["RowVersionKey"] as byte[],
                EstFictif = reader["EstFictif"] != DBNull.Value ? (bool?)reader["EstFictif"] : null,

                // Joined properties
                ExportateurNom = reader["ExportateurNom"] != DBNull.Value ? reader["ExportateurNom"].ToString() : null,
                NumeroProduction = reader["NumeroProduction"] != DBNull.Value ? reader["NumeroProduction"].ToString() : null,
                TypeLotDesignation = reader["TypeLotDesignation"] != DBNull.Value ? reader["TypeLotDesignation"].ToString() : null,
                CertificationDesignation = reader["CertificationDesignation"] != DBNull.Value ? reader["CertificationDesignation"].ToString() : null,
            };

            if (ColumnExists(reader, "EstQueueText"))
            {
                lot.EstQueueText = reader["EstQueueText"] != DBNull.Value ? reader["EstQueueText"].ToString() : null;
            }
            if (ColumnExists(reader, "EstManuelText"))
            {
                lot.EstManuelText = reader["EstManuelText"] != DBNull.Value ? reader["EstManuelText"].ToString() : null;
            }
            if (ColumnExists(reader, "EstReusineText"))
            {
                lot.EstReusineText = reader["EstReusineText"] != DBNull.Value ? reader["EstReusineText"].ToString() : null;
            }

            return lot;
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lots.Add(MapReaderToLot(reader));
                        }
                    }
                }
            }
            return lots;
        }

        public Lot? GetLotById(Guid id)
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
                            return MapReaderToLot(reader);
                        }
                    }
                }
            }
            return null;
        }
    }
}
