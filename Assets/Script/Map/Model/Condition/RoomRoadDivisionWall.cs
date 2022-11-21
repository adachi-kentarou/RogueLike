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
		/// 部屋の通路仕切り壁
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoomRoadDivisionWall(CellData a_data, Point a_point, GameObject a_parent)
		{
			if (a_data.m_room_area == 0) return false;
			if (a_data.state == CellType.NONE) return false;

			var t_ret = false;
			//四方をチェック
			for (int ii = (int)Map.Direction.LEFT; ii < (int)Map.Direction.MAX_NUM; ii++)
			{
				var t_direction = (Map.Direction)ii;
				// 隣のセルデータ
				var t_next_cell_point = a_point + Point.PointCorrection(t_direction);

				t_ret = true;
				var t_obj = CreateCellObject(ObjeType.ROOM_ROAD_DIVISION_WALL, t_direction, a_data, a_point, a_parent);
			}

			return t_ret;
		}
	}
}