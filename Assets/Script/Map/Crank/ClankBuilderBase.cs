using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Crank
{
    /// <summary>
    /// 通路方向変更処理ビルダーパターン基底クラス
    /// </summary>
    public abstract class CrankBuilderBase
    {
        protected abstract void Proccess();

        protected int m_branch_count = 0;

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
        protected abstract bool IsStaightFunc(int a_x, int a_y, int a_z, Direction a_dir);

        public void CreateCrank(int a_count)
        {
            //ループ処理
            for (int i = 0; i < a_count; i++)
            {
                //実際の処理は継承先でやる
                Proccess();
            }
        }

        protected abstract void RemoveRoadBuf(Point a_branch);

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
            
        }
    }
}
