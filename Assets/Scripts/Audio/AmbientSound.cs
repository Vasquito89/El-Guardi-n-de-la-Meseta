using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    private AudioSource ambientSource;
    [SerializeField] private float normalVolume = 1.0f;
    [SerializeField] private float reducedVolume = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        ambientSource = GetComponent<AudioSource>();
        ambientSource.volume = normalVolume;
        ambientSource.Play();
        ambientSource.loop = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ambientSource.volume = reducedVolume; // Ajusta el volumen según sea necesario
        }
    }

    void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
            ambientSource.volume = normalVolume; // Ajusta el volumen según sea necesario
            }
    }
}
