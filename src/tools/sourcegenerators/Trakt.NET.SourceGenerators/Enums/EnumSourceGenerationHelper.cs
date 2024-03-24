using System.Text;

namespace TraktNET.SourceGenerators.Enums
{
    internal static class EnumSourceGenerationHelper
    {
        private const string UnspecifiedValue = "Unspecified";
        private const string NoneValue = "None";

        internal static string GenerateEnumExtensionClass(StringBuilder stringBuilder, in TraktEnumToGenerate enumToGenerate)
        {
            stringBuilder.Clear();

            bool hasUnspecifiedMember = enumToGenerate.Values.Any(m => m.Name == UnspecifiedValue);
            bool hasNoneMember = enumToGenerate.Values.Any(m => m.Name == NoneValue);

            string invalidValueMember = string.Empty;

            if (hasUnspecifiedMember)
                invalidValueMember = UnspecifiedValue;
            else if (hasNoneMember)
                invalidValueMember = NoneValue;

            stringBuilder.Append(Constants.Header);
            stringBuilder.Append(@"
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TraktNET
{
    /// <summary>Extension methods for <see cref=""").Append(enumToGenerate.Name).Append(@""" />.</summary>
");

            stringBuilder.Append(Constants.ExcludeCodeCoverage);
            stringBuilder.Append(@"
    public static partial class ").Append(enumToGenerate.EnumExtensionClassName);
            stringBuilder.Append(@"
    {");

            stringBuilder.Append(@"
        /// <summary>Returns the Json value for <see cref=""").Append(enumToGenerate.Name).Append(@""" />.</summary>");
            stringBuilder.Append(@"
        public static string? ToJson(this ").Append(enumToGenerate.Name).Append(" value)");

            stringBuilder.Append(@"
            => value switch
            {");

            foreach (TraktEnumMemberToGenerate member in enumToGenerate.Values)
            {
                if ((member.Name == UnspecifiedValue || member.Name == NoneValue) && !member.HasTraktEnumMemberAttribute)
                {
                    stringBuilder.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member.Name)
                .Append(" => null,");
                }
                else if (member.HasTraktEnumMemberAttribute && member.JsonValue != null)
                {
                    stringBuilder.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member.Name)
                    .Append(" => ").Append('"').Append(member.JsonValue).Append(@""",");
                }
                else
                {
                    stringBuilder.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member.Name)
                    .Append(" => ").Append('"').Append(member.Name.ToLowercaseNamingConvention()).Append(@""",");
                }
            }

            stringBuilder.Append(@"
                _ => null,");

            stringBuilder.Append(@"
            };");

            stringBuilder.Append(@"

        /// <summary>Returns a <see cref=""").Append(enumToGenerate.Name).Append(@""" /> for the given value, if possible.</summary>");
            stringBuilder.Append(@"
        public static ").Append(enumToGenerate.Name).Append(" To").Append(enumToGenerate.Name).Append("(this string? value)");

            stringBuilder.Append(@"
            => value switch
            {");

            foreach (TraktEnumMemberToGenerate member in enumToGenerate.Values)
            {
                if (member.HasTraktEnumMemberAttribute && member.JsonValue != null)
                {
                    stringBuilder.Append(@"
                ").Append($"\"{member.JsonValue}\" => {enumToGenerate.Name}.{member.Name},");
                }
                else
                {
                    stringBuilder.Append(@"
                ").Append($"\"{member.Name.ToLowercaseNamingConvention()}\" => {enumToGenerate.Name}.{member.Name},");
                }
            }

            if (hasUnspecifiedMember)
            {
                stringBuilder.Append(@"
                _ => ").Append(enumToGenerate.Name).Append('.').Append(UnspecifiedValue).Append(',');
            }
            else if (hasNoneMember)
            {
                stringBuilder.Append(@"
                _ => ").Append(enumToGenerate.Name).Append('.').Append(NoneValue).Append(',');
            }

            stringBuilder.Append(@"
            };");

            stringBuilder.Append(@"

        /// <summary>Returns the display name for <see cref=""").Append(enumToGenerate.Name).Append(@""" />.</summary>");
            stringBuilder.Append(@"
        public static string DisplayName(this ").Append(enumToGenerate.Name).Append(" value)");

            if (enumToGenerate.HasFlagsAttribute)
            {
                stringBuilder.Append(@"
        {");

                stringBuilder.Append(@"
            var values = new List<string>();");

                foreach (TraktEnumMemberToGenerate member in enumToGenerate.Values)
                {
                    if (member.Name == invalidValueMember)
                    {
                        if (member.HasTraktEnumMemberAttribute && !string.IsNullOrWhiteSpace(member.DisplayName))
                        {
                            stringBuilder.Append(@"

            if (value == ").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@")
                values.Add(""").Append(member.DisplayName).Append(@""");");
                        }
                        else
                        {
                            stringBuilder.Append(@"

            if (value == ").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@")
                values.Add(""").Append(member.Name.ToDisplayName()).Append(@""");");
                        }
                    }
                    else
                    {
                        if (member.HasTraktEnumMemberAttribute && !string.IsNullOrWhiteSpace(member.DisplayName))
                        {
                            stringBuilder.Append(@"

            if (value.HasFlagSet(").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@"))
                values.Add(""").Append(member.DisplayName).Append(@""");");
                        }
                        else
                        {
                            stringBuilder.Append(@"

            if (value.HasFlagSet(").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@"))
                values.Add(""").Append(member.Name.ToDisplayName()).Append(@""");");
                        }
                    }
                }

                stringBuilder.Append(@"

            return string.Join("", "", values);
        }");
            }
            else
            {
                stringBuilder.Append(@"
            => value switch
            {");

                foreach (TraktEnumMemberToGenerate member in enumToGenerate.Values)
                {
                    if (member.HasTraktEnumMemberAttribute && !string.IsNullOrWhiteSpace(member.DisplayName))
                    {
                        stringBuilder.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(" => ").Append('"').Append(member.DisplayName).Append(@""",");
                    }
                    else
                    {
                        stringBuilder.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(" => ").Append('"').Append(member.Name.ToDisplayName()).Append(@""",");
                    }
                }

                stringBuilder.Append(@"
                _ => value.ToString(),");

                stringBuilder.Append(@"
            };");
            }

            if (enumToGenerate.HasFlagsAttribute)
            {
                stringBuilder.Append(@"

        /// <summary>Determines whether one or more bit fields are set in <see cref=""").Append(enumToGenerate.Name).Append(@""" />.</summary>
        public static bool HasFlagSet(this ").Append(enumToGenerate.Name).Append(" value, ").Append(enumToGenerate.Name).Append(@" flag)
            => flag == 0 ? true : (value & flag) == flag;");
            }

            if (enumToGenerate.HasParameterEnumAttribute)
            {
                stringBuilder.Append(@"

        /// <summary>Converts a <see cref=""").Append(enumToGenerate.Name).Append(@""" /> to a valid URI path value.</summary>
        public static string ToUriPath(this ").Append(enumToGenerate.Name).Append(@" value)
        {");
                if (hasUnspecifiedMember || hasNoneMember)
                {
                    stringBuilder.Append(@"
            if (value == ").Append(enumToGenerate.Name).Append('.').Append(invalidValueMember).Append(@")
                return string.Empty;
");
                }

                if (enumToGenerate.HasFlagsAttribute)
                {
                    stringBuilder.Append(@"
            var values = new List<string>();");

                    foreach (TraktEnumMemberToGenerate member in enumToGenerate.Values)
                    {
                        if (member.Name == invalidValueMember)
                            continue;

                        stringBuilder.Append(@"

            if (value.HasFlagSet(").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@"))
                values.Add(").Append(enumToGenerate.Name).Append('.').Append(member.Name).Append(@".ToJson()!);");
                    }

                    stringBuilder.Append(@"

            return ");

                    if (!string.IsNullOrWhiteSpace(enumToGenerate.ParameterEnumAttributeValue))
                        stringBuilder.Append('"').Append(enumToGenerate.ParameterEnumAttributeValue).Append(@"="" + ");

                    stringBuilder.Append(@"string.Join("","", values);");
                }
                else
                {
                    stringBuilder.Append(@"
            return ");

                    if (!string.IsNullOrWhiteSpace(enumToGenerate.ParameterEnumAttributeValue))
                        stringBuilder.Append('"').Append(enumToGenerate.ParameterEnumAttributeValue).Append(@"="" + ");

                    stringBuilder.Append(@"value.ToJson();");
                }

                stringBuilder.Append(@"
        }");
            }

            stringBuilder.Append(@"
    }
");

            stringBuilder.Append(@"
    /// <summary>JSON converter for <see cref=""").Append(enumToGenerate.Name).Append(@""" />.</summary>
");
            stringBuilder.Append(Constants.ExcludeCodeCoverage);
            stringBuilder.Append(@"
    public sealed class ").Append(enumToGenerate.Name).Append("JsonConverter : JsonConverter<").Append(enumToGenerate.Name).Append(@">
    {");

            stringBuilder.Append(@"
        public override bool CanConvert(Type typeToConvert) => typeof(").Append(enumToGenerate.Name).Append(@") == typeToConvert;

        public override ").Append(enumToGenerate.Name).Append(@" Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? enumValue = reader.GetString();
            return string.IsNullOrEmpty(enumValue) ? default : enumValue.To").Append(enumToGenerate.Name).Append(@"();
        }

        public override void Write(Utf8JsonWriter writer, ").Append(enumToGenerate.Name).Append(@" value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToJson());");

            stringBuilder.Append(@"
    }
}
");

            return stringBuilder.ToString();
        }
    }
}
