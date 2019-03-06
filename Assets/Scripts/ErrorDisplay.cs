using UnityEngine;
using System.Collections;

//在屏幕上打印错误信息，用于在移动端的测试
public class ErrorDisplay : MonoBehaviour
{
    internal void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    internal void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    private string m_logs;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logString">错误信息</param>
    /// <param name="stackTrace">跟踪堆栈</param>
    /// <param name="type">错误类型</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        m_logs += logString + "\n";
    }
    public bool Log;
    private Vector2 m_scroll;
    public GUIStyle myStyle;
    internal void OnGUI()
    {
        if (!Log)
            return;
        m_scroll = GUILayout.BeginScrollView(m_scroll);
        GUILayout.Label(m_logs,myStyle);
        GUILayout.EndScrollView();
    }
}