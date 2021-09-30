param ($db,$id,$pwdd) 
Write-Output "---Script de suppression totale des données---"
$sqlConnection = New-Object System.Data.SqlClient.SqlConnection
$sqlConnection.ConnectionString = "Server=SRVJIRA\SQLJIRA; Database=$db; User Id=$id; Password=$pwdd;"
$sqlConnection.Open()
#### Vérifier que la connexion fonctionne avant d'aller plus loin
if ($sqlConnection.State -ne [Data.ConnectionState]::Open) 
{

"Impossible d'ouvrir la connexion."

Exit
}
write-output "Connexion réussie"

### Try Catch Gobal pour garantir la fermeture de la connexion SQL
try { 
    try {  
        $Command = New-Object System.Data.SQLClient.SQLCommand 
        $Command.Connection = $sqlConnection
        $Command.CommandText = "
        DELETE FROM Hardware;
        DELETE FROM Category;
        DELETE FROM HardwareInfo;
        DELETE FROM Component;
        DELETE FROM Application;
        DELETE FROM Tag;
        DELETE FROM ApplicationTemplate;
        DELETE FROM Node;
        ";
        $Command.ExecuteNonQuery();
        }
        catch {
        write-output " Erreur instruction SQL : " $Error[0]     
        }

#### Ci-dessous, fin Try Catch Gobal pour garantir la fermeture de la connexion SQL
}
catch
{
	write-output " Erreur traitement central : " $Error[0]
}

#### Fermer la connexion à l'instance SQL Server.
if ($sqlConnection.State -eq [Data.ConnectionState]::Open) 
{
	$sqlConnection.Close()
	write-output "Connexion fermée"
}