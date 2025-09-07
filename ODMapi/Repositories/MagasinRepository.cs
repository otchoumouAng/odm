using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace odm_api.Repositories
{
    public class MagasinRepository
    {
        private readonly DatabaseService _dbService;

        public MagasinRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private Magasin MapReaderToMagasin(SqlDataReader reader)
        {
            // The select SP aliases maPontBascule to APontBascule
            // The get SP also aliases maPontBascule to APontBascule
            // The select SP aliases maLocalisation to MagasinLocalisation
            // The get SP also aliases maLocalisation to MagasinLocalisation
            return new Magasin
            {
                Id = (int)reader["ID"],
                Designation = reader["Designation"].ToString() ?? string.Empty,
                StockTypeId = (int)reader["StockTypeID"],
                StockTypeDesignation = reader["StockTypeDesignation"]?.ToString(),
                Localisation = reader["MagasinLocalisation"].ToString() ?? string.Empty,
                EstExterne = (bool)reader["EstExterne"],
                EstTransit = (bool)reader["EstTransit"],
                Desactive = (bool)reader["Desactive"],
                CreationUtilisateur = reader["CreationUtilisateur"].ToString() ?? string.Empty,
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"] != DBNull.Value ? reader["ModificationUtilisateur"].ToString() : null,
                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                RowVersionKey = reader["RowVersionKey"] as byte[],
                SiteId = reader["SiteID"] != DBNull.Value ? (int?)reader["SiteID"] : null,
                SiteNom = reader["SiteNom"]?.ToString(),
                PontBascule = reader["APontBascule"] != DBNull.Value ? (bool?)reader["APontBascule"] : null
            };
        }

        public List<Magasin> GetAllMagasins()
        {
            var magasins = new List<Magasin>();

            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_Magasin_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            magasins.Add(MapReaderToMagasin(reader));
                        }
                    }
                }
            }
            return magasins;
        }

        public Magasin? GetMagasinById(int id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_Magasin_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToMagasin(reader);
                        }
                    }
                }
            }
            return null;
        }
    }
}
