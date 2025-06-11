using System.ComponentModel;
using System.Dynamic;

namespace Autobarn.Website.Api;

public static class HypermediaExtensions {
	public static dynamic ToDynamic(this object value) {
		IDictionary<string, object> expando = new ExpandoObject();
		var properties = TypeDescriptor.GetProperties(value.GetType());
		foreach (PropertyDescriptor property in properties) {
			if (Ignore(property)) continue;
			expando.Add(property.Name, property.GetValue(value));
		}
		return (ExpandoObject) expando;
	}

	private static bool Ignore(PropertyDescriptor property) {
		return
			property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any()
			||
			property.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any()

			;
	}
}
