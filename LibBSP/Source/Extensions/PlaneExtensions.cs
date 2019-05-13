#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace LibBSP {
#if UNITY
	using Plane = UnityEngine.Plane;
	using Vector3d = UnityEngine.Vector3;
	using Ray = UnityEngine.Ray;
#elif GODOT
	using Plane = Godot.Plane;
	using Vector3d = Godot.Vector3;
#endif

	/// <summary>
	/// Static class containing helper methods for <see cref="Plane"/> objects.
	/// </summary>
	public static class PlaneExtensions {

		/// <summary>
		/// Array of base texture axes. When referenced properly, provides a good default texture axis for any given plane.
		/// </summary>
		public static readonly Vector3d[] baseAxes = new Vector3d[] {
			new Vector3d(0, 0, 1), new Vector3d(1, 0, 0), new Vector3d(0, -1, 0),
			new Vector3d(0, 0, -1), new Vector3d(1, 0, 0), new Vector3d(0, -1, 0),
			new Vector3d(1, 0, 0), new Vector3d(0, 1, 0), new Vector3d(0, 0, -1),
			new Vector3d(-1, 0, 0), new Vector3d(0, 1, 0), new Vector3d(0, 0, -1),
			new Vector3d(0, 1, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, -1),
			new Vector3d(0, -1, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, -1)
		};

		/// <summary>
		/// Gets the normal of this <see cref="Plane"/>.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <returns>The normal of this <see cref="Plane"/>.</returns>
		public static Vector3d GetNormal(this Plane p) {
#if GODOT
			return p.Normal;
#else
			return p.normal;
#endif
		}

		/// <summary>
		/// Gets the distance of this <see cref="Plane"/> from the origin.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <returns>The distance of this <see cref="Plane"/> from the origin.</returns>
		public static double GetDistance(this Plane p) {
#if GODOT
			return p.D;
#else
			return p.distance;
#endif
		}

		/// <summary>
		/// Intersects three <see cref="Plane"/>s at a <see cref="Vector3d"/>. Returns NaN for all components if two or more <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1"><see cref="Plane"/> to intersect.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <param name="p3"><see cref="Plane"/> to intersect.</param>
		/// <returns>Point of intersection if all three <see cref="Plane"/>s meet at a point, (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3d Intersection(Plane p1, Plane p2, Plane p3) {
#if GODOT
			return p1.Intersect3(p2, p3);
#else
			Vector3d aN = p1.GetNormal();
			Vector3d bN = p2.GetNormal();
			Vector3d cN = p3.GetNormal();
			var p1d = p1.distance;
			var p2d = p2.distance;
			var p3d = p3.distance;

			var partSolx1 = (bN.y * cN.z) - (bN.z * cN.y);
			var partSoly1 = (bN.z * cN.x) - (bN.x * cN.z);
			var partSolz1 = (bN.x * cN.y) - (bN.y * cN.x);
			var det = (aN.x * partSolx1) + (aN.y * partSoly1) + (aN.z * partSolz1);
			if (det == 0) {
				return new Vector3d(float.NaN, float.NaN, float.NaN);
			}

			return new Vector3d((p1d * partSolx1 + p2d * (cN.y * aN.z - cN.z * aN.y) + p3d * (aN.y * bN.z - aN.z * bN.y)) / det,
			                    (p1d * partSoly1 + p2d * (aN.x * cN.z - aN.z * cN.x) + p3d * (bN.x * aN.z - bN.z * aN.x)) / det,
			                    (p1d * partSolz1 + p2d * (cN.x * aN.y - cN.y * aN.x) + p3d * (aN.x * bN.y - aN.y * bN.x)) / det);
#endif
		}

		/// <summary>
		/// Intersects this <see cref="Plane"/> with two other <see cref="Plane"/>s at a <see cref="Vector3d"/>. Returns NaN for all components if two or more <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1">This <see cref="Plane"/>.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <param name="p3"><see cref="Plane"/> to intersect.</param>
		/// <returns>Point of intersection if all three <see cref="Plane"/>s meet at a point, (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3d Intersect(this Plane p1, Plane p2, Plane p3) {
#if GODOT
			return p1.Intersect3(p2, p3);
#else
			return Intersection(p1, p2, p3);
#endif
		}

		/// <summary>
		/// Intersects a <see cref="Plane"/> "<paramref name="p"/>" with a <see cref="Ray"/> "<paramref name="r"/>" at a <see cref="Vector3d"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="p"><see cref="Plane"/> to intersect with.</param>
		/// <param name="r"><see cref="Ray"/> to intersect.</param>
		/// <returns>Point of intersection if "<paramref name="r"/>" intersects "<paramref name="p"/>", (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3d Intersection(Plane p, Ray r) {
#if UNITY
			float enter;
#else
			double enter;
#endif
			bool intersected = p.Raycast(r, out enter);
			if (intersected || enter != 0) {
				return r.GetPoint(enter);
			} else {
				return new Vector3d(float.NaN, float.NaN, float.NaN);
			}
		}

#if GODOT
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
		public static bool Raycast(this Plane p, Ray ray, out double enter) {
			double denom = ray.direction.Dot(p.Normal);
			if (denom > -0.005 && denom < 0.005) {
				enter = 0;
				return false;
			}
			enter = (-1 * ray.origin.Dot(p.Normal) + p.D) / denom;
			return enter > 0;
		}
#endif

		/// <summary>
		/// Intersects this <see cref="Plane"/> with a <see cref="Ray"/> "<paramref name="r"/>" at a <see cref="Vector3d"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="r"><see cref="Ray"/> to intersect.</param>
		/// <returns>Point of intersection if "<paramref name="r"/>" intersects this <see cref="Plane"/>, (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3d Intersect(this Plane p, Ray r) {
			return Intersection(p, r);
		}

		/// <summary>
		/// Intersects a <see cref="Plane"/> "<paramref name="p"/>" with this <see cref="Ray"/> at a <see cref="Vector3d"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="r">This <see cref="Ray"/>.</param>
		/// <param name="p"><see cref="Plane"/> to intersect with.</param>
		/// <returns>Point of intersection if this <see cref="Ray"/> intersects "<paramref name="p"/>", (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3d Intersect(this Ray r, Plane p) {
			return Intersection(p, r);
		}

		/// <summary>
		/// Intersects two <see cref="Plane"/>s at a <see cref="Ray"/>. Returns NaN for all components of both <see cref="Vector3d"/>s of the <see cref="Ray"/> if the <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1"><see cref="Plane"/> to intersect.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <returns>Line of intersection where "<paramref name="p1"/>" intersects "<paramref name="p2"/>", ((NaN, NaN, NaN) + p(NaN, NaN, NaN)) otherwise.</returns>
		public static Ray Intersection(Plane p1, Plane p2) {
			Vector3d direction = p1.GetNormal().Cross(p2.GetNormal());
			if (direction == new Vector3d(0, 0, 0)) {
				return new Ray(new Vector3d(float.NaN, float.NaN, float.NaN), new Vector3d(float.NaN, float.NaN, float.NaN));
			}
			// If x == 0, solve for y in terms of z, or z in terms of y	

			Vector3d origin;

			Vector3d sqrDirection = new Vector3d(direction.x * direction.x, direction.y * direction.y, direction.z * direction.z);
			if (sqrDirection.x >= sqrDirection.y && sqrDirection.x >= sqrDirection.z) {
				var denom = (p1.GetNormal().y * p2.GetNormal().z) - (p2.GetNormal().y * p1.GetNormal().z);
				origin = new Vector3d(0,
				                      ((p1.GetNormal().z * (float)p2.GetDistance()) - (p2.GetNormal().z * (float)p1.GetDistance())) / denom,
				                      ((p2.GetNormal().y * (float)p1.GetDistance()) - (p1.GetNormal().y * (float)p2.GetDistance())) / denom);
			} else if (sqrDirection.y >= sqrDirection.x && sqrDirection.y >= sqrDirection.z) {
				var denom = (p1.GetNormal().x * p2.GetNormal().z) - (p2.GetNormal().x * p1.GetNormal().z);
				origin = new Vector3d(((p1.GetNormal().z * (float)p2.GetDistance()) - (p2.GetNormal().z * (float)p1.GetDistance())) / denom,
				                      0,
				                      ((p2.GetNormal().x * (float)p1.GetDistance()) - (p1.GetNormal().x * (float)p2.GetDistance())) / denom);
			} else {
				var denom = (p1.GetNormal().x * p2.GetNormal().y) - (p2.GetNormal().x * p1.GetNormal().y);
				origin = new Vector3d(((p1.GetNormal().y * (float)p2.GetDistance()) - (p2.GetNormal().y * (float)p1.GetDistance())) / denom,
				                      ((p2.GetNormal().x * (float)p1.GetDistance()) - (p1.GetNormal().x * (float)p2.GetDistance())) / denom,
				                      0);
			}

			return new Ray(origin, direction);
		}

		/// <summary>
		/// Intersects this <see cref="Plane"/> with another <see cref="Plane"/> at a <see cref="Ray"/>. Returns NaN for all components of both <see cref="Vector3d"/>s of the <see cref="Ray"/> if the <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1">This <see cref="Plane"/>.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <returns>Line of intersection where this <see cref="Plane"/> intersects "<paramref name="p2"/>", ((NaN, NaN, NaN) + p(NaN, NaN, NaN)) otherwise.</returns>
		public static Ray Intersect(this Plane p1, Plane p2) {
			return Intersection(p1, p2);
		}

		/// <summary>
		/// Generates three points which can be used to define this <see cref="Plane"/>.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="scalar">Scale of distance between the generated points. The points will define the same <see cref="Plane"/> but will be farther apart the larger this value is. Must not be zero.</param>
		/// <returns>Three points which define this <see cref="Plane"/>.</returns>
		public static Vector3d[] GenerateThreePoints(this Plane p, float scalar = 16) {
			Vector3d[] points = new Vector3d[3];
			// Figure out if the plane is parallel to two of the axes.
			if (p.GetNormal().y == 0 && p.GetNormal().z == 0) {
				// parallel to plane YZ
				points[0] = new Vector3d((float)p.GetDistance() / p.GetNormal().x, -scalar, scalar);
				points[1] = new Vector3d((float)p.GetDistance() / p.GetNormal().x, 0, 0);
				points[2] = new Vector3d((float)p.GetDistance() / p.GetNormal().x, scalar, scalar);
				if (p.GetNormal().x > 0) {
					Array.Reverse(points);
				}
			} else if (p.GetNormal().x == 0 && p.GetNormal().z == 0) {
				// parallel to plane XZ
				points[0] = new Vector3d(scalar, (float)p.GetDistance() / p.GetNormal().y, -scalar);
				points[1] = new Vector3d(0, (float)p.GetDistance() / p.GetNormal().y, 0);
				points[2] = new Vector3d(scalar, (float)p.GetDistance() / p.GetNormal().y, scalar);
				if (p.GetNormal().y > 0) {
					Array.Reverse(points);
				}
			} else if (p.GetNormal().x == 0 && p.GetNormal().y == 0) {
				// parallel to plane XY
				points[0] = new Vector3d(-scalar, scalar, (float)p.GetDistance() / p.GetNormal().z);
				points[1] = new Vector3d(0, 0, (float)p.GetDistance() / p.GetNormal().z);
				points[2] = new Vector3d(scalar, scalar, (float)p.GetDistance() / p.GetNormal().z);
				if (p.GetNormal().z > 0) {
					Array.Reverse(points);
				}
			} else if (p.GetNormal().x == 0) {
				// If you reach this point the plane is not parallel to any two-axis plane.
				// parallel to X axis
				points[0] = new Vector3d(-scalar, scalar * scalar, (-(scalar * scalar * p.GetNormal().y - (float)p.GetDistance())) / p.GetNormal().z);
				points[1] = new Vector3d(0, 0, (float)p.GetDistance() / p.GetNormal().z);
				points[2] = new Vector3d(scalar, scalar * scalar, (-(scalar * scalar * p.GetNormal().y - (float)p.GetDistance())) / p.GetNormal().z);
				if (p.GetNormal().z > 0) {
					Array.Reverse(points);
				}
			} else if (p.GetNormal().y == 0) {
				// parallel to Y axis
				points[0] = new Vector3d((-(scalar * scalar * p.GetNormal().z - (float)p.GetDistance())) / p.GetNormal().x, -scalar, scalar * scalar);
				points[1] = new Vector3d((float)p.GetDistance() / p.GetNormal().x, 0, 0);
				points[2] = new Vector3d((-(scalar * scalar * p.GetNormal().z - (float)p.GetDistance())) / p.GetNormal().x, scalar, scalar * scalar);
				if (p.GetNormal().x > 0) {
					Array.Reverse(points);
				}
			} else if (p.GetNormal().z == 0) {
				// parallel to Z axis
				points[0] = new Vector3d(scalar * scalar, (-(scalar * scalar * p.GetNormal().x - (float)p.GetDistance())) / p.GetNormal().y, -scalar);
				points[1] = new Vector3d(0, (float)p.GetDistance() / p.GetNormal().y, 0);
				points[2] = new Vector3d(scalar * scalar, (-(scalar * scalar * p.GetNormal().x - (float)p.GetDistance())) / p.GetNormal().y, scalar);
				if (p.GetNormal().y > 0) {
					Array.Reverse(points);
				}
			} else {
				// If you reach this point the plane is not parallel to any axis. Therefore, any two coordinates will give a third.
				points[0] = new Vector3d(-scalar, scalar * scalar, -(-scalar * p.GetNormal().x + scalar * scalar * p.GetNormal().y - (float)p.GetDistance()) / p.GetNormal().z);
				points[1] = new Vector3d(0, 0, (float)p.GetDistance() / p.GetNormal().z);
				points[2] = new Vector3d(scalar, scalar * scalar, -(scalar * p.GetNormal().x + scalar * scalar * p.GetNormal().y - (float)p.GetDistance()) / p.GetNormal().z);
				if (p.GetNormal().z > 0) {
					Array.Reverse(points);
				}
			}
			return points;
		}

		/// <summary>
		/// Gets the signed distance from this <see cref="Plane"/> to a given point.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="to">Point to get the distance to.</param>
		/// <returns>Signed distance from this <see cref="Plane"/> to the given point.</returns>
		/// <remarks>Unity uses the plane equation "Ax + By + Cz + D = 0" while Quake-based engines
		/// use "Ax + By + Cz = D". The distance equation needs to be evaluated differently from
		/// Unity's default implementation to properly apply to planes read from BSPs.</remarks>
#if UNITY || GODOT
		public static float GetBSPDistanceToPoint(this Plane p, Vector3d to) {
			return (p.GetNormal().x * to.x + p.GetNormal().y * to.y + p.GetNormal().z * to.z - (float)p.GetDistance()) / (float)p.GetNormal().GetMagnitude();
		}
#else
		public static double GetBSPDistanceToPoint(this Plane p, Vector3d to) {
			return p.GetDistanceToPoint(to);
		}
#endif

		/// <summary>
		/// Is <paramref name="v"/> on the positive side of this <see cref="Plane"/>?
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="v">Point to get the side for.</param>
		/// <returns><c>true</c> if <paramref name="v"/> is on the positive side of this <see cref="Plane"/>.</returns>
		/// <remarks>Unity uses the plane equation "Ax + By + Cz + D = 0" while Quake-based engines
		/// use "Ax + By + Cz = D". The distance equation needs to be evaluated differently from
		/// Unity's default implementation to properly apply to planes read from BSPs.</remarks>
		public static bool GetBSPSide(this Plane p, Vector3d v) {
			return p.GetBSPDistanceToPoint(v) > 0;
		}

		/// <summary>
		/// Determines whether the given <see cref="Vector3d"/> is contained in this <see cref="Plane"/>.
		/// </summary>
		/// <param name="v">Point.</param>
		/// <returns><c>true</c> if the <see cref="Vector3d"/> is contained in this <see cref="Plane"/>.</returns>
		/// <remarks>Unity uses the plane equation "Ax + By + Cz + D = 0" while Quake-based engines
		/// use "Ax + By + Cz = D". The distance equation needs to be evaluated differently from
		/// Unity's default implementation to properly apply to planes read from BSPs.</remarks>
		public static bool BSPContains(this Plane p, Vector3d v) {
			var distanceTo = p.GetBSPDistanceToPoint(v);
			return distanceTo < 0.001 && distanceTo > -0.001;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Plane"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Plane"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null.</exception>
		/// <remarks>This function goes here since it can't go into Unity's Plane class, and so can't depend
		/// on having a constructor taking a byte array.</remarks>
		public static List<Plane> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = GetStructLength(type, version);
			int numObjects = data.Length / structLength;
			List<Plane> lump = new List<Plane>(numObjects);
			for (int i = 0; i < numObjects; ++i) {
				Vector3d normal = new Vector3d(BitConverter.ToSingle(data, structLength * i), BitConverter.ToSingle(data, (structLength * i) + 4), BitConverter.ToSingle(data, (structLength * i) + 8));
				float distance = BitConverter.ToSingle(data, (structLength * i) + 12);
				lump.Add(new Plane(normal, distance));
			}
			return lump;
		}

		/// <summary>
		/// Gets this <see cref="Plane"/> as a <c>byte</c> array to be used in a BSP of type <see cref="type"/>.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="type">The <see cref="MapType"/> of BSP this <see cref="Plane"/> is from.</param>
		/// <param name="version">The version of the planes lump in the BSP.</param>
		/// <returns><c>byte</c> array representing this <see cref="Plane"/>'s components.</returns>
		public static byte[] GetBytes(this Plane p, MapType type, int version = 0) {
			byte[] bytes = new byte[GetStructLength(type, version)];
			switch (type) {
				case MapType.Quake:
				case MapType.Nightfire:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.DMoMaM:
				case MapType.Vindictus:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.TacticalInterventionEncrypted: {
					BitConverter.GetBytes(p.Type()).CopyTo(bytes, 16);
					break;
				}
			}
			p.GetNormal().GetBytes().CopyTo(bytes, 0);
			BitConverter.GetBytes((float)p.GetDistance()).CopyTo(bytes, 12);
			return bytes;
		}

		/// <summary>
		/// Gets the axis this <see cref="Plane"/>'s normal is closest to (the <see cref="Plane"/>'s normal
		/// and the axis have the largest dot product).
		/// 0 = Positive Z
		/// 1 = Negative Z
		/// 2 = Positive X
		/// 3 = Negative X
		/// 4 = Positive Y
		/// 5 = Negative Y
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <returns>The best-match axis for this <see cref="Plane"/>.</returns>
		public static int BestAxis(this Plane p) {
			int bestaxis = 0;
			double best = 0; // "Best" dot product so far
			for (int i = 0; i < 6; ++i) {
				// For all possible axes, positive and negative
#if GODOT
				double dot = p.Normal.Dot(baseAxes[i * 3]);
#else
				double dot = p.normal.Dot(baseAxes[i * 3]);
#endif
				if (dot > best) {
					best = dot;
					bestaxis = i;
				}
			}
			return bestaxis;
		}

		/// <summary>
		/// Gets the axial type of this plane.
		/// 0 = X
		/// 1 = Y
		/// 2 = Z
		/// 3 = Closest to X
		/// 4 = Closest to Y
		/// 5 = Closest to Z
		/// </summary>
		/// <remarks>
		/// This more closely resembles the type calculation of zhlt, q2tools seems to use a greater-than-or-equal
		/// comparison for the "closest to" types whereas we use a greater-than here. The difference should be minimal.
		/// </remarks>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <returns>The axial type of this plane.</returns>
		public static int Type(this Plane p) {
			double ax = Math.Abs(p.GetNormal().x);
			if (ax >= 1.0) {
				return 0;
			}

			double ay = Math.Abs(p.GetNormal().y);
			if (ay >= 1.0) {
				return 1;
			}

			double az = Math.Abs(p.GetNormal().z);
			if (az >= 1.0) {
				return 2;
			}
			
			if (ax > ay && ax > az) {
				return 3;
			}
			if (ay > ax && ay > az) {
				return 4;
			}
			return 5;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.FAKK:
				case MapType.MOHAA:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF:
				case MapType.Nightfire:
				case MapType.Vindictus:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.DMoMaM:
				case MapType.Titanfall: {
					return 1;
				}
				case MapType.CoD:
				case MapType.Raven:
				case MapType.Quake3: {
					return 2;
				}
				case MapType.CoD2:
				case MapType.CoD4: {
					return 4;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="Plane"/> structure length for the specified <see cref="MapType"/>.
		/// </summary>
		/// <param name="type">The version of BSP this plane came from.</param>
		/// <param name="version">The version of the planes lump this plane came from.</param>
		/// <returns>The length of this structure, in bytes.</returns>
		public static int GetStructLength(MapType type, int version) {
			int structLength = 0;
			switch (type) {
				case MapType.Quake:
				case MapType.Nightfire:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.L4D2:
				case MapType.DMoMaM:
				case MapType.Vindictus:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.TacticalInterventionEncrypted: {
					structLength = 20;
					break;
				}
				case MapType.STEF2:
				case MapType.MOHAA:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.Titanfall: {
					structLength = 16;
					break;
				}
			}
			return structLength;
		}
	}
}
