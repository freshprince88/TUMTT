//-----------------------------------------------------------------------
// <copyright file="AbsoluteNumberOfTransition.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------
namespace TT.Models.Report
{
    using System.Collections.Generic;

    /// <summary>
    /// The transition statistics of a player.
    /// </summary>
    public class AbsoluteNumberOfTransition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbsoluteNumberOfTransition"/> class.
        /// </summary>
        public AbsoluteNumberOfTransition()
        {
            this.Transitions = new List<int>();
            this.PointsTransitionServer = new List<int>();
            this.PointsTransitionReceiver = new List<int>();
            this.TransitionProbabilities = new List<double>();
            this.PointsPropabilitiesServer = new List<double>();
            this.PointsPropabilitiesReceiver = new List<double>();
        }

        /// <summary>
        /// Gets or sets the list of transition counts.
        /// </summary>
        public List<int> Transitions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list if the transition's server points.
        /// </summary>
        public List<int> PointsTransitionServer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list if the transition's receiver points.
        /// </summary>
        public List<int> PointsTransitionReceiver
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the probability of the transitions
        /// </summary>
        public List<double> TransitionProbabilities
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the probabilities that the server scores
        /// </summary>
        public List<double> PointsPropabilitiesServer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the probabilities that the receiver scores
        /// </summary>
        public List<double> PointsPropabilitiesReceiver
        {
            get;
            set;
        }
    }
}
