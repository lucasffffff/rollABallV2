using System.Collections;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    public float detectionDistance = 2f;
    private GameObject player;
    private Renderer playerRenderer;
    private Color originalColor = Color.red;
    private Color alertColor = new Color(0.5f, 1f, 0.5f); // Verde claro
    private bool isBlinking = false;
    private MaterialPropertyBlock propBlock;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Damian");
        if (player != null)
        {
            playerRenderer = player.GetComponent<Renderer>();
            if (playerRenderer == null)
            {
                Debug.LogError("EnemyProximity: No se encontró Renderer en el jugador.");
            }
            propBlock = new MaterialPropertyBlock();
            playerRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", originalColor);
            playerRenderer.SetPropertyBlock(propBlock);
        }
    }

    private void Update()
    {
        if (player != null && playerRenderer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionDistance && !isBlinking)
            {
                StartBlinking();
            }
            else if (distanceToPlayer > detectionDistance && isBlinking)
            {
                StopBlinking();
            }
        }
    }

    private void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(BlinkRoutine());
        }
    }

    private void StopBlinking()
    {
        if (isBlinking)
        {
            isBlinking = false;
            StopAllCoroutines();
            SetPlayerColor(originalColor); // Restablece el color original
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (isBlinking)
        {
            SetPlayerColor(playerRenderer.material.color == originalColor ? alertColor : originalColor);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetPlayerColor(Color color)
    {
        if (playerRenderer != null && propBlock != null)
        {
            propBlock.SetColor("_Color", color);
            playerRenderer.SetPropertyBlock(propBlock);
        }
    }
}
