using UnityEngine;

public class SoundEnemyController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip actionSound; // Attack or special

    public void PlayWalk()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = walkSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopWalk()
    {
        if (audioSource.clip == walkSound)
        {
            audioSource.Stop();
        }
    }

    public void PlayAction()
    {
        audioSource.PlayOneShot(actionSound);
    }
}
