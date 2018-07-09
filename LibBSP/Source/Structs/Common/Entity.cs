#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if !UNITY
	using Vector3 = Vector3d;
	using Vector4 = Vector4d;
#endif
	/// <summary>
	/// Class containing all data for a single <see cref="Entity"/>, including attributes, Source Entity I/O connections and solids.
	/// </summary>
	[Serializable] public class Entity : Dictionary<string, string>, IComparable, IComparable<Entity>, ISerializable {

		private static IFormatProvider _format = CultureInfo.CreateSpecificCulture("en-US");

		public const char ConnectionMemberSeparater = (char)0x1B;

		public List<EntityConnection> connections = new List<EntityConnection>();
		public List<MAPBrush> brushes = new List<MAPBrush>();

		/// <summary>
		/// Gets whether this <see cref="Entity"/> is brush-based or not.
		/// </summary>
		public bool brushBased { get { return brushes.Count > 0 || modelNumber >= 0; } }

		/// <summary>
		/// Wrapper for the "spawnflags" attribute.
		/// </summary>
		public uint spawnflags {
			get {
				try {
					if (ContainsKey("spawnflags")) {
						return System.UInt32.Parse(this["spawnflags"]);
					} else {
						return 0;
					}
				} catch {
					return 0;
				}
			}
			set { this["spawnflags"] = value.ToString(); }
		}

		/// <summary>
		/// Wrapper for the "origin" attribute.
		/// </summary>
		public Vector3 origin {
			get { return GetVector("origin"); }
			set { this["origin"] = value.x + " " + value.y + " " + value.z; }
		}

		/// <summary>
		/// Wrapper for the "angles" attribute.
		/// </summary>
		public Vector3 angles {
			get { return GetVector("angles"); }
			set { this["angles"] = value.x + " " + value.y + " " + value.z; }
		}

		/// <summary>
		/// Wrapper for the "targetname" attribute.
		/// </summary>
		public string name {
			get {
				if (ContainsKey("targetname")) {
					return this["targetname"];
				} else if (ContainsKey("name")) {
					return this["name"];
				} else {
					return "";
				}
			}
			set { this["targetname"] = value; }
		}

		/// <summary>
		/// Wrapper for the "classname" attribute.
		/// </summary>
		/// <remarks>If an entity has no class, it has no behavior. It's either an error or metadata.</remarks>
		public string className {
			get {
				if (ContainsKey("classname")) {
					return this["classname"];
				} else {
					return "";
				}
			}
			set { this["classname"] = value; }
		}

		/// <summary>
		/// If there's a model number in the attributes list, this method fetches it
		/// and returns it. If there is no model defined, or it's not a numerical 
		/// value, then -1 is returned. If it's the worldspawn then a 0 is returned.
		/// </summary>
		public int modelNumber {
			get {
				try {
					if (this["classname"] == "worldspawn") {
						return 0;
					} else {
						if (ContainsKey("model")) {
							string st = this["model"];
							if (st[0] == '*') {
								int ret = -1;
								if (Int32.TryParse(st.Substring(1), out ret)) {
									return ret;
								} else {
									return -1;
								}
							} else {
								return -1;
							}
						} else {
							return -1;
						}
					}
				} catch {
					return -1;
				}
			}
		}

		/// <summary>
		/// Allows an attribute to be accessed easily using <see cref="Entity"/>["<paramref name="key"/>"] notation.
		/// If an attribute doesn't exist, it returns an empty <c>string</c>. This emulates the behavior of game engines.
		/// </summary>
		/// <remarks>
		/// It's up to the developer to ensure the empty string doesn't cause problems, because this won't return <c>null</c>.
		/// </remarks>
		/// <param name="key">The attribute to retrieve.</param>
		/// <returns>The value of the attribute if it exists, empty <c>string</c> otherwise.</returns>
		public new string this[string key] {
			get {
				if (ContainsKey(key)) {
					return base[key];
				} else {
					return "";
				}
			}
			set { base[key] = value; }
		}

		/// <summary>
		/// Initializes a new instance of an <see cref="Entity"/>, parsing the given <c>byte</c> array into an <see cref="Entity"/> structure.
		/// </summary>
		/// <param name="data">Array to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		public Entity(byte[] data, MapType type, int version = 0) : this(Encoding.ASCII.GetString(data).Split('\n')) { }

		/// <summary>
		/// Initializes a new instance of an <see cref="Entity"/> with the given classname.
		/// </summary>
		/// <param name="className">Classname of the new <see cref="Entity"/>.</param>
		public Entity(string className) : base(StringComparer.InvariantCultureIgnoreCase) {
			Add("classname", className);
		}

		/// <summary>
		/// Initializes a new instance of an <see cref="Entity"/> object with no initial properties.
		/// </summary>
		public Entity() : base(StringComparer.InvariantCultureIgnoreCase) { }

		/// <summary>
		/// Initializes a new instance of an <see cref="Entity"/> object, copying the attributes, connections and brushes of the passed <see cref="Entity"/>.
		/// </summary>
		/// <param name="copy">The <see cref="Entity"/> to copy.</param>
		public Entity(Entity copy) : base(copy, StringComparer.InvariantCultureIgnoreCase) {
			connections = new List<EntityConnection>(copy.connections);
			brushes = new List<MAPBrush>(copy.brushes);
		}

		/// <summary>
		/// Initializes a new instance of an <see cref="Entity"/>, parsing the given <c>string</c> array into an <see cref="Entity"/> structure.
		/// </summary>
		/// <param name="lines">Array of <c>string</c>s representing attributes, patches, brushes, displacements etc. to parse.</param>
		public Entity(string[] lines) : base(StringComparer.InvariantCultureIgnoreCase) {
			int braceCount = 0;

			bool inConnections = false;
			bool inBrush = false;

			List<string> child = new List<string>();

			foreach (string line in lines) {
				string current = line.Trim(' ', '\t', '\r');

				// Cull everything after a //
				bool inQuotes = false;

				for (int i = 0; i < current.Length; ++i) {
					if (current[i] == '\"' && (i == 0 || current[i - 1] != '\\')) {
						inQuotes = !inQuotes;
					}

					if (!inQuotes && current[i] == '/' && i != 0 && current[i - 1] == '/') {
						current = current.Substring(0, i - 1);
					}
				}

				if (string.IsNullOrEmpty(current)) {
					continue;
				}

				// Perhaps I should not assume these will always be the first thing on the line
				if (current[0] == '{') {
					// If we're only one brace deep, and we have no prior information, assume a brush
					if (braceCount == 1 && !inBrush && !inConnections) {
						inBrush = true;
					}
					++braceCount;
				} else if (current[0] == '}') {
					--braceCount;
					// If this is the end of an entity substructure
					if (braceCount == 1) {
						// If we determined we were inside a brush substructure
						if (inBrush) {
							child.Add(current);
							brushes.Add(new MAPBrush(child.ToArray()));
							child = new List<string>();
						}
						inBrush = false;
						inConnections = false;
					} else {
						child.Add(current);
					}
					continue;
				} else if (current.Length >= 5 && current.Substring(0, 5) == "solid") {
					inBrush = true;
					continue;
				} else if (current.Length >= 11 && current.Substring(0, 11) == "connections") {
					inConnections = true;
					continue;
				}

				if (inBrush) {
					child.Add(current);
					continue;
				}

				Add(current);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Entity"/> class with serialized data.
		/// </summary>
		/// <param name="info">A <c>SerializationInfo</c> object containing the information required to serialize the <see cref="Entity"/>.</param>
		/// <param name="context">A <c>StreamingContext</c> structure containing the source and destination of the serialized stream associated with the <see cref="Entity"/>.</param>
		protected Entity(SerializationInfo info, StreamingContext context) : base(info, context) {
			connections = (List<EntityConnection>)info.GetValue("connections", typeof(List<EntityConnection>));
			brushes = (List<MAPBrush>)info.GetValue("brushes", typeof(List<MAPBrush>));
		}

		/// <summary>
		/// Factory method to create an <see cref="Entity"/> from a <c>string</c> "<paramref name="st"/>" where
		/// "<paramref name="st"/>" contains all lines for the entity, including attributes, brushes, etc.
		/// </summary>
		/// <remarks>
		/// This was necessary since the <see cref="Entity(System.String)"/> constructor was already used in a different way.
		/// </remarks>
		/// <param name="st">The data to parse.</param>
		/// <returns>The resulting <see cref="Entity"/> object.</returns>
		public static Entity FromString(string st) {
			return new Entity(st.Split('\n'));
		}

		/// <summary>
		/// Renames the attribute named "<paramref name="oldName"/>" to "<paramref name="newName"/>". Replaces the old entry if it already exists.
		/// </summary>
		/// <param name="oldName">Attribute to be renamed.</param>
		/// <param name="newName">New name for this attribute.</param>
		public void RenameKey(string oldName, string newName) {
			if (ContainsKey(oldName)) {
				string val = this[oldName];
				Remove(oldName);
				if (ContainsKey(newName)) {
					Remove(newName);
				}
				Add(newName, val);
			}
		}

		/// <summary>
		/// Parses the input <c>string</c> "<paramref name="st"/>" into a key/value pair and adds
		/// it as an attribute to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="st">The <c>string</c> to be parsed.</param>
		public void Add(string st) {
			string key = "";
			string val = "";
			bool inQuotes = false;
			bool isVal = false;
			int numCommas = 0;
			for (int i = 0; i < st.Length; ++i) {
				// Some entity values in Source can use escape sequenced quotes. Need to make sure not to parse those.
				if (st[i] == '\"' && (i == 0 || st[i - 1] != '\\')) {
					if (inQuotes) {
						if (isVal) {
							break;
						}
						isVal = true;
					}
					inQuotes = !inQuotes;
				} else {
					if (inQuotes) {
						if (!isVal) {
							key += st[i];
						} else {
							val += st[i];
							if (st[i] == ',' || st[i] == ConnectionMemberSeparater) { ++numCommas; }
						}
					}
				}
			}
			val.Replace("\\\"", "\"");
			if (key != null && key != "") {
				if (numCommas == 4 || numCommas == 6) {
					st = st.Replace(',', ConnectionMemberSeparater);
					string[] connection = val.Split(',');
					if (connection.Length < 5) {
						connection = val.Split((char)0x1B);
					}
					if (connection.Length == 5 || connection.Length == 7) {
						connections.Add(new EntityConnection {
							name = key,
							target = connection[0],
							action = connection[1],
							param = connection[2],
							delay = Double.Parse(connection[3], _format),
							fireOnce = Int32.Parse(connection[4]),
							unknown0 = connection.Length > 5 ? connection[5] : "",
							unknown1 = connection.Length > 6 ? connection[6] : "",
						});
					}
				} else {
					if (!ContainsKey(key)) {
						this[key] = val;
					}
				}
			}
		}

		/// <summary>
		/// Gets a <c>string</c> representation of this <see cref="Entity"/>.
		/// </summary>
		/// <returns><c>string</c> representation of this <see cref="Entity"/>.</returns>
		public override string ToString() {
			StringBuilder output = new StringBuilder();
			output.Append("{\n");
			foreach (KeyValuePair<string, string> pair in this) {
				output.Append(string.Format("\"{0}\" \"{1}\"\n", pair.Key, pair.Value));
			}
			if (connections.Count > 0) {
				output.Append("connections\n{\n");
				foreach (EntityConnection c in connections) {
					output.Append(string.Format("\"{0}\" \"{1},{2},{3},{4},{5},{6},{7}\"\n", c.name, c.target, c.action, c.param, c.delay.ToString(_format), c.fireOnce, c.unknown0, c.unknown1));
				}
				output.Append("}\n");
			}
			return output + "}";
		}

		/// <summary>
		/// Checks if the attribute named "<paramref name="key"/>" has the value "<paramref name="value"/>".
		/// </summary>
		/// <param name="key">The attribute to check.</param>
		/// <param name="value">The value to compare.</param>
		/// <returns><c>true</c> if the values match.</returns>
		public bool ValueIs(string key, string value) {
			return value.Equals(this[key], StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Checks if the bits in "spawnflags" corresponding to the set bits set in <paramref name="bits"/> are set.
		/// </summary>
		/// <param name="bits">The bits to compare spawnflags to.</param>
		/// <returns><c>true</c> if all bits that were set in <paramref name="bits"/> were set in spawnflags.</returns>
		public bool SpawnflagsSet(uint bits) {
			return ((spawnflags & bits) == bits);
		}

		/// <summary>
		/// Toggles the bits in "spawnflags" which are set in <paramref name="bits"/>.
		/// </summary>
		/// <param name="bits">Bitmask of bits to toggle.</param>
		public void ToggleSpawnflags(uint bits) {
			this["spawnflags"] = (spawnflags ^ bits).ToString();
		}

		/// <summary>
		/// Clears the bits in "spawnflags" which are set in <paramref name="bits"/>.
		/// Equivalent to spawnflags = (<paramref name="bits"/> ^ 0xFFFFFFFF) & spawnflags.
		/// </summary>
		/// <param name="bits">Bitmask of bits to clear.</param>
		public void ClearSpawnflags(uint bits) {
			ToggleSpawnflags(spawnflags & bits);
		}

		/// <summary>
		/// Sets the bits in "spawnflags" which are set in <paramref name="bits"/>.
		/// </summary>
		/// <param name="bits">Bitmask of bits to set.</param>
		public void SetSpawnflags(uint bits) {
			this["spawnflags"] = (spawnflags | bits).ToString();
		}

		/// <summary>
		/// Gets a numeric attribute as a <c>float</c>. Throws if the attribute could not be converted to a numerical value
		/// and no <paramref name="failDefault"/> was provided.
		/// </summary>
		/// <param name="key">Name of the attribute to retrieve.</param>
		/// <param name="failDefault">Value to return if <paramref name="key"/> doesn't exist, or couldn't be converted.</param>
		/// <returns>The numeric value of the value corresponding to <paramref name="key"/>.</returns>
		public float GetFloat(string key, float? failDefault = null) {
			try {
				return Single.Parse(this[key], _format);
			} catch (Exception e) {
				if (!failDefault.HasValue) {
					throw e;
				}
				return failDefault.Value;
			}
		}

		/// <summary>
		/// Gets a numeric attribute as an <c>int</c>. Throws if the attribute could not be converted to a numerical value
		/// and no <paramref name="failDefault"/> was provided.
		/// </summary>
		/// <param name="key">Name of the attribute to retrieve.</param>
		/// <param name="failDefault">Value to return if <paramref name="key"/> doesn't exist, or couldn't be converted.</param>
		/// <returns>The numeric value of the value corresponding to <paramref name="key"/>.</returns>
		public int GetInt(string key, int? failDefault = null) {
			try {
				return Int32.Parse(this[key], _format);
			} catch (Exception e) {
				if (!failDefault.HasValue) {
					throw e;
				}
				return failDefault.Value;
			}
		}

		/// <summary>
		/// Gets a Vector attribute as a <see cref="Vector4"/>. This will only read as many values as are in the attribute.
		/// </summary>
		/// <param name="key">Name of the attribute to retrieve.</param>
		/// <returns>Vector representation of the components of the attribute.</returns>
		public Vector4 GetVector(string key) {
			float[] results = new float[4];
			if (ContainsKey(key) && !string.IsNullOrEmpty(this[key])) {
				string[] nums = this[key].Split(' ');
				for (int i = 0; i < results.Length && i < nums.Length; ++i) {
					try {
						results[i] = Single.Parse(nums[i], _format);
					} catch {
						results[i] = 0;
					}
				}
			}
			return new Vector4(results[0], results[1], results[2], results[3]);
		}

		#region IComparable
		/// <summary>
		/// Compares this <see cref="Entity"/> to another object. First "classname" attributes are compared, then "targetname".
		/// Attributes are compared alphabetically. Targetnames are only compared if classnames match.
		/// </summary>
		/// <param name="obj"><c>Object</c> to compare to.</param>
		/// <returns>Less than zero if this entity is first, 0 if they occur at the same time, greater than zero otherwise.</returns>
		/// <exception cref="ArgumentException"><paramref name="obj"/> was not of type <see cref="Entity"/>.</exception>
		public int CompareTo(object obj) {
			if (obj == null) { return 1; }
			Entity other = obj as Entity;
			if (other == null) { throw new ArgumentException("Object is not an Entity"); }

			int firstTry = className.CompareTo(other.className);
			return firstTry != 0 ? firstTry : name.CompareTo(other.name);
		}

		/// <summary>
		/// Compares this <see cref="Entity"/> to another <see cref="Entity"/>. First "classname" attributes are compared, then "targetname".
		/// Attributes are compared alphabetically. Targetnames are only compared if classnames match.
		/// </summary>
		/// <param name="other"><see cref="Entity"/> to compare to.</param>
		/// <returns>Less than zero if this entity is first, 0 if they occur at the same time, greater than zero otherwise.</returns>
		public int CompareTo(Entity other) {
			if (other == null) { return 1; }
			int firstTry = className.CompareTo(other.className);
			return firstTry != 0 ? firstTry : name.CompareTo(other.name);
		}
		#endregion

		#region ISerializable
		/// <summary>
		/// Implements the <c>ISerializable</c> interface and returns the data needed
		/// to serialize the <see cref="Entity"> instance.
		/// </summary>
		/// <param name="info">A <c>SerializationInfo</c> object that contains the information required to serialize the <see cref="Entity"> instance.</param>
		/// <param name="context">A <c>StreamingContext</c> structure that contains the source and destination of the serialized stream associated with the <see cref="Entity"/> instance.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			base.GetObjectData(info, context);
			info.AddValue("connections", connections, typeof(List<EntityConnection>));
			info.AddValue("brushes", brushes, typeof(List<MAPBrush>));

		}
		#endregion

		/// <summary>
		/// Factory method for an <see cref="Entities"/> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data">The data to parse.</param>
		/// <param name="type">The map type.</param>
		/// <param name="version">The version of this lump.</param>
		/// <returns>An <see cref="Entities"/> object, which is a <c>List</c> of <see cref="Entity"/>s.</returns>
		public static Entities LumpFactory(byte[] data, MapType type, int version = 0) {
			return new Entities(data, type);
		}

		/// <summary>
		/// Gets the index for this lump in the BSP file for a specific map format.
		/// </summary>
		/// <param name="type">The map type.</param>
		/// <returns>Index for this lump, or -1 if the format doesn't have this lump or it's not implemented.</returns>
		public static int GetIndexForLump(MapType type) {
			switch (type) {
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.Quake:
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.Daikatana:
				case MapType.SoF:
				case MapType.Nightfire:
				case MapType.Vindictus:
				case MapType.TacticalInterventionEncrypted:
				case MapType.L4D2:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.DMoMaM: {
					return 0;
				}
				case MapType.FAKK:
				case MapType.MOHAA: {
					return 14;
				}
				case MapType.STEF2:
				case MapType.STEF2Demo: {
					return 16;
				}
				case MapType.CoD: {
					return 29;
				}
				case MapType.CoD2: {
					return 37;
				}
				case MapType.CoD4: {
					return 39;
				}
				default: {
					return -1;
				}
			}
		}

		/// <summary>
		/// Struct containing the fields necessary for Source entity I/O.
		/// </summary>
		[Serializable] public struct EntityConnection {
			public string name;
			public string target;
			public string action;
			public string param;
			public double delay;
			public int fireOnce;
			// As I recall, these exist in Dark Messiah only. I have no idea what they are for.
			public string unknown0;
			public string unknown1;
		}

	}
}
