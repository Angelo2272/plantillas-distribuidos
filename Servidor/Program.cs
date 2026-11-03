using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Crear canal TCP
            TcpChannel canal = new TcpChannel(5000);

            // 2. Registrar canal
            ChannelServices.RegisterChannel(canal, false);

            // 3. Registrar objeto remoto
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Calculadora),
                "Calculadora",
                WellKnownObjectMode.Singleton
            );

            Console.WriteLine("Servidor Remoting iniciado");
            Console.WriteLine("Esperando peticiones...");

            Console.ReadLine();
        }
    }
}
