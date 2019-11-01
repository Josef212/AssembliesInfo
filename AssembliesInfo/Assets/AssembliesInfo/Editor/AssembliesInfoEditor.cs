using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

public class AssembliesInfoEditor : EditorWindow
{
    private Dictionary<string, AssemblyInfo> m_runtimeAssemblies = new Dictionary<string, AssemblyInfo>();
    private Dictionary<string, AssemblyInfo> m_editorAssemblies = new Dictionary<string, AssemblyInfo>();

    private DateTime m_compilationStartTime, m_compilationEndTime;
    private TimeSpan m_compileTimeSpan;

    [MenuItem("Tools/Assemblies Information")]
    public static AssembliesInfoEditor ShowWindow()
    {
        AssembliesInfoEditor window = GetWindow<AssembliesInfoEditor>(false, "AssembliesInfo", true);
        window.Init();
        return window;
    }

    private void OnGUI()
    {
        Toolbar();

        GUILayout.Label($"Runtime assemblies: {m_runtimeAssemblies.Count}");
        GUILayout.Label($"Editor assemblies: {m_editorAssemblies.Count}");
        

        Rect runtimeRect = new Rect()
        {
            x = 0, y = EditorGUIUtility.singleLineHeight * 2, width = 200, height = 300
        };

        GUILayout.BeginArea(runtimeRect, Texture2D.whiteTexture);
        
        GUILayout.EndArea();
    }

    private void Init()
    {
        LoadAllAssemblies();
        SubscribeToEvents();

        m_compilationStartTime = m_compilationEndTime = DateTime.Now;
        m_compileTimeSpan = TimeSpan.Zero;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void LoadAllAssemblies()
    {
        Assembly[] playerAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Player);
        Assembly[] editorAssemblies = CompilationPipeline.GetAssemblies(AssembliesType.Editor);

        m_runtimeAssemblies.Clear();
        m_editorAssemblies.Clear();

        foreach(var assmelby in playerAssemblies)
        {
            m_runtimeAssemblies.Add(assmelby.name, new AssemblyInfo(assmelby));
        }

        foreach (var assmelby in editorAssemblies)
        {
            m_editorAssemblies.Add(assmelby.name, new AssemblyInfo(assmelby));
        }
    }

    private void SubscribeToEvents()
    {
        CompilationPipeline.assemblyCompilationStarted += AssemblyCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished += AssemblyCompilationEnded;
        CompilationPipeline.compilationStarted += CompilationStarted;
        CompilationPipeline.compilationFinished += CompilationEnded;
    }

    private void UnsubscribeFromEvents()
    {
        CompilationPipeline.assemblyCompilationStarted -= AssemblyCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished -= AssemblyCompilationEnded;
        CompilationPipeline.compilationStarted -= CompilationStarted;
        CompilationPipeline.compilationFinished -= CompilationEnded;
    }

    private void AssemblyCompilationStarted(string assemblyName)
    {
        GetAssemblyInfoByName(assemblyName)?.CompilationStarted();
    }

    private void AssemblyCompilationEnded(string assemblyName, CompilerMessage[] compilerMessages)
    {
        GetAssemblyInfoByName(assemblyName)?.CompilationEnded(compilerMessages);
    }

    private void CompilationStarted(object obj)
    {
        m_compilationStartTime = DateTime.Now;
        m_compileTimeSpan = TimeSpan.Zero;
    }

    private void CompilationEnded(object obj)
    {
        m_compilationEndTime = DateTime.Now;
        m_compileTimeSpan = m_compilationEndTime - m_compilationStartTime;
    }

    private AssemblyInfo GetAssemblyInfoByName(string assemblyName)
    {
        if (m_runtimeAssemblies.ContainsKey(assemblyName))
            return m_runtimeAssemblies[assemblyName];

        if (m_editorAssemblies.ContainsKey(assemblyName))
            return m_editorAssemblies[assemblyName];
        
        return null;
    }

    private string FormatCompilationDuationTimeSpan(TimeSpan duration)
    {
        return duration.ToString("s\\.fff");
    }

// ---- GUI ----------------------------------------------------------------------
// -------------------------------------------------------------------------------

    private void Toolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Init", EditorStyles.toolbarButton))
        {
            Init();
            EditorGUIUtility.ExitGUI();
        }

        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
        {
            AssetDatabase.Refresh();
            EditorGUIUtility.ExitGUI();
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
    }
}
