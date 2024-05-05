using PdfSharp.Fonts;
using System.IO;
using System.Reflection;

public class CustomFontResolver : IFontResolver
{
    public string DefaultFontName => "Sedan";

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        string suffix = "";
        if (isBold)
            suffix += "Bold";
        else if (isItalic)
            suffix += "Italic";
        else
            suffix += "Regular";

        // The font name matched with the embedded resource name
        string fontName = $"{familyName}-{suffix}.ttf";

        return new FontResolverInfo(fontName);
    }

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
