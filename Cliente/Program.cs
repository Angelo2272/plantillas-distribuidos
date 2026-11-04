using System.Net.Sockets;
using System.Xml.Serialization;

  public class ClienteSocket
{
    public class Mensaje
    {
        public string Texto { get; set; }
        public string Remitente { get; set; }

        public  Mensaje()
        {
        }
    }
    private static void Main(string[] args)
    {
        try
        {
            // Conectar con el servidor
            TcpClient cliente = new TcpClient("127.0.0.1", 5000);

            NetworkStream flujo = cliente.GetStream();

            // Crear mensaje
            Mensaje mensaje = new Mensaje
            {
                Texto = "Hola servidor",
                Remitente = "Cliente"
            };

            // Serializar objeto a XML
            XmlSerializer serializer = new XmlSerializer(typeof(Mensaje));

            MemoryStream memoria = new MemoryStream();

            serializer.Serialize(memoria, mensaje);

            byte[] datos = memoria.ToArray();

            memoria.Close();

            // Enviar mensaje
            flujo.Write(datos, 0, datos.Length);

            // Recibir respuesta
            byte[] respuestaDatos = new byte[4096];

            int cantidad = flujo.Read(respuestaDatos, 0, respuestaDatos.Length);

            // Deserializar XML
            XmlSerializer serializerRespuesta = new XmlSerializer(typeof(Mensaje));

            MemoryStream memoriaRespuesta = new MemoryStream(respuestaDatos, 0, cantidad);

            Mensaje respuesta = (Mensaje)serializerRespuesta.Deserialize(memoriaRespuesta);

            memoriaRespuesta.Close();

            Console.WriteLine("Respuesta del servidor:");
            Console.WriteLine(respuesta.Remitente + ": " + respuesta.Texto);

            // Cerrar conexión
            flujo.Close();
            cliente.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.ReadKey();
    }
}
