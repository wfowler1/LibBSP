using System;
using System.Collections.Generic;

namespace LibBSP {
	public class Lump<T> : List<T>, ILump {

		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		public BSP Bsp { get; protected set; }

		/// <summary>
		/// The <see cref="LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		public LumpInfo LumpInfo { get; protected set; }
		
		/// <summary>
		/// Creates an empty <c>Lump</c> of <typeparamref name="T"/> objects.
		/// </summary>
		/// <param name="bsp">The <see cref="BSP"/> which <paramref name="data"/> came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> object for this <c>Lump</c>.</param>
		public Lump(BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) {
			Bsp = bsp;
			LumpInfo = lumpInfo;
		}

		/// <summary>
		/// Creates a new <c>Lump</c> that contains elements copied from the passed <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="items">The elements to copy into this <c>Lump</c>.</param>
		/// <param name="bsp">The <see cref="BSP"/> which <paramref name="data"/> came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> object for this <c>Lump</c>.</param>
		public Lump(IEnumerable<T> items, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(items) {
			Bsp = bsp;
			LumpInfo = lumpInfo;
		}

		/// <summary>
		/// Creates an empty <c>Lump</c> of <typeparamref name="T"/> objects with the specified initial capactiy.
		/// </summary>
		/// <param name="capacity">The number of elements that can initially be stored.</param>
		/// <param name="bsp">The <see cref="BSP"/> which <paramref name="data"/> came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> object for this <c>Lump</c>.</param>
		public Lump(int capacity, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(capacity) {
			Bsp = bsp;
			LumpInfo = lumpInfo;
		}

		/// <summary>
		/// Parses the passed <c>byte</c> array into a <c>Lump</c> of <typeparamref name="T"/> objects.
		/// </summary>
		/// <param name="data">Array of <c>byte</c>s to parse.</param>
		/// <param name="structLength">Number of <c>byte</c>s to copy into the elements. Negative values indicate a variable length, which is not supported by this constructor.</param>
		/// <param name="bsp">The <see cref="BSP"/> which <paramref name="data"/> came from.</param>
		/// <param name="lumpInfo">The <see cref="LumpInfo"/> object for this <c>Lump</c>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		/// <exception cref="NotSupportedException"><paramref name="structLength"/> is negative.</exception>
		public Lump(byte[] data, int structLength, BSP bsp = null, LumpInfo lumpInfo = default(LumpInfo)) : base(data.Length / structLength) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			if (structLength <= 0) {
				throw new NotSupportedException("Cannot use the base Lump constructor for variable length lumps (structLength was negative). Create a derived class with a new constructor instead.");
			}

			Bsp = bsp;
			LumpInfo = lumpInfo;
			for (int i = 0; i < data.Length / structLength; ++i) {
				byte[] bytes = new byte[structLength];
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				Add((T)Activator.CreateInstance(typeof(T), new object[] { bytes, this }));
			}
		}

	}
}
