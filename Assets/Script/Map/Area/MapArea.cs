using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Cell;


namespace Map.Area
{
    /// <summary>
    /// マップエリア処理に関するクラス
    /// </summary>
    public class MapArea
    {
        public int m_area_min_x;
        public int m_area_max_x;
        public int m_area_min_y;
        public int m_area_max_y;
        public int m_area_min_z;
        public int m_area_max_z;
        
        /// <summary>
        /// 指定座標エリア判定
        /// </summary>
        /// <param name="a_x">x座標</param>
        /// <param name="a_y">y座標</param>
        /// <param name="a_z">x座標</param>
        /// <returns>true: エリア内 false: エリア外</returns>
        public bool IsAreaIn(int a_x, int a_y, int a_z)
        {
            return a_x >= m_area_min_x &&
                   a_y >= m_area_min_y &&
                   a_z >= m_area_min_z &&
                   a_x < m_area_max_x &&
                   a_y < m_area_max_y &&
                   a_z < m_area_max_z;
        }

        /// <summary>
        /// 指定座標エリア判定
        /// </summary>
        /// <param name="a_pos">ポイント座標オブジェクト</param>
        /// <returns>true: エリア内 false: エリア外</returns>
        public bool IsAreaIn(Point a_pos)
		{
			return a_pos.x >= m_area_min_x &&
                   a_pos.y >= m_area_min_y &&
                   a_pos.z >= m_area_min_z &&
                   a_pos.x < m_area_max_x &&
                   a_pos.y < m_area_max_y &&
                   a_pos.z < m_area_max_z;
        }

        /// <summary>
        /// エリア範囲設定
        /// </summary>
        /// <param name="a_min_x">x座標最少値</param>
        /// <param name="a_max_x">x座標最大値</param>
        /// <param name="a_min_y">y座標最少値</param>
        /// <param name="a_max_y">y座標最大値</param>
        /// <param name="a_min_z">z座標最少値</param>
        /// <param name="a_max_z">z座標最大値</param>
        public void SetAreaIn(int a_min_x, int a_max_x, int a_min_y, int a_max_y, int a_min_z, int a_max_z)
        {
			m_area_min_x = a_min_x;
            m_area_max_x = a_max_x;
            m_area_min_y = a_min_y;
            m_area_max_y = a_max_y;
            m_area_min_z = a_min_z;
            m_area_max_z = a_max_z;
        }

        /// <summary>
        /// エリア座標設定
        /// </summary>
        /// <param name="a_min">ポイント座標オブジェクト最小値</param>
        /// <param name="a_max">ポイント座標オブジェクト最大値</param>
        public void SetAreaIn(Point a_min, Point a_max)
        {
			m_area_min_x = a_min.x;
            m_area_max_x = a_max.x;
            m_area_min_y = a_min.y;
            m_area_max_y = a_max.y;
            m_area_min_z = a_min.z;
            m_area_max_z = a_max.z;
        }

        /// <summary>
        /// 指定された部屋のエリア同士が重なるかどうか判定
        /// </summary>
        /// <returns>true:重なっている false:重なっていない</returns>
        public static bool IsOverLap(Map.Room.RoomData a_a_room, Map.Room.RoomData a_b_room)
        {
            return IsOverLap(a_a_room.m_area_start, a_a_room.m_area_end, a_b_room.m_area_start, a_b_room.m_area_end);
        }

        /// <summary>
        /// 指定されたポイントのエリア同士が重なるかどうか判定
        /// </summary>
        /// <returns>true:重なっている false:重なっていない</returns>
        public static bool IsOverLap(Point a_a_start_point, Point a_a_end_point, Point a_b_start_point, Point a_b_end_point)
        {
            return IsOverLap(a_a_start_point.x, a_a_end_point.x, a_a_start_point.y, a_a_end_point.y, a_a_start_point.z, a_a_end_point.z,
                a_b_start_point.x, a_b_end_point.x, a_b_start_point.y, a_b_end_point.y, a_b_start_point.z, a_b_end_point.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_a_start_point_x"></param>
        /// <param name="a_a_end_point_x"></param>
        /// <param name="a_a_start_point_y"></param>
        /// <param name="a_a_end_point_y"></param>
        /// <param name="a_a_start_point_z"></param>
        /// <param name="a_a_end_point_z"></param>
        /// <param name="a_b_start_point_x"></param>
        /// <param name="a_b_end_point_x"></param>
        /// <param name="a_b_start_point_y"></param>
        /// <param name="a_b_end_point_y"></param>
        /// <param name="a_b_start_point_z"></param>
        /// <param name="a_b_end_point_z"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsOverLap(int a_a_start_point_x, int a_a_end_point_x, int a_a_start_point_y, int a_a_end_point_y, int a_a_start_point_z, int a_a_end_point_z, int a_b_start_point_x, int a_b_end_point_x, int a_b_start_point_y, int a_b_end_point_y, int a_b_start_point_z, int a_b_end_point_z)
        {
            //AABB判定
            if (a_a_start_point_x > a_b_end_point_x) return false;
            if (a_a_end_point_x < a_b_start_point_x) return false;
            if (a_a_start_point_y > a_b_end_point_y) return false;
            if (a_a_end_point_y < a_b_start_point_y) return false;
            if (a_a_start_point_z > a_b_end_point_z) return false;
            if (a_a_end_point_z < a_b_start_point_z) return false;

            return true;
        }

        /// <summary>
        /// 引数で渡されたサイズのエリアが配置可能なPoint座標を検索する
        /// </summary>
        /// <param name="a_end_x">エリアのxサイズ</param>
        /// <param name="a_end_y">エリアのyサイズ</param>
        /// <param name="a_end_z">エリアのzサイズ</param>
        /// <returns>配置可能座標リスト</returns>
        public List<Point> SearchArea(int a_end_x, int a_end_y, int a_end_z, UnityEngine.Vector3Int a_map_size, List<Map.Room.RoomData> a_room_list)
        {
            List<Point> t_point_list = new List<Point>();

            Common.MultipleForLoop.Process(
                0, a_map_size.x - a_end_x,
                0, a_map_size.y - a_end_y,
                0, a_map_size.z - a_end_z,
                (j, k, l) => {
                    Point t_room_start_point = new Point(j, k, l);
                    Point t_room_end_point = t_room_start_point + new Point(a_end_x, a_end_y, a_end_z);

                    //既存の部屋と接触するか確認
                    bool t_room_hit_check = false;

                    for (int m = 0; m < a_room_list.Count; m++)
                    {
                        //エリア間の隙間サイズ
                        int t_area_padding = 1;

                        Point t_start_point = a_room_list[m].m_area_start - t_area_padding;
                        Point t_end_point = a_room_list[m].m_area_end + t_area_padding;

                        //エリア接触判定
                        if (Map.Area.MapArea.IsOverLap(t_start_point, t_end_point, t_room_start_point, t_room_end_point) == true)
                        {
                            t_room_hit_check = true;
                            break;
                        }

                    }

                    if (t_room_hit_check == true)
                    {
                        //部屋とかぶるのでパス
                        return true;
                    }

                    t_point_list.Add(t_room_start_point);
                    return true;
                });

            return t_point_list;
        }

        //通路+エリア外判定
        public bool IsRoadAndAreaIn(int a_x, int a_y, int a_z)
        {
            return IsAreaIn(a_x, a_y, a_z) && IsRoad(a_x, a_y, a_z);
        }
        public bool IsRoadAndAreaIn(Point a_pos)
        {
            return IsAreaIn(a_pos) && IsRoad(a_pos);
        }

        //空間+エリア外判定
        public bool IsNoneAndAreaIn(int a_x, int a_y, int a_z)
        {
            return IsAreaIn(a_x, a_y, a_z) && !IsRoad(a_x, a_y, a_z);
        }
        public bool IsNoneAndAreaIn(Point a_pos)
        {
            return IsAreaIn(a_pos) && !IsRoad(a_pos);
        }

        //通路判定
        private bool IsRoad(int a_x, int a_y, int a_z)
        {
            return Map.Param.CommonParams.m_data[a_x, a_y, a_z].state != CellType.NONE;
        }

        private bool IsRoad(Point a_pos)
        {
            return Map.Param.CommonParams.m_data[a_pos.x, a_pos.y, a_pos.z].state != CellType.NONE;
        }

        /// <summary>
        /// エリア座標文字列変換
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("(x_min_{0:G},x_max{1:G},y_min_{2:G},y_max{3:G},z_min_{4:G},z_max{5:G})", m_area_min_x, m_area_max_x, m_area_min_y, m_area_max_y, m_area_min_z, m_area_max_z);

        }
    }
}
