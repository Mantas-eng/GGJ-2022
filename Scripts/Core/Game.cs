namespace GGJ.Core
{

	public static partial class Game
	{
		static public GameStats GetModel<T>()
		{
			return GameManager.Instance.stats;
		}
	}

}
