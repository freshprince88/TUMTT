using Caliburn.Micro;
using System;

namespace TT.Lib.Results
{
    public class ShowScreenResult : IResult
    {
        readonly Type screenType;
        readonly string name;

        public IShell Shell { get; set; }

        public ShowScreenResult(string name)
        {
            this.name = name;
        }

        public ShowScreenResult(Type screenType)
        {
            this.screenType = screenType;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            Shell.ActivateItem(screen);
            Completed(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public static ShowScreenResult Of<T>()
        {
            return new ShowScreenResult(typeof(T));
        }
    }
}
