using UnityEngine;

public class CambiarVelocidad : MonoBehaviour
{
    public float velocidadNueva = 50f; // Velocidad que se aplicará al jugador al entrar en contacto con este objeto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = rb.velocity.normalized * velocidadNueva; // Aplicar nueva velocidad manteniendo la dirección
            }
        }
    }
}

