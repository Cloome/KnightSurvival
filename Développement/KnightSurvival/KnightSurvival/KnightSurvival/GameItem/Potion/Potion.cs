using System;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant la potion
    /// </summary>
    public class Potion : GameItemPhysique, IAnimable
    {
        #region attributs et propri�t�s
        Double tempsVie = 0;
        public override string TypeName { get { return "potion"; } }
        private string animEnCours;
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur de Potion
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="x">Pour le constructeur m�re</param>
        /// <param name="y">Pour le constructeur m�re</param>
        /// <param name="c">Pour le constructeur m�re, donne le canvas</param>
        /// <param name="g">Pour le constructeur m�re, donne le jeu</param>
        /// <param name="name">Pour le constructeur m�re, donne le nom du sprite � utiliser</param>
        public Potion(double x, double y, Canvas c, Game g, String name = "Potion heal.png") : base(x, y, c, g, name) //Appelle le constructeur m�re
        {
            animEnCours = name;
        }
        #endregion

        #region methodes
        /// <summary>
        /// Si la potion entre en collision avec le joueur, la potion est d�truite. Sinon rien. 
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="other">objet en collision avec la potion</param>
        public override void CollideEffect(GameItem other)
        {
            base.CollideEffect(other); //On appelle le collide effect de la classe m�re
            if (other.TypeName == "joueur")
            {
                this.Game.RemoveItem(this);
                this.Dispose();
            }
        }

        /// <summary>
        /// Affiche le sprite de 0 � 5sec puis clignote jusqu'� 7sec avant de s'autod�truire
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="dt">temps pass� depuis la cr�ation de l'objet</param>
        public override void Animate(TimeSpan dt) 
		{
            double[] position = new double[2] { this.Left, this.Top }; //Pour le change sprite made in felix 
            base.Animate(dt);
            tempsVie += dt.TotalSeconds; //On ajoute le temps pass� depuis la derni�re frame au temps de vie
            //Puis on fait clignoter le sprite de la potion sur sa fin de vie.
            if (((tempsVie >= 5.0 && tempsVie <= 5.4) || (tempsVie >= 5.8 && tempsVie <= 6.2) || (tempsVie >= 6.6 && tempsVie <= 7.0)) && animEnCours == "Potion heal.png")
            {
                this.ChangeSprite("Potion heal transparent.png");
                animEnCours = "Potion heal transparent.png";
            }
            else if (((tempsVie >= 5.4 && tempsVie <= 5.8) || (tempsVie >= 6.2 && tempsVie <= 6.6) || (tempsVie >= 7.0 && tempsVie <= 7.4)) && animEnCours == "Potion heal transparent.png")
            {               
                this.ChangeSprite("Potion heal.png");
                animEnCours = "Potion heal.png";
            }
            else if (tempsVie > 7.4) //Destruction de la potion sur sa fin de vie.
            {
                this.Game.RemoveItem(this);
                this.Dispose();
            }
        }
        #endregion
    }
}
