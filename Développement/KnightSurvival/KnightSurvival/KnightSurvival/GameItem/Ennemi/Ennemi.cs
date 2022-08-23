using System;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe ennemi abstraite : on ne peut pas créer juste un ennemi
    /// </summary>
    abstract public class Ennemi : GameItemPhysique, IAnimable
    {
        #region propriétés et attributs
        public override string TypeName { get { return "ennemi"; } }
        private char charPosJoueur; // position du joueur selon un caractère
        private bool forceAppliqueeGauche =false; // indique si la force a déjà été appliquée ou non
        private bool forceAppliqueeDroite =false; // indique si la force a déjà été appliquée ou non
        private double force; // force de l'ennemi en axe X (horizontale)
        private Joueur joueur; // joueur actuel
        private int pV;
        private Canvas canvas;
        private Game game;
        private bool alive = true;
        private bool visible = true;
        private int compteurFrameMort = 0;
        private GameItemSprite sprite;
        private int animationState = 0;

        public int CompteurFrameMort { get { return compteurFrameMort; } set { compteurFrameMort = value; } }

        public bool Visible { get { return visible; } set { visible = value; } }

        public bool Alive { get { return alive; } }

        public int PV { get { return pV; } set { pV = value; } } // met les pv de l'ennemi a 200

        public GameItemSprite Sprite { get => sprite; set => sprite = value; }
        public int AnimationState { get => animationState; set => animationState = value; }

        public abstract int GainXP { get; } 

        #endregion



        #region constructeur
        /// <summary>
        /// Constructeur d'ennemi qui appelle le constructeur gameItemPhysique seulement
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="x">coordonné x</param>
        /// <param name="y">coordonné y</param>
        /// <param name="c">canvas</param>
        /// <param name="g">jeu</param>
        /// <param name="name">nom du fichier contenant le sprite</param>
        /// <param name="joueur"> joueur actuel</param>
        public Ennemi(double x, double y, Canvas c, Game g, Joueur joueur, double force, String name = "") : base(x, y, c, g, name) //Appelle le constructeur mère
        {
            this.joueur = joueur;
            this.force = force;
            this.canvas = c;
            this.game = g;
        }

        #endregion

        #region methodes
        /// <summary>
        /// Collision avec un autre gameitem 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="other">autre game item</param>
        public override void CollideEffect(GameItem other)
        {
            base.CollideEffect(other);
            PrendDegat(other);

        }

        /// <summary>
        /// Recoit les dégats du joueur
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="other">autre gameitem qui touche l'ennemi</param>
        public void PrendDegat(GameItem other)
        {
            if (other.TypeName == "attaque" ) // si attaquer par joueur + pas encore été attaqué par celle ci perd vie
            {
                if (other is Attaque)
                {
                    if (!((Attaque)other).ennemisDejaTouche.Contains(this))
                    {
                        PV -= ((Attaque)other).Degats;
                        
                        IHMDegats iHMdegats = new IHMDegats((this.Left+(this.Width/2)), this.Top-25, canvas, this.Game, ((Attaque)other).Degats); //se met au dessus de l'ennemi et affiche les dégâts reçus
                        Game.AddItem(iHMdegats);
                        
                        ((Attaque)other).ennemisDejaTouche.Add(this);
                        if(
                            ((Attaque)other).Sprite == "JoueurAnimationAttackGauche_1.png"
                            || ((Attaque)other).Sprite == "JoueurAnimationAttackGauche_2.png"
                            || ((Attaque)other).Sprite == "JoueurAnimationAttackGauche_3.png"
                            )
                        {
                            VitesseAxeX -= joueur.KnockBack;
                        }
                        else if (
                            ((Attaque)other).Sprite == "JoueurAnimationAttackDroite_1.png"
                            || ((Attaque)other).Sprite == "JoueurAnimationAttackDroite_2.png"
                            || ((Attaque)other).Sprite == "JoueurAnimationAttackDroite_3.png"
                            )
                        {
                            VitesseAxeX += joueur.KnockBack;
                        }
                    }
                }


                if (PV <= 0)
                {
                    if (Alive)
                    {
                        joueur.Experience = GainXP;
                    }
                    alive = false;
                    this.Collidable = false;
                    GenerateurEnnemi.listEnnemiCrawlerPresent.Remove((EnnemiCrawler1)this);
                    
                    

                }
            }

         }


        /// <summary>
        /// Anime l'ennemi : le fait bouger et supprime si hors cadre
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX</author>
        /// <param name="dt">temps depuis derniere frame</param>
        public override void Animate(TimeSpan dt)
        {
            base.Animate(dt);
            if (alive)
            {
                MoveVersJoueur();
            }
            else
            {
                if (ForceXY[0] < 0)
                {
                    RemoveForce(-force, 0);
                }
                if (ForceXY[0] > 0)
                {
                    RemoveForce(force, 0);
                }
            }
        }


        /// <summary>
        /// Fait bouger l'ennemi vers le joueur 
        /// <author> Clotilde MALO et Félix ARNOUX </author>
        /// </summary>
        protected void MoveVersJoueur()
        {
            // Donne l'indication d'où est le joueur par rapport à l'ennemi selon la position de millieu X du joueur
            charPosJoueur = CharPosJoueur();

            // Fait bouger en direction de X
            if (charPosJoueur == 'G' && forceAppliqueeGauche == false)
            {
                AddForce(-force, 0);
                forceAppliqueeGauche = true;
                if (forceAppliqueeDroite)
                {
                    RemoveForce(force, 0);
                    forceAppliqueeDroite = false;
                }
                 
            }
            else if (charPosJoueur == 'D' && forceAppliqueeDroite == false)
            {
                AddForce(force, 0);
                forceAppliqueeDroite = true;
                if (forceAppliqueeGauche)
                {
                    RemoveForce(-force, 0);
                    forceAppliqueeGauche = false;
                }
                
            }
           
        }


        /// <summary>
        /// Attribue un caractère selon la position du joueur par rapport à l'ennemi 
        /// </summary>
        /// <author> Clotilde MALO et Félix ARNOUX</author>
        /// <returns>position du joueur par rapport à l'ennemi en X (G ou D, M si même)</returns>
        protected char CharPosJoueur()
        {
            double middlePosXJoueur = joueur.Left + (joueur.Width / 2); //position du joueur milieu en X
            double middlePosXEnnemi = this.Left + (this.Width / 2); // position de l'ennemi milieu en X
            double positionRelative = middlePosXEnnemi - middlePosXJoueur; // Position joueur en fonction de this ennemi
            char posJoueur;
            if (positionRelative > 0)
            {
                posJoueur = 'G'; // indique que le joueur est à gauche de l'ennemi
            }
            else if (positionRelative < 0)
            {
                posJoueur = 'D'; // indique que le joueur est à droite de l'ennemi
            }
            else
            {
                posJoueur = 'M'; // indique que le joueur est au même emplacement que l'ennemi
            }
            return posJoueur;
        }



        #endregion
    }
}