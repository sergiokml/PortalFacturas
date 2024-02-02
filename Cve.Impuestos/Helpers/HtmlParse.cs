using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Cve.Impuestos.Helpers
{
    internal class HtmlParse
    {
        // No se puede usar Tuple, porque debo usar las claves de los Dictionary.
        public static async Task<Dictionary<string, string>> GetValuesFromTabla(
            string tablepath,
            HttpResponseMessage msg,
            CancellationToken token
        )
        {
            if (msg is null)
            {
                return null!;
            }

            Dictionary<string, string> dics = new();
            HtmlParser? parser = new();
            IHtmlDocument? document = await parser.ParseDocumentAsync(
                await msg.Content.ReadAsStreamAsync(),
                token
            );
            try
            {
                await GuardarHtml(msg);
                IElement? tbody = document.DocumentElement.QuerySelector(tablepath);
                if (tbody != null)
                {
                    IHtmlCollection<IElement>? tr = tbody!.QuerySelectorAll("tr");
                    foreach (IElement? item in tr)
                    {
                        IHtmlCollection<IElement>? td = item.QuerySelectorAll("td");
                        dics.Add(td[0].TextContent.Trim(), td[1].TextContent.Trim());
                    }
                    return dics;
                }
                return null!;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public static async Task<Dictionary<string, string>> GetValuesFromTag(
            string tablepath,
            HttpResponseMessage? msg,
            CancellationToken token
        )
        {
            Dictionary<string, string> dics = new();
            HtmlParser? parser = new();
            IHtmlDocument? document = await parser.ParseDocumentAsync(
                await msg!.Content.ReadAsStreamAsync(),
                token
            );
            try
            {
                await GuardarHtml(msg);
                IHtmlCollection<IElement>? res = document.QuerySelectorAll(tablepath);
                if (res.Any())
                {
                    foreach (IHtmlInputElement? item in res)
                    {
                        _ = dics.TryAdd(item!.Name!, item.Value);
                    }
                }
                else
                {
                    IHtmlCollection<IElement> childs = document
                        .DocumentElement
                        .LastElementChild!
                        .Children;
                    foreach (IElement child in childs)
                    {
                        // EXTRAER ERRORES DEL HTML
                        foreach (IElement item in child.Children)
                        {
                            if (item.NodeName == "TABLE")
                            {
                                dics.Add(item.TagName, item.TextContent.Trim());
                            }
                        }
                    }
                }
                return dics;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        private static async Task GuardarHtml(HttpResponseMessage? msg)
        {
            await File.WriteAllBytesAsync(
                $"{Path.GetTempPath()}Html_resultado_{DateTime.Now:dd-MM-HH-mm-ss}.html",
                await msg!.Content.ReadAsByteArrayAsync()
            );
        }
    }
}
