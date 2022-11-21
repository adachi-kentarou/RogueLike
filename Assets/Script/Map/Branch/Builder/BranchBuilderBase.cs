using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Branch.Builder
{
    /// <summary>
    /// 通路分岐処理ビルダーパターン基底クラス
    /// </summary>
    public abstract class BranchBuilderBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化成功判定</returns>
        protected abstract bool Init();
        
        /// <summary>
        /// 接続確認フラグ
        /// </summary>
        protected bool m_connect_flg;

        /// <summary>
        /// セル書き込みタイプ
        /// </summary>
        protected abstract Map.Cell.StateType GetStyleType();

        /// <summary>
        /// ビルダーパターン接続タイプ判定
        /// </summary>
        /// <returns>接続可能判定</returns>
        protected abstract bool IsConnectType();

        /// <summary>
        /// 分岐数
        /// </summary>
        protected int m_branch_count = 0;

        /// <summary>
        /// ループインデックス
        /// </summary>
        protected int m_index = 0;

        protected bool[] m_direction_arr = new bool[(int)Map.Direction.MAX_NUM];

        protected bool IsStraight(int a_x, int a_y, int a_z, Direction a_dir)
        {
            return IsStaightFunc(a_x, a_y, a_z, a_dir);
        }

        protected bool IsStraight(Point a_pos, Direction a_dir)
        {
            return IsStaightFunc(a_pos.x, a_pos.y, a_pos.z, a_dir);
        }

        //タイプ毎にパラメーターが違うので継承先で実装
        protected bool IsStaightFunc(int a_x, int a_y, int a_z, Direction a_dir)
        {
            if (!Map.Param.CommonParams.m_map_area.IsAreaIn(new Point(a_x, a_y, a_z) + Point.PointCorrection(a_dir)))
            {

                return false;
            }

            if (Map.Param.CommonParams.GetCellData(a_x, a_y, a_z).GetConnect(a_dir) == Map.Cell.ConnectType.BLOCK)
            {
                return false;
            }

            (int, int, int, int, int, int) t_params = GetStaightParam(a_x,a_y,a_z,a_dir);

            bool t_ret = true;
            Common.MultipleForLoop.Process(
                t_params.Item1, t_params.Item2,
                t_params.Item3, t_params.Item4,
                t_params.Item5, t_params.Item6,
                (a_x1, a_y1, a_z1) => {
                    var t_pos = Point.PointCorrection(a_dir);
                    if (!Map.Param.CommonParams.m_map_area.IsAreaIn(t_pos.x + a_x, t_pos.y + a_y, t_pos.z + a_z))
                    {
                        t_ret = false;
                        return false;
                    }
                    if (Map.Param.CommonParams.m_data[t_pos.x + a_x, t_pos.y + a_y, t_pos.z + a_z].state == Map.Cell.CellType.NO_ENTRY)
                    {
                        t_ret = false;
                        return false;
                    }
                    if (!Map.Param.CommonParams.m_map_area.IsAreaIn(a_x1 + a_x, a_y1 + a_y, a_z1 + a_z))
                    {
                        return true;
                    }
                    if (Map.Param.CommonParams.m_data[a_x1 + a_x, a_y1 + a_y, a_z1 + a_z].state != Map.Cell.CellType.NONE)
                    {
                        t_ret = false;
                        return false;
                    }
                    return true;
                });

            return t_ret;
        }

        protected abstract (int, int, int, int, int, int) GetStaightParam(int a_x, int a_y, int a_z, Direction a_dir);
        
        /// <summary>
        /// バッファリセット
        /// </summary>
        protected abstract void BranchBufResetProcces();

        /// <summary>
        /// 移動先無し処理
        /// </summary>
        /// <returns></returns>
        protected abstract bool DestinationEmptyProcces();
        
        /// <summary>
        /// バッファーリセット処理
        /// </summary>
        protected abstract void BufReset();

        /// <summary>
        /// ループスキップカウント
        /// </summary>
        private int m_loop_skip_count = 0;

        public void Create(int a_count)
        {
            //初期化
            if (Init() == false) return;

            //ループ処理
            for (m_index = 0; m_index < a_count; m_index++)
            {
                /*
                if (m_loop_skip_count-- > 0)
                {
                    //スキップカウントがある場合は処理をスキップ
                    continue;
                }
                */
                //実際の処理は継承先でやる
                if (Process() == false) break;//処理中断
            }
        }

        /// <summary>
        /// 分岐ループ処理
        /// </summary>
        /// <returns>true ループ継続 false ループ終了</returns>
        private bool Process()
        {
#if false // 継承先に分離する

            if (m_sequence == Sequence.DETAILED)
            {
                m_branch_count--;

                if (m_branch_count <= 0)
                {
                    //ブランチエリアバッファリセット
                    Map.Param.CommonParams.m_branch_buf = new List<Point>(Map.Param.CommonParams.m_simple_road_buf);
                }
            }
#else
            BranchBufResetProcces();
#endif

            Map.Param.CommonParams.m_dir_buf.Clear();
            Map.Param.CommonParams.m_updown_buf.Clear();
            if (Map.Param.CommonParams.m_stair_flg == true)
            {
                CheckBranch(Map.Param.CommonParams.m_now_position);
            }

            if (Map.Param.CommonParams.m_dir_buf.Count == 0)
            {
                //Map.Env.MapEnv.SearchDirection(this);
                SearchDirection();
            }

            //分岐可能数確認
            if (Map.Param.CommonParams.m_dir_buf.Count == 0 && Map.Param.CommonParams.m_updown_buf.Count == 0)
            {
                //これ以上分岐できる場所がない
                //Debug.Log("これ以上分岐できる場所がない");
                return false;
            }

            Map.Cell.CellInfo t_cell_info_dir;
            Map.Cell.CellInfo t_cell_info_stair;

            //通路か階段か選択
            if (Map.Param.CommonParams.m_updown_buf.Count == 0 && Map.Param.CommonParams.m_dir_buf.Count != 0)
            {
                //通路のみ
                t_cell_info_dir = Map.Param.CommonParams.m_dir_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_dir_buf.Count)];
                Map.Param.CommonParams.m_now_position = t_cell_info_dir.Point;
            }
            else if (Map.Param.CommonParams.m_updown_buf.Count != 0 && Map.Param.CommonParams.m_dir_buf.Count == 0)
            {
                //階段のみ
                t_cell_info_stair = Map.Param.CommonParams.m_updown_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_updown_buf.Count)];
                Map.Param.CommonParams.m_now_position = t_cell_info_stair.Point;
            }
            else if (Map.Param.CommonParams.m_updown_buf.Count != 0 && Map.Param.CommonParams.m_dir_buf.Count != 0)
            {
                //階段通路両方
                if (Common.Math.RandomInt(0, 10) < 3)
                {
                    t_cell_info_dir = Map.Param.CommonParams.m_dir_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_dir_buf.Count)];
                    Map.Param.CommonParams.m_now_position = t_cell_info_dir.Point;
                }
                else
                {
                    t_cell_info_stair = Map.Param.CommonParams.m_updown_buf[Common.Math.RandomInt(0, Map.Param.CommonParams.m_updown_buf.Count)];
                    Map.Param.CommonParams.m_now_position = t_cell_info_stair.Point;
                }
            }
            else
            {
#if false // 継承先に分離する
                //分岐無し
                //Debug.Log("分岐無し");
                if (m_sequence == Sequence.DETAILED)
                {
                    m_branch_count = Common.Math.RandomInt(2, 5);
                    continue;
                }
#else
                if (DestinationEmptyProcces() == true)
                {
                    //処理スキップ
                    return true;
                }
#endif
                return false;
            }
            //UnityEngine.Debug.Log(Map.Param.CommonParams.m_now_position);
#if false // 継承先に分離する
            if (m_sequence == Sequence.DETAILED)
            {
                if (m_branch_count <= 0)
                {
                    m_branch_count = Common.Math.RandomInt(2, 5);
                    Map.Param.CommonParams.m_branch_buf.Clear();
                    Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
                    Map.Param.CommonParams.m_road_buf.Clear();
                    Map.Param.CommonParams.m_road_buf.Add(Map.Param.CommonParams.m_now_position);
                }

            }
#else
            BufReset();

#endif
            //Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);

            //忘れずに実装***********************************
            MakeCrank(Common.Math.RandomInt(2, 5));
#if false // 継承先に分離する
            //接続あるなら処理抜け
            if (m_sequence == Sequence.CONNECT || m_sequence == Sequence.CONNECT_AREA)
            {
                if (m_connect_flg == true)
                {
                    return false;
                }
                else
                {
                    i--;
                }
            }
#else
            if (IsConnectType() == true)
            {
                if (m_connect_flg == true)
                {
                    return false;
                }
                else
                {
                    m_index--;
                    //m_loop_skip_count = 1;//1回処理スキップ
                }
            }
#endif
            return true;
        }

        /// <summary>
        /// 延長可能調査
        /// </summary>
        /// <param name="branch">開始座標</param>
        public void CheckBranch(Point branch)
        {
            //Debug.Log("延長可能調査 " + branch.ToString());
            bool[] t_dir = new bool[(int)Direction.MAX_NUM];
            bool[] t_dir_area = new bool[(int)Direction.MAX_NUM];
            
            for (int i = 0; i < (int)Direction.MAX_NUM; i++)
            {

                if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(branch + Point.PointCorrection((Direction)i)))
                {

                    t_dir[i] = true;
                }


                if (IsStraight(branch, (Direction)i))
                {
                    //Debug.Log(((Direction)i).ToString());
                    t_dir_area[i] = true;

                }
            }
            
            //周囲の接続数のカウント
            int t_around_count = (t_dir[(int)Direction.LEFT] ? 1 : 0) +
                (t_dir[(int)Direction.RIGHT] ? 1 : 0) +
                (t_dir[(int)Direction.FRONT] ? 1 : 0) +
                (t_dir[(int)Direction.BACK] ? 1 : 0);

            int t_around_connect_count = (Map.Param.CommonParams.GetCellData(branch).m_left == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_right == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_front == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_back == Map.Cell.ConnectType.CONNECT ? 1 : 0);

            //階段カウント数
            int t_stair_count = (t_dir[(int)Direction.UP] ? 1 : 0) +
                (t_dir[(int)Direction.DOWN] ? 1 : 0);

            //階段フラグ
            bool t_chk_stair = Map.Param.CommonParams.GetCellData(branch).m_front == Map.Cell.ConnectType.CONNECT || Map.Param.CommonParams.GetCellData(branch).m_back == Map.Cell.ConnectType.CONNECT;

            //確率で上下配置を許可する
            if (Common.Math.RandomInt(0, 10) > 5)
            {
                if (t_chk_stair == true &&
                t_around_connect_count > 0 ||
                (Map.Param.CommonParams.GetCellData(branch).m_up == Map.Cell.ConnectType.CONNECT) ||
                (Map.Param.CommonParams.GetCellData(branch).m_down == Map.Cell.ConnectType.CONNECT)
                )
                {
                    //Debug.Log("UP DOWN 取り消し");
                    t_dir_area[(int)Direction.UP] = false;
                    t_dir_area[(int)Direction.DOWN] = false;
                }
            }


            //階段
            if (
                true ||
                t_chk_stair == false && t_around_count <= 1)
            {
                if (t_dir_area[(int)Direction.UP] == true
                    || t_dir_area[(int)Direction.DOWN] == true)
                {
                    //縦座標に接続可能の場合は上下座標を追加
                    Map.Param.CommonParams.m_updown_buf.Add(new Map.Cell.CellInfo(branch, t_dir_area));
                }
            }
            else
            {
                //縦座標を全て無効
                //Debug.Log("UP DOWN 取り消し");
                t_dir_area[(int)Direction.UP] = false;
                t_dir_area[(int)Direction.DOWN] = false;
            }


            //通路
            if (true || !t_chk_stair || t_chk_stair && t_around_count == 0)
            {
                if (t_dir_area[(int)Direction.LEFT] == true
                    || t_dir_area[(int)Direction.RIGHT] == true
                    || t_dir_area[(int)Direction.FRONT] == true
                    || t_dir_area[(int)Direction.BACK] == true)
                {
                    //横座標に接続可能の場合は通路座標を追加
                    Map.Param.CommonParams.m_dir_buf.Add(new Map.Cell.CellInfo(branch, t_dir_area));
                }
            }
            else
            {
                //横座標を全て無効
                //Debug.Log("RIGHT LEFT FRONT BACK 取り消し");
                t_dir_area[(int)Direction.RIGHT] = false;
                t_dir_area[(int)Direction.LEFT] = false;
                t_dir_area[(int)Direction.FRONT] = false;
                t_dir_area[(int)Direction.BACK] = false;
            }

            m_direction_arr = t_dir_area;

            //延長不可の為リストから削除
            if (
                t_dir_area[(int)Direction.RIGHT] == false &&
            t_dir_area[(int)Direction.LEFT] == false &&
            t_dir_area[(int)Direction.FRONT] == false &&
            t_dir_area[(int)Direction.BACK] == false &&
            t_dir_area[(int)Direction.UP] == false &&
            t_dir_area[(int)Direction.DOWN] == false &&
            (IsConnectType() == false)//未接続タイプ
                )
            {
                //接続可能ではないので削除
                Map.Param.CommonParams.m_road_buf.Remove(branch);
            }

            //Debug.Log("通路 " + Map.Param.CommonParams.m_dir_buf.Count.ToString());
            //Debug.Log("階段 " + Map.Param.CommonParams.m_updown_buf.Count.ToString());
        }

        /// <summary>
        /// 延長可能座標作製
        /// </summary>
        protected void SearchDirection()
        {

            Map.Param.CommonParams.m_dir_buf.Clear();
            Map.Param.CommonParams.m_updown_buf.Clear();

            //延長可能判定
            Map.Param.CommonParams.m_branch_buf.ForEach(branch => {

                CheckBranch(branch);

            });

            Map.Param.CommonParams.m_branch_buf = new List<Point>(Map.Param.CommonParams.m_road_buf);
        }


        //クランク処理
        void MakeCrank(int a_count)
        {
            //Debug.Log("**************クランク処理" + a_count.ToString());
            //未接続で初期化
            //m_connect_flg = false;

            for (int i = 0; i < a_count; i++)
            {
                Map.Param.CommonParams.m_dir_buf.Clear();
                Map.Param.CommonParams.m_updown_buf.Clear();
                CheckBranch(Map.Param.CommonParams.m_now_position);

                if (Map.Param.CommonParams.m_dir_buf.Count == 0 && Map.Param.CommonParams.m_updown_buf.Count == 0)
                {
                    //UnityEngine.Debug.Log("--クランク移動先無");
                    break;
                }

                //直進距離　デフォルト1
                int t_distance = 1;
                Map.Param.CommonParams.m_stair_flg = false;

                //通路か階段か選択
                if (Map.Param.CommonParams.m_updown_buf.Count == 0)
                {
                    //通路のみ
                    t_distance = Common.Math.RandomInt(1, 5);
                }
                else if (Map.Param.CommonParams.m_dir_buf.Count == 0)
                {
                    //階段のみ
                    Map.Param.CommonParams.m_stair_flg = true;
                    t_distance = 1;
                    //t_distance = Common.Math.RandomInt(1, 3);
                }
                else
                {
                    //階段通路両方
                    if (Common.Math.RandomInt(0, 10) < 3)
                    {
                        t_distance = Common.Math.RandomInt(1, 5);
                    }
                    else
                    {
                        Map.Param.CommonParams.m_stair_flg = true;
                        t_distance = 1;
                        //t_distance = Common.Math.RandomInt(1, 3);
                    }
                }

                //方向決定
                List<Direction> t_dir_list = new List<Direction>();
                if (Map.Param.CommonParams.m_stair_flg)
                {
                    //階段
                    if (m_direction_arr[0])
                    {
                        t_dir_list.Add(Direction.UP);
                    }
                    if (m_direction_arr[1])
                    {
                        t_dir_list.Add(Direction.DOWN);
                    }
                }
                else
                {
                    //通路
                    if (m_direction_arr[2])
                    {
                        t_dir_list.Add(Direction.LEFT);
                    }
                    if (m_direction_arr[3])
                    {
                        t_dir_list.Add(Direction.RIGHT);
                    }
                    if (m_direction_arr[4])
                    {
                        t_dir_list.Add(Direction.FRONT);
                    }
                    if (m_direction_arr[5])
                    {
                        t_dir_list.Add(Direction.BACK);
                    }
                }

                Direction t_direction = t_dir_list[Common.Math.RandomInt(0, t_dir_list.Count)];
                //Debug.Log("直進方向 " + t_direction.ToString());
                MakeRoad(t_distance, t_direction);

                //接続ステートで接続あるなら処理抜け
                //接続タプは継承先で実装
                if (IsConnectType() == true)
                {
                    if (m_connect_flg == true)
                    {
                        //m_loop_skip_count = 1;//1回処理スキップ
                        return;
                    }
                }
                //クランク可能が0なら処理を抜ける

            }
        }

        /// <summary>
        /// 道直進作製処理
        /// </summary>
        /// <param name="a_distance">直進セル数</param>
        /// <param name="a_direction">直進方向</param>
        void MakeRoad(int a_distance, Direction a_direction)
        {

            //Direction t_dir = GetDirection(Map.Param.CommonParams.m_now_position);
            //UnityEngine.Debug.Log(" 延長距離 " + a_distance.ToString());
            //通路延長
            for (int i = 0; i < a_distance; i++)
            {
                //接続ステートで接続可能判定
                if (IsConnectType() == true)
                {
                    if (Map.Env.MapEnv.IsConnected() == true)
                    {
                        //接続したので直進停止
                        m_connect_flg = true;
                        break;
                    }
                }

                if (IsStraight(Map.Param.CommonParams.m_now_position, a_direction))
                {
                    Map.Param.CommonParams.m_now_position += Point.PointCorrection(a_direction);
                    Map.Env.MapEnv.SetRoad(Map.Param.CommonParams.m_now_position, a_direction, GetStyleType());
                    Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
                    //Debug.Log(Map.Param.CommonParams.m_now_position.ToString());
                }
                else
                {
                    //Debug.Log("延長停止");
                    break;
                }

            }

        }
#if false
        /// <summary>
        /// 接続確認フラグ
        /// </summary>
        private bool m_connect_flg;

        /// <summary>
        /// 分岐バッファー開放処理
        /// </summary>
        /// <param name="a_branch">分岐座標</param>
        protected abstract void RemoveRoadBuf(Point a_branch);

        /// <summary>
        /// ビルダーパターン接続タイプ判定
        /// </summary>
        /// <returns>接続可能判定</returns>
        protected abstract bool IsConnectType();

        /// <summary>
        /// セル書き込みタイプ
        /// </summary>
        protected abstract Map.Cell.StateType IsStyleType();

        /// <summary>
        /// 延長可能調査
        /// </summary>
        /// <param name="branch">開始座標</param>
        public void CheckBranch(Point branch)
        {
            //Debug.Log("延長可能調査 " + branch.ToString());
            bool[] t_dir = new bool[(int)Direction.MAX_NUM];
            bool[] t_dir_area = new bool[(int)Direction.MAX_NUM];

            for (int i = 0; i < (int)Direction.MAX_NUM; i++)
            {

                if (Map.Param.CommonParams.m_map_area.IsRoadAndAreaIn(branch + Point.PointCorrection((Direction)i)))
                {

                    t_dir[i] = true;
                }


                if (IsStraight(branch, (Direction)i))
                {
                    //Debug.Log(((Direction)i).ToString());
                    t_dir_area[i] = true;

                }
            }

            //周囲の接続数のカウント
            int t_around_count = (t_dir[(int)Direction.LEFT] ? 1 : 0) +
                (t_dir[(int)Direction.RIGHT] ? 1 : 0) +
                (t_dir[(int)Direction.FRONT] ? 1 : 0) +
                (t_dir[(int)Direction.BACK] ? 1 : 0);

            int t_around_connect_count = (Map.Param.CommonParams.GetCellData(branch).m_left == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_right == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_front == Map.Cell.ConnectType.CONNECT ? 1 : 0) +
                (Map.Param.CommonParams.GetCellData(branch).m_back == Map.Cell.ConnectType.CONNECT ? 1 : 0);

            //階段カウント数
            int t_stair_count = (t_dir[(int)Direction.UP] ? 1 : 0) +
                (t_dir[(int)Direction.DOWN] ? 1 : 0);

            //階段フラグ
            bool t_chk_stair = Map.Param.CommonParams.GetCellData(branch).m_front == Map.Cell.ConnectType.CONNECT || Map.Param.CommonParams.GetCellData(branch).m_back == Map.Cell.ConnectType.CONNECT;

            //確率で上下配置を許可する
            if (Common.Math.RandomInt(0, 10) > 5)
            {
                if (t_chk_stair == true &&
                t_around_connect_count > 0 ||
                (Map.Param.CommonParams.GetCellData(branch).m_up == Map.Cell.ConnectType.CONNECT) ||
                (Map.Param.CommonParams.GetCellData(branch).m_down == Map.Cell.ConnectType.CONNECT)
                )
                {
                    //Debug.Log("UP DOWN 取り消し");
                    t_dir_area[(int)Direction.UP] = false;
                    t_dir_area[(int)Direction.DOWN] = false;
                }
            }


            //階段
            if (
                true ||
                t_chk_stair == false && t_around_count <= 1)
            {
                if (t_dir_area[(int)Direction.UP] == true
                    || t_dir_area[(int)Direction.DOWN] == true)
                {
                    //縦座標に接続可能の場合は上下座標を追加
                    Map.Param.CommonParams.m_updown_buf.Add(new Map.Cell.CellInfo(branch, t_dir_area));
                }
            }
            else
            {
                //縦座標を全て無効
                //Debug.Log("UP DOWN 取り消し");
                t_dir_area[(int)Direction.UP] = false;
                t_dir_area[(int)Direction.DOWN] = false;
            }


            //通路
            if (true || !t_chk_stair || t_chk_stair && t_around_count == 0)
            {
                if (t_dir_area[(int)Direction.LEFT] == true
                    || t_dir_area[(int)Direction.RIGHT] == true
                    || t_dir_area[(int)Direction.FRONT] == true
                    || t_dir_area[(int)Direction.BACK] == true)
                {
                    //横座標に接続可能の場合は通路座標を追加
                    Map.Param.CommonParams.m_dir_buf.Add(new Map.Cell.CellInfo(branch, t_dir_area));
                }
            }
            else
            {
                //横座標を全て無効
                //Debug.Log("RIGHT LEFT FRONT BACK 取り消し");
                t_dir_area[(int)Direction.RIGHT] = false;
                t_dir_area[(int)Direction.LEFT] = false;
                t_dir_area[(int)Direction.FRONT] = false;
                t_dir_area[(int)Direction.BACK] = false;
            }

            m_direction_arr = t_dir_area;
            
            //延長不可の為リストから削除
            if (
                t_dir_area[(int)Direction.RIGHT] == false &&
            t_dir_area[(int)Direction.LEFT] == false &&
            t_dir_area[(int)Direction.FRONT] == false &&
            t_dir_area[(int)Direction.BACK] == false &&
            t_dir_area[(int)Direction.UP] == false &&
            t_dir_area[(int)Direction.DOWN] == false &&
            (IsConnectType() == false)//未接続タイプ
                )
            {
                //接続可能ではないので削除
                Map.Param.CommonParams.m_road_buf.Remove(branch);
            }

            //Debug.Log("通路 " + Map.Param.CommonParams.m_dir_buf.Count.ToString());
            //Debug.Log("階段 " + Map.Param.CommonParams.m_updown_buf.Count.ToString());
        }

        //クランク処理
        void MakeCrank(int a_count)
        {
            //Debug.Log("**************クランク処理" + a_count.ToString());
            //未接続で初期化
            m_connect_flg = false;

            for (int i = 0; i < a_count; i++)
            {
                Map.Param.CommonParams.m_dir_buf.Clear();
                Map.Param.CommonParams.m_updown_buf.Clear();
                CheckBranch(Map.Param.CommonParams.m_now_position);

                if (Map.Param.CommonParams.m_dir_buf.Count == 0 && Map.Param.CommonParams.m_updown_buf.Count == 0)
                {
                    //Debug.Log("--クランク移動先無");
                    break;
                }

                //直進距離　デフォルト1
                int t_distance = 1;
                Map.Param.CommonParams.m_stair_flg = false;

                //通路か階段か選択
                if (Map.Param.CommonParams.m_updown_buf.Count == 0)
                {
                    //通路のみ
                    t_distance = Common.Math.RandomInt(1, 5);
                }
                else if (Map.Param.CommonParams.m_dir_buf.Count == 0)
                {
                    //階段のみ
                    Map.Param.CommonParams.m_stair_flg = true;
                    t_distance = 1;
                    //t_distance = Common.Math.RandomInt(1, 3);
                }
                else
                {
                    //階段通路両方
                    if (Common.Math.RandomInt(0, 10) < 3)
                    {
                        t_distance = Common.Math.RandomInt(1, 5);
                    }
                    else
                    {
                        Map.Param.CommonParams.m_stair_flg = true;
                        t_distance = 1;
                        //t_distance = Common.Math.RandomInt(1, 3);
                    }
                }

                //方向決定
                List<Direction> t_dir_list = new List<Direction>();
                if (Map.Param.CommonParams.m_stair_flg)
                {
                    //階段
                    if (m_direction_arr[0])
                    {
                        t_dir_list.Add(Direction.UP);
                    }
                    if (m_direction_arr[1])
                    {
                        t_dir_list.Add(Direction.DOWN);
                    }
                }
                else
                {
                    //通路
                    if (m_direction_arr[2])
                    {
                        t_dir_list.Add(Direction.LEFT);
                    }
                    if (m_direction_arr[3])
                    {
                        t_dir_list.Add(Direction.RIGHT);
                    }
                    if (m_direction_arr[4])
                    {
                        t_dir_list.Add(Direction.FRONT);
                    }
                    if (m_direction_arr[5])
                    {
                        t_dir_list.Add(Direction.BACK);
                    }
                }

                Direction t_direction = t_dir_list[Common.Math.RandomInt(0, t_dir_list.Count)];
                //Debug.Log("直進方向 " + t_direction.ToString());
                MakeRoad(t_distance, t_direction);

                //接続ステートで接続あるなら処理抜け
                //接続タプは継承先で実装
                if(IsConnectType() == true)
                {
                    if (m_connect_flg == true)
                    {
                        return;
                    }
                }
                //クランク可能が0なら処理を抜ける

            }

            /// <summary>
            /// 道直進作製処理
            /// </summary>
            /// <param name="a_distance">直進セル数</param>
            /// <param name="a_direction">直進方向</param>
            void MakeRoad(int a_distance, Direction a_direction)
            {

                //Direction t_dir = GetDirection(Map.Param.CommonParams.m_now_position);
                //Debug.Log(" 延長距離 " + a_distance.ToString());
                //通路延長
                for (int i = 0; i < a_distance; i++)
                {
                    //接続ステートで接続可能判定
                    if (IsConnectType() == true)
                    {
                        if (Map.Env.MapEnv.IsConnected() == true)
                        {
                            //接続したので直進停止
                            m_connect_flg = true;
                            break;
                        }
                    }

                    if (IsStraight(Map.Param.CommonParams.m_now_position, a_direction))
                    {
                        Map.Param.CommonParams.m_now_position += Point.PointCorrection(a_direction);
                        Map.Env.MapEnv.SetRoad(Map.Param.CommonParams.m_now_position, a_direction, IsStyleType());
                        Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
                        //Debug.Log(Map.Param.CommonParams.m_now_position.ToString());
                    }
                    else
                    {
                        //Debug.Log("延長停止");
                        break;
                    }

                }

            }

        }
#endif
    }
}
