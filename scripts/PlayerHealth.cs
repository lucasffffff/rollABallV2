using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // Necesario para acceder a EditorApplication
#endif

public class PlayerHealth : MonoBehaviour
{
    public int startingLives = 3; // Vidas iniciales del jugador
    private int currentLives; // Vidas actuales del jugador
    private Vector3 initialPosition; // Posición inicial del jugador
    private Renderer playerRenderer; // Referencia al componente Renderer del jugador
    public Material damageMaterial; // Material para mostrar el efecto de daño
    private Material originalMaterial; // Material original del jugador
    public float damageDuration = 1f; // Duración del efecto de daño

    private void Start()
    {
        currentLives = startingLives;
        initialPosition = transform.position;
        playerRenderer = GetComponent<Renderer>();
        originalMaterial = playerRenderer.material;
    }

    public void LoseLife()
    {
        currentLives--;

        Debug.Log("Has perdido una vida!");

        if (currentLives <= 0)
        {
            Debug.Log("Perdiste!. Ya no te quedan vidas.");
            // Aquí cierras la aplicación o recargas la escena según sea necesario
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // Detiene la ejecución en el editor de Unity
#else
            Application.Quit(); // Cierra la aplicación cuando se ejecuta como build
#endif
        }

        // Restablece la posición del jugador a la posición inicial y muestra efecto de daño
        transform.position = initialPosition;
        StartCoroutine(ShowDamageEffect());
    }

    private IEnumerator ShowDamageEffect()
    {
        if (damageMaterial != null && playerRenderer != null)
        {
            playerRenderer.material = damageMaterial;
            yield return new WaitForSeconds(damageDuration);
            playerRenderer.material = originalMaterial;
        }
    }

    public int CurrentLives
    {
        get { return currentLives; }
    }
}
