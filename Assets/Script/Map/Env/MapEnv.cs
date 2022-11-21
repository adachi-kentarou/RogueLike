using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Env
{
    /// <summary>
    /// マップ共通処理
    /// </summary>
    sealed class MapEnv
    {
        private static List<Direction> m_dir = new List<Direction>();


        /// <summary>
        /// 分岐処理でのタイプ別分岐可能座標調査用ディレクター
        /// </summary>
        /// <param name="a_branch_build"></param>
        public static void SearchDirection(Map.Branch.Builder.BranchBuilderBase a_branch_build)
        {

            Map.Param.CommonParams.m_dir_buf.Clear();
            Map.Param.CommonParams.m_updown_buf.Clear();

            //延長可能判定
            Map.Param.CommonParams.m_branch_buf.ForEach(branch => {

                a_branch_build.CheckBranch(branch);

            });

            Map.Param.CommonParams.m_branch_buf = new List<Point>(Map.Param.CommonParams.m_road_buf);
        }

        /// <summary>
        /// セル接続確認
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            //List<Direction> m_dir = new List<Direction>();
            m_dir.Clear();

            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.left) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.left).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.LEFT);
            }
            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.right) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.right).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.RIGHT);
            }
            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.front) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.front).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.FRONT);
            }
            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.back) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.back).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.BACK);
            }
            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.up) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.up).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.UP);
            }
            if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(Map.Param.CommonParams.m_now_position + Point.down) && Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position + Point.down).m_create_type != Map.Cell.StateType.BAN)
            {
                m_dir.Add(Direction.DOWN);
            }

            if (m_dir.Count != 0)
            {
                var m_direction = m_dir[Common.Math.RandomInt(0, m_dir.Count)];
                var t_near_pos = Map.Param.CommonParams.m_now_position + Point.PointCorrection(m_direction);
                switch (m_direction)
                {
                    case Direction.LEFT:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_left = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_right = Map.Cell.ConnectType.CONNECT;
                        break;
                    case Direction.RIGHT:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_right = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_left = Map.Cell.ConnectType.CONNECT;
                        break;
                    case Direction.FRONT:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_front = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_back = Map.Cell.ConnectType.CONNECT;
                        break;
                    case Direction.BACK:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_back = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_front = Map.Cell.ConnectType.CONNECT;
                        break;
                    case Direction.UP:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_up = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_down = Map.Cell.ConnectType.CONNECT;
                        break;
                    case Direction.DOWN:
                        Map.Param.CommonParams.GetCellData(Map.Param.CommonParams.m_now_position).m_down = Map.Cell.ConnectType.CONNECT;
                        Map.Param.CommonParams.GetCellData(t_near_pos).m_up = Map.Cell.ConnectType.CONNECT;
                        break;
                }
                return true;
            }
            return false;
        }


        public static Point RandomMapPos(UnityEngine.Vector3Int a_map_size)
        {
            return new Point(Common.Math.RandomInt(0, a_map_size.x), Common.Math.RandomInt(0, a_map_size.y), Common.Math.RandomInt(0, a_map_size.z));
        }

        public static Point SetRoad(int a_x, int a_y, int a_z, Direction a_direction, Map.Cell.StateType a_styleType)
        {
            return SetRoadFunc(new Point(a_x, a_y, a_z), a_direction, a_styleType);
        }

        public static Point SetRoad(Point a_point, Direction a_direction, Map.Cell.StateType a_styleType)
        {
            return SetRoadFunc(a_point, a_direction, a_styleType);
        }

        private static Point SetRoadFunc(Point a_point, Direction a_direction, Map.Cell.StateType a_styleType)
        {
            Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].state = Map.Cell.CellType.TILE;

            //if(IsRoadAndAreaIn(a_point.x + 1, a_point.y, a_point.z) == true)
            if (a_direction == Direction.LEFT)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_right = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x + 1, a_point.y, a_point.z].m_left = Map.Cell.ConnectType.CONNECT;
            }
            //if (IsRoadAndAreaIn(a_point.x - 1, a_point.y, a_point.z) == true)
            if (a_direction == Direction.RIGHT)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_left = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x - 1, a_point.y, a_point.z].m_right = Map.Cell.ConnectType.CONNECT;
            }
            //if (IsRoadAndAreaIn(a_point.x, a_point.y + 1, a_point.z) == true)
            if (a_direction == Direction.DOWN)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_up = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x, a_point.y + 1, a_point.z].m_down = Map.Cell.ConnectType.CONNECT;
            }
            //if (IsRoadAndAreaIn(a_point.x, a_point.y - 1, a_point.z) == true)
            if (a_direction == Direction.UP)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_down = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x, a_point.y - 1, a_point.z].m_up = Map.Cell.ConnectType.CONNECT;
            }
            //if (IsRoadAndAreaIn(a_point.x, a_point.y, a_point.z + 1) == true)
            if (a_direction == Direction.BACK)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_front = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z + 1].m_back = Map.Cell.ConnectType.CONNECT;
            }
            //if (IsRoadAndAreaIn(a_point.x, a_point.y, a_point.z - 1) == true)
            if (a_direction == Direction.FRONT)
            {
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_back = Map.Cell.ConnectType.CONNECT;
                Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z - 1].m_front = Map.Cell.ConnectType.CONNECT;
            }

            Point t_pos = new Point(a_point.x, a_point.y, a_point.z);
            Map.Param.CommonParams.m_road_buf.Add(t_pos);
            Map.Param.CommonParams.m_road_all_buf.Add(t_pos);

            /*
            switch (m_sequence)
            {
                case Sequence.SIMPLE:
                    Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_create_type = Map.Cell.StateType.SIMPLE;
                    break;
                case Sequence.DETAILED:
                case Sequence.ROOM_DETAILED:
                    Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_create_type = Map.Cell.StateType.DETAILED;
                    break;
                case Sequence.CONNECT:
                    Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_create_type = Map.Cell.StateType.BAN;
                    break;
            }
            */
            Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].m_create_type = a_styleType;

            return t_pos;
        }

        public static Point SetStair(int a_x, int a_y, int a_z)
        {
            return SetStairFunc(new Point(a_x, a_y, a_z));
        }

        public static Point SetStair(Point a_point)
        {
            return SetStairFunc(a_point);
        }

        private static Point SetStairFunc(Point a_point)
        {
            Map.Param.CommonParams.m_data[a_point.x, a_point.y, a_point.z].state = Map.Cell.CellType.TILE;
            Point t_pos = new Point(a_point.x, a_point.y, a_point.z);
            Map.Param.CommonParams.m_stair_buf.Add(t_pos);
            return t_pos;

        }

        /// <summary>
        /// branch_bufとroad_bufに指定範囲の未設定セルを記録
        /// </summary>
        /// <param name="a_min_x">x座標開始位置</param>
        /// <param name="a_max_x">x座標終了位置</param>
        /// <param name="a_min_y">y座標開始位置</param>
        /// <param name="a_max_y">y座標終了位置</param>
        /// <param name="a_min_z">z座標開始位置</param>
        /// <param name="a_max_z">z座標終了位置</param>
        public static void SetSpaceList(int a_min_x, int a_max_x, int a_min_y, int a_max_y, int a_min_z, int a_max_z)
        {
            SetSpaceListFunc(a_min_x, a_max_x, a_min_y, a_max_y, a_min_z, a_max_z);
        }

        /// <summary>
        /// branch_bufとroad_bufに指定範囲の未設定セルを記録
        /// </summary>
        /// <param name="a_min">開始ポイント</param>
        /// <param name="a_max">終了ポイント</param>
        public static void SetSpaceList(Point a_min, Point a_max)
        {
            SetSpaceListFunc(a_min.x, a_max.x, a_min.y, a_max.y, a_min.z, a_max.z);
        }

        /// <summary>
        /// branch_bufとroad_bufに指定範囲の未設定セルを記録
        /// </summary>
        /// <param name="a_min_x">x座標開始位置</param>
        /// <param name="a_max_x">x座標終了位置</param>
        /// <param name="a_min_y">y座標開始位置</param>
        /// <param name="a_max_y">y座標終了位置</param>
        /// <param name="a_min_z">z座標開始位置</param>
        /// <param name="a_max_z">z座標終了位置</param>
        private static void SetSpaceListFunc(int a_min_x, int a_max_x, int a_min_y, int a_max_y, int a_min_z, int a_max_z)
        {
            Map.Param.CommonParams.m_branch_buf.Clear();
            Map.Param.CommonParams.m_road_buf.Clear();

            Common.MultipleForLoop.Process(
                a_min_x, a_max_x,
                a_min_y, a_max_y,
                a_min_z, a_max_z,
                (a_x, a_y, a_z) => {
                    if (Map.Param.CommonParams.GetCellData(a_x, a_y, a_z).state == Map.Cell.CellType.NONE)
                    {
                        Map.Param.CommonParams.m_branch_buf.Add(new Point(a_x, a_y, a_z));
                    }
                    return true;
                });

            Map.Param.CommonParams.m_road_buf = new List<Point>(Map.Param.CommonParams.m_branch_buf);
        }

        /// <summary>
        /// 道バッファのroom_noを全て書き換える
        /// </summary>
        /// <param name="a_room_no">書き換えたいStateType</param>
        public static void ReSetRoad(Map.Cell.StateType a_room_no)
        {
            Map.Param.CommonParams.m_road_buf.ForEach(a_point =>
            {
                Map.Param.CommonParams.GetCellData(a_point).m_create_type = a_room_no;
            }
            );
        }

        /// <summary>
        /// 指定サイズのエリアに道が無い座標を分岐バッファにリスト化させる
        /// </summary>
        /// <param name="a_size"></param>
        public static void SearchSpaceAreaPosition(int a_size)
        {
            Map.Param.CommonParams.m_branch_buf.Clear();
            Map.Param.CommonParams.m_road_buf.Clear();

            int t_size = a_size;

            //基準座標からXYZ軸にa_size離れた立方体の座標を調査する
            while (t_size > 0)
            {
                Common.MultipleForLoop.Process(Map.Param.CommonParams.m_map_area,
                    (a_x1, a_y1, a_z1) => {
                            //エリア範囲確認

                            bool t_flg = false;

                        Common.MultipleForLoop.Process(
                            -t_size, t_size,
                            -t_size, t_size,
                            -t_size, t_size,
                            (a_x2, a_y2, a_z2) => {
                                if (Map.Param.CommonParams.m_map_area.IsAreaIn(a_x2 + a_x1, a_y2 + a_y1, a_z2 + a_z1) == false)
                                {
                                        //調査座標がエリア外だった場合はスキップ
                                        return true;
                                }
                                if (Map.Param.CommonParams.GetCellData(a_x2 + a_x1, a_y2 + a_y1, a_z2 + a_z1).state == Map.Cell.CellType.TILE)
                                {
                                        //調査座標に道があるフラグを有効にし調査を終了
                                        t_flg = true;
                                    return false;
                                }
                                return true;
                            },
                            true);//<=にする

                            //フラグが無効であれば分岐バッファに座標を追加する
                            if (t_flg == false) Map.Param.CommonParams.m_branch_buf.Add(new Point(a_x1, a_y1, a_z1));

                        return true;
                    });

                if (Map.Param.CommonParams.m_branch_buf.Count != 0)
                {
                    //一つでも分岐バッファに座標データがあれば接続可能判定とし調査を終了
                    break;
                }

                //立方体のサイズを１つ小さくする
                t_size--;
            }


            Map.Param.CommonParams.m_road_buf = new List<Point>(Map.Param.CommonParams.m_branch_buf);//元のデータを変更させないためにディープコピー

        }

        /// <summary>
        /// 既存の道のランダム座標を返す
        /// </summary>
        /// <returns>ランダムな道の座標</returns>
        public static UnityEngine.Vector3 GetRandomRoadPos()
        {
            return GetRandomRoadPos(UnityEngine.Vector3.zero);
        }

        /// <summary>
        /// 既存の道のランダム座標を返す
        /// </summary>
        /// <param name="a_add_pos">座標補正</param>
        /// <returns>ランダムな道の座標</returns>
        public static UnityEngine.Vector3 GetRandomRoadPos(UnityEngine.Vector3 a_add_pos)
        {
            Point t_target_pos;
			Cell.CellData t_data;
			do
			{
				var t_index = Common.Math.RandomInt(0, Map.Param.CommonParams.m_road_all_buf.Count);
				t_target_pos = Map.Param.CommonParams.m_road_all_buf[t_index];
				t_data = Map.Param.CommonParams.GetCellData(t_target_pos);

			}
			while (t_data.state != Cell.CellType.TILE || t_data.m_road_no == 0);
			
            //座標補正
            //セルの四隅ランダム設定 + 底上げ
            var t_rot = Common.Math.RandomInt(0,4) * 90;

			//通路なので中央配置
			if (t_data.m_room_area == 0) a_add_pos.x = 0f;
			if (t_data.m_up == Cell.ConnectType.CONNECT)
			{
				a_add_pos.x += 0f;
				a_add_pos.y += 5f;
			}

			var t_add_pos = UnityEngine.Quaternion.Euler(0f, (float)t_rot, 0f) * a_add_pos;
            
            return t_add_pos + (t_target_pos.GetConvertVec3() * 10f);

        }
    }
}
