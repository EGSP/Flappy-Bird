using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController soundController;

    private AudioSource aud;

    [SerializeField] private AudioClip BirdFly = null;
    [SerializeField] private AudioClip BirdDie = null;
    [SerializeField] private AudioClip GetPoint = null;


    // Start is called before the first frame update
    void Start()
    {
        soundController = this;
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBirdFly()
    {
        aud.clip = BirdFly;
        aud.Play();
    }

    public void PlayBirdDie()
    {
        aud.clip = BirdDie;
        aud.Play();
    }

    public void PlayGetPoint()
    {
        aud.clip = GetPoint;
        aud.Play();
    }
}
