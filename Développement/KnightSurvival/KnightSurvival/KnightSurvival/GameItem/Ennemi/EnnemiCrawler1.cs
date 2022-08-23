using System;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Gère les ennemis de type crawler1
    /// </summary>
    public class EnnemiCrawler1 : Ennemi, IAnimable
    {
        #region attributs et propriétés
        public override string TypeName { get { return base.TypeName; } }
        private string animEnCours; // stocke le nom du fichier en cours
        private double tempsDerniereAnim = 0; // depuis combieen de temps il y a eu une animation        
        public override int GainXP { get { return 1; } }

        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'ennemi crawler
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX</author>
        /// <param name="x">coordonné x</param>
        /// <param name="y">coordonné y</param>
        /// <param name="c">canvas</param>
        /// <param name="g">jeu</param>
        /// <param name="name">nom du fichier contenant le sprite</param>
        /// <param name="force"> force horizontale de l'ennemi</param>
        /// <param name="joueur"> joueur actuel </param>
        public EnnemiCrawler1(double x, double y, Canvas c, Game g, Joueur joueur, double force, String name = "Sprite-ennemi-Crawler-1-walk1.png") : base(x, y, c, g, joueur, force, name) //Appelle le constructeur mère
        {
            Sprite = new GameItemSprite(x, y, c, g, "Sprite-ennemi-Crawler-1-walk1.png");
            this.animEnCours = name;
            this.PV = 200;
            SpriteOpacity = 0;
        }
        #endregion

        #region methodes
        /// <summary>
        /// Anime l'ennemi en lui donnant une impression de sautiller/ meurs / marche dans une certaine direction
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX </author>
        /// <param name="dt">temps passé depuis la création de l'objet</param>
        public override void Animate(TimeSpan dt)
        {
            base.Animate(dt);
            Sprite.PublicPutXY(this.Left, this.Top);
            tempsDerniereAnim += dt.TotalSeconds;
            if (Alive && (tempsDerniereAnim >= 0.3 || LastOrientation != OrientationItem()))
            {
                // Faire en sorte que le sprite change 1 fois sur 2 toutes les 3 secondes
                if (ForceXY[0] < 0)
                {
                    switch (AnimationState)
                    {
                        case 0: if (animEnCours != "Sprite-ennemi-Crawler-1-walk1.png") { animEnCours = "Sprite-ennemi-Crawler-1-walk1.png"; Sprite.PublicChangeSprite(animEnCours); } break;
                        case 1: if (animEnCours != "Sprite-ennemi-Crawler-1-walk2.png") { animEnCours = "Sprite-ennemi-Crawler-1-walk2.png"; Sprite.PublicChangeSprite(animEnCours); } break;
                    }
                    tempsDerniereAnim = 0;                    
                }
                if (ForceXY[0] > 0)
                {
                    switch (AnimationState)
                    {
                        case 0: if (animEnCours != "Sprite-ennemi-Crawler-1-walk-Droite1.png") { animEnCours = "Sprite-ennemi-Crawler-1-walk-Droite1.png"; Sprite.PublicChangeSprite(animEnCours); } break;
                        case 1: if (animEnCours != "Sprite-ennemi-Crawler-1-walk2-Droite2.png") { animEnCours = "Sprite-ennemi-Crawler-1-walk-Droite2.png"; Sprite.PublicChangeSprite(animEnCours); } break;
                    }
                    tempsDerniereAnim = 0;                    
                }
                if (AnimationState < 1)
                {
                    AnimationState++;
                }
                else
                {
                    AnimationState = 0;
                }
            }
            else if(!Alive)
            {
                if (animEnCours != "Sprite-ennemi-Crawler-1-mort.png")
                {
                    animEnCours = "Sprite-ennemi-Crawler-1-mort.png";
                    Sprite.PublicChangeSprite("Sprite-ennemi-Crawler-1-mort.png");
                }
                
                if (Visible && tempsDerniereAnim >= 0.06)
                {
                    Sprite.PublicSpriteOpacity(0);
                    this.Visible = false;
                    tempsDerniereAnim = 0;
                    CompteurFrameMort++;
                    if (CompteurFrameMort >= 6)
                    {
                        Sprite.PublicSpriteOpacity(0);
                        Game.RemoveItem(this);
                        this.Dispose();
                    }
                }
                else if (!Visible && tempsDerniereAnim >= 0.06)
                {
                    Sprite.PublicSpriteOpacity(255);
                    this.Visible = true;
                    tempsDerniereAnim = 0;
                }
            }

        }
        #endregion

    }
}