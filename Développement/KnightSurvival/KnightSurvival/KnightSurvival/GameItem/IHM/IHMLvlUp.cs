using IUTGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant le visuel, l'IHM, de l'augmentation de niveau
    /// </summary>
    public class IHMLvlUp : GameItemIHM , IAnimable
    {
        #region attributs et propriétés
        private TextBlock lvlUp; // niveau vers lequel on augmente
        private TextBlock statUp; // statistique donné
        private double temps =0;
        private double yLvlUp;
        private double yStatUp;
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'IHMLvlUp
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        /// <param name="joueur">joueur</param>
        /// <param name="moduloLvl">modulo du niveau</param>
        public IHMLvlUp(Canvas c, Game g, Joueur joueur, int moduloLvl) : base((joueur.Left - (joueur.Width / 2)), joueur.Top - 25, c, g)
        {
            
            lvlUp = new TextBlock();                   
            lvlUp.TextAlignment = System.Windows.TextAlignment.Center;
            lvlUp.Text = "+ LEVEL  UP +";
            lvlUp.FontSize = 48;
            lvlUp.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));            
            yLvlUp = joueur.Top - 45;
            Canvas.SetLeft(lvlUp, (joueur.Left - 70 ));
            Canvas.SetTop(lvlUp, yLvlUp);
            c.Children.Add(lvlUp);


            statUp = new TextBlock();
            statUp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            statUp.Text = TextLvlUp(moduloLvl);
            statUp.FontSize = 32;
            statUp.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            yStatUp = joueur.Top;
            Canvas.SetLeft(statUp, (joueur.Left - 10));
            Canvas.SetTop(statUp, yStatUp);
            c.Children.Add(statUp);
            g.AddItem(this);
        }
        #endregion

        #region methodes
        /// <summary>
        /// Texte à afficher pour le level up
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="moduloLvl">modulo niveau</param>
        /// <returns></returns>
        private string TextLvlUp(int moduloLvl)
        {
            string textLvlUp = "";

            switch (moduloLvl)
            {
                case 1: textLvlUp = "PV Max +"; break;
                case 2: textLvlUp = "   Damage +\nKnockBack +"; break;
                case 0: textLvlUp = "Mobility +\nAttack Speed +"; break;
            }
            return textLvlUp;
        }

        /// <summary>
        /// Affiche le niveau et les stats pendant un certains temps
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="dt"> temps du jeu </param>
        public void Animate(TimeSpan dt)
        {
            temps += dt.TotalMilliseconds;
            if (temps > 1600)
            {
                this.lvlUp.Text = " ";
                this.statUp.Text = " ";
                this.Game.RemoveItem(this);
                this.Dispose();
            }

            this.lvlUp.Opacity = 1 - (1 / (1600 / temps));
            this.statUp.Opacity = 1 - (1 / (1600 / temps));
            
            yLvlUp-=2;
            yStatUp-=2;           
            Canvas.SetTop(lvlUp, yLvlUp);
            Canvas.SetTop(statUp, yStatUp);
        }

        #endregion

    }
}
