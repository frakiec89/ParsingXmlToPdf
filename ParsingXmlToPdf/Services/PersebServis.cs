using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingXmlToPdf.Services
{
    public interface IPersonServis
    {
        Core.Person GetPerson(Model.Person person);
        Model.Person SetPerson(Core.Person person);
        List<Model.Person> GetPersons(List<Core.Person> people);
    }


    public  class PersonServis : IPersonServis
    {

        /// <summary>
        /// Принимает модель
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Возвращает core</returns>
        public Core.Person GetPerson ( Model.Person person) // todo -зависимсоть  от Core
        {
            return new Core.Person { Age = person.Age, Company = person.Company, Name = person.Name, Number = person.Number };
        }

       

        public Model.Person SetPerson( Core.Person person) // todo -зависимсоть  от Core
        {
            return new Model.Person { Age = person.Age, Company = person.Company, Name = person.Name, Number = person.Number };
        }


        public List<Model.Person> GetPersons(List<Core.Person> people)
        {
            var us = new List<Model.Person>();

            foreach (var item in people)
            {
                us.Add(SetPerson(item));
            }

            return us;
        }
    }

   
}
