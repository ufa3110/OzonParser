using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Structures.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Parser
{
    public abstract class ParserBase
    {
        private static readonly ChromeOptions chromeOptions = new();
        internal static IWebDriver Driver = new ChromeDriver(chromeOptions);

        internal string ParseUrl { get; set; }

        internal string ParseText { get; set; }

        internal string CardNameClassName = "";

        internal string CardPriceClassName = "";

        internal string NextPageTextSelector = "";

        public void Open()
        {
            Driver.Navigate().GoToUrl(this.ParseUrl);
            Thread.Sleep(2000);

            double elementsCounter = 0;
            while (true)
            {
                try
                {
                    var results = ParsePage();
                    DBConnector.DBHelper.FlushParsePage(results, ParseText);
                    Driver.FindElement(By.LinkText(NextPageTextSelector)).Click();
                    elementsCounter += results.Count;
                    Console.WriteLine($"finded and flushed:{elementsCounter}");

                    Driver.SwitchTo().Window(Driver.WindowHandles.First());
                    Driver.Close();
                }
                catch (Exception ex)
                {
                    Driver.Quit();
                    throw new Exception("Ошибка в основном цикле парсера", ex);
                }
            }
        }

        public List<ParsedPageItem> ParsePage()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Thread.Sleep(2000);
            var cards = GetCards();

            var pageItems = new List<ParsedPageItem>();

            foreach (var card in cards)
            {
                try
                {
                    var item = new ParsedPageItem
                    {
                        Name = card.FindElement(By.ClassName(CardNameClassName)).Text,
                        Price = card.FindElement(By.ClassName(CardPriceClassName)).Text
                    };

                    item.ReviewCount = GetReviewCount(card);
                    item.SummaryStars = GetSummaryStars(card);
                    NavigateToProductPageAndWait(card);
                    
                    item.Link = Driver.Url;
                    item.ProductCode = GetProductCode();

                    CloseLastTabAndWait(1500);
                    pageItems.Add(item);
                }
                catch
                {
                    CloseLastTabAndWait(4000);
                }
            }
            return pageItems;
        }

        internal virtual double GetProductCode()
        {
            throw new NotImplementedException();
        }

        internal virtual void NavigateToProductPageAndWait(IWebElement card)
        {
            throw new NotImplementedException();
        }

        internal virtual double GetSummaryStars(IWebElement card)
        {
            throw new NotImplementedException();
        }

        internal virtual int GetReviewCount(IWebElement card)
        {
            throw new NotImplementedException();
        }

        internal virtual IReadOnlyCollection<IWebElement> GetCards()
        {
            throw new NotImplementedException();
        }

        private static void CloseLastTabAndWait(int waitTime)
        {
            Driver.Close();
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Thread.Sleep(waitTime);
        }

    }
}
