using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;
using System.Data.SqlTypes;

namespace odm_api.Repositories
{
    public class TransfertLotRepository
    {
        private readonly DatabaseService _dbService;

        public TransfertLotRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<TransfertLot> GetAllTransferts()
        {
            var transferts = new List<TransfertLot>();
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CropYear", "{Tous}");
                    cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@siteID", -1);
                    cmd.Parameters.AddWithValue("@ExportateurID", -1);
                    cmd.Parameters.AddWithValue("@statut", "-1");
                    cmd.Parameters.AddWithValue("@MagasinExpeditionID", -1);
                    cmd.Parameters.AddWithValue("@MagasinReceptionID", -1);
                    cmd.Parameters.AddWithValue("@EnTransit", 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transferts.Add(MapTransfertLotFromReader(reader));
                        }
                    }
                }
            }
            return transferts;
        }

        public TransfertLot GetTransfertById(Guid id)
        {
            // Note: A dedicated SP for GetById is recommended for performance.
            // This implementation filters the result of GetAllTransferts().
            return GetAllTransferts().FirstOrDefault(t => t.ID == id);
        }

        public void AddTransfert(TransfertLot transfert)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_New", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@campagneID", transfert.CampagneID);
                    cmd.Parameters.AddWithValue("@siteID", transfert.SiteID);
                    cmd.Parameters.AddWithValue("@LotID", transfert.LotID);
                    cmd.Parameters.AddWithValue("@NumBordereauExpedition", (object)transfert.NumBordereauExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinExpeditionID", (object)transfert.MagasinExpeditionID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nombreSacs", (object)transfert.NombreSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombrePalette", (object)transfert.NombrePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TareSac", (object)transfert.TareSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TarePalette", (object)transfert.TarePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsBrut", (object)transfert.PoidsBrutExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsNet", (object)transfert.PoidsNetExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmTracteur", (object)transfert.ImmTracteurExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmRemorque", (object)transfert.ImmRemorqueExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateExpedition", (object)transfert.DateExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Commentaire", (object)transfert.CommentaireExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@statut", (object)transfert.Statut ?? "NA");
                    cmd.Parameters.AddWithValue("@magasinTheoReceptionID", (object)transfert.MagReceptionTheoID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreationUser", (object)transfert.CreationUtilisateur ?? DBNull.Value);

                    // Output parameters
                    var idParam = cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });
                    var numeroExpeditionParam = cmd.Parameters.Add(new SqlParameter("@numeroExpedition", SqlDbType.VarChar, 10) { Direction = ParameterDirection.Output });
                    var rvParam = cmd.Parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Direction = ParameterDirection.Output });
                    var errParam = cmd.Parameters.Add(new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 1000) { Direction = ParameterDirection.Output });
                    var retParam = cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

                    cmd.ExecuteNonQuery();

                    // Get results
                    int returnValue = (int)retParam.Value;
                    string errorMessage = errParam.Value?.ToString();

                    if (returnValue == -1 && !string.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception(errorMessage);
                    }
                    else if (returnValue != 0)
                    {
                        throw new Exception($"Unexpected return value: {returnValue}");
                    }

                    // Set transfert properties
                    transfert.ID = idParam.Value is DBNull ? Guid.Empty : (Guid)idParam.Value;
                    transfert.NumeroExpedition = numeroExpeditionParam.Value?.ToString();
                    transfert.RowVersionKey = rvParam.Value is DBNull ? null : (byte[])rvParam.Value;
                }
            }
        }

        public void UpdateTransfert(TransfertLot transfert)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_Modify", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@ID", transfert.ID);
                    cmd.Parameters.AddWithValue("@campagneID", transfert.CampagneID);
                    cmd.Parameters.AddWithValue("@siteID", transfert.SiteID);
                    cmd.Parameters.AddWithValue("@LotID", transfert.LotID);
                    cmd.Parameters.AddWithValue("@numerolot", (object)transfert.NumeroLot ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NumBordereauExpedition", (object)transfert.NumBordereauExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinExpeditionID", (object)transfert.MagasinExpeditionID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nombreSacs", (object)transfert.NombreSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombrePalette", (object)transfert.NombrePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TareSac", (object)transfert.TareSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TarePalette", (object)transfert.TarePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsBrut", (object)transfert.PoidsBrutExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsNet", (object)transfert.PoidsNetExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmTracteur", (object)transfert.ImmTracteurExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmRemorque", (object)transfert.ImmRemorqueExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateExpedition", (object)transfert.DateExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Commentaire", (object)transfert.CommentaireExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@statut", (object)transfert.Statut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinTheoReceptionID", (object)transfert.MagReceptionTheoID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreationUser", (object)transfert.CreationUtilisateur ?? DBNull.Value);

                    // RowVersion param: InputOutput
                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp);
                    rvParam.Direction = ParameterDirection.InputOutput;
                    rvParam.Value = (object)transfert.RowVersionKey ?? DBNull.Value;

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
                        throw new DBConcurrencyException(errorMessage ?? "Concurrency conflict: Transfert was modified by another user.");
                    }
                    else if (returnValue != 0)
                    {
                        throw new Exception(errorMessage ?? $"Update failed with return code {returnValue}");
                    }

                    // Get new RowVersion
                    if (rvParam.Value != DBNull.Value)
                    {
                        transfert.RowVersionKey = (byte[])rvParam.Value;
                    }
                }
            }
        }
        
        private TransfertLot MapTransfertLotFromReader(SqlDataReader reader)
        {
            return new TransfertLot
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("ID")),
                CampagneID = reader.IsDBNull(reader.GetOrdinal("CampagneID")) ? null : reader.GetString(reader.GetOrdinal("CampagneID")),
                SiteID = reader.IsDBNull(reader.GetOrdinal("SiteID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SiteID")),
                SiteNom = reader.IsDBNull(reader.GetOrdinal("SiteNom")) ? null : reader.GetString(reader.GetOrdinal("SiteNom")),
                LotID = reader.IsDBNull(reader.GetOrdinal("LotID")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("LotID")),
                NumeroLot = reader.IsDBNull(reader.GetOrdinal("NumeroLot")) ? null : reader.GetString(reader.GetOrdinal("NumeroLot")),
                ExportateurID = reader.IsDBNull(reader.GetOrdinal("ExportateurID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExportateurID")),
                ExportateurNom = reader.IsDBNull(reader.GetOrdinal("ExportateurNom")) ? null : reader.GetString(reader.GetOrdinal("ExportateurNom")),
                NumeroExpedition = reader.IsDBNull(reader.GetOrdinal("NumeroExpedition")) ? null : reader.GetString(reader.GetOrdinal("NumeroExpedition")),
                NumBordereauExpedition = reader.IsDBNull(reader.GetOrdinal("NumBordereauExpedition")) ? null : reader.GetString(reader.GetOrdinal("NumBordereauExpedition")),
                MagasinExpeditionID = reader.IsDBNull(reader.GetOrdinal("MagasinExpeditionID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("MagasinExpeditionID")),
                MagasinExpeditionNom = reader.IsDBNull(reader.GetOrdinal("MagasinExpeditionNom")) ? null : reader.GetString(reader.GetOrdinal("MagasinExpeditionNom")),
                NombreSacsExpedition = reader.IsDBNull(reader.GetOrdinal("NombreSacsExpedition")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("NombreSacsExpedition")),
                NombrePaletteExpedition = reader.IsDBNull(reader.GetOrdinal("NombrePaletteExpedition")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("NombrePaletteExpedition")),
                TareSacsExpedition = reader.IsDBNull(reader.GetOrdinal("TareSacsExpedition")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("TareSacsExpedition")),
                TarePaletteExpedition = reader.IsDBNull(reader.GetOrdinal("TarePaletteExpedition")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("TarePaletteExpedition")),
                PoidsBrutExpedition = reader.IsDBNull(reader.GetOrdinal("PoidsBrutExpedition")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PoidsBrutExpedition")),
                PoidsNetExpedition = reader.IsDBNull(reader.GetOrdinal("PoidsNetExpedition")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PoidsNetExpedition")),
                ImmTracteurExpedition = reader.IsDBNull(reader.GetOrdinal("ImmTracteurExpedition")) ? null : reader.GetString(reader.GetOrdinal("ImmTracteurExpedition")),
                ImmRemorqueExpedition = reader.IsDBNull(reader.GetOrdinal("ImmRemorqueExpedition")) ? null : reader.GetString(reader.GetOrdinal("ImmRemorqueExpedition")),
                DateExpedition = reader.IsDBNull(reader.GetOrdinal("DateExpedition")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DateExpedition")),
                CommentaireExpedition = reader.IsDBNull(reader.GetOrdinal("CommentaireExpedition")) ? null : reader.GetString(reader.GetOrdinal("CommentaireExpedition")),
                NumBordereauReception = reader.IsDBNull(reader.GetOrdinal("NumBordereauReception")) ? null : reader.GetString(reader.GetOrdinal("NumBordereauReception")),
                MagasinReceptionID = reader.IsDBNull(reader.GetOrdinal("MagasinReceptionID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("MagasinReceptionID")),
                MagasinReceptionNom = reader.IsDBNull(reader.GetOrdinal("MagasinReceptionNom")) ? null : reader.GetString(reader.GetOrdinal("MagasinReceptionNom")),
                NombreSacsReception = reader.IsDBNull(reader.GetOrdinal("NombreSacsReception")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("NombreSacsReception")),
                NombrePaletteReception = reader.IsDBNull(reader.GetOrdinal("NombrePaletteReception")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("NombrePaletteReception")),
                PoidsNetReception = reader.IsDBNull(reader.GetOrdinal("PoidsNetReception")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PoidsNetReception")),
                PoidsBrutReception = reader.IsDBNull(reader.GetOrdinal("PoidsBrutReception")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PoidsBrutReception")),
                TareSacsReception = reader.IsDBNull(reader.GetOrdinal("TareSacsReception")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("TareSacsReception")),
                TarePaletteReception = reader.IsDBNull(reader.GetOrdinal("TarePaletteReception")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("TarePaletteReception")),
                ImmTracteurReception = reader.IsDBNull(reader.GetOrdinal("ImmTracteurReception")) ? null : reader.GetString(reader.GetOrdinal("ImmTracteurReception")),
                ImmRemorqueReception = reader.IsDBNull(reader.GetOrdinal("ImmRemorqueReception")) ? null : reader.GetString(reader.GetOrdinal("ImmRemorqueReception")),
                CommentaireReception = reader.IsDBNull(reader.GetOrdinal("CommentaireReception")) ? null : reader.GetString(reader.GetOrdinal("CommentaireReception")),
                DateReception = reader.IsDBNull(reader.GetOrdinal("DateReception")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DateReception")),
                Statut = reader.IsDBNull(reader.GetOrdinal("Statut")) ? null : reader.GetString(reader.GetOrdinal("Statut")),
                Desactive = reader.IsDBNull(reader.GetOrdinal("Desactive")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Desactive")),
                CreationUtilisateur = reader.IsDBNull(reader.GetOrdinal("CreationUtilisateur")) ? null : reader.GetString(reader.GetOrdinal("CreationUtilisateur")),
                CreationDate = reader.IsDBNull(reader.GetOrdinal("CreationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                ModificationUtilisateur = reader.IsDBNull(reader.GetOrdinal("ModificationUtilisateur")) ? null : reader.GetString(reader.GetOrdinal("ModificationUtilisateur")),
                ModificationDate = reader.IsDBNull(reader.GetOrdinal("ModificationDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("ModificationDate")),
                RowVersionKey = reader.IsDBNull(reader.GetOrdinal("RowVersionKey")) ? null : (byte[])reader["RowVersionKey"],
                MagReceptionTheoID = reader.IsDBNull(reader.GetOrdinal("MagReceptionTheoID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("MagReceptionTheoID")),
                MagReceptionTheoNom = reader.IsDBNull(reader.GetOrdinal("MagReceptionTheoNom")) ? null : reader.GetString(reader.GetOrdinal("MagReceptionTheoNom"))
            };
        }
    }
}
