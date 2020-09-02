using System;
using System.IO.Ports;
using System.Windows.Forms;
using EasyModbus;

namespace DSHModbus
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cmbCOM.Items.Clear();
            foreach (string port in ports)
            {
                cmbCOM.Items.Add(port);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
			if (txtBaud.TextLength > 3)
            {
                if (txtSlaveID.TextLength > 0)
                {
                    if (cmbCOM.SelectedIndex > -1)
                    {
                        //Let's do dis man.
                        ModbusClient modbusClient = new ModbusClient(cmbCOM.SelectedItem.ToString());
                        modbusClient.UnitIdentifier = Byte.Parse(txtSlaveID.Text);
                        modbusClient.Baudrate = Int32.Parse(txtBaud.Text); 
                        modbusClient.Parity = System.IO.Ports.Parity.None;
                        modbusClient.StopBits = System.IO.Ports.StopBits.One;
                        modbusClient.ConnectionTimeout = 500;
                        modbusClient.Connect();
                        if(modbusClient.Connected)
                        {
                            //yay it works?? or not, .Connected is a fraud. It always "connects" but it doesn't do shit to detect if it's actually connected. check if exception is thrown.

                            lsbDebug.Items.Clear();
                            lsbDebug.Items.Add("Testen op slaveID " + txtSlaveID.Text + " en Baud " + txtBaud.Text); 
                            lsbDebug.Items.Add("Opvragen temperatuur...");
                            // exception thrown, especially timeout exception. We can catch this and know if this crap works.
                            try
                            {
                                lsbDebug.Items.Add("Gelukt, huidige temperatuur: " + modbusClient.ReadInputRegisters(6, 1));
                            } catch (System.TimeoutException)
                            {
                                lsbDebug.Items.Add("Gefaald, niet verbonden.");
                                lsbDebug.Items.Add("Controleer waardes met waardes toestel");
                            }
                        }
                        modbusClient.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show("Controleer COM poort", "Controleer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Controleer SlaveID", "Controleer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
            else
            {
                MessageBox.Show("Controleer baudwaarde", "Controleer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
