// Fichier:    Livrable.cs
// Auteur:  ColineB AlbanP
// But: Definition de la classe Livrable
using System;
using System.Collections;
using System.Data.SQLite;

namespace catalogue_ensc
{
    class Livrable
    {
        // Attributs de la classe
        private int idLivrable;
        private String natureLivrable;
        private DateTime dateRendu;
        private double noteLivrable;

        // Propriétés
        public int IdLivrable
        {
            get
            {
                return idLivrable;
            }
            set
            {
                this.idLivrable = value;
            }
        }

        public String NatureLivrable
        {
            get
            {
                return natureLivrable;
            }
            set
            {
                this.natureLivrable = value;
            }
        }

        public DateTime DateRendu
        {
            get
            {
                return dateRendu;
            }
            set
            {
                this.dateRendu = value;
            }
        }

        public double NoteLivrable
        {
            get
            {
                return noteLivrable;
            }
            set
            {
                this.noteLivrable = value;
            }
        }
        // Constructeur complet
        public Livrable(int id, string natureLivrable, DateTime date, double note)
        {
            IdLivrable = id;
            NatureLivrable = natureLivrable;
            DateRendu = date;
            NoteLivrable = note;
        }
        // Constructeur raccourci pour la BD
        public Livrable(int id, string natureLivrable)
        {
            IdLivrable = id;
            NatureLivrable = natureLivrable;
        }
        // But : Permet de créer une liste des livrables rendus pour un projet
        // Paramètres : Objet de connexion SQLite et un projet_id
        // Retourne : Une liste contenant les livrables associés à un projet
        public static ArrayList CreerListeLivrables(int projet_id, SQLiteConnection maConnexion)
        {
            // Initialisation de la liste
            ArrayList listeL = new ArrayList();

            // Requete préparée pour recup tous les livrable_id qui sont associés au projet_id donné
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM rendu WHERE projet_id = @projet_id";

            cmd.Parameters.AddWithValue("@projet_id", projet_id);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();

            // Tant qu'il y a un livrable_id à lire
            while (reader.Read())
            {
                // On recup les donnees du livrable
                var cmdLivrable = new SQLiteCommand(maConnexion);
                cmdLivrable.CommandText = "SELECT * FROM livrable WHERE livrable_id = @livrable_id";

                cmdLivrable.Parameters.AddWithValue("@livrable_id", (int)reader["livrable_id"]);
                cmdLivrable.Prepare();

                SQLiteDataReader reader2 = cmdLivrable.ExecuteReader();
                // Attributs
                DateTime rendu_date = DateTime.Parse((string)reader["rendu_date"]);
                double note = (double)reader["rendu_note"];

                // On cree les livrables
                while (reader2.Read())
                {
                    // Création de l'objet livrable
                    Livrable livrable = new Livrable((int)reader2["livrable_id"], (string)reader2["livrable_nature"], rendu_date, note);
                    // On ajoute l'objet à la liste
                    listeL.Add(livrable);
                }
            }
            // On retourne la liste
            return listeL;
        }

        // But : Permet d'ajouter un lien entre un livrable et un projet dans la table Rendu
        // Paramètres : Objet de connexion SQLite, un long projet_id, un int livrable_id, une date de rendu en chaine de caractères et une note du livrable
        public static bool AjouterRenduLivrableBD(SQLiteConnection maConnexion, long projet_id, int livrable_id, string rendu_date, double rendu_note)
        {
            bool insertion = false;

            var cmdInsert = new SQLiteCommand(maConnexion);
            cmdInsert.CommandText = "SELECT * FROM rendu WHERE projet_id = @projet_id AND livrable_id = @livrable_id";
            cmdInsert.Parameters.AddWithValue("@projet_id", projet_id);
            cmdInsert.Parameters.AddWithValue("@livrable_id", livrable_id);
            cmdInsert.Prepare();
            SQLiteDataReader readerInsert = cmdInsert.ExecuteReader();
            if (readerInsert.Read()) // Si un résultat a été trouvé c'est qu'il existe déjà
            {
                Console.WriteLine("Objet déjà ajouté");
            }
            else
            {
                insertion = true;
            }
            if (insertion)
            {
                // Insertion de données dans la table projet
                var cmd = new SQLiteCommand(maConnexion);
                cmd.CommandText = "INSERT INTO rendu values (@projet_id, @livrable_id, @rendu_date, @rendu_note)";

                cmd.Parameters.AddWithValue("@projet_id", projet_id);
                cmd.Parameters.AddWithValue("@livrable_id", livrable_id);
                cmd.Parameters.AddWithValue("@rendu_date", rendu_date);
                cmd.Parameters.AddWithValue("@rendu_note", rendu_note);

                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1) Console.WriteLine("Problème dans l'insertion du livrable"); 
            }
            return insertion;
        }
        // But : Permet d'afficher tous les livrables présents en BD
        // Paramètres : Objet de connexion SQLite
        // Retourne : Une liste contenant les id des livrables
        public static ArrayList ListeLivrablesBD(SQLiteConnection maConnexion)
        {
            // Liste correspondants aux id des livrables affichés
            ArrayList idLivrable = new ArrayList();
            Console.WriteLine("Listes des livrables existants : \n\n");
            // Recup tous les livrables existants
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM livrable";
            cmd.Prepare();
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // On affiche les informations
                Console.WriteLine("- " + reader["livrable_id"] + " : " + reader["livrable_nature"]);
                // On ajoute l'id du livrable dans la liste
                idLivrable.Add(reader["livrable_id"]);
            }
            // On retourne la liste
            return idLivrable;
        }
        // But : Permet de trouver le dernier id dans la table livrable
        // Paramètres : Objet de connexion SQLite
        static long ChercherDernierIdLivrable(SQLiteConnection maConnexion)
        {
            // Id du livrable
            long lastId = 0;
            // Requete pour trouver le dernier id Livrable
            var cmdId = new SQLiteCommand(maConnexion);
            cmdId.CommandText = "SELECT MAX(livrable_id) as livrable_idMax FROM livrable";
            cmdId.Prepare();
            SQLiteDataReader readerId = cmdId.ExecuteReader();

            while (readerId.Read())
            {
                lastId = (long)readerId["livrable_idMax"];
            }
            // On retourne l'id
            return lastId;
        }
        // But : Permet d'ajouter un Livrable
        // Paramètres : Objet de connexion SQLite
        public static void AjouterLivrable(SQLiteConnection maConnexion)
        {
            // dernier id livrable
            long lastId = ChercherDernierIdLivrable(maConnexion) + 1;

            // Nature du livrable
            string natureLivrable = "";
            do
            {
                Console.WriteLine("Entrer la nature du livrable que vous souhaitez ajouter : ");
                natureLivrable = Console.ReadLine();
            } while (natureLivrable.Length < 1);
            // Création de l'objet Livrable
            Livrable l = new Livrable((int)lastId, natureLivrable);
            // On appelle la fonction pour ajouter un livrable en BD
            AjouterLivrableBD(maConnexion, l);
        }
        // But : Permet d'ajouter un livrable en BD
        // Paramètres : Objet de connexion SQLite, un objet Livrable
        public static void AjouterLivrableBD(SQLiteConnection maConnexion, Livrable l)
        {
            // On vérifie que le livrable n'a pas déjà été ajouté en BD
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM livrable WHERE livrable_nature = @livrable_nature";
            cmd.Parameters.AddWithValue("@livrable_nature", l.NatureLivrable);
            cmd.Prepare();
            SQLiteDataReader reader = cmd.ExecuteReader();

            bool existe = false;

            while (reader.Read())
            {
                existe = true;
            }
            reader.Close();
            if (existe)
            {
                Console.WriteLine("Ajout impossible, ce livrable existe déjà");
            }
            else // le livrable n'existe pas
            {
                // Insertion de données dans la table Livrable
                cmd = new SQLiteCommand(maConnexion);
                cmd.CommandText = "INSERT INTO livrable VALUES (@idL, @nomL)";
                cmd.Parameters.AddWithValue("@idL", l.IdLivrable);
                cmd.Parameters.AddWithValue("@nomL", l.NatureLivrable);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("Insertion correctement effectuée");
                }
                else
                {
                    Console.WriteLine("Une erreur est survenue dans l'insertion");
                }
            }
        }
        // But : Permet d'afficher les informations de la classe
        // Retourne : Une chaine de caractères
        public override string ToString()
        {
            string texte = "";
            texte += "Nature du livrable :"+ NatureLivrable + " \n";
            texte += "Date du rendu :" + DateRendu.ToString("MM/dd/yyyy HH:mm") + " \n";
            texte += "Note :" + NoteLivrable;

            return texte;

        }
    }
}
