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
	/// 通常道モデル表示オブジェクト変更用
	/// </summary>
	public class Road : MonoBehaviour , IcellInitModel
	{
	
		/***********************************************
		* 右壁
		************************************************/
		/// <summary>
		/// 右壁ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_right_root_panel;
		
		/// <summary>
		/// 右後壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_right_back_panel;
		
		/// <summary>
		/// 右中壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_right_center_panel;
		
		
		/// <summary>
		/// 右前壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_right_front_panel;

		/***********************************************
		* 下床
		************************************************/
		/// <summary>
		/// 下床パネル
		/// </summary>
		[SerializeField]
		public GameObject m_bottom_panel;
		
		/***********************************************
		* 左壁
		************************************************/
		/// <summary>
		/// 左壁ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_left_root_panel;
		
		/// <summary>
		/// 左後壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_left_back_panel;
		
		/// <summary>
		/// 左中壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_left_center_panel;
		
		/// <summary>
		/// 左前壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_left_front_panel;
		
		/***********************************************
		* 上天井
		************************************************/
		/// <summary>
		/// 上天井パネル
		/// </summary>
		[SerializeField]
		public GameObject m_top_panel;

		/***********************************************
		* 前壁
		************************************************/
		/// <summary>
		/// 前壁ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_front_root_panel;
		
		/// <summary>
		/// 前右壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_front_right_panel;
		
		/// <summary>
		/// 前中壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_front_center_panel;
		
		/// <summary>
		/// 前左壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_front_left_panel;
		
		/***********************************************
		* 後壁
		************************************************/
		/// <summary>
		/// 後壁ルートパネル
		/// </summary>
		[SerializeField]
		public GameObject m_back_root_panel;
		
		/// <summary>
		/// 後右壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_back_right_panel;
		
		/// <summary>
		/// 後中壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_back_center_panel;

		/// <summary>
		/// 後左壁パネル
		/// </summary>
		[SerializeField]
		public GameObject m_back_left_panel;


		/// <summary>
		/// モデルの初期化
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		public void InitModel(Map.Cell.CellData a_data, Map.Cell.CellData a_sub_data = null)
		{
			if (a_data.m_right == ConnectType.CONNECT) m_right_center_panel.SetActive(false);
			if (a_data.m_front == ConnectType.CONNECT) m_front_center_panel.SetActive(false);
			if (a_data.m_left == ConnectType.CONNECT) m_left_center_panel.SetActive(false);
			if (a_data.m_back == ConnectType.CONNECT) m_back_center_panel.SetActive(false);

			if (a_data.m_right == ConnectType.CONNECT && a_data.m_front != ConnectType.CONNECT) m_right_front_panel.SetActive(false);
			if (a_data.m_front == ConnectType.CONNECT && a_data.m_back != ConnectType.CONNECT) m_right_back_panel.SetActive(false);
			
			if (a_data.m_front == ConnectType.CONNECT && a_data.m_right != ConnectType.CONNECT) m_front_right_panel.SetActive(false);
			if (a_data.m_front == ConnectType.CONNECT && a_data.m_left != ConnectType.CONNECT) m_front_left_panel.SetActive(false);
			
			if (a_data.m_left == ConnectType.CONNECT && a_data.m_front != ConnectType.CONNECT) m_left_front_panel.SetActive(false);
			if (a_data.m_left == ConnectType.CONNECT && a_data.m_back != ConnectType.CONNECT) m_left_back_panel.SetActive(false);
			
			if (a_data.m_back == ConnectType.CONNECT && a_data.m_right != ConnectType.CONNECT) m_back_right_panel.SetActive(false);
			if (a_data.m_back == ConnectType.CONNECT && a_data.m_left != ConnectType.CONNECT) m_back_left_panel.SetActive(false);
			
		}

	}
}
#endif