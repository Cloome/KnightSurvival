using IUTGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;


namespace KnightSurvival
{
    /// <summary>
    /// Défini le visuel, l'IHM, de la vie
    /// </summary>
    public class IHMVie : GameItemIHM, IAnimable
    {

        #region attributs et propriétés
        public override string TypeName { get => "vie"; }
        private TextBlock vieIHM;
        private Joueur joueur;
        private GameItemIHM heart;

        public TextBlock VieIHM { get { return vieIHM; } set { vieIHM = value; } }

        public GameItemIHM Heart { get => heart; set => heart = value; }
        #endregion


        #region constructeur
        /// <summary>
        /// Constructeur d'IHMVie
        /// </summary>
        /// <author> Morgane VIALA et Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="g">jeu</param>
        /// <param name="c">canva</param>
        /// <param name="joueur">joueur</param>
        public IHMVie(double x, double y, Game g, Canvas c, Joueur joueur) : base(x, y, c, g)
        {
            Heart = new GameItemIHM(x, y, c, g, "HeartLife.png");
            this.joueur = joueur;
            vieIHM = new TextBlock();
            vieIHM.Text = this.joueur.PV.ToString();
            vieIHM.FontSize = 32;

            vieIHM.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            vieIHM.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Canvas.SetLeft(vieIHM, x + 42);
            Canvas.SetTop(vieIHM, y);

            c.Children.Add(vieIHM);
        }

        #endregion

        #region méthodes

        /// <summary>
        /// Collide effect ne fait rien, la barre de vie n'est pas un objet sur lequel on peut interagir
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name = "other" > Objet avec qui la collision a eu lieu</param>
        public override void CollideEffect(GameItem other)
        {
        }

        /// <summary>
        /// Animation du texte de la vie
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="dt"></param>
        public void Animate(TimeSpan dt)
        {
            vieIHM.Text = this.joueur.PV.ToString();
        }

        #endregion
    }
}
