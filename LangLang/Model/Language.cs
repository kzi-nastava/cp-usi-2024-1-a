using Consts;
using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Language
    {
        private string name;
        private string code;

        public string Name { get; set; }
        public string Code { get; set; }

        public Language(string name, string code)
        {
            Name = name;
            Code = code;
        }

    }
}
