namespace LangLang.Application.Utility
{
    public static class ExamEmailConstants
    {
        public const string ResultEmailSubject = "{0} exam results";
        public const string ResultEmailBodyFail = @"Dear {0},

You have taken part in {1} exam on {2}. Unfortunatly, you have failed. However, don't worry, because you can always try again.

Warm regards,

LangLang school";

        public const string ResultEmailBodyPass = @"Dear {0},

You have taken part in {1} exam on {2}. Congratulations, you have passed.
Your grade on this exam is:
    - Reading: {3}
    - Writing: {4}
    - Listening: {5}
    - Speaking: {6}

Warm regards,

LangLang school";
    }
}
