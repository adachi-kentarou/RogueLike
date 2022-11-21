using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
namespace Map.Cell
{
    public enum eCellLoad
    {
        Right,
        Left,
        Front,
        Back,
        Top,
        Bottom
    }

    public class CellObj : MonoBehaviour
    {
        [SerializeField]
        public Transform m_right_obj;
        [SerializeField]
        public Transform m_left_obj;
        [SerializeField]
        public Transform m_front_obj;
        [SerializeField]
        public Transform m_back_obj;
        [SerializeField]
        public Transform m_top_obj;
        [SerializeField]
        public Transform m_bottom_obj;
        [SerializeField]
        public Transform m_right_up_obj;
        [SerializeField]
        public Transform m_right_down_obj;
        [SerializeField]
        public Transform m_left_up_obj;
        [SerializeField]
        public Transform m_left_down_obj;
        [SerializeField]
        public Transform m_front_up_obj;
        [SerializeField]
        public Transform m_front_down_obj;
        [SerializeField]
        public Transform m_back_up_obj;
        [SerializeField]
        public Transform m_back_down_obj;

        [SerializeField]
        public Transform m_right_panel_obj;
        [SerializeField]
        public Transform m_left_panel_obj;
        [SerializeField]
        public Transform m_front_panel_obj;
        [SerializeField]
        public Transform m_back_panel_obj;
        [SerializeField]
        public Transform m_top_panel_obj;
        [SerializeField]
        public Transform m_bottom_panel_obj;
        [SerializeField]
        public TextMesh m_text_obj;
        [SerializeField]
        public TextMesh m_area_text_obj;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Awake()
        {
            m_right_obj.gameObject.SetActive(false);
            m_left_obj.gameObject.SetActive(false);
            m_front_obj.gameObject.SetActive(false);
            m_back_obj.gameObject.SetActive(false);
            m_top_obj.gameObject.SetActive(false);
            m_bottom_obj.gameObject.SetActive(false);

            m_right_up_obj.gameObject.SetActive(false);
            m_right_down_obj.gameObject.SetActive(false);
            m_left_up_obj.gameObject.SetActive(false);
            m_left_down_obj.gameObject.SetActive(false);
            m_front_up_obj.gameObject.SetActive(false);
            m_front_down_obj.gameObject.SetActive(false);
            m_back_up_obj.gameObject.SetActive(false);
            m_back_down_obj.gameObject.SetActive(false);

            m_right_panel_obj.gameObject.SetActive(false);
            m_left_panel_obj.gameObject.SetActive(false);
            m_front_panel_obj.gameObject.SetActive(false);
            m_back_panel_obj.gameObject.SetActive(false);
            m_top_panel_obj.gameObject.SetActive(false);
            m_bottom_panel_obj.gameObject.SetActive(false);

        }
        public void SetLoad(Map.Direction a_load_type, bool a_visible)
        {
            switch (a_load_type)
            {
                case Map.Direction.RIGHT:
                    m_right_obj.gameObject.SetActive(a_visible);
                    break;
                case Map.Direction.LEFT:
                    m_left_obj.gameObject.SetActive(a_visible);
                    break;
                case Map.Direction.FRONT:
                    m_front_obj.gameObject.SetActive(a_visible);
                    break;
                case Map.Direction.BACK:
                    m_back_obj.gameObject.SetActive(a_visible);
                    break;
                case Map.Direction.UP:
                    m_top_obj.gameObject.SetActive(a_visible);
                    break;
                case Map.Direction.DOWN:
                    m_bottom_obj.gameObject.SetActive(a_visible);
                    break;
            }
        }

        public void SetRoadColor(Color a_color)
        {
            var t_cube_arr = GetComponentsInChildren<BoxCollider>();

            foreach (var a_mesh in t_cube_arr)
            {
                a_mesh.gameObject.GetComponent<MeshRenderer>().material.color = a_color;
            }
        }

        public void SetRoomColor(Color a_color)
        {

            var t_plane_arr = GetComponentsInChildren<MeshCollider>();

            a_color.a = 127;

            foreach (var a_mesh in t_plane_arr)
            {
                a_mesh.gameObject.GetComponent<MeshRenderer>().material.color = a_color;
            }
        }

        public void SetRoadNum(int a_num)
        {
            m_text_obj.text = a_num.ToString();
        }

        public void SetAreaNum(int a_num)
        {
            m_area_text_obj.text = a_num.ToString();
        }
        // Update is called once per frame
        /*
        void Update()
        {

        }
        */
    }

}
