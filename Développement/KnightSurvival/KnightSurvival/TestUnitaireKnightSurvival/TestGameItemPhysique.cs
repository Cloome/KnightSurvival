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
    public class TestGameItemPhysique
    {
        [Fact]
        /// <summary>
        /// Test si le joueur tombe bien au file du temps
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestGravité()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                TimeSpan dt = new TimeSpan(0, 0, 0, 0, 20);

                Assert.Equal(0, joueur.VitesseAxeY);

                joueur.Physique(dt);

                Assert.NotEqual(0, joueur.VitesseAxeY);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Fact]
        /// <summary>
        /// Test si le fait d'être posé sur une plateforme empêche bien le fait de tomber au fil du temps
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestGravitéEtCollisionPlateforme()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                TimeSpan dt = new TimeSpan(0, 0, 0, 0, 20);
                joueur.Pose = true;

                Assert.Equal(0, joueur.VitesseAxeY);

                joueur.Physique(dt);

                Assert.Equal(0, joueur.VitesseAxeY);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Fact]
        /// <summary>
        /// Test si le fait d'être posé sur une plateforme empêche bien le fait de tomber au fil du temps
        /// </summary>  
        /// <author> Félix ARNOUX </author>
        public void TestOrientationItem()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);

                Assert.Equal("Droite", joueur.OrientationItem());

                joueur.KeyDown(System.Windows.Input.Key.Q);
                Assert.Equal("Gauche", joueur.OrientationItem());
                joueur.KeyUp(System.Windows.Input.Key.Q);

                joueur.KeyDown(System.Windows.Input.Key.D);
                Assert.Equal("Droite", joueur.OrientationItem());
                joueur.KeyUp(System.Windows.Input.Key.D);

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
