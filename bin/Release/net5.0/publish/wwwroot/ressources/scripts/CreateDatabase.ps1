param($db,$id,$pwdd)
Write-Output "---Script de creation de base de données---"
$sqlConnection = New-Object System.Data.SqlClient.SqlConnection
$sqlConnection.ConnectionString = "Server=SRVJIRA\SQLJIRA; Database=$db; User Id=$id; Password=$pwdd;"
$sqlConnection.Open()

#### Vérifier que la connexion fonctionne avant d'aller plus loin
if ($sqlConnection.State -ne [Data.ConnectionState]::Open) 
{

write-output "Impossible d'ouvrir la connexion."

Exit
}

write-output "Connexion réussie"

### Try Catch Gobal pour garantir la fermeture de la connexion SQL
try { 
    
        try {  
            $Command = New-Object System.Data.SQLClient.SQLCommand 
            $Command.Connection = $sqlConnection
            $Command.CommandText = "
            CREATE TABLE  ApplicationTemplate (
                ID int PRIMARY KEY NOT NULL,
                ApplicationTemplateName varchar(255),
                );
            CREATE TABLE Tag (
		        TemplateID int NOT NULL,
		        TagName varchar(255),
		        CONSTRAINT FK_TagApplicationTemplate FOREIGN KEY(TemplateID)
		        REFERENCES ApplicationTemplate(ID)
		        );    
            CREATE TABLE Node (
                ID int PRIMARY KEY NOT NULL,
                NodeName varchar(255),
                CodeClient varchar(255),
                CodeCS varchar(255),
                Status int,
                DetailsUrl varchar(255)
                );
            CREATE TABLE Application(
                ID int PRIMARY KEY NOT NULL,
                ApplicationTemplateID int NOT NULL,
                NodeID int NOT NULL,
                Status int,
                DetailsUrl varchar(255),
                CONSTRAINT FK_ApplicationTemplate FOREIGN KEY(ApplicationTemplateID) REFERENCES ApplicationTemplate(ID),
                CONSTRAINT FK_ApplicatonNode FOREIGN KEY(NodeID) REFERENCES Node(ID)
                );
            CREATE TABLE Component (
                ID int NOT NULL,
                ApplicationID int NOT NULL,
                ComponentName varchar(255),
                SeverityStatus int,
                DetailsUrl varchar(255),
                PRIMARY KEY (ID),
                CONSTRAINT FK_ComponentApplicationTemplate FOREIGN KEY (ApplicationID)
                REFERENCES Application(ID)
                );
            CREATE TABLE HardwareInfo (
                ID int NOT NULL,
                NodeID int NOT NULL,
                Status int,
                DetailsUrl varchar(255),
                LastMessage varchar(255),
                PRIMARY KEY(ID),
                CONSTRAINT FK_NodeHardwareInfo FOREIGN KEY (NodeID)
                REFERENCES Node(ID)
                );
            CREATE TABLE Category(
                ID int PRIMARY KEY NOT NULL,
                CategoryName varchar(255)
                );
	        CREATE TABLE Hardware (
                CategoryID int NOT NULL,
                HardwareInfoID int NOT NULL,
		        PRIMARY KEY(CategoryID,HardwareInfoID),
                CONSTRAINT FK_HardwareInfo FOREIGN KEY (HardwareInfoID)
                REFERENCES HardwareInfo(ID),
                CONSTRAINT FK_Category FOREIGN KEY (CategoryID)
                REFERENCES Category(ID)
                );
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
