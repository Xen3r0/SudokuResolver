using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Resolver
    {
        int[,] m_tabGrille = new int[9, 9];
        int[,] m_tabSolution = new int[9, 9];

        public Resolver()
        {
            
        }

        /// <summary>
        /// Définir la grille d'origine à résoudre
        /// </summary>
        /// <param name=tabGrille>Tableau à deux dimensions contenant la grille d'origine</param>
        public void SetGrille(int[,] tabGrille)
        {
            m_tabGrille = tabGrille;
        }

        /// <summary>
        /// Définir le chiffre d'une cellule de la grille d'origine
        /// </summary>
        /// <param name=nLigne>Ligne concernée</param>
        /// <param name=nColonne>Colonne concernée</param>
        /// <param name=nValeur>Chiffre concernée</param>
        public void SetGrilleCellule(int nLigne, int nColonne, int nValeur)
        {
            m_tabGrille[nLigne, nColonne] = nValeur;
        }

        /// <summary>
        /// Récupérer la grille d'origine
        /// </summary>
        /// <returns>Tableau à deux dimensions contenant la grille d'origine</returns>
        public int[,] GetGrille()
        {
            return m_tabGrille;
        }

        /// <summary>
        /// Récupérer le chiffre d'une cellule de la grille d'origine
        /// </summary>
        /// <returns>Chiffre de la cellule</returns>
        public int GetGrilleCellule(int nLigne, int nColonne)
        {
            return m_tabGrille[nLigne, nColonne];
        }

        /// <summary>
        /// Récupérer la grille résolue
        /// </summary>
        /// <returns>Tableau à deux dimensions contenant la grille résolue</returns>
        public int[,] GetSolution()
        {
            return m_tabSolution;
        }

        /// <summary>
        /// Définir le chiffre d'une cellule de la grille d'origine
        /// </summary>
        /// <param name=nLigne>Ligne concernée</param>
        /// <param name=nColonne>Colonne concernée</param>
        /// <param name=nValeur>Chiffre concernée</param>
        public int GetSolutionCellule(int nLigne, int nColonne)
        {
            return m_tabSolution[nLigne, nColonne];
        }

        /// <summary>
        /// Lancer la résolution de la grille
        /// </summary>
        /// <returns>Vrai si la grille est résolue, faux si on ne peut pas résoudre la grille</returns>
        public bool bResoudreGrille()
        {
            m_tabSolution = m_tabGrille;
            return bResoudreCellule();
        }

        /// <summary>
        /// Rechercher le chiffre possible à une cellule
        /// </summary>
        /// <param name=nLigne>Numéro de la ligne</param>
        /// <param name=nColonne>Numéro de la colonne</param>
        /// <returns>Vrai si un chiffre est trouvé, faux si aucun chiffre n'a été trouvé</returns>
        public bool bResoudreCellule(int nLigne = 0, int nColonne = 0)
        {
            if (nColonne >= 9)
            {
                // On a terminée la résolution de la grille
                return true;
            }
            else if (nLigne >= 9)
            {
                // On a fait toute les lignes, on repart de la première mais on décale d'une colonne à droite
                return bResoudreCellule(0, (nColonne + 1));
            }
            else if (m_tabSolution[nLigne, nColonne] != 0)
            {
                // Un chiffre est déjà présent, on test la cohérence
                if (bChiffrePossible(nLigne, nColonne, m_tabSolution[nLigne, nColonne]))
                {
                    // Chiffre possible dans la ligne, colonne et zone, on décale d'une ligne vers le bas
                    return bResoudreCellule((nLigne + 1), nColonne);
                }

               return false;
            }

            // On test pour tous les chiffres de 1 à 9 pour savoir les possibilités
            for (int nChiffre = 1; nChiffre < 10; nChiffre++)
            {
                // Si le chiffre est possible, on test le reste avec celui-ci, sinon on change de chiffre
                if (bChiffrePossible(nLigne, nColonne, nChiffre))
                {
                    // Chiffre possible, on continue la résolution
                    m_tabSolution[nLigne, nColonne] = nChiffre;
                   
                    if (bResoudreCellule((nLigne + 1), nColonne))
                    {
                        // Chiffre cohérent avec le reste
                        return true;
                    }
                }
            }

            // Pas de solution pour cette cellule, on remonte dans le brute force
            m_tabSolution[nLigne, nColonne] = 0;

            return false;
        }

        /// <summary>
        /// Savoir si un chiffre est possible en testant dans la ligne, colonne et la zone en cours
        /// </summary>
        /// <param name=nLigne>Numéro de la ligne</param>
        /// <param name=nColonne>Numéro de la colonne</param>
        /// <param name=nChiffre>Chiffre</param>
        /// <returns>Vrai si un chiffre est possible, faux si c'est pas possible</returns>
        public bool bChiffrePossible(int nLigne, int nColonne, int nChiffre = 1)
        {
            // Vérification sur la même ligne
            if (bPresentLigne(nLigne, nColonne, nChiffre)) return false;

            // Vérification sur la même colonne
            if (bPresentColonne(nLigne, nColonne, nChiffre)) return false;

            // Vérification dans le même carré (3x3)
            if (bPresentBloc(nLigne, nColonne, nChiffre)) return false;

            return true;
        }

        /// <summary>
        /// Savoir si un chiffre est possible sur une ligne
        /// </summary>
        /// <param name=nLigne>Numéro de la ligne</param>
        /// <param name=nColonne>Numéro de la colonne</param>
        /// <param name=nChiffre>Chiffre</param>
        /// <returns>Vrai si un chiffre est possible, faux si c'est pas possible</returns>
        public bool bPresentLigne(int nLigne, int nColonne, int nChiffre)
        {
            int nPosition;
           
            for (nPosition = 0; nPosition < 9; nPosition++)
            {
                if (nPosition != nColonne && m_tabSolution[nLigne, nPosition] == nChiffre)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Savoir si un chiffre est possible sur une colonne
        /// </summary>
        /// <param name=nLigne>Numéro de la ligne</param>
        /// <param name=nColonne>Numéro de la colonne</param>
        /// <param name=nChiffre>Chiffre</param>
        /// <returns>Vrai si un chiffre est possible, faux si c'est pas possible</returns>
        public bool bPresentColonne(int nLigne, int nColonne, int nChiffre)
        {
            int nPosition;

            for (nPosition = 0; nPosition < 9; nPosition++)
            {
                if (nPosition != nLigne && m_tabSolution[nPosition, nColonne] == nChiffre)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Savoir si un chiffre est possible sur une zone (3x3)
        /// </summary>
        /// <param name=nLigne>Numéro de la ligne</param>
        /// <param name=nColonne>Numéro de la colonne</param>
        /// <param name=nChiffre>Chiffre</param>
        /// <returns>Vrai si un chiffre est possible, faux si c'est pas possible</returns>
        public bool bPresentBloc(int nLigne, int nColonne, int nChiffre)
        {
            // Variables
            int nPosLigne;
            int nPosColonne;
            int nLigneMin = 0;
            int nLigneMax = 0;
            int nColonneMin = 0;
            int nColonneMax = 0;
            double dLigne = (nLigne / 3);
            double dColonne = (nColonne / 3);

            // Détermination de la zone 3x3
            nLigneMin = Convert.ToInt32(System.Math.Floor(dLigne)) * 3;
            nLigneMax = (nLigneMin + 3);

            nColonneMin = Convert.ToInt32(System.Math.Floor(dColonne)) * 3;
            nColonneMax = (nColonneMin + 3);

            // Parcours des lignes de la zone
            for (nPosLigne = nLigneMin; nPosLigne < nLigneMax; nPosLigne++)
            {
                // Parcours des colonnes de la zone
                for (nPosColonne = nColonneMin; nPosColonne < nColonneMax; nPosColonne++)
                {
                    if (nPosLigne != nLigne && nPosColonne != nColonne && m_tabSolution[nPosLigne, nPosColonne] == nChiffre)
                    {
                        // Le chiffre est déjà présent
                        return true;
                    }
                }
            }

            // Le chiffre n'a pas été trouvé dans la zone
            return false;
        }
    }
}
