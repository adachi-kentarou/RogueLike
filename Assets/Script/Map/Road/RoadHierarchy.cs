using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Road
{
    //道のランクを元にした階層データ作製クラス
    public class RoadHierarchy
    {
        /// <summary>
        /// 道のランクを元に階層データ作製
        /// </summary>
        /// <param name="a_start_point">道ランク開始座標</param>
        public void CreateRoadHierarchy(Point a_start_Point)
        {
            RoadStructure.Init();

            Map.Param.CommonParams.m_road_structure = new RoadStructure();
            Map.Param.CommonParams.m_road_structure.m_origin = a_start_Point;
            Map.Param.CommonParams.m_road_structure.m_parent = null;

            BranchRoad(ref Map.Param.CommonParams.m_road_structure);
        }

        /// <summary>
        /// 再帰処理で階層作成
        /// </summary>
        /// <param name="a_road">再帰元の階層データ</param>
        private void BranchRoad(ref RoadStructure a_road)
        {
            Point t_now_point = a_road.m_origin;


            while (true)
            {

                Map.Cell.CellData t_cell = Map.Param.CommonParams.GetCellData(t_now_point);
                a_road.m_road_list.Add(t_now_point);
                Map.Param.CommonParams.GetCellData(t_now_point).m_road_structure = a_road;

                int t_road_num = t_cell.m_road_no + 1;

                bool t_up = t_cell.m_up == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.up).m_road_no == t_road_num;
                bool t_down = t_cell.m_down == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.down).m_road_no == t_road_num;
                bool t_left = t_cell.m_left == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.left).m_road_no == t_road_num;
                bool t_right = t_cell.m_right == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.right).m_road_no == t_road_num;
                bool t_front = t_cell.m_front == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.front).m_road_no == t_road_num;
                bool t_back = t_cell.m_back == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(t_now_point + Point.back).m_road_no == t_road_num;

                //接続数をカウント
                int t_count = (t_up == true ? 1 : 0) +
                    (t_down == true ? 1 : 0) +
                    (t_left == true ? 1 : 0) +
                    (t_right == true ? 1 : 0) +
                    (t_front == true ? 1 : 0) +
                    (t_back == true ? 1 : 0);

                if (t_count == 0)
                {
                    //接続無ければ処理を抜ける
                    return;
                }

                //分岐した場合は再帰処理開始
                if (t_count >= 2)
                {
                    if (t_up == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.up;
                        t_road_structure.m_road_list.Add(t_now_point + Point.up);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    if (t_down == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.down;
                        t_road_structure.m_road_list.Add(t_now_point + Point.down);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    if (t_left == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.left;
                        t_road_structure.m_road_list.Add(t_now_point + Point.left);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    if (t_right == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.right;
                        t_road_structure.m_road_list.Add(t_now_point + Point.right);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    if (t_front == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.front;
                        t_road_structure.m_road_list.Add(t_now_point + Point.front);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    if (t_back == true)
                    {
                        RoadStructure t_road_structure = new RoadStructure();
                        t_road_structure.m_origin = t_now_point + Point.back;
                        t_road_structure.m_road_list.Add(t_now_point + Point.back);
                        t_road_structure.m_parent = a_road;
                        BranchRoad(ref t_road_structure);
                    }
                    return;
                }

                //分岐していなければ接続方向に直進
                if (t_up == true)
                {
                    t_now_point += Point.up;
                    continue;
                }
                if (t_down == true)
                {
                    t_now_point += Point.down;
                    continue;
                }
                if (t_left == true)
                {
                    t_now_point += Point.left;
                    continue;
                }
                if (t_right == true)
                {
                    t_now_point += Point.right;
                    continue;
                }
                if (t_front == true)
                {
                    t_now_point += Point.front;
                    continue;
                }
                if (t_back == true)
                {
                    t_now_point += Point.back;
                    continue;
                }
            }


        }
    }
}
