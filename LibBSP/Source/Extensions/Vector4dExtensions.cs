#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector4d = Vector4;
#endif

	/// <summary>
	/// Class containing helper methods for <see cref="Vector4d"/> objects.
	/// </summary>
	public static class Vector4dExtensions {

		/// <summary>
		/// Gets a <c>byte</c> array representing the components of this <see cref="Vector4d"/> as <c>float</c>s.
		/// </summary>
		/// <param name="vector">This <see cref="Vector4d"/>.</param>
		/// <returns><c>byte</c> array with the components' bytes.</returns>
		public static byte[] GetBytes(this Vector4d vector) {
			byte[] ret = new byte[16];
			byte[] bytes = BitConverter.GetBytes((float)vector.x);
			bytes.CopyTo(ret, 0);
			bytes = BitConverter.GetBytes((float)vector.y);
			bytes.CopyTo(ret, 4);
			bytes = BitConverter.GetBytes((float)vector.z);
			bytes.CopyTo(ret, 8);
			bytes = BitConverter.GetBytes((float)vector.w);
			bytes.CopyTo(ret, 12);
			return ret;
		}

	}
}
