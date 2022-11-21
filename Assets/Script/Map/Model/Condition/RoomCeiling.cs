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
		/// 部屋天井
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoomCeiling(CellData a_data, Point a_point, GameObject a_parent)
		{
			//部屋天井
			//if (a_data.state != Map.Cell.CellType.TILE) return false;
			if (a_data.m_room_area == 0) return false;
			if (a_data.m_up == ConnectType.CONNECT) return false;

			// 上のセルデータ
			var t_next_cell_point = a_point + Point.PointCorrection(Direction.UP);
			//接続先判定
			if (Map.Param.CommonParams.m_map_area.IsAreaIn(t_next_cell_point) == false) return false;

			//接続先セルデータ
			var t_next_cell_data = Map.Param.CommonParams.GetCellData(t_next_cell_point);
			if (t_next_cell_data.m_room_area == a_data.m_room_area) return false;

			var t_obj = CreateCellObject(ObjeType.ROOM_CELLING, Map.Direction.RIGHT, a_data, a_point, a_parent);

			return true;
		}
	}
}