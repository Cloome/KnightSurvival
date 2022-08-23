using System;
using System.Windows.Controls;
using System.Collections.Generic;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe abstraite de tout les objet physique 
    /// </summary>
    public abstract class GameItemPhysique : GameItem, IAnimable
    {

        #region attribut et propri�t� pr�sente


        private double[] forceXY = new double[2] { 0, 5000 }; // somme des forces appliqu� � l'objet en axe X [0] puis axe Y [1]
        private double vitesseAxeX = 0 ; // vitesse horizontal de l'objet
        private double vitesseAxeY = 0; // vitesse verticale de l'objet
        private bool pose = false; // �tat de l'objet, pos� ou non sur une plateforme
        private Canvas canvas;
        private string lastOrientation = "Droite";

        public string LastOrientation { get { return lastOrientation; } set { lastOrientation = value; } }

        public bool Pose { get { return pose; } set { pose = value; } }

        public double VitesseAxeY { get { return vitesseAxeY; } set { vitesseAxeY = value; } }

        public double[] ForceXY { get { return forceXY; } }

        public double VitesseAxeX { get { return vitesseAxeX; } set { vitesseAxeX = value; } }

        #endregion

        #region attribut et propri�t� pass�
        private Dictionary<GameItem, bool> listeFausseColisionPasse = new Dictionary<GameItem, bool>(); // Repr�sente si il y avait une fausse collision avec une ou des platform sur la frame pr�cedente
        private List<GameItem> listeDePlateformeRencontrer = new List<GameItem>();  // Repr�sente la liste des plateforme avec lesquelles il y a eu collision � la frame pr�c�dente
        public Dictionary<GameItem, bool> ListeFausseColisionPasse
        {
            get { return listeFausseColisionPasse; }
            set { this.listeFausseColisionPasse = value; }
        }
        public List<GameItem> ListeDePlateformeRencontrer
        {
            get { return listeDePlateformeRencontrer; }
            set { this.listeDePlateformeRencontrer = value; }
        }
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur d'un objet physique
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="x">position horizontale</param>
        /// <param name="y">position verticale</param>
        /// <param name="c">canvas dans lequel �volue l'objet</param>
        /// <param name="g">this g�rer par le game g</param>
        /// <param name="name"></param>
        public GameItemPhysique(double x, double y, Canvas c, Game g, string name) : base(x, y, c, g, name)
        {
            this.canvas = c;
        }
        #endregion

        #region m�thodes
        /// <summary>
        /// calcul de la physique (force, acceleration, vitesse)
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="dt">temps �coul� depuis la pr�c�dente frame</param>
        public void Physique(TimeSpan dt)
        {
            // v�rification � faire quand � l'ordre pour axe x
            vitesseAxeX += forceXY[0] * dt.TotalSeconds - (12* vitesseAxeX) * dt.TotalSeconds; // calcul vitesse horizontale 
            if (forceXY[0] == 0 && vitesseAxeX < 100 && vitesseAxeX > -100) // si aucune force horizontale et vitesse tr�s faible alors arreter l'objet (permet d'�conomiser la puissance de calcul)
            {
                vitesseAxeX = 0;
            }
            if (!pose || forceXY[1] != 5000) // si l'objet n'est pas pos� ou que sa force est diff�rente que celle de la gravit� de base alors calcul vitesse vertical (permet d'�conomiser la puissance de calcul) 
            {
                vitesseAxeY += forceXY[1] * dt.TotalSeconds - (1.5 * vitesseAxeY) * dt.TotalSeconds;
            }
        }

        /// <summary>
        /// Permet d'ajouter une force
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="x">en axe x (horizontal)</param>
        /// <param name="y">en axe y (vertical)</param>
        protected void AddForce(double x, double y)
        {
            forceXY[0] += x;
            forceXY[1] += y;
        }
        /// <summary>
        /// Permet d'enlever une force 
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="x">en axe x (horizontal)</param>
        /// <param name="y">en axe y (vertical)</param>
        protected void RemoveForce(double x, double y)
        {
            forceXY[0] -= x;
            forceXY[1] -= y;
        }

        /// <summary>
        /// Effet de collision
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="other">objet avec lequel entre en collision</param>
        public override void CollideEffect(GameItem other)
        {
            if (other.TypeName == "platform")
            {
                listeDePlateformeRencontrer.Add(other);
                if (!listeFausseColisionPasse.ContainsKey(other))
                {
                    listeFausseColisionPasse.Add(other, false);
                }

                if (vitesseAxeY >= 0)
                {
                    if (
                        (other.Left - this.Right) > (other.Top - this.Bottom)
                        ||
                        (this.Left - other.Right) > (other.Top - this.Bottom)
                        )
                    {
                        listeFausseColisionPasse[other] = true;
                    }
                }

                if (vitesseAxeY >= 0 && !listeFausseColisionPasse[other] && (this.Bottom - other.Top) < other.Height) // Si collision avec plateformes et vitesse me dit que je vais vers bas et tombe et pas de fausse collision sur la frame pr�c�dente
                {
                    pose = true;
                    vitesseAxeY = 0;
                    listeFausseColisionPasse[other] = false;                    
                    PutXY(this.Left, other.Top - this.Height); // permet d'�tre pile sur la plateforme 
                }
                else  // juste contact plateforme
                {
                    listeFausseColisionPasse[other] = true;                    
                }
            }
        }

        /// <summary>
        /// Anime selon le temps
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <param name="dt">temps donn�e</param>
        public virtual void Animate(TimeSpan dt)
        {            
            // DEBUT du traitement de l'information de la frame precedente
            #region traitementFramePrecedente

            if (listeDePlateformeRencontrer.Count > 0) // si l'objet N'A PAS re�u l'information d'une collision avec une plateforme, alors il n'y avait forc�ment pas de fausse colision
            {
                for (int i =0; i < listeDePlateformeRencontrer.Count; i++)
                {
                    GameItem platform = listeDePlateformeRencontrer[i];
                    listeFausseColisionPasse[platform] = false;
                }
                listeDePlateformeRencontrer.RemoveRange(0,listeDePlateformeRencontrer.Count); //r�initialisation pour la prochaine frame
            }
            Physique(dt);
            lastOrientation = OrientationItem();
            #endregion
            // FIN du traitement de la frame precedente

            // DEBUT du traitement de l'information pour la frame en cours
            #region traitementFrameEnCours
            //SetGoodOrientationSprite();
            MoveXY(vitesseAxeX * dt.TotalSeconds, vitesseAxeY * dt.TotalSeconds); // bouge l'objet en fonction de sa vitesse et du temps �coul� depuis la pr�c�dente frame

            pose = false; //r�initialisation pour la prochaine frame

            #endregion
            // FIN du traitement de la frame en cours
        }

        /// <summary>
        /// Donne la bonne orientation droite/ gauche d'un item
        /// </summary>
        /// <author> F�lix ARNOUX </author>
        /// <returns>orientation de l'item : gauche ou droite</returns>
        public string OrientationItem()
        {
            string orientation;
            if (ForceXY[0] > 0)
            {
                orientation = "Droite";
                lastOrientation = orientation;
            }
            else if (ForceXY[0] < 0)
            {
                orientation = "Gauche";
                lastOrientation = orientation;
            }
            else orientation = lastOrientation;
            return orientation;
        }
        #endregion
    }
}
