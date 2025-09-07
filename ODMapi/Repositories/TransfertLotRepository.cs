using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace odm_api.Repositories
{
    public class TransfertLotRepository
    {
        private readonly DatabaseService _dbService;

        public TransfertLotRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private TransfertLot MapReaderToTransfertLot(SqlDataReader reader)
        {
            return new TransfertLot
            {
                Id = (Guid)reader["ID"],
                CampagneId = reader["CampagneID"].ToString(),
                SiteId = (int)reader["SiteID"],
                LotId = (Guid)reader["LotID"],
                NumeroExpedition = reader["NumeroExpedition"]?.ToString(),
                NumBordereauExpedition = reader["NumBordereauExpedition"]?.ToString(),
                MagasinExpeditionId = reader["MagasinExpeditionID"] != DBNull.Value ? (int?)reader["MagasinExpeditionID"] : null,
                NombreSacsExpedition = reader["NombreSacsExpedition"] != DBNull.Value ? (int?)reader["NombreSacsExpedition"] : null,
                NombrePaletteExpedition = reader["NombrePaletteExpedition"] != DBNull.Value ? (int?)reader["NombrePaletteExpedition"] : null,
                TareSacsExpedition = reader["TareSacsExpedition"] != DBNull.Value ? (decimal?)reader["TareSacsExpedition"] : null,
                TarePaletteExpedition = reader["TarePaletteExpedition"] != DBNull.Value ? (decimal?)reader["TarePaletteExpedition"] : null,
                PoidsBrutExpedition = reader["PoidsBrutExpedition"] != DBNull.Value ? (decimal?)reader["PoidsBrutExpedition"] : null,
                PoidsNetExpedition = reader["PoidsNetExpedition"] != DBNull.Value ? (decimal?)reader["PoidsNetExpedition"] : null,
                ImmTracteurExpedition = reader["ImmTracteurExpedition"]?.ToString(),
                ImmRemorqueExpedition = reader["ImmRemorqueExpedition"]?.ToString(),
                DateExpedition = reader["DateExpedition"] != DBNull.Value ? (DateTime?)reader["DateExpedition"] : null,
                CommentaireExpedition = reader["CommentaireExpedition"]?.ToString(),
                NumBordereauReception = reader["NumBordereauReception"]?.ToString(),
                MagasinReceptionId = reader["MagasinReceptionID"] != DBNull.Value ? (int?)reader["MagasinReceptionID"] : null,
                NombreSacsReception = reader["NombreSacsReception"] != DBNull.Value ? (int?)reader["NombreSacsReception"] : null,
                NombrePaletteReception = reader["NombrePaletteReception"] != DBNull.Value ? (int?)reader["NombrePaletteReception"] : null,
                PoidsNetReception = reader["PoidsNetReception"] != DBNull.Value ? (decimal?)reader["PoidsNetReception"] : null,
                PoidsBrutReception = reader["PoidsBrutReception"] != DBNull.Value ? (decimal?)reader["PoidsBrutReception"] : null,
                TareSacsReception = reader["TareSacsReception"] != DBNull.Value ? (decimal?)reader["TareSacsReception"] : null,
                TarePaletteReception = reader["TarePaletteReception"] != DBNull.Value ? (decimal?)reader["TarePaletteReception"] : null,
                ImmTracteurReception = reader["ImmTracteurReception"]?.ToString(),
                ImmRemorqueReception = reader["ImmRemorqueReception"]?.ToString(),
                CommentaireReception = reader["CommentaireReception"]?.ToString(),
                DateReception = reader["DateReception"] != DBNull.Value ? (DateTime?)reader["DateReception"] : null,
                Statut = reader["Statut"]?.ToString(),
                Desactive = reader["Desactive"] != DBNull.Value ? (bool?)reader["Desactive"] : null,
                CreationUtilisateur = reader["CreationUtilisateur"].ToString(),
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                RowVersionKey = reader["RowVersionKey"] as byte[],
                MagReceptionTheo = reader["MagReceptionTheoID"] != DBNull.Value ? (int?)reader["MagReceptionTheoID"] : null,
                SiteNom = reader["SiteNom"]?.ToString(),
                MagasinExpeditionNom = reader["MagasinExpeditionNom"]?.ToString(),
                MagasinReceptionNom = reader["MagasinReceptionNom"]?.ToString(),
                NumeroLot = reader["NumeroLot"]?.ToString(),
                ExportateurId = reader["ExportateurID"] != DBNull.Value ? (int?)reader["ExportateurID"] : null,
                ExportateurNom = reader["ExportateurNom"]?.ToString(),
                MagReceptionTheoNom = reader["MagReceptionTheoNom"]?.ToString()
            };
        }

        public List<TransfertLot> GetAllTransfertLots()
        {
            var list = new List<TransfertLot>();
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
                            list.Add(MapReaderToTransfertLot(reader));
                        }
                    }
                }
            }
            return list;
        }

        public TransfertLot? GetTransfertLotById(Guid id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToTransfertLot(reader);
                        }
                    }
                }
            }
            return null;
        }

        public TransfertLot AddTransfertLot(TransfertLot transfertLot)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_New", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@campagneID", transfertLot.CampagneId);
                    cmd.Parameters.AddWithValue("@siteID", transfertLot.SiteId);
                    cmd.Parameters.AddWithValue("@LotID", transfertLot.LotId);
                    cmd.Parameters.AddWithValue("@NumBordereauExpedition", (object)transfertLot.NumBordereauExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinExpeditionID", (object)transfertLot.MagasinExpeditionId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nombreSacs", (object)transfertLot.NombreSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombrePalette", (object)transfertLot.NombrePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TareSac", (object)transfertLot.TareSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TarePalette", (object)transfertLot.TarePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsBrut", (object)transfertLot.PoidsBrutExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsNet", (object)transfertLot.PoidsNetExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmTracteur", (object)transfertLot.ImmTracteurExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmRemorque", (object)transfertLot.ImmRemorqueExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateExpedition", (object)transfertLot.DateExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Commentaire", (object)transfertLot.CommentaireExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@statut", (object)transfertLot.Statut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinTheoReceptionID", (object)transfertLot.MagReceptionTheo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreationUser", transfertLot.CreationUtilisateur);

                    var idParam = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                    idParam.Direction = ParameterDirection.Output;
                    var numExpParam = cmd.Parameters.Add("@numeroExpedition", SqlDbType.VarChar, 10);
                    numExpParam.Direction = ParameterDirection.Output;
                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp);
                    rvParam.Direction = ParameterDirection.Output;
                    var errParam = cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                    errParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var error = errParam.Value as string;
                    if (error != null && !string.IsNullOrEmpty(error) && error != DBNull.Value.ToString())
                    {
                        throw new Exception(error);
                    }

                    transfertLot.Id = (Guid)idParam.Value;
                    transfertLot.NumeroExpedition = numExpParam.Value as string;
                    transfertLot.RowVersionKey = (byte[])rvParam.Value;
                    return transfertLot;
                }
            }
        }

        public void UpdateTransfertLot(TransfertLot transfertLot)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V5_Transfert_Lot_Modify", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", transfertLot.Id);
                    cmd.Parameters.AddWithValue("@campagneID", transfertLot.CampagneId);
                    cmd.Parameters.AddWithValue("@siteID", transfertLot.SiteId);
                    cmd.Parameters.AddWithValue("@LotID", transfertLot.LotId);
                    cmd.Parameters.AddWithValue("@numerolot", (object)transfertLot.NumeroLot ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NumBordereauExpedition", (object)transfertLot.NumBordereauExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinExpeditionID", (object)transfertLot.MagasinExpeditionId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nombreSacs", (object)transfertLot.NombreSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombrePalette", (object)transfertLot.NombrePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TareSac", (object)transfertLot.TareSacsExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TarePalette", (object)transfertLot.TarePaletteExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsBrut", (object)transfertLot.PoidsBrutExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsNet", (object)transfertLot.PoidsNetExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmTracteur", (object)transfertLot.ImmTracteurExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImmRemorque", (object)transfertLot.ImmRemorqueExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateExpedition", (object)transfertLot.DateExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Commentaire", (object)transfertLot.CommentaireExpedition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@statut", (object)transfertLot.Statut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@magasinTheoReceptionID", (object)transfertLot.MagReceptionTheo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreationUser", transfertLot.ModificationUtilisateur);

                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp);
                    rvParam.Direction = ParameterDirection.InputOutput;
                    rvParam.Value = transfertLot.RowVersionKey;
                    var errParam = cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                    errParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var error = errParam.Value as string;
                    if (error != null && !string.IsNullOrEmpty(error) && error != DBNull.Value.ToString())
                    {
                        throw new Exception(error);
                    }

                    transfertLot.RowVersionKey = (byte[])rvParam.Value;
                }
            }
        }
    }
}
