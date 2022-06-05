using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance;

  private Dictionary<AudioRef, AudioSource> audioSources;
  public AudioList audioList;

  private AudioSource oneShotAudioSource;

  void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start() {
    DontDestroyOnLoad(gameObject);

    oneShotAudioSource = gameObject.AddComponent<AudioSource>();
    // audioSources = new Dictionary<AudioRef, AudioSource>();
    // foreach (AudioRef audio in audioList.audios) {
    //   AudioSource src = gameObject.AddComponent<AudioSource>();
    //   src.clip = audio.clip;
    //   audioSources.Add(audio, src);
    // }
  }

  public void PlayOneShot(AudioRef audio) {
    oneShotAudioSource.PlayOneShot(audio.clip);
  }

  public void PlayOneShot(AudioRef audio, float vol) {
    oneShotAudioSource.PlayOneShot(audio.clip, vol);
  }
}
