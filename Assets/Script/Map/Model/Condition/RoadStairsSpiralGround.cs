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
		/// 通路螺旋階段床
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_point">座標</param>
		/// <param name="a_parent">配置する親オブジェクト</param>
		private bool CreateRoadStairsSpiralGround(CellData a_data, Point a_point, GameObject a_parent)
		{
			//螺旋階段壁判定 既に螺旋階段と螺旋階段壁判定を終えている前提

			//接続あるので部屋壁配置
			for (int ii = (int)Map.Direction.LEFT; ii < (int)Map.Direction.MAX_NUM; ii++)
			{
				var t_direction = (Map.Direction)ii;

				//螺旋階段部屋は四方を部屋壁にする
				CreateCellObject(ObjeType.ROOM_WALL, t_direction, a_data, a_point, a_parent);
			}

			//螺旋階段用天井
			CreateCellObject(ObjeType.ROAD_STAIRS_SPIRAL_CELLING, Map.Direction.RIGHT, a_data, a_point, a_parent);



			if (a_data.m_down == ConnectType.CONNECT)
			{
				//螺旋階段用床配置
				CreateCellObject(ObjeType.ROAD_STAIRS_SPIRAL_GROUND, Direction.RIGHT, a_data, a_point, a_parent);
			}
			else
			{
				//部屋床配置
				CreateCellObject(ObjeType.ROOM_GROUND, Direction.RIGHT, a_data, a_point, a_parent);
			}

			return true;
		}
	}
}