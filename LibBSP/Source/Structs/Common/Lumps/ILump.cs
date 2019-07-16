namespace LibBSP {

	/// <summary>
	/// Interface for a Lump object.
	/// </summary>
	public interface ILump {

		/// <summary>
		/// The <see cref="BSP"/> this <see cref="ILump"/> came from.
		/// </summary>
		BSP Bsp { get; }

		/// <summary>
		/// The <see cref="LumpInfo"/> associated with this <see cref="ILump"/>.
		/// </summary>
		LumpInfo LumpInfo { get; }

	}
}
