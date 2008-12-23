using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HID;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace usbSimRadio
{
    public class usbSimRadio : Control
    {
        int _HIDHandle;
        Hid _MyHID = new Hid();
        DeviceManagement _MyDeviceManagement = new DeviceManagement();
        HID.Debugging _MyDebugging = new HID.Debugging();
        string DevicePath;
        int DeviceID;
        Thread thread;
        bool terminated;
        string _MyDevicePathName;
        bool _MyDeviceDetected;
        int _ReadHandle;
        Hid.InputReport inputReport;
        float FrequencyActive = 118.00F;
        float FrequencyStandby = 118.00F;
        byte[] Buttons = new byte[2];

        int OFFSET_FREQ_ACTIVE = 2;
        int OFFSET_FREQ_STANDBY = 4;
        int OFFSET_BUTTONS = 6;

        public usbSimRadio(string p, int i)
        {
            this.DevicePath = p;
            this.DeviceID = i;
        }

        private void FindRadio()
        {
            bool DeviceFound = false;
            string[] DevicePathName = new string[128];
            string GUIDString;
            System.Guid HidGuid;
            int MemberIndex = 0;
            ushort MyVendorID = 0x16c0;
            string MyVendorString = "usbSim http://workbench.freetcp.com/";
            ushort MyProductID = 0x05df;
            string MyProductString = "usbSimRadio";
            int Result = 0;
            FileIOApiDeclarations.SECURITY_ATTRIBUTES Security = new HID.FileIOApiDeclarations.SECURITY_ATTRIBUTES();
            //bool Success = false;

            HidGuid = Guid.Empty;
            _MyDeviceDetected = false;

            // Values for the SECURITY_ATTRIBUTES structure:
            Security.lpSecurityDescriptor = 0;
            Security.bInheritHandle = System.Convert.ToInt32(true);
            Security.nLength = Marshal.SizeOf(Security);

            /*
              API function: 'HidD_GetHidGuid
              Purpose: Retrieves the interface class GUID for the HID class.
              Accepts: 'A System.Guid object for storing the GUID.
              */

            HidApiDeclarations.HidD_GetHidGuid(ref HidGuid);
            //Debug.WriteLine(_MyDebugging.ResultOfAPICall("GetHidGuid"));

            // Display the GUID.
            GUIDString = HidGuid.ToString();
            //Debug.WriteLine("  GUID for system HIDs: " + GUIDString);

            // Fill an array with the device path names of all attached HIDs.
            DeviceFound = _MyDeviceManagement.FindDeviceFromGuid(HidGuid, ref DevicePathName);

            // If there is at least one HID, attempt to read the Vendor ID and Product ID
            // of each device until there is a match or all devices have been examined.

            if (DeviceFound)
            {
                MemberIndex = 0;
                do
                {
                    // ***
                    // API function:
                    // CreateFile
                    // Purpose:
                    // Retrieves a handle to a device.
                    // Accepts:
                    // A device path name returned by SetupDiGetDeviceInterfaceDetail
                    // The type of access requested (read/write).
                    // FILE_SHARE attributes to allow other processes to access the device while this handle is open.
                    // A Security structure. Using Null for this may cause problems under Windows XP.
                    // A creation disposition value. Use OPEN_EXISTING for devices.
                    // Flags and attributes for files. Not used for devices.
                    // Handle to a template file. Not used.
                    // Returns: a handle that enables reading and writing to the device.
                    // ***

                    _HIDHandle = FileIOApiDeclarations.CreateFile
                        (DevicePathName[MemberIndex],
                        FileIOApiDeclarations.GENERIC_READ | FileIOApiDeclarations.GENERIC_WRITE,
                        FileIOApiDeclarations.FILE_SHARE_READ | FileIOApiDeclarations.FILE_SHARE_WRITE,
                        ref Security,
                        FileIOApiDeclarations.OPEN_EXISTING, 0, 0);

                    if (_HIDHandle != FileIOApiDeclarations.INVALID_HANDLE_VALUE)
                    {
                        // The returned handle is valid,
                        // so find out if this is the device we're looking for.

                        // Set the Size property of DeviceAttributes to the number of bytes in the structure.
                        //_MyHID.DeviceAttributes.Size = _MyHID.DeviceAttributes.ToString().Length;
                        _MyHID.DeviceAttributes.Size = Marshal.SizeOf(_MyHID.DeviceAttributes);

                        // ***
                        // API function:
                        // HidD_GetAttributes
                        // Purpose:
                        // Retrieves a HIDD_ATTRIBUTES structure containing the Vendor ID,
                        // Product ID, and Product Version Number for a device.
                        // Accepts:
                        // A handle returned by CreateFile.
                        // A pointer to receive a HIDD_ATTRIBUTES structure.
                        // Returns:
                        // True on success, False on failure.
                        // ***

                        Result = HidApiDeclarations.HidD_GetAttributes(_HIDHandle, ref _MyHID.DeviceAttributes);

                        if (Result != 0)
                        {
                            byte[] buffer = new byte[128];
                            string VendorString;
                            string ProductString;

                            if (!HidApiDeclarations.HidD_GetManufacturerString(_HIDHandle, ref buffer[0], buffer.Length))
                            {
                                Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                                break;
                            }
                            VendorString = buffer.ToString();
                            buffer.Initialize();

                            if (!HidApiDeclarations.HidD_GetProductString(_HIDHandle, ref buffer[0], buffer.Length))
                            {
                                Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                                break;
                            }
                            ProductString = buffer.ToString();

                            // Find out if the device matches the one we're looking for.
                            if ((_MyHID.DeviceAttributes.VendorID == MyVendorID)
                                & (_MyHID.DeviceAttributes.ProductID == MyProductID)
                                & (VendorString == MyVendorString)
                                & (ProductString == MyProductString)
                                & (_MyHID.DeviceAttributes.VersionNumber == DeviceID))
                            {
                                _MyDeviceDetected = true;

                                // Save the DevicePathName so OnDeviceChange() knows which name is my device.
                                _MyDevicePathName = DevicePathName[MemberIndex];
                            }
                            else
                            {
                                // It's not a match, so close the handle.
                                Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                            }
                        }
                        else
                        {
                            // There was a problem in retrieving the information.
                            _MyDeviceDetected = false;
                            Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                        }
                    }

                    // Keep looking until we find the device or there are no more left to examine.
                    MemberIndex = MemberIndex + 1;

                } while (!((_MyDeviceDetected == true) | (MemberIndex == DevicePathName.Length)));
            }

            if (_MyDeviceDetected)
            {
                // Learn the capabilities of the device.
                _MyHID.Capabilities = _MyHID.GetDeviceCapabilities(_HIDHandle);

                // Get and display the Input report buffer size.
                //_MyHID.GetNumberOfInputBuffers(_HIDHandle, ref NumberOfInputBuffers);

                // Get another handle to use in overlapped ReadFiles (for requesting Input reports).
                _ReadHandle = FileIOApiDeclarations.CreateFile(_MyDevicePathName, FileIOApiDeclarations.GENERIC_READ | FileIOApiDeclarations.GENERIC_WRITE, FileIOApiDeclarations.FILE_SHARE_READ | FileIOApiDeclarations.FILE_SHARE_WRITE, ref Security, FileIOApiDeclarations.OPEN_EXISTING, FileIOApiDeclarations.FILE_FLAG_OVERLAPPED, 0);

                // (optional)
                // Flush any waiting reports in the input buffer.
                _MyHID.FlushQueue(_ReadHandle);
            }
        }

        private void FindRadioAgain()
        {
            FileIOApiDeclarations.SECURITY_ATTRIBUTES Security = new HID.FileIOApiDeclarations.SECURITY_ATTRIBUTES();
            _MyDeviceDetected = false;
            Security.lpSecurityDescriptor = 0;
            Security.bInheritHandle = System.Convert.ToInt32(true);
            Security.nLength = Marshal.SizeOf(Security);

            _HIDHandle = FileIOApiDeclarations.CreateFile
                (_MyDevicePathName,
                FileIOApiDeclarations.GENERIC_READ | FileIOApiDeclarations.GENERIC_WRITE,
                FileIOApiDeclarations.FILE_SHARE_READ | FileIOApiDeclarations.FILE_SHARE_WRITE,
                ref Security,
                FileIOApiDeclarations.OPEN_EXISTING, 0, 0);

            if (_HIDHandle != FileIOApiDeclarations.INVALID_HANDLE_VALUE)
            {
                // The returned handle is valid,
                // so find out if this is the device we're looking for.

                // Set the Size property of DeviceAttributes to the number of bytes in the structure.
                //_MyHID.DeviceAttributes.Size = _MyHID.DeviceAttributes.ToString().Length;
                _MyHID.DeviceAttributes.Size = Marshal.SizeOf(_MyHID.DeviceAttributes);

                HidApiDeclarations.HidD_GetAttributes(_HIDHandle, ref _MyHID.DeviceAttributes);
                _MyDeviceDetected = true;
            }

            if (_MyDeviceDetected)
            {
                // Learn the capabilities of the device.
                _MyHID.Capabilities = _MyHID.GetDeviceCapabilities(_HIDHandle);

                // Get and display the Input report buffer size.
                //_MyHID.GetNumberOfInputBuffers(_HIDHandle, ref NumberOfInputBuffers);

                // Get another handle to use in overlapped ReadFiles (for requesting Input reports).
                _ReadHandle = FileIOApiDeclarations.CreateFile(_MyDevicePathName, FileIOApiDeclarations.GENERIC_READ | FileIOApiDeclarations.GENERIC_WRITE, FileIOApiDeclarations.FILE_SHARE_READ | FileIOApiDeclarations.FILE_SHARE_WRITE, ref Security, FileIOApiDeclarations.OPEN_EXISTING, FileIOApiDeclarations.FILE_FLAG_OVERLAPPED, 0);

                // (optional)
                // Flush any waiting reports in the input buffer.
                _MyHID.FlushQueue(_ReadHandle);
            }
        }

        private void ReadData()
        {
            byte[] buffer = new byte[_MyHID.Capabilities.InputReportByteLength];
            bool Success = true;
            inputReport = new Hid.InputReport();

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

                inputReport.Read(_ReadHandle, _HIDHandle, ref _MyDeviceDetected, ref buffer, ref Success);
                
                if (Success && buffer != null && buffer.Length == _MyHID.Capabilities.InputReportByteLength)
                {
                    HandleInput(ref buffer);
                }
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
            this.FrequencyActive = BytesToFreq(ref input[OFFSET_FREQ_ACTIVE], ref input[input[OFFSET_FREQ_ACTIVE + 1]]);
            this.FrequencyStandby = BytesToFreq(ref input[OFFSET_FREQ_STANDBY], ref input[OFFSET_FREQ_STANDBY + 1]);
            this.Buttons[0] = input[OFFSET_BUTTONS];
            this.Buttons[1] = input[OFFSET_BUTTONS + 1];
        }

        private float BytesToFreq(ref byte major, ref byte minor)
        {
            float val = float.Parse(major.ToString());
            val += float.Parse(minor.ToString()) * .001F;
            return val;
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
    }
}
