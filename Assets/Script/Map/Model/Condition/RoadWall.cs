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
        /// 通路の壁
        /// </summary>
        /// <param name="a_data">セルデータ</param>
        /// <param name="a_point">座標</param>
        /// <param name="a_parent">配置する親オブジェクト</param>
        private bool CreateRoadWall(CellData a_data, Point a_point, GameObject a_parent)
        {
            //道判定
            if (a_data.state == Map.Cell.CellType.NONE) return false;
            if (a_data.m_road_no == 0) return false;
			if (a_data.m_room_area != 0) return false;
			if (a_data.m_up == ConnectType.CONNECT || a_data.m_down == ConnectType.CONNECT) return false;

            //四方をチェック
            for (int ii = (int)Map.Direction.LEFT; ii < (int)Map.Direction.MAX_NUM; ii++)
            {
                var t_direction = (Map.Direction)ii;

                // 接続判定
                //if (a_data.GetConnect(t_direction) == Map.Cell.ConnectType.CONNECT)
                {
                    var t_obj = CreateCellObject(ObjeType.ROAD_WALL, t_direction, a_data, a_point, a_parent);
                    
                }
            }

			return true;
        }
	}
}