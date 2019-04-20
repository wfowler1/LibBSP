#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace LibBSP {
#if UNITY
	using Vector2d = UnityEngine.Vector2;
#elif GODOT
	using Vector2d = Godot.Vector2;
#endif

	/// <summary>
	/// Class containing helper methods for <see cref="Vector2d"/> objects.
	/// </summary>
	public static class Vector2dExtensions {

#if !GODOT
		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">This <see cref="Vector2d"/>.</param>
		/// <param name="v2">The <see cref="Vector2d"/> to dot with this <see cref="Vector2d"/>.</param>
		/// <returns>Dot product of this <see cref="Vector2d"/> and <paramref name="v"/>.</returns>
		public static double Dot(this Vector2d v1, Vector2d v) {
			return Vector2d.Dot(v1, v);
		}
#endif

		/// <summary>
		/// Gets the magnitude of this <see cref="Vector2d"/>.
		/// </summary>
		/// <param name="v">This <see cref="Vector2d"/>.</param>
		/// <returns>The magnitude of this <see cref="Vector2d"/>.</returns>
		public static double GetMagnitude(this Vector2d v) {
#if GODOT
			return v.Length();
#else
			return v.magnitude;
#endif
		}

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
