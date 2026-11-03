
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Servidor
{
    static void Main()
    {
        // 1. Crear servidor
        TcpListener servidor = new TcpListener(
            IPAddress.Any, 5000);

        // 2. Iniciar servidor
        servidor.Start();

        Console.WriteLine("Servidor iniciado...");
        Console.WriteLine("Esperando cliente...");

        // 3. Aceptar conexión
        TcpClient cliente = servidor.AcceptTcpClient();

        Console.WriteLine("Cliente conectado");

        // 4. Obtener flujo
        NetworkStream flujo = cliente.GetStream();

        // 5. Recibir mensaje
        byte[] buffer = new byte[1024];

        int cantidad = flujo.Read(
            buffer, 0, buffer.Length);

        string mensaje = Encoding.UTF8.GetString(
            buffer, 0, cantidad);

        Console.WriteLine("Recibido: " + mensaje);

        // 6. Procesar mensaje
        string respuesta = "Mensaje recibido";

        // 7. Enviar respuesta
        byte[] datos = Encoding.UTF8.GetBytes(respuesta);

        flujo.Write(datos, 0, datos.Length);

        // 8. Cerrar conexión
        flujo.Close();
        cliente.Close();
        servidor.Stop();

        Console.WriteLine("Servidor finalizado");
    }
}
