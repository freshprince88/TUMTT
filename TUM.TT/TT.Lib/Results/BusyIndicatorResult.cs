//-----------------------------------------------------------------------
// <copyright file="BusyIndicatorResult.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Results
{
    using System;
    using System.Linq;
    using System.Windows;
    using Caliburn.Micro;
    using TT.Models.Util;
    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Marks the application as busy.
    /// </summary>
    public class BusyIndicatorResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyIndicatorResult"/> class.
        /// </summary>
        /// <param name="inner">The inner result.</param>
        public BusyIndicatorResult(IResult inner)
        {
            this.Inner = inner;
        }

        /// <summary>
        /// Notifies about the completion of this action.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        /// <summary>
        /// Gets or sets a message informing about the action being conducted.
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        /// Gets the inner result.
        /// </summary>
        public IResult Inner { get; private set; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(CoroutineExecutionContext context)
        {
            var view = context.View as FrameworkElement;
            if (view != null)
            {                
                var indicator = view.TraverseParents()
                   .OfType<BusyIndicator>()
                   .FirstOrDefault();

                indicator = indicator ?? LogicalTreeHelper.GetChildren(view).OfType<BusyIndicator>().FirstOrDefault();

                if (indicator != null)
                {
                    Caliburn.Micro.Execute.OnUIThread(() =>
                    {
                        indicator.BusyContent = this.Message;
                        indicator.IsBusy = true;
                    });
                    Coroutine.BeginExecute(
                        this.Inner.AsCoroutine().GetEnumerator(),
                        context,
                        (sender, args) =>
                        {
                            Caliburn.Micro.Execute.OnUIThread(() =>
                            {
                                indicator.IsBusy = false;
                                this.Completed(this, args);
                            });
                        });
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Could not find BusyIndicator for {0}", view));
                }
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Cannot determine view for {0}", context.Target));
            }
        }
    }
}
