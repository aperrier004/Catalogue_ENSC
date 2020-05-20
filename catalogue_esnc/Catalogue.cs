// Fichier:    Catalogue.cs
// Auteur:  ColineB AlbanP
// But: Definition de la classe Eleve
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;

namespace catalogue_ensc
{
    class Catalogue
    {
        // Attributs de la classe
        private List<Projet.Projet> listeProjets;
        private int nbProjets;

        // Propriétés
        public List<Projet.Projet> ListeProjets
        {
            get
            {
                return listeProjets;
            }
            set
            {
                this.listeProjets = value;
            }
        }

        public int NbProjets
        {
            get
            {
                return nbProjets;
            }
            set
            {
                this.nbProjets = value;
            }
        }

        public void AjouterProjet(Projet.Projet p)
        {
            ListeProjets.Add(p);
            NbProjets += 1;

        }

        public void SupprimerProjet(Projet.Projet p)
        {
            ListeProjets.Remove(p);
            NbProjets -= 1;

        }

        public Catalogue(List<Projet.Projet> liste)
        {
            ListeProjets = liste;
            NbProjets = ListeProjets.Count;
        }
        // Constructeur
        public Catalogue()
        {
            ListeProjets = new List<Projet.Projet>();
            NbProjets = 0;
        }

        // But : Permet de créer un objet catalogue
        // Paramètres : Objet de connexion SQLite, un critere, et deux champs de données
        // Retourne : Un objet catalogue
        public static Catalogue CreerCatalogue(SQLiteConnection maConnexion, string critere, string data1, string data2)
        {
            // Requete préparée 
            var cmd = new SQLiteCommand(maConnexion);
            // Requête SQL selon le critère
            switch (critere)
            {
                case "annee":
                    // Critère de tri = par année
                    // transformation de la valeur donnee en argument en la valeur attendue en BD
                    int annee = Int32.Parse(data1);

                    // Requete
                    cmd.CommandText = "SELECT * FROM projet WHERE projet_annee = @annee";
                    cmd.Parameters.AddWithValue("@annee", annee);
                    break;

                case "eleve":
                    // Critère de tri = par eleve
                    // Requete pour recuperer l'eleve
                    var cmdEleve = new SQLiteCommand(maConnexion);
                    cmdEleve.CommandText = "SELECT * FROM eleve WHERE eleve_nom = @nom AND eleve_prenom = @prenom";
                    cmdEleve.Parameters.AddWithValue("@nom", data1);
                    cmdEleve.Parameters.AddWithValue("@prenom", data2);

                    cmdEleve.Prepare();
                    //execute la requete et mets les lignes dans le reader.
                    SQLiteDataReader readerEleve = cmdEleve.ExecuteReader();
                    //pour chaque ligne
                    while (readerEleve.Read())
                    {
                        // Recupere les projets que l'éleve a realisés
                        var cmdReal = new SQLiteCommand(maConnexion);
                        cmdReal.CommandText = "SELECT * FROM realiser WHERE eleve_id = @eleve_id";
                        cmdReal.Parameters.AddWithValue("@eleve_id", readerEleve["eleve_id"]);

                        cmdReal.Prepare();
                        SQLiteDataReader readerReal = cmdReal.ExecuteReader();
                        if (readerReal.HasRows)
                        {
                            //pour chaque ligne
                            while (readerReal.Read())
                            {
                                // Requete pour recuperer les infos du projet
                                cmd.CommandText = "SELECT * FROM projet WHERE projet_id = @projet_id";
                                cmd.Parameters.AddWithValue("@projet_id", readerReal["projet_id"]);
                            }
                        }
                        else
                        {
                            // Requete par défaut
                            cmd.CommandText = "SELECT * FROM realiser WHERE eleve_id = @eleve_id";
                            cmd.Parameters.AddWithValue("@eleve_id", readerEleve["eleve_id"]);
                        }
                    }
                   
                    break;

                case "promotion":
                    // Requete pour recuperer les élèves appartenant à la promotion passee en parametre
                    var cmdPromotion = new SQLiteCommand(maConnexion);
                    cmdPromotion.CommandText = "SELECT * FROM eleve WHERE eleve_promotion = @promotion";
                    cmdPromotion.Parameters.AddWithValue("@promotion", data1);

                    cmdPromotion.Prepare();
                    SQLiteDataReader readerPromotion = cmdPromotion.ExecuteReader();
                    while (readerPromotion.Read()) // On parcourt ces élèves
                    {
                        // Recup les projets que l'éleve appartenant à cette promotion ont realisés
                        var cmdReal = new SQLiteCommand(maConnexion);
                        cmdReal.CommandText = "SELECT * FROM realiser WHERE eleve_id = @eleve_id";
                        cmdReal.Parameters.AddWithValue("@eleve_id", readerPromotion["eleve_id"]);

                        cmdReal.Prepare();
                        SQLiteDataReader readerReal = cmdReal.ExecuteReader();
                        if (readerReal.HasRows)
                        {
                            while (readerReal.Read())
                            {
                                // Requete pour le projet
                                cmd.CommandText = "SELECT * FROM projet WHERE projet_id = @projet_id";
                                cmd.Parameters.AddWithValue("@projet_id", readerReal["projet_id"]);
                            }

                        }
                        else
                        {
                            // par défaut
                            cmd.CommandText = "SELECT * FROM realiser WHERE eleve_id = @eleve_id";
                            cmd.Parameters.AddWithValue("@eleve_id", readerPromotion["eleve_id"]);
                        }

                    }
                    break;
                case "motClef":
                    // Requete pour recuperer les projets associés au mot clef
                    var cmdMotClef = new SQLiteCommand(maConnexion);
                    cmdMotClef.CommandText = "SELECT * FROM motClef WHERE motClef_lib = @mot";
                    cmdMotClef.Parameters.AddWithValue("@mot", data1);

                    cmdMotClef.Prepare();
                    SQLiteDataReader readerMotCLef = cmdMotClef.ExecuteReader();
                    while (readerMotCLef.Read())
                    {
                        // recuperer les projets qui ont pour mot clef celui donnée
                        cmd.CommandText = "SELECT * FROM projet WHERE projet_id = @projet_id";
                        cmd.Parameters.AddWithValue("@projet_id", readerMotCLef["projet_id"]);
                    }
                    break;

                case "typeProjet":
                    // Requete
                    cmd.CommandText = "SELECT * FROM projet WHERE projet_libType = @typeP";
                    cmd.Parameters.AddWithValue("@typeP", data1);
                    break;

                case "matiere":
                    // Requete pour recuperer les matieres
                    var cmdMatiere = new SQLiteCommand(maConnexion);
                    cmdMatiere.CommandText = "SELECT * FROM matiere WHERE matiere_lib = @matiere";
                    cmdMatiere.Parameters.AddWithValue("@matiere", data1);

                    cmdMatiere.Prepare();
                    SQLiteDataReader readerMatiere = cmdMatiere.ExecuteReader();
                    while (readerMatiere.Read())
                    {
                        // Requete pour trouver les projets
                        cmd.CommandText = "SELECT * FROM projet WHERE projet_idMatiere = @matiere_id";
                        cmd.Parameters.AddWithValue("@matiere_id", readerMatiere["matiere_id"]);
                    }
                    break;

                case "prof":
                    // Requete pour recuperer les profs
                    var cmdProf = new SQLiteCommand(maConnexion);
                    cmdProf.CommandText = "SELECT * FROM prof WHERE prof_nom = @prof_nom AND prof_prenom = @prof_prenom";
                    cmdProf.Parameters.AddWithValue("@prof_nom", data1);
                    cmdProf.Parameters.AddWithValue("@prof_prenom", data2);

                    cmdProf.Prepare();
                    SQLiteDataReader readerProf = cmdProf.ExecuteReader();
                    while (readerProf.Read())
                    {
                        // Requete pour trouver les projets
                        cmd.CommandText = "SELECT * FROM projet WHERE prof_id = @prof_id";
                        cmd.Parameters.AddWithValue("@prof_id", readerProf["prof_id"]);
                    }
                    break;

                case "intervenant":
                    // Requete pour recuperer les intervenants
                    var cmdInt = new SQLiteCommand(maConnexion);
                    cmdInt.CommandText = "SELECT * FROM intervenant WHERE intervenant_nom = @intervenant_nom AND intervenant_prenom = @intervenant_prenom";
                    cmdInt.Parameters.AddWithValue("@intervenant_nom", data1);
                    cmdInt.Parameters.AddWithValue("@intervenant_prenom", data2);

                    cmdInt.Prepare();
                    SQLiteDataReader readerInt = cmdInt.ExecuteReader();
                    while (readerInt.Read())
                    {
                        // Requete pour trouver les projets
                        cmd.CommandText = "SELECT * FROM projet WHERE projet_idClientInt = @intervenant_id OR projet_idTuteurInt = @intervenant_id";
                        cmd.Parameters.AddWithValue("@intervenant_id", readerInt["intervenant_id"]);
                    }

                    break;

                default:
                    // Requete pour trouver tous les projets
                    cmd.CommandText = "SELECT * FROM projet";
                    break;
            }

            // on execute la requête
            cmd.Prepare();
            SQLiteDataReader reader = cmd.ExecuteReader();

            // On cree la liste de projets en fonctions des resultats obtenus precedents inscrits dans le reader
            List<Projet.Projet> listeProjets = Projet.Projet.CreerListeProjets(maConnexion, reader);
            // On créé un catalogue recensant les projets triés selon le ou les criteres precisés
            Catalogue cat = new Catalogue(listeProjets);
            // On retourne le catalogue
            return cat;

        }

        // But : Permet de lancer la génération du catalogue sans tri
        // Paramètres : objet Catalogue
        public static void AfficherCatalogue(Catalogue cat, SQLiteConnection maConnexion)
        {
            // Liste d'id des projets afficheés
            ArrayList id = new ArrayList();
            for (int i = 0; i < cat.NbProjets; i++)
            {
                // On ajoute l'id a la liste
                id.Add(cat.ListeProjets[i].IdProjet);
            }

            Console.WriteLine(cat.ToString());
            if(cat.NbProjets > 0)
            {
                // On demande si l'utlisateur souhaite connaitre le détail du projet
                string idS = "";
                do
                {
                    Console.WriteLine("Si vous souhaitez connaître le détail d'un projet, entrer l'id de celui-ci, sinon appuyez sur entrée");
                    idS = Console.ReadLine();
                    if (idS.Equals("")) break;

                } while (!Program.ValiderChaine("id", idS, id));

                if (!idS.Equals("")) // S'il entre un id de projet
                {
                    int projet_id = Int32.Parse(idS);
                    for (int i = 0; i < cat.NbProjets; i++)
                    {
                        if (cat.ListeProjets[i].IdProjet == projet_id)
                        {
                            // On crée un objet Projet
                            Projet.Projet pDetail = cat.ListeProjets[i];

                            // On appeller la fonction pour afficher le détail du projet
                            Projet.Projet.AfficherDetailProjet(pDetail, cat, maConnexion);
                        }
                    }
                } else
                {
                    Program.AfficherMenuCatalogue();
                }
            }
            else // Retour au menu
            {
                Program.AfficherMenuCatalogue();
            }
        }

        // But : Permet d'afficher les informations de la classe
        // Retourne : Une chaine de caractères
        public override string ToString()
        {
            string txt = "";
            for (int i = 0; i < NbProjets; i++)
            {
                txt += "=========== Projet " + ListeProjets[i].IdProjet + "=========== \n";
                txt += "Titre du projet : " + ListeProjets[i].LibelleProjet + "\n";
                txt += "Type du projet : " + ListeProjets[i].TypeProjet + "\n";
                txt += "Année : " + ListeProjets[i].Annee + "\n";
                txt += "Note : " + ListeProjets[i].NoteProjet + "\n";
                txt += "Etudiant(s) : \n";

                for (int j = 0; j < ListeProjets[i].ListeEleves.Count; j++)
                {
                    txt += ListeProjets[i].ListeEleves[j].ToString() + "\n";
                }
            }
            if(txt == "")
            {
                txt = "Aucun projet ne correspond à ces critères.";
            }

            return txt;
        }
    }
}
