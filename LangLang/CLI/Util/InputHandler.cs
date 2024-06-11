using System;

namespace LangLang.CLI.Util;

public static class InputHandler
{
    public static string? ReadString(string? prompt = null)
    {
        if (prompt != null)
        {
            Console.Write(prompt);
        }
        return Console.ReadLine();
    }

    public static string ReadSecretString(string? prompt = null)
    {
        if (prompt != null)
        {
            Console.Write(prompt);
        }
        var secret = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (secret.Length > 0)
                {
                    secret = secret[..^1];
                    Console.Write("\b \b");
                }
            }
            else
            {
                secret += key.KeyChar;
                Console.Write("*");
            }
        }
        return secret;
    }
}