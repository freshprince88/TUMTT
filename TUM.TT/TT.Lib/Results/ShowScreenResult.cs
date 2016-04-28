using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TT.Lib.Results
{
    public class ShowScreenResult : IResult
    {
        readonly Type screenType;
        readonly string name;

        public Dictionary<string, object> Properties { get; set; }

        public IShell Shell { get; set; }

        public ShowScreenResult(string name)
        {
            this.name = name;
            Properties = new Dictionary<string, object>();
        }

        public ShowScreenResult(Type screenType)
        {
            this.screenType = screenType;
            Properties = new Dictionary<string, object>();
        }

        public void Execute(CoroutineExecutionContext context)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            foreach(string key in Properties.Keys)
            {
                PropertyInfo propertyInfo = screen.GetType().GetProperty(key);
                propertyInfo.SetValue(screen, Convert.ChangeType(Properties[key], propertyInfo.PropertyType), null);
            }

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
