using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [SerializeField] private float velocidadRotacion = 300f;

    void Update()
    {
        transform.Rotate(0f, 0f, -velocidadRotacion * Time.deltaTime);
    }
}
