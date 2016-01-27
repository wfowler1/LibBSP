#if !(UNITY || UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace LibBSP {
	using Color32 = Color;
	/// <summary>
	/// Holds three <c>double</c>s representing a 3-dimensional vector.
	/// </summary>
	[Serializable] public struct Vector3d : IEquatable<Vector3d>, IEnumerable, IEnumerable<double> {

		/// <summary>Returns <see cref="Vector3d"/>(NaN, NaN, NaN).</summary>
		public static Vector3d undefined { get { return new Vector3d(System.Double.NaN, System.Double.NaN, System.Double.NaN); } }
		/// <summary>Returns <see cref="Vector3d"/>(1, 0, 0).</summary>
		public static Vector3d right { get { return new Vector3d(1, 0, 0); } }
		/// <summary>Returns <see cref="Vector3d"/>(0, 1, 0).</summary>
		public static Vector3d forward { get { return new Vector3d(0, 1, 0); } }
		/// <summary>Returns <see cref="Vector3d"/>(0, 0, 1).</summary>
		public static Vector3d up { get { return new Vector3d(0, 0, 1); } }
		/// <summary>Returns <see cref="Vector3d"/>(-1, 0, 0).</summary>
		public static Vector3d left { get { return new Vector3d(-1, 0, 0); } }
		/// <summary>Returns <see cref="Vector3d"/>(0, -1, 0).</summary>
		public static Vector3d back { get { return new Vector3d(0, -1, 0); } }
		/// <summary>Returns <see cref="Vector3d"/>(0, 0, -1).</summary>
		public static Vector3d down { get { return new Vector3d(0, 0, -1); } }
		/// <summary>Returns <see cref="Vector3d"/>(0, 0, 0).</summary>
		public static Vector3d zero { get { return new Vector3d(0, 0, 0); } }
		/// <summary>Returns <see cref="Vector3d"/>(1, 1, 1).</summary>
		public static Vector3d one { get { return new Vector3d(1, 1, 1); } }

		public double x;
		public double y;
		public double z;

		/// <summary>
		/// Gets or sets a component using an indexer, x=0, y=1, z=2.
		/// </summary>
		/// <param name="index">Component to get or set.</param>
		/// <returns>Component.</returns>
		/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> was negative or greater than 2.</exception>
		public double this[int index] {
			get {
				switch (index) {
					case 0: {
						return x;
					}
					case 1: {
						return y;
					}
					case 2: {
						return z;
					}
					default: {
						throw new IndexOutOfRangeException();
					}
				}
			}
			set {
				switch (index) {
					case 0: {
						x = value;
						break;
					}
					case 1: {
						y = value;
						break;
					}
					case 2: {
						z = value;
						break;
					}
					default: {
						throw new IndexOutOfRangeException();
					}
				}
			}
		}

		/// <summary>
		/// Gets the magnitude of this <see cref="Vector3d"/>, or its distance from (0, 0, 0).
		/// </summary>
		public double magnitude { get { return Math.Sqrt(sqrMagnitude); } }

		/// <summary>
		/// Gets the magnitude of this <see cref="Vector3d"/> squared. This is useful for when you are comparing the lengths of two vectors
		/// but don't need to know the exact length, and avoids calculating a square root.
		/// </summary>
		public double sqrMagnitude { get { return System.Math.Pow(x, 2) + System.Math.Pow(y, 2) + System.Math.Pow(z, 2); } }

		/// <summary>
		/// Gets the normalized version of this <see cref="Vector3d"/> (unit vector with the same direction).
		/// </summary>
		public Vector3d normalized {
			get {
				if (this == Vector3d.zero) { return Vector3d.zero; }
				double magnitude = this.magnitude;
				return new Vector3d(x / magnitude, y / magnitude, z / magnitude);
			}
		}

		/// <summary>
		/// Creates a new <see cref="Vector3d"/> object using elements in the passed array as components.
		/// </summary>
		/// <param name="point">Components of the vector.</param>
		public Vector3d(params float[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			z = 0;
			if (point.Length >= 3) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
				z = Convert.ToDouble(point[2]);
			} else if (point.Length == 2) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
			} else if (point.Length == 1) {
				x = Convert.ToDouble(point[0]);
			}
		}

		/// <summary>
		/// Creates a new <see cref="Vector3d"/> object using elements in the passed array as components.
		/// </summary>
		/// <param name="point">Components of the vector.</param>
		public Vector3d(params double[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			z = 0;
			if (point.Length >= 3) {
				x = point[0];
				y = point[1];
				z = point[2];
			} else if (point.Length == 2) {
				x = point[0];
				y = point[1];
			} else if (point.Length == 1) {
				x = point[0];
			}
		}

		/// <summary>
		/// Creates a new <see cref="Vector3d"/> object using elements in the passed array as components.
		/// </summary>
		/// <param name="point">Components of the vector.</param>
		public Vector3d(params int[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			z = 0;
			if (point.Length >= 3) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
				z = Convert.ToDouble(point[2]);
			} else if (point.Length == 2) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
			} else if (point.Length == 1) {
				x = Convert.ToDouble(point[0]);
			}
		}

		/// <summary>
		/// Crates a new <see cref="Vector3d"/> instance using the components from the supplied <see cref="Vector3d"/>.
		/// </summary>
		/// <param name="vector">Vector to copy components from.</param>
		public Vector3d(Vector3d vector) {
			x = vector.x;
			y = vector.y;
			z = vector.z;
		}

		/// <summary>
		/// Adds two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector to add.</param>
		/// <param name="v2">Second vector to add.</param>
		/// <returns>The resulting vector.</returns>
		public static Vector3d operator +(Vector3d v1, Vector3d v2) {
			return Add(v1, v2);
		}

		/// <summary>
		/// Adds two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector to add.</param>
		/// <param name="v2">Second vector to add.</param>
		/// <returns>The resulting vector.</returns>
		public static Vector3d Add(Vector3d v1, Vector3d v2) {
			return new Vector3d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
		}

		/// <summary>
		/// Subtracts one vector from another. This operation is NOT commutative.
		/// </summary>
		/// <param name="v1">Vector to subtract from.</param>
		/// <param name="v2">Vector to subtract.</param>
		/// <returns>Difference from <paramref name="v1"/> to <paramref name="v2"/>.</returns>
		public static Vector3d operator -(Vector3d v1, Vector3d v2) {
			return Subtract(v1, v2);
		}

		/// <summary>
		/// Subtracts one vector from another. This operation is NOT commutative.
		/// </summary>
		/// <param name="v1">Vector to subtract from.</param>
		/// <param name="v2">Vector to subtract.</param>
		/// <returns>Difference from <paramref name="v1"/> to <paramref name="v2"/>.</returns>
		public static Vector3d Subtract(Vector3d v1, Vector3d v2) {
			return new Vector3d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
		}

		/// <summary>
		/// Returns the negative of this vector. Equivalent to (0, 0, 0) - <paramref name="v1"/>.
		/// </summary>
		/// <param name="v1">Vector to negate.</param>
		/// <returns><paramref name="v1"/> with all components negated.</returns>
		public static Vector3d operator -(Vector3d v1) {
			return Negate(v1);
		}

		/// <summary>
		/// Returns the negative of this vector. Equivalent to (0, 0, 0) - <paramref name="v1"/>.
		/// </summary>
		/// <param name="v1">Vector to negate.</param>
		/// <returns><paramref name="v1"/> with all components negated.</returns>
		public static Vector3d Negate(Vector3d v1) {
			return new Vector3d(-v1.x, -v1.y, -v1.z);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1"/> by <paramref name="scalar"/> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to scale.</param>
		/// <param name="scalar">Scalar.</param>
		/// <returns>Resulting Vector.</returns>
		public static Vector3d operator *(Vector3d v1, double scalar) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1"/> by <paramref name="scalar"/> and returns the result.
		/// </summary>
		/// <param name="scalar">Scalar.</param>
		/// <param name="v1">Vector to scale.</param>
		/// <returns>Resulting Vector.</returns>
		public static Vector3d operator *(double scalar, Vector3d v1) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1"/> by <paramref name="scalar"/> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to scale.</param>
		/// <param name="scalar">Scalar.</param>
		/// <returns>Resulting Vector.</returns>
		public static Vector3d Scale(Vector3d v1, double scalar) {
			return new Vector3d(v1.x * scalar, v1.y * scalar, v1.z * scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1"/> by <paramref name="scalar"/> and returns the result.
		/// </summary>
		/// <param name="scalar">Scalar.</param>
		/// <param name="v1">Vector to scale.</param>
		/// <returns>Resulting Vector.</returns>
		public static Vector3d Scale(double scalar, Vector3d v1) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Multiplies two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>Resulting vector when the passed vectors' components are multiplied.</returns>
		public static Vector3d Scale(Vector3d v1, Vector3d v2) {
			return new Vector3d(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>Dot product of these two vectors.</returns>
		public static double operator *(Vector3d v1, Vector3d v2) {
			return Dot(v1, v2);
		}

		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>Dot product of these two vectors.</returns>
		public static double Dot(Vector3d v1, Vector3d v2) {
			return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
		}

		/// <summary>
		/// Vector cross product. This operation is NOT commutative.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>Cross product of these two vectors. Can be thought of as the normal to the plane defined by these two vectors.</returns>
		public static Vector3d operator ^(Vector3d v1, Vector3d v2) {
			return Cross(v1, v2);
		}

		/// <summary>
		/// Vector cross product. This operation is NOT commutative.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns>Cross product of these two vectors. Can be thought of as the normal to the plane defined by these two vectors.</returns>
		public static Vector3d Cross(Vector3d v1, Vector3d v2) {
			return new Vector3d(v1.y * v2.z - v2.y * v1.z, v2.x * v1.z - v1.x * v2.z, v1.x * v2.y - v2.x * v1.y);
		}

		/// <summary>
		/// Scalar division. Divides all components of <paramref name="v1"/> by <paramref name="divisor"/> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to divide.</param>
		/// <param name="divisor">Divisor.</param>
		/// <returns>Resulting vector when all components of <paramref name="v1"/> are divided by <paramref name="divisor"/>.</returns>
		public static Vector3d operator /(Vector3d v1, double divisor) {
			return Scale(v1, 1.0 / divisor);
		}

		#region IEquatable
		/// <summary>
		/// Equivalency. Returns <c>true</c> if the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns><c>true</c> if the components of two vectors are equal or approximately equal.</returns>
		public static bool operator ==(Vector3d v1, Vector3d v2) {
			return v1.Equals(v2);
		}

		/// <summary>
		/// Non-Equivalency. Returns <c>true</c> if the components of two vectors are not equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns><c>true</c> if the components of two vectors are not equal or approximately equal.</returns>
		public static bool operator !=(Vector3d v1, Vector3d v2) {
			return !v1.Equals(v2);
		}

		/// <summary>
		/// Equivalency. Returns <c>true</c> if the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns><c>true</c> if the components of two vectors are equal or approximately equal.</returns>
		public bool Equals(Vector3d other) {
			return (Math.Abs(x - other.x) < 0.001 && Math.Abs(y - other.y) < 0.001 && Math.Abs(z - other.z) < 0.001);
		}

		/// <summary>
		/// Equivalency. Returns <c>true</c> if the other object is a vector, and the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector.</param>
		/// <param name="v2">Second vector.</param>
		/// <returns><c>true</c> if the other object is a vector, and the components of two vectors are equal or approximately equal.</returns>
		public override bool Equals(object obj) {
			if (object.ReferenceEquals(obj, null) || !GetType().IsAssignableFrom(obj.GetType())) { return false; }
			return Equals((Vector3d)obj);
		}

		/// <summary>
		/// Generates a hash code for this instance based on instance data.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode() {
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Calculates the distance from this vector to another.
		/// </summary>
		/// <param name="to">Vector to calculate distance to.</param>
		/// <returns>Distance from this vector to the passed vector.</returns>
		public double Distance(Vector3d to) {
			return (this - to).magnitude;
		}

		/// <summary>
		/// Gets a human-readable <c>string</c> representation of this vector.
		/// </summary>
		/// <returns>Human-readable <c>string</c> representation of this vector.</returns>
		public override string ToString() {
			return string.Format("( {0} , {1} , {2} )", x.ToString(), y.ToString(), z.ToString());
		}

		/// <summary>
		/// Changes this vector to its normalized version (it will have a magnitude of 1).
		/// </summary>
		public void Normalize() {
			if (this == Vector3d.zero) { return; }
			double magnitude = this.magnitude;
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
		}

		/// <summary>
		/// Gets the area of the triangle defined by three points using Heron's formula.
		/// </summary>
		/// <param name="p1">First vertex of triangle.</param>
		/// <param name="p2">Second vertex of triangle.</param>
		/// <param name="p3">Third vertex of triangle.</param>
		/// <returns>Area of the triangle defined by these three vertices.</returns>
		public static double TriangleArea(Vector3d p1, Vector3d p2, Vector3d p3) {
			return Math.Sqrt(SqrTriangleArea(p1, p2, p3)) / 4.0;
		}

		/// <summary>
		/// Gets the square of the area of the triangle defined by three points. This is useful when simply comparing two areas when you don't need to know exactly what the area is.
		/// </summary>
		/// <param name="p1">First vertex of triangle.</param>
		/// <param name="p2">Second vertex of triangle.</param>
		/// <param name="p3">Third vertex of triangle.</param>
		/// <returns>Square of the area of the triangle defined by these three vertices.</returns>
		public static double SqrTriangleArea(Vector3d p1, Vector3d p2, Vector3d p3) {
			double a = p1.Distance(p2);
			double b = p1.Distance(p3);
			double c = p2.Distance(p3);
			return 4.0 * a * a * b * b - Math.Pow((a * a) + (b * b) - (c * c), 2);
		}

		#region IEnumerable
		/// <summary>
		/// Allows enumeration through the components of a <see cref="Vector3d"/> using a foreach loop.
		/// </summary>
		public IEnumerator<double> GetEnumerator() {
			yield return x;
			yield return y;
			yield return z;
		}

		/// <summary>
		/// Allows enumeration through the components of a <see cref="Vector3d"/> using a foreach loop, auto-boxed version.
		/// </summary>
		/// <remarks>
		/// This foreach loop will look like foreach(object o in Vector3d). This will auto-box the doubles in System.Double
		/// objects, allocating memory on the heap which the garbage collector will have to free later. In general, iterate
		/// through doubles rather than objects.
		/// </remarks>
		IEnumerator IEnumerable.GetEnumerator() {
			yield return x;
			yield return y;
			yield return z;
		}
		#endregion

		/// <summary>
		/// Implicitly converts this <see cref="Vector3d"/> into a <see cref="Vector2d"/>. This will be called when <c>Vector2d v2 = v3</c> is used.
		/// </summary>
		/// <param name="v"><see cref="Vector3d"/> to convert.</param>
		/// <returns>The input vector as a <see cref="Vector2d"/>, Z component discarded.</returns>
		public static implicit operator Vector2d(Vector3d v) {
			return new Vector2d(v.x, v.y);
		}

		/// <summary>
		/// Implicitly converts this <see cref="Vector3d"/> into a <see cref="Vector4d"/>. This will be called when <c>Vector4d v4 = v3</c> is used.
		/// </summary>
		/// <param name="v"><see cref="Vector3d"/> to convert.</param>
		/// <returns>The input vector as a <see cref="Vector4d"/>, W component set to 0.</returns>
		public static implicit operator Vector4d(Vector3d v) {
			return new Vector4d(v.x, v.y, v.z, 0);
		}

		/// <summary>
		/// Implicitly converts this <see cref="Vector3d"/> into a <c>Color</c> by interpreting (x, y, z) as (r, g, b) respectively, and alpha set to 100%.
		/// </summary>
		/// <param name="v"><see cref="Vector3d"/> to convert.</param>
		/// <returns>This <see cref="Vector3d"/> in a <c>Color</c> object interpreted as RGB.</returns>
		public static implicit operator Color32(Vector3d v) {
			return Color32Extensions.FromArgb(255, (int)Math.Max(v.x, 255), (int)Math.Max(v.y, 255), (int)Math.Max(v.z, 255));
		}
	}
}
#endif
