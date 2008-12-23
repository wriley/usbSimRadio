using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HID;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace usbSimRadio
{
    class usbSimDevices
    {
        int _HIDHandle;
        Hid _MyHID = new Hid();
        DeviceManagement _MyDeviceManagement = new DeviceManagement();
        HID.Debugging _MyDebugging = new HID.Debugging();
        public usbSimDevice[] MyDevices = new usbSimDevice[64];

        public struct usbSimDevice
        {
            public int DeviceID;
            public string DevicePath;
        }

        public string getDevicePathFromID(int id)
        {
            for (int i = 0; i < MyDevices.Length; i++)
            {
                if (MyDevices[i].DeviceID == id)
                {
                    return MyDevices[i].DevicePath;
                }
            }
            return null;
        }

        public void FindDevices()
        {
            bool DeviceFound = false;
            string[] DevicePathName = new string[128];
            string GUIDString;
            System.Guid HidGuid;
            int MemberIndex = 0;
            int DevicePathIndex = 0;
            ushort MyVendorID = 0x16c0;
            string MyVendorString = "usbSim http://workbench.freetcp.com/";
            ushort MyProductID = 0x05df;
            string MyProductString = "usbSimRadio";

            int Result = 0;
            FileIOApiDeclarations.SECURITY_ATTRIBUTES Security = new HID.FileIOApiDeclarations.SECURITY_ATTRIBUTES();

            HidGuid = Guid.Empty;

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
            Debug.WriteLine(_MyDebugging.ResultOfAPICall("GetHidGuid"));

            // Display the GUID.
            GUIDString = HidGuid.ToString();
            Debug.WriteLine("  GUID for system HIDs: " + GUIDString);

            // Fill an array with the device path names of all attached HIDs.
            DeviceFound = _MyDeviceManagement.FindDeviceFromGuid(HidGuid, ref DevicePathName);

            // If there is at least one HID, attempt to read the Vendor ID and Product ID
            // of each device until all devices have been examined.

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
                                & (ProductString == MyProductString))
                            {
                                MyDevices[DevicePathIndex].DevicePath = DevicePathName[MemberIndex];
                                MyDevices[DevicePathIndex].DeviceID = _MyHID.DeviceAttributes.VersionNumber;
                                DevicePathIndex++;
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
                            Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                        }
                    }

                    if (_HIDHandle != FileIOApiDeclarations.INVALID_HANDLE_VALUE)
                    {
                        Result = FileIOApiDeclarations.CloseHandle(_HIDHandle);
                    }

                    // Keep looking until there are no more left to examine.
                    MemberIndex = MemberIndex + 1;

                } while (!(MemberIndex == DevicePathName.Length));
            }
        }
    }
}
