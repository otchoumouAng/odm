using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace odm_api.Repositories
{
    public class ExportateurRepository
    {
        private readonly DatabaseService _dbService;

        public ExportateurRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private Exportateur MapReaderToExportateur(SqlDataReader reader)
        {
            return new Exportateur
            {
                Id = (int)reader["ID"],
                Nom = reader["Nom"].ToString() ?? string.Empty,
                Adresse = reader["Adresse"].ToString() ?? string.Empty,
                TelephoneFixe = reader["TelephoneFixe"].ToString() ?? string.Empty,
                TelephoneMobile = reader["TelephoneMobile"].ToString() ?? string.Empty,
                Fax = reader["Fax"].ToString() ?? string.Empty,
                Desactive = (bool)reader["Desactive"],
                PrefixeFacture = reader["PrefixeFacture"] != DBNull.Value ? reader["PrefixeFacture"].ToString() : null,
                Prefixe = reader["Prefixe"] != DBNull.Value ? reader["Prefixe"].ToString() : null,
                CreationUtilisateur = reader["CreationUtilisateur"].ToString() ?? string.Empty,
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"] != DBNull.Value ? reader["ModificationUtilisateur"].ToString() : null,
                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                RowVersionKey = reader["RowVersionKey"] as byte[]
            };
        }

        public List<Exportateur> GetAllExportateurs()
        {
            var exportateurs = new List<Exportateur>();
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("Exportateur_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exportateurs.Add(MapReaderToExportateur(reader));
                        }
                    }
                }
            }
            return exportateurs;
        }

        public Exportateur? GetExportateurById(int id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("Exportateur_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToExportateur(reader);
                        }
                    }
                }
            }
            return null;
        }
    }
}
