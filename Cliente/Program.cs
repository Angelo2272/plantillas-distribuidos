using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            TcpChannel canal = new TcpChannel();

            
            ChannelServices.RegisterChannel(canal, false);

            
            Calculadora calculadora =
                (Calculadora)Activator.GetObject(
                    typeof(Calculadora),
                    "tcp://localhost:5000/Calculadora"
                );
            Console.WriteLine("Ingrese un número: ");
            int numero1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Ingrese segundo número: ");
            int numero2 = int.Parse(Console.ReadLine());


            int resultado = calculadora.Sumar(numero1, numero2);

            Console.WriteLine("Resultado: " + resultado);

            Console.ReadLine();
        }
    }
}
