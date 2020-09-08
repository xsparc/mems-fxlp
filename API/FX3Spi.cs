using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyUSB;
using FX3Api;
using FX3USB;

namespace mems_fx3lp.API
{
    partial class FX3Peripheral
    {
        // Single transaction 32-bit bidirectional SPI
        // if DrActive == false, the function would behave asynchronously
        // INPUT: Uint32 writeValue 
        // RETURN: Uint32 readValue 
        public UInt32 SpiTransfer(UInt32 writeValue)
        {
            UInt32 MISO = 0; // DO NOT INITIALIZE TO ZERO AFTER FINALIZING CODE!!

            UInt32 readValue;
            Byte[] buf = new Byte[7];
            Int32 numBytes;
            UInt32 status;

            // Configure control endpoint
            ConfigureControlEndpoint((Byte)USBCommands.ADI_TRANSFER_BYTES, false);

            // Setting the write value
            FX3ControlEndPt.Index =(UInt16)((writeValue & 0xFFFF0000) >> 16);
            FX3ControlEndPt.Value = (UInt16)(writeValue & 0xFFFF); 

            // Send vendor command
            if (!XferControlData(ref buf, 8, 2000))
            {
                throw new FX3CommunicationException("ERROR: control endpoint timeout during SPI transfer");
            }
            
            //numBytes = m_F
            

            return MISO;
        }

        // Methodd for configuring control endpoint
        // INPUT: byte requestCode -> bitmap that represents a configuration
        // INPUT: bool toDevice -> USB packet direction
        private void ConfigureControlEndpoint(Byte requestCode, bool toDevice)
        {
            // Null reference handler
            if (m_ActiveFX3 == null)
            {
                throw new FX3Exception("ERROR: Attempting to configure FX3 device control endpoint without being enumerated");
            }

            // Device connectivity handler
            if (m_FX3Connected == false)
            {
                throw new FX3Exception("ERROR: Attempting to configure FX3 device control endpoint without being connected");
            }

            // Get control endpoint reference from FX3 Device
            FX3ControlEndPt = m_ActiveFX3.ControlEndPt;

            // Control endpoint configuration
            FX3ControlEndPt.ReqCode = requestCode;
            FX3ControlEndPt.ReqType = CyConst.REQ_VENDOR;
            FX3ControlEndPt.Target = CyConst.TGT_DEVICE;
            FX3ControlEndPt.Value = 0;
            FX3ControlEndPt.Index = 0;
            if (toDevice)
            {
                FX3ControlEndPt.Direction = CyConst.DIR_TO_DEVICE;
            }
            else
            {
                FX3ControlEndPt.Direction = CyConst.DIR_FROM_DEVICE;
            }
        }

    }
}
