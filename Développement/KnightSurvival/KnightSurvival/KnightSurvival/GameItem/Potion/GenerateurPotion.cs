using System;
using System.Windows.Controls;
using IUTGame;
namespace KnightSurvival
{
    /// <summary>
    /// Classe qui g�n�re les potions dans le jeu
    /// </summary>
    public class GenerateurPotion : GameItem , IAnimable 
    {
        #region attributs et propri�t�s
        public override string TypeName { get => "generateurPotions"; } 

        private Canvas canvas; 
        private Double tempsDepuisGeneration = 0; //Temps qui s'�coule depuis la g�n�ration de la derni�re potion.
        private const Double tempsEntreGeneration = 20000; //Temps qui s'�coule entre la g�n�ration de deux potions.
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur du g�n�rateur
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

        #region m�thodes
        /// <summary>
        /// Anime l'apparition du g�n�rateur
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="dt">temps entre chaque frame</param>
        public void Animate(TimeSpan dt)
        {
            tempsDepuisGeneration = tempsDepuisGeneration + dt.TotalSeconds; //on ajoute le temps pass� depuis la derni�re frame
            Random r = new Random();
            double tempsAlea = r.NextDouble() * tempsEntreGeneration;
            if (tempsDepuisGeneration > tempsAlea)
            {
                
                double x = r.NextDouble() * GameWidth; //coordonn�e x al�atoire sur toute la largeur du jeu
                double y = r.NextDouble() * GameHeight / 2; //coordonn�e y al�atoire sur la moiti� de la hauteur du jeu

                Potion p = new Potion(x, y, canvas, Game); //cr�ation de la potion selon x et y
                Game.AddItem(p);
                //double ms = r.NextDouble() * 10000 + 2000; //entre 0 et 10+2sec
                tempsDepuisGeneration = 0; //Une potion ayant �t� g�n�r�e, on r�initialise le temps depuis la g�n�ration.
            }
        }

        /// <summary>
        /// Indique ce qu'il faut faire en cas de collision
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="other">Ce qui rentre en collision avec le g�n�rateur</param>
        public override void CollideEffect(GameItem other) //Ne doit rien faire
        {
        }
        #endregion

    }
}
