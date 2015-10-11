#if !(UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
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
		/// The <c>a</c> component of this <c>Plane</c>
		/// </summary>
		public double a {
			get {
				return normal.x;
			}
		}

		/// <summary>
		/// The <c>b</c> component of this <c>Plane</c>
		/// </summary>
		public double b {
			get {
				return normal.y;
			}
		}

		/// <summary>
		/// The <c>c</c> component of this <c>Plane</c>
		/// </summary>
		public double c {
			get {
				return normal.z;
			}
		}

		/// <summary>
		/// This <c>Plane</c>, flipped over so the negative side is now the positive side, and vice versa.
		/// </summary>
		public Plane flipped {
			get {
				return new Plane(-normal, -distance);
			}
		}

		/// <summary>
		/// Creates a new <c>Plane</c> object using <c>float</c>s. The first three <c>float</c>s are the normal, and the last one is the distance.
		/// </summary>
		/// <param name="nums">Components of this <c>Plane</c></param>
		/// <exception cref="ArgumentException">4 <c>float</c>s were not passed</exception>
		/// <exception cref="ArgumentNullException">The passed array was null</exception>
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
		/// Creates a new <c>Plane</c> object using <c>double</c>s. The first three <c>double</c>s are the normal, and the last one is the distance.
		/// </summary>
		/// <param name="nums">Components of this <c>Plane</c></param>
		/// <exception cref="ArgumentException">4 <c>double</c>s were not passed</exception>
		/// <exception cref="ArgumentNullException">The passed array was null</exception>
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
		/// Creates a new <c>Plane</c> object using a normal and distance.
		/// </summary>
		/// <param name="normal">Normal of this <c>Plane</c></param>
		/// <param name="dist">Distance from the origin to this <c>Plane</c></param>
		public Plane(Vector3d normal, double dist) {
			this._normal = new Vector3d(normal);
			_normal.Normalize();
			this.distance = dist;
		}

		/// <summary>
		/// Creates a new <c>Plane</c> object using a normal and distance.
		/// </summary>
		/// <param name="normal">Normal of this <c>Plane</c></param>
		/// <param name="dist">Distance from the origin to this <c>Plane</c></param>
		public Plane(Vector3d normal, float dist) : this(normal, Convert.ToDouble(dist)) { }

		/// <summary>
		/// Creates a new <c>Plane</c> object by copying another <c>Plane</c>.
		/// </summary>
		/// <param name="copy"><c>Plane</c> to copy.</param>
		public Plane(Plane copy) {
			_normal = new Vector3d(copy.normal);
			distance = copy.distance;
		}

		/// <summary>
		/// Creates a new <c>Plane</c> object using a normal and a point on the <c>Plane</c>.
		/// </summary>
		/// <param name="normal">Normal of this <c>Plane</c></param>
		/// <param name="point">A point on this <c>Plane</c></param>
		public Plane(Vector3d normal, Vector3d point) {
			this._normal = normal;
			_normal.Normalize();
			this.distance = point * normal;
		}

		/// <summary>
		/// Creates a new <c>Plane</c> object using three points on the <c>Plane</c>.
		/// </summary>
		/// <param name="point0">A point on the <c>Plane</c></param>
		/// <param name="point1">A point on the <c>Plane</c></param>
		/// <param name="point2">A point on the <c>Plane</c></param>
		public Plane(Vector3d point0, Vector3d point1, Vector3d point2) {
			_normal = ((point0 - point2) ^ (point0 - point1));
			_normal.Normalize();
			distance = point0 * _normal;
		}

		/// <summary>
		/// Determines whether this <c>Plane</c> is equal to another <c>object</c>.
		/// </summary>
		/// <param name="obj"><c>object</c> to compare to</param>
		/// <returns>Whether <paramref name="obj" /> is a <c>Plane</c> and is equal to this <c>Plane</c></returns>
		public override bool Equals(object obj) {
			if (object.ReferenceEquals(obj, null) || !GetType().IsAssignableFrom(obj.GetType())) { return false; }
			return Equals((Plane)obj);
		}

		/// <summary>
		/// Compares whether two <c>Plane</c>s are equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <c>Plane</c> to compare to</param>
		/// <returns><c>true</c> if this <c>Plane</c> is parallel to, faces the same direction, and has the same distance as, the given <c>Plane</c>.</returns>
		public bool Equals(Plane other) {
			return (normal == other.normal && distance + 0.001 >= other.distance && distance - 0.001 <= other.distance);
		}

		/// <summary>
		/// Generates a hash code for this instance based on instance data.
		/// </summary>
		/// <returns>The hash code for this instance</returns>
		public override int GetHashCode() {
			return _normal.GetHashCode() ^ distance.GetHashCode();
		}

		/// <summary>
		/// Compares whether two <c>Plane</c>s are equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <c>Plane</c> to compare to</param>
		/// <returns><c>true</c> if this <c>Plane</c> is parallel to, faces the same direction, and has the same distance as, the given <c>Plane</c>.</returns>
		public static bool operator ==(Plane p1, Plane p2) {
			return p1.Equals(p2);
		}

		/// <summary>
		/// Compares whether two <c>Plane</c>s are not equal, or approximately equal.
		/// </summary>
		/// <param name="other">The <c>Plane</c> to compare to</param>
		/// <returns><c>false</c> if this <c>Plane</c> is parallel to, faces the same direction, and has the same distance as, the given <c>Plane</c>.</returns>
		public static bool operator !=(Plane p1, Plane p2) {
			return !p1.Equals(p2);
		}

		/// <summary>
		/// Determines whether the given <c>Vector3d</c> is contained in this <c>Plane</c>.
		/// </summary>
		/// <param name="v">Vector</param>
		/// <returns><c>true</c> if the <c>Vector3d</c> is contained in this <c>Plane</c>.</returns>
		public bool Contains(Vector3d v) {
			double distanceTo = GetDistanceToPoint(v);
			return distanceTo < 0.001 && distanceTo > -0.001;
		}
		
		/// <summary>
		/// Gets the signed distance from this <c>Plane</c> to a given point.
		/// </summary>
		/// <param name="to">Point to get the distance to</param>
		/// <returns>Signed distance from this <c>Plane</c> to the given point.</returns>
		public double GetDistanceToPoint(Vector3d to) {
			// Ax + By + Cz - d = DISTANCE = normDOTpoint - d
			double normLength = System.Math.Pow(normal.x, 2) + System.Math.Pow(normal.y, 2) + System.Math.Pow(normal.z, 2);
			if (System.Math.Abs(normLength - 1.00) > 0.01) {
				normLength = System.Math.Sqrt(normLength);
			}
			return (normal.x * to.x + normal.y * to.y + normal.z * to.z - distance) / normLength;
		}

		/// <summary>
		/// Is <paramref name="v" /> on the positive side of this <c>Plane</c>?
		/// </summary>
		/// <param name="v">Point to get the side for</param>
		/// <returns><c>true</c> if <paramref name="v" /> is on the positive side of this <c>Plane</c></returns>
		public bool GetSide(Vector3d v) {
			return GetDistanceToPoint(v) > 0;
		}

		/// <summary>
		/// Flips this <c>Plane</c> to face the opposite direction.
		/// </summary>
		public void Flip() {
			normal = -normal;
			distance = -distance;
		}

		/// <summary>
		/// Flips this <c>Plane</c> to face the opposite direction.
		/// </summary>
		public static Plane operator -(Plane flipMe) {
			return new Plane(-flipMe.normal, -flipMe.distance);
		}

		/// <summary>
		/// Gets a nicely formatted string representation of this <c>Plane</c>.
		/// </summary>
		/// <returns>A nicely formatted string representation of this <c>Plane</c></returns>
		public override string ToString() {
			return "(" + normal.ToString() + ") " + distance;
		}

		/// <summary>
		/// Raycasts a <c>Ray</c> against this <c>Plane</c>.
		/// </summary>
		/// <param name="ray"><c>Ray</c> to raycast against</param>
		/// <param name="enter">Out parameter. The distance along <paramref name="ray" /> where the collision happened</param>
		/// <returns>
		/// <c>true</c> and <paramref name="enter" /> is positive if <c>Ray</c> intersects this <c>Plane</c> in front of the way,
		/// <c>false</c> and <paramref name="enter" /> is negative if <c>Ray</c> intersects this <c>Plane</c> behind the ray,
		/// <c>false</c> and <paramref name="enter" /> is 0 if the <c>Ray</c> is parallel to this <c>Plane</c>
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

		/// <summary>
		/// Generates three points which can be used to define this <c>Plane</c>.
		/// </summary>
		/// <returns>Three points which define this <c>Plane</c></returns>
		public Vector3d[] GenerateThreePoints() {
			//DecompilerThread.OnMessage(this, "Calculating arbitrary plane points");
			double planePointCoef = 32;
			Vector3d[] plane = new Vector3d[3];
			// Figure out if the plane is parallel to two of the axes. If so it can be reproduced easily
			if (normal.y == 0 && normal.z == 0) {
				// parallel to plane YZ
				plane[0] = new Vector3d(this.distance / normal.x, -planePointCoef, planePointCoef);
				plane[1] = new Vector3d(this.distance / normal.x, 0, 0);
				plane[2] = new Vector3d(this.distance / normal.x, planePointCoef, planePointCoef);
				if (normal.x > 0) {
					Array.Reverse(plane);
				}
			} else {
				if (normal.x == 0 && normal.z == 0) {
					// parallel to plane XZ
					plane[0] = new Vector3d(planePointCoef, this.distance / normal.y, -planePointCoef);
					plane[1] = new Vector3d(0, this.distance / normal.y, 0);
					plane[2] = new Vector3d(planePointCoef, this.distance / normal.y, planePointCoef);
					if (normal.y > 0) {
						Array.Reverse(plane);
					}
				} else {
					if (normal.x == 0 && normal.y == 0) {
						// parallel to plane XY
						plane[0] = new Vector3d(-planePointCoef, planePointCoef, this.distance / normal.z);
						plane[1] = new Vector3d(0, 0, this.distance / normal.z);
						plane[2] = new Vector3d(planePointCoef, planePointCoef, this.distance / normal.z);
						if (normal.z > 0) {
							Array.Reverse(plane);
						}
					} else {
						// If you reach this point the plane is not parallel to any two-axis plane.
						if (normal.x == 0) {
							// parallel to X axis
							plane[0] = new Vector3d(-planePointCoef, planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * normal.y - this.distance)) / normal.z);
							plane[1] = new Vector3d(0, 0, this.distance / normal.z);
							plane[2] = new Vector3d(planePointCoef, planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * normal.y - this.distance)) / normal.z);
							if (normal.z > 0) {
								Array.Reverse(plane);
							}
						} else {
							if (normal.y == 0) {
								// parallel to Y axis
								plane[0] = new Vector3d((-(planePointCoef * planePointCoef * normal.z - this.distance)) / normal.x, -planePointCoef, planePointCoef * planePointCoef);
								plane[1] = new Vector3d(this.distance / normal.x, 0, 0);
								plane[2] = new Vector3d((-(planePointCoef * planePointCoef * normal.z - this.distance)) / normal.x, planePointCoef, planePointCoef * planePointCoef);
								if (normal.x > 0) {
									Array.Reverse(plane);
								}
							} else {
								if (normal.z == 0) {
									// parallel to Z axis
									plane[0] = new Vector3d(planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * normal.x - this.distance)) / normal.y, -planePointCoef);
									plane[1] = new Vector3d(0, this.distance / normal.y, 0);
									plane[2] = new Vector3d(planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * normal.x - this.distance)) / normal.y, planePointCoef);
									if (normal.y > 0) {
										Array.Reverse(plane);
									}
								} else {
									// If you reach this point the plane is not parallel to any axis. Therefore, any two coordinates will give a third.
									plane[0] = new Vector3d(-planePointCoef, planePointCoef * planePointCoef, -(-planePointCoef * normal.x + planePointCoef * planePointCoef * normal.y - this.distance) / normal.z);
									plane[1] = new Vector3d(0, 0, this.distance / normal.z);
									plane[2] = new Vector3d(planePointCoef, planePointCoef * planePointCoef, -(planePointCoef * normal.x + planePointCoef * planePointCoef * normal.y - this.distance) / normal.z);
									if (normal.z > 0) {
										Array.Reverse(plane);
									}
								}
							}
						}
					}
				}
			}
			return plane;
		}
	}
}
#endif
