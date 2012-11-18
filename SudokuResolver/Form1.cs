using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ConfigGlobaleNameSpace;
using SudokuSolver;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        ConfigGlobale m_oConfig = new ConfigGlobale();
        Resolver m_oResolver;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnParcourirGrille_Click(object sender, EventArgs e)
        {
            // Répertoire par défaut : emplacement de l'executable
            this.ofdGrille.InitialDirectory = m_oConfig.GetRepApplication();

            // Ouverture de la fenêtre sélecteur de fichier
            DialogResult drResult = this.ofdGrille.ShowDialog();
            
            // Si l'utilisateur valide
            if (drResult == DialogResult.OK)
            {
                // Affichage du fichier sélectionné dans le TextBox
                this.txtParcourir.Text = this.ofdGrille.FileName;

                // Si le fichier existe, on l'ouvre et on extrait la grille
                if (File.Exists(this.txtParcourir.Text))
                {
                    // Variables
                    StreamReader srGrille = null;
                    string sLigne;
                    int nLigne = 0;
                    int nColonne;
                    int[,] tabGrille = new int[9, 9];

                    try
                    {
                        // Ouverture du fichier texte
                        srGrille = new StreamReader(this.txtParcourir.Text);

                        // Lecture du contenu, ligne pas ligne
                        sLigne = srGrille.ReadLine();
                        while (sLigne != null)
                        {
                            if (sLigne != string.Empty)
                            {
                                // Récupération des chiffres de chaque colonne de la ligne
                                for (nColonne = 0; nColonne < 9; nColonne++)
                                {
                                    tabGrille[nLigne, nColonne] = int.Parse(sLigne.Substring(nColonne, 1));
                                }

                                // Ligne suivante
                                sLigne = srGrille.ReadLine();
                                nLigne++;
                            }
                        }

                        // Parcours des lignes
                        for (nLigne = 0; nLigne < 9; nLigne++)
                        {
                            // Parcours des colonnes
                            for (nColonne = 0; nColonne < 9; nColonne++)
                            {
                                // Recherche du champs TextBox correspondant à la cellule
                                TextBox txtRecherche = this.Controls.Find("txtCell_" + nLigne + "_" + nColonne, true).FirstOrDefault() as TextBox;
                                if (txtRecherche != null)
                                {
                                    // Si un chiffre est saisie dans la grille récupérée
                                    if (tabGrille[nLigne, nColonne] != 0)
                                    {
                                        // Affichage du chiffre
                                        txtRecherche.Text = tabGrille[nLigne, nColonne].ToString();

                                        // On met le champs en lecture seule
                                        txtRecherche.ReadOnly = true;
                                    }
                                    else
                                    {
                                        // On vide le contenu du champs
                                        txtRecherche.Text = "";

                                        // On met le champs en écriture
                                        txtRecherche.ReadOnly = false;
                                    }
                                }
                            }
                        }

                        // Création de la grille d'origine
                        m_oResolver = new Resolver();
                        m_oResolver.SetGrille(tabGrille);
                    }
                    finally
                    {
                        // En cas d'erreur, on ferme le fichier s'il est ouvert
                        if (srGrille != null) srGrille.Close();
                    }
                }
            }
        }

        private void btnResoudre_Click(object sender, EventArgs e)
        {
            if (m_oResolver.bResoudreGrille())
            {
                // Solution trouvée, on l'affichage pour l'utilisateur
                for (int nLigne = 0; nLigne < 9; nLigne++)
                {
                    for (int nColonne = 0; nColonne < 9; nColonne++)
                    {
                        TextBox txtRecherche = this.Controls.Find("txtCell_" + nLigne + "_" + nColonne, true).FirstOrDefault() as TextBox;
                        if (txtRecherche != null)
                        {
                            txtRecherche.Text = m_oResolver.GetSolutionCellule(nLigne, nColonne).ToString();
                        }
                    }
                }

                // On alerte l'utilisateur que c'est terminé
                System.Windows.Forms.MessageBox.Show("Résolution de la grille terminée", "Terminée", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Aucune solution possible, on affichage un message d'erreur
                System.Windows.Forms.MessageBox.Show("Impossible de résoudre la grille de sudoku", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
