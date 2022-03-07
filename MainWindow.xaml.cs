using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public DataTable Select(string selectSQL) 
        {
            DataTable dataTable = new DataTable("dataBase");                
                                                                           
            SqlConnection sqlConnection = new SqlConnection("server=DIAMONDPC;Trusted_Connection=Yes;DataBase=CustPostOrd;");
            sqlConnection.Open();                                         
            SqlCommand sqlCommand = sqlConnection.CreateCommand();       
            sqlCommand.CommandText = selectSQL;                          
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); 
            sqlDataAdapter.Fill(dataTable);                                
            return dataTable;
        }


        string sql;
        DataTable data;
        public MainWindow()
        {
            InitializeComponent();
             data = Select("SELECT * FROM [dbo].[Customer]");
            DataGrid.ItemsSource = data.DefaultView;


        }

        private void ComboBox_Initialized(object sender, EventArgs e)
        {
            ComboBox.Items.Add("1.Найти покупателей, проживающих в городе Казань");
            ComboBox.Items.Add("2.Найти покупателей, фамилии которых начинаются с заданного символа");
            ComboBox.Items.Add("3.Найти покупателей, фамилии которых содержат заданную последовательность символов");
            ComboBox.Items.Add("4.Найти покупателей, фамилии которых оканчиваются заданным символом");
            ComboBox.Items.Add("5.Выдать список покупателей с указанием значения выражения Balance*100");
            ComboBox.Items.Add("6.Определить число поставщиков каждого товара");
            ComboBox.Items.Add("7.Найти минимальную цену заданного товара");
            ComboBox.Items.Add("8.Выдать упорядоченный по возрастанию цен список поставщиков заданного товара");
            ComboBox.Items.Add("9.Найти покупателей, некоторые заказы которых можно выполнить, не нарушая лимитирующего ограничения");
            ComboBox.Items.Add("10.Найти всех покупателей указанного товара");
            ComboBox.Items.Add("11.Найти максимальный по стоимости заказ");
            ComboBox.Items.Add("12.Найти все тройки <покупатель,поставщик,заказ>, удовлетворяющие условию");
            ComboBox.Items.Add("13.Вывести таблицу с заказами но вместо id фамилии");
            ComboBox.Items.Add("14.Вывести города поставщиков без повторений");
            ComboBox.Items.Add("15.Вывести список покупателей у которых фамилия заканчивается на 'ев' и проживают в Казани" +
                            "и их баланс больше 4000");
            ComboBox.Items.Add("16.Вывести количество городов в которых есть поставщики");
            ComboBox.Items.Add("17. Вывести города в которых проживают покупатели без повторений большими буквами");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    sql = "select * from \"Customer\" where \"Address\" like 'Казань%'";
                    data = Select(sql);
                    break;
                case 1:
                    sql = "select * from \"Customer\" where \"Family\" like 'П%'";
                    data = Select(sql);
                    break;
                case 2:
                    sql = "select * from \"Customer\" where \"Family\" like 'Ам%'";
                    data = Select(sql);
                    break;
                case 3:
                    sql = "select * from \"Customer\" where \"Family\" like '%ов'";
                    data = Select(sql);
                    break;
                case 4:
                    sql = "select \"Family\", \"Balance\" * 100 from \"Customer\"";
                    data = Select(sql);
                    break;
                case 5:
                    sql = "select \"Commodity\", count(*) from \"Providers\" Group by \"Commodity\"";
                    data = Select(sql);
                    break;
                case 6:
                    sql = "Select min(\"Price\"),\"Commodity\" from \"Providers\" where \"Commodity\" = 'Мышь' Group by \"Commodity\", \"Price\"";
                    data = Select(sql);
                    break;
                case 7:
                    sql = "select min(\"Price\"),\"Commodity\",\"Name\" from \"Providers\" where \"Commodity\" = 'Монитор' group by \"Commodity\", \"Price\", \"Name\"";
                    data = Select(sql);
                    break;
                case 8:
                    sql = "select \"Family\" from \"Customer\" where \"IdCs\" in (select \"IdCs\" from \"Orders\" inner join \"Providers\" on \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" and \"Orders\".\"Number\" * \"Providers\".\"Price\" < \"Orders\".\"Limit\")";
                    data = Select(sql);
                    break;
                case 9:
                    sql = "select \"Family\" from \"Customer\" where \"IdCs\" in (select \"IdCs\" from \"Orders\" where \"Commodity\" = 'Принтер')";
                    data = Select(sql);
                    break;
                case 10:
                    sql = "select max(\"Orders\".\"Number\"*\"Providers\".\"Price\"), \"Providers\".\"Commodity\" from \"Orders\" inner join \"Providers\" on \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" group by \"Providers\".\"Commodity\" order by max(\"Orders\".\"Number\" * \"Providers\".\"Price\") DESC limit 1";
                    data = Select(sql);
                    break;
                case 11:
                    sql = "select \"Customer\".\"Family\",\"Providers\".\"Name\",\"Providers\".\"Address\",\"Orders\".\"Commodity\" from \"Orders\",\"Providers\",\"Customer\" where \"Customer\".\"IdCs\" = \"Orders\".\"IdCs\" and \"Providers\".\"Address\" = \"Customer\".\"Address\" and \"Orders\".\"Commodity\" = \"Providers\".\"Commodity\" and \"Orders\".\"Number\" * \"Providers\".\"Price\" <= \"Orders\".\"Limit\"";
                    data = Select(sql);
                    break;
                case 12:
                    sql = "select \"Customer\".\"Family\",\"Orders\".\"Commodity\",\"Orders\".\"Number\",\"Orders\".\"Limit\" from \"Orders\",\"Customer\" where \"Customer\".\"IdCs\" = \"Orders\".\"IdCs\" group by \"Customer\".\"Family\", \"Orders\".\"Commodity\", \"Orders\".\"Number\", \"Orders\".\"Limit\"";
                    data = Select(sql);
                    break;
                case 13:
                    sql = "select \"Providers\".\"Address\" from \"Providers\" group by \"Address\"";
                    data = Select(sql);
                    break;
                case 14:
                    sql = "select * from \"Customer\" where \"Customer\".\"Family\" like '%ев' and \"Customer\".\"Address\" like '%нь%' and \"Balance\" > money(4000)";
                    data = Select(sql);
                    break;
                case 15:
                    sql = "select count(distinct (\"Address\")) from \"Providers\"";
                    data = Select(sql);
                    break;
                case 16:
                    sql = "select distinct (upper(\"Address\")) as Адрес from \"Customer\"";
                    data = Select(sql);
                    break;
            }
            TextBox.Text = sql;
            DataGrid.ItemsSource = data.DefaultView;

        }

   
        private void ComboboxParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboboxParam.SelectedIndex)
            {
                case 0:
                    sql = $"select * from \"Customer\" where \"Address\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 1:
                    sql = $"select * from \"Customer\" where \"Family\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 2:
                    sql = $"select * from \"Customer\" where \"Family\" like '{TexBoxParam.Text}%'";
                    data = Select(sql);
                    break;
                case 3:
                    sql = $"select * from \"Customer\" where \"Family\" like '%{TexBoxParam.Text}'";
                    data = Select(sql);
                    break;
            }
            TextBox.Text = sql;
            DataGrid.ItemsSource = data.DefaultView;
        }

        private void TexBoxParam_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TexBoxParam.Text = "";
        }

        private void TexBoxParam_TouchDown(object sender, TouchEventArgs e)
        {
            TexBoxParam.Text = "";
        }

        private void ComboboxParam_Initialized(object sender, EventArgs e)
        {
            ComboboxParam.Items.Add("1.Найти покупателей, проживающих в заданном городе ");
            ComboboxParam.Items.Add("2.Найти покупателей, фамилии которых начинаются с заданного символа");
            ComboboxParam.Items.Add("3.Найти покупателей, фамилии которых содержат заданную последовательность символов");
            ComboboxParam.Items.Add("4.Найти покупателей, фамилии которых оканчиваются заданным символом");

        }
    }
}
