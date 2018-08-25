using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Class representing a group of <see cref="DisplacementVertex"/> objects. Contains helpful methods to handle Displacement Vertices in the <c>List</c>.
	/// </summary>
	public class DisplacementVertices : List<DisplacementVertex> {

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <c>List</c> of <see cref="DisplacementVertex"/> objects.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="type">Format identifier.</param>
		/// <param name="version">The version of this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public DisplacementVertices(byte[] data, MapType type, int version = 0) : base(data.Length / 20) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 20;
			byte[] bytes = new byte[structLength];
			int numObjects = data.Length / structLength;
			for (int i = 0; i < numObjects; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				Add(new DisplacementVertex(bytes, type, version));
			}
		}

		/// <summary>
		/// Gets enough vertices from the list for a displacement of power <paramref name="power"/>, starting at <paramref name="first"/>.
		/// </summary>
		/// <param name="first">The first vertex to get.</param>
		/// <param name="power">The power of the displacement.</param>
		/// <returns>Array of <see cref="DisplacementVertex"/> objects containing all the vertices in this displacement</returns>
		public virtual DisplacementVertex[] GetVerticesInDisplacement(int first, int power) {
			int numVerts = 0;
			switch (power) {
				case 2: {
					numVerts = 25;
					break;
				}
				case 3: {
					numVerts = 81;
					break;
				}
				case 4: {
					numVerts = 289;
					break;
				}
			}
			DisplacementVertex[] ret = new DisplacementVertex[numVerts];
			for (int i = 0; i < numVerts; ++i) {
				ret[i] = this[first + i];
			}
			return ret;
		}

	}
}
