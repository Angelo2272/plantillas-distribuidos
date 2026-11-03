
using System;
using System.Net.Sockets;
using System.Text;

class Cliente
{
    static void Main()
    {
        // 1. Conectar con servidor
        TcpClient cliente = new TcpClient(
            "127.0.0.1", 5000);

        Console.WriteLine("Conectado al servidor");

        // 2. Obtener flujo
        NetworkStream flujo = cliente.GetStream();

        // 3. Preparar mensaje
        Console.WriteLine("Ingrese un mensaje: ");
        string mensaje = Console.ReadLine();

        byte[] datos = Encoding.UTF8.GetBytes(mensaje);

        // 4. Enviar mensaje
        flujo.Write(datos, 0, datos.Length);

        // 5. Recibir respuesta
        byte[] buffer = new byte[1024];

        int cantidad = flujo.Read(
            buffer, 0, buffer.Length);

        string respuesta = Encoding.UTF8.GetString(
            buffer, 0, cantidad);

        Console.WriteLine("Respuesta: " + respuesta);

        // 6. Cerrar conexión
        flujo.Close();
        cliente.Close();
        Console.ReadLine();
    }
}
