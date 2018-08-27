#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
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
#endif

	/// <summary>
	/// This class contains the texture scaling information for certain formats.
	/// Some BSP formats lack this lump (or the information is contained in a
	/// different lump) so their cases will be left out.
	/// </summary>
	[Serializable] public class TextureInfo {

		public const int S = 0;
		public const int T = 1;
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

        public Vector3d[] axes;
        public Vector2d translation;
        public Vector2d scale;
        public int flags;
        public int texture;
        public double rotation;

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object with sensible defaults.
		/// </summary>
		public TextureInfo() {
			axes = new Vector3d[2];
			translation = Vector2d.zero;
			scale = Vector2d.one;
			flags = 0;
			texture = 0;
			rotation = 0;
		}

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
			axes = new Vector3d[2];
			translation = new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 28));
			axes[S] = new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			axes[T] = new Vector3d(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
			// Texture scaling information is compiled into the axes by changing their length.
			scale = Vector2d.one;
			rotation = 0;
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
		/// <param name="t">The T texture axis.</param>
		/// <param name="translation">Texture translation along both axes (in pixels).</param>
		/// <param name="scale">Texture scale along both axes.</param>
		/// <param name="flags">The flags for this <see cref="TextureInfo"/>.</param>
		/// <param name="texture">Index into the texture list for the texture this <see cref="TextureInfo"/> uses.</param>
		/// <param name="rotation">Rotation of the texutre axes.</param>
		public TextureInfo(Vector3d s, Vector3d t, Vector2d translation, Vector2d scale, int flags, int texture, double rotation) {
			axes = new Vector3d[2];
			axes[S] = s;
			axes[T] = t;
			this.translation = translation;
			this.scale = scale;
			this.flags = flags;
			this.texture = texture;
			this.rotation = rotation;
		}

		/// <summary>
		/// Given a <see cref="Plane"/> <c>p</c>, return an optimal set of texture axes for it.
		/// </summary>
		/// <param name="p"><see cref="Plane"/> of the surface.</param>
		/// <returns>The best matching texture axes for the given <see cref="Plane"/>.</returns>
		public static Vector3d[] TextureAxisFromPlane(Plane p) {
			int bestaxis = 0;
			double best = 0; // "Best" dot product so far
			for (int i = 0; i < 6; ++i) {
				// For all possible axes, positive and negative
				double dot = Vector3d.Dot(p.normal, new Vector3d(baseAxes[i * 3][0], baseAxes[i * 3][1], baseAxes[i * 3][2]));
				if (dot > best) {
					best = dot;
					bestaxis = i;
				}
			}
			Vector3d[] newAxes = new Vector3d[2];
			newAxes[0] = new Vector3d(baseAxes[bestaxis * 3 + 1][0], baseAxes[bestaxis * 3 + 1][1], baseAxes[bestaxis * 3 + 1][2]);
			newAxes[1] = new Vector3d(baseAxes[bestaxis * 3 + 2][0], baseAxes[bestaxis * 3 + 2][1], baseAxes[bestaxis * 3 + 2][2]);
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
			int numObjects = data.Length / structLength;
			List<TextureInfo> lump = new List<TextureInfo>(numObjects);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < numObjects; ++i) {
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
