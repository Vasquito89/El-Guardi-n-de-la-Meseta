using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    private AudioSource ambientSource;
    [SerializeField] private float normalVolume = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        ambientSource = GetComponent<AudioSource>();
        ambientSource.volume = normalVolume;
        ambientSource.Play();
        ambientSource.loop = true;
    }
}
