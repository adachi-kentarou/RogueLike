using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 道の階層構造体
     */
namespace Map
{
    sealed public class RoadStructure
    {
        public static List<RoadStructure> m_road_structure_list = new List<RoadStructure>();
        public static int m_list_count = 0;

        public int m_num;
        public Point m_origin;
        public List<Point> m_road_list = new List<Point>();
        public RoadStructure m_parent = null;
        public RoadStructure m_child_up;
        public RoadStructure m_child_down;
        public RoadStructure m_child_left;
        public RoadStructure m_child_right;
        public RoadStructure m_child_front;
        public RoadStructure m_child_back;
        
        public static void Init()
        {
            m_road_structure_list = new List<RoadStructure>();
            m_list_count = 0;
        }

        public RoadStructure()
        {
            m_num = m_list_count;
            m_list_count++;

        }

        public List<RoadStructure> GetParentList()
        {
            List<RoadStructure> t_list = new List<RoadStructure>();
            t_list.Add(this);
            
            if (m_num != 0)
            {
                //List<RoadStructure> t_list2 = m_parent.GetParentList();
                //t_list2.AddRange(t_list);
                //t_list = t_list2;
                t_list.AddRange(m_parent.GetParentList());
            }
            
            return t_list;
        }
        
    }
}
