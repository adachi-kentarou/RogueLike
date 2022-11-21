using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Map.Cell;
using Map.Model;

/// <summary>
/// マップオブジェクト判定
/// </summary>
namespace Map.Model
{
    /// <summary>
    /// マップのオブジェクト条件判定用分離クラス
    /// </summary>
    partial class MapModel
	{
		/// <summary>
        /// 道床
        /// </summary>
        /// <param name="a_data">セルデータ</param>
        /// <param name="a_point">座標</param>
        /// <param name="a_parent">配置する親オブジェクト</param>
        private bool CreateRoadGround(CellData a_data, Point a_point, GameObject a_parent)
        {
            //床判定
            if (a_data.state != Map.Cell.CellType.TILE) return false;
			if (a_data.m_road_no == 0) return false;
			if (a_data.m_room_area != 0) return false;
			if (a_data.m_down == ConnectType.CONNECT) return false;
            if (a_data.m_up == ConnectType.CONNECT) return false;
            
            var t_obj = CreateCellObject(ObjeType.ROAD_GROUND, Map.Direction.RIGHT, a_data, a_point, a_parent);

			return true;
		}
	}
}