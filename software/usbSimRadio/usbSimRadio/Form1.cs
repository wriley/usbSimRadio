using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using usbSimRadio;

namespace usbSimRadio
{
    public partial class usbSimRadioForm : Form
    {
        usbSimRadio radio;

        public usbSimRadioForm()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usbSimRadioForm_Load(object sender, EventArgs e)
        {
            radio = new usbSimRadio();
            listBoxDevices.Items.Clear();
            if (radio.DeviceDetected())
            {
                listBoxDevices.Items.Add(radio.GetProductName());
            }
        }

        private void listBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FreqActive1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FreqActive2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FreqStandby1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FreqStandby2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            listBoxDevices.Items.Clear();
            if (radio.DeviceDetected())
            {
                listBoxDevices.Items.Add(radio.GetProductName());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float freqActive = radio.GetFrequencyActive();
            float freqStandby = radio.GetFrequencyStandby();

            string[] pieces = new string[2];
            pieces = freqActive.ToString("000.000").Split('.');
            if (pieces.Length == 2)
            {
                FreqActive1.Value = int.Parse(pieces[0]);
                FreqActive2.Value = int.Parse(pieces[1]);
            }

            pieces = freqStandby.ToString("000.000").Split('.');
            if (pieces.Length == 2)
            {
                FreqStandby1.Value = int.Parse(pieces[0]);
                FreqStandby2.Value = int.Parse(pieces[1]);
            }
        }
    }
}
