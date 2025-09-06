using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace odm_api.Repositories
{
    public class ClientRepository
    {
        private readonly DatabaseService _dbService;

        public ClientRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();

            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("pv_Client_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                Id = (int)reader["Id"],
                                Nom = reader["Nom"]?.ToString() ?? string.Empty,
                                Adresse = reader["Adresse"]?.ToString() ?? string.Empty,
                                TelephoneFixe = reader["TelephoneFixe"]?.ToString() ?? string.Empty,
                                TelephoneMobile = reader["TelephoneMobile"]?.ToString() ?? string.Empty,
                                Fax = reader["Fax"]?.ToString() ?? string.Empty,
                                RowVersionKey = (byte[])reader["RowVersionKey"],
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString() ?? string.Empty,
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }
            return clients;
        }

        public Client GetClientById(int id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("pv_Client_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            {
                                Id = (int)reader["Id"],
                                Nom = reader["Nom"]?.ToString() ?? string.Empty,
                                Adresse = reader["Adresse"]?.ToString() ?? string.Empty,
                                TelephoneFixe = reader["TelephoneFixe"]?.ToString() ?? string.Empty,
                                TelephoneMobile = reader["TelephoneMobile"]?.ToString() ?? string.Empty,
                                Fax = reader["Fax"]?.ToString() ?? string.Empty,
                                CreationUtilisateur = reader["CreationUtilisateur"]?.ToString() ?? string.Empty,
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString() ?? string.Empty,
                                RowVersionKey = (byte[])reader["RowVersionKey"]
                            };
                        }
                    }
                }
            }
            return new Client();
        }

        public void AddClient(Client client)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("pv_Client_New", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@nom", client.Nom);
                    cmd.Parameters.AddWithValue("@adresse", client.Adresse);
                    cmd.Parameters.AddWithValue("@telephonefixe", client.TelephoneFixe);
                    cmd.Parameters.AddWithValue("@telephonemobile", client.TelephoneMobile);
                    cmd.Parameters.AddWithValue("@fax", client.Fax);
                    cmd.Parameters.AddWithValue("@CreationUser", client.CreationUtilisateur);

                    // Output parameters
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output });
                    
                    // Return value
                    cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

                    cmd.ExecuteNonQuery();

                    // Get results
                    int returnValue = (int)cmd.Parameters["@ReturnValue"].Value;
                    string errorMessage = cmd.Parameters["@ErrorMessage"].Value?.ToString();

                    if (returnValue == -1 && !string.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception(errorMessage);
                    }
                    else if (returnValue != 0)
                    {
                        throw new Exception($"Unexpected return value: {returnValue}");
                    }

                    // Set client properties
                    client.Id = (int)cmd.Parameters["@ID"].Value;
                    client.RowVersionKey = (byte[])cmd.Parameters["@RowVersion"].Value;
                }
            }
        }
        
        public void UpdateClient(Client client)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("pv_Client_Modify", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = client.Id;
                    cmd.Parameters.Add("@nom", SqlDbType.VarChar, 100).Value = (object)client.Nom ?? DBNull.Value;
                    cmd.Parameters.Add("@adresse", SqlDbType.VarChar, 100).Value = (object)client.Adresse ?? DBNull.Value;
                    cmd.Parameters.Add("@telephonefixe", SqlDbType.VarChar, 100).Value = (object)client.TelephoneFixe ?? DBNull.Value;
                    cmd.Parameters.Add("@telephonemobile", SqlDbType.VarChar, 100).Value = (object)client.TelephoneMobile ?? DBNull.Value;
                    cmd.Parameters.Add("@fax", SqlDbType.VarChar, 100).Value = (object)client.Fax ?? DBNull.Value;
                    cmd.Parameters.Add("@ModificationUser", SqlDbType.VarChar, 50).Value = (object)client.ModificationUtilisateur ?? DBNull.Value;

                    // RowVersion param: InputOutput (client sends current rowversion; SP returns new rowversion in same param)
                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Binary, 8);
                    rvParam.Direction = ParameterDirection.InputOutput;
                    rvParam.Value = (object)client.RowVersionKey ?? DBNull.Value; // client.RowVersionKey doit être byte[]

                    // Error message output
                    var errParam = cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                    errParam.Direction = ParameterDirection.Output;

                    // Return value
                    var returnParam = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    returnParam.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    int returnValue = returnParam.Value != DBNull.Value ? Convert.ToInt32(returnParam.Value) : 0;
                    string errorMessage = errParam.Value != DBNull.Value ? errParam.Value.ToString() : null;

                    if (returnValue == -1)
                    {
                        throw new DBConcurrencyException(errorMessage ?? "Concurrency conflict: Client was modified by another user");
                    }
                    else if (returnValue == -2)
                    {
                        throw new KeyNotFoundException(errorMessage ?? "Client not found");
                    }
                    else if (returnValue != 0)
                    {
                        throw new Exception(errorMessage ?? $"Update failed with return code {returnValue}");
                    }

                    // récupérer la nouvelle RowVersion renvoyée dans le même paramètre
                    if (rvParam.Value != DBNull.Value)
                    {
                        client.RowVersionKey = (byte[])rvParam.Value;
                    }
                }
            }
        }

    }
}