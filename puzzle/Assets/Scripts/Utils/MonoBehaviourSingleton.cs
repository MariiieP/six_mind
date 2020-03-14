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
				if (!ReferenceEquals(_instance, null) || _isBehaviourDestroyed) return null;
				
				var singleton = new GameObject("[Singleton] " + typeof(T));
				_instance = FindObjectOfType<T>();
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
