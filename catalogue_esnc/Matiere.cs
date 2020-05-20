// Fichier: Matiere.cs
// Auteur:  ColineB AlbanP
// But:     Definition de la classe Matiere
using System;
using System.Collections;
using System.Data.SQLite;

namespace catalogue_ensc
{
    class Matiere
    {
        // Attributs de la classe
        private int idMatiere;
        private string libelleMatiere;
        private string codeMatiere;

        // Propriétés
        public int IdMatiere
        {
            get
            {
                return idMatiere;
            }
            set
            {
                this.idMatiere = value;
            }
        }

        public string NomMatiere
        {
            get
            {
                return libelleMatiere;
            }
            set
            {
                this.libelleMatiere = value;
            }
        }

        public string CodeMatiere
        {
            get
            {
                return codeMatiere;
            }
            set
            {
                this.codeMatiere = value;
            }
        }


        // Constructeur
        public Matiere(int id, string nomMatiere, string code)
        {
            IdMatiere = id;
            NomMatiere = nomMatiere;
            CodeMatiere = code;
        }
        // But : Permet de créer une liste des matières qu'un prof enseigne
        // Paramètres : Objet de connexion SQLite, un int prof_id
        // Retourne : Une liste contenant la liste des matières
        public static ArrayList CreerListeMatiere(int prof_id, SQLiteConnection maConnexion)
        {
            // Initialisation de la liste
            ArrayList listeMat = new ArrayList();
            // Requete préparée pour recuperer tous les matiere_id qui sont associés au prof_id donné
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM enseignement WHERE prof_id = @prof_id";

            cmd.Parameters.AddWithValue("@prof_id", prof_id);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();

            // Tant qu'il y a une matiere_id à lire
            while (reader.Read())
            {
                // On recup les donnees du livrable
                var cmdMat = new SQLiteCommand(maConnexion);
                cmdMat.CommandText = "SELECT * FROM matiere WHERE matiere_id = @matiere_id";

                cmdMat.Parameters.AddWithValue("@matiere_id", (int)reader["matiere_id"]);
                cmdMat.Prepare();

                SQLiteDataReader reader2 = cmdMat.ExecuteReader();
                
                while (reader2.Read())
                {
                    // On cree l'objet Matière
                    Matiere matiere = new Matiere((int)reader2["matiere_id"], (string)reader2["matiere_lib"], (string)reader2["matiere_code"]);
                    // On ajoute l'objet a la liste
                    listeMat.Add(matiere);
                }
            }
            return listeMat;
        }
        // But : Permet d'afficher toutes les matières présentes en BD
        // Paramètres : Objet de connexion SQLite
        // Retourne : Une liste contenant les id des matières
        public static ArrayList ListeMatiereBD(SQLiteConnection maConnexion)
        {
            // Initialisation de la liste des id Matiere
            ArrayList idMatiere = new ArrayList();
            Console.WriteLine("Listes des matières existantes : \n\n");
            // Recup toutes les matières existantes
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM matiere";
            cmd.Prepare();
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // On affiche les informations
                Console.WriteLine("- " + reader["matiere_id"] + " : " + reader["matiere_lib"] + " (" + reader["matiere_code"] + ")");
                // On ajoute l'id à la liste
                idMatiere.Add(reader["matiere_id"]);
            }
            // On retourne la liste
            return idMatiere;
        }
        // But : Permet de chercher le dernier id de la table matiere
        // Paramètres : Objet de connexion SQLite
        // Retourne : un long id de la matière
        static long ChercherDernierIdMatiere(SQLiteConnection maConnexion)
        {
            // Id de la matiere
            long lastId = 0;
            // Requête pour chercher le dernier id
            var cmdId = new SQLiteCommand(maConnexion);
            cmdId.CommandText = "SELECT MAX(matiere_id) as matiere_idMax FROM matiere";
            cmdId.Prepare();
            SQLiteDataReader readerId = cmdId.ExecuteReader();

            while (readerId.Read())
            {
                lastId = (long)readerId["matiere_idMax"];
            }
            return lastId;
        }
        // But : Permet d'ajouter un matière
        // Paramètres : Objet de connexion SQLite
        public static void AjouterMatiere(SQLiteConnection maConnexion)
        {
            // dernier id matiere
            long lastId = ChercherDernierIdMatiere(maConnexion) + 1;

            // Nom de la matière
            string nomMatiere = "";
            do
            {
                Console.WriteLine("Entrer le nom de la matière que vous souhaitez ajouter : ");
                nomMatiere = Console.ReadLine();
            } while (nomMatiere.Length < 1);

            // Code de la matière
            string code = "";
            do
            {
                Console.WriteLine("Entrer le code de la matière correspondant :");
                code = Console.ReadLine();
            } while (code.Length < 1);
            // Création de l'objet Matière
            Matiere m = new Matiere((int)lastId, nomMatiere, code);
            // On appelle la fonction pour ajouter une matière en BD
            AjouterMatiereBD(maConnexion, m);
        }
        // But : Permet d'ajouter une matière en BD
        // Paramètres : Objet de connexion SQLite, un objet Matiere
        public static void AjouterMatiereBD(SQLiteConnection maConnexion, Matiere m)
        {
            // On vérifie que la matière n'a pas déjà été ajoutée
            var cmd = new SQLiteCommand(maConnexion);
            cmd.CommandText = "SELECT * FROM matiere WHERE matiere_code = @codeM";
            cmd.Parameters.AddWithValue("@codeM", m.CodeMatiere);
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
                Console.WriteLine("Ajout impossible, la matières existe déjà");
            }
            else // la matière n'existe pas
            {
                // Insertion de données dans la table matiere
                cmd.CommandText = "INSERT INTO matiere VALUES (@idM, @nomM, @codeM)";
                cmd.Parameters.AddWithValue("@idM", m.IdMatiere);
                cmd.Parameters.AddWithValue("@nomM", m.NomMatiere);
                cmd.Parameters.AddWithValue("@codeM", m.CodeMatiere);
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
            texte += "Libellé de la matière : " + NomMatiere + "\n";
            texte += "Code de la matière : "+ CodeMatiere ;
            return texte;

        }

    }
}
