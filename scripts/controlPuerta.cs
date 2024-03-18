using UnityEngine;

public class ControlPuerta : MonoBehaviour
{
    public float distanciaSubida = 4f; // Altura a la que subirá la puerta al presionar 'Q'
    private Vector3 posicionOriginal; // Posición original de la puerta
    private bool puertaBajando = false; // Variable para controlar si la puerta está bajando

    void Start()
    {
        // Guardamos la posición original al iniciar
        posicionOriginal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica si la puerta no está bajando y si el jugador presiona la tecla 'Q'
        if (!puertaBajando && Input.GetKeyDown(KeyCode.Q))
        {
            // Calculamos la nueva posición de la puerta
            Vector3 nuevaPosicion = posicionOriginal + Vector3.up * distanciaSubida;

            // Movemos la puerta a la nueva posición
            transform.position = nuevaPosicion;

            // Actualizamos el estado de la puerta a bajando
            puertaBajando = true;
        }
        // Verifica si la puerta está bajando y si ha vuelto a su posición original
        else if (puertaBajando && transform.position.y <= posicionOriginal.y)
        {
            // Restablecemos el estado de la puerta a no bajando
            puertaBajando = false;
        }

        // Verifica si el jugador presiona la tecla 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Movemos la puerta de vuelta a su posición original
            transform.position = posicionOriginal;
        }
    }
}
