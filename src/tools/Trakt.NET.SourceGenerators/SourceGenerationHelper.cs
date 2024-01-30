﻿using System.Globalization;
using System.Text;

namespace TraktNET.SourceGenerators
{
    internal static class SourceGenerationHelper
    {
        internal static string GenerateEnumExtensionClass(StringBuilder stringBuilder, in TraktEnumToGenerate enumToGenerate)
        {
            string fullyQualifiedName = $"global::{enumToGenerate.FullyQualifiedName}";

            stringBuilder.Clear();
            stringBuilder.Append(Constants.Header);
            stringBuilder.Append(@"
/// <summary>Extension methods for <see cref=""").Append(fullyQualifiedName).Append(@""" />.</summary>
");

            stringBuilder.Append(@"#if NET5_0_OR_GREATER
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = ""Generated by the Trakt.NET source generator."")]
#else
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif");
            stringBuilder.Append(@"
internal static partial class ").Append(enumToGenerate.CompleteName);
            stringBuilder.Append(@"
{");

            stringBuilder.Append(@"
    /// <summary>Returns the Json value for <see cref=""").Append(fullyQualifiedName).Append(@""" />.</summary>");
            stringBuilder.Append(@"
    internal static string? ToJson(this ").Append(fullyQualifiedName).Append(" value)");

            stringBuilder.Append(@"
        => value switch
        {");

            foreach (string member in enumToGenerate.Values)
            {
                if (member == "Unspecified")
                {
                    stringBuilder.Append(@"
            ").Append(fullyQualifiedName).Append('.').Append(member)
                .Append(" => null,");
                }
                else
                {
                    stringBuilder.Append(@"
            ").Append(fullyQualifiedName).Append('.').Append(member)
                    .Append(" => ").Append('"').Append(member.ToLower(CultureInfo.InvariantCulture)).Append(@""",");
                }
            }

            stringBuilder.Append(@"
            _ => null,");

            stringBuilder.Append(@"
        };");

            stringBuilder.Append(@"

    /// <summary>
    /// Returns a <see cref=""").Append(fullyQualifiedName).Append(@""" /> for the given value, if possible.
    /// <para />
    /// If not possible, the value <see cref=""").Append(fullyQualifiedName).Append(".Unspecified").Append(@""" /> will be returned.
    /// </summary>");
            stringBuilder.Append(@"
    internal static ").Append(fullyQualifiedName).Append(" To").Append(enumToGenerate.Name).Append("(this string? value)");

            stringBuilder.Append(@"
        => value switch
        {");

            foreach (string member in enumToGenerate.Values)
            {
                if (member == "Unspecified")
                    continue;

                stringBuilder.Append(@"
            ").Append($"\"{member.ToLower(CultureInfo.InvariantCulture)}\" => {fullyQualifiedName}.{member},");

                stringBuilder.Append(@"
            ").Append($"\"{member.ToUpper(CultureInfo.InvariantCulture)}\" => {fullyQualifiedName}.{member},");

                stringBuilder.Append(@"
            ").Append($"\"{member}\" => {fullyQualifiedName}.{member},");
            }

            stringBuilder.Append(@"
            _ => ").Append(fullyQualifiedName).Append(".Unspecified,");

            stringBuilder.Append(@"
        };");

            stringBuilder.Append(@"

    /// <summary>Returns the display name for <see cref=""").Append(fullyQualifiedName).Append(@""" />.</summary>");
            stringBuilder.Append(@"
    internal static string DisplayName(this ").Append(fullyQualifiedName).Append(" value)");

            stringBuilder.Append(@"
        => value switch
        {");

            foreach (string member in enumToGenerate.Values)
            {
                stringBuilder.Append(@"
            ").Append(fullyQualifiedName).Append('.').Append(member)
                .Append(" => ").Append('"').Append(member).Append(@""",");
            }

            stringBuilder.Append(@"
            _ => value.ToString(),");

            stringBuilder.Append(@"
        };");

            stringBuilder.Append(@"
}
");

            return stringBuilder.ToString();
        }
    }
}
