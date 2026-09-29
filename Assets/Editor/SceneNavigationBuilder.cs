using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SceneNavigationBuilder : EditorWindow
{
    [MenuItem("Tools/Generar Escena Navegable (Actividad 2.2)")]
    public static void CreateNavigationScene()
    {
        GameObject canvasGO = new GameObject("MainNavigation_Canvas");
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

        NavigationController navController = canvasGO.AddComponent<NavigationController>();
        SceneSwitcher switcher = canvasGO.AddComponent<SceneSwitcher>();

        // TÍTULO
        GameObject titleGO = CreateTextObject("Header_Title", "APLICACIÓN INTERACTIVA - UNIDAD 2", canvasGO.transform);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        SetAnchors(titleRect, new Vector2(0.1f, 0.88f), new Vector2(0.9f, 0.98f), new Vector2(0.5f, 0.5f));
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI titleTMP = titleGO.GetComponent<TextMeshProUGUI>();
        titleTMP.fontSize = 40;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.fontStyle = FontStyles.Bold;

        // BARRA DE NAVEGACIÓN
        GameObject navBarGO = new GameObject("Navigation_Bar", typeof(Image));
        navBarGO.transform.SetParent(canvasGO.transform, false);
        navBarGO.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.9f);
        
        RectTransform navBarRect = navBarGO.GetComponent<RectTransform>();
        SetAnchors(navBarRect, new Vector2(0f, 0f), new Vector2(1f, 0.12f), new Vector2(0.5f, 0f));
        navBarRect.anchoredPosition = Vector2.zero;
        navBarRect.sizeDelta = Vector2.zero;

        HorizontalLayoutGroup navLayout = navBarGO.AddComponent<HorizontalLayoutGroup>();
        navLayout.spacing = 20;
        navLayout.childControlWidth = true;
        navLayout.childControlHeight = true;
        navLayout.padding = new RectOffset(30, 30, 15, 15);

        // VISTAS DE CONTENIDO
        GameObject view1 = CreatePanel("View_Inicio", "PANEL DE INICIO", new Color(0.2f, 0.2f, 0.25f), canvasGO.transform);
        GameObject view2 = CreatePanel("View_Seccion1", "SECCIÓN 1 - CONTENIDO", new Color(0.2f, 0.3f, 0.4f), canvasGO.transform);
        GameObject view3 = CreatePanel("View_Seccion2", "SECCIÓN 2 - AJUSTES", new Color(0.3f, 0.2f, 0.3f), canvasGO.transform);

        navController.views = new GameObject[] { view1, view2, view3 };

        // BOTÓN VOLVER A ESCENA 1
        GameObject backBtnGO = CreateButton("NavBtn_Back", "<- VOLVER A ESCENA 1", navBarGO.transform);
        backBtnGO.GetComponent<Image>().color = new Color(0.75f, 0.25f, 0.25f);
        Button backBtn = backBtnGO.GetComponent<Button>();
        UnityEventTools.AddStringPersistentListener(backBtn.onClick, switcher.LoadScene, "Escena1");

        // BOTONES DE NAVEGACIÓN VISTAS
        string[] btnLabels = { "Inicio", "Sección 1", "Sección 2" };
        for (int i = 0; i < btnLabels.Length; i++)
        {
            int index = i;
            GameObject btnGO = CreateButton($"NavBtn_{i}", btnLabels[i], navBarGO.transform);
            Button btn = btnGO.GetComponent<Button>();
            UnityEventTools.AddIntPersistentListener(btn.onClick, navController.ShowView, index);
        }

        navController.ShowView(0);

        Undo.RegisterCreatedObjectUndo(canvasGO, "Crear Escena Navegable Actividad 2.2");
        Selection.activeObject = canvasGO;
        Debug.Log("✅ Escena 2 generada correctamente con botones de navegación y retorno a Escena 1.");
    }

    private static GameObject CreatePanel(string name, string title, Color color, Transform parent)
    {
        GameObject panel = new GameObject(name, typeof(Image));
        panel.transform.SetParent(parent, false);
        panel.GetComponent<Image>().color = color;
        
        RectTransform rect = panel.GetComponent<RectTransform>();
        SetAnchors(rect, new Vector2(0.05f, 0.15f), new Vector2(0.95f, 0.85f), new Vector2(0.5f, 0.5f));
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

        GameObject txt = CreateTextObject("Text", title, panel.transform);
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        SetAnchors(txtRect, new Vector2(0f, 0.4f), new Vector2(1f, 0.6f), new Vector2(0.5f, 0.5f));
        txtRect.anchoredPosition = Vector2.zero;
        txtRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = txt.GetComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 32;

        return panel;
    }

    private static GameObject CreateButton(string name, string label, Transform parent)
    {
        GameObject btnGO = new GameObject(name, typeof(Image), typeof(Button));
        btnGO.transform.SetParent(parent, false);
        btnGO.GetComponent<Image>().color = new Color(0.25f, 0.45f, 0.75f);

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