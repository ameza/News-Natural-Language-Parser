using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NewsParserDomain.Models;

namespace NewsParserParadigmas.Models
{
    public class ReportViewModel
    {
        public List<DiagnosticoItem> Diagnostico { get; set; }
        public List<ParseResult> ParseResults { get; set; }

        public ReportViewModel(List<ParseResult> parseResults, String palabra, string categoria)
        {
            ParseResults = parseResults;
            Diagnostico = GenerarDiagnostico(palabra, categoria);

        }
        
        private List<DiagnosticoItem> GenerarDiagnostico(String palabra, String categoria)
        {
            var resultados = new List<DiagnosticoItem>();
            foreach (var item in ParseResults)
            {
                //hacemos busquedas en las listas atributo valor del NER y las ponemos en resultados
                
                resultados.AddRange(item.ResultadoNerClase3.Where(x => x.NamedEntity.ToLower().Equals(categoria.ToLower()) && x.Valor.ToLower().Equals(palabra.ToLower())).Select(x=>x.Valor).Select(result=>new DiagnosticoItem()
                {
                    Identificador = item.Identificador,
                    NER = categoria,
                    Palabra = result
                }    
                ));
                resultados.AddRange(item.ResultadoNerClase7.Where(x => x.NamedEntity.ToLower().Equals(categoria.ToLower()) && x.Valor.ToLower().Equals(palabra.ToLower())).Select(x => x.Valor).Select(result => new DiagnosticoItem()
                {
                    Identificador = item.Identificador,
                    NER = categoria,
                    Palabra = result
                }));
            }
            return resultados;

        }
    }

    public class DiagnosticoItem
    {
        public String Identificador { get; set; }
        public String Palabra { get; set; }
        public String NER { get; set; }
    }
}