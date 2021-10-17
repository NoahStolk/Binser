using Microsoft.CodeAnalysis.Text;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Binser.Sourcegen;

[Generator]
public class SerializerSourceGenerator : ISourceGenerator
{
	private const string _typeName = $"%{nameof(_typeName)}%";
	private const string _writeProperties = $"%{nameof(_writeProperties)}%";
	private const string _template = $@"using Binser.Data;
using Binser.Extensions;

namespace Binser;

public static class {_typeName}Serializer
{{
	public static byte[] Serialize({_typeName} obj)
	{{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		{_writeProperties}

		return ms.ToArray();
	}}
}}
";

	private readonly SerializerContext _serializerContext = new();

	public void Initialize(GeneratorInitializationContext context) => _serializerContext.FindTypes();

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (KeyValuePair<string, List<string>> type in _serializerContext.Types)
			GenerateSerializer(context, type.Key, type.Value);
	}

	private void GenerateSerializer(GeneratorExecutionContext context, string typeName, List<string> propertyNames)
	{
		List<string> writePropertyLines = new();
		foreach (string propertyName in propertyNames)
			writePropertyLines.Add($"bw.Write(obj.{propertyName});");

		string code = _template
			.Replace(_typeName, typeName)
			.Replace(_writeProperties, string.Join("\n", writePropertyLines));
		context.AddSource($"{typeName}Serializer", SourceText.From(code, Encoding.UTF8));
	}
}
