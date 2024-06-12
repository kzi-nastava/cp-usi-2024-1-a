using System;
using LangLang.CLI.Util;

namespace LangLang.CLI.Views;

public class TutorMenu : ICliMenu
{
    public void Show()
    {
        Console.WriteLine("=== Tutor menu ===");
        InputHandler.ReadString();
    }
}