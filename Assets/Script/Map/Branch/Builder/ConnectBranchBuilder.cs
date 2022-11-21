using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Branch.Builder
{
    /// <summary>
    /// 接続分岐処理
    /// </summary>
    class ConnectBranchBuilder : BranchBuilderBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        protected override bool Init()
        {
            m_connect_flg = false;

            Map.Param.CommonParams.m_branch_buf.Clear();
            Map.Param.CommonParams.m_branch_buf.Add(Map.Param.CommonParams.m_now_position);
            //Map.Param.CommonParams.m_road_buf = new List<Point>(Map.Param.CommonParams.m_branch_buf);
            Map.Param.CommonParams.BranchBufCopyToRoadBuf();

            //Map.Param.CommonParams.m_simple_road_buf = new List<Point>(Map.Param.CommonParams.m_road_buf);
            Map.Param.CommonParams.RoadBufCopyToSimpleRoad();
            
            return true;
        }

        /// <summary>
        /// 接続タイプ判定
        /// </summary>
        /// <returns></returns>
        protected override bool IsConnectType()
        {
            //接続
            return true;
        }

        /// <summary>
        /// 接続タイプで分岐候補無しの処理
        /// </summary>
        protected override void BranchBufResetProcces()
        {

        }

        /// <summary>
        /// 接続タイプ方向候補無しの処理
        /// </summary>
        /// <returns></returns>
        protected override bool DestinationEmptyProcces()
        {
            return false;
        }

        /// <summary>
        /// 接続タイプバッファリセット処理
        /// </summary>
        protected override void BufReset()
        {
            //接続タイプはバッファリセット無し
        }

        /// <summary>
        /// セル書き込みタイプ
        /// </summary>
        override protected Map.Cell.StateType GetStyleType()
        {
            //書き込みタイプはBAN
            return Cell.StateType.BAN;
        }

        /// <summary>
        /// 直進条件
        /// </summary>
        /// <param name="a_x">x座標</param>
        /// <param name="a_y">y座標</param>
        /// <param name="a_z">z座標</param>
        /// <param name="a_dir">方向</param>
        /// <returns>true 直進可能 false 直進不可</returns>
        override protected (int, int, int, int, int, int) GetStaightParam(int a_x, int a_y, int a_z, Direction a_dir)
        {
            int t_x_min = 0;
            int t_x_max = 0;
            int t_y_min = 0;
            int t_y_max = 0;
            int t_z_min = 0;
            int t_z_max = 0;

            //各方向毎の判定座標パラメータ
            switch (a_dir)
            {
                case Direction.UP:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 1;
                    t_y_max = 2;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.DOWN:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = -1;
                    t_y_max = 0;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.LEFT:
                    t_x_min = -1;
                    t_x_max = 0;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.RIGHT:
                    t_x_min = 1;
                    t_x_max = 2;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 0;
                    t_z_max = 1;
                    break;
                case Direction.FRONT:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = 1;
                    t_z_max = 2;
                    break;
                case Direction.BACK:
                    t_x_min = 0;
                    t_x_max = 1;
                    t_y_min = 0;
                    t_y_max = 1;
                    t_z_min = -1;
                    t_z_max = 0;
                    break;
            }
            var t_ret = (t_x_min, t_x_max, t_y_min, t_y_max, t_z_min, t_z_max);
            return t_ret;
        }
    }
}
