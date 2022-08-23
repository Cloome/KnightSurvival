using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant le visuel, l'IHM, des dégâts occasionné à l'ennemi
    /// </summary>
    public class IHMDegats : GameItemIHM, IAnimable
    {
        #region attributs et propriétés
        private TextBlock degatsText;
        public override string TypeName { get => "degats"; }
        public static List<IHMDegats> ListIHMDegats { get => listIHMDegats; set => listIHMDegats = value; }
        public TextBlock DegatsText { get => degatsText; set => degatsText = value; }

        private double temps = 0;
        private double x; // axe x
        private double y; // axe y
        private static List<IHMDegats> listIHMDegats = new List<IHMDegats>();
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'IHMDegats
        /// </summary>
        /// <author> Morgane VIALA et Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        /// <param name="degats">dégat occasionné</param>
        public IHMDegats(double x, double y, Canvas c, Game g, int degats) : base(x, y, c, g)
        {
            ListIHMDegats.Add(this);
            Game.AddItem(this);
            this.x = x;
            this.y = y;
            DegatsText = new TextBlock();
            DegatsText.Text = degats.ToString();
            DegatsText.FontSize = 40;
            DegatsText.Foreground = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            DegatsText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Canvas.SetLeft(DegatsText, x);
            Canvas.SetTop(DegatsText, y);
            c.Children.Add(DegatsText);
        }

        #endregion

        #region méthodes

        /// <summary>
        /// Collide effect ne fait rien, l'affichage des dégats n'est pas un objet sur lequel on peut interagir
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="other">Objet avec qui la collision a eu lieu</param>
        public override void CollideEffect(GameItem other)
        {
        }

        /// <summary>
        /// Affiche les dégats que subissent le joueur et les ennemis 
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="dt"></param>
        public void Animate(TimeSpan dt)
        {
            temps += dt.TotalMilliseconds;
            if (temps > 800)
            {
                this.DegatsText.Text = " ";
                this.Game.RemoveItem(this);
                this.Dispose();
            }
            
            this.DegatsText.Opacity = 1-(1/(800/temps));
            
            y--;
            x = x - 1.8*Math.Sin(temps/200);
            Canvas.SetLeft(DegatsText, x);
            Canvas.SetTop(DegatsText, y);
            
        }
        #endregion
    }
}
