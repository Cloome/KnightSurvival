using IUTGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant le visuel, l'IHM, du chrono
    /// </summary>
    public class IHMChrono : GameItemIHM, IAnimable
    {

        #region attributs et proprietes
        public override string TypeName { get => "chrono"; }
        private Time time;
        private TextBlock chronoText;
        private DateTime dateTime;
        private TimeSpan timeSpan;
        private double temps = 0;

        public TextBlock ChronoText { get => chronoText; set => chronoText = value; }
        public Time Time { get => time; set => time = value; }

        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'IHMChrono
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        public IHMChrono(double x, double y, Canvas c, Game g) : base(x, y, c, g)
        {
            Time = new Time();
            ChronoText = new TextBlock();
            ChronoText.Text = this.ToString();
            ChronoText.FontSize = 32;
            ChronoText.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            ChronoText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Canvas.SetLeft(ChronoText, x);
            Canvas.SetTop(ChronoText, y);
            c.Children.Add(ChronoText);            
        }

        #endregion

        #region méthodes
        /// <summary>
        /// Collide effect ne fait rien, le temps n'est pas un objet sur lequel on peut interagir
        /// </summary>
        /// <author> Morgane VIALA </author>
        /// <param name="other">Objet avec qui la collision a eu lieu</param>
        public override void CollideEffect(GameItem other)
        {
        }

        /// <summary>
        /// Anime le chrono pour qu'il avance dans le temps
        /// </summary>
        /// <author> Morgane VIALA et Félix ARNOUX </author>
        /// <param name="dt"></param>
        public void Animate(TimeSpan dt)
        {
            DateTime dateTimeNow = DateTime.Now;
            timeSpan = dateTimeNow - dateTime;           
            temps += timeSpan.Milliseconds;           
            Normalize();
            ChronoText.Text = this.ToString();
            dateTime = DateTime.Now;
            
        }

        /// <summary>
        /// Renvoie une string avec le chrono à afficher
        /// </summary>
        /// <author> Morgane VIALA et Félix ARNOUX </author>
        /// <returns>Une chaine de caractères à afficher dans le textbloc chronoText</returns>
        public override string ToString()
        {
            string min = "";
            string sec = ""; 
            string ms = ""; 
            if (Time.Minutes < 10)
            {
                min = "0";
            }
            
            if (Time.Secondes < 10)
            {
                sec = "0";
            }
            
            if (Time.Millisecondes < 10)
            {
                ms = "00";
            }
            else if (Time.Millisecondes < 100)
            {
                ms = "0";
            }
            return String.Format("{0}{1}:{2}{3}.{4}{5}", min, Time.Minutes, sec, Time.Secondes, ms, Time.Millisecondes);
        }

        /// <summary>
        /// Répartit les millisecondes pour donner des minutes, des secondes et des millisecondes bien réparties pour afficher le temps
        /// <author> Morgane VIALA </author>
        /// </summary>
        private void Normalize()
        {

            Time.Millisecondes = (int)temps;
            Time.Secondes = Time.Millisecondes / 1000; //les secondes font 1000millisec
            Time.Millisecondes = Time.Millisecondes % 1000; //on prend le reste de la division pour les millisecondes
            Time.Minutes = Time.Secondes / 60; //les minutes font 60sec
            Time.Secondes %= 60; //on prend le reste de la division pour les secondes
            Time.Minutes %= 60;
        }
        #endregion
    }

}
