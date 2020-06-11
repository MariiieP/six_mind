using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class PlayerPrefsEditor : EditorWindow
	{
		private string _content;
		private string _key;
		private string _log = string.Empty;

		[MenuItem("Lines/Player Prefs Editor")]
		private static void ShowWindow()
		{
			var window = GetWindow<PlayerPrefsEditor>();
			window.title = "PlayerPrefsEditor";
			window.Show();
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Key:", GUILayout.Width(60));
			_key = EditorGUILayout.DelayedTextField(_key);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Content:", GUILayout.Width(60));
			_content = EditorGUILayout.DelayedTextField(_content);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Log:", GUILayout.Width(60));
			EditorGUILayout.TextArea(_log);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Save"))
			{
				PlayerPrefs.SetString(_key, _content);
				PlayerPrefs.Save();
			}
			if (GUILayout.Button("Load"))
			{
				_log = PlayerPrefs.GetString(_key, "Error");
				PlayerPrefs.Save();
			}
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Clear prefs"))
			{
				PlayerPrefs.DeleteAll();
			}
		}
	}
}
