using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollisionDetector : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    private Image objectImage;
    private Color defaultColor;
    public Color collisionColor = Color.red;

    private void Awake()
    {
        objectImage = GetComponent<Image>();
        if (objectImage != null)
        {
            defaultColor = objectImage.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (objectImage != null)
        {
            objectImage.color = collisionColor;
        }

        if (statusText != null)
        {
            statusText.text = "⚠️ ¡COLISIÓN DETECTADA CON: " + other.gameObject.name + "!";
            statusText.color = Color.red;
        }

        Debug.Log("💥 Colisión detectada con: " + other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectImage != null)
        {
            objectImage.color = defaultColor;
        }

        if (statusText != null)
        {
            statusText.text = "✅ Estado: Sin colisiones (Arrastra el sprite hacia el obstáculo)";
            statusText.color = Color.green;
        }

        Debug.Log("✅ Colisión terminada");
    }
}