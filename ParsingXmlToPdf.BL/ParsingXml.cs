using ParsingXmlToPdf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParsingXmlToPdf.BL
{
    public class ParsingXml : IParsingXml
    {
        public List<Person> ParsingXmlPerrcon(string fileName)
        {
            List<Person> users = new List<Person>();
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fileName);
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
                        users.Add(user);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Какая то  ошибка  из БЛ " + ex.Message);
            }

            users = GrouPerson(users);
            return users;
        }


        private List<Person> GrouPerson(List<Person> users)
        {
            var us = users.GroupBy(x => x.Company);
            List<Person> newList = new List<Person>();

            foreach (var usCompany in us)
            {
                int step = 1;
                foreach (var person in usCompany.OrderBy(x => x.Name))
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
    }
}