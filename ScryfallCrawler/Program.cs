using System.Globalization;
using ScryfallCrawler;
using HtmlAgilityPack;
using System.Web;

var mainUrl = "https://scryfall.com/sets/pblb";
var urls = new List<string>();
var cards = new List<Card>();
var html = new HtmlWeb().LoadFromWebAsync(mainUrl).Result;
