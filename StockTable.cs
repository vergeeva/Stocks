using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Валюты
{
    public class StockTable //для отображения купленных акций
    {
        public StockTable(String name, DateTime date_of_last_buy, int Count_of_stocks)
        {
            this.name = name;
            this.date_of_last_buy = date_of_last_buy;
            this.Count_of_stocks = Count_of_stocks;
        }
        public String name;
        public DateTime date_of_last_buy;
        public int Count_of_stocks;

    }

    public class LastStockCourse //хранение курса акция для прогноза
    {
        public String Name; //Наименование акции
        public double Yesterday; //курс вчера
        public double DayBeforeYesterday; //курс позавчера

        public LastStockCourse(String Name, double Yesterday, double DayBeforeYesterday)
        {
            this.Name = Name;
            this.Yesterday = Yesterday;
            this.DayBeforeYesterday = DayBeforeYesterday;
        }
    }


}

