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
	/// 部屋天井モデル表示オブジェクト変更用
	/// </summary>
	public class RoadCeiling : BaseInitModel
	{
		[SerializeField]
		public GameObject m_right_panel;
		[SerializeField]
		public GameObject m_left_panel;
		[SerializeField]
		public GameObject m_front_panel;
		[SerializeField]
		public GameObject m_back_panel;

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
		protected override BaseInitModel.RotType SetObject(Map.Direction a_direction ,Map.Cell.CellData a_data, Map.Point a_point = null)
		{

			if (a_data.m_right != ConnectType.CONNECT)
			{
				m_right_panel.SetActive(false);
			}
			if (a_data.m_left != ConnectType.CONNECT)
			{
				m_left_panel.SetActive(false);
			}
			if (a_data.m_front != ConnectType.CONNECT)
			{
				m_front_panel.SetActive(false);
			}
			if (a_data.m_back != ConnectType.CONNECT)
			{
				m_back_panel.SetActive(false);
			}
			return BaseInitModel.RotType.RIGHT;
		}
	}
}