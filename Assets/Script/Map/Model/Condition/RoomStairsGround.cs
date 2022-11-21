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
		/// 部屋階段床
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoomStairsGround(CellData a_data, Point a_point, GameObject a_parent)
		{
			//階段床判定 既に螺旋階段判定を終えている前提
			if (a_data.m_road_no == 0) return false;
			if (a_data.m_down != ConnectType.CONNECT) return false;

			//部屋床配置
			CreateCellObject(ObjeType.ROAD_STAIRS_SPIRAL_GROUND, Direction.RIGHT, a_data, a_point, a_parent);

			return true;
		}
	}
}