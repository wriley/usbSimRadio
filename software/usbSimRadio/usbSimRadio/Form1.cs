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
        usbSimDevices devices = new usbSimDevices();

        public usbSimRadioForm()
        {
            InitializeComponent();
        }

        public void FindDevices()
        {
            
            devices.FindDevices();
            listBoxDevices.Items.Clear();
            for(int i = 0; i < devices.MyDevices.Length; i++)
            {
                if (devices.MyDevices[i].DeviceID > 0)
                {
                    listBoxDevices.Items.Add(devices.MyDevices[i].DeviceID.ToString());
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usbSimRadioForm_Load(object sender, EventArgs e)
        {
            FindDevices();
        }

        private void listBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelDevicePath.Text = devices.getDevicePathFromID(int.Parse(listBoxDevices.Items[listBoxDevices.SelectedIndex].ToString()));
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
    }
}
