using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Parser
{
    public class HozDomParser : ParserBase
    {
        internal const string ReviewCountSelector = "a8p2";
        internal const string SearchResultsContainerClassName = "widget-search-result-container";
        internal const string DivTagName = "div";
        internal const string CardsTableClassName = "bi";
        internal const string ReviewSummaryClassName = "ui-a0c";
        internal const string StyleAttributeSelector = "style";
        internal const string ProductPageClassName = "b0c8";

        internal override double GetProductCode()
        {
            var productCodeString = new string(Driver.FindElement(By.ClassName("fk1")).Text.Where(c => char.IsDigit(c) || c == '.').ToArray()).Replace(".", ",");
            return double.Parse(productCodeString);
        }

        internal override IReadOnlyCollection<IWebElement> GetCards()
        {
            var container = Driver.FindElement(By.ClassName(SearchResultsContainerClassName)).FindElement(By.TagName(DivTagName));
            var cards = container.FindElements(By.ClassName(CardsTableClassName));

            return cards;
        }

        internal override int GetReviewCount(IWebElement card)
        {
            var reviewCountElement = card.FindElement(By.ClassName(ReviewCountSelector));
            return int.Parse(new string(reviewCountElement.Text.Where(c => char.IsDigit(c)).ToArray()));
        }

        internal override double GetSummaryStars(IWebElement card)
        {
            var reviewSummary = card.FindElement(By.ClassName(ReviewSummaryClassName)).GetAttribute(StyleAttributeSelector);
            var summaryStarsString = new string(reviewSummary.Where(c => char.IsDigit(c) || c == '.').ToArray()).Replace(".", ",");
            return double.Parse(summaryStarsString);
        }

        internal override void NavigateToProductPageAndWait(IWebElement card)
        {
            var productPage = card.FindElement(By.ClassName(ProductPageClassName));
            Actions action = new(Driver);
            action.KeyDown(Keys.Control).MoveToElement(productPage).Click().Perform();
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Thread.Sleep(4000);
        }

        public HozDomParser(string parseText)
        {
            var uribuilder = new UriBuilder()
            {
                Scheme = "https",
                Host = "www.ozon.ru",
                Path = "search/",
                Query = $"deny_category_prediction=true&from_global=true&rating=t&sorting=rating&text={parseText}"
            };

            this.ParseUrl = uribuilder.Uri.ToString();
            this.ParseText = parseText;

            CardNameClassName = "bj5";
            CardPriceClassName = "ui-q1";
            NextPageTextSelector = "Дальше";
        }
    }
}
