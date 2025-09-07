using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace odm_api.Repositories
{
    public class SiteRepository
    {
        private readonly DatabaseService _dbService;

        public SiteRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private Site MapReaderToSite(SqlDataReader reader)
        {
            return new Site
            {
                Id = (int)reader["ID"],
                Nom = reader["Nom"].ToString() ?? string.Empty,
                Desactive = (bool)reader["Desactive"],
                CreationUtilisateur = reader["CreationUtilisateur"].ToString() ?? string.Empty,
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"].ToString() ?? string.Empty,
                ModificationDate = (DateTime)reader["ModificationDate"],
                RowVersionKey = reader["RowVersionKey"] as byte[],
                PrefixeSite = reader["PrefixeSite"] != DBNull.Value ? (int?)reader["PrefixeSite"] : null,
                DestinationId = reader["DestinationID"] != DBNull.Value ? (int?)reader["DestinationID"] : null,
                DestinationNom = reader["DestinationNom"] != DBNull.Value ? reader["DestinationNom"].ToString() : null,
                ProvenanceId = reader["ProvenanceID"] != DBNull.Value ? (int?)reader["ProvenanceID"] : null,
                ProvenanceNom = reader["ProvenanceNom"] != DBNull.Value ? reader["ProvenanceNom"].ToString() : null
            };
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sites.Add(MapReaderToSite(reader));
                        }
                    }
                }
            }
            return sites;
        }

        public Site? GetSiteById(int id)
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
                            return MapReaderToSite(reader);
                        }
                    }
                }
            }
            return null;
        }
    }
}
