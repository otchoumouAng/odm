using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;

namespace odm_api.Repositories
{
    public class SiteRepository
    {
        private readonly DatabaseService _dbService;

        public SiteRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<Site> GetAllSites()
        {
            var sites = new List<Site>();

            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("Site_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", -1);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sites.Add(new Site
                            {
                                ID = (int)reader["ID"],
                                Nom = reader["Nom"]?.ToString(),
                                DestinationID = reader["DestinationID"] as int?,
                                DestinationNom = reader["DestinationNom"]?.ToString(),
                                ProvenanceID = reader["ProvenanceID"] as int?,
                                ProvenanceNom = reader["ProvenanceNom"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = (DateTime)reader["ModificationDate"],
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                PrefixeSite = reader["PrefixeSite"] as int?
                            });
                        }
                    }
                }
            }
            return sites;
        }

        public Site GetSiteById(int id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("Site_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Site
                            {
                                ID = (int)reader["ID"],
                                Nom = reader["Nom"]?.ToString(),
                                DestinationID = reader["DestinationID"] as int?,
                                DestinationNom = reader["DestinationNom"]?.ToString(),
                                ProvenanceID = reader["ProvenanceID"] as int?,
                                ProvenanceNom = reader["ProvenanceNom"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = (DateTime)reader["ModificationDate"],
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                PrefixeSite = reader["PrefixeSite"] as int?
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}