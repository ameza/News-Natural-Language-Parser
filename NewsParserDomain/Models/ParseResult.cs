using System;
using System.Collections.Generic;

namespace NewsParserDomain.Models
{
    public class ParseResult
    {
        public String Identificador { get; set; }
        public String Titulo { get; set; }
        public String TextoOriginal { get; set; }
        public String ResultadoClase3  { get; set; }
        public List<ParseObject> ResultadoNerClase3 { get; set; }
        public String ResultadoClase7  { get; set; }
        public List<ParseObject> ResultadoNerClase7  { get; set; }
    }
}
