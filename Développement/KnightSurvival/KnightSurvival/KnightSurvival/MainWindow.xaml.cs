using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IUTGame;

namespace KnightSurvival
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region attributs et propriétés
        private KnightSurvivalJeu jeu;
        private Boolean partieLancee; // indique si une partie est en cours
        private string language; // indique la langue actuel du jeu
        private string textLoose = "You loose"; // texte ecrit quand le joueur perd
        private TextBlock scoreBlock = new TextBlock(); // bloc des scores
        public string TextLoose { get => textLoose; set => textLoose = value; } // texte qui s'affiche quand le joueur perd
        #endregion

        #region constructeur
        /// <summary>
        /// Initialise le lancement du jeu avec paramètre de départ
        /// </summary>
        /// <author> Clotilde MALO </author>
        public MainWindow()
        {
            InitializeComponent();
            jeu = new KnightSurvivalJeu(canvas_Game);
            partieLancee = false; // indique qu'il n'y a pas encore de partie lancée

            Menu_buttonContinue.Visibility = Visibility.Hidden; // cache le bouton continue du menu principal 
            Menu_buttonContinue.IsEnabled = false; // rend le bouton continue du menu principal inactif 

            language = "english"; // met par défaut le jeu en anglais

            this.PreviewKeyDown += new KeyEventHandler(PressEsc); // prend en charge l'event echap = mainMenu

            

        }
        #endregion

        
        #region methodes

        /// <summary>
        /// Gère le slider de volume 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            jeu.BackgroundVolume = SliderSound.Value ;

        }

        /// <summary>
        /// Baisse le son au max musique de fond 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundMin(object sender, MouseButtonEventArgs e)
        {
            jeu.BackgroundVolume = 0;
            SliderSound.Value = 0; // fait en sorte que le slider soit cohérent avec le son min


        }

        /// <summary>
        /// Monte le son au max musique de fond
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundMax(object sender, MouseButtonEventArgs e)
        {
            jeu.BackgroundVolume = 1; 
            SliderSound.Value = 1; // fait en sorte que le slider soit cohérent avec le son max

        }


        /// <summary>
        /// Lance une nouvelle partie pour jouer 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void GoGame(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_MainMenu, 0);
            Panel.SetZIndex(canvas_Score , 0);
            Panel.SetZIndex(canvas_Commande , 0);

            Panel.SetZIndex(canvas_Game, 1); // Affiche le jeu
            canvas_Game.Focus(); // Donne le focus
            jeu.Run();
            partieLancee = true;

            Menu_buttonNew.Visibility = Visibility.Hidden; // rend le bouton new game du menu principal invisible
            Menu_buttonNew.IsEnabled = false; // rend le bouton new game du menu principal inactif
            Menu_buttonContinue.Visibility = Visibility.Visible; // affiche le bouton continue du menu principal
            Menu_buttonContinue.IsEnabled = true; // rend le bouton continue du menu principal actif

        }
        /// <summary>
        /// Envoie vers l'affichage des commandes
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoCommande(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_MainMenu, 0);
            Panel.SetZIndex(canvas_Score, 0);
            Panel.SetZIndex(canvas_Game, 0);

            Panel.SetZIndex(canvas_Commande, 1); // Affiche le jeu
            canvas_Commande.Focus(); // Donne le focus
        }


        /// <summary>
        /// Reviens dans le jeu
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnGame(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            // Mettre les autres canvas à l'arrière plan
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_MainMenu, 0);
            Panel.SetZIndex(canvas_Score, 0);
            Panel.SetZIndex(canvas_Commande, 0);

            Panel.SetZIndex(canvas_Game, 1); // Affiche le jeu
            canvas_Game.Focus(); // Donne le focus

            if (partieLancee == true) 
            {
                jeu.Resume(); // enlève pause
            }
            else
            {
                jeu.Run();
            }

        }

        /// <summary>
        /// Renvoie vers les scores (hall of fame) 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoScore(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            if (jeu.IsRunning == true)
            {
                jeu.Pause();
            }
            // Mettre les autres canvas à l'arrière plan
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_MainMenu, 0);
            Panel.SetZIndex(canvas_Game, 0);
            Panel.SetZIndex(canvas_Commande, 0);

            Panel.SetZIndex(canvas_Score, 1); // Affiche menu principal
            canvas_Score.Focus(); // Donne focus


            AffichageScore();
            
        }

        
        /// <summary>
        /// Affiche le score dans le hall of fame
        /// </summary>
        /// <author> Félix ARNOUX </author>
        protected void AffichageScore()
        {
            //Test d'affichage du hall of fame

            Score score = new Score();
            scoreBlock.Text = score.ToString();
            scoreBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            scoreBlock.FontSize = 42;
            Canvas.SetLeft(scoreBlock, (1264 / 2) -90);
            Canvas.SetTop(scoreBlock, 140);

            if (!canvas_Score.Children.Contains(scoreBlock))
            {
                canvas_Score.Children.Add(scoreBlock);
            }
        }

        /// <summary>
        /// Ouvre le menu de paramètres 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoSettings(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            if (jeu.IsRunning == true)
            {
                jeu.Pause();
            }
            // Mettre les autres canvas à l'arrière plan
            Panel.SetZIndex(canvas_MainMenu, 0);
            Panel.SetZIndex(canvas_Score, 0);
            Panel.SetZIndex(canvas_Game, 0);
            Panel.SetZIndex(canvas_Commande, 0);

            Panel.SetZIndex(canvas_Settings, 1); // Affiche menu paramètre english
            canvas_Settings.Focus(); // Donne le focus english
            
        }

        /// <summary>
        /// Ouvre le menu principal 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMainMenu(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            if (jeu.IsRunning == true)
            {
                jeu.Pause();
            }
            // Mettre les autres canvas à l'arrière plan
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_Score, 0);
            Panel.SetZIndex(canvas_Game, 0);
            Panel.SetZIndex(canvas_Commande, 0);


            Panel.SetZIndex(canvas_MainMenu, 1); // Affiche menu principal 
            canvas_MainMenu.Focus(); // Donne focus

           

        }

        /// <summary>
        /// Quitte le jeu
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGame(object sender, RoutedEventArgs e)
        {
            PlayClickInterface();
            this.Close();
        }

        /// <summary>
        /// Change la langue du jeu en français
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoFr(object sender, MouseButtonEventArgs e)
        {
            language = "français"; // stocke la variable pour dire jeu français

            flagFR.Opacity = 1; // change l'opacité du drapeau pour rendre "actif" cette langue
            flagUK.Opacity = 0.45; // change l'opacité du drapeau pour rendre "inactif" cette langue

            // Change les textes du jeu
            Settings_Settings.Content = "PARAMETRES";
            Settings_Language.Content = "Langue";
            Settings_Return.Content = "Retour";

            Menu_buttonNew.Content = "NOUVEAU JEU";
            Menu_buttonContinue.Content = "CONTINUER";
            Menu_buttonScore.Content = "SCORES";
            Menu_buttonSettings.Content = "PARAMETRES";
            Menu_buttonExit.Content = "QUITTER";

            Score_Return.Content = "Retour";

            ContinuerCommande.Content = "Continuer";

            Commande_Saut.Content = "SAUTER";
            Commande_Attaque.Content = "ATTAQUER";
            Commande_Droite.Content = "AVANCER A DROITE";
            Commande_Gauche.Content = "AVANCER A GAUCHE";
            TextLoose = "Vous avez perdu";

            Titre_HallOfFame.Content = "Scores";


        }
        /// <summary>
        /// Change la langue du jeu en anglais 
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoEn(object sender, MouseButtonEventArgs e)
        {

            language = "english"; // stocke la variable pour dire jeu en

            flagFR.Opacity = 0.45; // change l'opacité du drapeau pour rendre "inactif" cette langue
            flagUK.Opacity = 1; // change l'opacité du drapeau pour rendre "actif" cette langue

            // Change les textes du jeu
            Settings_Settings.Content = "SETTINGS";
            Settings_Language.Content = "Language";
            Settings_Return.Content = "Back";

            Menu_buttonNew.Content = "NEW GAME";
            Menu_buttonContinue.Content = "CONTINUE";
            Menu_buttonScore.Content = "HALL OF FAME";
            Menu_buttonSettings.Content = "SETTINGS";
            Menu_buttonExit.Content = "EXIT";

            Score_Return.Content = "Back";

            ContinuerCommande.Content = "Continue";

            Commande_Saut.Content = "JUMP";
            Commande_Attaque.Content = "ATTACK";
            Commande_Droite.Content = "MOVE RIGHT";
            Commande_Gauche.Content = "MOVE LEFT";
            TextLoose = "You loose";

            Titre_HallOfFame.Content = "Hall of Fame";


        }

        /// <summary>
        /// Fait en sorte que lorsque l'on presse echap renvoie vers le menu principal
        /// </summary>
        /// <author> Clotilde MALO </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PressEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape )
            {
                GoMainMenu(sender, e); 
            }

        }

        /// <summary>
        /// (Pas utilisé) Lance un son quand on clique sur l'interface 
        /// </summary>
        /// <author> Félix ARNOUX </author>
        private void PlayClickInterface()
        {
            //SoundStore.Get("ClickInterface.wav").Play(false);
        }

        /// <summary>
        /// (Pas utilisé) Lance un son quand on passe au dessus d'un élément
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayMouseOverSound(object sender, MouseEventArgs e)
        {
            //SoundStore.Get("MouseOverInterface.wav").Play(false);
        }





        /// <summary>
        /// Retourne sur le menu principal, a appeler quand le joueur a perdu
        /// </summary>
        /// <author> Clotilde MALO </author>
        public void LooseGoBack()
        {

            partieLancee = false;

            Menu_buttonNew.Visibility = Visibility.Visible; // rend le bouton new game du menu principal invisible
            Menu_buttonNew.IsEnabled = true; // rend le bouton new game du menu principal inactif
            Menu_buttonContinue.Visibility = Visibility.Hidden; // affiche le bouton continue du menu principal
            Menu_buttonContinue.IsEnabled = false; // rend le bouton continue du menu principal actif

            // Mettre les autres canvas à l'arrière plan
            Panel.SetZIndex(canvas_Settings, 0);
            Panel.SetZIndex(canvas_Score, 0);
            Panel.SetZIndex(canvas_Game, 0);
            Panel.SetZIndex(canvas_Commande, 0);


            Panel.SetZIndex(canvas_MainMenu, 1); // Affiche menu principal 
            canvas_MainMenu.Focus(); // Donne focus
        }

        #endregion

    }
}

