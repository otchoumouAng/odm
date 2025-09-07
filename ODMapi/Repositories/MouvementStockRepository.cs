using Microsoft.Data.SqlClient;
using odm_api.Models;
using odm_api.Services;
using System.Data;
using System.Data.SqlTypes;

namespace odm_api.Repositories
{
    public class MouvementStockRepository
    {
        private readonly DatabaseService _dbService;

        public MouvementStockRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<MouvementStock> GetAllMouvements()
        {
            var mouvements = new List<MouvementStock>();
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_Select", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@magasinID", -1);
                    cmd.Parameters.AddWithValue("@campagneID", "{Toute}");
                    cmd.Parameters.AddWithValue("@exportateurID", -1);
                    cmd.Parameters.AddWithValue("@mouvementTypeID", -2);
                    cmd.Parameters.AddWithValue("@certificationID", -1);
                    cmd.Parameters.AddWithValue("@sens", -2);
                    cmd.Parameters.AddWithValue("@datedebut", DBNull.Value);
                    cmd.Parameters.AddWithValue("@datefin", DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", "-1");
                    cmd.Parameters.AddWithValue("@EmplacementID", 1);
                    cmd.Parameters.AddWithValue("@siteID", -1);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mouvements.Add(MapMouvementStockFromReader(reader));
                        }
                    }
                }
            }
            return mouvements;
        }

        public MouvementStock GetMouvementById(Guid id)
        {
            // Note: A dedicated SP for GetById is recommended for performance.
            // This implementation filters the result of GetAllMouvements().
            return GetAllMouvements().FirstOrDefault(m => m.ID == id);
        }

        public void AddMouvement(MouvementStock mouvement)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_New", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@magasinId", mouvement.MagasinID);
                    cmd.Parameters.AddWithValue("@campagneID", mouvement.CampagneID);
                    cmd.Parameters.AddWithValue("@exportateurId", (object)mouvement.ExportateurID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mouvementTypeId", mouvement.MouvementTypeID);
                    cmd.Parameters.AddWithValue("@objectEnStockID", (object)mouvement.ObjetEnStockID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@objectEnStockType", (object)mouvement.ObjetEnStockType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantite", mouvement.Quantite);
                    cmd.Parameters.AddWithValue("@statut", (object)mouvement.Statut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference1", (object)mouvement.Reference1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference2", (object)mouvement.Reference2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datemouvement", mouvement.DateMouvement);
                    cmd.Parameters.AddWithValue("@sens", mouvement.Sens);
                    cmd.Parameters.AddWithValue("@poidsbrut", mouvement.PoidsBrut);
                    cmd.Parameters.AddWithValue("@tarebags", mouvement.TareSacs);
                    cmd.Parameters.AddWithValue("@tarepalette", mouvement.TarePalettes);
                    cmd.Parameters.AddWithValue("@poidsnetlivre", mouvement.PoidsNetLivre);
                    cmd.Parameters.AddWithValue("@retention", mouvement.RetentionPoids);
                    cmd.Parameters.AddWithValue("@poidsnetaccepte", mouvement.PoidsNetAccepte);
                    cmd.Parameters.AddWithValue("@CreationUser", (object)mouvement.CreationUtilisateur ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmplacementID", (object)mouvement.EmplacementID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sactypeId", (object)mouvement.SacTypeID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@commentaire", (object)mouvement.Commentaire ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SiteID", mouvement.SiteID);
                    cmd.Parameters.AddWithValue("@produitID", (object)mouvement.ProduitID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@certificationId", (object)mouvement.CertificationID ?? DBNull.Value);

                    // Output parameters
                    var idParam = cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });
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

                    // Set mouvement properties
                    mouvement.ID = idParam.Value is DBNull ? Guid.Empty : (Guid)idParam.Value;
                    mouvement.RowVersionKey = rvParam.Value is DBNull ? null : (byte[])rvParam.Value;
                }
            }
        }

        public void UpdateMouvement(MouvementStock mouvement)
        {
            using (var conn = _dbService.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("V2_MouvementStock_Modify", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@ID", mouvement.ID);
                    cmd.Parameters.AddWithValue("@magasinId", mouvement.MagasinID);
                    cmd.Parameters.AddWithValue("@campagneID", mouvement.CampagneID);
                    cmd.Parameters.AddWithValue("@exportateurId", (object)mouvement.ExportateurID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datemouvement", mouvement.DateMouvement);
                    cmd.Parameters.AddWithValue("@sens", mouvement.Sens);
                    cmd.Parameters.AddWithValue("@mouvementTypeId", mouvement.MouvementTypeID);
                    cmd.Parameters.AddWithValue("@NumeroBordereau", (object)mouvement.Reference1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference", (object)mouvement.Reference2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantite", mouvement.Quantite);
                    cmd.Parameters.AddWithValue("@poidsbrut", mouvement.PoidsBrut);
                    cmd.Parameters.AddWithValue("@tarebags", mouvement.TareSacs);
                    cmd.Parameters.AddWithValue("@tarepalette", mouvement.TarePalettes);
                    cmd.Parameters.AddWithValue("@poidsnetlivre", mouvement.PoidsNetLivre);
                    cmd.Parameters.AddWithValue("@retention", mouvement.RetentionPoids);
                    cmd.Parameters.AddWithValue("@poidsnetaccepte", mouvement.PoidsNetAccepte);
                    cmd.Parameters.AddWithValue("@commentaire", (object)mouvement.Commentaire ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModificationUser", (object)mouvement.ModificationUtilisateur ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmplacementID", (object)mouvement.EmplacementID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sactypeId", (object)mouvement.SacTypeID ?? DBNull.Value);
                    
                    // RowVersion param: InputOutput
                    var rvParam = cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp);
                    rvParam.Direction = ParameterDirection.InputOutput;
                    rvParam.Value = (object)mouvement.RowVersionKey ?? DBNull.Value;

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
                        throw new DBConcurrencyException(errorMessage ?? "Concurrency conflict: Mouvement was modified by another user.");
                    }
                    else if (returnValue != 0)
                    {
                        throw new Exception(errorMessage ?? $"Update failed with return code {returnValue}");
                    }

                    // Get new RowVersion
                    if (rvParam.Value != DBNull.Value)
                    {
                        mouvement.RowVersionKey = (byte[])rvParam.Value;
                    }
                }
            }
        }

        private MouvementStock MapMouvementStockFromReader(SqlDataReader reader)
        {
            return new MouvementStock
            {
                ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("ID")),
                MagasinID = reader.IsDBNull(reader.GetOrdinal("MagasinID")) ? 0 : reader.GetInt32(reader.GetOrdinal("MagasinID")),
                MagasinNom = reader.IsDBNull(reader.GetOrdinal("MagasinNom")) ? null : reader.GetString(reader.GetOrdinal("MagasinNom")),
                CampagneID = reader.IsDBNull(reader.GetOrdinal("CampagneID")) ? null : reader.GetString(reader.GetOrdinal("CampagneID")),
                ExportateurID = reader.IsDBNull(reader.GetOrdinal("ExportateurID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ExportateurID")),
                ExportateurNom = reader.IsDBNull(reader.GetOrdinal("ExportateurNom")) ? null : reader.GetString(reader.GetOrdinal("ExportateurNom")),
                MouvementTypeID = reader.IsDBNull(reader.GetOrdinal("MouvementTypeID")) ? 0 : reader.GetInt32(reader.GetOrdinal("MouvementTypeID")),
                MouvementTypeDesignation = reader.IsDBNull(reader.GetOrdinal("MouvementTypeDesignation")) ? null : reader.GetString(reader.GetOrdinal("MouvementTypeDesignation")),
                CertificationID = reader.IsDBNull(reader.GetOrdinal("CertificationID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CertificationID")),
                CertificationDesignation = reader.IsDBNull(reader.GetOrdinal("CertificationDesignation")) ? null : reader.GetString(reader.GetOrdinal("CertificationDesignation")),
                SacTypeID = reader.IsDBNull(reader.GetOrdinal("SacTypeID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("SacTypeID")),
                SacTypeDesignation = reader.IsDBNull(reader.GetOrdinal("SacTypeDesignation")) ? null : reader.GetString(reader.GetOrdinal("SacTypeDesignation")),
                ObjetEnStockID = reader.IsDBNull(reader.GetOrdinal("ObjetEnStockID")) ? null : (Guid?)reader.GetGuid(reader.GetOrdinal("ObjetEnStockID")),
                ObjetEnStockType = reader.IsDBNull(reader.GetOrdinal("ObjetEnStockType")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ObjetEnStockType")),
                EmplacementID = reader.IsDBNull(reader.GetOrdinal("EmplacementID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("EmplacementID")),
                EmplacementDesignation = reader.IsDBNull(reader.GetOrdinal("EmplacementDesignation")) ? null : reader.GetString(reader.GetOrdinal("EmplacementDesignation")),
                SiteID = reader.IsDBNull(reader.GetOrdinal("SiteID")) ? 0 : reader.GetInt32(reader.GetOrdinal("SiteID")),
                SiteNom = reader.IsDBNull(reader.GetOrdinal("SiteNom")) ? null : reader.GetString(reader.GetOrdinal("SiteNom")),
                Reference1 = reader.IsDBNull(reader.GetOrdinal("Reference1")) ? null : reader.GetString(reader.GetOrdinal("Reference1")),
                Reference2 = reader.IsDBNull(reader.GetOrdinal("Reference2")) ? null : reader.GetString(reader.GetOrdinal("Reference2")),
                Reference3 = reader.IsDBNull(reader.GetOrdinal("Reference3")) ? null : reader.GetString(reader.GetOrdinal("Reference3")),
                DateMouvement = reader.IsDBNull(reader.GetOrdinal("DateMouvement")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateMouvement")),
                Sens = reader.IsDBNull(reader.GetOrdinal("Sens")) ? (short)0 : reader.GetInt16(reader.GetOrdinal("Sens")),
                Quantite = reader.IsDBNull(reader.GetOrdinal("Quantite")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantite")),
                PoidsBrut = reader.IsDBNull(reader.GetOrdinal("PoidsBrut")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PoidsBrut")),
                TareSacs = reader.IsDBNull(reader.GetOrdinal("TareSacs")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TareSacs")),
                TarePalettes = reader.IsDBNull(reader.GetOrdinal("TarePalettes")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TarePalettes")),
                PoidsNetLivre = reader.IsDBNull(reader.GetOrdinal("PoidsNetLivre")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PoidsNetLivre")),
                RetentionPoids = reader.IsDBNull(reader.GetOrdinal("RetentionPoids")) ? 0 : reader.GetDecimal(reader.GetOrdinal("RetentionPoids")),
                PoidsNetAccepte = reader.IsDBNull(reader.GetOrdinal("PoidsNetAccepte")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PoidsNetAccepte")),
                Statut = reader.IsDBNull(reader.GetOrdinal("Statut")) ? null : reader.GetString(reader.GetOrdinal("Statut")),
                Commentaire = reader.IsDBNull(reader.GetOrdinal("Commentaire")) ? null : reader.GetString(reader.GetOrdinal("Commentaire")),
                Desactive = reader.IsDBNull(reader.GetOrdinal("Desactive")) ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("Desactive")),
                ApprobationUtilisateur = reader.IsDBNull(reader.GetOrdinal("ApprobationUtilisateur")) ? null : reader.GetString(reader.GetOrdinal("ApprobationUtilisateur")),
                ApprobationDate = reader.IsDBNull(reader.GetOrdinal("ApprobationDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("ApprobationDate")),
                CreationUtilisateur = reader.IsDBNull(reader.GetOrdinal("CreationUtilisateur")) ? null : reader.GetString(reader.GetOrdinal("CreationUtilisateur")),
                CreationDate = reader.IsDBNull(reader.GetOrdinal("CreationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                ModificationUtilisateur = reader.IsDBNull(reader.GetOrdinal("ModificationUtilisateur")) ? null : reader.GetString(reader.GetOrdinal("ModificationUtilisateur")),
                ModificationDate = reader.IsDBNull(reader.GetOrdinal("ModificationDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("ModificationDate")),
                RowVersionKey = reader.IsDBNull(reader.GetOrdinal("RowVersionKey")) ? null : (byte[])reader["RowVersionKey"]
            };
        }
    }
}
