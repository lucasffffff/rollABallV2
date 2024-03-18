using UnityEngine;

public class EnemigoPerseguir : MonoBehaviour
{
    public Transform objetivo; // Referencia al transform del jugador
    public float velocidadMovimiento = 4f; // Velocidad de movimiento del enemigo
    public float distanciaCambioColor = 2f; // Distancia a partir de la cual el color del enemigo cambia a verde claro
    public Color colorVerdeClaro = Color.green; // Color al que cambiará el enemigo cuando esté cerca del jugador

    private new Renderer renderer; // Cambio de nombre de la variable renderer para evitar ocultar el miembro heredado
    private Color colorOriginal;
    private bool cambioColor = false;

    private int golpesAlJugador = 0;
    private float tiempoUltimoGolpe = 0f;
    public float intervaloEntreGolpes = 5f; // Tiempo en segundos entre cada golpe

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        colorOriginal = renderer.material.color;

        if (objetivo == null)
        {
            // Si no se asignó un objetivo, intenta encontrar automáticamente al jugador
            objetivo = GameObject.FindGameObjectWithTag("Damian")?.transform;
        }
    }

    private void Update()
    {
        if (objetivo != null)
        {
            // Calcula la distancia entre el enemigo y el jugador
            float distanciaAlJugador = Vector3.Distance(transform.position, objetivo.position);

            // Cambia el color del enemigo a morado si está muy cerca del jugador
            if (distanciaAlJugador <= distanciaCambioColor)
            {
                renderer.material.color = colorVerdeClaro;
            }
            else
            {
                // Si el jugador está fuera del rango de detección, vuelve al color original
                renderer.material.color = colorOriginal;
            }

            // Verifica si ha pasado suficiente tiempo desde el último golpe
            if (Time.time - tiempoUltimoGolpe >= intervaloEntreGolpes)
            {
                // Golpear al jugador
                GolpearJugador();
                tiempoUltimoGolpe = Time.time;
            }

            // Calcula la dirección hacia la que debe moverse el enemigo para perseguir al jugador
            Vector3 direccion = (objetivo.position - transform.position).normalized;

            // Calcula el vector de movimiento utilizando la dirección y la velocidad
            Vector3 movimiento = direccion * velocidadMovimiento * Time.deltaTime;

            // Mueve al enemigo hacia el jugador
            transform.Translate(movimiento);
        }
    }

    private void GolpearJugador()
    {
        // Incrementa el contador de golpes
        golpesAlJugador++;

        // Si se alcanzó el límite de golpes, llama al método LoseLife del jugador
        if (golpesAlJugador >= 3)
        {
            PlayerHealth playerHealth = objetivo.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.LoseLife();
                // Cambia el color del enemigo a amarillo durante 1 segundo
                renderer.material.color = Color.yellow;
                Invoke("RestoreOriginalColor", 1f); // Restaura el color original después de 1 segundo
            }
            Debug.Log("He perdido una vida!");
        }
    }

    // Método para restaurar el color original del enemigo
    private void RestoreOriginalColor()
    {
        renderer.material.color = colorOriginal;
    }
}