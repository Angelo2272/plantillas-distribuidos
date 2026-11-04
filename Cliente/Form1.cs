using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("Celsius");
            comboBox1.Items.Add("Fahrenheit");

            comboBox2.Items.Add("Celsius");
            comboBox2.Items.Add("Fahrenheit");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Leer configuración
                string host = ConfigurationManager.AppSettings["host"];
                int port = int.Parse(ConfigurationManager.AppSettings["port"]);

                // Crear petición
                ConvertRequest peticion = new ConvertRequest();

                peticion.from = comboBox1.Text;
                peticion.to = comboBox2.Text;
                peticion.units = double.Parse(textBox1.Text);

                // Serializar petición
                MemoryStream memoria = new MemoryStream();

                XmlSerializer serializador = new XmlSerializer(typeof(ConvertRequest));

                serializador.Serialize(memoria, peticion);

                byte[] datos = memoria.ToArray();

                // Conectar con servidor
                TcpClient cliente = new TcpClient(host, port);

                NetworkStream stream = cliente.GetStream();

                // Enviar petición
                stream.Write(datos, 0, datos.Length);

                // Recibir respuesta
                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string xml = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Deserializar respuesta
                StringReader lector = new StringReader(xml);

                XmlSerializer serializadorRespuesta =
                    new XmlSerializer(typeof(ConvertResponse));

                ConvertResponse respuesta =
                    (ConvertResponse)serializadorRespuesta.Deserialize(lector);

                // Mostrar resultado
                label1.Text = "Resultado: " + respuesta.units + " " + respuesta.to;

                // Cerrar conexiones
                lector.Close();
                memoria.Close();
                stream.Close();
                cliente.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
