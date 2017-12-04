namespace TT.Models.Util.Enums
{
    public static class Media
    {
        public enum Source
        {
            LiveScouter,
            RemoteScouter,
            Viewer,
            None
        }
        public enum Repeat
        {
            On,
            Off
        }
        public enum Infinite
        {
            On,
            Off
        }
        public enum Mute
        {
            Mute,
            Unmute
        }

        public enum Speed
        {
            Quarter = 25,
            Half = 50,
            Third = 33,
            Full = 100,
            None
        }

        public enum LiveScouterSpeed
        {
            Quarter = 25,
            Half = 50,
            Third = 33,
            Full = 100,
            Faster = 150,
            None
        }

        public enum Control
        {
            Previous,
            Next,
            Stop,
            Pause,
            Play,
            NextF,
            PrevF,
            Next5F,
            Prev5F,
            None
        }
        public enum Fullscreen
        {
            On,
            Off
        }
    }

    public static class Positions
    {
        public enum Table
        {
            TopLeft = 1,
            TopMid,
            TopRight,
            MidLeft,
            MidMid,
            MidRight,
            BotLeft,
            BotMid,
            BotRight,
            None
        }

        public enum Server
        {
            Left = 1,
            HalfLeft,
            Mid,
            HalfRight,
            Right,
            None
        }

        public enum Length
        {
            OverTheTable,
            AtTheTable,
            HalfDistance,
            None
        }

        public static Positions.Table GetTablePositionFromName(string name)
        {
            if (name.Contains("BotRight"))
                return Positions.Table.BotRight;
            else if (name.Contains("BotMid"))
                return Positions.Table.BotMid;
            else if (name.Contains("BotLeft"))
                return Positions.Table.BotLeft;
            else if (name.Contains("MidRight"))
                return Positions.Table.MidRight;
            else if (name.Contains("MidMid"))
                return Positions.Table.MidMid;
            else if (name.Contains("MidLeft"))
                return Positions.Table.MidLeft;
            else if (name.Contains("TopRight"))
                return Positions.Table.TopRight;
            else if (name.Contains("TopMid"))
                return Positions.Table.TopMid;
            else if (name.Contains("TopLeft"))
                return Positions.Table.TopLeft;
            else
                return Positions.Table.None;
        }

        public static Positions.Server GetServePositionFromName(string name)
        {
            if (name.Contains("HalfLeft"))
                return Positions.Server.HalfLeft;
            else if (name.Contains("HalfRight"))
                return Positions.Server.HalfRight;
            else if (name.Contains("Right"))
                return Positions.Server.Right;
            else if (name.Contains("Left"))
                return Positions.Server.Left;
            else if (name.Contains("Mid"))
                return Positions.Server.Mid;
            else
                return Positions.Server.None;
        }

        public static Positions.Length GetStrokeLengthFromName(string name)
        {
            if (name.Contains("Short"))
                return Positions.Length.OverTheTable;
            else if (name.Contains("Half"))
                return Positions.Length.AtTheTable;
            else if (name.Contains("Long"))
                return Positions.Length.HalfDistance;
            else
                return Positions.Length.None;
        }
    }

    public static class ViewMode
    {
        public enum Position
        {
            Top,
            Bottom
        }
    }

    public static class Stroke
    {
        public enum Player
        {
            Player1,
            Player2,
            None,
            Both
        }

        public enum Point
        {
            Player1,
            Player2,
            None,
            Both
        }
        public enum Success
        {
            Total,
            PointWon,
            DirectPointWon,
            PointLost
        }
        public enum Server
        {
            Player1,
            Player2,
            None,
            Both
        }

        public enum Crunch
        {
            CrunchTime,
            Not
        }
        public enum BeginningOfGame
        {
            BeginningOfGame,
            Not
        }
        public enum GamePhase
        {
            BeginningOfGame,
            CrunchTime,
            Not
        }

        public enum Specials
        {
            EdgeTable,
            EdgeNet,
            EdgeNetTable,
        }

        public enum Services
        {
            Pendulum,
            Reverse,
            Tomahawk,
            Special
        }

        public enum Spin
        {
            TS,
            SR,
            No,
            SL,
            US,
            TSSR,
            USSR,
            USSL,
            TSSL,
            Hidden
        }

        public enum Quality
        {
            Bad,
            Good,
            None,
            Both
        }
        public enum Aggressiveness
        {
            Aggressive,
            Control,
            Passive
        }
        public enum StepAround
        {
            StepAround,
            Not
        }
        public enum OpeningShot
        {
            OpeningShot,
            Not
        }
        public enum WinnerOrNetOut
        {
            Winner,
            NetOut,
            None,
            Both
        }

        public enum Course
        {
            Continue,
            Winner,
            NetOut,
            None
        }

        public enum Technique
        {
            Push,
            PushAggressive,
            Flip,
            Banana,
            Topspin,
            TopspinSpin,
            TopspinTempo,
            Block,
            BlockTempo,
            BlockChop,
            Counter,
            Smash,
            Lob,
            Tetra,
            Chop,
            Special,
            Miscellaneous
        }

        public enum TechniqueBasic
        {
            Push,
            Flip,
            Topspin,
            Block,
            Counter,
            Smash,
            Lob,
            Chop,
            Special,
            Miscellaneous
        }

        public enum PointOfContact
        {
            Over,
            Behind,
            HalfDistance,
            None
        }

        public enum Hand
        {
            Forehand,
            Backhand,
            None,
            Both
        }

        public enum ServiceWinner
        {
            All,
            Short,
            Long
        }
    }
}
