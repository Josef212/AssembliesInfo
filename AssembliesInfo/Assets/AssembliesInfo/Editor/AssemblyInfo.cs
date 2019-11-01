using System;
using UnityEditor.Compilation;

public class AssemblyInfo
{
    public string Name => AssemblyReference?.name;
    public Assembly AssemblyReference { get; private set; }
    public DateTime CompilationStartTime { get; private set; }
    public DateTime CompilationEndTime { get; private set; }
    public TimeSpan CompilationTimeSpan { get; private set; }

    public AssemblyInfo(Assembly assembly)
    {
        AssemblyReference = assembly;
    }

    public void CompilationStarted()
    {
        CompilationTimeSpan = TimeSpan.Zero;
        CompilationStartTime = DateTime.Now;
    }

    public void CompilationEnded(CompilerMessage[] compileMessages)
    {
        CompilationEndTime = DateTime.Now;
        CompilationTimeSpan = CompilationEndTime - CompilationStartTime;
    }
}
