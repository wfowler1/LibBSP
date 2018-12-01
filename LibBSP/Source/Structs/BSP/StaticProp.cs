#if UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Text;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Color = Color32;
	using Vector3d = Vector3;
#else
	using Color = System.Drawing.Color;
#endif

	/// <summary>
	/// Handles the data needed for a static prop object.
	/// </summary>
	public struct StaticProp {

		public byte[] data;
		public MapType type;
		public int version;

		public Vector3d origin {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								return new Vector3d(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
							}
							default: {
								return new Vector3d(float.NaN, float.NaN, float.NaN);
							}
						}
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								value.GetBytes().CopyTo(data, 0);
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public Vector3d angles {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								return new Vector3d(BitConverter.ToSingle(data, 12), BitConverter.ToSingle(data, 16), BitConverter.ToSingle(data, 20));
							}
							default: {
								return new Vector3d(float.NaN, float.NaN, float.NaN);
							}
						}
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								value.GetBytes().CopyTo(data, 12);
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public short dictionaryEntry {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								return BitConverter.ToInt16(data, 24);
							}
							default: {
								return -1;
							}
						}
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 24);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public short firstLeaf {
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11: {
								return BitConverter.ToInt16(data, 26);
							}
							default: {
								return -1;
							}
						}
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11: {
								bytes.CopyTo(data, 26);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public short numLeafs {
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11: {
								return BitConverter.ToInt16(data, 28);
							}
							default: {
								return -1;
							}
						}
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11: {
								bytes.CopyTo(data, 28);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public byte solidity {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								return data[30];
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								data[30] = value;
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public byte flags {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								return data[31];
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12: {
								data[31] = value;
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public int skin {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return BitConverter.ToInt32(data, 32);
							}
							case 9: {
								if (data.Length == 76) {
									return BitConverter.ToInt32(data, 36);
								} else {
									return BitConverter.ToInt32(data, 32);
								}
							}
							default: {
								return -1;
							}
						}
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 32);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									bytes.CopyTo(data, 36);
								} else {
									bytes.CopyTo(data, 32);
								}
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public float minFadeDist {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return BitConverter.ToSingle(data, 36);
							}
							case 9: {
								if (data.Length == 76) {
									return BitConverter.ToSingle(data, 40);
								} else {
									return BitConverter.ToSingle(data, 36);
								}
							}
							default: {
								return float.NaN;
							}
						}
					}
					default: {
						return float.NaN;
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 36);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									bytes.CopyTo(data, 40);
								} else {
									bytes.CopyTo(data, 36);
								}
								break;
							}
						}
						break;
					}
				}
			}
		}
		
		public float maxFadeDist {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return BitConverter.ToSingle(data, 40);
							}
							case 9: {
								if (data.Length == 76) {
									return BitConverter.ToInt32(data, 44);
								} else {
									return BitConverter.ToInt32(data, 40);
								}
							}
							default: {
								return float.NaN;
							}
						}
					}
					default: {
						return float.NaN;
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 40);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									bytes.CopyTo(data, 36);
								} else {
									bytes.CopyTo(data, 32);
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public Vector3d lightingOrigin {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return new Vector3d(BitConverter.ToSingle(data, 44), BitConverter.ToSingle(data, 48), BitConverter.ToSingle(data, 52));
							}
							case 9: {
								if (data.Length == 76) {
									return new Vector3d(BitConverter.ToSingle(data, 48), BitConverter.ToSingle(data, 52), BitConverter.ToSingle(data, 56));
								} else {
									return new Vector3d(BitConverter.ToSingle(data, 44), BitConverter.ToSingle(data, 48), BitConverter.ToSingle(data, 52));
								}
							}
							default: {
								return new Vector3d(float.NaN, float.NaN, float.NaN);
							}
						}
					}
					default: {
						return new Vector3d(float.NaN, float.NaN, float.NaN);
					}
				}
			}
			set {
				byte[] bytes = value.GetBytes();
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 44);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									value.GetBytes().CopyTo(data, 48);
								} else {
									value.GetBytes().CopyTo(data, 44);
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public float forcedFadeScale {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return BitConverter.ToSingle(data, 56);
							}
							case 9: {
								if (data.Length == 76) {
									return BitConverter.ToSingle(data, 60);
								} else {
									return BitConverter.ToSingle(data, 56);
								}
							}
							default: {
								return float.NaN;
							}
						}
					}
					default: {
						return float.NaN;
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 5:
							case 6:
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								bytes.CopyTo(data, 56);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									bytes.CopyTo(data, 60);
								} else {
									bytes.CopyTo(data, 56);
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public short minDXLevel {
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
					case MapType.TacticalInterventionEncrypted: {
						switch (version) {
							case 6:
							case 7: {
								return BitConverter.ToInt16(data, 60);
							}
							default: {
								return -1;
							}
						}
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
					case MapType.TacticalInterventionEncrypted: {
						switch (version) {
							case 6:
							case 7: {
								bytes.CopyTo(data, 60);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public short maxDXLevel {
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
					case MapType.TacticalInterventionEncrypted: {
						switch (version) {
							case 6:
							case 7: {
								return BitConverter.ToInt16(data, 62);
							}
							default: {
								return -1;
							}
						}
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
					case MapType.TacticalInterventionEncrypted: {
						switch (version) {
							case 6:
							case 7: {
								bytes.CopyTo(data, 62);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public byte minCPULevel {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								return data[60];
							}
							case 9: {
								if (data.Length == 76) {
									return data[64];
								} else {
									return data[60];
								}
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								data[60] = value;
								break;
							}
							case 9: {
								if (data.Length == 76) {
									data[64] = value;
								} else {
									data[60] = value;
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public byte maxCPULevel {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								return data[61];
							}
							case 9: {
								if (data.Length == 76) {
									return data[65];
								} else {
									return data[61];
								}
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								data[61] = value;
								break;
							}
							case 9: {
								if (data.Length == 76) {
									data[65] = value;
								} else {
									data[61] = value;
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public byte minGPULevel {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								return data[62];
							}
							case 9: {
								if (data.Length == 76) {
									return data[66];
								} else {
									return data[62];
								}
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								data[62] = value;
								break;
							}
							case 9: {
								if (data.Length == 76) {
									data[66] = value;
								} else {
									data[62] = value;
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public byte maxGPULevel {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								return data[63];
							}
							case 9: {
								if (data.Length == 76) {
									return data[67];
								} else {
									return data[63];
								}
							}
							default: {
								return 0;
							}
						}
					}
					default: {
						return 0;
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 8:
							case 10:
							case 11:
							case 12: {
								data[63] = value;
								break;
							}
							case 9: {
								if (data.Length == 76) {
									data[67] = value;
								} else {
									data[63] = value;
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		public Color diffuseModulaton {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								return ColorExtensions.FromArgb(data[67], data[64], data[65], data[66]);
							}
							case 9: {
								if (data.Length == 76) {
									return ColorExtensions.FromArgb(data[72], data[69], data[70], data[71]);
								} else {
									return ColorExtensions.FromArgb(data[67], data[64], data[65], data[66]);
								}
							}
							default: {
								return ColorExtensions.FromArgb(255, 255, 255, 255);
							}
						}
					}
					default: {
						return ColorExtensions.FromArgb(255, 255, 255, 255);
					}
				}
			}
			set {
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
					case MapType.Vindictus:
					case MapType.DMoMaM:
					case MapType.Titanfall: {
						switch (version) {
							case 7:
							case 8:
							case 10:
							case 11:
							case 12: {
								value.GetBytes().CopyTo(data, 64);
								break;
							}
							case 9: {
								if (data.Length == 76) {
									value.GetBytes().CopyTo(data, 69);
								} else {
									value.GetBytes().CopyTo(data, 64);
								}
									break;
							}
						}
						break;
					}
				}
			}
		}

		public float scale {
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 11: {
								return BitConverter.ToSingle(data, 76);
							}
							default: {
								return float.NaN;
							}
						}
					}
					default: {
						return float.NaN;
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
					case MapType.Vindictus:
					case MapType.DMoMaM: {
						switch (version) {
							case 11: {
								bytes.CopyTo(data, 76);
								break;
							}
						}
						break;
					}
				}
			}
		}

		public string targetname {
			get {
				switch (type) {
					case MapType.Source20: {
						switch (version) {
							case 5:
							case 6: {
								if (data.Length > 128) {
									return data.ToNullTerminatedString(data.Length - 128, 128);
								}
								return null;
							}
							default: {
								return null;
							}
						}
					}
					default: {
						return null;
					}
				}
			}
			set {
				switch (type) {
					case MapType.Source20: {
						switch (version) {
							case 5:
							case 6: {
								if (data.Length > 128) {
									for (int i = 0; i < 128; ++i) {
										data[data.Length - i - 1] = 0;
									}
									byte[] strBytes = Encoding.ASCII.GetBytes(value);
									Array.Copy(strBytes, 0, data, data.Length - 128, Math.Min(strBytes.Length, 127));
								}
								break;
							}
						}
						break;
					}
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="StaticProp"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of static prop lump this object is a member of.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was <c>null</c>.</exception>
		public StaticProp(byte[] data, MapType type, int version = 0) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			this.data = data;
			this.type = type;
			this.version = version;
		}

		/// <summary>
		/// Factory method to create a <see cref="StaticProps"/> object.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of the Static Prop lump.</param>
		/// <returns>A <see cref="StaticProps"/> object.</returns>
		public static StaticProps LumpFactory(byte[] data, MapType type, int version = 0) {
			return new StaticProps(data, type, version);
		}

	}
}
