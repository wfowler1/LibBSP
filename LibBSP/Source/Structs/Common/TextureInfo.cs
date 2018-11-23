#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
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

		public byte[] data;
		public MapType type;
		public int version;

		// No BSP format uses these so they are fields.
		public Vector2d scale = Vector2d.one;
		public double rotation = 0;

		public Vector3d uAxis {
			get {
				return new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
			}
			set {
				value.GetBytes().CopyTo(data, 0);
			}
		}

		public Vector3d vAxis {
			get {
				return new Vector3d(BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20), BitConverter.ToSingle(data, 24));
			}
			set {
				value.GetBytes().CopyTo(data, 16);
			}
		}

		public Vector2d translation {
			get {
				return new Vector2d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 28));
			}
			set {
				BitConverter.GetBytes((float)value.x).CopyTo(data, 12);
				BitConverter.GetBytes((float)value.y).CopyTo(data, 28);
			}
		}

		public int flags {
			get {
				switch (type) {
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
						return BitConverter.ToInt32(data, 64);
					}
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(data, 88);
					}
					case MapType.Quake: {
						return BitConverter.ToInt32(data, 36);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						bytes.CopyTo(data, 64);
						break;
					}
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 88);
						break;
					}
					case MapType.Quake: {
						bytes.CopyTo(data, 36);
						break;
					}
				}
			}
		}

		public int texture {
			get {
				switch (type) {
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
						return BitConverter.ToInt32(data, 68);
					}
					case MapType.DMoMaM: {
						return BitConverter.ToInt32(data, 92);
					}
					case MapType.Quake: {
						return BitConverter.ToInt32(data, 32);
					}
					default: {
						return -1;
					}
				}
			}
			set {
				byte[] bytes = BitConverter.GetBytes(value);
				switch (type) {
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
						bytes.CopyTo(data, 68);
						break;
					}
					case MapType.DMoMaM: {
						bytes.CopyTo(data, 92);
						break;
					}
					case MapType.Quake: {
						bytes.CopyTo(data, 32);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object with sensible defaults.
		/// </summary>
		public TextureInfo() {
			data = new byte[40];
			type = MapType.Quake;
			version = 0;

			uAxis = Vector3d.zero;
			vAxis = Vector3d.zero;
			translation = Vector2d.zero;
			flags = 0;
			texture = -1;

			scale = Vector2d.one;
			rotation = 0;
		}

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public TextureInfo(byte[] data, MapType type, int version = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;

			scale = Vector2d.one;
			rotation = 0;
		}

		/// <summary>
		/// Creates a new <see cref="TextureInfo"/> object using the passed data.
		/// </summary>
		/// <param name="u">The U texture axis.</param>
		/// <param name="v">The V texture axis.</param>
		/// <param name="translation">Texture translation along both axes (in pixels).</param>
		/// <param name="scale">Texture scale along both axes.</param>
		/// <param name="flags">The flags for this <see cref="TextureInfo"/>.</param>
		/// <param name="texture">Index into the texture list for the texture this <see cref="TextureInfo"/> uses.</param>
		/// <param name="rotation">Rotation of the texutre axes.</param>
		public TextureInfo(Vector3d u, Vector3d v, Vector2d translation, Vector2d scale, int flags, int texture, double rotation) {
			data = new byte[40];
			type = MapType.Quake;
			version = 0;

			uAxis = u;
			vAxis = v;
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
			int bestaxis = p.BestAxis();
			Vector3d[] newAxes = new Vector3d[2];
			newAxes[0] = PlaneExtensions.baseAxes[bestaxis * 3 + 1];
			newAxes[1] = PlaneExtensions.baseAxes[bestaxis * 3 + 2];
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
			for (int i = 0; i < numObjects; ++i) {
				byte[] bytes = new byte[structLength];
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
