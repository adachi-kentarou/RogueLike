using System;
using System.Collections.Generic;
/**
 マップの１セルのデータ
     */
namespace Map.Cell
{
    class CellInfo
    {

        private Point m_point;
        public Point Point { get { return m_point; } }
        private bool[] m_dir;
        public bool[] Dir { get { return m_dir; } }

        public CellInfo(Point a_pos, bool[] a_dir)
        {
            m_point = a_pos;
            m_dir = a_dir;
        }
    }
}
