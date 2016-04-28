//-----------------------------------------------------------------------
// <copyright file="Transitions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra;
    /// <summary>
    /// Transitions to point state.
    /// </summary>
    public class Transitions : MatchStatistics
    {
        /// <summary>
        /// The maximum rally length in transition matrixes.
        /// </summary>
        private const int StrokeLimit = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transitions"/> class.
        /// </summary>
        /// <param name="match">The match</param>
        public Transitions(Match match)
            : base(match)
        {
            this.TransitionsByPlayer = new Dictionary<MatchPlayer, Matrix<double>>();
            this.PointsAtStrokeByPlayer = new Dictionary<MatchPlayer, Vector<double>>();
            this.ErrorsAtStrokeByPlayer = new Dictionary<MatchPlayer, Vector<double>>();
            this.ComputeTransitionsForPlayer(MatchPlayer.First);
            this.ComputeTransitionsForPlayer(MatchPlayer.Second);

            this.ProbabilitiesByPlayer = new Dictionary<MatchPlayer, Matrix<double>>();
            this.PointAtStrokeProbabilityByPlayer = new Dictionary<MatchPlayer, Vector<double>>();
            this.ErrorAtStrokeProbabilityByPlayer = new Dictionary<MatchPlayer, Vector<double>>();
            this.ComputeProbabilitiesForPlayer(MatchPlayer.First);
            this.ComputeProbabilitiesForPlayer(MatchPlayer.Second);

            this.TransitionProbabilities = new SparseMatrix(
                this.ProbabilitiesByPlayer.Values.Sum(m => m.RowCount) + 2,
                this.ProbabilitiesByPlayer.Values.Sum(m => m.ColumnCount) + 2);
            this.ComputeProbabilities();
        }

        /// <summary>
        /// Gets the matrix describing the absolute transitions for each player.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For each player, the matrix m contains the number of transitions from
        /// stroke i of the player to stroke j of the other player at m[i,j], for i ≤ k and j ≤ k.
        /// </para>
        /// <para>
        /// m[k,j] contains the transitions from all strokes &gt; k to the stroke i of the 
        /// other player.
        /// </para>
        /// <para>
        /// m[i,k] contains the transitions from the stroke i of the player to all strokes
        /// &gt; k of the other player
        /// </para>
        /// <para>
        /// m has the k+2 rows and k+2 columns.
        /// </para>
        /// </remarks>
        public IDictionary<MatchPlayer, Matrix<double>> TransitionsByPlayer { get; private set; }

        /// <summary>
        /// Gets the points at stroke for each player.
        /// </summary>
        /// <remarks>
        /// For each player, the vector v contains the points scored with 
        /// stroke i in v[i], for ≤ k.  v[k+1] contains the points scored at
        /// The at all strokes &gt; k.  |v| is k+2 hence.
        /// </remarks>
        public Dictionary<MatchPlayer, Vector<double>> PointsAtStrokeByPlayer { get; private set; }

        /// <summary>
        /// Gets the errors at stroke for each player.
        /// </summary>
        /// <remarks>
        /// For each player, the vector v contains the errors made with the
        /// stroke i at v[i], for i ≤ k. v[k+1] contains the errors make
        /// at all strokes &gt; k.  |v| is hence k+2.
        /// </remarks>
        public Dictionary<MatchPlayer, Vector<double>> ErrorsAtStrokeByPlayer { get; private set; }

        /// <summary>
        /// Gets the matrix describing the transition probabilities for each player.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For each player, the matrix m contains the probability of a transitions
        /// from stroke i of the player to stroke j of the other player at m[i,j],
        /// for i ≤ k and j ≤ k.
        /// </para>
        /// <para>
        /// m[k,j] contains the probabilities of a transition from all strokes &gt; k
        /// to the stroke i of the other player.
        /// </para>
        /// <para>
        /// m[i,k] contains the probabilities of a transition from the stroke i 
        /// of the player to all strokes &gt; k of the other player
        /// </para>
        /// <para>
        /// m has the k+2 rows and k+2 columns.
        /// </para>
        /// </remarks>        
        public IDictionary<MatchPlayer, Matrix<double>> ProbabilitiesByPlayer { get; private set; }

        /// <summary>
        /// Gets the probability of scoring a point at stroke for each player.
        /// </summary>
        /// <remarks>
        /// For each player, the vector v contains the probability of scoring a 
        /// point with stroke i in v[i], for ≤ k.  v[k+1] contains the probability
        /// of scoring a point at all strokes &gt; k.  |v| is k+2 hence.
        /// </remarks>
        public Dictionary<MatchPlayer, Vector<double>> PointAtStrokeProbabilityByPlayer { get; private set; }

        /// <summary>
        /// Gets the probability of making an error at stroke for each player.
        /// </summary>
        /// <remarks>
        /// For each player, the vector v contains the probability of making an
        /// error with stroke i in v[i], for ≤ k.  v[k+1] contains the probability
        /// of making an errors at all strokes &gt; k.  |v| is k+2 hence.
        /// </remarks>        
        public Dictionary<MatchPlayer, Vector<double>> ErrorAtStrokeProbabilityByPlayer { get; private set; }

        /// <summary>
        /// Gets the complete transition probability for the match.
        /// </summary>        
        /// <remarks>
        /// <para>
        /// This matrix represents the state transition probabilities of the whole
        /// match.  It is a square matrix of m x m rows and columns, zero-indexed.
        /// </para>
        /// <para>
        /// The (m-1)'th row represents the probability of a transition from a point
        /// of the first player, and the (m-1)'th column the probability of a transition to
        /// a point of the first player.  The (m-2)'th row and column represent the 
        /// probabilities from and to points of the second player.
        /// </para>
        /// <para>
        /// All other rows and columns represent probabilities of transition
        /// between strokes of the players, according to the following pattern:
        /// </para>
        /// <para>
        /// Each row (j*2) represents the probability of a transition from the j'th
        /// stroke of the first player.  Each row (j*2)+1 represents the probability
        /// of a transition from the j'th stroke of the second player.
        /// </para>
        /// <para>
        /// Each column (k*2) represents the probability of a transition to the k'th
        /// stroke of the first player.  Each column (k*2)+1 represents the probability
        /// of a transition to the k'th stroke of the second player.
        /// </para>
        /// <para>
        /// The last two of these rows and columns are cumulative.
        /// </para>
        /// </remarks>
        public Matrix<double> TransitionProbabilities { get; private set; }

        /// <summary>
        /// Computes the transitions for a player.
        /// </summary>
        /// <param name="player">The player.</param>
        private void ComputeTransitionsForPlayer(MatchPlayer player)
        {
            var transitions = new SparseMatrix(StrokeLimit + 2, StrokeLimit + 2);
            var points = new DenseVector(StrokeLimit + 2);
            var errors = new DenseVector(StrokeLimit + 2);

            // Fill the transitions
            for (int i = 1; i < transitions.ColumnCount - 1; ++i)
            {
                // Determine which player has to serve for our player to
                // make this stroke.  For odd numbers, it's the player herself,
                // but for even numbers, she plays the return.
                var server = i % 2 == 0 ? player.Other() : player;

                // Get all rallies that are actually long enough for our player
                // to get to stroke i+1
                var rallies = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Length >= i && r.Server == server);

                // Now determine how many rallies end here
                var endingRallies = rallies.Where(r => r.Length == i);

                // The number of transitions to the next stroke by the other
                // player is then obviously the number of all rallies long enough,
                // minus the number of ending rallies.
                transitions[i, i + 1] = rallies.Count() - endingRallies.Count();

                // And now let's just find out quickly, how the ending rallies
                // ended: With a point or an error of our player
                points[i] = endingRallies.Count(r => r.Winner == player);
                errors[i] = endingRallies.Count(r => r.Winner == player.Other());
            }

            // Now compute the catch all state.
            var allRemainingRallies = this.Match.DefaultPlaylist.FinishedRallies
                .Where(r => r.Length > StrokeLimit);

            var remainingRalliesEnding = allRemainingRallies
                .Where(r => r.Server == (r.Length % 2 == 0 ? player.Other() : player));

            transitions[StrokeLimit + 1, StrokeLimit + 1] = allRemainingRallies
                .Sum(r =>
                {
                    // Calculate the number of transitions from our player to
                    // the other player, this rally as gone through above the
                    // stroke limit.
                    var no = (r.Length - StrokeLimit) / 2.0;
                    return (int)(r.Server == player ? Math.Floor(no) : Math.Ceiling(no));
                }) - remainingRalliesEnding.Count();

            points[StrokeLimit + 1] = remainingRalliesEnding.Count(r => r.Winner == player);
            errors[StrokeLimit + 1] = remainingRalliesEnding.Count(r => r.Winner == player.Other());

            this.PointsAtStrokeByPlayer[player] = points;
            this.ErrorsAtStrokeByPlayer[player] = errors;
            this.TransitionsByPlayer[player] = transitions;
        }

        /// <summary>
        /// Computes the transition probabilities.
        /// </summary>
        /// <param name="player">The player.</param>
        private void ComputeProbabilitiesForPlayer(MatchPlayer player)
        {
            var transitions = this.TransitionsByPlayer[player];
            var points = this.PointsAtStrokeByPlayer[player];
            var errors = this.ErrorsAtStrokeByPlayer[player];

            var rowSums = transitions.EnumerateRowsIndexed()
                .Select(c => c.Item2.Sum() + points[c.Item1] + errors[c.Item1])
            .ToArray();

            var pTransition = SparseMatrix.OfMatrix(transitions);
            pTransition.MapIndexedInplace((r, _, n) => rowSums[r] != 0 ? n / rowSums[r] : 0);
            this.ProbabilitiesByPlayer[player] = pTransition;

            var pPoints = DenseVector.OfVector(points);
            pPoints.MapIndexedInplace((r, n) => rowSums[r] != 0 ? n / rowSums[r] : 0);
            this.PointAtStrokeProbabilityByPlayer[player] = pPoints;

            var pErrors = DenseVector.OfVector(errors);
            pErrors.MapIndexedInplace((r, n) => rowSums[r] != 0 ? n / rowSums[r] : 0);
            this.ErrorAtStrokeProbabilityByPlayer[player] = pErrors;
        }

        /// <summary>
        /// Computes the total probabilities matrix.
        /// </summary>
        private void ComputeProbabilities()
        {
            var probabilities = this.TransitionProbabilities;
            var first = this.ProbabilitiesByPlayer[MatchPlayer.First];
            var second = this.ProbabilitiesByPlayer[MatchPlayer.Second];

            foreach (var item in first.EnumerateIndexed())
            {
                var row = item.Item1;
                var column = item.Item2;
                var p = item.Item3;
                probabilities[row * 2, (column * 2) + 1] = p;
            }

            foreach (var item in second.EnumerateIndexed())
            {
                var row = item.Item1;
                var column = item.Item2;
                var p = item.Item3;
                probabilities[(row * 2) + 1, column * 2] = p;
            }

            foreach (var item in this.PointAtStrokeProbabilityByPlayer[MatchPlayer.First].EnumerateIndexed())
            {
                var row = item.Item1;
                var p = item.Item2;
                probabilities[row * 2, first.ColumnCount * 2] = p;
            }

            foreach (var item in this.ErrorAtStrokeProbabilityByPlayer[MatchPlayer.First].EnumerateIndexed())
            {
                var row = item.Item1;
                var p = item.Item2;
                probabilities[row * 2, (first.ColumnCount * 2) + 1] = p;
            }

            foreach (var item in this.PointAtStrokeProbabilityByPlayer[MatchPlayer.Second].EnumerateIndexed())
            {
                var row = item.Item1;
                var p = item.Item2;
                probabilities[(row * 2) + 1, (second.ColumnCount * 2) + 1] = p;
            }

            foreach (var item in this.ErrorAtStrokeProbabilityByPlayer[MatchPlayer.Second].EnumerateIndexed())
            {
                var row = item.Item1;
                var p = item.Item2;
                probabilities[(row * 2) + 1, (second.ColumnCount * 2)] = p;
            }

            probabilities[first.RowCount * 2, first.ColumnCount * 2] = 1;
            probabilities[(second.RowCount * 2) + 1, (second.ColumnCount * 2) + 1] = 1;
        }
    }
}
