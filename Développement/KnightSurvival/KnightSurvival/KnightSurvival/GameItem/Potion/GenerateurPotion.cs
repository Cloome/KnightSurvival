using System;
using System.Windows.Controls;
using IUTGame;
namespace KnightSurvival
{
    /// <summary>
    /// Classe qui génère les potions dans le jeu
    /// </summary>
    public class GenerateurPotion : GameItem , IAnimable 
    {
        #region attributs et propriétés
        public override string TypeName { get => "generateurPotions"; } 

        private Canvas canvas; 
        private Double tempsDepuisGeneration = 0; //Temps qui s'écoule depuis la génération de la dernière potion.
        private const Double tempsEntreGeneration = 20000; //Temps qui s'écoule entre la génération de deux potions.
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur du générateur
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="c">Canvas du jeu</param>
        /// <param name="g">Jeu</param>
        /// <param name="name">Nom du sprite (inexistant)</param>
        public GenerateurPotion(Canvas c, Game g, String name = "") : base(0, 0, c, g, name)
        {
            //this.cooldown = false;
            canvas = c;
        }

        #endregion

        #region méthodes
        /// <summary>
        /// Anime l'apparition du générateur
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="dt">temps entre chaque frame</param>
        public void Animate(TimeSpan dt)
        {
            tempsDepuisGeneration = tempsDepuisGeneration + dt.TotalSeconds; //on ajoute le temps passé depuis la dernière frame
            Random r = new Random();
            double tempsAlea = r.NextDouble() * tempsEntreGeneration;
            if (tempsDepuisGeneration > tempsAlea)
            {
                
                double x = r.NextDouble() * GameWidth; //coordonnée x aléatoire sur toute la largeur du jeu
                double y = r.NextDouble() * GameHeight / 2; //coordonnée y aléatoire sur la moitié de la hauteur du jeu

                Potion p = new Potion(x, y, canvas, Game); //création de la potion selon x et y
                Game.AddItem(p);
                //double ms = r.NextDouble() * 10000 + 2000; //entre 0 et 10+2sec
                tempsDepuisGeneration = 0; //Une potion ayant été générée, on réinitialise le temps depuis la génération.
            }
        }

        /// <summary>
        /// Indique ce qu'il faut faire en cas de collision
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="other">Ce qui rentre en collision avec le générateur</param>
        public override void CollideEffect(GameItem other) //Ne doit rien faire
        {
        }
        #endregion

    }
}
