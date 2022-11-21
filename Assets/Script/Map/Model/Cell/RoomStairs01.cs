using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Map.Cell;
#if false
namespace Map.Model.Cell
{
	/// <summary>
	/// 通常道直進階段モデル表示オブジェクト変更用
	/// </summary>
	public class RoomStairs01 : MonoBehaviour, IcellInitModel
	{
		/***********************************************
		* 右壁
		************************************************/
		/// <summary>
		/// 上壁ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_up_root_panel;
		
		/// <summary>
		/// 中段床ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_middle_root_panel;
		
		/// <summary>
		/// 下壁床ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_down_root_panel;
		
		/// <summary>
		/// モデルの初期化
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		public void InitModel(Map.Cell.CellData a_data, Map.Cell.CellData a_sub_data = null)
		{
			bool t_up_room_flg = a_sub_data.m_area_no != 0;

			if (t_up_room_flg == true)
			{
				//上が部屋の場合
				m_up_root_panel.SetActive(false);
			}
			else
			{
				//上が通路の場合
				m_middle_root_panel.SetActive(false);
			}

/*
var rot = 0f;
                switch (t_connect_bit)
                {
                    case Cell.PanelBits.right://右
                        rot = -90f;
                        break;
                    case Cell.PanelBits.front://前
                        rot = 0f;
                        break;
                    case Cell.PanelBits.left://左
                        rot = 90f;
                        break;
                    case Cell.PanelBits.back://後
                        rot = 180f;
                        break;
                }
*/			
			//通路接続方向に合わせて回転させる
			var rot = 0f;
			if (a_data.m_right == ConnectType.CONNECT)
			{
				rot = 90f;
			}
			else if (a_data.m_front == ConnectType.CONNECT)
			{
				rot = 0f;
			}
			else if (a_data.m_front == ConnectType.CONNECT)
			{
				rot = -90f;
			}
			else if (a_data.m_front == ConnectType.CONNECT)
			{
				rot = 180f;
			}
			this.transform.rotation = Quaternion.Euler(0f, 0f, rot);
		}

	}
}
#endif