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
				if (_isBehaviourDestroyed) return null;

				if (!ReferenceEquals(_instance, null)) return _instance;
				_instance = FindObjectOfType<T>();

				if (!ReferenceEquals(_instance, null)) return _instance;
				
				var singleton = new GameObject("[Singleton] " + typeof(T));
				_instance = singleton.AddComponent<T>();

				return _instance;
			}
		}

		public virtual void OnDestroy()
		{
			_isBehaviourDestroyed = true;
		}
	}

}
