using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KnightSurvival
{
    /// <summary>
    /// Classe qui gère les scores
    /// </summary>
    [DataContract]
    public class Score
    {
        [DataMember]
        #region attributs et propriétés
        private List<Time> classement = new List<Time>();

        public List<Time> Classement { get => classement; set => classement = value; }
        #endregion

        #region constructeur
        /// <summary>
        /// Constructeur de l'objet Score
        /// </summary>
        /// <author> Félix ARNOUX </author>
        public Score()
        {
            try
            {
                OuvrirFichierScore();
            }
            catch
            {
                CreerFichierScore();
                OuvrirFichierScore();
            }
        }
        #endregion

        #region méthodes
        /// <summary>
        /// Ouvre le fichier Score.txt et le place dans classement
        /// </summary>
        /// <author> Félix ARNOUX </author>
        public void OuvrirFichierScore()
        {
            using(FileStream stream = File.OpenRead(@".\Score.txt"))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Score));
                Score score = ser.ReadObject(stream) as Score;
                this.Classement = score.Classement;
            }

        }

        /// <summary>
        /// Créer le fichier Score.txt
        /// </summary>
        /// <author> Félix ARNOUX </author>
        public void CreerFichierScore()
        {
            for (int i = 0; i < 10; i++)
            {
                Time time = new Time();
                this.Classement.Add(time);
            }
            using (FileStream stream = File.OpenWrite(@".\Score.txt"))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Score));
                ser.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// Insere un nouveau temps (Insere si le nouveau temps est meilleur qu'un des 10 temps stocké)
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <param name="newTemps"></param>
        public void InsererTemps(Time newTemps)
        {
            bool estInserer = false;
            Time tampon = new Time();
            for(int i = 0; i < 10; i++)
            {
                if (estInserer)
                {
                    newTemps = tampon;
                    tampon = new Time();
                }
                if (newTemps.Minutes > Classement[i].Minutes)
                {
                    tampon = Classement[i];
                    Classement.Insert(i, newTemps);
                    estInserer = true;
                }
                else if (newTemps.Minutes == Classement[i].Minutes && newTemps.Secondes > Classement[i].Secondes)
                {
                    tampon = Classement[i];
                    Classement.Insert(i, newTemps);
                    estInserer = true;
                }
                else if (newTemps.Minutes == Classement[i].Minutes && newTemps.Secondes == Classement[i].Secondes && newTemps.Millisecondes > Classement[i].Millisecondes)
                {
                    tampon = Classement[i];
                    Classement.Insert(i, newTemps);
                    estInserer = true;
                }                
            }
            if (Classement.Count == 11)
            {
                Classement.RemoveAt(10);
            }
            this.Sauvegarde();
        }

        /// <summary>
        /// Sauvegarde les temps dans le fichier Score.txt
        /// </summary>
        /// <author> Félix ARNOUX </author>
        public void Sauvegarde()
        {
            using (FileStream stream = File.OpenWrite(@".\Score.txt"))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Score));
                ser.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// Mise en forme de tout les temps stocké pour afficher sur le tableau des scores
        /// </summary>
        /// <author> Félix ARNOUX </author>
        /// <returns> Chaine de caractere de tous les scores </returns>
        public override string ToString()
        {
            string toString = "";
            for (int i = 0; i < Classement.Count; i++)
            {
                string min = "";
                string sec = "";
                string ms = "";
                if (Classement[i].Minutes < 10)
                {
                    min = "0";
                }

                if (Classement[i].Secondes < 10)
                {
                    sec = "0";
                }

                if (Classement[i].Millisecondes < 10)
                {
                    ms = "00";
                }
                else if (Classement[i].Millisecondes < 100)
                {
                    ms = "0";
                }
                if (i == 0)
                {
                    toString += String.Format("1er : {0}{1}:{2}{3}.{4}{5}\n", min, Classement[i].Minutes, sec, Classement[i].Secondes, ms, Classement[i].Millisecondes);
                }
                else
                {
                    toString += String.Format("{0}e : {1}{2}:{3}{4}.{5}{6}\n", i + 1, min, Classement[i].Minutes, sec, Classement[i].Secondes, ms, Classement[i].Millisecondes);
                }
            }

            return toString;
        }
        #endregion
    }
}
