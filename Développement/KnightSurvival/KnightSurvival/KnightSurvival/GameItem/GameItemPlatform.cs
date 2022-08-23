using System;
using System.Windows.Controls;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Classe contenant les game item de type plateformes
    /// </summary>
    public class GameItemPlatform : GameItem
    {
        #region attributs et propriétés
        public override string TypeName { get { return "platform"; } }
        #endregion
        #region constructeur
        /// <summary>
        /// Constructeur de plateforme
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="x">axe x</param>
        /// <param name="y">axe y</param>
        /// <param name="g">jeu</param>
        /// <param name="c">canva</param>
        /// <param name="name">nom de l'item</param>
        public GameItemPlatform(double x, double y, Game g, Canvas c, string name) : base(x, y, c, g, name)
        {

        }
        #endregion

        #region methodes
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
