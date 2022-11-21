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
	/// 部屋仕切り壁モデル表示オブジェクト変更用
	/// </summary>
	public class RoomDivisionWall : BaseInitModel
	{
		[SerializeField]
		public GameObject m_root;
		[SerializeField]
		public GameObject m_right_cube;
		[SerializeField]
		public GameObject m_center_cube;
		[SerializeField]
		public GameObject m_left_cube;

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
				m_root.SetActive(false);
			}
			else
			{
				m_root.SetActive(true);
				m_center_cube.SetActive(true);
				m_left_cube.SetActive(true);
				m_right_cube.SetActive(true);
			}

			return BaseInitModel.RotType.NONE;
		}
	}
}