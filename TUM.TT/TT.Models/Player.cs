//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-------------------------------------------------------------------

namespace TT.Models
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
        /// Backs the <see cref="PlayingStyle"/> property.
        /// </summary>
        private PlayingStyle playingStyle = PlayingStyle.None;

        /// <summary>
        /// Backs the <see cref="Handedness"/> property.
        /// </summary>
        private Handedness handedness = Handedness.None;

        /// <summary>
        /// Backs the <see cref="Grip"/> property.
        /// </summary>
        private Grip grip = Grip.None;
        /// <summary>
        /// Backs the <see cref="StartingTableEnd"/> property.
        /// </summary>
        private StartingTableEnd startingTableEnd = StartingTableEnd.None;

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

        public Player(int index) : this()
        {
            this.name = string.Format(Properties.Resources.player_name_default, index);
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
        /// Gets or sets the PlayingStyle of this player.
        /// </summary>
        [XmlAttribute]
        public PlayingStyle PlayingStyle
        {
            get
            {
                return this.playingStyle;
            }

            set
            {
                if (this.playingStyle != value)
                {
                    this.playingStyle = value;
                    this.NotifyPropertyChanged("PlayingStyle");
                    this.NotifyPropertyChanged();

                }
            }
        }


        /// <summary>
        /// Gets or sets the StartingTableEnd of this player.
        /// </summary>
        [XmlAttribute]
        public StartingTableEnd StartingTableEnd
        {
            get
            {
                return this.startingTableEnd;
            }

            set
            {
                if (this.startingTableEnd != value)
                {
                    this.startingTableEnd = value;
                    this.NotifyPropertyChanged("StartingTableEnd");
                    this.NotifyPropertyChanged();

                }
            }
        }

        /// <summary>
        /// Gets or sets the Handedness of this player.
        /// </summary>
        [XmlAttribute]
        public Handedness Handedness
        {
            get
            {
                return this.handedness;
            }

            set
            {
                if (this.handedness != value)
                {
                    this.handedness = value;
                    this.NotifyPropertyChanged("Handedness");
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Grip of this player.
        /// </summary>
        [XmlAttribute]
        public Grip Grip
        {
            get
            {
                return this.grip;
            }

            set
            {
                if (this.grip != value)
                {
                    this.grip = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the PlayerIndex of this player.
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
