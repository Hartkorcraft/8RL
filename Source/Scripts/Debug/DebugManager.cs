using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class DebugManager : Control
{
    public static bool Console_Visible { get => debugConsole?.Visible ?? false; }

    public static DebugConsole? debugConsole { get; private set; }
    static Label? debugInfoLabel;
    static Dictionary<string, DebugInfo> logs = new Dictionary<string, DebugInfo>(); // Use AddLog, DeleteLog, UpdateLog and ClearLogs()
    static string debugLabelText = "Debug label";

    public static string GetLogDisplay { get => debugInfoLabel?.Text ?? "debugInfoNull"; }

    public override void _EnterTree()
    {
        debugInfoLabel = (Label)GetNode("UiLayer/DebugInfoLabel");
        debugConsole = (DebugConsole)GetNode("UiLayer/DebugConsole");
        debugInfoLabel.Text = debugLabelText;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Toggle_Debug_Menu"))
        {
            DisplayConsole();
        }

        void DisplayConsole()
        {
            if (debugConsole is null) { return; }
            GD.Print("Showed debug console");
            debugConsole.Visible = !debugConsole.Visible;
            debugConsole.GrabInputFocus();
        }
    }


    public static void AddLog(DebugInfo log)
    {
        logs.Add(log.Name, log);
        UpdateLogsDisplay();
    }

    public static void DeleteLog(string name)
    {
        logs.Remove(name);
        UpdateLogsDisplay();
    }

    public static void ClearLogs()
    {
        logs.Clear();
        UpdateLogsDisplay();
    }

    public static void UpdateLog(string name, string logText, bool display = true, bool displayIfEmpty = true)
    {
        if (logs.ContainsKey(name) is false) { GD.PrintErr("no log " + name); }
        logs[name].LabelText = logText;
        logs[name].Display = display;
        UpdateLogsDisplay(displayIfEmpty);
    }

    public static void UpdateLogsDisplay(bool displayIfEmpty = true)
    {
        string debugLabelText = "";

        foreach (KeyValuePair<string, DebugInfo> log in logs)
        {
            if (log.Value.Display is false) continue;
            if (displayIfEmpty == false && String.IsNullOrWhiteSpace(log.Value.LabelText)) { continue; }
            debugLabelText += log.Value.Name + ": " + log.Value.LabelText + NewLine;
        }
        if (debugInfoLabel is not null) { debugInfoLabel.Text = debugLabelText; }
    }
}

