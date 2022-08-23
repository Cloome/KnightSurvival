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
    /// Test unitaire d'un ennemi
    /// </summary>
    public class TestEnnemi
    {
      
        [Fact]
        /// <summary>
        /// Permet de vérifier si l'ennemi apparaît au bon endroit 
        /// </summary>  
        /// <author> Clotilde MALO </author>
        public void TestLieuPop()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {

                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                GenerateurEnnemi generateurEnnemi = new GenerateurEnnemi(canvas, jeu, joueur);
                jeu.AddItem(generateurEnnemi);

                Assert.True(generateurEnnemi.XEnnemi == 0 || generateurEnnemi.XEnnemi == 1250 || generateurEnnemi.XEnnemi == 550 || generateurEnnemi.XEnnemi == 800 || generateurEnnemi.XEnnemi == 400 || generateurEnnemi.XEnnemi == 300);
                Assert.True(generateurEnnemi.YEnnemi == 0 || generateurEnnemi.YEnnemi == 500 || generateurEnnemi.YEnnemi == 250 || generateurEnnemi.YEnnemi == 300);

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Fact]

        /// <summary>
        /// Permet de vérifier si l'ennemi apparaît le bon nombre de fois 
        /// </summary>
        /// <author> Clotilde MALO</author>
        public void TestNombreApparition()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                GenerateurEnnemi generateurEnnemi = new GenerateurEnnemi(canvas, jeu, joueur);
                jeu.AddItem(generateurEnnemi);
                Assert.True(generateurEnnemi.PopMax(10) == 10/10);
                Assert.True(generateurEnnemi.PopMax(9) == 9/10);
                Assert.True(generateurEnnemi.PopMax(11) == 11/10);
                Assert.True(generateurEnnemi.PopMax(25) == 25/10);
                Assert.True(generateurEnnemi.PopMax(26) == 26/10);
                Assert.True(generateurEnnemi.PopMax(40) == 40/10);
                Assert.True(generateurEnnemi.PopMax(41) == 41/10);
                Assert.True(generateurEnnemi.PopMax(60) == 60/10);
                Assert.True(generateurEnnemi.PopMax(61) == 61/10);
                Assert.True(generateurEnnemi.PopMax(75) == 75/10);
                Assert.True(generateurEnnemi.PopMax(90) == 90/10);
                Assert.True(generateurEnnemi.PopMax(200) == 200/10);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        [Fact]

        /// <summary>
        /// Permet de vérifier si l'ennemi apparaît au bon moment 
        /// </summary>
        /// <author> Clotilde MALO</author>
        public void TestTempsApparition()
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                Canvas canvas = new Canvas();
                KnightSurvivalJeu jeu = new KnightSurvivalJeu(canvas);
                Joueur joueur = new Joueur(canvas, jeu);
                jeu.AddItem(joueur);
                GenerateurEnnemi generateurEnnemi = new GenerateurEnnemi(canvas, jeu, joueur);
                jeu.AddItem(generateurEnnemi);

                Assert.True(generateurEnnemi.VitessePop(2) == (10 / (1 + 2 /10)) ); 

                Assert.True(generateurEnnemi.VitessePop(0.05) == (10 / (1 + 0.05 / 10)) );
                Assert.True(generateurEnnemi.VitessePop(248) == (10 / (1 + 248 / 10)) );
                Assert.True(generateurEnnemi.VitessePop(1247) == (10 / (1 + 1247 / 10)) );

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


    }

}
