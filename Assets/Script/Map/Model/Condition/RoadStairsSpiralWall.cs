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
		/// 通路螺旋階段壁
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoadStairsSpiralWall(CellData a_data, Point a_point, GameObject a_parent)
		{
			//螺旋階段壁判定 既に螺旋階段判定を終えている前提
			
			var t_ret = true;
			//四方をチェック
			for (int ii = (int)Map.Direction.LEFT; ii < (int)Map.Direction.MAX_NUM; ii++)
			{
				var t_direction = (Map.Direction)ii;

				//螺旋階段部屋は四方を部屋壁にする
				//var t_obj = CreateCellObject(ObjeType.ROOM_WALL, t_direction, a_data, a_point, a_parent);

				// 接続判定
				if (a_data.GetConnect(t_direction) == ConnectType.CONNECT)
				{
					t_ret = false;

				}

			}

			if (t_ret == true && a_data.m_down == ConnectType.CONNECT)
			{
				//接続ないため螺旋階段用壁配置
				CreateCellObject(ObjeType.ROAD_STAIRS_SPIRAL_WALL, Direction.RIGHT, a_data, a_point, a_parent);
			}

			return t_ret && a_data.m_down == ConnectType.CONNECT;
		}
	}
}