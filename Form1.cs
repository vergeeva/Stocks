using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Linq;

namespace Валюты
{
    public partial class Form1 : Form
    { //------------ОБЪЕКТЫ ДЛЯ РАБОТЫ С АКЦИЯМИ--------------------
        List<Stock> stocks = new List<Stock>(); //для загрузки акций из джейсон файла
        Body body = new Body(); //для выгрузки курса акций
        double Balance = 1000000; //Для хранения баланса
        List<StockTable> Buy = new List<StockTable>(); //Купленные акции
        List<LastStockCourse> OldCourse = new List<LastStockCourse>(); //Прошлые курсы акций
        Dictionary<string, double> NamePrice = new Dictionary<string, double>(); //Для хранения наименования и цены нового курса
        Dictionary<String, double> Predict = new Dictionary<string, double>(); //для сохранения прогноза для всех акций
        Dictionary<String, double> PredictFormula = new Dictionary<string, double>(); //для сохранения прогноза по формуле для всех акций
        Dictionary<String, double> PredictBuyFormula = new Dictionary<string, double>(); //Прогноз для купленных акций по формулe
        Dictionary<String, double> PredictBuy = new Dictionary<string, double>(); //Прогноз для купленных акций
        const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nastya\Desktop\6сем\Сартасов_ВССИТ\Валюты\Database1.mdf;Integrated Security=True";

        private const string RequestUri = "https://quote.rbc.ru/v5/ajax/catalog/get-tickers?type=share&sort=blue_chips&limit=200&offset=0";
        private const string RequestUri_Currency = "https://www.cbr-xml-daily.ru/daily_json.js";

        //-------НАЙТИ В ГРИДЕ ПО ПЕРВОМУ СТОЛБЦУ---------
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

        //-----------НАЙТИ В СПИСКЕ КУПЛЕННЫХ АКЦИЙ----------------
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

        //-------НАЙТИ В АРХИВНОМ КУРСЕ АКЦИЙ--------------------------------
        public int FindInLastStockCourse(List<LastStockCourse> st, string name)
        {
            for (int i = 0; i < st.Count; i++)
            {
                if (st[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        //------------ВЫСВЕТИТЬ КУПЛЕННЫЕ АКЦИИ В ГРИД------------------------
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

        //СОБЫТИЕ ЗАГРУЗКИ ДАННЫХ С САЙТА СПИСКА ВСЕХ АКЦИЙ + ПЕРЕВОДА В ПРОТИВОПОЛОЖНУЮ ВАЛЮТУ-----
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
                    NamePrice.Add(element.Company.Title, element.Price);
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
                        double temp_value = Math.Round((double)((double)dataGridView1.Rows[i].Cells[1].Value * body.valute.usd.value), 5);
                        dataGridView1.Rows[i].Cells[3].Value = temp_value;
                        dataGridView1.Rows[i].Cells[4].Value = "RUB";
                    }
                    else
                    {
                        double temp_value = Math.Round((double)((double)dataGridView1.Rows[i].Cells[1].Value / body.valute.usd.value), 5);
                        dataGridView1.Rows[i].Cells[3].Value = temp_value;
                        dataGridView1.Rows[i].Cells[4].Value = "USD";
                    }

                }

            }
            button1.Enabled = true;
            button2.Enabled = true;
            buttonBuy.Enabled = true;
            buttonSell.Enabled = true;
            dataGridView1.AutoResizeColumns();
        }

        //КНОПКА "СДЕЛАТЬ ПРОГНОЗ"------------------------------------
        private void button2_Click(object sender, EventArgs e)
        { //сделать прогноз
            //Скачать в бд список акций - для второго задания оставлено
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

            //-------------------ПРОГНОЗ---------------------------------------------------
            try
            {
                Predict.Clear();
                PredictBuy.Clear();
                int indexInGrid;
                double YesterdayPrice;
                double DayBeforeYesterdayPrice;
                foreach (var element in stocks) //так как грид составлен по объекту Стокс
                {
                    double ValuePredict; //значение предсказания для текущей компании
                    double ActualPrice = element.Price; //актуальная цена

                    int i = FindInLastStockCourse(OldCourse, element.Company.Title); //ищем акцию в старом курсе

                    if (element.Currency == "USD")
                    {//если в долларах, переводим в рубли
                        ActualPrice = (double)(element.Price * body.valute.usd.value);
                        YesterdayPrice = (double)(OldCourse[i].Yesterday * body.valute.usd.value); //вчерашняя цена
                        DayBeforeYesterdayPrice = (double)(OldCourse[i].DayBeforeYesterday * body.valute.usd.value);//Позавчерашняя цена
                       
                    }
                    else
                    {//иначе оставляем рубли
                        YesterdayPrice = OldCourse[i].Yesterday; //вчерашняя цена
                        DayBeforeYesterdayPrice = OldCourse[i].DayBeforeYesterday;//Позавчерашняя цена
                    }

                    //Прогноз = 3 * курс_сегодня - 3 * курс_вчера + курс_позавчера
                    ValuePredict = Math.Round((3 * ActualPrice - 3 * YesterdayPrice + DayBeforeYesterdayPrice) - ActualPrice, 3);
                    indexInGrid = FindInDGV(dataGridView1, element.Company.Title);//ищем в таблице нужную компанию
                    Predict.Add(element.Company.Title, ValuePredict);//записываем значение в словарь
                    dataGridView1.Rows[indexInGrid].Cells[5].Value = ValuePredict; //высвечиваем в грид
                }

                //заполняем скрытый столбец прогноза в списке купленных акций
                for (int i = 0; i < Buy.Count; i++)
                {
                    int iBuy = FindInStockTable(Buy, dataGridView2.Rows[i].Cells[0].Value.ToString()); //ищем, так как грид может быть сортированным
                    dataGridView2.Rows[i].Cells[3].Value = Predict[Buy[iBuy].name]; //высвечиваем в грид
                    try
                    {
                        PredictBuy.Add(Buy[iBuy].name, Predict[Buy[iBuy].name]); //записываем в словарь
                    }
                    catch
                    {
                        PredictBuy[Buy[iBuy].name] = Predict[Buy[iBuy].name];
                    } //избегать ошибки повторений
                }
            }
            catch { throw; }
            buttonBuyAuto.Enabled = true;
            buttonSellAuto.Enabled = true;
            SimplePrediction.Checked = true;
        }

        //-------ЗАПРЕТ НА ВВОД БУКВ И ЗНАКОВ В ТЕКСТ БОКС------------------------------------
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ограничения на ввод в текстовое поле, разрешены:
            //Цифры + работающий BackSpace
            if ((Char.IsNumber(e.KeyChar)  || e.KeyChar == '\b')) return;
            else
                e.Handled = true;
        }

        //---------------КНОПКА "КУПИТЬ"---------------------------------------------------
        private void buttonBuy_Click(object sender, EventArgs e)
        {
            //купить акцию
            try
            {      
                buttonBuyAuto.Enabled = false;
                buttonSellAuto.Enabled = false;
                string name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString(); //читаем имя выделенной строки
                int count = Convert.ToInt32(textBoxCount.Text); //читаем количество из текстового поля
                DateTime date = DateTime.Now; //дату покупки будем записывать в реальном времени
                if (count == 0)
                {//если по ошибке введен ноль
                    MessageBox.Show("Введите осмысленное число акций", "Нельзя купить ноль акций");
                }
                else
                {//если же все в порядке
                    double price;
                    if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "USD")
                    {//если валюта в долларах, то берем курс в рублях
                        price = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[3].Value) * count;
                    }
                    else
                    {//иначе так и берем в рублях
                        price = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[1].Value) * count;
                    }
                     
                    if (Balance > price)
                    {//если денег у пользователя больше, чем стоимость желаемых акций
                        Balance -= price; //вычитаем стоимость из баланса
                        textBoxBalance.Text = Balance.ToString(); //обновляем баланс на форме
                        dataGridView2.Rows.Add(name, date, count);//добавляем акцию в список купленных акций

                        //Чтобы было без повторов
                        int ind = FindInStockTable(Buy, name);
                        //используем вспомогательную функцию поиска в гриде
                        if (ind != -1)
                        {//если такая акция уже имеется
                            Buy[ind].date_of_last_buy = DateTime.Now;
                            Buy[ind].Count_of_stocks += count;
                            //обновляем количество акций
                        }
                        else
                        {//иначе добавляем новую акцию в список купленных
                            Buy.Add(new StockTable(name, date, count));
                        }
                        ViewListInDGV(dataGridView2, Buy);
                        //Отображаем список купленных акций
                    }
                    else
                    {//если денег не хватает, сообщаем
                        MessageBox.Show("Попробуйте продать что-нибудь", "Недостаточно средств для покупки");
                    }
                }
            }
            catch (Exception)
            {//если пользователь что-то забыл для покупки
                MessageBox.Show("Необходимо выделить строку в списке акций и заполнить текстовое поле количества акций", "Предупреждение");
            }
        }

        //--------ЗАГРУЗКА ФОРМЫ-----------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                button1_Click(sender, e);
                //Загрузка данных из базы данных
                //качаем баланс
                System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(connectionString);

                SqlCommand cmnd = new SqlCommand("SELECT * FROM [Баланс]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                SqlDataReader reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    Balance = Convert.ToDouble(reader[1]);
                }
                cmnd.Connection.Close();
                textBoxBalance.Text = Balance.ToString();


                //грузим старый курс в объект
                cmnd = new SqlCommand("SELECT * FROM [Для_прогноза]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();
                reader = cmnd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader[1].ToString();
                    double DayBeforeYesterday = Convert.ToDouble(reader[2]);
                    double Yesterday = Convert.ToDouble(reader[3]);

                    OldCourse.Add(new LastStockCourse(name, Yesterday, DayBeforeYesterday));
                }
                cmnd.Connection.Close();

                try
                {
                    cmnd = new SqlCommand("SELECT * FROM [КупленныеАкции]", sqlConnection1);
                    cmnd.Connection.Open();
                    cmnd.ExecuteNonQuery();
                    reader = cmnd.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader[1].ToString();
                        DateTime date = Convert.ToDateTime(reader[2]);
                        int count = Convert.ToInt32(reader[3]);
                        Buy.Add(new StockTable(name, date, count));
                    }
                    cmnd.Connection.Close();
                    ViewListInDGV(dataGridView2, Buy);
                }
                catch
                {
                    //если нет купленных акций
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }
            this.WindowState = FormWindowState.Maximized;
            dataGridView1.AutoResizeColumns();

        }

        //--------КНОПКА "ПРОДАТЬ"---------------------------------------------
        private void buttonSell_Click(object sender, EventArgs e)
        {
            //Продать акцию
            try
            {
                buttonBuyAuto.Enabled = false;//скрыли кнопки
                buttonSellAuto.Enabled = false;
                string name = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();//берем имя выделеннй акции
                int count = Convert.ToInt32(textBoxCount.Text);//берем количество 
                int i = FindInStockTable(Buy, name); //Для поиска в массиве
                int index = FindInDGV(dataGridView1, name); //Для поиска в гриде 1
                double ActualPrice; //надо искать в гриде

                if (dataGridView1.Rows[index].Cells[2].Value.ToString() == "USD")
                {//если курс в долларах, берем в рублях
                    ActualPrice = Convert.ToDouble(dataGridView1.Rows[index].Cells[3].Value);
                }
                else
                {//иначе берем в рублях сразу
                    ActualPrice = Convert.ToDouble(dataGridView1.Rows[index].Cells[1].Value);
                }
                //Сверить количество акций
                if (count <= Buy[i].Count_of_stocks)
                {//если количество меньше или равно тому, сколько хочет продать пользователь
                    Buy[i].Count_of_stocks -= count;//убавляем количество
                    if (Buy[i].Count_of_stocks == 0)
                    {
                        //предупреждение, если все акции продали
                        MessageBox.Show(String.Format("Вы продали все акции компании {0}", Buy[i].name), "Акции кончились");
                        Buy.RemoveAt(i);                        
                        //удаляем из купленных, если все продали
                    }
                    Balance += count * ActualPrice; //актуализируем баланс
                    textBoxBalance.Text = Balance.ToString(); //выводим в текстовое поле
                }
                else
                {//на случай, если пользователь общитался и решил продать больше, чем есть
                    MessageBox.Show("Нельзя продать акций больше, чем есть", "Недостаточно акций для продажи");

                }
                ViewListInDGV(dataGridView2, Buy);
                //Высветить в грид массив купленных акций
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо выделить строку в списке акций и заполнить текстовое поле количества акций", "Предупреждение");
            }
        }

        //--------------ПРИ ЗАКРЫТИИ ФОРМЫ------------------------------------------------
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
//---------------Загрузка купленных акций в базу данных--------------------------------------------------
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

//------обновляем дату, если данные старые----------------------------------------------------------------------------------------

                cmnd = new SqlCommand("SELECT * FROM [DateLastUpdate]", sqlConnection1);
                cmnd.Connection.Open();
                cmnd.ExecuteNonQuery();

                SqlDataReader reader = cmnd.ExecuteReader();
                reader.Read();
                
                DateTime date = Convert.ToDateTime(reader[1]);
                System.TimeSpan val = DateTime.Now - date;
                cmnd.Connection.Close();

                if (val.TotalHours > 8) //если прошло более восьми часов после последнего обновления
                {
                    SqlConnection sqlConnection2 = new SqlConnection(connectionString);

                    SqlCommand command = new SqlCommand("UPDATE [DateLastUpdate] SET DateOfLastUpdate=@value1", sqlConnection2);
                    command.Parameters.AddWithValue("@value1", DateTime.Now);

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection2.Close();

//-------------------обновление таблицы старого курса----------------------------------------------------------------------------

                    for (int i = 0; i < OldCourse.Count; i++)
                    {
                        try
                        {
                            command = new SqlCommand("UPDATE [Для_прогноза] SET Yesterday=@value1, DayBeforeYesterday=@value2 WHERE Name=@value3", sqlConnection2);
                            sqlConnection2 = new SqlConnection(connectionString);
                            command.Parameters.AddWithValue("@value1", NamePrice[OldCourse[i].Name]);
                            command.Parameters.AddWithValue("@value2", OldCourse[i].Yesterday);
                            command.Parameters.AddWithValue("@value3", OldCourse[i].Name);

                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            sqlConnection2.Close();
                        }
                        catch (KeyNotFoundException)
                        {
                            //если в новом курсе нет такой акции, то просто ловим исключение
                            //и катим дальше
                            continue;
                        }
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }
        }

        //----------------ПОКУПКА ПО АВТОМАТУ----------------------
        private void buttonBuyAuto_Click(object sender, EventArgs e)
        {
            //Купить по автомату
            if (FormulaPrediction.Checked)
            {//если выбран прогноз по формуле
                dataGridView1.Sort(FormulaPredict, ListSortDirection.Descending); //сортировка по убыванию
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int CountBuy = 10 - i;
                        dataGridView1.Rows[i].Selected = true;//выделим строку
                        textBoxCount.Text = CountBuy.ToString(); //запишем количество
                        buttonBuy_Click(sender, e); //вызовем покупку
                        buttonBuyAuto.Enabled = true;
                        buttonSellAuto.Enabled = true;
                    }

                    textBoxCount.Text = "";//очистим поле
                }
                catch { MessageBox.Show("Произошла ошибка"); }
            }
            else if (SimplePrediction.Checked)
            {//если выбран простой прогноз
                dataGridView1.Sort(Prediction, ListSortDirection.Descending); //сортировка по убыванию
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int CountBuy = 10 - i;
                        dataGridView1.Rows[i].Selected = true;//выделим строку
                        textBoxCount.Text = CountBuy.ToString(); //запишем количество
                        buttonBuy_Click(sender, e); //вызовем покупку
                        buttonBuyAuto.Enabled = true;
                        buttonSellAuto.Enabled = true;
                    }

                    textBoxCount.Text = "";//очистим поле
                }
                catch { MessageBox.Show("Произошла ошибка"); }
            }

            //если появились новые акции, надо записать для них прогноз
            for (int i = 0; i < Buy.Count; i++)
            {
                int iBuy = FindInStockTable(Buy, dataGridView2.Rows[i].Cells[0].Value.ToString()); //ищем, так как грид может быть сортированным
                try
                {
                    dataGridView2.Rows[i].Cells[3].Value = Predict[Buy[iBuy].name]; //высвечиваем в грид простой курс
                }
                catch
                {
                    //Если  пользователь не сделал простой прогноз и решил купить по  другому прогнозу
                }

                try
                {
                    dataGridView2.Rows[i].Cells[4].Value = PredictFormula[Buy[iBuy].name]; //высвечиваем в грид курс по формуле
                }
                catch
                {
                    //если пользователь не сделал прогноз по формуле и решил купить по простому прогнозу
                }

                try
                {
                    if (Predict.Count != 0)
                    {
                        PredictBuy.Add(Buy[iBuy].name, Predict[Buy[iBuy].name]); //записываем в словарь
                    }
                    else if (PredictFormula.Count != 0)
                    {
                        PredictBuyFormula.Add(Buy[iBuy].name, PredictFormula[Buy[iBuy].name]);
                    }

                }
                catch
                {
                    if (Predict.Count != 0)
                    {
                        PredictBuy[Buy[iBuy].name] = Predict[Buy[iBuy].name];
                    }
                    else if (PredictFormula.Count != 0)
                    {
                        PredictBuyFormula[Buy[iBuy].name] = PredictFormula[Buy[iBuy].name];
                    }
                } //избегать ошибки повторений
            }

        }

        //--------------------------ПРОДАЖА ПО АВТОМАТУ------------------------------------------
        private void buttonSellAuto_Click(object sender, EventArgs e)
        {
            //Сортировка по возрастанию при продаже

            if (FormulaPrediction.Checked)
            {//если выбран прогноз по формуле
                try
                {//продать по автомату

                    if (Buy.Count != 0) //если есть купленные акции
                    {
                        int StockCount = Buy.Count / 2;
                        int i = 0;
                        foreach (var pair in PredictBuyFormula.OrderBy(pair => pair.Value))
                        {//отображаем словарь
                            int j = FindInStockTable(Buy, pair.Key); //ищем в списке купленных акций
                            int k = FindInDGV(dataGridView2, pair.Key); //ищем в гриде
                            dataGridView2.Rows[k].Selected = true; //выделяем необходимую строку
                            textBoxCount.Text = Buy[j].Count_of_stocks.ToString(); //запишем количество
                            buttonSell_Click(sender, e); //вызовем продажу  
                            PredictBuyFormula.Remove(pair.Key); //удаляем из списка для предсказаний
                            buttonBuyAuto.Enabled = true;
                            buttonSellAuto.Enabled = true;
                            i++;
                            //чтобы продать только половину от количества всех акций
                            if (i == StockCount) { break; }
                        }

                        dataGridView2.Sort(PredictionFormula, ListSortDirection.Ascending); //сортировка по возрастанию
                    }

                    else
                    {
                        MessageBox.Show("Чтобы что-то продать,надо для начала что-то купить");
                    }
                    textBoxCount.Text = "";//очистим поле
                }
                catch { MessageBox.Show("Произошла ошибка"); }
            }
            else if (SimplePrediction.Checked)
            {//если выбран просто прогноз
                try
                {//продать по автомату

                    if (Buy.Count != 0) //если есть купленные акции
                    {
                        int StockCount = Buy.Count / 2;
                        int i = 0;
                        foreach (var pair in PredictBuy.OrderBy(pair => pair.Value))
                        {//отображаем словарь
                            int j = FindInStockTable(Buy, pair.Key); //ищем в списке купленных акций
                            int k = FindInDGV(dataGridView2, pair.Key); //ищем в гриде
                            dataGridView2.Rows[k].Selected = true; //выделяем необходимую строку
                            textBoxCount.Text = Buy[j].Count_of_stocks.ToString(); //запишем количество
                            buttonSell_Click(sender, e); //вызовем продажу  
                            PredictBuy.Remove(pair.Key); //удаляем из списка для предсказаний
                            buttonBuyAuto.Enabled = true;
                            buttonSellAuto.Enabled = true;
                            i++;
                            //чтобы продать только половину от количества всех акций
                            if (i == StockCount) { break; }
                        }
                        
                        dataGridView2.Sort(PredictionBuy, ListSortDirection.Ascending); //сортировка по возрастанию
                    }
                    else
                    {
                        MessageBox.Show("Чтобы что-то продать,надо для начала что-то купить");
                    }
                    textBoxCount.Text = "";//очистим поле
                }
                catch { MessageBox.Show("Произошла ошибка"); }
            }

            //если что-то продали, надо актуализировать прогноз
            for (int i = 0; i < Buy.Count; i++)
            {
                int iBuy = FindInStockTable(Buy, dataGridView2.Rows[i].Cells[0].Value.ToString()); //ищем, так как грид может быть сортированным
                try
                {
                    dataGridView2.Rows[i].Cells[3].Value = Predict[Buy[iBuy].name]; //высвечиваем в грид простой курс
                }
                catch
                {
                    //Если  пользователь не сделал простой прогноз и решил купить по  другому прогнозу
                }

                try
                {
                    dataGridView2.Rows[i].Cells[4].Value = PredictFormula[Buy[iBuy].name]; //высвечиваем в грид курс по формуле
                }
                catch
                {
                    //если пользователь не сделал прогноз по формуле и решил купить по простому прогнозу
                }

                try
                {
                    if (Predict.Count != 0)
                    {
                        PredictBuy.Add(Buy[iBuy].name, Predict[Buy[iBuy].name]); //записываем в словарь
                    }
                    else if (PredictFormula.Count != 0)
                    {
                        PredictBuyFormula.Add(Buy[iBuy].name, PredictFormula[Buy[iBuy].name]);
                    }

                }
                catch
                {
                    if (Predict.Count != 0)
                    {
                        PredictBuy[Buy[iBuy].name] = Predict[Buy[iBuy].name];
                    }
                    else if (PredictFormula.Count != 0)
                    {
                        PredictBuyFormula[Buy[iBuy].name] = PredictFormula[Buy[iBuy].name];
                    }
                } //избегать ошибки повторений
            }


        }

        //--------------СДЕЛАТЬ ПРОГНОЗ ПО ФОРМУЛЕ----------------------------------------
        private void button1_Click_1(object sender, EventArgs e)
        {//прогноз на завтра = скользящая вчера +1/3*(сегодня-вчера)
            //скользящая вчера = (позавчера + вчера + сегодня)/3
            try
            {
                PredictFormula.Clear();
                PredictBuyFormula.Clear();
                int indexInGrid;
                double YesterdayPrice;
                double DayBeforeYesterdayPrice;
                foreach (var element in stocks) //так как грид составлен по объекту Стокс
                {
                    double ValuePredict; //значение предсказания для текущей компании
                    double ActualPrice = element.Price; //актуальная цена

                    int i = FindInLastStockCourse(OldCourse, element.Company.Title); //ищем акцию в старом курсе

                    if (element.Currency == "USD")
                    {//если в долларах, переводим в рубли
                        ActualPrice = (double)(element.Price * body.valute.usd.value);
                        YesterdayPrice = (double)(OldCourse[i].Yesterday * body.valute.usd.value); //вчерашняя цена
                        DayBeforeYesterdayPrice = (double)(OldCourse[i].DayBeforeYesterday * body.valute.usd.value);//Позавчерашняя цена

                    }
                    else
                    {//иначе оставляем рубли
                        YesterdayPrice = OldCourse[i].Yesterday; //вчерашняя цена
                        DayBeforeYesterdayPrice = OldCourse[i].DayBeforeYesterday;//Позавчерашняя цена
                    }

                    double SlidingYesterday = (DayBeforeYesterdayPrice + YesterdayPrice + ActualPrice) / 3;//скользящая вчера = (позавчера + вчера + сегодня)/3
                    ValuePredict = Math.Round(SlidingYesterday + (ActualPrice - YesterdayPrice) / 3, 3) - ActualPrice;   //прогноз на завтра = скользящая вчера +1/3*(сегодня-вчера)
                    indexInGrid = FindInDGV(dataGridView1, element.Company.Title);
                    PredictFormula.Add(element.Company.Title, ValuePredict);//записываем значение в словарь
                    dataGridView1.Rows[indexInGrid].Cells[6].Value = ValuePredict; //высвечиваем в грид
                }

                //заполняем скрытый столбец прогноза в списке купленных акций
                for (int i = 0; i < Buy.Count; i++)
                {
                    int iBuy = FindInStockTable(Buy, dataGridView2.Rows[i].Cells[0].Value.ToString()); //ищем, так как грид может быть сортированным
                    dataGridView2.Rows[i].Cells[4].Value = PredictFormula[Buy[iBuy].name]; //высвечиваем в грид
                    try
                    {
                        PredictBuyFormula.Add(Buy[iBuy].name, PredictFormula[Buy[iBuy].name]); //записываем в словарь
                    }
                    catch
                    {
                        PredictBuyFormula[Buy[iBuy].name] = PredictFormula[Buy[iBuy].name];
                    } //избегать ошибки повторений
                }
            }
            catch { throw; }
            buttonBuyAuto.Enabled = true;
            buttonSellAuto.Enabled = true;
            FormulaPrediction.Checked = true;
        }
    }
}
 