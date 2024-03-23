﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace TraktNET.SourceGenerators
{
    [Generator]
    public class TraktSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(Constants.GeneratedTraktEnumAttributeFilename,
                SourceText.From(Constants.TraktEnumAttribute, Encoding.UTF8)));

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(Constants.GeneratedTraktEnumMemberJsonValueAttributeFilename,
                SourceText.From(Constants.TraktEnumMemberJsonValueAttribute, Encoding.UTF8)));

            IncrementalValuesProvider<TraktEnumToGenerate?> enumValuesProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    Constants.FullTraktEnumAttributeName,
                    predicate: static (syntaxNode, _) => syntaxNode is EnumDeclarationSyntax,
                    transform: GetEnumTypeToGenerate
                )
                .WithTrackingName(Constants.TrackingNames.InitialEnumExtraction)
                .Where(static syntaxNode => syntaxNode is not null)
                .WithTrackingName(Constants.TrackingNames.NullFilteredEnums);

            IncrementalValueProvider<(Compilation Left, ImmutableArray<TraktEnumToGenerate?> Right)> compilation =
                context.CompilationProvider.Combine(enumValuesProvider.Collect());

            context.RegisterSourceOutput(compilation, Execute);
        }

        private static TraktEnumToGenerate? GetEnumTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
        {
            if (context.TargetSymbol is not INamedTypeSymbol enumSymbol)
                return null;

            cancellationToken.ThrowIfCancellationRequested();

            string enumExtensionName = enumSymbol.Name + "Extensions";
            ImmutableArray<ISymbol> enumMembers = enumSymbol.GetMembers();
            var members = new List<TraktEnumMemberToGenerate>(enumMembers.Length);

            foreach (ISymbol member in enumMembers)
            {
                if (member is not IFieldSymbol field || field.ConstantValue is null)
                    continue;

                bool hasAttribute = false;
                string? jsonValue = string.Empty;
                string? displayName = string.Empty;

                AttributeData? attribute = field.GetAttributes()
                    .FirstOrDefault(attr => attr?.AttributeClass?.ToDisplayString() == Constants.FullTraktEnumMemberJsonValueAttributeName);

                if (attribute != null)
                {
                    hasAttribute = true;

                    ImmutableArray<TypedConstant> constructorArguments = attribute.ConstructorArguments;
                    jsonValue = constructorArguments[0].Value as string;

                    var namedArguments = attribute.NamedArguments.ToImmutableDictionary();

                    if (namedArguments.TryGetValue(Constants.TraktEnumMemberJsonValuePropertyDisplayName, out TypedConstant displayNameConstant))
                    {
                        if (!displayNameConstant.IsNull)
                        {
                            displayName = displayNameConstant.Value as string;
                        }
                    }
                }

                members.Add(new TraktEnumMemberToGenerate(field.Name, hasAttribute, jsonValue!, displayName!));
            }

            return new TraktEnumToGenerate(enumSymbol.Name, enumExtensionName, members);
        }

        private static void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<TraktEnumToGenerate?> Right) tuple)
        {
            (Compilation _, ImmutableArray<TraktEnumToGenerate?> enumValues) = tuple;

            var stringBuilder = new StringBuilder();

            foreach (TraktEnumToGenerate? enumToGenerate in enumValues)
            {
                if (enumToGenerate is { } enumToBeGenerated)
                {
                    string enumExtensionContent = SourceGenerationHelper.GenerateEnumExtensionClass(stringBuilder, in enumToBeGenerated);

                    context.AddSource(enumToGenerate.Value.Name + Constants.GeneratedTraktEnumFileExtension,
                        SourceText.From(enumExtensionContent, Encoding.UTF8));
                }
            }
        }
    }
}
