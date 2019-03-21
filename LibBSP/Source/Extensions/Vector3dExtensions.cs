#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace LibBSP {
#if UNITY
	using Vector3d = UnityEngine.Vector3;
#elif GODOT
	using Vector3d = Godot.Vector3;
#endif

	/// <summary>
	/// Class containing helper methods for <see cref="Vector3d"/> objects.
	/// </summary>
	public static class Vector3dExtensions {

#if !GODOT
		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">This <see cref="Vector3d"/>.</param>
		/// <param name="v">The <see cref="Vector3d"/> to dot with this <see cref="Vector3d"/>.</param>
		/// <returns>Dot product of this <see cref="Vector3d"/> and <paramref name="v"/>.</returns>
		public static double Dot(this Vector3d v1, Vector3d v) {
			return Vector3d.Dot(v1, v);
		}

		/// <summary>
		/// Vector cross product. This operation is NOT commutative.
		/// </summary>
		/// <param name="v1">This <see cref="Vector3d"/>.</param>
		/// <param name="v">The <see cref="Vector3d"/> to have this <see cref="Vector3d"/> cross.</param>
		/// <returns>Cross product of these two vectors. Can be thought of as the normal to the plane defined by these two vectors.</returns>
		public static Vector3d Cross(this Vector3d v1, Vector3d v) {
			return Vector3d.Cross(v1, v);
		}
#endif

		/// <summary>
		/// Gets the magnitude of this <see cref="Vector3d"/>.
		/// </summary>
		/// <param name="v">This <see cref="Vector3d"/>.</param>
		/// <returns>The magnitude of this <see cref="Vector3d"/>.</returns>
		public static double GetMagnitude(this Vector3d v) {
#if GODOT
			return v.Length();
#else
			return v.magnitude;
#endif
		}

		/// <summary>
		/// Gets a <c>byte</c> array representing the components of this <see cref="Vector3d"/> as <c>float</c>s.
		/// </summary>
		/// <param name="vector">This <see cref="Vector3d"/>.</param>
		/// <returns><c>byte</c> array with the components' bytes.</returns>
		public static byte[] GetBytes(this Vector3d vector) {
			byte[] ret = new byte[12];
			byte[] bytes = BitConverter.GetBytes((float)vector.x);
			bytes.CopyTo(ret, 0);
			bytes = BitConverter.GetBytes((float)vector.y);
			bytes.CopyTo(ret, 4);
			bytes = BitConverter.GetBytes((float)vector.z);
			bytes.CopyTo(ret, 8);
			return ret;
		}

	}
}
