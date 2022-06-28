using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLibrary))]
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;

    private AudioSource source;

    private readonly Dictionary<int, AudioSource> temporarySources = new Dictionary<int, AudioSource>();
    private int index;

    [SerializeField] private float audioVolumeModifier = 1f;
    [SerializeField] private float musicVolumeModifier = 0.5f;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(AudioPlayer)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (AudioPlayer)FindObjectOfType(typeof(AudioPlayer));
            DontDestroyOnLoad(gameObject);
        }

        index = 0;
        source = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        source.PlayOneShot(clip, 1 / source.volume * PlayerPrefs.GetFloat("AudioVolume", 1f) * audioVolumeModifier);
    }

    public void PlaySoundEffect(string clipName)
    {
        if (AudioLibrary.Instance.CheckIfClipExist(clipName))
        {
            PlaySoundEffect(AudioLibrary.Instance.GetAudioClip(clipName));
        }
    }

    public int StartLoopingSoundEffect(AudioClip clip)
    {
        temporarySources.Add(index, gameObject.AddComponent<AudioSource>());

        PlayMusic(clip, temporarySources[index], PlayerPrefs.GetFloat("AudioVolume", 1f) * audioVolumeModifier);

        index++;
        return index - 1;
    }

    public int StartLoopingSoundEffect(string clipName)
    {
        if (AudioLibrary.Instance.CheckIfClipExist(clipName))
        {
            return StartLoopingSoundEffect(AudioLibrary.Instance.GetAudioClip(clipName));
        }
        return -1;
    }

    public void StopLoopingSoundEffect(int index)
    {
        if (!temporarySources.ContainsKey(index)) return;

        temporarySources[index].Stop();
        Destroy(temporarySources[index]);
        temporarySources.Remove(index);
    }

    public void StopAllSounds()
    {
        StopMusic();

        foreach (KeyValuePair<int, AudioSource> source in temporarySources)
        {
            StopLoopingSoundEffect(source.Key);
        }
    }

    public void PlayMusic(string clipName)
    {
        if (AudioLibrary.Instance.CheckIfClipExist(clipName))
        {
            PlayMusic(AudioLibrary.Instance.GetAudioClip(clipName));
        }
    }

    public void StopMusic()
    {
        source.Stop();
    }

    public void AdjustVolume(float volume)
    {
        source.volume = volume * musicVolumeModifier;
    }

    private void PlayMusic(AudioClip clip, AudioSource source, float volume)
    {
        if (source.clip != null && clip.name == source.clip.name && source.isPlaying) return;

        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.Play();
    }

    private void PlayMusic(AudioClip clip)
    {
        PlayMusic(clip, source, musicVolumeModifier * PlayerPrefs.GetFloat("MusicVolume", 1f));
    }
}