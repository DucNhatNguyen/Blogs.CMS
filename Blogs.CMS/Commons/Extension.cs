﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blogs.CMS.Commons
{
    public static class Extension
    {
        public static string? GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;

        }
    }
}
