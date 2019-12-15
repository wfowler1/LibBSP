using System;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Class representing a group of <see cref="DisplacementVertex"/> objects. Contains helpful methods to handle Displacement Vertices in the <c>List</c>.
	/// </summary>
	public class DisplacementVertices : Lump<DisplacementVertex> {

		/// <summary>
		/// Creates an empty <see cref="DisplacementVertices"/> object.
		/// </summary>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public DisplacementVertices(BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(bsp, lumpInfo) { }

		/// <summary>
		/// Creates a new <see cref="DisplacementVertices"/> that contains elements copied from the passed <see cref="IEnumerable{DisplacementVertex}"/>.
		/// </summary>
		/// <param name="items">The elements to copy into this <c>Lump</c>.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public DisplacementVertices(IEnumerable<DisplacementVertex> items, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(items, bsp, lumpInfo) { }

		/// <summary>
		/// Creates an empty <see cref="DisplacementVertices"/> object with the specified initial capactiy.
		/// </summary>
		/// <param name="capacity">The number of elements that can initially be stored.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		public DisplacementVertices(int capacity, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(capacity, bsp, lumpInfo) { }

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <see cref="DisplacementVertices"/> object.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="structLength">Number of <c>byte</c>s to copy into the children.</param>
		/// <param name="bsp">The <see cref="BSP"/> this lump came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> associated with this lump.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public DisplacementVertices(byte[] data, int structLength, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(data, structLength, bsp, lumpInfo) { }

		/// <summary>
		/// Gets enough vertices from the list for a displacement of power <paramref name="power"/>, starting at <paramref name="first"/>.
		/// </summary>
		/// <param name="first">The first vertex to get.</param>
		/// <param name="power">The power of the displacement.</param>
		/// <returns>Array of <see cref="DisplacementVertex"/> objects containing all the vertices in this displacement</returns>
		public DisplacementVertex[] GetVerticesInDisplacement(int first, int power) {
			int numSideVerts = (int)Math.Pow(2, power) + 1;
			int numVerts = numSideVerts * numSideVerts;
			DisplacementVertex[] ret = new DisplacementVertex[numVerts];
			for (int i = 0; i < numVerts; ++i) {
				ret[i] = this[first + i];
			}
			return ret;
		}

	}
}
