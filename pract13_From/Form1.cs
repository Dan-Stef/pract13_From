using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace pract13_From
{
    public partial class Form1 : Form
    {
        private const String PATH = "..\\..\\goods.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileStream fileStream = new FileStream(PATH, FileMode.OpenOrCreate);
            StreamReader sR = new StreamReader(fileStream);
            LinkedList<Good> goods = new LinkedList<Good>();
            String str1 = String.Empty;
            while ((str1 = sR.ReadLine()) != null && str1 != String.Empty)
            {
                String[] str = str1.Split('@');
                switch (str[0])
                {
                    case "Пр":
                        {
                            goods.AddLast(new Product(str[1], Convert.ToInt32(str[2]), Convert.ToDateTime(str[3]), Convert.ToInt32(str[4])));
                            break;
                        }
                    case "Па":
                        {
                            goods.AddLast(new Batch(str[1], Convert.ToInt32(str[2]), Convert.ToInt32(str[3]), Convert.ToDateTime(str[4]), Convert.ToInt32(str[5])));
                            break;
                        }
                    case "К":
                        {
                            goods.AddLast(new Set(str[1], Convert.ToInt32(str[2]), str[3]));
                            break;
                        }
                    default:
                        {
                            textBox1.AppendText("Incorrect data" + Environment.NewLine);
                            break;
                        }
                }
            }
            sR.Close();
            fileStream.Close();

            DateTime now = DateTime.Now;
            foreach (Good item in goods)
            {
                textBox1.AppendText(item.ShowInfo() + Environment.NewLine);
                textBox1.AppendText((item.IsFresh(now) ? "Годен" : "Не годен" ) + Environment.NewLine);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }
    abstract class Good
    {

        protected DateTime creationDate;

        protected string name;

        protected int price;

        protected int expirationDate;

        public virtual string ShowInfo()
        {
            return string.Format("Название продукта - " + name + " Цена " + price + " ");
        }

        public virtual bool IsFresh(DateTime currentDate)
        {
            return (currentDate < creationDate + new TimeSpan(expirationDate, 0, 0, 0));
        }
    }

    internal class Product : Good
    {

        public Product(string name, int price, DateTime creationDate, int expirationDate)
        {
            base.name = name;
            base.price = price;
            base.creationDate = creationDate;
            base.expirationDate = expirationDate;
        }

        public override string ShowInfo()
        {
            return base.ShowInfo() + string.Format("Дата производства - {0} Срок годности - {1} дней ",
                                 creationDate, expirationDate);
        }
    }


    internal class Batch : Good
    {
        private int amount;

        public Batch(string name, int price, int amount, DateTime creationDate, int expirationDate)
        {
            base.name = name;
            base.price = price;
            base.creationDate = creationDate;
            base.expirationDate = expirationDate;
            this.amount = amount;

        }

        public override string ShowInfo()
        {
            return base.ShowInfo() +
                   string.Format("\nКоличество - {0}\nДата производства - {1}\nСрок годности - {2} дней",
                                 amount, creationDate, expirationDate);
        }
    }

    internal class Set : Good
    {
        private string productSet;

        public Set(string name, int price, string productSet)
        {
            base.name = name;
            base.price = price;
            this.productSet = productSet;

        }

        public override string ShowInfo()
        {
            return base.ShowInfo() +
                   string.Format("\nПеречень продуктов - {0}",
                                 productSet);
        }

        public override bool IsFresh(DateTime currentDate) => true;

    }


