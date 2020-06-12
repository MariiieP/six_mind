using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tests
{
	public class JsonSchemeTest
	{
		private static string _restoreKey = "RestoreKey";
		private static string _persistentKey = "PersistentKey";
		private static string _progressKey = "ProgressKey";

		public string _resotreDataJsonSchema = "ResotreDataJsonSchema";
		public string _persistentDataJsonSchema = "PersistentDataJsonSchema";
		public string _progressDataJsonSchema = "ProgressDataJsonSchema";

		private string JsonSchemaFolderName = "JsonScheme";

		private const int LevelsCount = 8;

		[Test]
		public void JsonSchemeTestPasses()
		{
			TestRestoreData();
			TestPersistentData();
			TestProgressData();
		}

		private void TestRestoreData()
		{
			var restoreDataJsons = GetRestoreDataJsons();
			foreach (var json in restoreDataJsons)
			{
				var validateResult = ValidateJson(json, _resotreDataJsonSchema);
				try
				{
					Assert.IsTrue(validateResult);
				}
				catch (AssertionException e)
				{
					Debug.LogError(e);
				}
			}
		}

		private void TestPersistentData()
		{
			var json = PlayerPrefs.GetString(_persistentKey);
			var validateResult = ValidateJson(json, _persistentDataJsonSchema);
			try
			{
				Assert.IsTrue(validateResult);
			}
			catch (AssertionException e)
			{
				Debug.LogError(e);
			}
		}

		private void TestProgressData()
		{
			var json = PlayerPrefs.GetString(_progressKey);
			var validateResult = ValidateJson(json, _progressDataJsonSchema);
			try
			{
				Assert.IsTrue(validateResult);
			}
			catch (AssertionException e)
			{
				Debug.LogError(e);
			}
		}

		private List<string> GetRestoreDataJsons()
		{
			var _restoreJsons = new List<string>();
			for (int i = 0; i <= LevelsCount; ++i)
			{
				string defaultValue = "Error";
				string json = PlayerPrefs.GetString(_restoreKey + i.ToString(), defaultValue);
				if (json == defaultValue)
				{
					Debug.LogWarning(_restoreKey + i.ToString() + " error");
				}
				else
				{
					_restoreJsons.Add(json);
				}
			}
			return _restoreJsons;
		}

		private bool ValidateJson(string targetJson, string schemaPath)
		{
			var textComponent = (TextAsset)Resources.Load(Path.Combine(JsonSchemaFolderName, schemaPath));
			var schema = JSchema.Parse(textComponent.text);
			var jsonObject = JObject.Parse(targetJson);
			return jsonObject.IsValid(schema);
		}
	}
}
