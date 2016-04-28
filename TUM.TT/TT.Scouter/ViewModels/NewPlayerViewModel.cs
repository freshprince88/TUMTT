using Caliburn.Micro;
using System.Collections.Generic;
using TT.Models;
using TT.Models.Results;

namespace TT.Scouter.ViewModels
{
    public class NewPlayerViewModel : Screen
    {
        public Player Player { get; set; }

        public IEnumerable<IResult> SubmitPlayer()
        {
            var nextScreen = ShowScreenResult.Of<NewMatchViewModel>();
            yield return nextScreen;
        }

    }
}
