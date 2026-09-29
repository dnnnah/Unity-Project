using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteLoopAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 12f; // Fotogramas por segundo
    public Color[] colorFrames;   // Variación de color para simular animación de fuego/luces (8 frames)

    private Image image;
    private int currentFrame = 0;
    private float timer = 0f;

    private void Awake()
    {
        image = GetComponent<Image>();
        
        // Si no se asignan sprites externos, se crean 8 estados/frames de color para la animación en bucle
        if (colorFrames == null || colorFrames.Length == 0)
        {
            colorFrames = new Color[8];
            Color baseColor = image.color;
            for (int i = 0; i < 8; i++)
            {
                float intensity = Mathf.PingPong(i * 0.25f, 0.4f) + 0.6f;
                colorFrames[i] = new Color(baseColor.r * intensity, baseColor.g * intensity, baseColor.b * intensity, baseColor.a);
            }
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;

            if (frames != null && frames.Length > 0)
            {
                // Animación mediante Sprites
                currentFrame = (currentFrame + 1) % frames.Length;
                image.sprite = frames[currentFrame];
            }
            else if (colorFrames != null && colorFrames.Length > 0)
            {
                // Animación mediante Secuencia de 8 Frames de Color (Llama/Luz)
                currentFrame = (currentFrame + 1) % colorFrames.Length;
                image.color = colorFrames[currentFrame];
            }
        }
    }
}