using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe qui gère les attaques
    /// </summary>
    public class Attaque : GameItem, IAnimable
    {
        #region attributs et proprietes
        public override string TypeName { get { return "attaque"; } }
        private int degats;
        private Joueur joueur;
        public int Degats { get { return degats; } }
        private double tempsVie = 0;
        private double tempsAnim = 0;
        private string orientation;
        private string sprite;
        private double[] positionJoueurFramePrecedente =new double[2];

        public string Sprite { get { return sprite; } }

        public List<Ennemi> ennemisDejaTouche = new List<Ennemi>();

        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'une nouvelle attaque
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="degats">Montant des dégats qu'infligera cette attaque</param>
        /// <param name="positionX">position du cote gauche du joueur pour le point de reference pour l'attaque</param>
        /// <param name="joueur">instance qui a lancer cette attaque</param>
        /// <param name="c">canvas dans lequel évolue l'objet</param>
        /// <param name="g">this gérer par le game g</param>
        public Attaque(int degats, double positionX, Joueur joueur, Canvas c, Game g):base(positionX,joueur.Top, c, g) 
        {
            this.SpriteOpacity = 0;
            this.degats = degats;
            this.joueur = joueur;
            joueur.Attacking = true;
            this.orientation = joueur.OrientationItem();
            if (orientation == "Gauche") { sprite = "JoueurAnimationAttackGauche_1.png"; }
            else { sprite = "JoueurAnimationAttackDroite_1.png"; }
            this.ChangeSprite(sprite);
            joueur.AnimationAttackState = 0;
            positionJoueurFramePrecedente[0] = 0;
            positionJoueurFramePrecedente[1] = 0;
            this.Top = joueur.Top - 16;
            if (orientation == "Gauche")
            {
                this.Left = joueur.Left - 64;
            }
            else
            {
                this.Left = joueur.Right - 18;
            }
        }

        #endregion

        #region méthodes
        /// <summary>
        /// Anime selon le temps
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="dt"></param>
        public void Animate(TimeSpan dt)
        {
            
            tempsVie += dt.TotalSeconds;
            tempsAnim += dt.TotalSeconds;
            if(tempsAnim > joueur.AttackSpeed/3)
            {
                if (sprite == "JoueurAnimationAttackGauche_2.png")
                {
                    sprite = "JoueurAnimationAttackGauche_3.png";
                }
                if (sprite == "JoueurAnimationAttackGauche_1.png")
                {
                    sprite = "JoueurAnimationAttackGauche_2.png";
                }

                if (sprite == "JoueurAnimationAttackDroite_2.png")
                {
                    sprite = "JoueurAnimationAttackDroite_3.png";
                }
                if (sprite == "JoueurAnimationAttackDroite_1.png")
                {
                    sprite = "JoueurAnimationAttackDroite_2.png";
                }
                
                this.ChangeSprite(sprite);
                joueur.AnimationAttackState++;
                tempsAnim = 0;
            }
            this.SetPosition();
            if (tempsVie > joueur.AttackSpeed)
            {                
                joueur.Attacking = false;
                Game.RemoveItem(this);
                this.Dispose();
            }
            if(this.SpriteOpacity == 0)
            {
                this.SpriteOpacity = 255;
            }
        }

        /// <summary>
        /// Replace cet objet attaque en fonction de la position du joueur
        /// </summary>
        /// <author> Félix ARNOUX </author>
        private void SetPosition()
        {
            if (positionJoueurFramePrecedente[0] != joueur.Left || positionJoueurFramePrecedente[1] != joueur.Top)
            {
                this.Top = joueur.Top - 16;
                if (orientation == "Gauche")
                {
                    this.Left = joueur.Left - 64;
                }
                else
                {
                    this.Left = joueur.Right - 18;
                }
                positionJoueurFramePrecedente[0] = joueur.Left;
                positionJoueurFramePrecedente[1] = joueur.Top;
            }
        }

        /// <summary>
        /// Effet de collision : ici rien
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="other"> objet avec qui il entre en collision</param>
        public override void CollideEffect(GameItem other)
        {
            
        }

        #endregion
    }
}
