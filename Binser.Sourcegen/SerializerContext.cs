using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Binser.Sourcegen;

internal class SerializerContext
{
	public Dictionary<string, List<string>> Types { get; } = new();

	public void FindTypes()
	{
		Types.Clear();

		foreach (string csFilePath in Directory.GetFiles(@"C:\Users\NOAH\source\repos\Binser\Binser\Data"))
		{
			string typeName = Path.GetFileNameWithoutExtension(csFilePath);
			List<string> propertyNames = ExtractPropertyNames(CSharpSyntaxTree.ParseText(File.ReadAllText(csFilePath))).ToList();
			if (propertyNames.Count == 0)
				continue;

			Types.Add(typeName, propertyNames);
		}
	}

	private static IEnumerable<string> ExtractPropertyNames(SyntaxTree syntaxTree)
	{
		SyntaxNode root = syntaxTree.GetRoot();

		RecordDeclarationSyntax? rds = (RecordDeclarationSyntax)root.DescendantNodes().FirstOrDefault(sn => sn.IsKind(SyntaxKind.RecordDeclaration));
		if (rds == null)
			yield break;

		foreach (ParameterSyntax pds in rds.ParameterList?.Parameters ?? new())
			yield return pds.Identifier.ToString();
	}
}
