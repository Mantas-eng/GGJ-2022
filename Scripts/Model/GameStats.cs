using System.Collections.Generic;

[System.Serializable]
public partial class GameStats
{
	public float maxHealth = 200f;
	public GameStateEnum startingState = GameStateEnum.Mater;

	public int PlayersKilled = 0;
	public float WalkedDistance = 0f;
	public int BulletShots = 0;
	public List<GunData> guns = new List<GunData>();
	
}
