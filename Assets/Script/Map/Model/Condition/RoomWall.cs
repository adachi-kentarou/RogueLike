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
		/// 部屋の壁
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoomWall(CellData a_data, Point a_point, GameObject a_parent)
		{

			
			//部屋判定
			//if (a_data.state == Map.Cell.CellType.NONE) return false;
			//if (a_data.m_road_no == 0) return false;
			if (a_data.m_room_area == 0) return false;

			var t_ret = false;
			//四方をチェック
			for (int ii = (int)Map.Direction.LEFT; ii < (int)Map.Direction.MAX_NUM; ii++)
			{
				var t_direction = (Map.Direction)ii;

				// 接続判定
				//if (a_data.GetConnect(t_direction) == Map.Cell.ConnectType.CONNECT)
				{
					//if (a_data.state == Map.Cell.CellType.NONE) continue;
					//if (a_data.m_room_area == 0) continue;

					// 隣のセルデータ
					var t_next_cell_point = a_point + Point.PointCorrection(t_direction);

					//接続先判定
					if (Map.Param.CommonParams.m_map_area.IsAreaIn(t_next_cell_point) == true)
					{
						//接続先セルデータ
						var t_next_cell_data = Map.Param.CommonParams.GetCellData(t_next_cell_point);
						if (t_next_cell_data.m_room_area != a_data.m_room_area)
						{
							t_ret = true;
							var t_obj = CreateCellObject(ObjeType.ROOM_WALL, t_direction, a_data, a_point, a_parent);

							//部屋の色
							var t_room_wall = t_obj as Map.Model.Cell.RoomWall;
							var t_color = m_ColorList[a_data.m_room_area - 1] + new Color(0.5f,0.5f,0.5f) / 2f;
							t_room_wall.SetWallColor(t_color);
						}
					}
					else
					{
						//エリア外　=　壁
						t_ret = true;
						var t_obj = CreateCellObject(ObjeType.ROOM_WALL, t_direction, a_data, a_point, a_parent);
						
						//部屋の色
						var t_room_wall = t_obj as Map.Model.Cell.RoomWall;
						var t_color = m_ColorList[a_data.m_room_area - 1] + new Color(0.5f, 0.5f, 0.5f) / 2f;
						t_room_wall.SetWallColor(t_color);
					}
				}
			}

			return t_ret;
		}
	}
}