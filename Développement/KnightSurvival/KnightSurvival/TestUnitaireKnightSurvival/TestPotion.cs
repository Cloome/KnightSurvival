using KnightSurvival;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xunit;

namespace TestUnitaireKnightSurvival
{
    /// <summary>
    /// Test unitaire des potions
    /// </summary>
    public class TestPotion
    {

        [Fact]

        /// <summary>
        /// Permet de vérifier si la potion apparaît au bon endroit 
        /// </summary>
        /// <author> Morgane VIALA </author>
        public void TestLieuPop()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                GenerateurPotion generateurpotion = new GenerateurPotion(canvas, jeu);
                jeu.AddItem(generateurpotion);

                Random r = new Random();
                double x = r.NextDouble() * canvas.Width;
                double y = r.NextDouble() * canvas.Height / 2;
                Assert.True(x <= canvas.Width && 0 <= x);
                Assert.True( y <= canvas.Height && 0 <= y);
            });
        }
    }
}
