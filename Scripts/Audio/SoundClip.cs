using System;
using UnityEngine;

[Serializable]
public class SoundClip
{
    [SerializeField] private string audioName;
    [SerializeField] private AudioClip soundSource;

    public string AudioName { get => audioName; set => audioName = value; }
    public AudioClip SoundSource { get => soundSource; set => soundSource = value; }
}