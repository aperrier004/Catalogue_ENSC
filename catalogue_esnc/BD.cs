// Fichier:    BD.cs
// Auteur:  ColineB AlbanP
// But: Definition de la classe BD
using System;
using System.Data;
using System.Data.SQLite;

namespace catalogue_ensc
{
    class BD // Classe permettant de gérer la Base de Données
    {
        // But : Initialise la BD
        public static void InitialiserBD(SQLiteConnection maConnexion)
        {
            Console.WriteLine("La BD va être initialisé");

            if (maConnexion != null && maConnexion.State.HasFlag(ConnectionState.Open)) // On regarde si la connexion a déjà été ouverte
            {
                // Si c'est le cas on l'a ferme
                maConnexion.Close();
                // On demande au Garbage Collector de faire son travail
                GC.Collect();
            }
            // On le laisse terminer
            GC.WaitForPendingFinalizers();
            // On créé un fichier de BD
            SQLiteConnection.CreateFile("MaBaseDeDonnees.sqlite");

            // On ouvre la connexion avec le fichier
            maConnexion = new SQLiteConnection("Data Source=MaBaseDeDonnees.sqlite;Version=3;");
            maConnexion.Open();

            // Suppression des tables deja existantes au cas où elles existent
            SupprimerTable(maConnexion);
            // Creation de la structure de la DB
            CreerStructure(maConnexion);
            // Insertion de donnees dans la DB
            InsererDonnee(maConnexion);
        }

        // But : Permet de créer la structure de la BD
        // Paramètres : Objet de connexion SQLite
        public static void CreerStructure(SQLiteConnection maConnexion)
        {
            // CREATE TABLE

            // Création de la table prof
            string sql = "create table prof (prof_id int, prof_nom text, prof_prenom text)";
            SQLiteCommand commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table enseignement
            sql = "create table enseignement (prof_id int, matiere_id int)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table eleve
            sql = "create table eleve (eleve_id int, eleve_nom text, eleve_prenom text, eleve_promotion int)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table intervenant
            sql = "create table intervenant (intervenant_id int, intervenant_nom text, intervenant_prenom text, intervenant_metier text)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table matiere
            sql = "create table matiere (matiere_id int, matiere_lib text, matiere_code text)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table livrable
            sql = "create table livrable (livrable_id int, livrable_nature text)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table rendu
            sql = "create table rendu (livrable_id int, projet_id int, rendu_date text, rendu_note real)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table intervient
            sql = "create table intervient (intervenant_id int, projet_id int)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table motClef
            sql = "create table motClef (projet_id int, motClef_lib text)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table projet
            sql = "create table projet (projet_id int, projet_dateDebut text, projet_dateFin text, projet_sujetLibre int, projet_annee int, projet_lib text, projet_libSujet text, prof_id int, projet_collectif int, projet_noteProjet real, projet_idClientProf int, projet_idClientInt int, projet_idTuteurProf int, projet_idTuteurInt int, projet_idMatiere, projet_libType text)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();

            // Création de la table realiser
            sql = "create table realiser (projet_id int, eleve_id int)";
            commande = new SQLiteCommand(sql, maConnexion);
            commande.ExecuteNonQuery();
        }
        // But : Permet d'insérer des données dans la table
        // Paramètres : Objet de connexion SQLite
        public static void InsererDonnee(SQLiteConnection maConnexion)
        {
            // INSERTION dans les tables

            // Insertion de données dans la table prof
            string sql2 = "insert into prof values (1,'Sophie', 'Solomas')";
            SQLiteCommand commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into prof values (2,'Saracco', 'Jerome')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into prof values (3,'Favier', 'Pierre-Alexandre')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into prof values (4,'Semal', 'Catherine')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into prof values (5,'Clermont', 'Edwige')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table matiere
            sql2 = "insert into matiere values (1,'Anglais', 'CO5INAN0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into matiere values (2,'Probabilités et statistique', 'CO5SFMA0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into matiere values (3,'Programmation avancée', 'CO6SFPA0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into matiere values (4,'Projet Transdisciplinaire', 'CO5PRTD0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into matiere values (5,'Projet Transpromotion', 'CO5PRTP0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into matiere values (6,'Communication Web', 'CO6SFCW0')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table enseignement (enseignant, matiere)
            sql2 = "insert into enseignement values (1,1)"; // Mme Solomas enseigne l'anglais
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into enseignement values (2,2)"; // M. Saracco enseigne proba et stats
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into enseignement values (3,3)"; // M.Favier enseigne la prog av.
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into enseignement values (4,4)"; // Mme Semal est responsable des projets transdi
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into enseignement values (4,5)"; // Mme Semal est responsable des projets transpromo
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into enseignement values (5,6)"; // Mme Clermont enseigne la communication web
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table eleve
            sql2 = "insert into eleve values (1,'Perrier', 'Alban', 2022)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (2,'Binet', 'Coline', 2022)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (3,'De Clairmont', 'Hugues', 2022)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (4,'Weasley', 'Fred', 2021)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (5,'Weasley', 'George', 2021)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (6,'Dilleux', 'Eulalie', 2018)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (7,'Pevencie', 'Susan', 2018)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into eleve values (8,'Bishop', 'Diana', 2018)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table intervenant
            sql2 = "insert into intervenant values (1,'Fernandez', 'Alvaro', 'Thesard')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (2,'Directrice', 'Faragonda', 'Directrice de Alphéa')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (3,'Thatch', 'Milo', 'Expert linguiste et cartographe')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (4,'Nedakh', 'Kidagakash', 'Atlante')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (5,'Bak', 'Sun', 'Assistante et Boxeuse')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (6,'Layton', 'Hershel', 'Professeur d Archéologie')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervenant values (7,'Scamander', 'Newt', 'auteur du livre Vie et habitat des animaux fantastiques')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table livrable
            sql2 = "insert into livrable values (1, 'Rapport')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into livrable values (2, 'Code source')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into livrable values (3, 'Soutenance')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into livrable values (4, 'Cahier des Charges')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into livrable values (5, 'Site Web')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table motClef
            sql2 = "insert into motClef values (1, 'transdisciplinaire'), (1, 'BCI'), (1, 'auditif')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into motClef values (2, 'programmation avancée'), (2, 'Catalogue'), (2, 'projet')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into motClef values (3, 'transdisciplinaire'), (3, 'Créatures fantastiques'), (3, 'Grande Bretagne')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into motClef values (4, 'programmation avancee'), (4, 'Enigmes')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            //0 pour des id car données nulles 
            // Insertion de données dans la table projet
            sql2 = "insert into projet values (1, '2019-09-10', '2020-01-10', 0, 2019, 'BCI-AIDP300', 'Tester la performance d un BCI auditif', 1, 1, 15, 1, 0, 1, 4, 4, 'Transdisciplinaire')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            /* id_projet, dateDebut, DateFin, sujet libre, annee, libelleProjet, libelleSujet, profReferant, projetCollectif?,
             note, idClientProf, idClientIntervenant, idTuteurProf, idTuteurIntervenant, idMatiere, libTypeProjet */

            sql2 = "insert into projet values (2, '2020-03-23', '2020-05-18', 0, 2020, 'Projet Programmation Avancée',"+ 
            "'Créer un catalogue de projets', 3, 1, 20, 0, 0, 0, 3, 3, 'Projet Programmation Avancée')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into projet values (3, '2018-09-10', '2019-06-10', 0, 2018, 'Projet Transdisciplinaire'," +
            "'Recenser les créatures fantastiques de Grande Bretagne', 3, 1, 14, 0, 7, 0, 4, 0,'Transdisciplinaire')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into projet values (4, '2018-03-23', '2018-05-18', 0, 2018, 'Projet Programmation Avancée'," +
            "'Dictionnaire enigmes', 3, 1, 16, 0, 0, 0, 3, 3, 'Projet Programmation Avancée')";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table rendu
            sql2 = "insert into rendu values (1, 1, '2020-01-10 10:04 AM', 15)"; //rapport projet 1 rendu le 10 janvier 2020, note = 15
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into rendu values (1, 2, '2020-05-15 10:04 AM', 19)"; //rapport projet 2 rendu le 15 main 2020, note = 19
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into rendu values (2, 2, '2020-05-15 10:04 AM', 20)"; //Code source projet 2 rendu le 15 main 2020, note = 20
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();



            sql2 = "insert into rendu values (4, 3, '2019-01-10 10:04 AM', 14)"; //CdC projet 3 rendu le 10 janvier 2019, note = 14
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into rendu values (3, 3, '2019-06-10 10:04 AM', 14)"; //soutenance projet 3 effectuee le 10 juin 2019, note = 14
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into rendu values (1, 4, '2018-05-18 10:04 AM', 15)"; //rapport projet 4 rendu le 18 mai 2018, note = 15
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into rendu values (2, 4, '2018-05-18 10:04 AM', 17)"; //code source projet 4 rendu le 18 mai 2018, note = 16
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table realiser
            sql2 = "insert into realiser values (1, 1)"; //Alban realise le projet transdi BCI-AIDP300
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (2, 1)"; // Alban realise le projet progAv Catalogue
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (2, 2)"; // Coline aussi le realise evidemment
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (3, 4)"; // Fred Weasley realise le transdi avec les créatures fantastiques
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (3, 5)"; // George aussi, les jumeaux sont inséparables.
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (4, 6)"; // Eulalie Dilleux travaille sur le sujet de prog Avancée des enigmes
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into realiser values (4, 7)"; // Susan a rangé son arc et travaille également sur le sujet des énigmes
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            // Insertion de données dans la table intervient
            sql2 = "insert into intervient values (1, 1)";
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervient values (3, 7)"; // Newt Scamander intervient dans le projet sur les créatures fantastiques
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();

            sql2 = "insert into intervient values (4, 6)"; // Qui de mieux que le professeur Layton pour aider Eulalie et Susan à rassembler des énigmes ?
            commande2 = new SQLiteCommand(sql2, maConnexion);
            commande2.ExecuteNonQuery();
        }
        // But : Permet de Supprimer les tables
        // Paramètres : Objet de connexion SQLite
        public static void SupprimerTable(SQLiteConnection maConnexion)
        {
            // Tableau contenant le nom des tables
            string[] table = new string[] { "prof", "enseignement", "eleve", "intervenant", "matiere", "livrable", "rendu", "projet", "realiser", "intervient", "motClef" };

            foreach (string element in table) // Pour chaque élément dans la table
            {
                // On supprime toutes les tables qui existent
                string sql0 = "DROP TABLE IF EXISTS " + element;
                // Requete préparée
                SQLiteCommand commande0 = new SQLiteCommand(sql0, maConnexion);
                // Que l'on éxecute
                commande0.ExecuteNonQuery();
            }

        }

        // But : Permet de Supprimer les données incohérentes dans la BD
        // Paramètres : Objet de connexion SQLite
        public static void VerifierDonneesApresFermetureBrusque(SQLiteConnection maConnexion)
        {
            // Tableau contenant le nom des tables que l'on veut vérifier
            string[] table = new string[] { "rendu", "realiser", "intervient", "motClef" };

            long projet_id = Projet.Projet.ChercherDernierIdProjet(maConnexion);
            // On passe à l'id supérieur qui en théorie n'existe pas
            projet_id++;

            foreach (string element in table) // Pour chaque élément dans la table
            {
                // On regarde si une table a un projet_id supérieur au dernier projet ajouté
                string sql = "SELECT * FROM " + element + " WHERE projet_id = " + projet_id;
                // Requete préparée
                SQLiteCommand cmd = new SQLiteCommand(sql, maConnexion);
                // Que l'on éxecute
                SQLiteDataReader reader = cmd.ExecuteReader();
                // Si un résultat a été trouvé
                if(reader.Read())
                {
                    // On supprime les données
                    string sql1 = "DELETE FROM " + element + " WHERE projet_id = " + projet_id;
                    // Requete préparée
                    SQLiteCommand commande0 = new SQLiteCommand(sql1, maConnexion);
                    // Que l'on éxecute
                    commande0.ExecuteNonQuery();
                }
            }
        }
    }
}
