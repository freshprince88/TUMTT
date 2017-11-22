namespace TT.Lib.Events
{
    public class DeactivationEvent
    {
        public bool Close { get; set; }

        public DeactivationEvent()
        {
            Close = false;
        }

        public DeactivationEvent(bool close)
        {
            Close = close;
        }

    }
}
