using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Personnage controlé par l'utilisateur
    /// </summary>
    public class Joueur : GameItemPhysique, IAnimable, IKeyboardInteract
    {
        #region attributs et propriétés
        private double periodeInvincible = 1;  //en travaux
        // private int degatAttaque;            //en travaux


        public override string TypeName { get { return "joueur"; } }
        private int pVMax;
        private int pV;
        private double forceSaut = -90000; // force du saut du joueur
        private double force = 6000; // force horizontale du joueur
        private int animationState = 0;
        private double periodeAnimation = 0;
        private bool attacking = false;
        private int animationAttackState = 0;
        private string animationName = "JoueurAnimationStandDroite_1.png";
        private Canvas c;
        private Game g;
        private Random random;
        private double bonusDegats= 1.0;
        private GameItemSprite sprite;
        private int experience = 0;
        private int niveau = 1;
        private int knockBack = 1500;
        private double attackSpeed = 0.3;
        public int Experience
        {
            get { return experience; }

            set
            {
                if (experience + value < 10 * Math.Pow(2, niveau - 1))
                {
                    experience += value;
                }
                else
                {
                    experience = value - (int)(10 * Math.Pow(2, niveau - 1) - experience);
                    LvlUp();
                    niveau++;
                }
            }
        }

        public int Niveau { get { return niveau; } }



        private Dictionary<Key, bool> toucheActive = new Dictionary<Key, bool>(); // liste des touches utilisé par le joueur avec leur état vrai ou faux, utile pour le saut

        public double Degats
        {
            get
            {
                return Math.Round((60 + (random.Next(0, 41))) * bonusDegats);
            }
        }
        public int PVMax { get { return pVMax; } }
        public int PV { get { return pV; } }
        public bool Attacking { get { return attacking; } set { attacking = value; } }
        public int AnimationAttackState { get { return animationAttackState; } set { animationAttackState = value; } }

        public int KnockBack { get => knockBack; set => knockBack = value; }
        public double AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur du joueur 
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="c">Canvas dans lequel le joueur évolue</param>
        /// <param name="g">Game qui doit gerer le joueur</param>
        public Joueur(Canvas c, Game g) : base(100, 300, c, g, "JoueurAnimationStandDroite_1.png")
        {
            sprite = new GameItemSprite(100, 300, c, g, "JoueurAnimationStandDroite_1.png");
            random = new Random();
            this.c = c;
            this.g = g;
            pVMax = 100;
            pV = 100;
            SpriteOpacity = 0;

        }
        #endregion

        #region méthodes
        


        /// <summary>
        /// Effet de collision 
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="other">objet avec qui il entre en collision</param>
        public override void CollideEffect(GameItem other)
        {
            base.CollideEffect(other);
            if (other.TypeName == "potion")
            {
                RestaurePV();
            }
            if (other.TypeName == "ennemi")
            {
                PrendDegats();
            }
        }

        /// <summary>
        /// Remet les pv du joueur au maximum
        /// </summary>
        /// <author> Félix ARNOUX</author>
        public void RestaurePV()
        {
            pV += 40;
            if (pV > pVMax)
            {
                pV = pVMax;
            }
        }

        /// <summary>
        /// Joueur perd de la vie
        /// </summary>
        /// <author> Félix ARNOUX</author>
        public void PrendDegats()
        {
            if (periodeInvincible > 0.1)
            {
                periodeInvincible = 0;
                pV -= 2;
                if (pV <= 0)
                {
                    this.Game.Loose();
                }
            }
        }

        /// <summary>
        /// Joueur monte d'un niveau
        /// </summary>
        /// <author> Félix ARNOUX </author>
        private void LvlUp()
        {
            IHMLvlUp iHMLvlUp;
            bool gauche = false;
            bool droite = false;
            switch (niveau % 3)
            {
                case 1: pVMax += 20; pV += 20; iHMLvlUp = new IHMLvlUp(c, g, this, 1); break;
                case 2: bonusDegats += 0.15; KnockBack += 200; pV += 20; iHMLvlUp = new IHMLvlUp(c, g, this, 2); break;
                case 0:
                    if (ForceXY[0] > 0)
                    {
                        droite = true;
                        RemoveForce(force, 0);
                    }
                    else if (ForceXY[0] < 0)
                    {
                        gauche = true;
                        RemoveForce(-force, 0);
                    }
                    force += 600; AttackSpeed *= 0.8; pV += 20; iHMLvlUp = new IHMLvlUp(c, g, this, 0);
                    if (droite)
                    {                        
                        AddForce(force, 0);
                    }
                    else if (gauche)
                    {
                        AddForce(-force, 0);
                    }
                    break;
            }
        }

        /// <summary>
        /// Anime selon le temps
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="dt">Temps écoulé depuis la dernière frame</param>
        public override void Animate(TimeSpan dt)
        {
            #region physique
            base.Animate(dt);
            if (toucheActive.ContainsKey(Key.S))
            {
                if (toucheActive[Key.S])
                {
                    Pose = false;
                    if (ListeDePlateformeRencontrer.Count != 0)
                    {
                        ListeFausseColisionPasse[ListeDePlateformeRencontrer[0]] = true;
                    }
                    toucheActive[Key.S] = false;
                }
            }
            if (toucheActive.ContainsKey(Key.Z))
            {
                if (toucheActive[Key.Z])
                {
                    toucheActive[Key.Z] = false;
                    RemoveForce(0, forceSaut);
                }
            }
            periodeInvincible += dt.TotalSeconds;
            if(this.Left < 0)
            {
                this.Left = 0;
            }
            if(this.Right > 1264)
            {
                this.Right = 1264;
            }
            
            #endregion

            #region animation
            sprite.PublicPutXY(this.Left, this.Top);
            periodeAnimation += dt.TotalSeconds;
            if (periodeAnimation > 0.12 || (LastOrientation != OrientationItem()))
            {
                if (ForceXY[0] > 0)
                {
                    switch (animationState)
                    {
                        case 0: if (animationName != "JoueurAnimationCourseDroite_1.png") { animationName = "JoueurAnimationCourseDroite_1.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 1: if (animationName != "JoueurAnimationCourseDroite_2.png") { animationName = "JoueurAnimationCourseDroite_2.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 2: if (animationName != "JoueurAnimationCourseDroite_3.png") { animationName = "JoueurAnimationCourseDroite_3.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 3: if (animationName != "JoueurAnimationCourseDroite_4.png") { animationName = "JoueurAnimationCourseDroite_4.png"; sprite.PublicChangeSprite(animationName); } break;
                    }
                    periodeAnimation = 0;
                }
                else if (ForceXY[0] < 0)
                {
                    switch (animationState)
                    {
                        case 0: if (animationName != "JoueurAnimationCourseGauche_1.png") { animationName = "JoueurAnimationCourseGauche_1.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 1: if (animationName != "JoueurAnimationCourseGauche_2.png") { animationName = "JoueurAnimationCourseGauche_2.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 2: if (animationName != "JoueurAnimationCourseGauche_3.png") { animationName = "JoueurAnimationCourseGauche_3.png"; sprite.PublicChangeSprite(animationName); } break;
                        case 3: if (animationName != "JoueurAnimationCourseGauche_4.png") { animationName = "JoueurAnimationCourseGauche_4.png"; sprite.PublicChangeSprite(animationName); } break;
                    }
                    periodeAnimation = 0;
                }
                else
                {
                    if (OrientationItem() == "Droite")
                    {
                        switch (animationState)
                        {
                            case 0: case 1: if (animationName != "JoueurAnimationStandDroite_1.png") { animationName = "JoueurAnimationStandDroite_1.png"; sprite.PublicChangeSprite(animationName); } break;
                            case 2: case 3: if (animationName != "JoueurAnimationStandDroite_2.png") { animationName = "JoueurAnimationStandDroite_2.png"; sprite.PublicChangeSprite(animationName); } break;
                        }
                    }
                    else
                    {
                        switch (animationState)
                        {
                            case 0: case 1: if (animationName != "JoueurAnimationStandGauche_1.png") { animationName = "JoueurAnimationStandGauche_1.png"; sprite.PublicChangeSprite(animationName); } break;
                            case 2: case 3: if (animationName != "JoueurAnimationStandGauche_2.png") { animationName = "JoueurAnimationStandGauche_2.png"; sprite.PublicChangeSprite(animationName); } break;
                        }
                    }
                    periodeAnimation = 0;
                }
                if (animationState < 3)
                {
                    animationState++;
                }
                else
                {
                    animationState = 0;
                }
            }


            #endregion

        }

        /// <summary>
        /// Lorsque l'utilisateur appuie sur une touche
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="key">touche du clavier qui a été activé</param>
        public void KeyDown(Key key)
        {
            if (!toucheActive.ContainsKey(key))
            {
                
                switch (key)
                {
                    case Key.Q: AddForce(-force, 0); toucheActive.Add(key, true); break;
                    case Key.D: AddForce(force, 0); toucheActive.Add(key, true); break;
                }
                if (key == Key.Z && Pose)
                {
                    toucheActive.Add(key, true);
                    AddForce(0, forceSaut);
                }
                if(key == Key.S && Pose)
                {
                    toucheActive.Add(key, true);
                }
                if (key == Key.Space && !Attacking)
                {
                    toucheActive.Add(key, true);
                    double positionAttaque;
                    if(OrientationItem() == "Gauche")
                    {
                        positionAttaque = this.Left - 74;
                    }
                    else
                    {
                        positionAttaque = this.Right - 18;
                    }
                    Attaque attaque = new Attaque((int)Degats, positionAttaque, this, c, g);
                    this.Game.AddItem(attaque);
                    toucheActive[key] = false;

                }
            }
            else
            {
                if (!toucheActive[key])
                {
                    
                    switch (key)
                    {
                        case Key.Q: AddForce(-force, 0); toucheActive[key] = true; break;
                        case Key.D: AddForce(force, 0); toucheActive[key] = true; break;
                    }
                    if (key == Key.Z && Pose && !toucheActive[Key.Z])
                    {
                        toucheActive[key] = true;
                        AddForce(0, forceSaut);
                    }
                    if (key == Key.S && Pose)
                    {
                        toucheActive[key] = true;
                    }
                    if (key == Key.Space && !Attacking)
                    {
                        toucheActive[key] = true;
                        double positionAttaque;
                        if (OrientationItem() == "Gauche")
                        {
                            positionAttaque = this.Left - 74;
                        }
                        else
                        {
                            positionAttaque = this.Right - 18;
                        }
                        Attaque attaque = new Attaque((int)Degats, positionAttaque, this, c, g);
                        this.Game.AddItem(attaque);
                        toucheActive[key] = false;
                    }
                }
            }
           
        }

        /// <summary>
        /// Lorsqu'une touche est relaché 
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="key">touche</param>
        public void KeyUp(Key key)
        {
            
            switch (key)
            {
                case Key.Q: RemoveForce(-force, 0); toucheActive[key] = false; break;
                case Key.D: RemoveForce(force, 0); toucheActive[key] = false; break;
            }

        }
        #endregion

    }
}