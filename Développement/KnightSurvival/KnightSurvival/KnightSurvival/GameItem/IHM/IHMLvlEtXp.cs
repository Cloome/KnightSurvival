using IUTGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant le visuel, l'IHM, des niveaux et expériences
    /// </summary>
    public class IHMLvlEtXp : GameItemIHM, IAnimable
    {
        #region attributs et propriétés
        public override string TypeName { get => "lvl&xp"; }
        private TextBlock lvl;
        private TextBlock xp;
        private Joueur joueur;

        public TextBlock Lvl { get => lvl; set => lvl = value; }
        public TextBlock Xp { get => xp; set => xp = value; }
        #endregion

        #region constructeur
        /// <summary>
        /// Construit un élément d'IHMLVLetXP
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        /// <param name="joueur">joueur</param>
        public IHMLvlEtXp(double x, double y, Canvas c, Game g, Joueur joueur):base(x,y,c,g)
        {
            this.joueur = joueur;

            lvl = new TextBlock();
            lvl.Text = String.Format("Lvl : {0}", 1);
            lvl.FontSize = 32;
            lvl.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Canvas.SetLeft(lvl, x);
            Canvas.SetTop(lvl, y);
            c.Children.Add(lvl);


            xp = new TextBlock();
            xp.Text = String.Format("Xp : {0}/{1}",0,10);
            xp.FontSize = 32;
            xp.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Canvas.SetLeft(xp, x);
            Canvas.SetTop(xp, y+40);
            c.Children.Add(xp);
        }
        #endregion

        #region méthodes
        /// <summary>
        /// Affiche le niveau et xp
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="dt"> temps du jeu</param>
        public void Animate(TimeSpan dt)
        {
            lvl.Text = String.Format("Lvl : {0}", joueur.Niveau);
            xp.Text = String.Format("Xp : {0}/{1}", joueur.Experience, 10 * Math.Pow(2, joueur.Niveau - 1));
        }

        #endregion


    }
}
