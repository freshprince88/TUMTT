﻿using System;
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
        [DataMember(Name = "material")]
        public string Material { get; set; }
        [DataMember(Name = "materialBackhand")]
        public string MaterialBackhand { get; set; }
        [DataMember(Name = "materialForehand")]
        public string MaterialForehand { get; set; }
        [DataMember(Name = "handedness")]
        public string Handedness { get; set; }
        [DataMember(Name = "grip")]
        public string Grip { get; set; }

        static public PlayerMeta FromPlayer(Player Player)
        {
            PlayerMeta PlayerMeta = new PlayerMeta()
            {
                Name = Player.Name,
                Nationality = Player.Nationality,
                PlayingStyle = Enum.GetName(typeof(PlayingStyle), Player.PlayingStyle),
                Material = Player.Material,
                MaterialBackhand = Enum.GetName(typeof(MaterialBH), Player.MaterialBH),
                MaterialForehand = Enum.GetName(typeof(MaterialFH), Player.MaterialFH),
                Handedness = Enum.GetName(typeof(Handedness), Player.Handedness),
                Grip = Enum.GetName(typeof(Grip), Player.Grip)
            };
            return PlayerMeta;
        }
    }
}