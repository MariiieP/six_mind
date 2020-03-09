using UnityEngine;

namespace Utils
{
	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static bool _isBehaviourDestroyed = false;

		public static T Instance
		{
			get
			{
				if (_instance == null && !_isBehaviourDestroyed)
				{
					_instance = FindObjectOfType<T>();
					if (_instance != null)
					{
						var singleton = new GameObject("[Singleton] " + typeof(T));
						_instance = singleton.AddComponent<T>();
						return _instance;
					}

				}

				return null;
			}
		}

		public virtual void OnDestroy()
		{
			_isBehaviourDestroyed = true;
		}
	}

}
