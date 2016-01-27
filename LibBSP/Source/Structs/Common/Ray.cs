#if !(UNITY || UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
using System;

namespace LibBSP {
	/// <summary>
	/// A struct for a <see cref="Ray"/> defined by a starting point and a direction vector.
	/// </summary>
	public struct Ray : IEquatable<Ray> {

		public Vector3d origin;
		private Vector3d _direction;

		public Vector3d direction {
			get { return _direction; }
			set {
				_direction = value.normalized;
			}
		}

		/// <summary>
		/// Creates a new <see cref="Ray"/> object using the specified origin and direction.
		/// </summary>
		/// <param name="origin">Origin point of this <see cref="Ray"/>.</param>
		/// <param name="direction">Direction vector of this <see cref="Ray"/>.</param>
		public Ray(Vector3d origin, Vector3d direction) {
			this.origin = origin;
			this._direction = direction;
			_direction.Normalize();
		}

		/// <summary>
		/// Gets the point at <paramref name="distance"/> units along this <see cref="Ray"/>.
		/// </summary>
		/// <param name="distance">Distance of the point to get.</param>
		/// <returns>The point at <paramref name="distance"/> units along this <see cref="Ray"/>.</returns>
		public Vector3d GetPoint(double distance) {
			return origin + (distance * direction);
		}

		/// <summary>
		/// Gets a nicely formatted <c>string</c> representation of this <see cref="Ray"/>.
		/// </summary>
		/// <returns>A nicely formatted <c>string</c> representation of this <see cref="Ray"/>.</returns>
		public override string ToString() {
			return string.Format("( {0}, {1} )", origin, direction);
		}

		#region IEquatable
		/// <summary>
		/// Determines whether this <see cref="Ray"/> is equivalent to another.
		/// </summary>
		/// <param name="r1">A <see cref="Ray"/> to compare.</param>
		/// <param name="r2">A <see cref="Ray"/> to compare.</param>
		/// <returns><c>true</c> if <paramref name="r1"/> and <paramref name="r2"/> have the same <see cref="Ray.origin"/> and <see cref="Ray.direction"/>.</returns>
		public static bool operator ==(Ray r1, Ray r2) {
			return r1.Equals(r2);
		}

		/// <summary>
		/// Determines whether this <see cref="Ray"/> is not equivalent to another.
		/// </summary>
		/// <param name="r1">A <see cref="Ray"/> to compare.</param>
		/// <param name="r2">A <see cref="Ray"/> to compare.</param>
		/// <returns><c>true</c> if <paramref name="r1"/> and <paramref name="r2"/> don't have the same <see cref="Ray.origin"/> and <see cref="Ray.direction"/>.</returns>
		public static bool operator !=(Ray r1, Ray r2) {
			return !r1.Equals(r2);
		}

		/// <summary>
		/// Determines whether this <see cref="Ray"/> is equivalent to another.
		/// </summary>
		/// <param name="other">The <see cref="Ray"/> to compare to.</param>
		/// <returns><c>true</c> if this <see cref="Ray"/> and <paramref name="other"/> have the same <see cref="Ray.origin"/> and <see cref="Ray.direction"/>.</returns>
		public bool Equals(Ray other) {
			return origin.Equals(other.origin) && direction.Equals(other.direction);
		}

		/// <summary>
		/// Determines whether this <see cref="Ray"/> is equivalent to another <c>object</c>.
		/// </summary>
		/// <param name="obj">The <c>object</c> to compare to.</param>
		/// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Ray"/> and this <see cref="Ray"/> and <paramref name="obj"/> have the same <see cref="Ray.origin"/> and <see cref="Ray.direction"/>.</returns>
		public override bool Equals(object obj) {
			if (object.ReferenceEquals(obj, null) || !GetType().IsAssignableFrom(obj.GetType())) { return false; }
			return Equals((Ray)obj);
		}

		/// <summary>
		/// Generates a hash code for this instance based on instance data.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode() {
			return origin.GetHashCode() ^ direction.GetHashCode();
		}
		#endregion

	}
}
#endif
