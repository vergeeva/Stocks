using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace Валюты
{
    public partial class Form1 : Form
    {
        private const string RequestUri = "https://quote.rbc.ru/v5/ajax/catalog/get-tickers?type=share&sort=blue_chips&limit=200&offset=0";

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
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private async void button1_Click(object sender, EventArgs e)
        {//Загрузить данные в грид с сайта
            dataGridView1.Rows.Clear();
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 2560000;

            HttpResponseMessage response = await httpClient.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync();



            var stocks = JsonSerializer.Deserialize<List<Stock>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (stocks != null)
            {
                foreach (var element in stocks!)
                {
                    dataGridView1.Rows.Add(element.Company.Title, element.Price, element.Currency);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Скачать в бд
            
        }
    }
}
