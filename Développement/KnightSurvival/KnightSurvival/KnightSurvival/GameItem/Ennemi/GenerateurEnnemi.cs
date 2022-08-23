using System;
using System.Collections.Generic;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Génère les ennemis
    /// </summary>
    public class GenerateurEnnemi : GameItem, IAnimable
    {
        #region attributs et propriétés
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
        /// Constructeur du générateur ennemi
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
        /// <author> Clotilde MALO et Félix ARNOUX </author>
        /// <param name="dt"> temps entre chaque frame</param>
        public void Animate(TimeSpan dt)
        {
            chronoJeu += dt.TotalSeconds;
            tempsLastApparition +=  dt.TotalSeconds; //on ajoute le temps passé depuis la dernière frame
            // recupère le lieu de pop
            double[] popXY = LieuPopEnnemi();
            double XEnnemi = popXY[0];
            double YEnnemi = popXY[1];

            // Création d'ennemi s'il n'y a aucun ennemi présent
            if (listEnnemiCrawlerPresent.Count == 0)
            {
                double force = 3500 * (((double)rnd.Next(0, 71) / 100) + 0.55); // force aléatoire à partir d'une constante à appliqué au crawler1
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(XEnnemi, YEnnemi, canvas, Game, joueur, force);
                Game.AddItem(crawler1);
                listEnnemiCrawlerPresent.Add(crawler1);
                tempsLastApparition = 0;
            }

            
            // Création d'ennemi quand il y en a déjà, à partir du nombre max d'ennemi et de la vitesse d'apparition entre chaque ennemi
            else if (listEnnemiCrawlerPresent.Count < PopMax(chronoJeu) && tempsLastApparition >= VitessePop(chronoJeu))
            {
                double force = 3500 * (((double)rnd.Next(0, 71) / 100) + 0.55); // force aléatoire à partir d'une constante à appliqué au crawler1
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(XEnnemi, YEnnemi, canvas, Game, joueur, force);
                Game.AddItem(crawler1);
                listEnnemiCrawlerPresent.Add(crawler1);
                tempsLastApparition = 0;
                
            }

        }

        /// <summary>
        /// Indique un nombre d'apparition maximum pour les ennemis selon une durée
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX </author>
        /// <param name="chronoJeu"> temps depuis lequel le jeu est lancé </param>
        /// <returns> nombre d'ennemi maximum </returns>
        public int PopMax(double chronoJeu)
        {
            int popmax = 0;
            popmax = (int)chronoJeu / 10;

            return popmax;
        }

        /// <summary>
        /// Indique la vitesse entre chaque apparition d'ennemis selon la durée de la derniere apparition
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX </author>
        /// <param name="chronoJeu"> temps depuis lequel le jeu est lancé </param>
        /// <returns> indique le temps entre chaque apparition d'ennemi en secondes </returns>
        public double VitessePop(double chronoJeu)
        {
            return (10 / (1 + chronoJeu / 10)); // plus temps passe plus le temps entre chaque apparition diminue
        }

        /// <summary>
        /// Choisi aléatoirement entre des coordonnées x et y établis
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <returns> tableau de double avec coordonnées X et Y</returns>
        protected double[] LieuPopEnnemi()
        {
            // Repertorie les coordonnées choisis
            Random rnd = new Random();

            // Repertorie les coordonnées choisis
            List<double[]> lieuPossible = new List<double[]>();
            lieuPossible.Add(new double[2] { 650, 500 }); // milieu bas 
            lieuPossible.Add(new double[2] { 0, 500 }); // bas gauche 
            lieuPossible.Add(new double[2] { 1250, 500 }); // bas droite
            lieuPossible.Add(new double[2] { 0, 250 }); // plateforme du coté gauche
            lieuPossible.Add(new double[2] { 1250, 300 }); // plateforme du côté droite
            lieuPossible.Add(new double[2] { 550, 0 }); // plateforme du haut au milieu
            lieuPossible.Add(new double[2] { 800, 0 }); // plateforme du haut à droite
            lieuPossible.Add(new double[2] { 400, 0 }); // plateforme du haut à gauche

            // Genere un index pour regrouper les positions
            int lieuIndex = rnd.Next(lieuPossible.Count);

            double[] popXY = lieuPossible[lieuIndex];
   
            return popXY ;
        }




        /// <summary>
        /// Indique ce qu'ennemi fait quand collision : ici rien car gérer dans joueur
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="other">item qui rentre en collision avec ennemi</param>
        public override void CollideEffect(GameItem other)
        {
        }
        #endregion

    }
}