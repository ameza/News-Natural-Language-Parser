using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using edu.stanford.nlp.ie.crf;
using NewsParserDomain.Models;

namespace NewsParserDomain.Controllers
{
    public class ParserController
    {
        public static ParseResult ParseString(NewsItem item, String identificador)
        {
            // Path to the folder with classifies models
            const string jarRoot = @"C:\Dev\ProyectoParadigmas\paket-files\nlp.stanford.edu\stanford-ner-2015-12-09";
            const string classifiersDirecrory = jarRoot + @"\classifiers";

            // Loading 3 class classifier model
            var classifier = CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + @"\english.all.3class.distsim.crf.ser.gz");


            var classifier2 = CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + @"\english.muc.7class.distsim.crf.ser.gz");

            // Console.WriteLine("\n\nPrueba #1 de Entidades Nombradas NER\n\n");

            return new ParseResult()
            {
                Identificador =identificador,
                ResultadoClase3 = classifier.classifyToString(item.Summary),
                ResultadoClase7 = classifier2.classifyToString(item.Summary),
                ResultadoNerClase3 = XMLStringToNer(classifier.classifyToString(item.Summary,"xml",true)),
                ResultadoNerClase7 = XMLStringToNer(classifier2.classifyToString(item.Summary, "xml", true)),
                TextoOriginal = item.Summary,
                Titulo = item.Title
            };

        }


       

        public static List<ParseObject> XMLStringToNer(String xmlString)
        {

            var result = new List<ParseObject>();

            xmlString = "<root>" + xmlString + "</root>";

            var doc = XDocument.Parse(xmlString);

            if (doc.Root != null)
                result.AddRange(doc.Root.Elements().Select(element => new ParseObject()
                {
                    NamedEntity = element.Attribute("entity").Value, Valor = element.Value
                }));
            return result;
        }

}
}
