using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace usbSimRadio
{
    public class usbSimRadio : Control
    {
        USBSharp usb = new USBSharp();
        Thread thread;
        bool terminated;
        bool _MyDeviceDetected;
        string _MyDevicePathName;
        string _MyVendorName;
        string _MyProductName;
        int _MyDeviceID;
        ushort MyVersionNumber;

        float FrequencyActive = 118.00F;
        float FrequencyStandby = 118.00F;
        byte[] Buttons = new byte[2];
        int OFFSET_FREQ_ACTIVE = 2;
        int OFFSET_FREQ_STANDBY = 4;
        int OFFSET_BUTTONS = 6;

        public float GetFrequencyActive()
        {
            return FrequencyActive;
        }

        public float GetFrequencyStandby()
        {
            return FrequencyStandby;
        }

        public string GetDevicePathName()
        {
            return _MyDevicePathName;
        }

        public int GetDeviceID()
        {
            return _MyDeviceID;
        }

        public string GetVendorName()
        {
            return _MyVendorName;
        }

        public string GetProductName()
        {
            return _MyProductName;
        }

        public bool DeviceDetected()
        {
            return _MyDeviceDetected;
        }

        public void FindRadio()
        {
            ushort MyVendorID = 0x16c0;
            string MyVendorString = "usbSim http://workbench.freetcp.com/";
            ushort MyProductID = 0x05df;
            string MyProductString = "usbSimRadio";
            

            usb.CT_HidGuid();
            usb.CT_SetupDiGetClassDevs();
            bool success = true;
            int i = 0;

            while (success)
            {
                if (usb.CT_SetupDiEnumDeviceInterfaces(i) == 0)
                {
                    Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
                    usb.CT_SetupDiDestroyDeviceInfoList();
                    success = false;
                }
                else
                {
                    int requiredSize = 0;
                    int size = 0;
                    usb.CT_SetupDiGetDeviceInterfaceDetail(ref requiredSize, size);
                    size = requiredSize;
                    usb.CT_SetupDiGetDeviceInterfaceDetailx(ref requiredSize, size);
                    int h = usb.CT_CreateFile(usb.DevicePathName);
                    if (h == 1)
                    {
                        usb.CT_HidD_GetAttributes(usb.HidHandle);
                        int PreparsedDataPointer = 0;
                        usb.CT_HidD_GetPreparsedData(usb.HidHandle, ref PreparsedDataPointer);
                        usb.CT_HidP_GetCaps(PreparsedDataPointer);
                        byte[] buffer = new byte[128];
                        string VendorString = "";
                        string ProductString = "";
                        string SerialString = "";
                        if (HidApiDeclarations.HidD_GetManufacturerString(usb.HidHandle, buffer, buffer.Length))
                        {
                            BytesToString(ref buffer, ref VendorString);
                        }
                        buffer = new byte[128];

                        if (HidApiDeclarations.HidD_GetProductString(usb.HidHandle, buffer, buffer.Length))
                        {
                            BytesToString(ref buffer, ref ProductString);
                        }
                        buffer = new byte[128];

                        if (HidApiDeclarations.HidD_GetSerialNumberString(usb.HidHandle, buffer, buffer.Length))
                        {
                            BytesToString(ref buffer, ref SerialString);
                        }

                        // Find out if the device matches the one we're looking for.
                        if ((usb.myHIDD_ATTRIBUTES.VendorID == MyVendorID)
                            & (usb.myHIDD_ATTRIBUTES.ProductID == MyProductID)
                            & (VendorString == MyVendorString)
                            & (ProductString == MyProductString))
                            //& (usb.myHIDD_ATTRIBUTES.VersionNumber == MyVersionNumber))
                        {
                            _MyDeviceDetected = true;
                            _MyDevicePathName = usb.DevicePathName;
                            _MyVendorName = VendorString;
                            _MyProductName = ProductString;
                            success = false;
                        }
                    }
                }
                i++;
            }

        }

        private void FindRadioAgain()
        {
            int h = usb.CT_CreateFile(usb.DevicePathName);
            if (h == 1)
            {
                usb.CT_HidD_GetAttributes(usb.HidHandle);
                int PreparsedDataPointer = 0;
                usb.CT_HidD_GetPreparsedData(usb.HidHandle, ref PreparsedDataPointer);
                usb.CT_HidP_GetCaps(PreparsedDataPointer);
                
            }
        }

        private void ReadData()
        {
            byte[] buffer = new byte[usb.myHIDP_CAPS.FeatureReportByteLength];
            
            while (!terminated)
            {
                if (!_MyDeviceDetected)
                {
                    FindRadioAgain();
                    if (!_MyDeviceDetected)
                    {
                        return;
                    }
                }

                HidApiDeclarations.HidD_GetFeature(usb.HidHandle, buffer, buffer.Length);

                if (buffer != null && buffer.Length == usb.myHIDP_CAPS.FeatureReportByteLength)
                {
                    HandleInput(ref buffer);
                }
                Thread.Sleep(100);
            }

        }

        private void HandleInput(ref byte[] input)
        {
            /*
            input[]
            Start	Size	Description
            0       1       Dummy Report ID
            1	    1	    TypeID
            2	    2	    Active Frequency
            4	    2	    Standby Frequency
            6	    2	    Button State
            */
            this.FrequencyActive = BytesToFreq(ref input[OFFSET_FREQ_ACTIVE], ref input[OFFSET_FREQ_ACTIVE + 1]);
            this.FrequencyStandby = BytesToFreq(ref input[OFFSET_FREQ_STANDBY], ref input[OFFSET_FREQ_STANDBY + 1]);
            this.Buttons[0] = input[OFFSET_BUTTONS];
            this.Buttons[1] = input[OFFSET_BUTTONS + 1];
        }

        private float BytesToFreq(ref byte major, ref byte minor)
        {
            float val = float.Parse(major.ToString());
            val += float.Parse(minor.ToString()) * .010F;
            return val;
        }

        public void BytesToString(ref byte[] b, ref string s)
        {
            for (int j = 0; j < b.Length; j++)
                if (b[j] != 0)
                    s += Convert.ToChar(b[j]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                terminated = true;
                if (thread != null && thread.ThreadState != ThreadState.Unstarted)
                {
                    if (!thread.Join(1000))
                        thread.Abort();
                    thread = null;
                }
            }
            base.Dispose(disposing);
        }

        public usbSimRadio()
        {
            FindRadio();
            if (_MyDeviceDetected)
            {
                thread = new Thread(ReadData);
                thread.Start();
            }
        }

        public byte[] ReadFile()
        {
            byte[] buffer = new byte[usb.myHIDP_CAPS.FeatureReportByteLength];
            HidApiDeclarations.HidD_GetFeature(usb.HidHandle, buffer, buffer.Length);
            return buffer;
        }
    }
}
