#if !(UNITY || UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Holds the data for a plane in 3D space in Hessian Normal Form.
	/// </summary>
	[Serializable] public struct Plane : IEquatable<Plane> {

		private Vector3d _normal;
		public double distance;
		public Vector3d normal {
			get { return _normal; }
			set {
				_normal = value;
				_normal.Normalize();
			}
		}

		/// <summary>
		/// The <c>a</c> component of this <see cref="Plane"/>.
		/// </summary>
		public double a {
			get {
				return normal.x;
			}
		}

		/// <summary>
		/// The <c>b</c> component of this <see cref="Plane"/>.
		/// </summary>
		public double b {
			get {
				return normal.y;
			}
		}

		/// <summary>
		/// The <c>c</c> component of this <see cref="Plane"/>.
		/// </summary>
		public double c {
			get {
				return normal.z;
			}
		}

		/// <summary>
		/// This <see cref="Plane"/>, flipped over so the negative side is now the positive side, and vice versa.
		/// </summary>
		public Plane flipped {
			get {
				return new Plane(-normal, -distance);
			}
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using <c>float</c>s. The first three <c>float</c>s are the normal, and the last one is the distance.
		/// </summary>
		/// <param name="nums">Components of this <see cref="Plane"/>.</param>
		/// <exception cref="ArgumentException">4 <c>float</c>s were not passed.</exception>
		/// <exception cref="ArgumentNullException">The passed array was <c>null</c>.</exception>
		public Plane(params float[] nums) {
			if (nums == null) {
				throw new ArgumentNullException();
			}
			if (nums.Length != 4) {
				throw new ArgumentException("You must provide four numbers to generate a plane!");
			}
			_normal = new Vector3d(nums[0], nums[1], nums[2]);
			_normal.Normalize();
			distance = Convert.ToDouble(nums[3]);
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using <c>double</c>s. The first three <c>double</c>s are the normal, and the last one is the distance.
		/// </summary>
		/// <param name="nums">Components of this <see cref="Plane"/>.</param>
		/// <exception cref="ArgumentException">4 <c>double</c>s were not passed.</exception>
		/// <exception cref="ArgumentNullException">The passed array was <c>null</c>.</exception>
		public Plane(params double[] nums) {
			if (nums == null) {
				throw new ArgumentNullException();
			}
			if (nums.Length != 4) {
				throw new ArgumentException("You must provide four numbers to generate a plane!");
			}
			_normal = new Vector3d(nums[0], nums[1], nums[2]);
			_normal.Normalize();
			distance = nums[3];
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using a normal and distance.
		/// </summary>
		/// <param name="normal">Normal of this <see cref="Plane"/>.</param>
		/// <param name="dist">Distance from the origin to this <see cref="Plane"/>.</param>
		public Plane(Vector3d normal, double dist) {
			this._normal = new Vector3d(normal);
			_normal.Normalize();
			this.distance = dist;
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using a normal and distance.
		/// </summary>
		/// <param name="normal">Normal of this <see cref="Plane"/>.</param>
		/// <param name="dist">Distance from the origin to this <see cref="Plane"/>.</param>
		public Plane(Vector3d normal, float dist) : this(normal, Convert.ToDouble(dist)) { }

		/// <summary>
		/// Creates a new <see cref="Plane"/> object by copying another <see cref="Plane"/>.
		/// </summary>
		/// <param name="copy"><see cref="Plane"/> to copy.</param>
		public Plane(Plane copy) {
			_normal = new Vector3d(copy.normal);
			distance = copy.distance;
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using a normal and a point on the <see cref="Plane"/>.
		/// </summary>
		/// <param name="normal">Normal of this <see cref="Plane"/>.</param>
		/// <param name="point">A point on this <see cref="Plane"/>.</param>
		public Plane(Vector3d normal, Vector3d point) {
			this._normal = normal;
			_normal.Normalize();
			this.distance = point * normal;
		}

		/// <summary>
		/// Creates a new <see cref="Plane"/> object using three points on the <see cref="Plane"/>.
		/// </summary>
		/// <param name="point0">A point on the <see cref="Plane"/>.</param>
		/// <exception cref="ArgumentNullException"><param name="points"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">3 <see cref="Vector3d"/>s were not passed.</exception>
		public Plane(params Vector3d[] points) {
			if (points == null) {
				throw new ArgumentNullException();
			}
			if (points.Length < 3) {
				throw new ArgumentException("Plane constructor was not given enough points to define a plane.");
			}
			_normal = ((points[0] - points[2]) ^ (points[0] - points[1]));
			_normal.Normalize();
			distance = points[0] * _normal;
		}

		#region IEquatable
		/// <summary>
		/// Determines whether this <see cref="Plane"/> is equal to another <c>object</c>.
		/// </summary>
		/// <param name="obj"><c>object</c> to compare to.</param>
		/// <returns>Whether <paramref name="obj"/> is a <see cref="Plane"/> and is equal to this <see cref="Plane"/>.</returns>
		public override bool Equals(object obj) {
			if (object.ReferenceEquals(obj, null) || !GetType().IsAssignableFrom(obj.GetType())) { return false; }
			return Equals((Plane)obj);
		}

		/// <summary>
		/// Compares whether two <see cref="Plane"/>s are equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <see cref="Plane"/> to compare to.</param>
		/// <returns><c>true</c> if this <see cref="Plane"/> is parallel to, faces the same direction, and has the same distance as, the given <see cref="Plane"/>.</returns>
		public bool Equals(Plane other) {
			return (normal == other.normal && distance + 0.001 >= other.distance && distance - 0.001 <= other.distance);
		}

		/// <summary>
		/// Generates a hash code for this instance based on instance data.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode() {
			return _normal.GetHashCode() ^ distance.GetHashCode();
		}

		/// <summary>
		/// Compares whether two <see cref="Plane"/>s are equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <see cref="Plane"/> to compare to.</param>
		/// <returns><c>true</c> if this <see cref="Plane"/> is parallel to, faces the same direction, and has the same distance as, the given <see cref="Plane"/>.</returns>
		public static bool operator ==(Plane p1, Plane p2) {
			return p1.Equals(p2);
		}

		/// <summary>
		/// Compares whether two <see cref="Plane"/>s are not equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <see cref="Plane"/> to compare to.</param>
		/// <returns><c>false</c> if this <see cref="Plane"/> is parallel to, faces the same direction, and has the same distance as, the given <see cref="Plane"/>.</returns>
		public static bool operator !=(Plane p1, Plane p2) {
			return !p1.Equals(p2);
		}
		#endregion

		/// <summary>
		/// Determines whether the given <see cref="Vector3d"/> is contained in this <see cref="Plane"/>.
		/// </summary>
		/// <param name="v">Point.</param>
		/// <returns><c>true</c> if the <see cref="Vector3d"/> is contained in this <see cref="Plane"/>.</returns>
		public bool Contains(Vector3d v) {
			double distanceTo = GetDistanceToPoint(v);
			return distanceTo < 0.001 && distanceTo > -0.001;
		}
		
		/// <summary>
		/// Gets the signed distance from this <see cref="Plane"/> to a given point.
		/// </summary>
		/// <param name="to">Point to get the distance to.</param>
		/// <returns>Signed distance from this <see cref="Plane"/> to the given point.</returns>
		public double GetDistanceToPoint(Vector3d to) {
			// Ax + By + Cz - d = DISTANCE = normDOTpoint - d
			double normLength = System.Math.Pow(normal.x, 2) + System.Math.Pow(normal.y, 2) + System.Math.Pow(normal.z, 2);
			if (System.Math.Abs(normLength - 1.00) > 0.01) {
				normLength = System.Math.Sqrt(normLength);
			}
			return (normal.x * to.x + normal.y * to.y + normal.z * to.z - distance) / normLength;
		}

		/// <summary>
		/// Is <paramref name="v"/> on the positive side of this <see cref="Plane"/>?
		/// </summary>
		/// <param name="v">Point to get the side for.</param>
		/// <returns><c>true</c> if <paramref name="v"/> is on the positive side of this <see cref="Plane"/>.</returns>
		public bool GetSide(Vector3d v) {
			return GetDistanceToPoint(v) > 0;
		}

		/// <summary>
		/// Flips this <see cref="Plane"/> to face the opposite direction.
		/// </summary>
		public void Flip() {
			normal = -normal;
			distance = -distance;
		}

		/// <summary>
		/// Flips this <see cref="Plane"/> to face the opposite direction.
		/// </summary>
		public static Plane operator -(Plane flipMe) {
			return new Plane(-flipMe.normal, -flipMe.distance);
		}

		/// <summary>
		/// Gets a nicely formatted string representation of this <see cref="Plane"/>.
		/// </summary>
		/// <returns>A nicely formatted <c>string</c> representation of this <see cref="Plane"/>.</returns>
		public override string ToString() {
			return "(" + normal.ToString() + ") " + distance;
		}

		/// <summary>
		/// Raycasts a <see cref="Ray"/> against this <see cref="Plane"/>.
		/// </summary>
		/// <param name="ray"><see cref="Ray"/> to raycast against.</param>
		/// <param name="enter"><c>out</c> parameter that will contain the distance along <paramref name="ray"/> where the collision happened.</param>
		/// <returns>
		/// <c>true</c> and <paramref name="enter"/> is positive if <see cref="Ray"/> intersects this <see cref="Plane"/> in front of the ray,
		/// <c>false</c> and <paramref name="enter"/> is negative if <see cref="Ray"/> intersects this <see cref="Plane"/> behind the ray,
		/// <c>false</c> and <paramref name="enter"/> is 0 if the <see cref="Ray"/> is parallel to this <see cref="Plane"/>.
		/// </returns>
		public bool Raycast(Ray ray, out double enter) {
			double denom = (Vector3d.Dot(ray.direction, normal));
			if (denom > -0.005 && denom < 0.005) {
				enter = 0;
				return false;
			}
			enter = (-1 * (Vector3d.Dot(ray.origin, normal) + distance)) / denom;
			return enter > 0;
		}
	}
}
#endif
