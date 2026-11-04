using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;

public  class Program
{
    public class Mensaje
    {
        public string Texto { get; set; }
        public string Remitente { get; set; }

        public Mensaje()
        {
        }
    }
    private static void Main(string[] args)
    {
            TcpListener servidor = new TcpListener(IPAddress.Any, 5000);

            servidor.Start();

            Console.WriteLine("Servidor iniciado...");
            Console.WriteLine("Esperando cliente...");

            while (true)
            {
                TcpClient cliente = servidor.AcceptTcpClient();

                Console.WriteLine("Cliente conectado.");

                NetworkStream flujo = cliente.GetStream();

                try
                {
                    // Recibir mensaje
                    byte[] datos = new byte[4096];

                    int cantidad = flujo.Read(datos, 0, datos.Length);

                    // Deserializar XML
                    XmlSerializer serializer = new XmlSerializer(typeof(Mensaje));

                    Mensaje mensaje;

                    MemoryStream memoria = new MemoryStream(datos, 0, cantidad);

                    mensaje = (Mensaje)serializer.Deserialize(memoria);

                    memoria.Close();

                    Console.WriteLine(mensaje.Remitente + ": " + mensaje.Texto);

                    // Crear respuesta
                    Mensaje respuesta = new Mensaje
                    {
                        Texto = "Mensaje recibido correctamente",
                        Remitente = "Servidor"
                    };

                    // Serializar respuesta
                    XmlSerializer serializerRespuesta = new XmlSerializer(typeof(Mensaje));

                    MemoryStream memoriaRespuesta = new MemoryStream();

                    serializerRespuesta.Serialize(memoriaRespuesta, respuesta);

                    byte[] respuestaDatos = memoriaRespuesta.ToArray();

                    memoriaRespuesta.Close();

                    // Enviar respuesta
                    flujo.Write(respuestaDatos, 0, respuestaDatos.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    flujo.Close();
                    cliente.Close();
                }

                Console.WriteLine("Esperando otro cliente...");
            }
        }
}