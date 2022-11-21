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
		/// 通路螺旋階段
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoadStairsSpiral(CellData a_data, Point a_point, GameObject a_parent)
		{
			//螺旋階段判定 既に直進階段判定を終えている前提
			if (a_data.state == Map.Cell.CellType.NONE) return false;
			if (a_data.m_road_no == 0) return false;
			if (a_data.m_room_area != 0) return false;

			if (a_data.m_up != ConnectType.CONNECT) return false;

			//上に繋がっているので階段を配置
			CreateCellObject(ObjeType.ROAD_STAIRS_SPIRAL, Direction.RIGHT, a_data, a_point, a_parent);
			
			return true;
		}
	}
}