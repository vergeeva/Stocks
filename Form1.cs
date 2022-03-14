using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Data.SqlClient;

namespace Валюты
{
    public partial class Form1 : Form
    {
        List<Stock> stocks = new List<Stock>();
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



            stocks = JsonSerializer.Deserialize<List<Stock>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nastya\Desktop\6сем\Сартасов_ВССИТ\Валюты\Database1.mdf;Integrated Security=True";
            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(connectionString);

            SqlCommand cmnd = new SqlCommand("DELETE FROM [Table]", sqlConnection1);
            cmnd.Connection.Open();
            cmnd.ExecuteNonQuery();
            sqlConnection1.Close();

            foreach (var element in stocks)
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Table] (NameOfCompany, StockPrice, Currency) VALUES (@value1,@value2,@value3)", sqlConnection1);
                command.Parameters.AddWithValue("@value1", element.Company.Title);
                command.Parameters.AddWithValue("@value2", element.Price);
                command.Parameters.AddWithValue("@value3", element.Currency);

                command.Connection.Open();
                command.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            MessageBox.Show("Сохранение в базу данных выполнено!");
        }
    }
}
