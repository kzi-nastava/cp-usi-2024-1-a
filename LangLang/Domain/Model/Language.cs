using System;
using System.ComponentModel.DataAnnotations;

namespace LangLang.Domain.Model
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

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Language other = (Language) obj;
            return Code == other.Code;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Code);
        }
    }
}
