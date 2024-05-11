using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/SettingsWindow")]
    public static void ShowExample()
    {
        SettingsWindow wnd = GetWindow<SettingsWindow>();
        wnd.titleContent = new GUIContent("SettingsWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
