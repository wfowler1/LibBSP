using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace LibBSP {

	/// <summary>
	/// Handles reading of a BSP file on-demand.
	/// </summary>
	public class BSPReader {

		private FileInfo bspFile;
		private Dictionary<int, LumpInfo> lumpFiles = null;

		private bool _bigEndian = false;

		/// <summary>
		/// An XOr encryption key for encrypted map formats. Must be read and set.
		/// </summary>
		private byte[] key = new byte[0];

		/// <summary>
		/// Was this map determined to be in big endian format?
		/// </summary>
		public bool bigEndian { get { return _bigEndian; } set { _bigEndian = value; } }

		/// <summary>
		/// Creates a new instance of a <see cref="BSPReader"/> class to read the specified file.
		/// </summary>
		/// <param name="file">The <c>FileInfo</c> representing the file this <see cref="BSPReader"/> should read.</param>
		public BSPReader(FileInfo file) {
			if (!File.Exists(file.FullName)) {
				throw new FileNotFoundException("Unable to open BSP file; file " + file.FullName + " not found.");
			} else {
				bspFile = file;
			}
		}

		/// <summary>
		/// Gets the information for lump "<paramref name="index"/>" for this BSP file when reading it as "<paramref name="version"/>".
		/// </summary>
		/// <param name="index">The numerical index of this lump.</param>
		/// <param name="version">The type of BSP to interpret the file as.</param>
		/// <returns>A <see cref="LumpInfo"/> object containing information about the lump.</returns>
		/// <exception cref="IndexOutOfRangeException">"<paramref name="index"/>" is less than zero, or greater than the number of lumps allowed by "<paramref name="version"/>".</exception>
		public LumpInfo GetLumpInfo(int index, MapType version) {
			if (index < 0 || index >= BSP.GetNumLumps(version)) {
				throw new IndexOutOfRangeException();
			}

			switch (version) {
				case MapType.Quake:
				case MapType.Nightfire: {
					return GetLumpInfoAtOffset(4 + (8 * index), version);
				}
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Quake3:
				case MapType.Raven:
				case MapType.CoD:
				case MapType.CoD2: {
					return GetLumpInfoAtOffset(8 + (8 * index), version);
				}
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.FAKK: {
					return GetLumpInfoAtOffset(12 + (8 * index), version);
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
				case MapType.Vindictus:
				case MapType.DMoMaM: {
					if (lumpFiles == null) {
						LoadLumpFiles();
					}
					if (lumpFiles.ContainsKey(index)) { return lumpFiles[index]; }
					return GetLumpInfoAtOffset(8 + (16 * index), version);
				}
				case MapType.Titanfall: {
					if (lumpFiles == null) {
						LoadLumpFiles();
					}
					if (lumpFiles.ContainsKey(index)) { return lumpFiles[index]; }
					return GetLumpInfoAtOffset((16 * (index + 1)), version);
				}
				case MapType.CoD4: {
					using (FileStream stream = new FileStream(bspFile.FullName, FileMode.Open, FileAccess.Read)) {
						BinaryReader binaryReader = new BinaryReader(stream);
						stream.Seek(8, SeekOrigin.Begin);
						int numlumps = binaryReader.ReadInt32();
						int offset = (numlumps * 8) + 12;
						for (int i = 0; i < numlumps; i++) {
							int id = binaryReader.ReadInt32();
							int length = binaryReader.ReadInt32();
							if (id == index) {
								return new LumpInfo() {
									offset = offset,
									length = length
								};
							} else {
								offset += length;
								while (offset % 4 != 0) { offset++; }
							}
						}
						binaryReader.Close();
					}
					return default(LumpInfo);
				}
				default: {
					return default(LumpInfo);
				}
			}
		}

		/// <summary>
		/// Gets the lump information at offset "<paramref name="offset"/>" for this BSP file when reading it as "<paramref name="version"/>".
		/// </summary>
		/// <param name="offset">The offset of the lump's information.</param>
		/// <param name="version">The type of BSP to interpret the file as.</param>
		/// <returns>A <see cref="LumpInfo"/> object containing information about the lump.</returns>
		private LumpInfo GetLumpInfoAtOffset(int offset, MapType version) {
			if (bspFile.Length < offset + 16) {
				return default(LumpInfo);
			}
			byte[] input;
			using (FileStream stream = new FileStream(bspFile.FullName, FileMode.Open, FileAccess.Read)) {
				BinaryReader binaryReader = new BinaryReader(stream);
				stream.Seek(offset, SeekOrigin.Begin);
				input = binaryReader.ReadBytes(16);
				binaryReader.Close();
			}
			if (version == MapType.TacticalInterventionEncrypted) {
				input = XorWithKeyStartingAtIndex(input, offset);
			}

			int lumpOffset = 0;
			int lumpLength = 0;
			int lumpVersion = 0;
			int lumpIdent = 0;
			if (version == MapType.L4D2 || version == MapType.Source27) {
				lumpVersion = BitConverter.ToInt32(input, 0);
				lumpOffset = BitConverter.ToInt32(input, 4);
				lumpLength = BitConverter.ToInt32(input, 8);
				lumpIdent = BitConverter.ToInt32(input, 12);
				// TODO: This is awful. Let's rework the enum to have internal ways to check engine forks.
			} else if (version == MapType.Source17 ||
						 version == MapType.Source18 ||
						 version == MapType.Source19 ||
						 version == MapType.Source20 ||
						 version == MapType.Source21 ||
						 version == MapType.Source22 ||
						 version == MapType.Source23 ||
						 version == MapType.Vindictus ||
						 version == MapType.DMoMaM ||
						 version == MapType.TacticalInterventionEncrypted ||
						 version == MapType.Titanfall) {
				lumpOffset = BitConverter.ToInt32(input, 0);
				lumpLength = BitConverter.ToInt32(input, 4);
				lumpVersion = BitConverter.ToInt32(input, 8);
				lumpIdent = BitConverter.ToInt32(input, 12);
			} else if (version == MapType.CoD || version == MapType.CoD2) {
				lumpLength = BitConverter.ToInt32(input, 0);
				lumpOffset = BitConverter.ToInt32(input, 4);
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

			return new LumpInfo() {
				offset = lumpOffset,
				length = lumpLength,
				version = lumpVersion,
				ident = lumpIdent
			};
		}

		/// <summary>
		/// Reads the lump in the BSP file using the information in "<paramref name="info"/>".
		/// </summary>
		/// <param name="info">The <see cref="LumpInfo"/> object representing the lump's information.</param>
		/// <returns>A <c>byte</c> array containing the data from the file for the lump at the offset with the length from "<paramref name="info"/>".</returns>
		public byte[] ReadLump(LumpInfo info) {
			if (info.length == 0) { return new byte[0]; }
			byte[] output;

			if (info.lumpFile != null) {
				using (FileStream fs = new FileStream(info.lumpFile.FullName, FileMode.Open, FileAccess.Read)) {
					BinaryReader br = new BinaryReader(fs);
					fs.Seek(info.offset, SeekOrigin.Begin);
					output = br.ReadBytes(info.length);
					br.Close();
					return output;
				}
			}

			using (FileStream stream = new FileStream(bspFile.FullName, FileMode.Open, FileAccess.Read)) {
				BinaryReader binaryReader = new BinaryReader(stream);
				stream.Seek(info.offset, SeekOrigin.Begin);
				output = binaryReader.ReadBytes(info.length);
				binaryReader.Close();
			}

			if (key.Length != 0) {
				output = XorWithKeyStartingAtIndex(output, info.offset);
			}

			return output;
		}

		/// <summary>
		/// Loads any lump files associated with the BSP.
		/// </summary>
		private void LoadLumpFiles() {
			lumpFiles = new Dictionary<int, LumpInfo>();
			// Scan the BSP's directory for lump files
			DirectoryInfo dir = bspFile.Directory;
			List<FileInfo> files = dir.GetFiles(bspFile.Name.Substring(0, bspFile.Name.Length - 4) + "_?_*.lmp").ToList();
			// Sort the list by the number on the file
			files.Sort((f1, f2) => {
				int startIndex = bspFile.Name.Length - 1;
				int f1EndIndex = f1.Name.LastIndexOf('.');
				int f2EndIndex = f2.Name.LastIndexOf('.');
				int f1Position = int.Parse(f1.Name.Substring(startIndex, f1EndIndex - startIndex));
				int f2Position = int.Parse(f2.Name.Substring(startIndex, f2EndIndex - startIndex));
				return f1Position - f2Position;
			});
			// Read the files in order. The last file in the list for a specific lump will replace that lump.
			foreach (FileInfo file in files) {
				using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read)) {
					BinaryReader br = new BinaryReader(fs);
					fs.Seek(0, SeekOrigin.Begin);
					byte[] input = br.ReadBytes(20);
					int offset = BitConverter.ToInt32(input, 0);
					int lumpIndex = BitConverter.ToInt32(input, 4);
					int version = BitConverter.ToInt32(input, 8);
					int length = BitConverter.ToInt32(input, 12);
					lumpFiles[lumpIndex] = new LumpInfo() {
						offset = offset,
						version = version,
						length = length,
						lumpFile = file
					};
					br.Close();
				}
			}
		}

		/// <summary>
		/// Xors the <paramref name="data"/> <c>byte</c> array with the locally stored key <c>byte</c> array, starting at a certain <paramref name="index"/> in the key.
		/// </summary>
		/// <param name="data">The byte array to Xor.</param>
		/// <param name="index">The index in the key byte array to start reading from.</param>
		/// <returns>The input <c>byte</c> array Xored with the key <c>byte</c> array.</returns>
		/// <exception cref="ArgumentNullException">The passed <paramref name="data"/> parameter was <c>null</c>.</exception>
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
		/// Tries to get the <see cref="MapType"/> member most closely represented by the referenced file. If the file is 
		/// found to be big-endian, this will set <see cref="BSPReader.bigEndian"/> to <c>true</c>.
		/// </summary>
		/// <returns>The <see cref="MapType"/> of this BSP, <see cref="MapType.Undefined"/> if it could not be determined.</returns>
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
		/// Tries to get the <see cref="MapType"/> member most closely represented by the referenced file.
		/// </summary>
		/// <param name="bigEndian">Set to <c>true</c> to attempt reading the data in big-endian byte order.</param>
		/// <returns>The <see cref="MapType"/> of this BSP, <see cref="MapType.Undefined"/> if it could not be determined.</returns>
		private MapType GetVersion(bool bigEndian) {
			MapType current = MapType.Undefined;
			using (FileStream stream = new FileStream(bspFile.FullName, FileMode.Open, FileAccess.Read)) {
				if (stream.Length < 4) {
					return current;
				}
				BinaryReader binaryReader = new BinaryReader(stream);
				stream.Seek(0, SeekOrigin.Begin);
				int num = binaryReader.ReadInt32();
				if (bigEndian) {
					byte[] bytes = BitConverter.GetBytes(num);
					Array.Reverse(bytes);
					num = BitConverter.ToInt32(bytes, 0);
				}
				switch (num) {
					case 1347633737: {
						// 1347633737 reads in ASCII as "IBSP"
						// Versions: CoD, CoD2, CoD4, Quake 2, Daikatana, Quake 3 (RtCW), Soldier of Fortune
						int num2 = binaryReader.ReadInt32();
						if (bigEndian) {
							byte[] bytes = BitConverter.GetBytes(num2);
							Array.Reverse(bytes);
							num2 = BitConverter.ToInt32(bytes, 0);
						}
						switch (num2) {
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
						break;
					}
					case 892416050:
					case 1095516485: {
						// 892416050 reads in ASCII as "2015," the game studio which developed MoHAA
						// 1095516485 reads in ASCII as "EALA," the ones who developed MoHAA Spearhead and Breakthrough
						current = MapType.MOHAA;
						break;
					}
					case 1347633750: {
						// 1347633750 reads in ASCII as "VBSP." Indicates Source engine.
						// Some source games handle this as 2 shorts.
						// TODO: Big endian?
						// Formats: Source 17-23 and 27, DMoMaM, Vindictus
						int num2 = (int)binaryReader.ReadUInt16();
						switch (num2) {
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
									current = MapType.Source20;
									// Hack for detecting Vindictus: Look in the GameLump for offset/length/flags outside of ranges we'd expect
									LumpInfo gameLumpInfo = GetLumpInfo(35, MapType.Source20);
									stream.Seek(gameLumpInfo.offset, SeekOrigin.Begin);
									int numGameLumps = binaryReader.ReadInt32();
									if (numGameLumps > 0) {
										// Normally this would be the offset and length for the first game lump.
										// But in Vindictus it's the version indicator for it instead.
										stream.Seek(gameLumpInfo.offset + 12, SeekOrigin.Begin);
										int testOffset = binaryReader.ReadInt32();
										if (numGameLumps > 1) {
											if (testOffset < 24) {
												current = MapType.Vindictus;
												break;
											}
										} else {
											// Normally this would be the ident for the second game lump.
											// But in Vindictus it's the length for the first instead.
											stream.Seek(gameLumpInfo.offset + 20, SeekOrigin.Begin);
											int testName = binaryReader.ReadInt32();
											// A lump ident tends to have a value far above 1090519040, longer than any GameLump should be.
											if (testOffset < 24 && testName < gameLumpInfo.length) {
												current = MapType.Vindictus;
												break;
											}
										}
									}
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
						break;
					}
					case 1347633746: {
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
						break;
					}
					case 556942917: {
						// "EF2!"
						current = MapType.STEF2;
						break;
					}
					case 1347633778: {
						// "rBSP". Respawn's format for Titanfall
						current = MapType.Titanfall;
						break;
					}
					case 1263223110: {
						// "FAKK"
						// Formats: STEF2 demo, Heavy Metal FAKK2 (American McGee's Alice)
						int num2 = binaryReader.ReadInt32();
						if (bigEndian) {
							byte[] bytes = BitConverter.GetBytes(num2);
							Array.Reverse(bytes);
							num2 = BitConverter.ToInt32(bytes, 0);
						}
						switch (num2) {
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
						break;
					}
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
						int num2 = BitConverter.ToInt32(XorWithKeyStartingAtIndex(binaryReader.ReadBytes(4)), 0);
						if (num2 == 1347633750) {
							current = MapType.TacticalInterventionEncrypted;
						} else {
							current = MapType.Undefined;
						}
						break;
					}
				}
				binaryReader.Close();
			}
			return current;
		}
	}
}
