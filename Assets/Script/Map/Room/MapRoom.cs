using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map;
using Map.Cell;
using Map.Room;
//using Map.Area;
using Common;

namespace Map.Room
{
    public class MapRoom
    {
        //リストの参照保持用
        Map.Area.MapArea m_map_area;
        List<RoomData> m_room_list;
        UnityEngine.Vector3Int m_map_size;

        /// <summary>
        /// パラメータ初期化
        /// </summary>
        /// <param name="a_map_area"></param>
        /// <param name="a_room_list"></param>
        /// <param name="a_map_size"></param>
        public void InitParams(Map.Area.MapArea a_map_area, List<Map.Room.RoomData> a_room_list, UnityEngine.Vector3Int a_map_size)
        {
            m_map_area =  a_map_area;
            m_room_list = a_room_list;
            m_map_size = a_map_size;
        }

        /// <summary>
        /// 部屋の作成
        /// </summary>
        /// <param name="a_room_count">作製する部屋の数</param>
        public void MakeRoom(int a_room_count, int a_room_min_size, int a_room_max_size)
        {
            //m_sequence = Sequence.MAKE_ROOM;
            for (int i = 0; i < a_room_count; i++)
            {
                //候補ポイント作成
                int t_end_x = Common.Math.RandomInt(a_room_min_size, a_room_max_size);
                int t_end_y = Common.Math.RandomInt(a_room_min_size, a_room_max_size);
                int t_end_z = Common.Math.RandomInt(a_room_min_size, a_room_max_size);

                List<Point> t_point_list = m_map_area.SearchArea(t_end_x, t_end_y, t_end_z, m_map_size, m_room_list);

                if (t_point_list.Count != 0)
                {
                    //候補があるならそこから選ぶ
                    Point t_point = t_point_list[Common.Math.RandomInt(0, t_point_list.Count)];

                    int t_room_count = m_room_list.Count + 1;

                    //部屋データリストに追加
                    RoomData t_room_data = new RoomData();
                    t_room_data.m_area_start = t_point;
                    t_room_data.m_area_end = t_point + new Point(t_end_x, t_end_y, t_end_z);
                    m_room_list.Add(t_room_data);

                    //エリアを反映
                    Common.MultipleForLoop.Process(
                        t_point.x, t_point.x + t_end_x,
                        t_point.y, t_point.y + t_end_y,
                        t_point.z, t_point.z + t_end_z,
                        (a_x, a_y, a_z) =>
                        {
                            //Map.MapCreate.m_data[a_x, a_y, a_z].m_room_area = t_room_count;
                            Map.Param.CommonParams.GetCellData(a_x, a_y, a_z).m_room_area = t_room_count;
                            return true;
                        },
                        true);

                }
            }
        }

        /// <summary>
        /// 指定された部屋のセル情報から部屋番号を削除する
        /// </summary>
        /// <param name="a_room_no">部屋番号</param>
        void ReleaseRoom(int a_room_no)
        {
            //要素数が範囲外なら終了
            if (a_room_no < 0 || a_room_no >= m_room_list.Count)
            {
                return;
            }

            m_room_list.ForEach(a_room =>
            {
                Common.MultipleForLoop.Process(
                    a_room.m_area_start.x, a_room.m_area_end.x,
                    a_room.m_area_start.y, a_room.m_area_end.y,
                    a_room.m_area_start.z, a_room.m_area_end.z,
                    (a_x, a_y, a_z) =>
                    {
                        //Map.MapCreate.m_data[a_x, a_y, a_z].m_room_area = 0;
                        Map.Param.CommonParams.GetCellData(a_x, a_y, a_z).m_room_area = 0;
                        return true;
                    }
                    , true);
            });
        }



    }
}
