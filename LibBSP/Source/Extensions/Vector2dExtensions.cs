#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
# endif

namespace LibBSP {
#if UNITY
	using Vector2d = Vector2;
#endif

	/// <summary>
	/// Class containing helper methods for <see cref="Vector2d"/> objects.
	/// </summary>
	public static class Vector2dExtensions {

		/// <summary>
		/// Gets a <c>byte</c> array representing the components of this <see cref="Vector2d"/> as <c>float</c>s.
		/// </summary>
		/// <param name="vector">This <see cref="Vector2d"/>.</param>
		/// <returns><c>byte</c> array with the components' bytes.</returns>
		public static byte[] GetBytes(this Vector2d vector) {
			byte[] ret = new byte[8];
			byte[] bytes = BitConverter.GetBytes((float)vector.x);
			bytes.CopyTo(ret, 0);
			bytes = BitConverter.GetBytes((float)vector.y);
			bytes.CopyTo(ret, 4);
			return ret;
		}

	}
}
