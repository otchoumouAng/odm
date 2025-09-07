using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;

namespace odm_api.Repositories
{
    public class ExportateurRepository
    {
        private readonly DatabaseService _dbService;

        public ExportateurRepository(DatabaseService dbService)
        {
            _dbService = dbService;
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
                    cmd.Parameters.AddWithValue("@Status", -1);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exportateurs.Add(new Exportateur
                            {
                                ID = (int)reader["ID"],
                                Nom = reader["Nom"]?.ToString(),
                                Adresse = reader["Adresse"]?.ToString(),
                                TelephoneFixe = reader["TelephoneFixe"]?.ToString(),
                                TelephoneMobile = reader["TelephoneMobile"]?.ToString(),
                                Fax = reader["Fax"]?.ToString(),
                                PrefixeFacture = reader["PrefixeFacture"]?.ToString(),
                                Prefixe = reader["Prefixe"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[]
                            });
                        }
                    }
                }
            }
            return exportateurs;
        }

        public Exportateur GetExportateurById(int id)
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
                            return new Exportateur
                            {
                                ID = (int)reader["ID"],
                                Nom = reader["Nom"]?.ToString(),
                                Adresse = reader["Adresse"]?.ToString(),
                                TelephoneFixe = reader["TelephoneFixe"]?.ToString(),
                                TelephoneMobile = reader["TelephoneMobile"]?.ToString(),
                                Fax = reader["Fax"]?.ToString(),
                                PrefixeFacture = reader["PrefixeFacture"]?.ToString(),
                                Prefixe = reader["Prefixe"]?.ToString(),
                                Desactive = (bool)reader["Desactive"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] as DateTime?,
                                RowVersionKey = reader["RowVersionKey"] as byte[]
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}