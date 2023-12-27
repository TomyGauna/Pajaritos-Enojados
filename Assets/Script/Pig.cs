using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private float minCollisionForce = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar la fuerza de la colisión y destruir al pájaro si es suficientemente fuerte
        if (collision.relativeVelocity.magnitude >= minCollisionForce)
        {
            Destroy(gameObject);
            // Puedes agregar aquí la lógica para instanciar un nuevo pájaro si lo deseas.
        }
    }
}
