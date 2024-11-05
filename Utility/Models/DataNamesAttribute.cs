/*
 * 
 * Apply this attribute on class properties to be mapped  
 *      How to call :    [DataNames("first_name", "firstName")]
 *                       public string FirstName { get; set; } 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataNamesAttribute : Attribute
    {
        protected List<string> _valueNames { get; set; }

        public List<string> ValueNames
        {
            get
            {
                return _valueNames;
            }
            set
            {
                _valueNames = value; 
            }
        }

        public DataNamesAttribute()
        {
            _valueNames = new List<string>();
        }

        public DataNamesAttribute(params string[] valueNames)
        {
            _valueNames = valueNames.ToList();
        }
    }
}
