using UnityEngine;
using TMPro;

public class FontController : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public float changeAmount = 4f;

    public void IncreaseFontSize()
    {
        if (targetText != null) targetText.fontSize += changeAmount;
    }

    public void DecreaseFontSize()
    {
        if (targetText != null && targetText.fontSize > 12) targetText.fontSize -= changeAmount;
    }
}