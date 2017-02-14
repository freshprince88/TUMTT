using System;
using System.Windows.Markup;
using TT.Models;

namespace TT.Converters
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public string ReplaceExpression(string expression, object param = null, MatchPlayer? player = null, int? strokeNumber = null)
        {
            expression = expression.Replace("$$Player$$", player != null ? param.ToString() : "");
            if (player != null)
                expression = expression.Replace("$$PlayerString$$", player.Value.ToString());
            if (strokeNumber != null)
            {
                expression = expression.Replace("$$StrokeNumber$$", (strokeNumber.Value - 1).ToString());
                expression = expression.Replace("$$EqualsOrNot$$", strokeNumber % 2 == 0 ? "==" : "!=");
            }
            expression = expression.Replace('\'', '"');
            expression = expression.Replace("MatchPlayer.None", "\"None\"");
            expression = expression.Replace("MatchPlayer.First", "\"First\"");
            expression = expression.Replace("MatchPlayer.Second", "\"Second\"");
            expression = expression.Replace(".Winner", ".Winner.ToString()");
            expression = expression.Replace(".Player", ".Player.ToString()");
            expression = expression.Replace(".Server", ".Server.ToString()");

            return expression;

        }
    }
}
