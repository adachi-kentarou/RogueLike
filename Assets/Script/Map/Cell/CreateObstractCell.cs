namespace Map.Cell
{
    /// <summary>
    /// 指定範囲のエリアにランダムにブロックタイプにするクラス
    /// </summary>
    public class ObstractCell
    {
        /// <summary>
        /// ランダムブロックタイプ作成
        /// </summary>
        /// <param name="a_num">ブロック数</param>
        /// <param name="a_map_size">ブロックするエリア範囲</param>
        public void Create(int a_num, UnityEngine.Vector3Int a_map_size)
        {
            for (int i = 0; i < a_num; i++)
            {
                //ランダムで選ばれた座標と上下左右前後に隣接している座標をブロックタイプにする
                var pos = Map.Env.MapEnv.RandomMapPos(a_map_size);
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.left) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_left = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.left).m_right = ConnectType.BLOCK;
                }
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.right) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_right = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.right).m_left = ConnectType.BLOCK;
                }
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.front) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_front = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.front).m_back = ConnectType.BLOCK;
                }
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.back) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_back = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.back).m_front = ConnectType.BLOCK;
                }
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.up) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_up = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.up).m_down = ConnectType.BLOCK;
                }
                if (Map.Param.CommonParams.m_map_area.IsAreaIn(pos + Point.down) == true)
                {
                    Map.Param.CommonParams.GetCellData(pos).m_down = ConnectType.BLOCK;
                    Map.Param.CommonParams.GetCellData(pos + Point.down).m_up = ConnectType.BLOCK;
                }

            }
        }
    }
}
