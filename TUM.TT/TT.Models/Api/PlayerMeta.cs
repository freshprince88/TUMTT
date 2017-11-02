using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using TT.Models.Util;

namespace TT.Models.Api
{
    [DataContract]
    public class PlayerMeta
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "nationality")]
        public string Nationality { get; set; }
        [DataMember(Name = "playingStyle")]
        public string PlayingStyle { get; set; }
        [DataMember(Name = "playingStyle")]
        public string Material { get; set; }
        [DataMember(Name = "handedness")]
        public string Handedness { get; set; }
        [DataMember(Name = "grip")]
        public string Grip { get; set; }

        static public PlayerMeta fromPlayer(Player Player)
        {
            PlayerMeta PlayerMeta = new PlayerMeta();
            PlayerMeta.Name = Player.Name;
            PlayerMeta.Nationality = Player.Nationality;
            PlayerMeta.PlayingStyle = EnumExtensions.GetDescription<PlayingStyle>(Player.PlayingStyle);
            PlayerMeta.Material = Player.Material;
            PlayerMeta.Handedness = EnumExtensions.GetDescription<Handedness>(Player.Handedness);
            PlayerMeta.Grip = EnumExtensions.GetDescription<Grip>(Player.Grip);

            return PlayerMeta;
        }
    }
}
