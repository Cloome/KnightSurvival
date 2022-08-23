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
    public class TestJoueur
    {

        [Fact]
        /// <summary>
        /// Test la collision avec un ennemi pour tester sa repercution sur les point de vie du joueur
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestPrendDegat()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(0, 0, canvas, jeu, joueur, 0);
                jeu.AddItem(crawler1);

                joueur.CollideEffect(crawler1);

                Assert.Equal(98, joueur.PV);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Fact]
        /// <summary>
        /// Test la collision avec une potion pour tester sa repercution sur les point de vie du joueur
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestRestaurePV()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                EnnemiCrawler1 crawler1 = new EnnemiCrawler1(0, 0, canvas, jeu, joueur, 0);
                jeu.AddItem(crawler1);
                Potion potion = new Potion(0, 0, canvas, jeu); 
                jeu.AddItem(potion);

                joueur.CollideEffect(crawler1);

                joueur.CollideEffect(potion);
                Assert.Equal(100, joueur.PV);

                joueur.CollideEffect(potion);
                Assert.Equal(100, joueur.PV);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Fact]
        /// <summary>
        /// Test Si l'acquisition d'experience entraine bien la monté en niveau et les changement qui doivent s'operer sur le joueur
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestLvlUp()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);

                joueur.Experience = 9;
                Assert.Equal(100, joueur.PV);
                joueur.Experience = 1;
                Assert.Equal(120, joueur.PV);

                joueur.Experience = 19;
                Assert.Equal(1500, joueur.KnockBack);
                joueur.Experience = 1;
                Assert.Equal(1700, joueur.KnockBack);

                joueur.Experience = 39;
                Assert.Equal(0.3, joueur.AttackSpeed);
                joueur.Experience = 1;
                Assert.Equal(0.24, joueur.AttackSpeed);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
