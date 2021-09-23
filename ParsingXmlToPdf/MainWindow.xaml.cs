using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ParsingXmlToPdf.Model;
using ParsingXmlToPdf.Utility;
using ParsingXmlToPdf.Core;
using ParsingXmlToPdf.BL;
using ParsingXmlToPdf.Services;


namespace ParsingXmlToPdf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Model.Person> users = new List<Model.Person>();
        IParsingXml _parsingXml;
        IPersonServis _personServis;


        public MainWindow()
        {
            InitializeComponent();

            _parsingXml = new ParsingXml(); 
            _personServis = new PersonServis();
        } 

        /// <summary>
        /// кнопка выбора  файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectXml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog xmlfile = MyDialog.OfD("Xml |*.xml|Все файлы|*.*"); // немного изменили

            if ( xmlfile.ShowDialog() == true)
            {
                users = _personServis.GetPersons(_parsingXml.ParsingXmlPerrcon(xmlfile.FileName));
                PrintUSer(users);
            }
        }

        private void PrintUSer(List<Model.Person> users)
        {
            foreach (Model.Person u in users)
            {
                ContentXml.Text += $"{u.Number}. Имя: {u.Name}\nКомпания: {u.Company}\nВозраст: {u.Age}\n________________\n";
            }
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
                    
                    foreach (Model.Person u in users)
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

  
}
