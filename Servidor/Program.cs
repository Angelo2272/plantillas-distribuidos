
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace ServidorConversor
{
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

    internal class Program
    {
        static void Main(string[] args)
        {
            string host = ConfigurationManager.AppSettings["host"];
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);

            TcpListener servidor = new TcpListener(IPAddress.Parse(host), port);

            servidor.Start();

            Console.WriteLine("Host: " + host);
            Console.WriteLine("Puerto: " + port);
            Console.WriteLine("Servidor iniciado...");
            Console.WriteLine("Esperando conexiones...");

            while (true)
            {
                TcpClient cliente = servidor.AcceptTcpClient();

                NetworkStream stream = cliente.GetStream();

                // Recibir petición XML
                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string xml = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                StringReader lector = new StringReader(xml);

                XmlSerializer serializador = new XmlSerializer(typeof(ConvertRequest));

                ConvertRequest peticion =
                    (ConvertRequest)serializador.Deserialize(lector);

                Console.WriteLine("Conversión recibida:");
                Console.WriteLine(peticion.units + " " + peticion.from);

                // Realizar conversión
                ConvertResponse respuesta = new ConvertResponse();

                respuesta.to = peticion.to;

                if (peticion.from == "Celsius" && peticion.to == "Fahrenheit")
                {
                    respuesta.units = (peticion.units * 9 / 5) + 32;
                }
                else
                {
                    respuesta.units = peticion.units;
                }

                // Convertir respuesta a XML
                MemoryStream memoria = new MemoryStream();

                XmlSerializer serializadorRespuesta =
                    new XmlSerializer(typeof(ConvertResponse));

                serializadorRespuesta.Serialize(memoria, respuesta);

                byte[] datos = memoria.ToArray();

                // Enviar respuesta
                stream.Write(datos, 0, datos.Length);

                Console.WriteLine("Resultado: " + respuesta.units + " " + respuesta.to);

                memoria.Close();
                lector.Close();
                stream.Close();
                cliente.Close();

                Console.WriteLine("Esperando otra conexión...");
            }
        }
    }
}
