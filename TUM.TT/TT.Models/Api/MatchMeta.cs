using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TT.Models.Util;
using System.Runtime.Serialization;

namespace TT.Models.Api
{
    [DataContract]
    public enum VideoStatus
    {
        None,
        Processing,
        Ready,
        Error
    }

    [DataContract]
    public class MatchMeta
    {
        [DataMember(Name = "_guid")]
        public Guid Guid { get; set; }
        [DataMember(Name = "tournament")]
        public string Tournament { get; set; }
        [DataMember(Name = "category")]
        public string Category { get; set; }
        [DataMember(Name = "round")]
        public string Round { get; set; }
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
        [IgnoreDataMember]
        public Boolean AnalysisFileStatus { get; set; }
        [IgnoreDataMember]
        public DateTime AnalysisFileUpdatedAt { get; set; }
        [IgnoreDataMember]
        public VideoStatus VideoStatus { get; set; }
        [IgnoreDataMember]
        public String VideoInfo { get; set; }
        [IgnoreDataMember]
        public string VideoUrl { get; set; }
        [IgnoreDataMember]
        public string PreviewUrl { get; set; }
        [IgnoreDataMember]
        public DateTime CreatedAt { get; set; }
        [IgnoreDataMember]
        public DateTime UpdatedAt { get; set; }
        [IgnoreDataMember]
        public DateTime LastOpenedAt { get; set; }
        [IgnoreDataMember]
        public String FileName { get; set; }
        [IgnoreDataMember]
        public String VideoFileName { get; set; }

        [DataMember(Name = "firstPlayer")]
        public PlayerMeta FirstPlayer { get; set; }
        [DataMember(Name = "secondPlayer")]
        public PlayerMeta SecondPlayer { get; set; }

        [IgnoreDataMember]
        public String ConvertedVideoFile { get; set; }
        [IgnoreDataMember]
        public String TempAnalysisFile { get; set; }
        [IgnoreDataMember]
        public object Thumbnail { get; set; }

        public string _id
        {
            get
            {
                return Guid.ToString();
            }
        }

        static public MatchMeta fromMatch(Match Match)
        {
            MatchMeta MatchMeta = new MatchMeta();
            MatchMeta.Guid = Match.ID;
            MatchMeta.Tournament = Match.Tournament;
            MatchMeta.Category = EnumExtensions.GetDescription<MatchCategory>(Match.Category);
            MatchMeta.Mode = EnumExtensions.GetDescription<MatchMode>(Match.Mode);
            MatchMeta.Round = EnumExtensions.GetDescription<MatchRound>(Match.Round);
            MatchMeta.Date = Match.DateTime;

            MatchMeta.FirstPlayer = PlayerMeta.fromPlayer(Match.FirstPlayer);
            MatchMeta.SecondPlayer = PlayerMeta.fromPlayer(Match.SecondPlayer);

            return MatchMeta;
        }
    }

    [DataContract]
    public class MatchMetaResult
    {
        [DataMember(Name = "rows")]
        public List<MatchMeta> rows { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }
}
