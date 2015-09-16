#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif
#if !(UNITY_4_6 || UNITY_5)
#if UNITY
using UnityEngine;
#else
using System.Drawing;
#endif
using System;

namespace LibBSP {
#if !UNITY
	using Color32 = Color;
	using Vector2 = Vector2d;
	using Vector3 = Vector3d;
	using Vector4 = Vector4d;
#endif
	/// <summary>
	/// Replacement UIVertex struct for Unity versions before 4.6, and standalone projects.
	/// </summary>
	/// <remarks>4.5 had UIVertex but the implementation was incomplete.</remarks>
	public struct UIVertex {
		public Color32 color;
		public Vector3 normal;
		public Vector3 position;
		public Vector4 tangent;
		public Vector2 uv0;
		public Vector2 uv1;

		public static UIVertex simpleVert {
			get {
				return new UIVertex {
					color = Color32Extensions.FromArgb(255, 255, 255, 255),
					normal = new Vector3(0, 0, -1),
					position = new Vector3(0, 0, 0),
					tangent = new Vector4(1, 0, 0, -1),
					uv0 = new Vector2(0, 0),
					uv1 = new Vector2(0, 0)
				};
			}
		}
	}
}
#endif
