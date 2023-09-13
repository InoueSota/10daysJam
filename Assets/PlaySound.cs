using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    [SerializeField] private AudioClip sound0;
    [SerializeField] private AudioClip sound1;
    [SerializeField] private AudioClip sound2;
    [SerializeField] private AudioClip sound3;

    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = this.GetComponent<AudioSource>();
    }



    public void PlaySound0()
    {
        if (audioSource != null && sound0 != null)
        {
            audioSource.PlayOneShot(sound0);
        }
    }

    public void PlaySound1()
    {
        if (audioSource != null && sound1 != null)
        {
            audioSource.PlayOneShot(sound1);
        }
    }

    public void PlaySound2()
    {
        if (audioSource != null && sound2 != null)
        {
            audioSource.PlayOneShot(sound2);
        }
    }

    public void PlaySound3()
    {
        if (audioSource != null && sound3 != null)
        {
            audioSource.PlayOneShot(sound3);
        }
    } 
}
