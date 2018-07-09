#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER)
#define UNITY
#endif

#if UNITY
using UnityEngine;
#else
using System.Drawing;
#endif

namespace LibBSP {
#if !UNITY
	using Color32 = Color;
#endif
	/// <summary>
	/// Static class containing helper methods for <c>Color</c> objects.
	/// </summary>
	public static class Color32Extensions {
		
		/// <summary>
		/// Constructs a new <c>Color</c> from the passed values.
		/// </summary>
		/// <param name="a">Alpha component of the color.</param>
		/// <param name="r">Red component of the color.</param>
		/// <param name="g">Green component of the color.</param>
		/// <param name="b">Blue component of the color.</param>
		/// <returns>The resulting <c>Color</c> object.</returns>
		public static Color32 FromArgb(int a, int r, int g, int b) {
#if UNITY
			return new Color32((byte)r, (byte)g, (byte)b, (byte)a);
#else
			return Color.FromArgb(a, r, g, b);
#endif
		}

	}
}
