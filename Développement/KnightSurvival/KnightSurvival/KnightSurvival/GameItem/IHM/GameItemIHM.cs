using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe gérant l'IHM de tous les game item
    /// </summary>
    public class GameItemIHM : GameItem
    {
        #region attributs et propriétés
        public override string TypeName { get { return "ihm"; } }
        #endregion

        #region constructeur
        /// <summary>
        /// Construit l'IHM d'un GameItem
        /// </summary>
        /// <author> Félix ARNOUX</author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="c">canva</param>
        /// <param name="g">jeu</param>
        /// <param name="sprite">sprite du gameitem</param>
        public GameItemIHM(double x, double y, Canvas c, Game g, string sprite = "") : base(x, y, c, g, sprite)
        {
            Collidable = false;
            if (sprite == "") { }
        }

        #endregion

        #region methodes
        /// <summary>
        /// Effet de collision : nul
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="other"></param>
        public override void CollideEffect(GameItem other)
        {

        }

        #endregion
    }
}
