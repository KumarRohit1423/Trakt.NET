﻿namespace TraktNET.SourceGenerators
{
    internal static class Constants
    {
        internal const string Namespace = "TraktNET.SourceGenerators";

        internal const string Header = @"//-----------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Trakt.NET source generator.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------------------------------

#nullable enable
";

        internal const string ExcludeCodeCoverage = @"#if NET5_0_OR_GREATER
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = ""Generated by the Trakt.NET source generator."")]
#else
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif";
    }
}
