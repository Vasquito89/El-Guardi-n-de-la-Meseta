using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 300f;
    private AudioSource audioSource;

    private AudioSource ambientSource;
    [SerializeField] private float normalVolume = 1.0f;
    [SerializeField] private float reducedVolume = 0.2f;

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false; // Asegúrese de que no comience inmediatamente
        }
            ambientSource = FindObjectOfType<AmbientSound>()?.GetComponent<AudioSource>();
        if (ambientSource == null)
        {
            Debug.LogWarning("No se encontró AmbientSound en la escena. El volumen no se ajustará.");
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
                ambientSource.volume = reducedVolume;
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
                ambientSource.volume = normalVolume;
            }
            Debug.Log("El jugador abandonó el área de Tumbleweed - Deteniendo el sonido");
        }
    }
}

