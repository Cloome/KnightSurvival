using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using IUTGame;


namespace KnightSurvival
{
    /// <summary>
    /// Classe gérant les sprites des game item
    /// </summary>
    public class GameItemSprite : GameItem
    {
        #region attributs et propriétés
        public override string TypeName { get { return "sprite"; } }
        #endregion

        #region constructeur
        /// <summary>
        /// Construit le sprite du game item 
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        /// <param name="sprite">sprite de l'item</param>
        public GameItemSprite(double x, double y, Canvas c, Game g, string sprite) : base(x, y, c, g, sprite)
        {
            Collidable = false;
            Game.AddItem(this);
        }
        #endregion

        #region méthodes
        /// <summary>
        /// Effet de collision : rien
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="other">objet avec qui il entre en collision</param>
        public override void CollideEffect(GameItem other)
        {
        }

        /// <summary>
        /// Changement de sprite
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="newSprite"> nouveau sprite</param>
        public void PublicChangeSprite(string newSprite)
        {
            this.ChangeSprite(newSprite);
        }

        /// <summary>
        /// Place le sprite
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="x"> axe x</param>
        /// <param name="y">axe y</param>
        public void PublicPutXY(double x, double y)
        {
            x = Math.Round(x);
            y = Math.Round(y);
            PutXY(x, y);
        }

        /// <summary>
        /// Règle l'opacité du sprite
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="opacity">opacité désiré</param>
        public void PublicSpriteOpacity(int opacity)
        {
            this.SpriteOpacity = opacity;
        }
        #endregion

    }
}
