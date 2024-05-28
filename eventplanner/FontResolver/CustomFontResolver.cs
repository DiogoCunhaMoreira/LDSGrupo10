using PdfSharp.Fonts;
using System.IO;
using System.Reflection;


// Implementação de um CustomFontResolver para uso de fontes personalizadas
public class CustomFontResolver : IFontResolver
{
    public string DefaultFontName => "Sedan";

    // Método para resolver o tipo de fonte
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        string suffix = "";
        if (isBold)
            suffix += "Bold";
        else if (isItalic)
            suffix += "Italic";
        else
            suffix += "Regular";

        // esta fonte coicinde com o nome do arquivo da fonte
        string fontName = $"{familyName}-{suffix}.ttf";

        return new FontResolverInfo(fontName);
    }

    // Método para obter a fonte
    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = $"eventplanner.wwwroot.Fonts.{faceName}";
        using (var stream = assembly.GetManifestResourceStream(resourcePath))
        {
            if (stream == null)
                throw new FileNotFoundException($"Fonte '{resourcePath}' não encontrada.");

            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return data;
        }
    }
}
