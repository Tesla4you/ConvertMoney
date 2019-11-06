using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

//Написать программу, которая на основании запроса данных с сайта
//http://www.cbr.ru/scripts/XML_daily.asp определит самую дорогую и самую дешевую валюту (с
//максимальной и минимальной стоимостью 1-ой единицы в рублях)




namespace ConvertMoney
{
    public partial class Form1 : Form
    {
        class Currency
        {

            public string ID { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru");
            Look();
        }
        public void Look()
        {
               
            List<Currency> currencies = new List<Currency>();

            XmlDocument xDoc = new XmlDocument();

            try
            {
                xDoc.Load("http://www.cbr.ru/scripts/XML_daily.asp");
                XmlElement xRoot = xDoc.DocumentElement;

                foreach (XmlNode xnode in xRoot)
                {
                    Currency currency = new Currency();
                    XmlNode attr = xnode.Attributes.GetNamedItem("ID");
                    if (attr != null)
                    {
                        currency.ID = attr.Value;
                    }
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "Name")
                            currency.Name = childnode.InnerText;

                        if (childnode.Name == "Value")
                            currency.Value = Double.Parse(childnode.InnerText);

                    }
                    currencies.Add(currency);
                }

                var result = from Currency in currencies
                             orderby Currency.Value
                             select Currency;
                foreach (Currency cur in result)
                    textBox1.Text += $"{cur.Name} - {cur.Value} " + '\r' + '\n' + '\r';

                textBox2.Text = $"{result.Last().Name} - {result.Last().Value}";
                textBox3.Text = $"{result.First().Name} - {result.First().Value}";
            }
            catch (Exception e )
            {
                MessageBox.Show("Не удалось загрузить данные");
            } 

            
                       
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            Look();
        }
    }
}
