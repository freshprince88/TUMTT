//-----------------------------------------------------------------------
// <copyright file="RelevanceOfStroke.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Computes the relevance of stroke.
    /// </summary>
    public class RelevanceOfStroke : MatchTransitionStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceOfStroke"/> class.
        /// </summary>
        /// <param name="transitions">The transitions</param>
        /// <param name="iterations">The iterations</param>
        public RelevanceOfStroke(Transitions transitions, int iterations = 50)
            : base(transitions)
        {
            this.Iterations = iterations;

            this.CountedWinningProbabilities = DenseVector.Create(
                2,
                i =>
                {
                    var w = i == 0 ? MatchPlayer.First : MatchPlayer.Second;
                    return this.Match.Rallies.Count(r => r.Winner == w) /
                        (double)this.Match.Rallies.Count;
                });

            var probabilities = transitions.TransitionProbabilities;
            var start = MakeStartVector(probabilities.ColumnCount);
            this.WinningProbabilities = Markov.Simulate(probabilities, start, iterations)
                .SubVector(probabilities.ColumnCount - 2, 2);

            this.ByScorer = this.ComputeRelevanceOfStroke();

            // Transform relevance of stroke to support plotting
            this.ByStriker = new DenseMatrix(
                this.ByScorer.RowCount,
                this.ByScorer.ColumnCount);

            foreach (var pair in this.ByScorer.EnumerateRowsIndexed())
            {
                var v = pair.Item2;
                var sourceRow = pair.Item1;

                var targetColumn = sourceRow % 2;
                var targetRow = sourceRow - targetColumn;

                this.ByStriker.SetColumn(
                    targetColumn,
                    targetRow,
                    v.Count,
                    targetColumn == 0 ? v : DenseVector.OfEnumerable(v.Reverse()));
            }
        }

        /// <summary>
        /// Gets the number of iterations for the winning probability.
        /// </summary>
        public int Iterations { get; private set; }

        /// <summary>
        /// Gets the counted winning probabilities.
        /// </summary>
        /// <remarks>
        /// The value of this property is a 2-element vector, where the 0th
        /// item is the winning probability of the first player, and the 1st
        /// item the probability of the second player.
        /// </remarks>
        public Vector<double> CountedWinningProbabilities
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the simulated winning probabilities.
        /// </summary>
        /// <remarks>
        /// The value of this property is a 2-element vector, where the 0th
        /// item is the winning probability of the first player, and the 1st
        /// item the probability of the second player.
        /// </remarks>
        public Vector<double> WinningProbabilities
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the relevance of stroke matrix by scoring player.       
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of this property is a mx2 matrix.
        /// </para>
        /// <para>
        /// Each row (j*2) represents the relevance of the j'th stroke of the first
        /// player.  Each Row (j*2)+1 represents the relevance of the j'th stroke
        /// of the second player.
        /// </para>
        /// <para>
        /// The first column represents the relevance of a stroke at which the
        /// first player scored.  The second column represents the relevance of
        /// stroke at which the second player scored.
        /// </para>
        /// </remarks>
        public Matrix<double> ByScorer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the relevance of stroke matrix by striking player.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of this property is a mx2 matrix.
        /// </para>
        /// <para>
        /// Each row (j*2) represents the relevance of the j'th stroke when the
        /// striking player scored.  Each row (j+2)+1 represents the relevance 
        /// of the j'th stroke when the striking player lost.
        /// </para>
        /// <para>
        /// The first column represents the relevance of a stroke of the first
        /// player.  The second column represents the relevance of strokes of 
        /// the second player.
        /// </para>
        /// </remarks>
        public Matrix<double> ByStriker
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a start vector for an iteration.
        /// </summary>
        /// <param name="elements">The number of elements</param>
        /// <returns>The start vector</returns>
        private static Vector<double> MakeStartVector(int elements)
        {
            var start = new SparseVector(elements);
            start[2] = 0.5;
            start[3] = 0.5;
            return start;
        }

        /// <summary>
        /// Computes the delta for the given probability.
        /// </summary>
        /// <param name="p">The probability</param>
        /// <returns>The delta</returns>
        private static double Delta(double p)
        {
            var d = 0.01 + (4 * 0.05 * p * (1 - p));
            return p < 0.5 ? d : -d;
        }

        /// <summary>
        /// Computes the relevance of stroke.
        /// </summary>
        /// <returns>The relevance of strokes</returns>
        private Matrix<double> ComputeRelevanceOfStroke()
        {
            var probabilities = this.Transitions.TransitionProbabilities;
            var relevance = new DenseMatrix(probabilities.RowCount - 2, 2);
            var start = MakeStartVector(probabilities.ColumnCount);

            for (int j = 2; j < probabilities.RowCount - 2; ++j)
            {
                // Compute the relevance for each row in the transition matrix. 
                // We can skip the zero rows, because there are no transitions 
                // to zero strokes anyway
                for (int k = 0; k < 2; ++k)
                {
                    // and for each win/loose combination. k==0 means the first
                    // player won, k==1 means the second player won.
                    var m = probabilities.Clone();

                    var p = m[j, m.ColumnCount - 2 + k];
                    var d = Delta(p);
                    for (int i = 0; i < m.ColumnCount; ++i)
                    {
                        if (m[j, i] != 0)
                        {
                            m[j, i] = m[j, i] - ((d * m[j, i]) / (1 - p));
                        }
                    }

                    m[j, m.ColumnCount - 2 + k] = p + d;

                    // For even rows, we take the winning probability of the first player,
                    // for odd rows, the probability of the second player.
                    var result = Markov.Simulate(m, start, this.Iterations)[m.ColumnCount - 2 + (j % 2)];
                    var pWin = this.WinningProbabilities[j % 2];
                    relevance[j, k] = result - pWin;
                }
            }

            return relevance;
        }
    }
}