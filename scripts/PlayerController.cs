using System.Collections;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro
using UnityEngine.InputSystem; // Necesario para el manejo de inputs

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText; // Texto de UI para mostrar el conteo de monedas
    public GameObject winTextObject; // Objeto de UI para mostrar el mensaje de victoria

    public float jumpForce = 10f;
    private bool canJump = true;
    private float jumpCooldown = 5f;
    private float lastJumpTime;

    private PlayerHealth playerHealth; // Referencia al script de manejo de vidas del jugador
    private Vector3 initialPosition; // Posición inicial del jugador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0; // Inicializa el contador de monedas a 0
        SetCountText();
        winTextObject.SetActive(false); // Asegura que el texto de victoria no se muestre al iniciar
        playerHealth = GetComponent<PlayerHealth>();
        initialPosition = transform.position;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY) * speed;
        rb.AddForce(movement);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && canJump && Time.time - lastJumpTime >= jumpCooldown)
        {
            Jump();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            if (count % 3 == 0)
            {
                count *= 2; // Duplica el conteo cada tres monedas recogidas
            }
            speed *= 1.5f; // Aumenta la velocidad con cada moneda recogida
            SetCountText();
        }

        if (other.gameObject.CompareTag("Damian"))
        {
            playerHealth.LoseLife();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 20) // Si el jugador alcanza 20 monedas, muestra mensaje de victoria
        {
            winTextObject.SetActive(true); // Muestra el mensaje de victoria
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
        lastJumpTime = Time.time;
        StartCoroutine(DeformOnJump());
    }

    IEnumerator DeformOnJump()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 jumpScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z * 1.2f);

        transform.localScale = jumpScale;

        yield return new WaitForSeconds(0.2f);

        while (transform.localScale != originalScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 5);
            yield return null;
        }
    }

    void Update()
    {
        if (!canJump && Time.time - lastJumpTime >= jumpCooldown)
        {
            canJump = true;
        }
    }
}
