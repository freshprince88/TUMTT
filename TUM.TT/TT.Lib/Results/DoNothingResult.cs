using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Results
{
    public class DoNothingResult : IResult
    {
        public event EventHandler<ResultCompletionEventArgs> Completed;

        public void Execute(CoroutineExecutionContext context)
        {
            var args = new ResultCompletionEventArgs()
            {
                WasCancelled = false
            };
            this.Completed(this, args);
        }
    }
}
