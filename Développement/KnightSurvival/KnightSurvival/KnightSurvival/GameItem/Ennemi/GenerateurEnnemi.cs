using System;
using System.Collections.Generic;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// G�n�re les ennemis
    /// </summary>
    public class GenerateurEnnemi : GameItem, IAnimable
    {
        #region attributs et propri�t�s
        private Canvas canvas;
        Random rnd = new Random();
        public static List<EnnemiCrawler1> listEnnemiCrawlerPresent = new List<EnnemiCrawler1>(); // variable pour savoir combien d'ennemi actuellement sur la map



        private double tempsLastApparition = 0; // variable pour voir depuis combien de temps il y a eu apparition d'un ennemi sur la map
        private Joueur joueur;


        public override string TypeName { get => "generateurEnnemi"; }



        private double xEnnemi;
        private double yEnnemi;

        public double XEnnemi { get => xEnnemi; set => xEnnemi = value; }
        public double YEnnemi { get => yEnnemi; set => yEnnemi = value; }

        private double chronoJeu = 0; // temps du jeu
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur du g�n�rateur ennemi
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="c">canvas actuel (jeu)</param>
        /// <param name="g">jeu</param>
        /// <param name="j">nom du joueur</param>
        public GenerateurEnnemi(Canvas c, Game g, Joueur j) : base(0,0,c,g)
        {
            listEnnemiCrawlerPresent.Clear();
            canvas = c;
            joueur = j;            
        }
        #endregion

        #region methodes
        /// <summary>
        /// Anime les ennemis
        /// </summary>
        /// <author> Clotilde MALO et F�lix ARNOUX </author>
        /// <param name="dt"> temps entre chaque frame</param>
        public void Animate(TimeSpan dt)
        {
            chronoJeu += dt.TotalSeconds;
            tempsLastApparition +=  dt.TotalSeconds; //on ajoute le temps pass� depuis la derni�re frame
            // recup�re le lieu de pop
            double[] popXY = LieuPopEnnemi();
            double XEnnemi = popXY[0];
            double YEnnemi = popXY[1];

            // Cr�ation d'ennemi s'il n'y a aucun ennemi pr�sent
            if (listEnnemiCrawlerPresent.Count == 0)
            {
                double force = 3500 * (((double)rnd.Next(0, 71) / 100) + 0.55); // force al�atoire � partir d'une constante � appliqu� au crawler1
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(XEnnemi, YEnnemi, canvas, Game, joueur, force);
                Game.AddItem(crawler1);
                listEnnemiCrawlerPresent.Add(crawler1);
                tempsLastApparition = 0;
            }

            
            // Cr�ation d'ennemi quand il y en a d�j�, � partir du nombre max d'ennemi et de la vitesse d'apparition entre chaque ennemi
            else if (listEnnemiCrawlerPresent.Count < PopMax(chronoJeu) && tempsLastApparition >= VitessePop(chronoJeu))
            {
                double force = 3500 * (((double)rnd.Next(0, 71) / 100) + 0.55); // force al�atoire � partir d'une constante � appliqu� au crawler1
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(XEnnemi, YEnnemi, canvas, Game, joueur, force);
                Game.AddItem(crawler1);
                listEnnemiCrawlerPresent.Add(crawler1);
                tempsLastApparition = 0;
                
            }

        }

        /// <summary>
        /// Indique un nombre d'apparition maximum pour les ennemis selon une dur�e
        /// </summary>
        /// <author> Clotilde MALO et F�lix ARNOUX </author>
        /// <param name="chronoJeu"> temps depuis lequel le jeu est lanc� </param>
        /// <returns> nombre d'ennemi maximum </returns>
        public int PopMax(double chronoJeu)
        {
            int popmax = 0;
            popmax = (int)chronoJeu / 10;

            return popmax;
        }

        /// <summary>
        /// Indique la vitesse entre chaque apparition d'ennemis selon la dur�e de la derniere apparition
        /// </summary>
        /// <author> Clotilde MALO et F�lix ARNOUX </author>
        /// <param name="chronoJeu"> temps depuis lequel le jeu est lanc� </param>
        /// <returns> indique le temps entre chaque apparition d'ennemi en secondes </returns>
        public double VitessePop(double chronoJeu)
        {
            return (10 / (1 + chronoJeu / 10)); // plus temps passe plus le temps entre chaque apparition diminue
        }

        /// <summary>
        /// Choisi al�atoirement entre des coordonn�es x et y �tablis
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <returns> tableau de double avec coordonn�es X et Y</returns>
        protected double[] LieuPopEnnemi()
        {
            // Repertorie les coordonn�es choisis
            Random rnd = new Random();

            // Repertorie les coordonn�es choisis
            List<double[]> lieuPossible = new List<double[]>();
            lieuPossible.Add(new double[2] { 650, 500 }); // milieu bas 
            lieuPossible.Add(new double[2] { 0, 500 }); // bas gauche 
            lieuPossible.Add(new double[2] { 1250, 500 }); // bas droite
            lieuPossible.Add(new double[2] { 0, 250 }); // plateforme du cot� gauche
            lieuPossible.Add(new double[2] { 1250, 300 }); // plateforme du c�t� droite
            lieuPossible.Add(new double[2] { 550, 0 }); // plateforme du haut au milieu
            lieuPossible.Add(new double[2] { 800, 0 }); // plateforme du haut � droite
            lieuPossible.Add(new double[2] { 400, 0 }); // plateforme du haut � gauche

            // Genere un index pour regrouper les positions
            int lieuIndex = rnd.Next(lieuPossible.Count);

            double[] popXY = lieuPossible[lieuIndex];
   
            return popXY ;
        }




        /// <summary>
        /// Indique ce qu'ennemi fait quand collision : ici rien car g�rer dans joueur
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="other">item qui rentre en collision avec ennemi</param>
        public override void CollideEffect(GameItem other)
        {
        }
        #endregion

    }
}