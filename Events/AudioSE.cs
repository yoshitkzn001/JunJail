using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSE : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    public AudioClip[] Nomal_SE;
    public AudioClip[] Skill2_SE;
    public AudioClip[] Skill6_SE;

    public void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
