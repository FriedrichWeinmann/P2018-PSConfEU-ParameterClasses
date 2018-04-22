using System;
using System.Collections.Generic;
using System.Text;

namespace ParameterClasses
{
    /// <summary>
    /// Attribute marking a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ParameterClassAttribute : Attribute
    {
    }
}
