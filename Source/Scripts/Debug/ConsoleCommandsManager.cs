using System;
using System.Collections.Generic;

public class ConsoleCommandsManager
{
    DebugManager debugManager;
    DebugConsole console;
    public List<object> commandList = new List<object>();

    public static DebugCommand? HELLO;
    public static DebugCommand? HELP;
    public static DebugCommand? GET_PLAYER_CHARACTER_INFO;
    public static DebugCommand? CLEAR_CONSOLE;
    public static DebugCommand? PRINT_LOGS;

    private void AddCommands()
    {
        HELLO = new DebugCommand("hello", "Prints Hello!", "hello", () =>
        {
            console.OutputText("* Hello!");
        });

        HELP = new DebugCommand("help", "Prints commands and other help", "help", () =>
        {
            console.OutputText("* Available Commands:");
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase? commandBase = commandList[i] as DebugCommandBase;
                console.OutputText($"- {commandBase?.commandId} {commandBase?.commandDescription} {commandBase?.commandFormat}");
            }
        });

        // GET_PLAYER_CHARACTER_INFO = new DebugCommand("get_player_info", "prints IGetInfoAble Info", "get_player_info", () =>
        // {
        //     var index = 0;

        //     console.OutputText($"*Current selection: {GameManager.SelectionModule.CurrentSelection?.ToString()}");
        //     foreach (var player_character in GameManager.InfoModule.GetPlayerInfoObjects())
        //     {
        //         console.OutputText("---------------------------------------------------");
        //         console.OutputText($"{index}. ");
        //         console.OutputText($"{player_character.GetInfo()}");
        //         index++;
        //     }
        // });

        CLEAR_CONSOLE = new DebugCommand("clear_console", "clears console", "clear_console", () =>
        {
            console.Clear();
        });

        PRINT_LOGS = new DebugCommand("print_logs", "prints logs", "print_log", () =>
        {
            console.OutputText(DebugManager.GetLogDisplay);
        });

        commandList = new List<object>
        {
            HELLO,
            HELP,
            //GET_PLAYER_CHARACTER_INFO,
            CLEAR_CONSOLE,
            PRINT_LOGS
        };
    }
    public bool HandleInput(string input)
    {
        string[] words = input.Trim().Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            var commandBase = commandList[i] as DebugCommandBase;

            if (string.Equals(words[0], commandBase?.commandId, StringComparison.OrdinalIgnoreCase))
            {
                DebugCommand? debugCommand;
                DebugCommand<int>? debugCommand_int;
                DebugCommand<int, int>? debugCommand_int_int;
                if ((debugCommand = commandList[i] as DebugCommand) is not null)
                {
                    (debugCommand).Invoke();
                    return true;
                }
                else if ((debugCommand_int = commandList[i] as DebugCommand<int>) is not null && int.TryParse(words[1], out int num1))
                {
                    (debugCommand_int).Invoke(num1);
                    return true;
                }
                else if ((debugCommand_int_int = commandList[i] as DebugCommand<int, int>) is not null && int.TryParse(words[1], out int _num1) && int.TryParse(words[2], out int _num2))
                {
                    (debugCommand_int_int).Invoke(_num1, _num2);
                    return true;
                }
            }
        }
        return false;
    }

    public ConsoleCommandsManager(DebugManager _debug_Manager, DebugConsole _console)
    {
        debugManager = _debug_Manager;
        console = _console;
        AddCommands();
    }
}