using System;
using System.Windows.Forms;
using System.IO.Ports;  // necessário para ter acesso as portas


namespace interfaceArduinoVS2013
{
    public partial class TermometroApp : Form
    {   
        public TermometroApp()
        {
            InitializeComponent();
            timerCOM.Enabled = true;
                         
        }

        private void atualizaListaCOMs()
        {
            int i;
            bool quantDiferente;    //flag para sinalizar que a quantidade de portas mudou

            i = 0;
            quantDiferente = false;

            //se a quantidade de portas mudou
            if (comboBox1.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (comboBox1.Items[i++].Equals(s) == false)
                    {
                        quantDiferente = true;
                    }
                }
            }
            else
            {
                quantDiferente = true;
            }

            //Se não foi detectado diferença
            if (quantDiferente == false)
            {
                return;                     //retorna
            }

            //limpa comboBox
            comboBox1.Items.Clear();

            //adiciona todas as COM diponíveis na lista
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
            //seleciona a primeira posição da lista
            comboBox1.SelectedIndex = 0;
        }

        private void timerCOM_Tick(object sender, EventArgs e)
        {
            atualizaListaCOMs();
        }

        private void btConectar_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {
                try
                {
                    serialPort1.PortName = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                    serialPort1.Open();
                }
                catch
                {
                    return;
                }
                if (serialPort1.IsOpen)
                {
                    btConectar.Text = "Desconectar";
                    comboBox1.Enabled = false;
                }
            }
            else
            {
                try
                {
                    serialPort1.Close();
                    comboBox1.Enabled = true;
                    btConectar.Text = "Conectar";
                }
                catch
                {
                    return;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serialPort1.IsOpen == true)  // se porta aberta 
             serialPort1.Close();            //fecha a porta
        }  

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }
        
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            BeginInvoke(new Action(() =>

            {
                try

                {                

                    int limite = 10000;//Número máximo de pontos que o gráfico pode ter.
                    string dados = serialPort1.ReadLine();//Pegue o que tem no buffer até um '\n'.
                    //Thread.Sleep(100);
                    string erro = serialPort1.ReadLine();
                   // Thread.Sleep(100);
                    string referencia = serialPort1.ReadLine();
                    // Thread.Sleep(100);
                    string pwm = serialPort1.ReadLine();
                    string kp = serialPort1.ReadLine();
                    string ki = serialPort1.ReadLine();
                    string kd = serialPort1.ReadLine();

                    txtTemp.Text = dados;
                    textBox6.Text = referencia;
                    txtErro.Text = erro;
                    textBox3.Text = kp;
                    textBox4.Text = ki;
                    textBox5.Text = kd;

                    double num4 = Convert.ToDouble(pwm);
                    double num5 = (num4 / 255);
                    textBox1.Text = Convert.ToString(num5);                  
                                        
                    double num =  Convert.ToDouble(dados);//Converta a string para int.
                    double num2 = Convert.ToDouble(erro);//Converta a string para int.
                    double num3 = Convert.ToDouble(referencia);//Converta a string para int.                
                    
                    chart1.Series[0].Points.AddY(num);//Adiciona o valor lido como um novo Y;
                    chart1.Series[1].Points.AddY(num2);//Adiciona o valor lido como um novo Y;
                    chart1.Series[2].Points.AddY(num3);//Adiciona o valor lido como um novo Y;                   
                    float tempo = chart1.Series[0].Points.Count;
                    textBox2.Text = Convert.ToString(tempo);                  

                    //Se o gráfico tiver número de pontos maior que o limite...

                    if (chart1.Series[0].Points.Count > limite && chart1.Series[1].Points.Count > limite && chart1.Series[2].Points.Count > limite)

                    {
                        chart1.Series[0].Points.RemoveAt(0);//Remova o primeiro ponto.
                        chart1.Series[1].Points.RemoveAt(0);
                        chart1.Series[2].Points.RemoveAt(0);                    
                        chart1.Update();//Atualiza o grafico.

                    }              

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
        }
        private void btnki_Click(object sender, EventArgs e)
        {

            try
            {
                serialPort1.Write("BB");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnd_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("AA");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("DD");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CC");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("FF");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("EE");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("HH");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("GG");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtTemp_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }   

        private void txtRef_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtErro_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
          
        }       


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void kp_TextChanged(object sender, EventArgs e)
        {

        }

        private void ki_TextChanged(object sender, EventArgs e)
        {

        }

        private void kd_TextChanged(object sender, EventArgs e)
        {

        }

   
        private void TermometroApp_Load(object sender, EventArgs e)
        {
           

        }

       
        private void btConectar_Click_1(object sender, EventArgs e)
        {

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void re_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
