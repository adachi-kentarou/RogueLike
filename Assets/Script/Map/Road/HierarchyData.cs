using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Road
{
    /// <summary>
    /// 道階層データ
    /// </summary>
    public class HierarchyData
    {
        public HierarchyData(Point a_point, Direction a_direction, int a_extend)
        {
            m_point = a_point;
            m_direction = a_direction;
            m_extend = a_extend;
        }
        public Point m_point;
        public Direction m_direction;
        public int m_extend;

    }
}
