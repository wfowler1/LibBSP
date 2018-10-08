#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#if !UNITY_5_6_OR_NEWER
// UIVertex was introduced in Unity 4.5 but it only had color, position and one UV.
// From 4.6.0 until 5.5.6 it was missing two sets of UVs.
#define OLDUNITY
#endif
#endif

#if !UNITY || OLDUNITY

using System;

namespace LibBSP {
#if UNITY
	using Color = UnityEngine.Color32;
	using Vector2d = UnityEngine.Vector2;
	using Vector3d = UnityEngine.Vector3;
	using Vector4d = UnityEngine.Vector4;
#else
	using Color = System.Drawing.Color;
#endif

	/// <summary>
	/// Vertex struct, including fields for normal, tangent and four sets of UVs.
	/// </summary>
	[Serializable] public struct Vertex {
		public Vector3d position;
		public Vector3d normal;
		public Color color;
		public Vector2d uv0;
		public Vector2d uv1;
		public Vector2d uv2;
		public Vector2d uv3;
		public Vector4d tangent;

		/// <summary>
		/// Simple Vertex with sensible settings.
		/// </summary>
		public static Vertex simpleVert {
			get {
				return new Vertex {
					color = ColorExtensions.FromArgb(255, 255, 255, 255),
					normal = new Vector3d(0, 0, -1),
					position = new Vector3d(0, 0, 0),
					tangent = new Vector4d(1, 0, 0, -1),
					uv0 = new Vector2d(0, 0),
					uv1 = new Vector2d(0, 0),
					uv2 = new Vector2d(0, 0),
					uv3 = new Vector2d(0, 0),
				};
			}
		}
	}
}
#endif
