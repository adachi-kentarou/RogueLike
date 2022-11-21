using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Map.Cell;

namespace Map.Model.Cell
{
	/// <summary>
	/// 部屋壁モデル表示オブジェクト変更用
	/// </summary>
	public class RoomWall : BaseInitModel
	{
		[SerializeField]
		public GameObject m_root;
		[SerializeField]
		public GameObject m_top_panel;
		[SerializeField]
		public GameObject m_back_panel;
		[SerializeField]
		public GameObject m_center_panel;
		[SerializeField]
		public GameObject m_front_panel;

		/// <summary>
		/// 色変更用のマテリアルリスト取得
		/// </summary>
		/// <returns></returns>
		protected override UnityEngine.Material[] GetMaterials()
		{
			return null;
		}
		
		/// <summary>
		//// オブジェクトの表示を設定　継承先で実装
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		/// <returns>オブジェクトの設定の回転タイプ 条件一致しない場合はNONEを返す</returns>
		protected override BaseInitModel.RotType SetObject(Map.Direction a_direction, Map.Cell.CellData a_data, Map.Point a_point = null)
		{
			// 接続判定
			if (a_data.GetConnect(a_direction) == Map.Cell.ConnectType.CONNECT)
			{
				m_center_panel.SetActive(false);
				m_front_panel.SetActive(true);
				m_back_panel.SetActive(true);
				m_top_panel.SetActive(true);
			}
			else
			{
				m_center_panel.SetActive(true);
				m_front_panel.SetActive(false);
				m_back_panel.SetActive(false);
				m_top_panel.SetActive(false);
			}
			
			return BaseInitModel.RotType.NONE;
		}

		public void SetWallColor(Color a_color)
		{
			var meshs = m_root.GetComponentsInChildren<MeshRenderer>();

			for (int i = 0; i < meshs.Length;i++)
			{
				meshs[i].material.color = a_color;
			}
		}
	}
}