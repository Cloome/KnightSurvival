using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KnightSurvival
{
    /// <summary>
    /// Défini le temps du jeu
    /// </summary>
    /// <author> Félix ARNOUX </author>
    [DataContract]
    public class Time
    {
        #region attributs et propriétés
        [DataMember]
        private int minutes = 0;
        [DataMember]
        private int secondes = 0;
        [DataMember]
        private int millisecondes = 0;

        public int Minutes { get => minutes; set => minutes = value; }
        public int Secondes { get => secondes; set => secondes = value; }
        public int Millisecondes { get => millisecondes; set => millisecondes = value; }
        #endregion
    }
}
