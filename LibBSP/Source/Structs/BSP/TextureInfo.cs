#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
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
	/// This class contains the texture scaling information for certain formats.
	/// Some BSP formats lack this lump (or the information is contained in a
	/// different lump) so their cases will be left out.
	/// </summary>
	public class TextureInfo {

		public const int S = 0;
		public const int T = 1;
		/// <summary>
		/// Array of base texture axes. When referenced properly, provides a good default texture axis for any given plane.
		/// </summary>
		public static readonly Vector3[] baseAxes = new Vector3[] { 
			new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, -1, 0),
			new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(0, -1, 0),
			new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, -1),
			new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, -1),
			new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, -1),
			new Vector3(0, -1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, -1)
		};

		public Vector3[] axes { get; private set; }
		public float[] shifts { get; private set; }
		public float[] scales { get; private set; }
		public int flags { get; private set; }
		public int texture { get; private set; }

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public TextureInfo(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			texture = -1;
			flags = -1;
			axes = new Vector3[2];
			shifts = new float[2];
			axes[S] = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			shifts[S] = BitConverter.ToSingle(data, 12);
			axes[T] = new Vector3(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
			shifts[T] = BitConverter.ToSingle(data, 28);
			// Texture scaling information is compiled into the axes by changing their length.
			scales = new float[] { 1, 1 };
			switch (type) {
				// Excluded engines: Quake 2-based, Quake 3-based
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
				case MapType.Vindictus: {
					texture = BitConverter.ToInt32(data, 68);
					flags = BitConverter.ToInt32(data, 64);
					break;
				}
				case MapType.DMoMaM: {
					texture = BitConverter.ToInt32(data, 92);
					flags = BitConverter.ToInt32(data, 88);
					break;
				}
				case MapType.Quake: {
					texture = BitConverter.ToInt32(data, 32);
					flags = BitConverter.ToInt32(data, 36);
					break;
				}
				case MapType.Nightfire: {
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the TextureInfo class.");
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object using the passed data.
		/// </summary>
		/// <param name="s">The S texture axis.</param>
		/// <param name="sShift">The texture shift on the S axis.</param>
		/// <param name="sScale">The texture scale on the S axis.</param>
		/// <param name="t">The T texture axis.</param>
		/// <param name="tShift">The texture shift on the T axis.</param>
		/// <param name="tScale">The texture scale on the T axis.</param>
		/// <param name="flags">The flags for this <see cref="TextureInfo"/>.</param>
		/// <param name="texture">Index into the texture list for the texture this <see cref="TextureInfo"/> uses.</param>
		public TextureInfo(Vector3 s, float sShift, float sScale, Vector3 t, float tShift, float tScale, int flags, int texture) {
			axes = new Vector3[2];
			axes[S] = s;
			axes[T] = t;
			shifts = new float[2];
			scales = new float[2];
			shifts[S] = sShift;
			shifts[T] = tShift;
			scales[S] = sScale;
			scales[T] = tScale;
			this.flags = flags;
			this.texture = texture;
		}

		/// <summary>
		/// Adapted from code in the Quake III Arena source code. Stolen without permission
		/// because it falls under the terms of the GPL v2 license, and because I'm not making
		/// any money, just awesome tools.
		/// </summary>
		/// <param name="p"><see cref="Plane"/> of the surface.</param>
		/// <returns>The best matching texture axes for the given <see cref="Plane"/>.</returns>
		public static Vector3[] TextureAxisFromPlane(Plane p) {
			int bestaxis = 0;
			double best = 0; // "Best" dot product so far
			for (int i = 0; i < 6; ++i) {
				// For all possible axes, positive and negative
				double dot = Vector3.Dot(p.normal, new Vector3(baseAxes[i * 3][0], baseAxes[i * 3][1], baseAxes[i * 3][2]));
				if (dot > best) {
					best = dot;
					bestaxis = i;
				}
			}
			Vector3[] newAxes = new Vector3[2];
			newAxes[0] = new Vector3(baseAxes[bestaxis * 3 + 1][0], baseAxes[bestaxis * 3 + 1][1], baseAxes[bestaxis * 3 + 1][2]);
			newAxes[1] = new Vector3(baseAxes[bestaxis * 3 + 2][0], baseAxes[bestaxis * 3 + 2][1], baseAxes[bestaxis * 3 + 2][2]);
			return newAxes;
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <see cref="TextureInfo"/> objects.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>A <c>List</c> of <see cref="TextureInfo"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> was null.</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype.</exception>
		public static List<TextureInfo> LumpFactory(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Nightfire: {
					structLength = 32;
					break;
				}
				case MapType.Quake: {
					structLength = 40;
					break;
				}
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
				case MapType.Vindictus: {
					structLength = 72;
					break;
				}
				case MapType.DMoMaM: {
					structLength = 96;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Leaf lump factory.");
				}
			}
			List<TextureInfo> lump = new List<TextureInfo>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new TextureInfo(bytes, type, version));
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
					return 6;
				}
				case MapType.Nightfire: {
					return 17;
				}
				default: {
					return -1;
				}
			}
		}
	}
}
