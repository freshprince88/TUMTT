namespace TT.Lib.Events
{
    public class FollowMouseEvent
    {

        public System.Windows.Point LastPosition { get; set; }

        public FollowMouseEvent(System.Windows.Point lastPosition)
        {
            LastPosition = lastPosition;
        }

    }
}