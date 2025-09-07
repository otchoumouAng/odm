using System.Data;
using odm_api.Models;
using odm_api.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace odm_api.Repositories
{
    public class MouvementStockRepository
    {
        private readonly DatabaseService _dbService;

        public MouvementStockRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private MouvementStock MapReaderToMouvementStock(SqlDataReader reader)
        {
            // This mapper is based on the V2_MouvementStock_Get procedure I wrote
            return new MouvementStock
            {
                Id = (Guid)reader["ID"],
                MagasinId = (int)reader["MagasinId"],
                CampagneId = reader["CampagneId"].ToString() ?? string.Empty,
                ExportateurId = reader["ExportateurId"] != DBNull.Value ? (int?)reader["ExportateurId"] : null,
                MouvementTypeId = (int)reader["MouvementTypeId"],
                CertificationId = reader["CertificationId"] != DBNull.Value ? (int?)reader["CertificationId"] : null,
                EmballageTypeId = reader["SacTypeID"] != DBNull.Value ? (int?)reader["SacTypeID"] : null,
                ObjetEnStockId = reader["ObjetEnStockId"] != DBNull.Value ? (Guid?)reader["ObjetEnStockId"] : null,
                ObjetEnStockType = reader["ObjetEnStockType"] != DBNull.Value ? (int?)reader["ObjetEnStockType"] : null,
                EmplacementId = reader["EmplacementId"] != DBNull.Value ? (int?)reader["EmplacementId"] : null,
                SiteId = (int)reader["SiteId"],
                Reference1 = reader["Reference1"]?.ToString(),
                Reference2 = reader["Reference2"]?.ToString(),
                Date = (DateTime)reader["Date"],
                Sens = (short)reader["Sens"],
                Quantite = (int)reader["Quantite"],
                PoidsBrut = (decimal)reader["PoidsBrut"],
                TareSacs = (decimal)reader["TareSacs"],
                TarePalettes = (decimal)reader["TarePalettes"],
                PoidsNetLivre = (decimal)reader["PoidsNetLivre"],
                Retention = (decimal)reader["Retention"],
                PoidsNetAccepte = (decimal)reader["PoidsNetAccepte"],
                Statut = reader["Statut"].ToString() ?? string.Empty,
                Commentaire = reader["Commentaire"]?.ToString(),
                Desactive = reader["Desactive"] != DBNull.Value ? (bool?)reader["Desactive"] : null,
                Approbateur = reader["Approbateur"]?.ToString(),
                DateApprobation = reader["DateApprobation"] != DBNull.Value ? (DateTime?)reader["DateApprobation"] : null,
                CreationUtilisateur = reader["CreationUtilisateur"].ToString() ?? string.Empty,
                CreationDate = (DateTime)reader["CreationDate"],
                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                RowVersionKey = reader["RowVersionKey"] as byte[],
                ProduitId = reader["ProduitId"] != DBNull.Value ? (int?)reader["ProduitId"] : null,
                MagasinNom = reader["MagasinNom"]?.ToString(),
                MouvementTypeDesignation = reader["MouvementTypeDesignation"]?.ToString(),
                ExportateurNom = reader["ExportateurNom"]?.ToString(),
                CertificationDesignation = reader["CertificationDesignation"]?.ToString(),
                SacTypeDesignation = reader["SacTypeDesignation"]?.ToString(),
                EmplacementDesignation = reader["EmplacementDesignation"]?.ToString(),
                SiteNom = reader["SiteNom"]?.ToString()
            };
        }

        public List<MouvementStock> GetAllMouvementsStock()
        {
            var list = new List<MouvementStock>();
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new MouvementStock {
                                Id = (Guid)reader["ID"],
                                MagasinId = (int)reader["MagasinID"],
                                MagasinNom = reader["MagasinNom"]?.ToString(),
                                CampagneId = reader["CampagneID"].ToString(),
                                MouvementTypeId = (int)reader["MouvementTypeID"],
                                MouvementTypeDesignation = reader["MouvementTypeDesignation"]?.ToString(),
                                ExportateurId = reader["ExportateurID"] != DBNull.Value ? (int?)reader["ExportateurID"] : null,
                                ExportateurNom = reader["ExportateurNom"]?.ToString(),
                                CertificationId = reader["CertificationID"] != DBNull.Value ? (int?)reader["CertificationID"] : null,
                                CertificationDesignation = reader["CertificationDesignation"]?.ToString(),
                                EmballageTypeId = reader["SacTypeID"] != DBNull.Value ? (int?)reader["SacTypeID"] : null,
                                SacTypeDesignation = reader["SacTypeDesignation"]?.ToString(),
                                ObjetEnStockId = reader["ObjetEnStockID"] != DBNull.Value ? (Guid?)reader["ObjetEnStockID"] : null,
                                ObjetEnStockType = reader["ObjetEnStockType"] != DBNull.Value ? (int?)reader["ObjetEnStockType"] : null,
                                EmplacementId = reader["EmplacementID"] != DBNull.Value ? (int?)reader["EmplacementID"] : null,
                                EmplacementDesignation = reader["EmplacementDesignation"]?.ToString(),
                                Reference1 = reader["Reference1"]?.ToString(),
                                Reference2 = reader["Reference2"]?.ToString(),
                                Reference3 = reader["Reference3"]?.ToString(),
                                Date = (DateTime)reader["DateMouvement"],
                                Sens = (short)reader["Sens"],
                                Quantite = (int)reader["Quantite"],
                                PoidsBrut = (decimal)reader["PoidsBrut"],
                                TareSacs = (decimal)reader["TareSacs"],
                                TarePalettes = (decimal)reader["TarePalettes"],
                                PoidsNetLivre = (decimal)reader["PoidsNetLivre"],
                                Retention = (decimal)reader["RetentionPoids"],
                                PoidsNetAccepte = (decimal)reader["PoidsNetAccepte"],
                                Statut = reader["Statut"].ToString(),
                                Commentaire = reader["Commentaire"]?.ToString(),
                                Desactive = reader["Desactive"] != DBNull.Value ? (bool?)reader["Desactive"] : null,
                                Approbateur = reader["ApprobationUtilisateur"]?.ToString(),
                                DateApprobation = reader["ApprobationDate"] != DBNull.Value ? (DateTime?)reader["ApprobationDate"] : null,
                                CreationUtilisateur = reader["CreationUtilisateur"].ToString(),
                                CreationDate = (DateTime)reader["CreationDate"],
                                ModificationUtilisateur = reader["ModificationUtilisateur"]?.ToString(),
                                ModificationDate = reader["ModificationDate"] != DBNull.Value ? (DateTime?)reader["ModificationDate"] : null,
                                RowVersionKey = reader["RowVersionKey"] as byte[],
                                SiteId = (int)reader["SiteID"],
                                SiteNom = reader["SiteNom"]?.ToString()
                            };
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public MouvementStock? GetMouvementStockById(Guid id)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_Get", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToMouvementStock(reader);
                        }
                    }
                }
            }
            return null;
        }

        public MouvementStock AddMouvementStock(MouvementStock mouvementStock)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_New", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@magasinId", mouvementStock.MagasinId);
                    cmd.Parameters.AddWithValue("@campagneID", mouvementStock.CampagneId);
                    cmd.Parameters.AddWithValue("@exportateurId", (object)mouvementStock.ExportateurId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@certificationId", (object)mouvementStock.CertificationId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datemouvement", mouvementStock.Date);
                    cmd.Parameters.AddWithValue("@sens", mouvementStock.Sens);
                    cmd.Parameters.AddWithValue("@mouvementTypeId", mouvementStock.MouvementTypeId);
                    cmd.Parameters.AddWithValue("@objectEnStockID", (object)mouvementStock.ObjetEnStockId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@objectEnStockType", (object)mouvementStock.ObjetEnStockType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantite", mouvementStock.Quantite);
                    cmd.Parameters.AddWithValue("@statut", mouvementStock.Statut);
                    cmd.Parameters.AddWithValue("@reference1", (object)mouvementStock.Reference1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference2", (object)mouvementStock.Reference2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@poidsbrut", mouvementStock.PoidsBrut);
                    cmd.Parameters.AddWithValue("@tarebags", mouvementStock.TareSacs);
                    cmd.Parameters.AddWithValue("@tarepalette", mouvementStock.TarePalettes);
                    cmd.Parameters.AddWithValue("@poidsnetlivre", mouvementStock.PoidsNetLivre);
                    cmd.Parameters.AddWithValue("@retention", mouvementStock.Retention);
                    cmd.Parameters.AddWithValue("@poidsnetaccepte", mouvementStock.PoidsNetAccepte);
                    cmd.Parameters.AddWithValue("@CreationUser", mouvementStock.CreationUtilisateur);
                    cmd.Parameters.AddWithValue("@EmplacementID", (object)mouvementStock.EmplacementId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sactypeId", (object)mouvementStock.EmballageTypeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@commentaire", (object)mouvementStock.Commentaire ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SiteID", mouvementStock.SiteId);
                    cmd.Parameters.AddWithValue("@produitID", (object)mouvementStock.ProduitId ?? DBNull.Value);

                    var idParam = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                    idParam.Direction = ParameterDirection.Output;
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

                    mouvementStock.Id = (Guid)idParam.Value;
                    mouvementStock.RowVersionKey = (byte[])rvParam.Value;
                    return mouvementStock;
                }
            }
        }

        public void UpdateMouvementStock(MouvementStock mouvementStock)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_Modify", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", mouvementStock.Id);
                    cmd.Parameters.AddWithValue("@magasinId", mouvementStock.MagasinId);
                    cmd.Parameters.AddWithValue("@campagneID", mouvementStock.CampagneId);
                    cmd.Parameters.AddWithValue("@exportateurId", (object)mouvementStock.ExportateurId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmplacementID", (object)mouvementStock.EmplacementId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datemouvement", mouvementStock.Date);
                    cmd.Parameters.AddWithValue("@sens", mouvementStock.Sens);
                    cmd.Parameters.AddWithValue("@mouvementTypeId", mouvementStock.MouvementTypeId);
                    cmd.Parameters.AddWithValue("@NumeroBordereau", (object)mouvementStock.Reference1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference", (object)mouvementStock.Reference2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sactypeId", (object)mouvementStock.EmballageTypeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantite", mouvementStock.Quantite);
                    cmd.Parameters.AddWithValue("@poidsbrut", mouvementStock.PoidsBrut);
                    cmd.Parameters.AddWithValue("@tarebags", mouvementStock.TareSacs);
                    cmd.Parameters.AddWithValue("@tarepalette", mouvementStock.TarePalettes);
                    cmd.Parameters.AddWithValue("@poidsnetlivre", mouvementStock.PoidsNetLivre);
                    cmd.Parameters.AddWithValue("@retention", mouvementStock.Retention);
                    cmd.Parameters.AddWithValue("@poidsnetaccepte", mouvementStock.PoidsNetAccepte);
                    cmd.Parameters.AddWithValue("@commentaire", (object)mouvementStock.Commentaire ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModificationUser", mouvementStock.ModificationUtilisateur);

                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp);
                    rvParam.Direction = ParameterDirection.InputOutput;
                    rvParam.Value = mouvementStock.RowVersionKey;
                    var errParam = cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                    errParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var error = errParam.Value as string;
                    if (error != null && !string.IsNullOrEmpty(error) && error != DBNull.Value.ToString())
                    {
                        throw new Exception(error);
                    }

                    mouvementStock.RowVersionKey = (byte[])rvParam.Value;
                }
            }
        }
    }
}
