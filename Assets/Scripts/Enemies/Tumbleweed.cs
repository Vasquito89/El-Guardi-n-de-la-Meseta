using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 300f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false; // Asegúrese de que no comience inmediatamente
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Debug.Log("El jugador está cerca de Tumbleweed - Reproduciendo sonido");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            Debug.Log("El jugador abandonó el área de Tumbleweed - Deteniendo el sonido");
        }
    }
}

