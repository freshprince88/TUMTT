//-----------------------------------------------------------------------
// <copyright file="PlotStyle.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Plots
{
    using OxyPlot;

    /// <summary>
    /// Styles for plots.
    /// </summary>
    public class PlotStyle
    {
        /// <summary>
        /// Gets or sets the text font.
        /// </summary>
        public string TextFont { get; set; }

        /// <summary>
        /// Gets or sets the text font size.
        /// </summary>
        public double TextSize { get; set; }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public OxyColor BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the color for grid lines.
        /// </summary>
        public OxyColor GridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the color for the first player.
        /// </summary>
        public OxyColor FirstPlayerColor { get; set; }

        /// <summary>
        /// Gets or sets the color for the second player.
        /// </summary>
        public OxyColor SecondPlayerColor { get; set; }

        /// <summary>
        /// Creates a new plot with this style.
        /// </summary>
        /// <returns>The plot</returns>
        public PlotModel CreatePlot()
        {
            return new PlotModel()
            {
                LegendBorderThickness = 0,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendFont = this.TextFont,
                LegendFontSize = this.TextSize,
                LegendItemAlignment = HorizontalAlignment.Center,
                LegendMargin = 4,
                LegendPadding = 0,
                TextColor = this.TextColor,
                DefaultFont = this.TextFont,
                DefaultFontSize = this.TextSize,
                TitleFont = this.TextFont,
                TitleFontSize = this.TextSize,
                PlotAreaBorderColor = this.BorderColor,
            };
        }
    }
}
