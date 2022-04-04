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
        Body body = new Body();
        double Balance = 100000;
        List<StockTable> Buy = new List<StockTable>();

        private const string RequestUri = "https://quote.rbc.ru/v5/ajax/catalog/get-tickers?type=share&sort=blue_chips&limit=200&offset=0";
        private const string RequestUri_Currency = "https://www.cbr-xml-daily.ru/daily_json.js";

        public int FindInDGV(DataGridView dgv, string name)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[0].Value.ToString() == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public int FindInStockTable(List<StockTable> st, string name)
        {
            for (int i = 0; i < st.Count; i++)
            {
                if (st[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public void ViewListInDGV(DataGridView dgv, List<StockTable> list)
        {
            if (list.Count == 0)
            {
                dgv.Rows[0].Cells[0].Value = "";
                dgv.Rows[0].Cells[1].Value = "";
                dgv.Rows[0].Cells[2].Value = "";
            }
            else
            {
                dgv.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    dgv.Rows[i].Cells[0].Value = list[i].name;
                    dgv.Rows[i].Cells[1].Value = list[i].date_of_last_buy;
                    dgv.Rows[i].Cells[2].Value = list[i].Count_of_stocks;
                }
            }
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

            //-----------------------------------------------------
            //Перевод в курс противоположную валюту
            HttpClient httpClient1 = new HttpClient();
            httpClient1.MaxResponseContentBufferSize = 2560000;

            HttpResponseMessage response1 = await httpClient1.GetAsync(RequestUri_Currency);
            response1.EnsureSuccessStatusCode();

            jsonString = await response1.Content.ReadAsStringAsync();

            body = JsonSerializer.Deserialize<Body>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (body != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "USD")
                    {
                        double temp_value = Math.Round(Convert.ToDouble(Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value) * body.valute.usd.value), 5);
                        dataGridView1.Rows[i].Cells[3].Value = temp_value;
                        dataGridView1.Rows[i].Cells[4].Value = "RUB";
                    }
                    else
                    {
                        double temp_value = Math.Round(Convert.ToDouble(Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value) / body.valute.usd.value), 5);
                        dataGridView1.Rows[i].Cells[3].Value = temp_value;
                        dataGridView1.Rows[i].Cells[4].Value = "USD";
                    }

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Скачать в бд список акций
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ограничения на ввод в текстовое поле, разрешены:
            //Цифры + работающий BackSpace
            if ((Char.IsNumber(e.KeyChar)  || e.KeyChar == '\b')) return;
            else
                e.Handled = true;
        }

        private void buttonBuy_Click(object sender, EventArgs e)
        {
            //купить акцию
            try
            {
                string name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int count = Convert.ToInt32(textBoxCount.Text);
                DateTime date = DateTime.Now;
                if (count == 0)
                {
                    MessageBox.Show("Ноль акций можно купить и без покупки", "Шутка-минутка про ноль акций");
                }
                else
                {
                    double price;
                    if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "USD")
                    {
                        price = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[3].Value) * count;
                    }
                    else
                    {
                        price = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[1].Value) * count;
                    }
                     
                    if (Balance > price)
                    {
                        Balance -= price;
                        textBoxBalance.Text = Balance.ToString();
                        dataGridView2.Rows.Add(name, date, count);

                        //Сделать без повторов
                        int ind = FindInStockTable(Buy, name);
                        if (ind != -1)
                        {
                            Buy[ind].date_of_last_buy = DateTime.Now;
                            Buy[ind].Count_of_stocks += count;
                            
                        }
                        else
                        {
                            Buy.Add(new StockTable(name, date, count));
                        }
                        ViewListInDGV(dataGridView2, Buy);
                        //перерисовать грид
                    }
                    else
                    {
                        MessageBox.Show("Денег нет, но вы держитесь", "Недостаточно средств для покупки");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо выделить строку в списке акций и заполнить текстовое поле количества акций", "Предупреждение");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Загрузка данных из базы данных
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nastya\Desktop\6сем\Сартасов_ВССИТ\Валюты\Database1.mdf;Integrated Security=True";
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(connectionString);

                SqlCommand cmnd = new SqlCommand("SELECT * FROM [КупленныеАкции]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                SqlDataReader reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader[1].ToString();
                    DateTime date = Convert.ToDateTime(reader[2]);
                    int count = Convert.ToInt32(reader[3]);
                    Buy.Add(new StockTable(name, date, count));
                }
                cmnd.Connection.Close();

                ViewListInDGV(dataGridView2, Buy);
                cmnd = new SqlCommand("SELECT * FROM [Баланс]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    Balance = Convert.ToDouble(reader[1]);
                }
                cmnd.Connection.Close();
                //Присвоение массиву Buy
                //Высвечиваем в грид 2
                //Вычитаем сумму цен акций из базового баланса
                textBoxBalance.Text = Balance.ToString();
            }
            catch
            {

            }
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            //Продать акцию
            try
            {
                string name = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                int count = Convert.ToInt32(textBoxCount.Text);
                int i = FindInStockTable(Buy, name); //Для поиска в массиве
                int index = FindInDGV(dataGridView1, name); //Для поиска в гриде 1
                double ActualPrice; //надо искать в гриде

                if (dataGridView1.Rows[index].Cells[2].Value.ToString() == "USD")
                {
                    ActualPrice = Convert.ToDouble(dataGridView1.Rows[index].Cells[3].Value);
                }
                else
                {
                    ActualPrice = Convert.ToDouble(dataGridView1.Rows[index].Cells[1].Value);
                }
                //Сверить количество акций
                if (count <= Buy[i].Count_of_stocks)
                {
                    Buy[i].Count_of_stocks -= count;
                    if (Buy[i].Count_of_stocks == 0)
                    {
                        MessageBox.Show(String.Format("Вы продали все акции компании {0}", Buy[i].name), "Недостаточно акций для продажи");
                        Buy.RemoveAt(i);
                        
                        //Высветить в грид массивчик
                    }
                    Balance += count * ActualPrice; //актуализируем баланс
                    textBoxBalance.Text = Balance.ToString(); //выводим в текстовое поле
                }
                else
                {
                    MessageBox.Show("Нельзя продать акций больше, чем есть", "Недостаточно акций для продажи");

                }
                ViewListInDGV(dataGridView2, Buy);
                //Если продаем меньше или столько же, сколько есть:
                //Продажа по текущему курсу
                //искать в соседней таблице такую акцию
                //Нашли, продали, количество*текущий курс
                //Прибавили к балансу
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо выделить строку в списке акций и заполнить текстовое поле количества акций", "Предупреждение");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //Загрузка купленных акций в базу данных
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nastya\Desktop\6сем\Сартасов_ВССИТ\Валюты\Database1.mdf;Integrated Security=True";
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(connectionString);

                SqlCommand cmnd = new SqlCommand("DELETE FROM [КупленныеАкции]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                sqlConnection1.Close();

                foreach (var element in Buy)
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [КупленныеАкции] (StockName, Date_of_last_buy, Count_of_buy) VALUES (@value1,@value2,@value3)", sqlConnection1);
                    command.Parameters.AddWithValue("@value1", element.name);
                    command.Parameters.AddWithValue("@value2", element.date_of_last_buy);
                    command.Parameters.AddWithValue("@value3", element.Count_of_stocks);

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection1.Close();
                }

                cmnd = new SqlCommand("DELETE FROM [Баланс]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                sqlConnection1.Close();

                cmnd = new SqlCommand("INSERT INTO [Баланс] (Balance) VALUES (@value1)", sqlConnection1);
                cmnd.Parameters.AddWithValue("@value1", Balance);

                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch
            {

            }
        }
    }
}
 