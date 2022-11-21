using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 部屋のデータ
     */
namespace Map.Room
{
    sealed public class RoomData
    {
        public int m_room_no;
        public List<Point> m_cell_list;
        public bool m_shell_flg;//外部からの道を含めるかどうか

        /// <summary>
        /// 部屋の始点座標
        /// </summary>
        public Point m_area_start;

        /// <summary>
        /// 部屋の終点座標
        /// </summary>
        public Point m_area_end;
        
        public RoomData()
        {
            Init();
        }

        public void Init()
        {
            m_shell_flg = false;
            m_area_start = new Point( 0, 0, 0);
            m_area_end = new Point(0, 0, 0);
        }
    }

    
}
