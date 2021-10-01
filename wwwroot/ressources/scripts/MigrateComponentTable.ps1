param($hostname, $username, $mdp, $db, $id, $pwdd)
Write-Output "--script de migration de la table component---"
try {
    $swis = Connect-Swis -Hostname $hostname -Username $username -Password $mdp
}
catch {
    write-output "$($PSItem.Exception.Message)`n"
}

$sqlConnection = New-Object System.Data.SqlClient.SqlConnection
$sqlConnection.ConnectionString = "Server=SRVJIRA\SQLJIRA; Database=$db; User Id=$id; Password=$pwdd;"
$sqlConnection.Open()

#### Vérifier que la connexion fonctionne avant d'aller plus loin
if ($sqlConnection.State -ne [Data.ConnectionState]::Open) {
    "Impossible d'ouvrir la connexion."
    Exit
}

write-output "Connexion réussie"
### Try Catch Gobal pour garantir la fermeture de la connexion SQL
try { 
    try {  
        $Command = New-Object System.Data.SQLClient.SQLCommand 
        $Command.Connection = $sqlConnection
        Get-SwisData $swis 'SELECT ComponentID as ID, ApplicationID, Name, Status, DetailsUrl FROM Orion.APM.Component' | ForEach-Object {
            ### Les données de type string sont converties en varchar en les mettants entre guillemets
            $Command.CommandText += "INSERT INTO Component VALUES($($_.ID),$($_.ApplicationID),'$($_.Name)','$($_.Status)','$($_.DetailsUrl)');" 
            $Command.Parameters.Clear();
        }
        $Command.ExecuteNonQuery(); 
    }
    catch { 
        write-output " Erreur instruction SQL : " $Error[0]     
    }

    #### Ci-dessous, fin Try Catch Gobal pour garantir la fermeture de la connexion SQL
}
catch {
    write-output " Erreur traitement central : " $Error[0]
}

#### Fermer la connexion à l'instance SQL Server.
if ($sqlConnection.State -eq [Data.ConnectionState]::Open) {
    $sqlConnection.Close()
    write-output "Connexion fermée"
}
