using System.Collections.Generic;

namespace ParsingXmlToPdf.Core
{
  public interface IParsingXml
    {
         List<Person> ParsingXmlPerrcon(string fileName);
    }
}