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
	/// 螺旋階段床表示オブジェクト変更用
	/// </summary>
	public class RoadStairsSpiralGround : BaseInitModel
	{
		[SerializeField]
		public GameObject m_landing_front;
		[SerializeField]
		public GameObject m_landing_back;
		[SerializeField]
		public GameObject m_landing_right;
		[SerializeField]
		public GameObject m_landing_left;

		/// <summary>
		/// 色変更用のマテリアルリスト取得
		/// </summary>
		/// <returns></returns>
		protected override UnityEngine.Material[] GetMaterials()
		{
			return new UnityEngine.Material[]{
				m_landing_front.GetComponent<UnityEngine.MeshRenderer>()?.material,
				m_landing_back.GetComponent<UnityEngine.MeshRenderer>()?.material,
				m_landing_right.GetComponent<UnityEngine.MeshRenderer>()?.material,
				m_landing_left.GetComponent<UnityEngine.MeshRenderer>()?.material
			};
		}
		/// <summary>
		//// オブジェクトの表示を設定　継承先で実装
		/// </summary>
		/// <param name="a_data">セルデータ</param>
		/// <param name="a_sub_data">追加セルデータ</param>
		/// <returns>オブジェクトの設定の回転タイプ 条件一致しない場合はNONEを返す</returns>
		protected override BaseInitModel.RotType SetObject(Map.Direction a_direction, Map.Cell.CellData a_data, Map.Point a_point = null)
		{

			return BaseInitModel.RotType.RIGHT;
		}
	}
}