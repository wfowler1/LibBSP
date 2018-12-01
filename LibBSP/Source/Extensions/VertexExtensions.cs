#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#if !UNITY_5_6_OR_NEWER
#define OLDUNITY
#endif
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector2d = Vector2;
	using Vector3d = Vector3;
	using Vector4d = Vector4;
#if !OLDUNITY
	using Vertex = UIVertex;
#endif
#endif

	/// <summary>
	/// Static class containing helper methods for <see cref="Vertex"/> objects.
	/// </summary>
	public static class VertexExtensions {

#if !UNITY
		/// <summary>
		/// Scales the position of this <see cref="Vertex"/> by a scalar.
		/// </summary>
		/// <param name="v1">This <see cref="Vertex"/>.</param>
		/// <param name="scalar">Scalar value.</param>
		public static void Scale(this ref Vertex v1, float scalar) {
			v1.position *= scalar;
		}

		/// <summary>
		/// Adds the position of this <see cref="Vertex"/> to another <see cref="Vertex"/>.
		/// </summary>
		/// <param name="v1">This <see cref="Vertex"/>.</param>
		/// <param name="v2">The other <see cref="Vertex"/>.</param>
		public static void Add(this ref Vertex v1, Vertex v2) {
			v1.position += v2.position;
		}

		/// <summary>
		/// Adds the position of a <c>Vector3d</c> to this <see cref="Vertex"/>.
		/// </summary>
		/// <param name="v1">This <see cref="Vertex"/>.</param>
		/// <param name="v2">The <see cref="Vector3d"/>.</param>
		public static void Translate(this ref Vertex v1, Vector3d v2) {
			v1.position += v2;
		}
#endif

		// Extension methods using "ref" are features of C# 7.2. They will not work in Unity, no matter what.
		// Instead use these vanilla static methods in any code intended to be used with Unity.

		/// <summary>
		/// Scales the position of a <see cref="Vertex"/> by a scalar and returns the result.
		/// </summary>
		/// <param name="v1">The <see cref="Vertex"/> to scale.</param>
		/// <param name="scalar">Scalar value.</param>
		/// <returns>The resulting <see cref="Vertex"/> of this scaling operation.</returns>
		public static Vertex Scale(Vertex v1, float scalar) {
			v1.position *= scalar;
			return v1;
		}

		/// <summary>
		/// Adds the position of a <see cref="Vertex"/> to another <see cref="Vertex"/> and returns the result.
		/// </summary>
		/// <param name="v1">A <see cref="Vertex"/> to be added to another.</param>
		/// <param name="v2">A <see cref="Vertex"/> to be added to another.</param>
		/// <returns>The resulting <see cref="Vertex"/> of this addition.</returns>
		public static Vertex Add(Vertex v1, Vertex v2) {
			v1.position += v2.position;
			return v1;
		}

		/// <summary>
		/// Adds the position of a <c>Vector3d</c> to a <see cref="Vertex"/> and returns the result.
		/// </summary>
		/// <param name="v1">The <see cref="Vertex"/> to translate.</param>
		/// <param name="v2">The <see cref="Vector3d"/> to translate by.</param>
		/// <returns>The resulting <see cref="Vertex"/> translated by <paramref name="v2"/>.</returns>
		public static Vertex Translate(Vertex v1, Vector3d v2) {
			v1.position += v2;
			return v1;
		}

		/// <summary>
		/// Creates a new <see cref="Vertex"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>The resulting <see cref="Vertex"/> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was null.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		/// <remarks><see cref="Vertex"/> has no constructor, so the object must be initialized field-by-field. Even if it
		/// did have a constructor, the way data needs to be read wouldn't allow use of it.</remarks>
		public static Vertex CreateVertex(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			Vertex result = new Vertex();
			switch (type) {
				case MapType.CoD: {
					if (version == 0) {
						goto case MapType.Quake3;
					} else if (version == 1) {
						// Patch vertex. Set color to white (makes things easier in CoDRadiant) and simply read position.
						result.color = ColorExtensions.FromArgb(255, 255, 255, 255);
						goto case MapType.Quake;
					}
					break;
				}
				case MapType.CoD2: {
					result.normal = new Vector3d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20));
					result.color = ColorExtensions.FromArgb(data[27], data[24], data[25], data[26]);
					result.uv0 = new Vector2d(BitConverter.ToSingle(data, 28), BitConverter.ToSingle(data, 32));
					result.uv1 = new Vector2d(BitConverter.ToSingle(data, 36), BitConverter.ToSingle(data, 40));
					// Use these fields to store additional unknown information
					result.tangent = new Vector4d(BitConverter.ToSingle(data, 44), BitConverter.ToSingle(data, 48), BitConverter.ToSingle(data, 52), BitConverter.ToSingle(data, 56));
					result.uv3 = new Vector2d(BitConverter.ToSingle(data, 60), BitConverter.ToSingle(data, 64));
					goto case MapType.Quake;
				}
				case MapType.MOHAA:
				case MapType.Quake3:
				case MapType.CoD4:
				case MapType.FAKK: {
					result.uv0 = new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.uv1 = new Vector2d(BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
					result.normal = new Vector3d(BitConverter.ToSingle(data, 28), BitConverter.ToSingle(data, 32), BitConverter.ToSingle(data, 36));
					result.color = ColorExtensions.FromArgb(data[43], data[40], data[41], data[42]);
					goto case MapType.Quake;
				}
				case MapType.Raven: {
					result.uv0 = new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.uv1 = new Vector2d(BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
					result.uv2 = new Vector2d(BitConverter.ToSingle(data, 28), BitConverter.ToSingle(data, 32));
					result.uv3 = new Vector2d(BitConverter.ToSingle(data, 36), BitConverter.ToSingle(data, 40));
					result.normal = new Vector3d(BitConverter.ToSingle(data, 52), BitConverter.ToSingle(data, 56), BitConverter.ToSingle(data, 60));
					result.color = ColorExtensions.FromArgb(data[67], data[64], data[65], data[66]);
					// Use for two more float fields and two more colors.
					// There's actually another field that seems to be color but I've only ever seen it be FFFFFFFF.
					result.tangent = new Vector4d(BitConverter.ToSingle(data, 44), BitConverter.ToSingle(data, 48), BitConverter.ToSingle(data, 68), BitConverter.ToSingle(data, 72));
					goto case MapType.Quake;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					result.uv0 = new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.uv1 = new Vector2d(BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
					result.uv2 = new Vector2d(BitConverter.ToSingle(data, 28), 0);
					result.color = ColorExtensions.FromArgb(data[35], data[32], data[33], data[34]);
					result.normal = new Vector3d(BitConverter.ToSingle(data, 36), BitConverter.ToSingle(data, 40), BitConverter.ToSingle(data, 44));
					goto case MapType.Quake;
				}
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Titanfall:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					result.position = new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " doesn't use Vertex objects, or reading them isn't implemented.");
				}
			}
			return result;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="Vertex"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="Vertex"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		/// <remarks>This function goes here since it can't be in Unity's <c>UIVertex</c> class, and so I can't
		/// depend on having a constructor taking a byte array.</remarks>
		public static List<Vertex> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = GetStructLength(type, version);
			int numObjects = data.Length / structLength;
			List<Vertex> lump = new List<Vertex>(numObjects);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < numObjects; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(CreateVertex(bytes, type, version));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for the vertices lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF: {
					return 2;
				}
				case MapType.Quake:
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
					return 3;
				}
				case MapType.MOHAA:
				case MapType.FAKK:
				case MapType.Nightfire: {
					return 4;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 6;
				}
				case MapType.CoD: {
					return 7;
				}
				case MapType.CoD2: {
					return 8;
				}
				case MapType.Raven:
				case MapType.Quake3: {
					return 10;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the index for the patch vertices lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump.</returns>
		public static int GetIndexForPatchVertsLump(MapType type) {
			switch (type) {
				case MapType.CoD: {
					return 25;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets the length of the <see cref="Vertex"/> struct for the given <see cref="MapType"/> and <paramref name="version"/>.
		/// </summary>
		/// <param name="type">The type of BSP to get struct length for.</param>
		/// <param name="version">Version of the lump.</param>
		/// <returns>The length of the struct for the given <see cref="MapType"/> of the given <paramref name="version"/>.</returns>
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Vindictus:
				case MapType.DMoMaM:
				case MapType.Titanfall: {
					structLength = 12;
					break;
				}
				case MapType.CoD: {
					if (version == 0) {
						goto case MapType.Quake3;
					} else if (version == 1) {
						goto case MapType.Quake;
					}
					break;
				}
				case MapType.CoD2: {
					structLength = 68;
					break;
				}
				case MapType.MOHAA:
				case MapType.Quake3:
				case MapType.FAKK: {
					structLength = 44;
					break;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					structLength = 48;
					break;
				}
				case MapType.Raven: {
					structLength = 80;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " doesn't use a Vertex lump or the lump is unknown.");
				}
			}
			return structLength;
		}

		/// <summary>
		/// Gets a byte array for this <see cref="Vertex"/> to be inserted into a map of type <paramref name="type"/> with lump version <paramref name="version"/>.
		/// </summary>
		/// <param name="v">This <see cref="Vertex"/>.</param>
		/// <param name="type">The <see cref="MapType"/> this <see cref="Vertex"/> is from.</param>
		/// <param name="version">The version of the lump this <see cref="Vertex"/> is from.</param>
		/// <returns><c>byte</c> array of the data for this <see cref="Vertex"/>.</returns>
		public static byte[] GetBytes(this Vertex v, MapType type, int version) {
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Vindictus:
				case MapType.DMoMaM:
				case MapType.Titanfall: {
					v.position.GetBytes().CopyTo(bytes, 0);
					break;
				}
				case MapType.CoD: {
					if (version == 0) {
						goto case MapType.Quake3;
					} else if (version == 1) {
						goto case MapType.Quake;
					}
					break;
				}
				case MapType.CoD2: {
					v.normal.GetBytes().CopyTo(bytes, 12);
					v.color.GetBytes().CopyTo(bytes, 24);
					v.uv0.GetBytes().CopyTo(bytes, 28);
					v.uv1.GetBytes().CopyTo(bytes, 36);
					// Use these fields to store additional unknown information
					v.tangent.GetBytes().CopyTo(bytes, 44);
					v.uv3.GetBytes().CopyTo(bytes, 60);
					break;
				}
				case MapType.MOHAA:
				case MapType.Quake3:
				case MapType.CoD4:
				case MapType.FAKK: {
					v.uv0.GetBytes().CopyTo(bytes, 12);
					v.uv1.GetBytes().CopyTo(bytes, 20);
					v.normal.GetBytes().CopyTo(bytes, 28);
					v.color.GetBytes().CopyTo(bytes, 40);
					goto case MapType.Quake;
				}
				case MapType.Raven: {
					v.uv0.GetBytes().CopyTo(bytes, 12);
					v.uv1.GetBytes().CopyTo(bytes, 20);
					v.uv2.GetBytes().CopyTo(bytes, 28);
					v.uv3.GetBytes().CopyTo(bytes, 36);
					BitConverter.GetBytes((float)v.tangent.x).CopyTo(bytes, 44);
					BitConverter.GetBytes((float)v.tangent.y).CopyTo(bytes, 48);
					v.normal.GetBytes().CopyTo(bytes, 52);
					v.color.GetBytes().CopyTo(bytes, 64);
					BitConverter.GetBytes((float)v.tangent.z).CopyTo(bytes, 68);
					BitConverter.GetBytes((float)v.tangent.w).CopyTo(bytes, 72);
					// There's actually another field that seems to be a color but I've only ever seen it be FFFFFFFF.
					bytes[76] = 255;
					bytes[77] = 255;
					bytes[78] = 255;
					bytes[79] = 255;
					goto case MapType.Quake;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					v.uv0.GetBytes().CopyTo(bytes, 12);
					v.uv1.GetBytes().CopyTo(bytes, 20);
					BitConverter.GetBytes(v.uv2.x).CopyTo(bytes, 28);
					v.color.GetBytes().CopyTo(bytes, 32);
					v.normal.GetBytes().CopyTo(bytes, 36);
					goto case MapType.Quake;
				}
				default: {
					bytes = new byte[0];
					break;
				}
			}
			return bytes;
		}

	}
}
