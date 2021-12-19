using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace indeed
{
    class Program
    {
        //prijsvergelijker
        static void comparePrices()
        {
            Console.Clear();
            Console.WriteLine("Find the best price!");
            Console.WriteLine("Search for a product: ");
            String zoekterm = Console.ReadLine();
            Console.WriteLine("Comparing prices from Coolblue, Bol and Amazon");

            // nieuw CSV bestand
            var csv = new StringBuilder();

            //open Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("log-level=3");
            options.AddArgument("--silent");
            IWebDriver driver = new ChromeDriver(@"C:\Users\robin\source\repos\indeed\bin\Debug", options);

            // ga naar Bol
            driver.Navigate().GoToUrl("https://www.bol.com/nl/nl/");
            var element = driver.FindElement(By.Id("searchfor"));
            element.SendKeys(zoekterm);
            element.Submit();
            var BolTitle = driver.FindElement(By.ClassName("product-title--inline"));
            var BolPrice = driver.FindElement(By.ClassName("price-block__highlight"));
            var BolLink =  driver.FindElement(By.ClassName("product-title"));
            Console.Clear();
            Console.WriteLine("Summary");
            Console.WriteLine("------- \n");
            Console.WriteLine("Bol price: ");
            Console.WriteLine(BolTitle.Text);
            Console.WriteLine(BolPrice.Text);
            Console.WriteLine(BolLink.GetAttribute("href"));
            Console.WriteLine("\n");
            var newLine = string.Format("Bol.com \n Titel: {0} \n prijs: {1} \n link: {2}  \n ", BolTitle.Text, BolPrice.Text, BolLink.GetAttribute("href"));
            csv.AppendLine(newLine);

            // ga naar Coolblue
            driver.Navigate().GoToUrl("https://www.coolblue.be/nl");
            var element2 = driver.FindElement(By.Id("search_query"));
            element2.SendKeys(zoekterm);
            element2.Submit();
            var coolblueTitle = driver.FindElement(By.ClassName("h3"));
            var CoolbluePrice = driver.FindElement(By.ClassName("sales-price__current"));
            var coolblueLink = driver.FindElement(By.ClassName("link"));
            Console.WriteLine("Coolblue Price: ");
            Console.WriteLine(coolblueTitle.Text);
            Console.WriteLine(CoolbluePrice.Text);
            Console.WriteLine(coolblueLink.GetAttribute("href"));
            Console.WriteLine("\n");
            var newLine2 = string.Format("coolblue.be \n Titel: {0} \n prijs: {1} \n link: {2}  \n ", coolblueTitle.Text, CoolbluePrice.Text, coolblueLink.GetAttribute("href"));
            csv.AppendLine(newLine2);

            // ga naar Amazon
            driver.Navigate().GoToUrl("https://www.amazon.de");
            var element3 = driver.FindElement(By.ClassName("nav-input"));
            element3.SendKeys(zoekterm);
            element3.Submit();
            var AmazonTitle = driver.FindElement(By.ClassName("a-size-mini"));
            var AmazonPrice = driver.FindElement(By.ClassName("a-price-whole"));
            var AmazonLink = driver.FindElement(By.ClassName("a-link-normal"));
            Console.WriteLine("Amazon Price: ");
            Console.WriteLine(AmazonTitle.Text);
            Console.WriteLine(AmazonPrice.Text);
            Console.WriteLine(AmazonLink.GetAttribute("href"));
            Console.WriteLine("\n");
            var newLine3 = string.Format("amazon.de \n Titel: {0} \n prijs: {1} \n link: {2}  \n ", AmazonTitle.Text, AmazonPrice.Text, AmazonLink.GetAttribute("href"));
            csv.AppendLine(newLine3);
            //pad naar output
            var filepath = "C:/Users/robin/prices.csv";

            //tekst in csv
            File.AppendAllText(filepath, csv.ToString());

            //succes bericht
            Console.WriteLine("U kan het bestand vinden in de map " + filepath.ToString());
            driver.Quit();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
        }
        //youtube scraper
        static void youtubeScraper()
        {
            // nieuw CSV bestand
            var csv = new StringBuilder();

            //open Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("log-level=3");
            options.AddArgument("--silent");
            IWebDriver driver = new ChromeDriver(@"C:\Users\robin\source\repos\indeed\bin\Debug", options);

            // ga naar youtube
            driver.Navigate().GoToUrl("https://www.youtube.com");

            //TOS accepteren
            Thread.Sleep(2000);
            var tos = driver.FindElement(By.XPath("/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[5]/div[2]/ytd-button-renderer[2]/a/tp-yt-paper-button"));
            tos.Click();
            //vind het zoekveld +zoekterm
            var element = driver.FindElement(By.Name("search_query"));
            Console.Clear();
            Console.WriteLine("Geef een zoekterm op: ");
            string zoekterm = Console.ReadLine();
            element.SendKeys(zoekterm);
            element.Submit();

            //filter
            var filter = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/div/ytd-toggle-button-renderer/a/tp-yt-paper-button"));
            filter.Click();
            var recent = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string"));
            recent.Click();
            Thread.Sleep(1000);

            //maak de Console leeg
            Console.Clear();

            //vind de juiste variabelen
            var titles = driver.FindElements(By.Id("title-wrapper"));
            var channelName = driver.FindElements(By.Id("channel-info"));
            var views = driver.FindElements(By.Id("metadata-line"));
            var url = driver.FindElements(By.Id("video-title"));
            int result = titles.Count();
            int count = 0;
            // for loop die de variabelen wegschrijft in csv
            for (var i = 0; i < result; i++)
            {
                if (count < 5)
                {
                    var title = titles[i].Text;
                    var channel = channelName[i].Text;
                    var viewCount = views[i].Text;
                    var link = url[i].GetAttribute("href");
                    var newLine = string.Format("Titel: {0} \n Kanaal: {1} \n Weergaven: {2}  \n link: {3} \n", title, channel, viewCount, link);
                    csv.AppendLine(newLine);
                    Thread.Sleep(500);
                }
                count++;

            }


            Console.WriteLine("----------------------------");


            //pad naar output
            var filepath = "C:/Users/robin/youtube.csv";

            //tekst in csv
            File.AppendAllText(filepath, csv.ToString());

            //succes bericht
            Console.WriteLine("U kan het bestand vinden in de map " + filepath.ToString());
            driver.Quit();
            Thread.Sleep(5000);
        }
        //indeed scraper
        static void indeedScraper()
        {
            // nieuw CSV bestand
            var csv = new StringBuilder();

            //open Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("log-level=3");
            options.AddArgument("--silent");
            IWebDriver driver = new ChromeDriver(@"C:\Users\robin\source\repos\indeed\bin\Debug", options);

            // ga naar Indeed
            driver.Navigate().GoToUrl("https://be.indeed.com");

            //vind het zoekveld +zoekterm
            var element = driver.FindElement(By.Id("text-input-what"));
            Console.Clear();
            Console.WriteLine("Geef een zoekterm op: ");
            string zoekterm = Console.ReadLine();
            element.SendKeys(zoekterm);
            element.Submit();

            //filter op 3 dagen
            var datum = driver.FindElement(By.CssSelector("#filter-dateposted"));
            datum.Click();
            var filter = driver.FindElement(By.CssSelector("li:nth-child(2)"));
            filter.Click();

            //irritante popup weghalen
            Thread.Sleep(2000);
            var popup = driver.FindElement(By.CssSelector(".popover-x-button-close"));
            popup.Click();
            Thread.Sleep(2000);


            //maak de Console leeg
            Console.Clear();

            //vind de juiste variabelen
            var titles = driver.FindElements(By.ClassName("jobTitle"));
            var companyNames = driver.FindElements(By.ClassName("companyName"));
            var companyLocations = driver.FindElements(By.ClassName("companyLocation"));
            var vacature = driver.FindElements(By.ClassName("tapItem"));
            int result = titles.Count();

            // for loop die de variabelen wegschrijft in csv
            Console.WriteLine("----------------------------");
            for (var i = 0; i < result; i++)
            {
                var title = titles[i].Text;
                var company = companyNames[i].Text;
                var location = companyLocations[i].Text;
                var link = vacature[i].GetAttribute("href");
                var newLine = string.Format("Titel: {0} \n Bedrijf: {1} \n Locatie: {2} \n link: {3} \n", title, company, location, link);
                csv.AppendLine(newLine);
            }

            //pad naar output
            var filepath = "C:/Users/robin/vacatures.csv";

            //tekst in csv
            File.AppendAllText(filepath, csv.ToString());

            //succes bericht
            Console.WriteLine("U kan het bestand vinden in de map " + filepath.ToString());
            driver.Quit();
            Thread.Sleep(5000);
        }
        //menu
        static bool keuzeMenu()
        {
            Console.Clear();
            Console.WriteLine("Webscraper Application:");
            Console.WriteLine("-----------------------");
            Console.WriteLine("1) Indeed Webscraper");
            Console.WriteLine("2) Youtube Webscraper");
            Console.WriteLine("3) Find the best price for a product");
            Console.WriteLine("4) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    indeedScraper();
                    return true;
                case "2":
                    youtubeScraper();
                    return true;
                case "3":
                    comparePrices();
                    return true;
                case "4":
                    return false;
                default:
                    return true;
            }
        }
        //main application
        static void Main(string[] args)
        {
            bool showMenu = true;
            while(showMenu){
                showMenu = keuzeMenu();
            }
        }
    }
}
    

