#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER)
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#else
using System.Drawing;
#endif

namespace LibBSP {
#if !UNITY
	using Color32 = Color;
	using Vector2 = Vector2d;
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Static class containing helper methods for <see cref="UIVertex"/> objects.
	/// </summary>
	public static class UIVertexExtensions {

		/// <summary>
		/// Scales the position of this <see cref="UIVertex"/> by a number.
		/// </summary>
		/// <param name="v1">This <see cref="UIVertex"/>.</param>
		/// <param name="scalar">Scalar value.</param>
		/// <returns>The scaled <see cref="UIVertex"/>.</returns>
		public static UIVertex Scale(this UIVertex v1, float scalar) {
			v1.position *= scalar;
			return v1;
		}

		/// <summary>
		/// Adds the position of this <see cref="UIVertex"/> to another <see cref="UIVertex"/>.
		/// </summary>
		/// <param name="v1">This <see cref="UIVertex"/>.</param>
		/// <param name="v2">The other <see cref="UIVertex"/>.</param>
		/// <returns>The resulting <see cref="UIVertex"/>.</returns>
		public static UIVertex Add(this UIVertex v1, UIVertex v2) {
			return new UIVertex {
				color = v1.color,
				normal = v1.normal,
				position = v1.position + v2.position,
				tangent = v1.tangent,
				uv0 = v1.uv0 + v2.uv0,
				uv1 = v1.uv1 + v2.uv1
			};
		}

		/// <summary>
		/// Adds the position of a <c>Vector3</c> to this <see cref="UIVertex"/>.
		/// </summary>
		/// <param name="v1">This <see cref="UIVertex"/>.</param>
		/// <param name="v2">The <see cref="Vector3"/>.</param>
		/// <returns>The resulting <see cref="UIVertex"/>.</returns>
		public static UIVertex Translate(this UIVertex v1, Vector3 v2) {
			return new UIVertex {
				color = v1.color,
				normal = v1.normal,
				position = v1.position + v2,
				tangent = v1.tangent,
				uv0 = v1.uv0,
				uv1 = v1.uv1
			};
		}

		/// <summary>
		/// Creates a new <see cref="UIVertex"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>The resulting <see cref="UIVertex"/> object.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was null.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		/// <remarks><see cref="UIVertex"/> has no constructor, so the object must be initialized field-by-field. Even if it
		/// did have a constructor, the way data needs to be read wouldn't allow use of it.</remarks>
		public static UIVertex CreateVertex(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			UIVertex result = new UIVertex();
			switch (type) {
				case MapType.MOHAA:
				case MapType.Quake3:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
				case MapType.FAKK: {
					result.uv0 = new Vector2(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.color = Color32Extensions.FromArgb(data[43], data[40], data[41], data[42]);
					goto case MapType.DMoMaM;
				}
				case MapType.Raven: {
					result.uv0 = new Vector2(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.color = Color32Extensions.FromArgb(data[67], data[64], data[65], data[66]);
					goto case MapType.DMoMaM;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					result.uv0 = new Vector2(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16));
					result.color = Color32Extensions.FromArgb(data[35], data[32], data[33], data[34]);
					goto case MapType.DMoMaM;
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
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					result.position = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the UIVertex class factory.");
				}
			}
			return result;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="UIVertex"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="UIVertex"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		/// <remarks>This function goes here since I can't put it into Unity's <c>UIVertex</c> class, and so I can't
		/// depend on having a constructor taking a byte array.</remarks>
		public static List<UIVertex> LumpFactory(byte[] data, MapType type, int version = 0) {
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
				case MapType.L4D2:
				case MapType.TacticalInterventionEncrypted:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					structLength = 12;
					break;
				}
				case MapType.MOHAA:
				case MapType.Quake3:
				case MapType.CoD:
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
					throw new ArgumentException("Map type " + type + " isn't supported by the Vertex lump factory.");
				}
			}
			List<UIVertex> lump = new List<UIVertex>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(CreateVertex(bytes, type, version));
			}
			return lump;
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
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
				case MapType.DMoMaM: {
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
				case MapType.Raven:
				case MapType.Quake3: {
					return 10;
				}
				default: {
					return -1;
				}
			}
		}

	}
}
