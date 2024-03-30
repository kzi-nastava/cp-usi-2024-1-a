﻿using Consts;
using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Language
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Language()
        {
            Name = Code = "";
        }
        
        public Language(string name, string code)
        {
            Name = name;
            Code = code;
        }

    }
}
