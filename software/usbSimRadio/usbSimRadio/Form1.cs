using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HID;
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
            radio.FindRadio();
        }
    }
}
