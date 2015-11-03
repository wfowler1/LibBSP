//#if (UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5)

using System;
using System.Reflection;

namespace LibBSP {
	/// <summary>
	/// Contains static methods for retrieving custom attributes.
	/// </summary>
	public static class CustomAttributeExtensions {
		
		/// <summary>
		/// Retrieves a custom attribute of a specified type that is applied to a specified member.
		/// </summary>
		/// <param name="element">The member to inspect.</param>
		/// <returns>A custom attribute that matches T, or null if no such attribute is found.</returns>
		/// <typeparam name="T">The type of attribute to search for.</typeparam>
		public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute {
			return Attribute.GetCustomAttribute(element, typeof(T)) as T;
		}

	}
}

//#endif
