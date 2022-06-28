using System.Drawing;

[System.Serializable]
public class GunData
{
	public string name = "matterializatorius";
	public float damage = 0.5f;
	public float fireRate = 1f;
	public GameStateEnum type = GameStateEnum.Mater;

	public string color = "#ff0000";
}
