using System.Linq;
using UnityEngine;

namespace Utils
{
	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				_instance = FindObjectOfType<T>();

				if (_instance != null)
				{
					return _instance;
				}

				var prefab = Resources.LoadAll<T>(string.Empty).FirstOrDefault();

				if (prefab == null)
				{
					Debug.LogErrorFormat("[Singleton] No instance of {0} found!", typeof(T).ToString());
				}
				else
				{
					_instance = Instantiate(prefab);
				}

				return _instance;
			}
		}
	}

}
