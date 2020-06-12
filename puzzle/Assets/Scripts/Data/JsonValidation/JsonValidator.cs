using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;
using UnityEngine;

namespace Data.JsonValidation
{
	public class JsonValidator
	{
		public static string ResotreDataJsonSchema = "ResotreDataJsonSchema";
		private static string JsonSchemaFolder = "JsonScheme";

		public static bool ValidateJson(string targetJson, string schemaPath)
		{
			var textComponent = (TextAsset)Resources.Load(Path.Combine(JsonSchemaFolder, schemaPath));
			var schema = JSchema.Parse(textComponent.text);
			var jsonObject = JObject.Parse(targetJson);
			var result = jsonObject.IsValid(schema);
			if (!result)
			{
				Debug.LogError($"Json for scheme {ResotreDataJsonSchema} is not valid");
			}
			else
			{
				Debug.Log($"Json for scheme {ResotreDataJsonSchema} is valid");
			}
			return result;
		}
	}
}
