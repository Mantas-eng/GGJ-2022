using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
	[SerializeField] private string clickSFX = "Click";
	public void LoadScene(int sceneId)
	{
		PlayAudio();
		SceneManager.LoadScene(sceneId);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void StartGame(string levelName)
	{
		GetComponent<AudioSource>().Play();
		StartCoroutine(LoadLevel(levelName));
	}

	private IEnumerator LoadLevel(string levelName)
	{
		yield return new WaitForSeconds(.5f);
		SceneManager.LoadScene(levelName, LoadSceneMode.Single);
	}

	public void PlayAudio()
	{
		AudioPlayer.Instance.PlaySoundEffect(clickSFX);
	}

	public void StopMusic()
	{
		AudioPlayer.Instance.StopMusic();
	}

	public void ResetHighScore()
	{
		PlayerPrefs.SetInt("HighScore", 0);
		PlayerPrefs.Save();

		PlayAudio();
	}
}