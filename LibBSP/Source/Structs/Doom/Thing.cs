#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP.Doom {
#if !UNITY
	using Vector2 = Vector2d;
	using Vector3 = Vector3d;
#endif
	/// <summary>
	/// Contains all necessary information for a Doom THING object. Essentially Doom's entities.
	/// </summary>
	public struct Thing {

		public Vector3 origin { get; private set; }
		public short angle { get; private set; }
		public short classNum { get; private set; }
		public short flags { get; private set; }

		public short id { get; private set; }
		public byte action { get; private set; }
		public byte[] arguments { get; private set; }

		/// <summary>
		/// Creates a new <c>Thing</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public Thing(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			switch (type) {
				case MapType.Doom: {
					origin = new Vector2(BitConverter.ToInt16(data, 0), BitConverter.ToInt16(data, 2));
					this.angle = BitConverter.ToInt16(data, 4);
					this.classNum = BitConverter.ToInt16(data, 6);
					this.flags = BitConverter.ToInt16(data, 8);
					break;
				}
				case MapType.Hexen: {
					id = BitConverter.ToInt16(data, 0);
					origin = new Vector3(BitConverter.ToInt16(data, 2), BitConverter.ToInt16(data, 4), BitConverter.ToInt16(data, 6));
					this.angle = BitConverter.ToInt16(data, 8);
					this.classNum = BitConverter.ToInt16(data, 10);
					this.flags = BitConverter.ToInt16(data, 12);
					action = data[14];
					arguments[0] = data[15];
					arguments[1] = data[16];
					arguments[2] = data[17];
					arguments[3] = data[18];
					arguments[4] = data[19];
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Thing class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Thing</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Thing</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<Thing> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Doom: {
					structLength = 10;
					break;
				}
				case MapType.Hexen: {
					structLength = 20;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Thing lump factory.");
				}
			}
			List<Thing> lump = new List<Thing>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, i * structLength, bytes, 0, structLength);
				lump.Add(new Thing(bytes, type));
			}
			return lump;
		}
	}
}
