using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;

namespace odm_api.Repositories
{
    public class MagasinRepository
    {
        private readonly DatabaseService _dbService;

        public MagasinRepository(DatabaseService dbService)
        {
            _dbService = dbService;
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
                    cmd.Parameters.AddWithValue("@Status", -1);
                    cmd.Parameters.AddWithValue("@SiteID", -1);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            magasins.Add(new Magasin
                            {
                                ID = (int)reader["ID"],
                                Designation = reader["Designation"]?.ToString(),
                                StockTypeID = (int)reader["StockTypeID"],
                                StockTypeDesignation = reader["StockTypeDesignation"]?.ToString(),
                                MagasinLocalisation = reader["MagasinLocalisation"]?.ToString(),
                                EstExterne = (bool)reader["EstExterne"],
                                EstTransit = (bool)reader["EstTransit"],
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                SiteID = reader["SiteID"] as int?,
                                SiteNom = reader["SiteNom"]?.ToString(),
                                PontBascule = reader["APontBascule"] as bool?
                            });
                        }
                    }
                }
            }
            return magasins;
        }

        public Magasin GetMagasinById(int id)
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
                            return new Magasin
                            {
                                ID = (int)reader["ID"],
                                Designation = reader["Designation"]?.ToString(),
                                StockTypeID = (int)reader["StockTypeID"],
                                StockTypeDesignation = reader["StockTypeDesignation"]?.ToString(),
                                MagasinLocalisation = reader["MagasinLocalisation"]?.ToString(),
                                EstExterne = (bool)reader["EstExterne"],
                                EstTransit = (bool)reader["EstTransit"],
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                SiteID = reader["SiteID"] as int?,
                                SiteNom = reader["SiteNom"]?.ToString(),
                                PontBascule = reader["APontBascule"] as bool?
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}