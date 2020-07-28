using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오디오 매니저(볼륨 설정)
public class AudioManager : MonoBehaviour
{
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = Option.audioVolume;
    }
}
