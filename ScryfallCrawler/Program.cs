using System.Text;
using System.Web;
using ScryfallCrawler;
using HtmlAgilityPack;

var urlMain = "https://scryfall.com/sets/pip";
var urls = new List<string>();
var cards = new List<Card>();
var html = new HtmlWeb().LoadFromWebAsync(urlMain).Result;
var mainDiv = html.DocumentNode.SelectNodes("//*[@class='card-grid-inner']");

Console.WriteLine("Crawler iniciado!");
// Seleciona todos os links <a> dentro das divs que estão dentro da div principal
foreach (var node in mainDiv)
{
    urls.AddRange(node.Descendants("a").Select(a => a.GetAttributeValue("href", string.Empty)));
}

Parallel.ForEach(urls, link =>
{
    var htmlCard = new HtmlWeb().LoadFromWebAsync(link).Result;

    var cardName = HttpUtility.HtmlDecode(htmlCard.DocumentNode.SelectSingleNode("//*[@class='card-text-title']").InnerText.Trim());
    cardName = System.Text.RegularExpressions.Regex.Replace(cardName, @"\s+", " ");
    var cardDesc = htmlCard.DocumentNode.SelectSingleNode("//*[@class='card-text-oracle']");
    // Seleciona todos os parágrafos dentro do cardDescNode
    var paragraphs = cardDesc.SelectNodes(".//p");
    
    // Concatena o texto de todos os parágrafos
    var cardDescText = string.Join(" ", paragraphs.Select(p => HttpUtility.HtmlDecode(p.InnerText.Trim().Replace("\r\n", " ")
        .Replace("\n", " ")
        .Replace("\r", " "))));
    cards.Add(new Card(cardName, cardDescText));
    
    
    html = null;
    GC.Collect();
});


    var csv = new StringBuilder();
    csv.AppendLine("NAME | DESCRIPTION");
    foreach (var card in cards)
    {
        csv.AppendLine($"{card.Nome} | {card.Descricao}");
    }
    File.WriteAllText("cards.csv", csv.ToString(), Encoding.UTF8);


Console.WriteLine("Crawler finalizado!");