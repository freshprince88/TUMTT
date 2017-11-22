namespace TT.Lib.Events
{
    public class DrawLineEvent
    {

        public System.Windows.Shapes.Line Line { get; set; }

        public DrawLineEvent(System.Windows.Shapes.Line line)
        {
            Line = line;
        }

    }
}