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
        /// 道直線階段上
        /// </summary>
        /// <param name="a_data">セルデータ</param>
        /// <param name="a_point">座標</param>
        /// <param name="a_parent">配置する親オブジェクト</param>
        private bool CreateRoadStairsStraightTop(CellData a_data, Point a_point, GameObject a_parent)
        {
            //直線階段判定
            if (a_data.state != Map.Cell.CellType.TILE) return false;
			if (a_data.m_road_no == 0) return false;
			if (a_data.m_room_area != 0) return false;
            if (a_data.m_down != ConnectType.CONNECT) return false;
            if (a_data.m_up == ConnectType.CONNECT) return false;
            
            //下の直線階段判定
            var t_data = Map.Param.CommonParams.GetCellData(a_point + Point.down);
			if (t_data.m_road_no == 0) return false;
			if (t_data.m_room_area != 0) return false;
            if (t_data.m_down == ConnectType.CONNECT) return false;
            if (t_data.m_up != ConnectType.CONNECT) return false;
            

            var t_count = 0;
            var t_connect_direction_up = Map.Direction.MAX_NUM;

            if (a_data.m_right == ConnectType.CONNECT)
            {
                t_connect_direction_up = Map.Direction.RIGHT;
                t_count++;
            }
            if (a_data.m_left == ConnectType.CONNECT)
            {
                t_connect_direction_up = Map.Direction.LEFT;
                t_count++;
            }if (a_data.m_front == ConnectType.CONNECT)
            {
                t_connect_direction_up = Map.Direction.FRONT;
                t_count++;
            }if (a_data.m_back == ConnectType.CONNECT)
            {
                t_connect_direction_up = Map.Direction.BACK;
                t_count++;
            }

            if (t_count != 1) return false;

            t_count = 0;
            var t_connect_direction_down = Map.Direction.MAX_NUM;

            if (t_data.m_right == ConnectType.CONNECT)
            {
                t_connect_direction_down = Map.Direction.RIGHT;
                t_count++;
            }
            if (t_data.m_left == ConnectType.CONNECT)
            {
                t_connect_direction_down = Map.Direction.LEFT;
                t_count++;
            }if (t_data.m_front == ConnectType.CONNECT)
            {
                t_connect_direction_down = Map.Direction.FRONT;
                t_count++;
            }if (t_data.m_back == ConnectType.CONNECT)
            {
                t_connect_direction_down = Map.Direction.BACK;
                t_count++;
            }

            if (t_count != 1) return false;

			//上下の接続方向チェック
			var t_ret = false;
            switch (t_connect_direction_down)
            {
                case Map.Direction.RIGHT:
                    if (t_connect_direction_up == Map.Direction.LEFT) t_ret = true;
                    break;
                case Map.Direction.LEFT:
                    if (t_connect_direction_up == Map.Direction.RIGHT) t_ret = true;
                    break;
                case Map.Direction.FRONT:
                    if (t_connect_direction_up == Map.Direction.BACK) t_ret = true;
                    break;
                case Map.Direction.BACK:
                    if (t_connect_direction_up == Map.Direction.FRONT) t_ret = true;
                    break;
            }

            if (t_ret == false) return false;
			
			var t_obj = CreateCellObject(ObjeType.ROAD_STAIRS_STRAIGHT_TOP, t_connect_direction_up, a_data, a_point, a_parent);

			return true;
        }
	}
}