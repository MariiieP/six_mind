namespace Gameplay
{
	public abstract class SessionRequest
	{
		public readonly Letter Prefab;

		protected SessionRequest(Letter prefab)
		{
			Prefab = prefab;
		}
	}
}
