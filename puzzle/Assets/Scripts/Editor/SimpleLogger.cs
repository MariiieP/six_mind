using System.Collections;
using UnityEngine;
using System;

namespace Editor
{
	public class SimpleLogger : MonoBehaviour
	{
		public string Output = string.Empty;
		public string Stack = string.Empty;
		public int MaxLines = 30;

		private bool _hidden = true;
		private Vector2 _scrollPos;
		private static string _log;
		private static Queue _logQueue = new Queue();

		[Obsolete]
		private void OnEnable()
		{
			Application.RegisterLogCallback(HandleLog);
		}

		[Obsolete]
		private void OnDisable()
		{
			Application.RegisterLogCallback(null);
		}

		private void HandleLog(string logString, string stackTrace, LogType type)
		{
			Output = logString;
			Stack = stackTrace;
			string newString = "\n [" + type + "] : " + Output;
			_logQueue.Enqueue(newString);
			if (type == LogType.Exception)
			{
				newString = "\n" + stackTrace;
				_logQueue.Enqueue(newString);
			}

			while (_logQueue.Count > MaxLines)
			{
				_logQueue.Dequeue();
			}

			_log = string.Empty;
			foreach (string s in _logQueue)
			{
				_log += s;
			}
		}

		private void OnGUI()
		{
			if (!_hidden)
			{
				GUI.TextArea(new Rect(0, 0, Screen.width / 3, Screen.height), _log);
				if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Hide"))
				{
					_hidden = true;
				}
			}
			else
			{
				if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 20), "Show"))
				{
					_hidden = false;
				}
			}
		}
	}
}