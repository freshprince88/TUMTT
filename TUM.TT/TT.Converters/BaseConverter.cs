using System;
using System.Windows.Markup;

namespace TT.Converters
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public string ReplaceExpression(string expression)
        {
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
