using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models
{
    [Flags]
    public enum CellType
    {
        None = 0,

        Start = 1 << 0,
        No_Event = 1 << 1,
        Supply = 1 << 2,
        Whirlpool = 1 << 3,
        Combat = 1 << 4,
        Boss = 1 << 5,
        気のせい = 1 << 6,  //Frameでは気のせい変更前(赤)
        Air_Battle = 1 << 7,
        HomePort = 1 << 8,
        Air_Recon = 1 << 9,

        Night_Battle = 1 << 31,
    }

    public static class CellTypeExtensions
    {
        public static CellType ToCellType(this int colorNo)
        {
            return (CellType)(1 << colorNo);
        }

        public static CellType ToCellType(this string battleType)
        {
            return battleType.Contains("sp_midnight") ? CellType.Night_Battle
                : battleType.Contains("airbattle") ? CellType.Air_Battle
                : CellType.None;
        }

        public static CellType GetCellType(this MapCell cell, IReadOnlyDictionary<MapCell, CellType> knownTypes)
        {
            var result = CellType.None;
            if (knownTypes.ContainsKey(cell)) result = result | knownTypes[cell];
            var cellMaster = Repositories.Master.Current.MapCells[cell.Id];
            result = result | cellMaster.ColorNo.ToCellType();
            return result;
        }
    }
}
