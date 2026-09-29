using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIAutoBuilder : EditorWindow
{
    [MenuItem("Tools/Generar Interfaz Responsiva (Actividad 2.1)")]
    public static void CreateResponsiveUI()
    {
        GameObject canvasGO = new GameObject("Responsive_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        if (Object.FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        SceneSwitcher switcher = canvasGO.AddComponent<SceneSwitcher>();

        // TÍTULO
        GameObject titleGO = CreateTextObject("TitleText", "MI VENTANA PRINCIPAL (ACTIVIDAD 2.1)", canvasGO.transform);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        SetAnchors(titleRect, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
        titleRect.anchoredPosition = new Vector2(0, -80);
        titleRect.sizeDelta = new Vector2(1000, 100);
        TextMeshProUGUI titleTMP = titleGO.GetComponent<TextMeshProUGUI>();
        titleTMP.fontSize = 44;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.fontStyle = FontStyles.Bold;

        // PANEL CENTRAL
        GameObject centerPanel = new GameObject("Center_Buttons_Panel");
        centerPanel.transform.SetParent(canvasGO.transform, false);
        RectTransform centerRect = centerPanel.AddComponent<RectTransform>();
        SetAnchors(centerRect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        centerRect.anchoredPosition = Vector2.zero;
        centerRect.sizeDelta = new Vector2(400, 320);

        VerticalLayoutGroup vGroup = centerPanel.AddComponent<VerticalLayoutGroup>();
        vGroup.spacing = 15;
        vGroup.childControlWidth = true;
        vGroup.childControlHeight = true;

        for (int i = 1; i <= 2; i++)
        {
            CreateButton($"Button_{i}", $"Botón Central {i}", centerPanel.transform);
        }

        // BOTÓN PARA IR A LA ESCENA 2 (ACTIVIDAD 2.2)
        GameObject nextBtnGO = CreateButton("Btn_GoToScene2", "IR A ESCENA 2 ->", centerPanel.transform);
        nextBtnGO.GetComponent<Image>().color = new Color(0.18f, 0.65f, 0.35f);
        Button nextBtn = nextBtnGO.GetComponent<Button>();
        UnityEventTools.AddStringPersistentListener(nextBtn.onClick, switcher.LoadScene, "Escena2");

        // ICONOS EN LAS 4 ESQUINAS
        Vector2[] iconAnchors = new Vector2[]
        {
            new Vector2(0.1f, 0.85f),
            new Vector2(0.9f, 0.85f),
            new Vector2(0.1f, 0.15f),
            new Vector2(0.9f, 0.15f)
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

        // ENLACES LATERALES
        string[] leftLinks = { "https://unity.com", "https://learn.unity.com", "https://assetstore.unity.com" };
        string[] rightLinks = { "https://google.com", "https://github.com", "https://docs.unity3d.com" };

        CreateSidePanel("Left_Links_Panel", canvasGO.transform, new Vector2(0f, 0.5f), new Vector2(0, 0.5f), new Vector2(180, 0), leftLinks, "Izq");
        CreateSidePanel("Right_Links_Panel", canvasGO.transform, new Vector2(1f, 0.5f), new Vector2(1, 0.5f), new Vector2(-180, 0), rightLinks, "Der");

        Undo.RegisterCreatedObjectUndo(canvasGO, "Crear Interfaz Responsiva Actividad 2.1");
        Selection.activeObject = canvasGO;
        Debug.Log("✅ Escena 1 generada correctamente con botón de enlace a Escena 2.");
    }

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
            UnityEventTools.AddPersistentListener(buttonComp.onClick, () => Application.OpenURL(url));
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