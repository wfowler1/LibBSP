#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Static class containing helper methods for <see cref="Plane"/> objects.
	/// </summary>
	public static class PlaneExtensions {
		/// <summary>
		/// Intersects three <see cref="Plane"/>s at a <see cref="Vector3"/>. Returns NaN for all components if two or more <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1"><see cref="Plane"/> to intersect.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <param name="p3"><see cref="Plane"/> to intersect.</param>
		/// <returns>Point of intersection if all three <see cref="Plane"/>s meet at a point, (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3 Intersection(Plane p1, Plane p2, Plane p3) {
			Vector3 aN = p1.normal;
			Vector3 bN = p2.normal;
			Vector3 cN = p3.normal;

#if UNITY
			float partSolx1 = (bN.y * cN.z) - (bN.z * cN.y);
			float partSoly1 = (bN.z * cN.x) - (bN.x * cN.z);
			float partSolz1 = (bN.x * cN.y) - (bN.y * cN.x);
			float det = (aN.x * partSolx1) + (aN.y * partSoly1) + (aN.z * partSolz1);
#else
			double partSolx1 = (bN.y * cN.z) - (bN.z * cN.y);
			double partSoly1 = (bN.z * cN.x) - (bN.x * cN.z);
			double partSolz1 = (bN.x * cN.y) - (bN.y * cN.x);
			double det = (aN.x * partSolx1) + (aN.y * partSoly1) + (aN.z * partSolz1);
#endif
			if (det == 0) {
				return new Vector3(System.Single.NaN, System.Single.NaN, System.Single.NaN);
			}

			return new Vector3((p1.distance * partSolx1 + p2.distance * (cN.y * aN.z - cN.z * aN.y) + p3.distance * (aN.y * bN.z - aN.z * bN.y)) / det,
									 (p1.distance * partSoly1 + p2.distance * (aN.x * cN.z - aN.z * cN.x) + p3.distance * (bN.x * aN.z - bN.z * aN.x)) / det,
									 (p1.distance * partSolz1 + p2.distance * (cN.x * aN.y - cN.y * aN.x) + p3.distance * (aN.x * bN.y - aN.y * bN.x)) / det);
		}

		/// <summary>
		/// Intersects this <see cref="Plane"/> with two other <see cref="Plane"/>s at a <see cref="Vector3"/>. Returns NaN for all components if two or more <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1">This <see cref="Plane"/>.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect</param>
		/// <param name="p3"><see cref="Plane"/> to intersect</param>
		/// <returns>Point of intersection if all three <see cref="Plane"/>s meet at a point, (NaN, NaN, NaN) otherwise</returns>
		public static Vector3 Intersect(this Plane p1, Plane p2, Plane p3) {
			return Intersection(p1, p2, p3);
		}

		/// <summary>
		/// Intersects a <see cref="Plane"/> "<paramref name="p" />" with a <see cref="Ray"/> "<paramref name="r" />" at a <see cref="Vector3"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="p"><see cref="Plane"/> to intersect with.</param>
		/// <param name="r"><see cref="Ray"/> to intersect.</param>
		/// <returns>Point of intersection if "<paramref name="r" />" intersects "<paramref name="p" />", (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3 Intersection(Plane p, Ray r) {
#if UNITY
			float enter;
#else
			double enter;
#endif
			bool intersected = p.Raycast(r, out enter);
			if(enter != 0 || intersected) {
				return r.GetPoint(enter);
			} else {
				return new Vector3(System.Single.NaN, System.Single.NaN, System.Single.NaN);
			}
		}

		/// <summary>
		/// Intersects this <see cref="Plane"/> with a <see cref="Ray"/> "<paramref name="r" />" at a <see cref="Vector3"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="r"><see cref="Ray"/> to intersect.</param>
		/// <returns>Point of intersection if "<paramref name="r" />" intersects this <see cref="Plane"/>, (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3 Intersect(this Plane p, Ray r) {
			return Intersection(p, r);
		}

		/// <summary>
		/// Intersects a <see cref="Plane"/> "<paramref name="p" />" with this <see cref="Ray"/> at a <see cref="Vector3"/>. Returns NaN for all components if they do not intersect.
		/// </summary>
		/// <param name="r">This <see cref="Ray"/>.</param>
		/// <param name="p"><see cref="Plane"/> to intersect with.</param>
		/// <returns>Point of intersection if this <see cref="Ray"/> intersects "<paramref name="p" />", (NaN, NaN, NaN) otherwise.</returns>
		public static Vector3 Intersect(this Ray r, Plane p) {
			return Intersection(p, r);
		}

		/// <summary>
		/// Intersects two <see cref="Plane"/>s at a <see cref="Ray"/>. Returns NaN for all components of both <see cref="Vector3"/>s of the <see cref="Ray"/> if the <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1"><see cref="Plane"/> to intersect.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <returns>Line of intersection where "<paramref name="p1" />" intersects "<paramref name="p2" />", ((NaN, NaN, NaN) + p(NaN, NaN, NaN)) otherwise.</returns>
		public static Ray Intersection(Plane p1, Plane p2) {
			Vector3 direction = Vector3.Cross(p1.normal, p2.normal);
			if (direction == Vector3.zero) {
				return new Ray(new Vector3(System.Single.NaN, System.Single.NaN, System.Single.NaN), new Vector3(System.Single.NaN, System.Single.NaN, System.Single.NaN));
			}
			// If x == 0, solve for y in terms of z, or z in terms of y	

			Vector3 origin;

			Vector3 sqrDirection = Vector3.Scale(direction, direction);
			if (sqrDirection.x >= sqrDirection.y && sqrDirection.x >= sqrDirection.z) {
#if UNITY
				float denom = (p1.normal.y * p2.normal.z) - (p2.normal.y * p1.normal.z);
#else
				double denom = (p1.normal.y * p2.normal.z) - (p2.normal.y * p1.normal.z);
#endif
				origin = new Vector3(0,
											((p1.normal.z * p2.distance) - (p2.normal.z * p1.distance)) / denom,
											((p2.normal.y * p1.distance) - (p1.normal.y * p2.distance)) / denom);
			} else if (sqrDirection.y >= sqrDirection.x && sqrDirection.y >= sqrDirection.z) {
#if UNITY
				float denom = (p1.normal.x * p2.normal.z) - (p2.normal.x * p1.normal.z);
#else
				double denom = (p1.normal.x * p2.normal.z) - (p2.normal.x * p1.normal.z);
#endif
				origin = new Vector3(((p1.normal.z * p2.distance) - (p2.normal.z * p1.distance)) / denom,
											0,
											((p2.normal.x * p1.distance) - (p1.normal.x * p2.distance)) / denom);
			} else {
#if UNITY
				float denom = (p1.normal.x * p2.normal.y) - (p2.normal.x * p1.normal.y);
#else
				double denom = (p1.normal.x * p2.normal.y) - (p2.normal.x * p1.normal.y);
#endif
				origin = new Vector3(((p1.normal.y * p2.distance) - (p2.normal.y * p1.distance)) / denom,
											((p2.normal.x * p1.distance) - (p1.normal.x * p2.distance)) / denom,
											0);
			}

			return new Ray(origin, direction);
		}

		/// <summary>
		/// Intersects this <see cref="Plane"/> with another <see cref="Plane"/> at a <see cref="Ray"/>. Returns NaN for all components of both <see cref="Vector3"/>s of the <see cref="Ray"/> if the <see cref="Plane"/>s are parallel.
		/// </summary>
		/// <param name="p1">This <see cref="Plane"/>.</param>
		/// <param name="p2"><see cref="Plane"/> to intersect.</param>
		/// <returns>Line of intersection where this <see cref="Plane"/> intersects "<paramref name="p2" />", ((NaN, NaN, NaN) + p(NaN, NaN, NaN)) otherwise.</returns>
		public static Ray Intersect(this Plane p1, Plane p2) {
			return Intersection(p1, p2);
		}

		/// <summary>
		/// Generates three points which can be used to define this <see cref="Plane"/>.
		/// </summary>
		/// <param name="p">This <see cref="Plane"/>.</param>
		/// <param name="planePointCoef">Scale of distance between the generated points. The points will define the same <see cref="Plane"/> but will be farther apart the larger this value is. May not be zero.</param>
		/// <returns>Three points which define this <see cref="Plane"/>.</returns>
		public static Vector3[] GenerateThreePoints(this Plane p, float planePointCoef = 16) {
			Vector3[] points = new Vector3[3];
			// Figure out if the plane is parallel to two of the axes. If so it can be reproduced easily
			if (p.normal.y == 0 && p.normal.z == 0) {
				// parallel to plane YZ
				points[0] = new Vector3(p.distance / p.normal.x, -planePointCoef, planePointCoef);
				points[1] = new Vector3(p.distance / p.normal.x, 0, 0);
				points[2] = new Vector3(p.distance / p.normal.x, planePointCoef, planePointCoef);
				if (p.normal.x > 0) {
					Array.Reverse(points);
				}
			} else if (p.normal.x == 0 && p.normal.z == 0) {
				// parallel to plane XZ
				points[0] = new Vector3(planePointCoef, p.distance / p.normal.y, -planePointCoef);
				points[1] = new Vector3(0, p.distance / p.normal.y, 0);
				points[2] = new Vector3(planePointCoef, p.distance / p.normal.y, planePointCoef);
				if (p.normal.y > 0) {
					Array.Reverse(points);
				}
			} else if (p.normal.x == 0 && p.normal.y == 0) {
				// parallel to plane XY
				points[0] = new Vector3(-planePointCoef, planePointCoef, p.distance / p.normal.z);
				points[1] = new Vector3(0, 0, p.distance / p.normal.z);
				points[2] = new Vector3(planePointCoef, planePointCoef, p.distance / p.normal.z);
				if (p.normal.z > 0) {
					Array.Reverse(points);
				}
			} else if (p.normal.x == 0) {
				// If you reach this point the plane is not parallel to any two-axis plane.
				// parallel to X axis
				points[0] = new Vector3(-planePointCoef, planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * p.normal.y - p.distance)) / p.normal.z);
				points[1] = new Vector3(0, 0, p.distance / p.normal.z);
				points[2] = new Vector3(planePointCoef, planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * p.normal.y - p.distance)) / p.normal.z);
				if (p.normal.z > 0) {
					Array.Reverse(points);
				}
			} else if (p.normal.y == 0) {
				// parallel to Y axis
				points[0] = new Vector3((-(planePointCoef * planePointCoef * p.normal.z - p.distance)) / p.normal.x, -planePointCoef, planePointCoef * planePointCoef);
				points[1] = new Vector3(p.distance / p.normal.x, 0, 0);
				points[2] = new Vector3((-(planePointCoef * planePointCoef * p.normal.z - p.distance)) / p.normal.x, planePointCoef, planePointCoef * planePointCoef);
				if (p.normal.x > 0) {
					Array.Reverse(points);
				}
			} else if (p.normal.z == 0) {
				// parallel to Z axis
				points[0] = new Vector3(planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * p.normal.x - p.distance)) / p.normal.y, -planePointCoef);
				points[1] = new Vector3(0, p.distance / p.normal.y, 0);
				points[2] = new Vector3(planePointCoef * planePointCoef, (-(planePointCoef * planePointCoef * p.normal.x - p.distance)) / p.normal.y, planePointCoef);
				if (p.normal.y > 0) {
					Array.Reverse(points);
				}
			} else {
				// If you reach this point the plane is not parallel to any axis. Therefore, any two coordinates will give a third.
				points[0] = new Vector3(-planePointCoef, planePointCoef * planePointCoef, -(-planePointCoef * p.normal.x + planePointCoef * planePointCoef * p.normal.y - p.distance) / p.normal.z);
				points[1] = new Vector3(0, 0, p.distance / p.normal.z);
				points[2] = new Vector3(planePointCoef, planePointCoef * planePointCoef, -(planePointCoef * p.normal.x + planePointCoef * planePointCoef * p.normal.y - p.distance) / p.normal.z);
				if (p.normal.z > 0) {
					Array.Reverse(points);
				}
			}
			return points;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <see cref="List"/> of <see cref="Plane"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <returns>A <see cref="List"/> of <see cref="Plane"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		/// <remarks>This function goes here since it can't go into Unity's Plane class, and so can't depend
		/// on having a constructor taking a byte array.</remarks>
		public static List<Plane> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
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
				case MapType.CoD4: {
					structLength = 16;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Plane class.");
				}
			}
			List<Plane> lump = new List<Plane>(data.Length / structLength);
			for (int i = 0; i < data.Length / structLength; ++i) {
				Vector3 normal = new Vector3(BitConverter.ToSingle(data, structLength * i), BitConverter.ToSingle(data, (structLength * i) + 4), BitConverter.ToSingle(data, (structLength * i) + 8));
				float distance = BitConverter.ToSingle(data, (structLength * i) + 12);
				lump.Add(new Plane(normal, distance));
			}
			return lump;
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
				case MapType.DMoMaM: {
					return 1;
				}
				case MapType.CoD:
				case MapType.Raven:
				case MapType.Quake3: {
					return 2;
				}
				case MapType.CoD4:
				case MapType.CoD2: {
					return 4;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
