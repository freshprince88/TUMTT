//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// A player participating in a <see cref="Match"/>.
    /// </summary>
    public class Player : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Name"/> property.
        /// </summary>
        private string name;

        /// <summary>
        /// Backs the <see cref="Nationality"/> property.
        /// </summary>
        private string nationality;

        /// <summary>
        /// Backs the <see cref="Rank"/> property.
        /// </summary>
        private Rank rank;

        /// <summary>
        /// Backs the <see cref="Spielsystem"/> property.
        /// </summary>
        private Spielsystem spielsystem = Spielsystem.None;

        /// <summary>
        /// Backs the <see cref="Händigkeit"/> property.
        /// </summary>
        private Händigkeit händigkeit = Händigkeit.None;

        /// <summary>
        /// Backs the <see cref="Griffhaltung"/> property.
        /// </summary>
        private Griffhaltung griffhaltung = Griffhaltung.None;

        /// <summary>
        /// Backs the <see cref="Material"/> property.
        /// </summary>
        private string material;

        private MatchPlayer playerIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.rank = new Rank(0, DateTime.Today);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        public Player(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="nationality">The nationality of the player.</param>
        public Player(string name, string nationality)
        {
            this.Name = name;
            this.Nationality = nationality;
        }

        /// <summary>
        /// Gets or sets the last name of this player.
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged();
                    this.NotifyPropertyChanged("Name");
                    this.NotifyPropertyChanged("FullName");
                }
            }
        }


        /// <summary>
        /// Gets or sets the Spielsystem of this player.
        /// </summary>
        [XmlAttribute]
        public Spielsystem Spielsystem
        {
            get
            {
                return this.spielsystem;
            }

            set
            {
                if (this.spielsystem != value)
                {
                    this.spielsystem = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Händigkeit of this player.
        /// </summary>
        [XmlAttribute]
        public Händigkeit Händigkeit
        {
            get
            {
                return this.händigkeit;
            }

            set
            {
                if (this.händigkeit != value)
                {
                    this.händigkeit = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Griffhaltung of this player.
        /// </summary>
        [XmlAttribute]
        public Griffhaltung Griffhaltung
        {
            get
            {
                return this.griffhaltung;
            }

            set
            {
                if (this.griffhaltung != value)
                {
                    this.griffhaltung = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Griffhaltung of this player.
        /// </summary>
        [XmlAttribute]
        public MatchPlayer PlayerIndex
        {
            get
            {
                return this.playerIndex;
            }

            set
            {
                if (this.playerIndex != value)
                {
                    this.playerIndex = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Material of this player.
        /// </summary>
        [XmlAttribute]
        public string Material
        {
            get
            {
                return this.material;
            }

            set
            {
                if (this.material != value)
                {
                    this.material = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the nationality of this player.
        /// </summary>
        [XmlAttribute]
        public string Nationality
        {
            get
            {
                return this.nationality;
            }

            set
            {
                if (this.nationality != value)
                {
                    this.nationality = value;
                    this.NotifyPropertyChanged();
                    this.NotifyPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the rank of this player.
        /// </summary>
        public Rank Rank
        {
            get
            {
                return this.rank;
            }

            set
            {
                if (this.rank != value)
                {
                    this.rank = value;
                    this.NotifyPropertyChanged();
                    this.NotifyPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets a human-readable name of the player, including nationality and rank.
        /// </summary>
        [XmlIgnore]
        public string FullName
        {
            get
            {
                var nationality = !string.IsNullOrEmpty(this.Nationality) ?
                    this.Nationality : "unknown nationality";
                var rank = this.Rank != null && this.Rank.Position > 0 ?
                    this.Rank.ToString() : "no ranking";
                return string.Format("{0} ({1}, {2})", this.Name, nationality, rank);
            }
        }

        /// <summary>
        /// Gets a human-readable string representation of this player.
        /// </summary>
        /// <returns>A string representation of this player.</returns>
        public override string ToString()
        {
            return this.FullName;
        }
    }
}
