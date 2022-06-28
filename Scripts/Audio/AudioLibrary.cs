using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    public static AudioLibrary Instance;

    [SerializeField] private List<SoundClip> audioFile = new List<SoundClip>();
    private readonly Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    private readonly List<string> clipsNameList = new List<string>();

    protected virtual void Awake()
    {
        if (FindObjectsOfType(typeof(AudioLibrary)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (AudioLibrary)FindObjectOfType(typeof(AudioLibrary));
            DontDestroyOnLoad(gameObject);
        }

        foreach (var soundClip in audioFile.Where(soundClip => soundClip != null &&
                                                  !clipDictionary.ContainsKey(soundClip.AudioName)))
        {
            clipDictionary.Add(soundClip.AudioName, soundClip.SoundSource);
            clipsNameList.Add(soundClip.AudioName);
        }
    }

    public List<string> GetAudioList()
    {
        return clipsNameList;
    }

    public AudioClip GetAudioClip(string name)
    {
        return clipDictionary[name];
    }

    public bool CheckIfClipExist(string name)
    {
        return clipDictionary.ContainsKey(name);
    }
}