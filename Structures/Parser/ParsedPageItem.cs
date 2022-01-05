using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.Parser
{
    public class ParsedPageItem
    {
        /// <summary>
        /// Код товара.
        /// </summary>
        public double ProductCode { get; set; }

        /// <summary>
        /// Наименование товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на товар.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Количество отзывов.
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Средняя оценка товара, в процентах.
        /// </summary>
        public double SummaryStars { get; set; }
    }
}
