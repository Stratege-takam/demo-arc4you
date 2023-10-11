using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common;

/// <summary>
/// Helper class for WPF styles and themes.
/// </summary>
public static class StyleHelper
{
    /// <summary>
    /// Prefix of a default style key.
    /// </summary>
    private const string DefaultKeyPrefix = "Default";

    /// <summary>
    /// Postfix of a default style key.
    /// </summary>
    private const string DefaultKeyPostfix = "Style";

    /// <summary>
    /// Creates style forwarders for default styles. This means that all styles found in the theme that are 
    /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
    /// control.
    /// 
    /// This method will use the current application (<see cref="System.Windows.Application.Current"/> to retrieve
    /// the resources. The forwarders will be written to the same dictionary.
    /// </summary>
    public static void CreateStyleForwardersForDefaultStyles()
    {
        CreateStyleForwardersForDefaultStyles(System.Windows.Application.Current.Resources);
    }

    /// <summary>
    /// Creates style forwarders for default styles. This means that all styles found in the theme that are 
    /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
    /// control.
    /// 
    /// This method will use the passed resources, but the forwarders will be written to the same dictionary as 
    /// the source dictionary.
    /// </summary>
    /// <param name="sourceResources">Resource dictionary to read the keys from (thus that contains the default styles).</param>
    /// <param name="targetResources">Resource dictionary where the forwarders will be written to.</param>
    public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary sourceResources)
    {
        CreateStyleForwardersForDefaultStyles(sourceResources, sourceResources);
    }

    /// <summary>
    /// Creates style forwarders for default styles. This means that all styles found in the theme that are 
    /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
    /// control.
    /// 
    /// This method will use the passed resources.
    /// </summary>
    /// <param name="sourceResources">Resource dictionary to read the keys from (thus that contains the default styles).</param>
    /// <param name="targetResources">Resource dictionary where the forwarders will be written to.</param>
    public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary sourceResources, ResourceDictionary targetResources)
    {
        // Get all keys from this resource dictionary
        var keys = from key in sourceResources.Keys as ICollection<object>
                    where key is string &&
                            ((string)key).StartsWith("Default", StringComparison.InvariantCulture) &&
                            ((string)key).EndsWith("Style", StringComparison.InvariantCulture)
                    select key;

        // Loop all keys
        foreach (string key in keys)
        {
            // Get the style
            Style style = sourceResources[key] as Style;

            // Is this a style?
            if (style != null)
            {
                // Get the style target type
                Type targetType = style.TargetType;
                if (targetType != null)
                {
                    // Yes, create a forwarder
                    Style styleForwarder = new Style(targetType, style);
                    targetResources.Add(targetType, styleForwarder);
                }
            }
        }

        // Loop all merged dictionaries as well
        foreach (ResourceDictionary resourceDictionary in sourceResources.MergedDictionaries)
        {
            CreateStyleForwardersForDefaultStyles(resourceDictionary, targetResources);
        }
    }
}