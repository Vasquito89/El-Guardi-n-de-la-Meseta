using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private enum VolumeType { VolMaster, VolFX, VolMenu, VolCharacter }
    [SerializeField] private VolumeType type;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterSlider(type.ToString(), GetComponent<Slider>());
        }
    }
}