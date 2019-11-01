using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HelperWindow : EditorWindow  
{
    private AssembliesInfoEditor m_target = null;

    [MenuItem("Tools/Window Helper")]
    public static void ShowWindow()
    {
        AssembliesInfoEditor aW = AssembliesInfoEditor.ShowWindow();
        HelperWindow helper = GetWindow<HelperWindow>(false, "HelperWindow", true);
        helper.Init(aW);
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    public void Init(AssembliesInfoEditor assembliesInfoEditor)
    {
        m_target = assembliesInfoEditor;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label("Static EditorWindow information", EditorStyles.boldLabel);
        GUILayout.Label($"Focused window: {EditorWindow.focusedWindow}");
        GUILayout.Label($"Mouse over window: {EditorWindow.mouseOverWindow}");

        GUILayout.EndVertical();

        Separator();

        GUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label("Generic EditorWindow information", EditorStyles.boldLabel);
        GUILayout.Label($"Window title: {m_target.titleContent.text}");
        GUILayout.Label($"Window position x: {m_target.position.x} - y: {m_target.position.y}");
        GUILayout.Label($"Window size x: {m_target.position.width} - y: {m_target.position.height}");
        GUILayout.Label($"Min size: {m_target.minSize}");
        GUILayout.Label($"Max size: {m_target.maxSize}");
        GUILayout.Label($"Depth buffer bits: {m_target.depthBufferBits}");
        GUILayout.Label($"Maximized: {m_target.maximized}");
        GUILayout.Label($"Autorepaint on scene change: {m_target.autoRepaintOnSceneChange}");
        GUILayout.Label($"Wants mouse enter-leave window: {m_target.wantsMouseEnterLeaveWindow}");
        GUILayout.Label($"Wants mouse move: {m_target.wantsMouseMove}");

        GUILayout.EndVertical();

        Separator();

        GUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label("AssembliesInfoEditor window information", EditorStyles.boldLabel);
        GUILayout.Label($"Runtime assemblies: {m_target.GetFieldValue<Dictionary<string, AssemblyInfo>>("m_runtimeAssemblies").Count}");
        GUILayout.Label($"Editor assemblies: {m_target.GetFieldValue<Dictionary<string, AssemblyInfo>>("m_editorAssemblies").Count}");

        GUILayout.EndVertical();
    }

    private void Separator()
    {
        GUILayout.Label("", GUI.skin.horizontalSlider);
    }
}
