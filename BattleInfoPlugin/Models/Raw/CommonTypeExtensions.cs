﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models.Raw
{
    public static class CommonTypeExtensions
    {
        private static readonly FleetDamages defaultValue = new FleetDamages();

        #region 支援

        public static FleetDamages GetEnemyDamages(this Api_Support_Info support)
            => support?.api_support_airatack?.api_stage3?.api_edam?.GetDamages()
               ?? support?.api_support_hourai?.api_damage?.GetDamages()
               ?? defaultValue;

        #endregion

        #region 砲撃

        public static FleetDamages GetFriendDamages(this Hougeki hougeki)
            => hougeki?.api_damage?.GetFriendDamages(hougeki.api_df_list)
               ?? defaultValue;

        public static FleetDamages GetEnemyDamages(this Hougeki hougeki)
            => hougeki?.api_damage?.GetEnemyDamages(hougeki.api_df_list)
               ?? defaultValue;

        #endregion

        #region 夜戦

        public static FleetDamages GetFriendDamages(this Midnight_Hougeki hougeki)
            => hougeki?.api_damage?.GetFriendDamages(hougeki.api_df_list)
               ?? defaultValue;

        public static FleetDamages GetEnemyDamages(this Midnight_Hougeki hougeki)
            => hougeki?.api_damage?.GetEnemyDamages(hougeki.api_df_list)
               ?? defaultValue;

        #endregion

        #region 航空戦

        public static FleetDamages GetFirstFleetDamages(this Api_Kouku kouku)
            => kouku?.api_stage3?.api_fdam.GetDamages()
               ?? defaultValue;

        public static FleetDamages GetSecondFleetDamages(this Api_Kouku kouku)
            => kouku?.api_stage3_combined?.api_fdam?.GetDamages()
               ?? defaultValue;

        public static FleetDamages GetEnemyDamages(this Api_Kouku kouku)
            => kouku?.api_stage3?.api_edam?.GetDamages()
               ?? defaultValue;

        public static AirSupremacy GetAirSupremacy(this Api_Kouku kouku)
            => (AirSupremacy)(kouku?.api_stage1?.api_disp_seiku ?? (int)AirSupremacy.航空戦なし);

        public static AirCombatResult[] ToResult(this Api_Kouku kouku, string prefixName = "")
        {
            return kouku != null
                ? new []
                {
                    kouku.api_stage1.ToResult($"{prefixName}空対空"),
                    kouku.api_stage2.ToResult($"{prefixName}空対艦")
                }
                : new AirCombatResult[0];
        }

        public static AirCombatResult ToResult(this Api_Stage1 stage1, string name)
            => stage1 == null ? new AirCombatResult(name)
            : new AirCombatResult(name, stage1.api_f_count, stage1.api_f_lostcount, stage1.api_e_count, stage1.api_e_lostcount);

        public static AirCombatResult ToResult(this Api_Stage2 stage2, string name)
            => stage2 == null ? new AirCombatResult(name)
            : new AirCombatResult(name, stage2.api_f_count, stage2.api_f_lostcount, stage2.api_e_count, stage2.api_e_lostcount);

        #endregion

        #region 雷撃戦

        public static FleetDamages GetFriendDamages(this Raigeki raigeki)
            => raigeki?.api_fdam?.GetDamages()
               ?? defaultValue;

        public static FleetDamages GetEnemyDamages(this Raigeki raigeki)
            => raigeki?.api_edam?.GetDamages()
               ?? defaultValue;

        #endregion

        #region ダメージ計算

        /// <summary>
        /// 12項目中先頭6項目取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="origin">ゴミ-1が付いてる場合1オリジン</param>
        /// <returns></returns>
        public static IEnumerable<T> GetFriendData<T>(this IEnumerable<T> source, int origin = 1)
            => source.Skip(origin).Take(6);

        /// <summary>
        /// 12項目中末尾6項目取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="origin">ゴミ-1が付いてる場合1オリジン</param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnemyData<T>(this IEnumerable<T> source, int origin = 1)
            => source.Skip(origin + 6).Take(6);

        /// <summary>
        /// 雷撃・航空戦ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_fdam/api_edam</param>
        /// <returns></returns>
        public static FleetDamages GetDamages(this double[] damages)
            => damages
                .GetFriendData() //敵味方共通
                .Select(Convert.ToInt32)
                .ToArray()
                .ToFleetDamages();

        #region 砲撃戦ダメージリスト算出

        /// <summary>
        /// 砲撃戦友軍ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_damage</param>
        /// <param name="df_list">api_df_list</param>
        /// <returns></returns>
        public static FleetDamages GetFriendDamages(this object[] damages, object[] df_list)
            => damages
                .ToIntArray()
                .ToSortedDamages(df_list.ToIntArray())
                .GetFriendData(0)
                .ToFleetDamages();

        /// <summary>
        /// 砲撃戦敵軍ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_damage</param>
        /// <param name="df_list">api_df_list</param>
        /// <returns></returns>
        public static FleetDamages GetEnemyDamages(this object[] damages, object[] df_list)
            => damages
                .ToIntArray()
                .ToSortedDamages(df_list.ToIntArray())
                .GetEnemyData(0)
                .ToFleetDamages();

        /// <summary>
        /// 砲撃戦ダメージリストint配列化
        /// 弾着観測射撃データはフラット化する
        /// api_df_listも同様の型なので流用可能
        /// </summary>
        /// <param name="damages">api_damage</param>
        /// <returns></returns>
        private static int[] ToIntArray(this object[] damages)
            => damages
                .Where(x => x is Array)
                .Select(x => ((Array) x).Cast<object>())
                .SelectMany(x => x.Select(Convert.ToInt32))
                .ToArray();

        /// <summary>
        /// フラット化したapi_damageとapi_df_listを元に
        /// 自軍6隻＋敵軍6隻の長さ12のダメージ合計配列を作成
        /// </summary>
        /// <param name="damages">api_damage</param>
        /// <param name="dfList">api_df_list</param>
        /// <returns></returns>
        private static int[] ToSortedDamages(this int[] damages, int[] dfList)
        {
            var zip = damages.Zip(dfList, (da, df) => new {df, da});
            var ret = new int[12];
            foreach (var d in zip.Where(d => 0 < d.df))
            {
                ret[d.df - 1] += d.da;
            }
            return ret;
        }

        #endregion

        #endregion
    }
}
