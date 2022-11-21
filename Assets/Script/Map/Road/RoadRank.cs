using System.Collections.Generic;
namespace Map.Road
{
    //道の順番を作製する
    class RoadRank
    {
        /// <summary>
        /// 道の順番を作製しCellDataのm_road_noに記録する
        /// </summary>
        /// <returns>
        /// 道ランク開始座標
        /// </returns>
        public Point CreateRoadRank()
        {
            //開始位置
            Point t_start_Point = Map.Param.CommonParams.m_road_all_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_road_all_buf.Count)];

            Map.Param.CommonParams.GetCellData(t_start_Point).m_road_no = 1;
            int t_road_num = 1;
            List<Point> t_road_list = new List<Point>();
            t_road_list.Add(t_start_Point);
            List<Point> t_next_list = new List<Point>();
            while (t_road_list.Count != 0)
            {
                t_road_num++;
                t_next_list.Clear();
                for (int i = 0; i < t_road_list.Count; i++)
                {
                    Point t_branch = t_road_list[i];
                    for (int j = 0; j < (int)Direction.MAX_NUM; j++)
                    {
                        Point t_dir_point = t_branch + Point.PointCorrection((Direction)j);

                        bool t_connect_flg = false;
                        switch ((Direction)j)
                        {
                            case Direction.UP:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_up == Map.Cell.ConnectType.CONNECT;
                                break;
                            case Direction.DOWN:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_down == Map.Cell.ConnectType.CONNECT;
                                break;
                            case Direction.LEFT:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_left == Map.Cell.ConnectType.CONNECT;
                                break;
                            case Direction.RIGHT:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_right == Map.Cell.ConnectType.CONNECT;
                                break;
                            case Direction.FRONT:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_front == Map.Cell.ConnectType.CONNECT;
                                break;
                            case Direction.BACK:
                                t_connect_flg = Map.Param.CommonParams.GetCellData(t_branch).m_back == Map.Cell.ConnectType.CONNECT;
                                break;
                        }

                        //次の確認座標バッファ追加条件
                        if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(t_dir_point)      //エリア内
                            && t_connect_flg                                                    //接続されている
                            && Map.Param.CommonParams.GetCellData(t_dir_point).m_road_no == 0)  //まだ道ランクが書き込まれていない
                        {

                            Map.Param.CommonParams.GetCellData(t_dir_point).m_road_no = t_road_num;
                            t_next_list.Add(t_dir_point);
                        }

                    }
                }
                t_road_list = new List<Point>(t_next_list);
            }

            return t_start_Point;
        }
    }
}
