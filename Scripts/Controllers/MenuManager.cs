using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string musicTrack = "MenuMusic";
    [SerializeField] private TMP_Text highScoreValue;

    void Start()
    {
        AudioPlayer.Instance.PlayMusic(musicTrack);

        highScoreValue.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}