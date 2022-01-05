using System.Collections.Generic;
using DBConnector.Postgree;
using LinqToDB;
using LinqToDB.Data;
using Postgree;
using Structures.Parser;

namespace DBConnector
{
    public static class DBHelper
    {
        public static void FlushParsePage(List<ParsedPageItem> parsedPageItems, string category, string siteName)
        {
            DataConnection.DefaultSettings = new MySettings();
            using var db = new PostgresDB();
            using var tr = db.BeginTransaction();
            {
                foreach (var item in parsedPageItems)
                {
                    db.InsertOrReplace(new ParseResult()
                    {
                        LinkMarket = item.Link,
                        Name = item.Name,
                        Price = item.Price,
                        ProductCode = item.ProductCode,
                        ReviewCount = item.ReviewCount,
                        SummaryStars = item.SummaryStars,
                        Category = category,
                        SiteName = siteName
                    });
                }
                tr.Commit();
            }
        }

    }
}
