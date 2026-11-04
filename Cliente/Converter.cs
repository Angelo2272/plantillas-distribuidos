using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public enum UnidadTemperatura
    {
        Celsius,
        Fahrenheit
    }
    public class ConvertRequest
    {
        public string from { get; set; }
        public string to { get; set; }
        public double units { get; set; }

        public ConvertRequest()
        {
        }
    }

    public class ConvertResponse
    {
        public string to { get; set; }
        public double units { get; set; }

        public ConvertResponse()
        {
        }
    }
    internal class Converter
    {
        
    }
}
