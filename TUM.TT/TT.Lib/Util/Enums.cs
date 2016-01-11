using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Util.Enums
{
    public static class Media
    {
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

        public enum Control
        {
            Previous,
            Next,
            Stop,
            Pause,
            Play,
            None
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
            Short,
            Half,
            Long,
            None
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

        public enum Point
        {
            Player1,
            Player2,
            None,
            Both
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

        public enum Specials
        {
            EdgeTable,
            EdgeRacket,
            None,
            Both
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
            ÜS,
            SR,
            No,
            SL,
            US,
            ÜSSR,
            USSR,
            USSL,
            ÜSSL,
            Hidden
        }

        public enum Quality
        {
            Bad,
            Good,
            None,
            Both
        }
        public enum StepAround
        {
            StepAround,
            Not
        }
        public enum WinnerOrNetOut
        {
            Winner,
            NetOut,
            None,
            Both
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
            Chop,
            Special
        }

        public enum Hand
        {
            Fore,
            Back,
            None,
            Both
        }
    }
}
