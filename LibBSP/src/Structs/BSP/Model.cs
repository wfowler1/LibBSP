using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// An attempt at an all-encompassing model class containing all data needed for any
	/// given models lump in any given BSP.
	/// </summary>
	/// <remarks>
	/// In general, we need to use models to find one or more leaves containing the
	/// information for the solids described by this model. Some formats do it by
	/// referencing a head node to iterate through and find the leaves. Others
	/// directly point to a set of leaves, and still others simply directly reference
	/// brushes. The ideal format simply points to brush information from here (Quake
	/// 3-based engines do), but most of them don't.
	/// </remarks>
	public struct Model {

		public int headNode { get; private set; } // Quake, Half-life, Quake 2, SiN
		public int firstLeaf { get; private set; } // 007 nightfire
		public int numLeaves { get; private set; }
		public int firstBrush { get; private set; } // Quake 3 and derivatives
		public int numBrushes { get; private set; }
		public int firstFace { get; private set; } // Quake/GoldSrc
		public int numFaces { get; private set; }

		/// <summary>
		/// Creates a new <c>Model</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public Model(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			headNode = -1;
			firstLeaf = -1;
			numLeaves = -1;
			firstBrush = -1;
			numBrushes = -1;
			firstFace = -1;
			numFaces = -1;
			switch (type) {
				case MapType.Quake: {
					headNode = BitConverter.ToInt32(data, 36);
					firstFace = BitConverter.ToInt32(data, 56);
					numFaces = BitConverter.ToInt32(data, 60);
					break;
				}
				case MapType.Quake2:
				case MapType.Daikatana:
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
				case MapType.TacticalIntervention:
				case MapType.Vindictus: {
					headNode = BitConverter.ToInt32(data, 36);
					firstFace = BitConverter.ToInt32(data, 40);
					numFaces = BitConverter.ToInt32(data, 44);
					break;
				}
				case MapType.DMoMaM: {
					headNode = BitConverter.ToInt32(data, 40);
					firstFace = BitConverter.ToInt32(data, 44);
					numFaces = BitConverter.ToInt32(data, 48);
					break;
				}
				case MapType.Nightfire: {
					firstLeaf = BitConverter.ToInt32(data, 40);
					numLeaves = BitConverter.ToInt32(data, 44);
					firstFace = BitConverter.ToInt32(data, 48);
					numFaces = BitConverter.ToInt32(data, 52);
					break;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.Quake3:
				case MapType.MOHAA:
				case MapType.Raven:
				case MapType.FAKK: {
					firstFace = BitConverter.ToInt32(data, 24);
					numFaces = BitConverter.ToInt32(data, 28);
					firstBrush = BitConverter.ToInt32(data, 32);
					numBrushes = BitConverter.ToInt32(data, 36);
					break;
				}
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4: {
					firstBrush = BitConverter.ToInt32(data, 40);
					numBrushes = BitConverter.ToInt32(data, 44);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Model class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Model</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Model</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<Model> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.FAKK: {
					structLength = 40;
					break;
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.CoD:
				case MapType.CoD2:
				case MapType.CoD4:
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
				case MapType.TacticalIntervention:
				case MapType.Vindictus: {
					structLength = 48;
					break;
				}
				case MapType.DMoMaM: {
					structLength = 52;
					break;
				}
				case MapType.Nightfire: {
					structLength = 56;
					break;
				}
				case MapType.Quake: {
					structLength = 64;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Leaf lump factory.");
				}
			}
			int offset = 0;
			List<Model> lump = new List<Model>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Model(bytes, type));
				offset += structLength;
			}
			return lump;
		}
	}
}
