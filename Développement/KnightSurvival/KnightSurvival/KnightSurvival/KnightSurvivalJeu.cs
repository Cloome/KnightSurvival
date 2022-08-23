using System;
using System.Windows.Controls;
using System.Text;
using IUTGame;
using System.Windows;

namespace KnightSurvival
{
/// <summary>
/// Classe contenant le jeu et ses fonctions de bases
/// </summary>

    public class KnightSurvivalJeu : Game
    {
        private IHMVie iHMVie;
        private IHMChrono iHMChrono;
        private Score score;
        private IHMLvlEtXp iHMLvlEtXp;

        public IHMLvlEtXp IHMLvlEtXp { get => iHMLvlEtXp; set => iHMLvlEtXp = value; }



        /// <summary>
        /// Constructeur de notre jeu Knight Survival -- Clotilde
        /// </summary>
        /// <param name="canvas"> canvas représentant la fenêtre du jeu </param>
        public KnightSurvivalJeu(Canvas canvas) : base(canvas, "Sprites", "Sounds")
        {
        }

        /// <summary>
        /// Initialise le jeu et créer les items
        /// </summary>
        protected override void InitItems()
        {
            score = new Score();
            GameItemPlatform sol = new GameItemPlatform(-200, 609, this, Canvas, "PlatformBaseSol.png");
            AddItem(sol);
            GameItemPlatform haut = new GameItemPlatform(356, 180, this, Canvas, "PlatformHaut.png");
            AddItem(haut);
            GameItemPlatform gauche = new GameItemPlatform(-600, 400, this, Canvas, "PlatformCote.png");
            AddItem(gauche);
            GameItemPlatform droite = new GameItemPlatform(900, 400, this, Canvas, "PlatformCote.png");
            AddItem(droite);
            Joueur joueur = new Joueur(Canvas, this); // création joueur
            AddItem(joueur);
             iHMVie = new IHMVie(1130, 20, this, Canvas, joueur);
            AddItem(iHMVie);
             iHMChrono = new IHMChrono(1130, 60, Canvas, this);
            AddItem(iHMChrono);
            IHMLvlEtXp = new IHMLvlEtXp(1130, 100, Canvas, this, joueur);
            AddItem(IHMLvlEtXp);
            GenerateurPotion generateurPotion = new GenerateurPotion(Canvas, this);
            AddItem(generateurPotion);
            GenerateurEnnemi generateurEnnemi = new GenerateurEnnemi(Canvas, this, joueur);            
            AddItem(generateurEnnemi);

            PlayBackgroundMusic("Jeremy Blake  Powerup  NO COPYRIGHT 8bit Music.mp3"); // lance la musique
            

        }

        /// <summary>
        /// Ouvre une fenêtre quand le joueur gagne : inutile pas de système de victoire --Clotilde
        /// </summary>
        protected override void RunWhenWin()
        {
            System.Windows.MessageBox.Show("Vous avez gagné"); 
        }

        /// <summary>
        /// Ouvre une nouvelle fenêtre quand le joueur perd (texte variable selon langue) et permet de rejouer --Clotilde
        /// </summary>
        protected override void RunWhenLoose()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var mainWindow = Application.Current.MainWindow as MainWindow; // recupere la mainwindow actuel

                System.Windows.MessageBox.Show(mainWindow.TextLoose); // ouvre une fenetre avec le texte défini dans textloose (dans mainwindow)
                score.InsererTemps(iHMChrono.Time);
                Canvas.Children.Remove(iHMVie.VieIHM);
                Canvas.Children.Remove(iHMChrono.ChronoText);
                Canvas.Children.Remove(iHMLvlEtXp.Lvl);
                Canvas.Children.Remove(iHMLvlEtXp.Xp);
                foreach(IHMDegats iHMDegats in IHMDegats.ListIHMDegats)
                {
                    this.RemoveItem(iHMDegats);
                    Canvas.Children.Remove(iHMDegats.DegatsText);
                    iHMDegats.Dispose();
                }

                this.RemoveItem(iHMVie);
                this.RemoveItem(iHMChrono);
                this.RemoveItem(iHMLvlEtXp);
                iHMVie.Dispose();
                iHMChrono.Dispose();
                iHMLvlEtXp.Dispose();


                mainWindow.LooseGoBack(); // permet de rejouer

            }));

        }

    }
}
