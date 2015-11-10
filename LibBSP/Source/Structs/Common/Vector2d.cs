#if !(UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
using System;
using System.Collections;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds two <c>double</c>s representing a 2-dimensional vector.
	/// </summary>
	[Serializable] public struct Vector2d : IEquatable<Vector2d>, IEnumerable, IEnumerable<double> {

		/// <summary>Returns <c>Vector2d</c>(NaN, NaN, NaN)</summary>
		public static Vector2d undefined { get { return new Vector2d(System.Double.NaN, System.Double.NaN); } }
		/// <summary>Returns <c>Vector2d</c>(1, 0)</summary>
		public static Vector2d right { get { return new Vector2d(1, 0); } }
		/// <summary>Returns <c>Vector2d</c>(0, 1)</summary>
		public static Vector2d up { get { return new Vector2d(0, 1); } }
		/// <summary>Returns <c>Vector2d</c>(-1, 0)</summary>
		public static Vector2d left { get { return new Vector2d(-1, 0); } }
		/// <summary>Returns <c>Vector2d</c>(0, -1)</summary>
		public static Vector2d down { get { return new Vector2d(0, -1); } }
		/// <summary>Returns <c>Vector2d</c>(0, 0)</summary>
		public static Vector2d zero { get { return new Vector2d(0, 0); } }
		/// <summary>Returns <c>Vector2d</c>(1, 1)</summary>
		public static Vector2d one { get { return new Vector2d(1, 1); } }

		public double x;
		public double y;

		/// <summary>
		/// Gets or sets a component using an indexer, x=0, y=1
		/// </summary>
		/// <param name="index">Component to get or set</param>
		/// <returns>Component</returns>
		/// <exception cref="IndexOutOfRangeException"><paramref name="index" /> was negative or greater than 1</exception>
		public double this[int index] {
			get {
				switch (index) {
					case 0: {
						return x;
					}
					case 1: {
						return y;
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
					default: {
						throw new IndexOutOfRangeException();
					}
				}
			}
		}

		/// <summary>
		/// Gets the magnitude of this <c>Vector2d</c>, or its distance from (0, 0).
		/// </summary>
		public double magnitude { get { return Math.Sqrt(sqrMagnitude); } }

		/// <summary>
		/// Gets the magnitude of this <c>Vector2d</c> squared. This is useful for when you are comparing the lengths of two vectors
		/// but don't need to know the exact length, and avoids a square root.
		/// </summary>
		public double sqrMagnitude { get { return System.Math.Pow(x, 2) + System.Math.Pow(y, 2); } }

		/// <summary>
		/// Gets the normalized version of this <c>Vector2d</c> (unit vector with the same direction).
		/// </summary>
		public Vector2d normalized {
			get {
				if (this == Vector2d.zero) { return Vector2d.zero; }
				double magnitude = this.magnitude;
				return new Vector2d(x / magnitude, y / magnitude);
			}
		}

		/// <summary>
		/// Creates a new <c>Vector2d</c> object using elements in the passed array as components
		/// </summary>
		/// <param name="point">Components of the vector</param>
		public Vector2d(params float[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			if (point.Length == 2) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
			} else if (point.Length == 1) {
				x = Convert.ToDouble(point[0]);
			}
		}

		/// <summary>
		/// Creates a new <c>Vector2d</c> object using elements in the passed array as components
		/// </summary>
		/// <param name="point">Components of the vector</param>
		public Vector2d(params double[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			if (point.Length == 2) {
				x = point[0];
				y = point[1];
			} else if (point.Length == 1) {
				x = point[0];
			}
		}

		/// <summary>
		/// Creates a new <c>Vector2d</c> object using elements in the passed array as components
		/// </summary>
		/// <param name="point">Components of the vector</param>
		public Vector2d(params int[] point) {
			if (point == null) {
				throw new ArgumentNullException();
			}
			x = 0;
			y = 0;
			if (point.Length == 2) {
				x = Convert.ToDouble(point[0]);
				y = Convert.ToDouble(point[1]);
			} else if (point.Length == 1) {
				x = Convert.ToDouble(point[0]);
			}
		}

		/// <summary>
		/// Crates a new <c>Vector2d</c> instance using the components from the supplied <c>Vector2d</c>
		/// </summary>
		/// <param name="vector">Vector to copy components from</param>
		public Vector2d(Vector2d vector) {
			x = vector.x;
			y = vector.y;
		}

		/// <summary>
		/// Adds two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector to add</param>
		/// <param name="v2">Second vector to add</param>
		/// <returns>The resulting vector</returns>
		public static Vector2d operator +(Vector2d v1, Vector2d v2) {
			return Add(v1, v2);
		}

		/// <summary>
		/// Adds two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector to add</param>
		/// <param name="v2">Second vector to add</param>
		/// <returns>The resulting vector</returns>
		public static Vector2d Add(Vector2d v1, Vector2d v2) {
			return new Vector2d(v1.x + v2.x, v1.y + v2.y);
		}

		/// <summary>
		/// Subtracts one vector from another. This operation is NOT commutative
		/// </summary>
		/// <param name="v1">Vector to subtract from</param>
		/// <param name="v2">Vector to subtract</param>
		/// <returns>Difference from <paramref name="v1" /> to <paramref name="v2" /></returns>
		public static Vector2d operator -(Vector2d v1, Vector2d v2) {
			return Subtract(v1, v2);
		}

		/// <summary>
		/// Subtracts one vector from another. This operation is NOT commutative
		/// </summary>
		/// <param name="v1">Vector to subtract from</param>
		/// <param name="v2">Vector to subtract</param>
		/// <returns>Difference from <paramref name="v1" /> to <paramref name="v2" /></returns>
		public static Vector2d Subtract(Vector2d v1, Vector2d v2) {
			return new Vector2d(v1.x - v2.x, v1.y - v2.y);
		}

		/// <summary>
		/// Returns the negative of this vector. Equivalent to (0, 0) - <paramref name="v1" />.
		/// </summary>
		/// <param name="v1">Vector to negate</param>
		/// <returns><paramref name="v1" /> with all components negated</returns>
		public static Vector2d operator -(Vector2d v1) {
			return Negate(v1);
		}

		/// <summary>
		/// Returns the negative of this vector. Equivalent to (0, 0) - <paramref name="v1" />.
		/// </summary>
		/// <param name="v1">Vector to negate</param>
		/// <returns><paramref name="v1" /> with all components negated</returns>
		public static Vector2d Negate(Vector2d v1) {
			return new Vector2d(-v1.x, -v1.y);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1" /> by <paramref name="scalar" /> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to scale</param>
		/// <param name="scalar">Scalar</param>
		/// <returns>Resulting Vector</returns>
		public static Vector2d operator *(Vector2d v1, double scalar) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1" /> by <paramref name="scalar" /> and returns the result.
		/// </summary>
		/// <param name="scalar">Scalar</param>
		/// <param name="v1">Vector to scale</param>
		/// <returns>Resulting Vector</returns>
		public static Vector2d operator *(double scalar, Vector2d v1) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1" /> by <paramref name="scalar" /> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to scale</param>
		/// <param name="scalar">Scalar</param>
		/// <returns>Resulting Vector</returns>
		public static Vector2d Scale(Vector2d v1, double scalar) {
			return new Vector2d(v1.x * scalar, v1.y * scalar);
		}

		/// <summary>
		/// Scalar multiplication. Multiplies all components of <paramref name="v1" /> by <paramref name="scalar" /> and returns the result.
		/// </summary>
		/// <param name="scalar">Scalar</param>
		/// <param name="v1">Vector to scale</param>
		/// <returns>Resulting Vector</returns>
		public static Vector2d Scale(double scalar, Vector2d v1) {
			return Scale(v1, scalar);
		}

		/// <summary>
		/// Multiplies two vectors together componentwise. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Resulting vector when the passed vectors' components are multiplied</returns>
		public static Vector2d Scale(Vector2d v1, Vector2d v2) {
			return new Vector2d(v1.x * v2.x, v1.y * v2.y);
		}

		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Dot product of these two vectors</returns>
		public static double operator *(Vector2d v1, Vector2d v2) {
			return Dot(v1, v2);
		}

		/// <summary>
		/// Vector dot product. This operation is commutative.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>Dot product of these two vectors</returns>
		public static double Dot(Vector2d v1, Vector2d v2) {
			return v1.x * v2.x + v1.y * v2.y;
		}

		/// <summary>
		/// Scalar division. Divides all components of <paramref name="v1" /> by <paramref name="divisor" /> and returns the result.
		/// </summary>
		/// <param name="v1">Vector to divide</param>
		/// <param name="divisor">Divisor</param>
		/// <returns>Resulting vector when all components of <paramref name="v1" /> are divided by <paramref name="divisor" />.</returns>
		public static Vector2d operator /(Vector2d v1, double divisor) {
			return Scale(v1, 1.0 / divisor);
		}

		/// <summary>
		/// Equivalency. Returns <c>true</c> if the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns><c>true</c> if the components of two vectors are equal or approximately equal.</returns>
		public static bool operator ==(Vector2d v1, Vector2d v2) {
			return v1.Equals(v2);
		}

		/// <summary>
		/// Non-Equivalency. Returns <c>true</c> if the components of two vectors are not equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns><c>true</c> if the components of two vectors are not equal or approximately equal.</returns>
		public static bool operator !=(Vector2d v1, Vector2d v2) {
			return !v1.Equals(v2);
		}

		/// <summary>
		/// Equivalency. Returns <c>true</c> if the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns><c>true</c> if the components of two vectors are equal or approximately equal.</returns>
		public bool Equals(Vector2d other) {
			return (Math.Abs(x - other.x) < 0.001 && Math.Abs(y - other.y) < 0.001);
		}

		/// <summary>
		/// Equivalency. Returns <c>true</c> if the other object is a vector, and the components of two vectors are equal or approximately equal.
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns><c>true</c> if the other object is a vector, and the components of two vectors are equal or approximately equal.</returns>
		public override bool Equals(object obj) {
			if (object.ReferenceEquals(obj, null) || !GetType().IsAssignableFrom(obj.GetType())) { return false; }
			return Equals((Vector2d)obj);
		}

		/// <summary>
		/// Generates a hash code for this instance based on instance data.
		/// </summary>
		/// <returns>The hash code for this instance</returns>
		public override int GetHashCode() {
			return x.GetHashCode() ^ y.GetHashCode();
		}

		/// <summary>
		/// Calculates the distance from this vector to another.
		/// </summary>
		/// <param name="to">Vector to calculate distance to</param>
		/// <returns>Distance from this vector to the passed vector</returns>
		public double Distance(Vector2d to) {
			return (this - to).magnitude;
		}

		/// <summary>
		/// Gets a human-readable string representation of this vector.
		/// </summary>
		/// <returns>Human-readable string representation of this vector</returns>
		public override string ToString() {
			return string.Format("( {0} , {1} )", x.ToString(), y.ToString());
		}

		/// <summary>
		/// Changes this vector to its normalized version (it will have a magnitude of 1).
		/// </summary>
		public void Normalize() {
			if (this == Vector2d.zero) { return; }
			double magnitude = this.magnitude;
			x /= magnitude;
			y /= magnitude;
		}

		/// <summary>
		/// Gets the area of the triangle defined by three points using Heron's formula.
		/// </summary>
		/// <param name="p1">First vertex of triangle</param>
		/// <param name="p2">Second vertex of triangle</param>
		/// <param name="p3">Third vertex of triangle</param>
		/// <returns>Area of the triangle defined by these three vertices</returns>
		public static double TriangleArea(Vector3d p1, Vector3d p2, Vector3d p3) {
			return Math.Sqrt(SqrTriangleArea(p1, p2, p3)) / 4.0;
		}

		/// <summary>
		/// Gets the square of the area of the triangle defined by three points. This is useful when simply comparing two areas when you don't need to know exactly what the area is.
		/// </summary>
		/// <param name="p1">First vertex of triangle</param>
		/// <param name="p2">Second vertex of triangle</param>
		/// <param name="p3">Third vertex of triangle</param>
		/// <returns>Square of the area of the triangle defined by these three vertices</returns>
		public static double SqrTriangleArea(Vector3d p1, Vector3d p2, Vector3d p3) {
			double a = p1.Distance(p2);
			double b = p1.Distance(p3);
			double c = p2.Distance(p3);
			return 4.0 * a * a * b * b - Math.Pow((a * a) + (b * b) - (c * c), 2);
		}

		/// <summary>
		/// Allows enumeration through the components of a <c>Vector2d</c> using a foreach loop.
		/// </summary>
		public IEnumerator<double> GetEnumerator() {
			yield return x;
			yield return y;
		}

		/// <summary>
		/// Allows enumeration through the components of a <c>Vector2d</c> using a foreach loop, auto-boxed version.
		/// </summary>
		/// <remarks>
		/// This foreach loop will look like foreach(object o in Vector2d). This will auto-box the doubles in System.Double
		/// objects, allocating memory on the heap which the garbage collector will have to free later. In general, iterate
		/// through doubles rather than objects.
		/// </remarks>
		IEnumerator IEnumerable.GetEnumerator() {
			yield return x;
			yield return y;
		}

		/// <summary>
		/// Implicitly converts this <c>Vector2d</c> into a <c>Vector3d</c>. This will be called when Vector3d v3 = v2 is used.
		/// </summary>
		/// <param name="v"><c>Vector2d</c> to convert</param>
		/// <returns>The input vector as a <c>Vector3d</c>, Z component set to 0</returns>
		public static implicit operator Vector3d(Vector2d v) {
			return new Vector3d(v.x, v.y, 0);
		}

		/// <summary>
		/// Implicitly converts this <c>Vector2d</c> into a <c>Vector4d</c>. This will be called when Vector4d v4 = v2 is used.
		/// </summary>
		/// <param name="v"><c>Vector2d</c> to convert</param>
		/// <returns>The input vector as a <c>Vector4d</c>, Z and W components set to 0</returns>
		public static implicit operator Vector4d(Vector2d v) {
			return new Vector4d(v.x, v.y, 0, 0);
		}
	}
}
#endif
