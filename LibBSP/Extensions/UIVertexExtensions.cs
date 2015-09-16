#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Static class containing helper methods for <c>UIVertex</c> objects.
	/// </summary>
	public static class UIVertexExtensions {

		/// <summary>
		/// Scales the position of this <c>UIVertex</c> by a number
		/// </summary>
		/// <param name="v1">This <c>UIVertex</c></param>
		/// <param name="scalar">Scalar</param>
		/// <returns>The scaled <c>UIVertex</c></returns>
		public static UIVertex Scale(this UIVertex v1, float scalar) {
			v1.position *= scalar;
			return v1;
		}

		/// <summary>
		/// Adds the position of this <c>UIVertex</c> to another <c>UIVertex</c>.
		/// </summary>
		/// <param name="v1">This <c>UIVertex</c></param>
		/// <param name="v2">The other <c>UIVertex</c></param>
		/// <returns>The resulting <c>UIVertex</c></returns>
		public static UIVertex Add(this UIVertex v1, UIVertex v2) {
			return new UIVertex {
				color = v1.color,
				normal = v1.normal,
				position = v1.position + v2.position,
				tangent = v1.tangent,
				uv0 = v1.uv0 + v2.uv0,
				uv1 = v1.uv1 + v2.uv1
			};
		}

		/// <summary>
		/// Adds the position of a <c>Vector3</c> to this <c>UIVertex</c>.
		/// </summary>
		/// <param name="v1">This <c>UIVertex</c></param>
		/// <param name="v2">The <c>Vector3</c></param>
		/// <returns>The resulting <c>UIVertex</c></returns>
		public static UIVertex Translate(this UIVertex v1, Vector3 v2) {
			return new UIVertex {
				color = v1.color,
				normal = v1.normal,
				position = v1.position + v2,
				tangent = v1.tangent,
				uv0 = v1.uv0,
				uv1 = v1.uv1
			};
		}

	}
}
