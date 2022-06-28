using UnityEngine;
using GGJ.Core;
using TMPro;

public class GameManager : MonoBehaviour
{
	
	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	[SerializeField] private GameObject player;
	[SerializeField] private TMP_Text scoreValue;
	[SerializeField] private GameStats _stats;

	public GameObject Player { get => player; }
	public GameStats stats {
		get => _stats;
		set
		{
			_stats = value;
		}
	}

	void Awake()
	{
		HealthBar.InstantiateHealthBar(stats.maxHealth, stats.startingState, stats.maxHealth/2);

		PlayerPrefs.SetInt("CurrentScore", 0);
		PlayerPrefs.Save();

		if (this.player != null) GameManager.Instance.player = this.player;
		if (GameManager.Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		
		
		DontDestroyOnLoad(this.gameObject);
	}

	public void UpdateScore()
	{
		scoreValue.text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();
	}
}