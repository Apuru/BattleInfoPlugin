using System.Runtime.Serialization;

namespace BattleInfoPlugin.Models
{
    [DataContract]
    public enum Formation
    {
        [EnumMember]
        Unknown = -1, 
        [EnumMember]
        なし = 0,
        [EnumMember]
        Line_Ahead = 1, 
        [EnumMember]
        Double_Line = 2, 
        [EnumMember]
        Diamond = 3,
        [EnumMember]
        Echelon = 4,
        [EnumMember]
        Line_Abreast = 5,
        [EnumMember]
        CF1_AntiSub = 11, // 対潜警戒
        [EnumMember]
        CF2_Forward = 12, // 前方警戒
        [EnumMember]
        CF3_AntiAir = 13, // 輪形陣
        [EnumMember]
        CF4_Battle = 14, // 戦闘隊形
    }
}