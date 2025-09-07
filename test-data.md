Voici des exemples de corps de requête JSON que vous pouvez utiliser pour tester la création (`POST`) d'un mouvement de stock et d'un transfert de lot.

**Note importante :** Les valeurs des `ID` (comme `magasinID`, `exportateurID`, `lotID`, etc.) sont des exemples. Vous devez les remplacer par des identifiants qui existent réellement dans vos tables correspondantes (Magasin, Exportateur, Lot, etc.) pour éviter d'autres erreurs de clé étrangère.

---

### Pour `POST /api/mouvementstock`

```json
{
  "magasinID": 1,
  "campagneID": "2024/2025",
  "exportateurID": 1,
  "mouvementTypeID": 1,
  "objetEnStockID": "d13cf224-2b63-4416-86f1-28956e1b74d6",
  "objetEnStockType": 1,
  "emplacementID": 1,
  "siteID": 1,
  "reference1": "MV-TEST-001",
  "reference2": "BATCH-42",
  "dateMouvement": "2025-09-07T10:00:00Z",
  "sens": 1,
  "quantite": 100,
  "poidsBrut": 1050.5,
  "tareSacs": 50.0,
  "tarePalettes": 25.0,
  "poidsNetLivre": 975.5,
  "retentionPoids": 0.0,
  "poidsNetAccepte": 975.5,
  "statut": "En attente",
  "commentaire": "Test de création de mouvement de stock.",
  "creationUtilisateur": "votre_nom_utilisateur"
}
```

---

### Pour `POST /api/transfertlot`

```json
{
  "campagneID": "2024/2025",
  "siteID": 1,
  "lotID": "d13cf224-2b63-4416-86f1-28956e1b74d6",
  "numeroLot": "LOT-2025-001",
  "numBordereauExpedition": "BE-2025-001",
  "magasinExpeditionID": 1,
  "nombreSacsExpedition": 100,
  "nombrePaletteExpedition": 4,
  "tareSacsExpedition": 50.0,
  "tarePaletteExpedition": 100.0,
  "poidsBrutExpedition": 1150.0,
  "poidsNetExpedition": 1000.0,
  "immTracteurExpedition": "TR-123-AB",
  "immRemorqueExpedition": "RM-456-CD",
  "dateExpedition": "2025-09-07T12:00:00Z",
  "commentaireExpedition": "Test de transfert de lot.",
  "statut": "Expédié",
  "magReceptionTheoID": 2,
  "creationUtilisateur": "votre_nom_utilisateur"
}
```
