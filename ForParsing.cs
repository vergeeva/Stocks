using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Data.SqlClient;

namespace Валюты
{//------------------ДЛЯ ТЕКУЩЕГО КУРСА АКЦИЙ----------------------------------
    public class Stock
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; } 

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("company")]
        public Company? Company { get; set; }
    }

    public class Company
    { //Для русского языка
        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }

    //---------------ДЛЯ КУРСА ДОЛЛАРА---------------------------------------------

    public class Body
    {
        [JsonPropertyName("Valute")]
        public Valute? valute { get; set; }
    }
    public class Valute
    {
        [JsonPropertyName("USD")]
        public USD? usd { get; set; }
    }

    public class USD
    {
        [JsonPropertyName("Value")]
        public double? value { get; set; }

    }
}
