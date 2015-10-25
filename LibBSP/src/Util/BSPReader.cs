using System;
using System.IO;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Handles reading of a BSP file on-demand.
	/// </summary>
	public class BSPReader {
		private FileStream stream;
		private BinaryReader binaryReader;

		private bool _bigEndian = false;

		// Decryption key for Tactical Intervention; should be 32 bytes
		private byte[] key = new byte[0];

		public bool bigEndian { get { return _bigEndian; } }

		/// <summary>
		/// Creates a new instance of a <c>BSPReader</c> class to read the specified file.
		/// </summary>
		/// <param name="file"></param>
		public BSPReader(FileInfo file) {
			if (!File.Exists(file.FullName)) {
				throw new FileNotFoundException("Unable to open BSP file; file " + file.FullName + " not found.");
			} else {
				this.stream = new FileStream(file.FullName, FileMode.Open);
				this.binaryReader = new BinaryReader(this.stream);
			}
		}

		/// <summary>
		/// Reads this lump in the map.
		/// </summary>
		/// <param name="index">The index of the lump to get</param>
		/// <param name="version">The version of BSP this is</param>
		/// <returns>Array of bytes read from the BSP file</returns>
		public byte[] ReadLumpNum(int index, MapType version) {
			switch (version) {
				case MapType.Quake:
				case MapType.Nightfire: {
					return ReadLumpFromOffsetLengthPairAtOffset(4 + (8 * index), version);
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.CoD: {
					return ReadLumpFromOffsetLengthPairAtOffset(8 + (8 * index), version);
				}
				case MapType.CoD2: {
					stream.Seek(8 + (8 * index), SeekOrigin.Begin);
					int temp = binaryReader.ReadInt32();
					return ReadLump(binaryReader.ReadInt32(), temp, version);
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.FAKK: {
					return ReadLumpFromOffsetLengthPairAtOffset(12 + (8 * index), version);
				}
				case MapType.CoD4: {
					stream.Seek(8, SeekOrigin.Begin);
					int numlumps = binaryReader.ReadInt32();
					int offset = (numlumps * 8) + 12;
					for (int i = 0; i < numlumps; i++) {
						int id = binaryReader.ReadInt32();
						int length = binaryReader.ReadInt32();
						if (id == index) {
							return ReadLump(offset, length, version);
						} else {
							offset += length;
							while (offset % 4 != 0) { offset++; }
						}
					}
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
				case MapType.TacticalInterventionEncrypted:
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					return ReadLumpFromOffsetLengthPairAtOffset(8 + (16 * index), version);
				}
				/*
				case MapType.Doom:
				case MapType.Hexen: {
					int[] ol = getLumpInfo(index);
					return readLump(ol[0], ol[1]);
				}*/
			}
			return new byte[0];
		}

		/// <summary>
		/// Returns the lump referenced by the offset/length pair at the specified <paramref name="offset"/>,
		/// read as two Int32.
		/// </summary>
		/// <param name="offset">The byte offset for the offset/length pair</param>
		/// <param name="version">The version of BSP this is</param>
		/// <returns>Array of bytes read from the BSP file</returns>
		private byte[] ReadLumpFromOffsetLengthPairAtOffset(int offset, MapType version) {
			stream.Seek(offset, SeekOrigin.Begin);
			byte[] input = binaryReader.ReadBytes(12);
			if (version == MapType.TacticalInterventionEncrypted) {
				input = XorWithKeyStartingAtIndex(input, offset);
			}
			int lumpOffset;
			int lumpLength;
			if (version == MapType.L4D2) {
				lumpOffset = BitConverter.ToInt32(input, 4);
				lumpLength = BitConverter.ToInt32(input, 8);
			} else {
				lumpOffset = BitConverter.ToInt32(input, 0);
				lumpLength = BitConverter.ToInt32(input, 4);
			}
			/*if (bigEndian) {
				byte[] bytes = BitConverter.GetBytes(lumpLength);
				Array.Reverse(bytes);
				lumpLength = BitConverter.ToInt32(bytes, 0);
				bytes = BitConverter.GetBytes(lumpOffset);
				Array.Reverse(bytes);
				lumpOffset = BitConverter.ToInt32(bytes, 0);
			}*/
			return ReadLump(lumpOffset, lumpLength, version);
		}

		/// <summary>
		/// Reads the lump <paramref name="length"/> bytes long at <paramref name="offset"/> in the file
		/// </summary>
		/// <param name="offset">Offset to start reading from</param>
		/// <param name="length">Length of the lump to read</param>
		/// <param name="version">The version of BSP this is</param>
		/// <returns>Array of bytes read from the BSP file</returns>
		public byte[] ReadLump(int offset, int length, MapType version) {
			stream.Seek(offset, SeekOrigin.Begin);
			byte[] input = binaryReader.ReadBytes(length);
			if (version == MapType.TacticalInterventionEncrypted) {
				input = XorWithKeyStartingAtIndex(input, offset);
			}
			return input;
		}

		/// <summary>
		/// Xors the <paramref name="data"/> <c>byte</c> array with the localls stored key <c>byte</c> array, starting at a certain <paramref name="index"/> in the key.
		/// </summary>
		/// <param name="data">The byte array to Xor</param>
		/// <param name="index">The index in the key byte array to start reading from</param>
		/// <returns>The input <c>byte</c> array Xored with the key <c>byte</c> array</returns>
		/// <exception cref="ArgumentNullException">The passed <paramref name="data"/> parameter was null</exception>
		private byte[] XorWithKeyStartingAtIndex(byte[] data, int index = 0) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			if (key.Length == 0 || data.Length == 0) {
				return data;
			}
			byte[] output = new byte[data.Length];
			for (int i = 0; i < data.Length; i++) {
				output[i] = (byte)(data[i] ^ key[(i + index) % key.Length]);
			}
			return output;
		}

		/// <summary>
		/// Tries to get the <c>MapType</c> member most closely represented by the referenced file. If the file is 
		/// found to be big-endian, this will set <c>bigEndian</c> to <c>true</c>.
		/// </summary>
		/// <returns>The <c>MapType</c> of this BSP, <c>MapType.Undefined</c> if it could not be determined</returns>
		public MapType GetVersion() {
			MapType ret = GetVersion(false);
			if (ret == MapType.Undefined) {
				ret = GetVersion(true);
				if (ret != MapType.Undefined) {
					_bigEndian = true;
				}
			}
			return ret;
		}

		/// <summary>
		/// Tries to get the <c>MapType</c> member most closely represented by the referenced file.
		/// </summary>
		/// <param name="bigEndian">Set to <c>true</c> to attempt reading the data in big-endian byte order</param>
		/// <returns>The <c>MapType</c> of this BSP, <c>MapType.Undefined</c> if it could not be determined</returns>
		private MapType GetVersion(bool bigEndian) {
			MapType current = MapType.Undefined;
			stream.Seek(0, SeekOrigin.Begin);
			int data = binaryReader.ReadInt32();
			if (bigEndian) {
				byte[] bytes = BitConverter.GetBytes(data);
				Array.Reverse(bytes);
				data = BitConverter.ToInt32(bytes, 0);
			}
			if (data == 1347633737) {
				// 1347633737 reads in ASCII as "IBSP"
				// Versions: CoD, CoD2, CoD4, Quake 2, Daikatana, Quake 3 (RtCW), Soldier of Fortune
				data = binaryReader.ReadInt32();
				if (bigEndian) {
					byte[] bytes = BitConverter.GetBytes(data);
					Array.Reverse(bytes);
					data = BitConverter.ToInt32(bytes, 0);
				}
				switch (data) {
					case 4: {
						current = MapType.CoD2;
						break;
					}
					case 22: {
						current = MapType.CoD4;
						break;
					}
					case 38: {
						current = MapType.Quake2;
						break;
					}
					case 41: {
						current = MapType.Daikatana;
						break;
					}
					case 46: {
						current = MapType.Quake3;
						// This version number is both Quake 3 and Soldier of Fortune. Find out the length of the
						// header, based on offsets.
						for (int i = 0; i < 17; i++) {
							stream.Seek((i + 1) * 8, SeekOrigin.Begin);
							int temp = binaryReader.ReadInt32();
							if (bigEndian) {
								byte[] bytes = BitConverter.GetBytes(temp);
								Array.Reverse(bytes);
								temp = BitConverter.ToInt32(bytes, 0);
							}
							if (temp == 184) {
								current = MapType.SoF;
								break;
							} else {
								if (temp == 144) {
									break;
								}
							}
						}
						break;
					}
					case 47: {
						current = MapType.Quake3;
						break;
					}
					case 59: {
						current = MapType.CoD;
						break;
					}
				}
			} else {
				if (data == 892416050) {
					// 892416050 reads in ASCII as "2015," the game studio which developed MoHAA
					current = MapType.MOHAA;
				} else {
					if (data == 1095516485) {
						// 1095516485 reads in ASCII as "EALA," the ones who developed MoHAA Spearhead and Breakthrough
						current = MapType.MOHAA;
					} else {
						if (data == 1347633750) {
							// 1347633750 reads in ASCII as "VBSP." Indicates Source engine.
							// Some source games handle this as 2 shorts.
							// TODO: Big endian?
							// Formats: Source 17-23 and 27, DMoMaM, Vindictus
							data = (int)binaryReader.ReadUInt16();
							switch (data) {
								case 17: {
									current = MapType.Source17;
									break;
								}
								case 18: {
									current = MapType.Source18;
									break;
								}
								case 19: {
									current = MapType.Source19;
									break;
								}
								case 20: {
									int version2 = (int)binaryReader.ReadUInt16();
									if (version2 == 4) {
										// TODO: This doesn't necessarily mean the whole map should be read as DMoMaM.
										current = MapType.DMoMaM;
									} else {
										// TODO: Vindictus? Before I was determining these by looking at the Game Lump data, is there a better way?
										current = MapType.Source20;
									}
									break;
								}
								case 21: {
									current = MapType.Source21;
									// Hack to determine if this is a L4D2 map. Read what would normally be the offset of
									// a lump. If it is less than the header length it's probably not an offset, indicating L4D2.
									stream.Seek(8, SeekOrigin.Begin);
									int test = binaryReader.ReadInt32();
									if (bigEndian) {
										byte[] bytes = BitConverter.GetBytes(test);
										Array.Reverse(bytes);
										test = BitConverter.ToInt32(bytes, 0);
									}
									if (test < 1032) {
										current = MapType.L4D2;
									}
									break;
								}
								case 22: {
									current = MapType.Source22;
									break;
								}
								case 23: {
									current = MapType.Source23;
									break;
								}
								case 27: {
									current = MapType.Source27;
									break;
								}
							}
						} else {
							if (data == 1347633746) {
								// Reads in ASCII as "RBSP". Raven software's modification of Q3BSP, or Ritual's modification of Q2.
								// Formats: Raven, SiN
								current = MapType.Raven;
								for (int i = 0; i < 17; i++) {
									// Find out where the first lump starts, based on offsets.
									stream.Seek((i + 1) * 8, SeekOrigin.Begin);
									int temp = binaryReader.ReadInt32();
									if (bigEndian) {
										byte[] bytes = BitConverter.GetBytes(temp);
										Array.Reverse(bytes);
										temp = BitConverter.ToInt32(bytes, 0);
									}
									if (temp == 168) {
										current = MapType.SiN;
										break;
									} else {
										if (temp == 152) {
											break;
										}
									}
								}
							} else {
								if (data == 556942917) {
									// "EF2!"
									current = MapType.STEF2;
								} else {
									if (data == 1263223110) {
										// "FAKK"
										// Formats: STEF2 demo, Heavy Metal FAKK2 (American McGee's Alice)
										data = binaryReader.ReadInt32();
										if (bigEndian) {
											byte[] bytes = BitConverter.GetBytes(data);
											Array.Reverse(bytes);
											data = BitConverter.ToInt32(bytes, 0);
										}
										switch (data) {
											case 19: {
												current = MapType.STEF2Demo;
												break;
											}
											case 12:
											case 42: {// American McGee's Alice
												current = MapType.FAKK;
												break;
											}
										}
									} else {
										switch (data) {
											// Various numbers not representing a string
											// Formats: HL1, Quake, Nightfire, or perhaps Tactical Intervention's encrypted format
											case 29:
											case 30: {
												current = MapType.Quake;
												break;
											}
											case 42: {
												current = MapType.Nightfire;
												break;
											}
											default: {
												// Hack to get Tactical Intervention's encryption key. At offset 384, there are two unused lumps whose
												// values in the header are always 0s. Grab these 32 bytes (256 bits) and see if they match an expected value.
												stream.Seek(384, SeekOrigin.Begin);
												key = binaryReader.ReadBytes(32);
												stream.Seek(0, SeekOrigin.Begin);
												data = BitConverter.ToInt32(XorWithKeyStartingAtIndex(binaryReader.ReadBytes(4)), 0);
												if (data == 1347633750) {
													current = MapType.TacticalInterventionEncrypted;
												} else {
													current = MapType.Undefined;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return current;
		}
	}
}
