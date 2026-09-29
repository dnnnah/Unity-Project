using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SceneNavigationBuilder : EditorWindow
{
    [MenuItem("Tools/Generar Escena Navegable (Actividad 2.4 - Escena 2)")]
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

        // 1. TÍTULO PRINCIPAL (Header)
        GameObject titleGO = CreateTextObject("Header_Title", "APLICACIÓN INTERACTIVA - ESCENA 2", canvasGO.transform);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        SetAnchors(titleRect, new Vector2(0.1f, 0.88f), new Vector2(0.9f, 0.98f), new Vector2(0.5f, 0.5f));
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI titleTMP = titleGO.GetComponent<TextMeshProUGUI>();
        titleTMP.fontSize = 38;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.fontStyle = FontStyles.Bold;

        FontController fontCtrl = titleGO.AddComponent<FontController>();
        fontCtrl.targetText = titleTMP;

        // 2. CREACIÓN DE VISTAS DE CONTENIDO (PANELES)
        GameObject view1 = CreatePanel("View_Inicio", "INICIO", new Color(0.18f, 0.22f, 0.28f), canvasGO.transform);
        GameObject view2 = CreatePanel("View_Seccion1", "SECCIÓN 1: CONTROL DE FUENTE Y ARRASTRE", new Color(0.2f, 0.3f, 0.4f), canvasGO.transform);
        GameObject view3 = CreatePanel("View_Seccion2", "SECCIÓN 2: ANIMACIONES EN BUCLE", new Color(0.28f, 0.2f, 0.32f), canvasGO.transform);

        navController.views = new GameObject[] { view1, view2, view3 };

        // --- CONTENIDO DE LA VISTA 1: INICIO ---
        GameObject welcomeMsg = CreateTextObject("Welcome_Message", "¡Bienvenido a la Escena 2/3!\nUtiliza la barra de navegación para explorar las secciones.", view1.transform);
        RectTransform welcomeRect = welcomeMsg.GetComponent<RectTransform>();
        SetAnchors(welcomeRect, new Vector2(0.1f, 0.2f), new Vector2(0.9f, 0.7f), new Vector2(0.5f, 0.5f));
        welcomeRect.anchoredPosition = Vector2.zero;
        welcomeRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI welcomeTMP = welcomeMsg.GetComponent<TextMeshProUGUI>();
        welcomeTMP.alignment = TextAlignmentOptions.Center;
        welcomeTMP.fontSize = 28;

        // --- CONTENIDO DE LA VISTA 2: SECCIÓN 1 (Control de Fuente + Sprite Arrastrable) ---
        // Panel de controles de fuente
        GameObject fontControlsPanel = new GameObject("Font_Controls_Panel");
        fontControlsPanel.transform.SetParent(view2.transform, false);
        RectTransform fontPanelRect = fontControlsPanel.AddComponent<RectTransform>();
        SetAnchors(fontPanelRect, new Vector2(0.3f, 0.62f), new Vector2(0.7f, 0.75f), new Vector2(0.5f, 0.5f));
        fontPanelRect.anchoredPosition = Vector2.zero;
        fontPanelRect.sizeDelta = Vector2.zero;

        HorizontalLayoutGroup fontLayout = fontControlsPanel.AddComponent<HorizontalLayoutGroup>();
        fontLayout.spacing = 20;
        fontLayout.childControlWidth = true;
        fontLayout.childControlHeight = true;

        GameObject btnFontInc = CreateButton("Btn_Font_Inc", "Tamaño Texto A+", fontControlsPanel.transform);
        btnFontInc.GetComponent<Image>().color = new Color(0.2f, 0.6f, 0.3f);
        Button fontIncComp = btnFontInc.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(fontIncComp.onClick, fontCtrl.IncreaseFontSize);

        GameObject btnFontDec = CreateButton("Btn_Font_Dec", "Tamaño Texto A-", fontControlsPanel.transform);
        btnFontDec.GetComponent<Image>().color = new Color(0.6f, 0.3f, 0.2f);
        Button fontDecComp = btnFontDec.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(fontDecComp.onClick, fontCtrl.DecreaseFontSize);

        // Sprite Arrastrable
        GameObject draggableSprite = new GameObject("Draggable_Sprite_Item", typeof(Image), typeof(SpriteDragger));
        draggableSprite.transform.SetParent(view2.transform, false);
        Image img = draggableSprite.GetComponent<Image>();
        img.color = new Color(0.95f, 0.5f, 0.1f, 1f);
        
        RectTransform spriteRect = draggableSprite.GetComponent<RectTransform>();
        SetAnchors(spriteRect, new Vector2(0.5f, 0.35f), new Vector2(0.5f, 0.35f), new Vector2(0.5f, 0.5f));
        spriteRect.anchoredPosition = Vector2.zero;
        spriteRect.sizeDelta = new Vector2(160, 160);

        GameObject spriteTxt = CreateTextObject("Label", "¡ARRÁSTRAME!", draggableSprite.transform);
        RectTransform spriteTxtRect = spriteTxt.GetComponent<RectTransform>();
        spriteTxtRect.anchorMin = Vector2.zero;
        spriteTxtRect.anchorMax = Vector2.one;
        spriteTxtRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI spriteTMP = spriteTxt.GetComponent<TextMeshProUGUI>();
        spriteTMP.alignment = TextAlignmentOptions.Center;
        spriteTMP.fontSize = 20;
        spriteTMP.fontStyle = FontStyles.Bold;

        // --- CONTENIDO DE LA VISTA 3: SECCIÓN 2 (Animaciones en Bucle) ---
        GameObject animationsParent = new GameObject("LoopAnimations_Container");
        animationsParent.transform.SetParent(view3.transform, false);

        Vector2[] animPositions = new Vector2[]
        {
            new Vector2(-350, -20),  // Fogata 1 (Izquierda)
            new Vector2(0, -20),     // Llama Azul (Centro)
            new Vector2(350, -20)    // Antorcha (Derecha)
        };

        Color[] animBaseColors = new Color[]
        {
            new Color(1f, 0.35f, 0.1f),
            new Color(0.2f, 0.8f, 1f),
            new Color(0.9f, 0.8f, 0.2f)
        };

        string[] animLabels = { "Fogata 1 (8 Frames)", "Llama Azul (8 Frames)", "Antorcha (8 Frames)" };

        for (int i = 0; i < 3; i++)
        {
            GameObject animGO = new GameObject($"LoopAnim_Item_{i + 1}", typeof(Image), typeof(SpriteLoopAnimator));
            animGO.transform.SetParent(animationsParent.transform, false);
            
            Image imgComp = animGO.GetComponent<Image>();
            imgComp.color = animBaseColors[i];

            RectTransform animRect = animGO.GetComponent<RectTransform>();
            SetAnchors(animRect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            animRect.anchoredPosition = animPositions[i];
            animRect.sizeDelta = new Vector2(120, 120);

            GameObject labelGO = CreateTextObject("Label", animLabels[i], animGO.transform);
            RectTransform labelRect = labelGO.GetComponent<RectTransform>();
            SetAnchors(labelRect, new Vector2(0f, -0.4f), new Vector2(1f, 0f), new Vector2(0.5f, 0.5f));
            labelRect.anchoredPosition = Vector2.zero;
            labelRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI labelTMP = labelGO.GetComponent<TextMeshProUGUI>();
            labelTMP.fontSize = 14;
            labelTMP.alignment = TextAlignmentOptions.Center;
        }

        // 3. BARRA DE NAVEGACIÓN
        GameObject navBarGO = new GameObject("Navigation_Bar", typeof(Image));
        navBarGO.transform.SetParent(canvasGO.transform, false);
        navBarGO.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.9f);
        
        RectTransform navBarRect = navBarGO.GetComponent<RectTransform>();
        SetAnchors(navBarRect, new Vector2(0f, 0f), new Vector2(1f, 0.12f), new Vector2(0.5f, 0f));
        navBarRect.anchoredPosition = Vector2.zero;
        navBarRect.sizeDelta = Vector2.zero;

        HorizontalLayoutGroup navLayout = navBarGO.AddComponent<HorizontalLayoutGroup>();
        navLayout.spacing = 15;
        navLayout.childControlWidth = true;
        navLayout.childControlHeight = true;
        navLayout.padding = new RectOffset(20, 20, 15, 15);

        // BOTÓN NAVEGACIÓN A OTRA ESCENA
        GameObject backBtnGO = CreateButton("NavBtn_Back", "<- ESCENA 1", navBarGO.transform);
        backBtnGO.GetComponent<Image>().color = new Color(0.75f, 0.25f, 0.25f);
        Button backBtn = backBtnGO.GetComponent<Button>();
        UnityEventTools.AddStringPersistentListener(backBtn.onClick, switcher.LoadScene, "Escena1");

        // BOTONES DE PANELES NAVEGABLES
        string[] btnLabels = { "Inicio", "Sección 1", "Sección 2" };
        for (int i = 0; i < btnLabels.Length; i++)
        {
            int index = i;
            GameObject btnGO = CreateButton($"NavBtn_{i}", btnLabels[i], navBarGO.transform);
            Button btn = btnGO.GetComponent<Button>();
            UnityEventTools.AddIntPersistentListener(btn.onClick, navController.ShowView, index);
        }

        navController.ShowView(0);

        Undo.RegisterCreatedObjectUndo(canvasGO, "Crear Escena Navegable 3");
        Selection.activeObject = canvasGO;
        Debug.Log("✅ Escena configurada correctamente con Inicio, Sección 1 y Sección 2 organizadas.");
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
        SetAnchors(txtRect, new Vector2(0f, 0.82f), new Vector2(1f, 0.95f), new Vector2(0.5f, 0.5f));
        txtRect.anchoredPosition = Vector2.zero;
        txtRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = txt.GetComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 28;

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