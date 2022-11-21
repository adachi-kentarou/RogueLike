
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Road
{
    //道階層情報を元に道を接続させるクラス
    public class RoadConnect
    {
        /// <summary>
        /// 道階層情報を元に道を接続させる
        /// </summary>
        /// <param name="a_count">接続道数</param>
        /// <param name="a_length">接続道の長さ</param>
        /// <param name="a_min_distance">接続2点の下限距離</param>
        /// <param name="a_max_distance">接続2点の上限距離</param>
        /// <param name="a_remove_min_distance">除外条件距離</param>
        public void CreateRoadConnect(int a_count, int a_length, int a_min_distance , int a_max_distance, int a_remove_min_distance)
        {
            List<HierarchyData> t_point_list = new List<HierarchyData>();
            
            foreach (Point branch in Map.Param.CommonParams.m_road_all_buf)
            {
                if (Map.Param.CommonParams.GetCellData(branch).m_road_structure == null) continue;

                //全方向調査
                for (int i = 0; i < (int)Direction.MAX_NUM; i++)
                {
                    Point t_point = branch;

                    if (Map.Param.CommonParams.GetCellData(t_point).GetConnect((Direction)i) == Map.Cell.ConnectType.CONNECT)
                    {
                        continue;
                    }

                    //直進距離ループ
                    for (int j = 0; j < a_length; j++)
                    {
                        t_point += Point.PointCorrection((Direction)i);

                        if (Map.Param.CommonParams.m_map_area.IsAreaIn(t_point) == false)
                        {
                            //エリア外に出たので調査終了
                            break;
                        }

                        Map.Cell.ConnectType t_connect = Map.Param.CommonParams.GetCellData(t_point).GetRevConnect((Direction)i);

                        if (t_connect == Map.Cell.ConnectType.BLOCK)
                        {
                            //ブロックセルにぶつかったので調査終了
                            break;
                        }



                        bool t_is_road = Map.Param.CommonParams.GetCellData(t_point).state != Map.Cell.CellType.NONE;
                        if (t_is_road == true && t_connect == Map.Cell.ConnectType.NONE)
                        {
                            //条件に一致したのでデータをリストに追加
                            t_point_list.Add(new HierarchyData(branch, (Direction)i, j));
                            break;
                        }


                    }
                }

            }

            //接続候補リストアップ
            List<HierarchyData> t_select_list = new List<HierarchyData>();

            foreach (var hierarchy_data in t_point_list)
            {
                {
                    Point t_point = (Point.PointCorrection(hierarchy_data.m_direction) * (hierarchy_data.m_extend + 1)) + hierarchy_data.m_point;

                    if (Map.Param.CommonParams.GetCellData(t_point).m_road_structure == null) continue;

                    int t_length = MatchHierarchyLength(hierarchy_data.m_point, t_point);
                    //2点の道の距離が一定の範囲のものだけリストアップ
                    if (t_length + hierarchy_data.m_extend > a_min_distance && t_length + hierarchy_data.m_extend < a_max_distance)
                    {
                        t_select_list.Add(hierarchy_data);
                    }
                }
            }

            //接続道本数
            for (int i = 0; i < a_count; i++)
            {
                if (t_select_list.Count == 0)
                {
                    break;
                }

                //リストから接続する道をランダムに選択
                HierarchyData t_hierarchy_data = t_select_list[Common.Math.RandomInt(0, t_select_list.Count)];
                Point t_point = t_hierarchy_data.m_point;
                int t_road_no = Map.Param.CommonParams.GetCellData(t_point).m_road_no;
                int t_area_no = Map.Param.CommonParams.GetCellData(t_point).m_area_no;
                //Debug.Log(t_point);
                for (int j = 0; j < t_hierarchy_data.m_extend + 1; j++)
                {

                    if (Map.Param.CommonParams.m_map_area.IsAreaIn(t_point) == false)
                    {
                        break;
                    }

                    Map.Param.CommonParams.GetCellData(t_point).m_road_no = t_road_no;
                    //GetCellData(t_point).m_area_no = t_area_no;
                    //最初の座標はエリアナンバーを変更しない
                    if (j != 0)
                    {
                        //接続用の道はエリアナンバーを0にする
                        Map.Param.CommonParams.GetCellData(t_point).m_area_no = 0;
                    }

                    t_point += Point.PointCorrection(t_hierarchy_data.m_direction);

                    Map.Env.MapEnv.SetRoad(t_point, t_hierarchy_data.m_direction, Map.Cell.StateType.DETAILED);

                    t_road_no++;


                }

                //選択した道から一定距離にある接続道をリストから除外する
                t_select_list.RemoveAll(
                    hierarchy_data =>
                    {
                        Point t_point1 = (Point.PointCorrection(hierarchy_data.m_direction) * (hierarchy_data.m_extend + 1)) + hierarchy_data.m_point;
                        Point t_point2 = (Point.PointCorrection(t_hierarchy_data.m_direction) * (t_hierarchy_data.m_extend + 1)) + t_hierarchy_data.m_point;

                        int t_lenght1 = MatchHierarchyLength(hierarchy_data.m_point, t_hierarchy_data.m_point);
                        int t_lenght2 = MatchHierarchyLength(t_point1, t_point2);
                        int t_lenght3 = MatchHierarchyLength(t_point1, t_hierarchy_data.m_point);
                        int t_lenght4 = MatchHierarchyLength(hierarchy_data.m_point, t_point2);

                        //Debug.Log(t_lenght1.ToString() + "," + t_lenght2.ToString() + "," + t_lenght3.ToString() + "," + t_lenght4.ToString() + "," + hierarchy_data.m_point.ToString() + t_point1.ToString()+ t_point2.ToString());

                        //接続した道から一定距離にあった場合はリストから除外しておく
                        if (t_lenght1 < a_remove_min_distance ||
                                t_lenght2 < a_remove_min_distance ||
                                t_lenght3 < a_remove_min_distance ||
                                t_lenght4 < a_remove_min_distance
                            )
                        {
                            //Debug.Log("delete" + hierarchy_data.m_point.ToString());
                            return true;
                        }

                        return false;
                    }
                );

                //今回選択した接続データも削除する
                t_select_list.Remove(t_hierarchy_data);

            }
        }

        /// <summary>
        /// マップの順位と階層データを元に2座標の距離を返す
        /// </summary>
        /// <param name="a_point1">座標1</param>
        /// <param name="a_point2">座標2</param>
        /// <returns>2座標の距離</returns>
        private int MatchHierarchyLength(Point a_point1, Point a_point2)
        {
            Map.Cell.CellData t_cell_data1 = Map.Param.CommonParams.GetCellData(a_point1);
            Map.Cell.CellData t_cell_data2 = Map.Param.CommonParams.GetCellData(a_point2);

            List<RoadStructure> t_list1 = t_cell_data1.m_road_structure.GetParentList();
            List<RoadStructure> t_list2 = t_cell_data2.m_road_structure.GetParentList();
            
            int t_list1_length = t_cell_data1.m_road_structure.m_road_list.IndexOf(a_point1);
            for (int i = 0; i < t_list1.Count; i++)
            {
                int t_list2_length = t_cell_data2.m_road_structure.m_road_list.IndexOf(a_point2); ;
                for (int j = 0; j < t_list2.Count; j++)
                {
                    if (t_list1[i].m_num == t_list2[j].m_num)
                    {
                        return t_list1_length + t_list2_length;
                    }
                    if (t_list2[j].m_parent != null)
                    {
                        t_list2_length += t_list2[j].m_parent.m_road_list.Count;
                    }
                }

                if (t_list1[i].m_parent != null)
                {
                    t_list1_length += t_list1[i].m_parent.m_road_list.Count;
                }
            }
            return 0;
        }
    }
}
