using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Xml;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ParsingXmlToPdf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathfile = "";
        string[] filename;
        List<Person> users = new List<Person>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectXml_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog xmlfile = new OpenFileDialog();
            xmlfile.Filter = "Xml |*.xml|Все файлы|*.*";
            if(xmlfile.ShowDialog() == true)
            {
                pathfile = xmlfile.FileName;
                filename = pathfile.Split('\\');
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlfile.FileName);
                XmlElement xRoot = xdoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    Person user = new Person();
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {

                        if (childnode.Name == "Name")
                        {
                            //ContentXml.Text += $"{childnode.InnerText}\n";
                            user.Name = childnode.InnerText;
                        }
                        if (childnode.Name == "Age")
                        {
                            //ContentXml.Text += $"{childnode.InnerText}\n";
                            user.Age = Convert.ToInt32(childnode.InnerText);
                        }
                        if (childnode.Name == "Company")
                        {
                            foreach (XmlNode companyname in childnode)
                            {
                                if (companyname.Name == "Name")
                                {
                                    //ContentXml.Text += $"{companyname.InnerText}\n---------\n";
                                    user.Company = companyname.InnerText;
                                }
                            }
                        }
                    }
                    users.Add(user);
                }

                users = GrouPerson();

                foreach (Person u in users)
                {
                    ContentXml.Text += $"{u.Number}. Имя: {u.Name}\nКомпания: {u.Company}\nВозраст: {u.Age}\n________________\n";
                }

            }
        }

        /// <summary>
        /// Группируем  людей  по  компаниям
        /// </summary>
        /// <returns></returns>
        private List<Person> GrouPerson()
        {
            var us = users.GroupBy(x => x.Company);
            List<Person> newList = new List<Person>();

            foreach (var usCompany in us)
            {
                int step = 1;
                foreach (var person in usCompany.OrderBy(x=>x.Name))
                {
                    newList.Add(new Person()
                    {
                        Age = person.Age,
                        Company = person.Company
                    ,
                        Name = person.Name,
                        Number = step
                    });
                    step++;
                }
            }
            return newList;
        }

        private void ParsingToPdf_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Укажите путь куда сохранить файл";
            saveFileDialog.Filter = "PDF Файл (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                { 
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    PdfPTable table = new PdfPTable(4);
                    PdfPCell cell = new PdfPCell();
                    cell.HorizontalAlignment = 1;
                    cell.Border = 0;
                    string[] tablesname = {"Number" , "Name", "Company", "Age" };

                    foreach (string name in tablesname)
                    {
                        cell = new PdfPCell(new Phrase(name));
                        cell.BackgroundColor = BaseColor.CYAN;
                        table.AddCell(cell);
                    }
                    
                    foreach (Person u in users)
                    {
                        table.AddCell(new Phrase(u.Number.ToString()));
                        table.AddCell(new Phrase(u.Name));
                        table.AddCell(new Phrase(u.Company));
                        table.AddCell(new Phrase($"{u.Age}"));
                    }
                    doc.Add(table);
                    doc.Close();
                    ContentXml.Text = $"Данные успешно сохранены в {saveFileDialog.FileName.ToString()}";
                }
            }
        }
    }
    public class Person
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }
    }
}
