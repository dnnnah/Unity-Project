using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Soluciona el error de EventSystem
using TMPro;

public class UIAutoBuilder : EditorWindow
{
    [MenuItem("Tools/Generar Interfaz Responsiva")]
    public static void CreateResponsiveUI()
    {
        // 1. Crear el Canvas principal
        GameObject canvasGO = new GameObject("Responsive_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Configuración de adaptación a cualquier resolución (Scale With Screen Size)
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f; // Equilibrio entre ancho y alto

        canvasGO.AddComponent<GraphicRaycaster>();

        // Asegurar que exista un EventSystem (usando FindAnyObjectByType para evitar warnings)
        if (Object.FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // 2. TÍTULO (Arriba al centro)
        GameObject titleGO = CreateTextObject("TitleText", "MI VENTANA PRINCIPAL", canvasGO.transform);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        SetAnchors(titleRect, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
        titleRect.anchoredPosition = new Vector2(0, -80);
        titleRect.sizeDelta = new Vector2(800, 100);
        TextMeshProUGUI titleTMP = titleGO.GetComponent<TextMeshProUGUI>();
        titleTMP.fontSize = 48;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.fontStyle = FontStyles.Bold;

        // 3. TRES BOTONES (Panel central vertical)
        GameObject centerPanel = new GameObject("Center_Buttons_Panel");
        centerPanel.transform.SetParent(canvasGO.transform, false);
        RectTransform centerRect = centerPanel.AddComponent<RectTransform>();
        SetAnchors(centerRect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        centerRect.anchoredPosition = Vector2.zero;
        centerRect.sizeDelta = new Vector2(350, 300);

        VerticalLayoutGroup vGroup = centerPanel.AddComponent<VerticalLayoutGroup>();
        vGroup.spacing = 20;
        vGroup.childControlWidth = true;
        vGroup.childControlHeight = true;

        for (int i = 1; i <= 3; i++)
        {
            CreateButton($"Button_{i}", $"Botón Central {i}", centerPanel.transform);
        }

        // 4. CUATRO ICONOS (Ubicados en las 4 esquinas/márgenes)
        Vector2[] iconAnchors = new Vector2[]
        {
            new Vector2(0.1f, 0.85f), // Esquina Superior Izquierda
            new Vector2(0.9f, 0.85f), // Esquina Superior Derecha
            new Vector2(0.1f, 0.15f), // Esquina Inferior Izquierda
            new Vector2(0.9f, 0.15f)  // Esquina Inferior Derecha
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject iconGO = new GameObject($"Icon_{i + 1}", typeof(Image));
            iconGO.transform.SetParent(canvasGO.transform, false);
            RectTransform iconRect = iconGO.GetComponent<RectTransform>();
            SetAnchors(iconRect, iconAnchors[i], iconAnchors[i], new Vector2(0.5f, 0.5f));
            iconRect.anchoredPosition = Vector2.zero;
            iconRect.sizeDelta = new Vector2(80, 80);
            iconGO.GetComponent<Image>().color = new Color(0.2f, 0.6f, 0.9f, 0.8f);
        }

        // 5. ENLACES A SITIOS WEB POR COSTADO (3 a la izquierda, 3 a la derecha)
        string[] leftLinks = { "https://unity.com", "https://learn.unity.com", "https://assetstore.unity.com" };
        string[] rightLinks = { "https://google.com", "https://github.com", "https://docs.unity3d.com" };

        // Costado Izquierdo
        CreateSidePanel("Left_Links_Panel", canvasGO.transform, new Vector2(0f, 0.5f), new Vector2(0, 0.5f), new Vector2(180, 0), leftLinks, "Izq");

        // Costado Derecho
        CreateSidePanel("Right_Links_Panel", canvasGO.transform, new Vector2(1f, 0.5f), new Vector2(1, 0.5f), new Vector2(-180, 0), rightLinks, "Der");

        Undo.RegisterCreatedObjectUndo(canvasGO, "Crear Interfaz Responsiva");
        Selection.activeObject = canvasGO;
        Debug.Log("✅ ¡Interfaz UI creada con éxito! Se adapta a cualquier resolución.");
    }

    // --- MÉTODOS AUXILIARES ---

    private static void CreateSidePanel(string name, Transform parent, Vector2 anchor, Vector2 pivot, Vector2 position, string[] urls, string prefix)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.AddComponent<RectTransform>();
        SetAnchors(rect, anchor, anchor, pivot);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(160, 220);

        VerticalLayoutGroup group = panel.AddComponent<VerticalLayoutGroup>();
        group.spacing = 15;
        group.childControlWidth = true;
        group.childControlHeight = true;

        for (int i = 0; i < urls.Length; i++)
        {
            string url = urls[i];
            GameObject btn = CreateButton($"Link_{prefix}_{i + 1}", $"Enlace {prefix} {i + 1}", panel.transform);
            
            Button buttonComp = btn.GetComponent<Button>();
            buttonComp.onClick.AddListener(() => Application.OpenURL(url));
        }
    }

    private static GameObject CreateButton(string name, string label, Transform parent)
    {
        GameObject btnGO = new GameObject(name, typeof(Image), typeof(Button));
        btnGO.transform.SetParent(parent, false);
        btnGO.GetComponent<Image>().color = new Color(0.25f, 0.27f, 0.3f);

        GameObject textGO = CreateTextObject("Text", label, btnGO.transform);
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = textGO.GetComponent<TextMeshProUGUI>();
        tmp.fontSize = 18;
        tmp.alignment = TextAlignmentOptions.Center;

        return btnGO;
    }

    private static GameObject CreateTextObject(string name, string text, Transform parent)
    {
        GameObject textGO = new GameObject(name, typeof(TextMeshProUGUI));
        textGO.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = textGO.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = Color.white;
        return textGO;
    }

    private static void SetAnchors(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.pivot = pivot;
    }
}