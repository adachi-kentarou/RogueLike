using System.Collections.Generic;

namespace Map.Road
{
    /// <summary>
    /// 道のエリア作成クラス
    /// </summary>
    public class RoadArea
    {
        /// <summary>
        /// マップのセルデータにエリア番号を設定する
        /// </summary>
        /// <param name="a_area_count">エリア数</param>
        /// <param name="a_area_branch_min">エリア分岐下限数</param>
        /// <param name="a_area_branch_max">エリア分岐上限数</param>
        /// <param name="a_length_range">エリア長さ補正倍率範囲</param>
        public void CreateRoadAreaNo(int a_area_count, int a_area_branch_min, int a_area_branch_max, float a_length_range)
        {
            //次の移動先候補リスト
            List<Point> t_next_list = new List<Point>();

            List<Point> t_road_list = new List<Point>();

            t_road_list.Add(Map.Param.CommonParams.m_road_all_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_road_all_buf.Count)]);

            for (int i = 0; i < a_area_count; i++)
            {

                //エリア毎の数　何本同じエリアの道を作成するか
                int t_area_num = Common.Math.RandomInt(a_area_branch_min, a_area_branch_max);
                
                for (int j = 0; j < t_area_num; j++)
                {
                    if (t_road_list.Count == 0)
                    {
                        break;
                    }
                    //開始位置
                    Point t_start_Point = t_road_list[Common.Math.RandomInt(0, t_road_list.Count)];
                    t_road_list.Remove(t_start_Point);
                    
                    t_next_list.Clear();
                    t_next_list.Add(t_start_Point);

                    //エリアの長さ
                    int t_count_param = (int)(Map.Param.CommonParams.m_road_all_buf.Count / a_area_count);
                    int t_count = Common.Math.RandomInt((int)((float)t_count_param * (1f - a_length_range)), (int)((float)t_count_param * (1f + a_length_range)));
                    
                    //エリアの長さだけ番号を設定する
                    while (t_count > 0)
                    {
                        if (t_next_list.Count == 0)
                        {
                            break;
                        }

                        //移動候補リストからランダムに選択
                        Map.Param.CommonParams.m_now_position = t_next_list[Common.Math.RandomInt(0, t_next_list.Count)];
                        Map.Param.CommonParams.m_data[Map.Param.CommonParams.m_now_position.x, Map.Param.CommonParams.m_now_position.y, Map.Param.CommonParams.m_now_position.z].m_area_no = i + 1;

                        t_next_list.Remove(Map.Param.CommonParams.m_now_position);

                        //道が接続しているセルを次の移動候補リストに入れる
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_up == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.UP) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.UP) + Map.Param.CommonParams.m_now_position);
                        }
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_down == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.DOWN) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.DOWN) + Map.Param.CommonParams.m_now_position);
                        }
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_left == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.LEFT) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.LEFT) + Map.Param.CommonParams.m_now_position);
                        }
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_right == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.RIGHT) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.RIGHT) + Map.Param.CommonParams.m_now_position);
                        }
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_front == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.FRONT) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.FRONT) + Map.Param.CommonParams.m_now_position);
                        }
                        if (Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_back == Map.Cell.ConnectType.CONNECT && Map.Param.CommonParams.GetCellData(Point.PointCorrection(Direction.BACK) + Map.Param.CommonParams.m_now_position).m_area_no == 0)
                        {
                            t_next_list.Add(Point.PointCorrection(Direction.BACK) + Map.Param.CommonParams.m_now_position);
                        }

                        t_count--;
                    }

                    t_road_list.AddRange(t_next_list);
                }

            }
        }
    }
}
