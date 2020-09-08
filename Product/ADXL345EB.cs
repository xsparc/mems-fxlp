using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

// Non Windows references
using FX3Api;

namespace mems_fx3lp.Product
{
    /// <summary>
    /// Class to be used for operating ADXL345EB EVB using FX3LP platform
    /// </summary>
    class ADXL345EB
    {
        private const int MaxFifoLength = 6;
        private const int InstLength = 1;
        private const uint SpiTimeout = 5000;

        /// <summary>
        /// Register addresses of ADXL345
        /// </summary>
        public enum RegisterAddress
        {
            DEVID = 0x00,
            THRESH_TAP = 0x1D,
            OFSX = 0x1E,
            OFSY = 0x1F,
            OFSZ = 0x20,
            DUR = 0x21,
            LATENT = 0x22,
            WINDOW = 0x23,
            HRESH_ACT = 0x24,
            THRESH_INACT = 0x25,
            TIME_INACT = 0x26,
            ACT_INACT_CTL = 0x27,
            THRESH_FF = 0x28,
            TIME_FF = 0x29,
            TAP_AXES = 0x2A,
            ACT_TAP_STATUS = 0x2B,
            BW_RATE = 0x2C,
            POWER_CTL = 0x2D,
            INT_ENABLE = 0x2E,
            INT_MAP = 0x2F,
            INT_SOURCE = 0x30,
            DATA_FORMAT = 0x31,
            DATAX0 = 0x32,
            DATAX1 = 0x33,
            DATAY0 = 0x34,
            DATAY1 = 0x35,
            DATAZ0 = 0x36,
            DATAZ1 = 0x37,
            FIFO_CTL = 0x38,
            FIFO_STATUS = 0x39
        }
        
        /// <summary>
        /// SPI instruction byte for ADXL345
        /// </summary>
        public enum SpiInstByte
        {
            SPI_WRITE = 0x00,
            SPI_READ = 0x80,
            SPI_MULTIBYTE = 0x40
        }

        public enum Presets
        {
            BYPASS = 0x00,
            FIFO_WITH_INT1 = 0x01
        }

        // Class fields
        private FX3Connection fX3Connection;
        private UInt16 dataX;
        private UInt16 dataY;
        private UInt16 dataZ;

        private bool IsIntSamplingEnabled;

        // Class properties
        public UInt16 DataX { get => dataX; }
        public UInt16 DataY { get => dataY; }
        public UInt16 DataZ { get => dataZ; }

        public Int32 DataX_S32 { get => ComplementToSigned32(dataX); }
        public Int32 DataY_S32 { get => ComplementToSigned32(dataY); }
        public Int32 DataZ_S32 { get => ComplementToSigned32(dataZ); }
        //public enum RegisterAddress { get => _registerAddress, };

        /// <summary>
        /// Constructor that require USB reference coming from FX3Api.FX3Connection
        /// </summary>
        /// <param name="fX3Connection"></param>
        public ADXL345EB(FX3Connection fX3Connection)
        {
            this.fX3Connection = fX3Connection;
            this.IsIntSamplingEnabled = false;
            //this.fX3Connection.ReadPins(fX3Connection.DIO1);
        }

        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="complement"></param>
        /// <returns></returns>
        public Int32 ComplementToSigned32(UInt16 complement)
        {

            if ((complement & 0x8000) == 0x8000)
            {
                return (Int32)((UInt32)0xFFFF0000 | (UInt32)complement);
            }
            else
            {
                return (Int32)complement;
            }

        }

        public void Deserialize(Byte[] serialData)
        {
            if (serialData.Length == MaxFifoLength + InstLength)
            {
                UInt16 temp1 = serialData[2];
                dataX = (UInt16)(temp1 << 8);
                dataX = (UInt16)(dataX | serialData[1]);

                UInt16 temp2 = serialData[4];
                dataY = (UInt16)(temp2 << 8);
                dataY = (UInt16)(dataY | serialData[3]);

                UInt16 temp3 = serialData[6];
                dataZ = (UInt16)(temp3 << 8);
                dataZ = (UInt16)(dataZ | serialData[5]);
            }
        }

        public void Configure(byte[] MOSI)
        {
            int misoLength = 2;
            int numBits = 8 * misoLength;
            // Not yet being used
            byte[] MISO = new byte[misoLength];

            fX3Connection.BitBangSpiConfig = new BitBangSpiConfig(true);

            MISO = fX3Connection.BitBangSpi((uint)numBits, 1, MOSI, SpiTimeout);

            fX3Connection.RestoreHardwareSpi();
        }

        public void WriteRegister(RegisterAddress registerAddress, byte registerValue)
        {
            int misoLength = 2;
            int numBits = 8 * misoLength;

            byte[] MOSI = new byte[misoLength];

            fX3Connection.BitBangSpiConfig = new BitBangSpiConfig(true);

            MOSI[0] = (byte)((byte)SpiInstByte.SPI_WRITE | (byte)registerAddress);
            MOSI[1] = registerValue;
            fX3Connection.BitBangSpi((uint)numBits, 1, MOSI, SpiTimeout);

            fX3Connection.RestoreHardwareSpi();
        }

        public byte ReadRegister(RegisterAddress registerAddress)
        {
            int misoLength = 2;
            int numBits = 8 * misoLength;
           
            byte[] MISO, MOSI = new byte[misoLength];

            fX3Connection.BitBangSpiConfig = new BitBangSpiConfig(true);

            MOSI[0] = (byte)((byte)SpiInstByte.SPI_READ | (byte)registerAddress);
            MISO = fX3Connection.BitBangSpi((uint)numBits, 1, MOSI, SpiTimeout);

            fX3Connection.RestoreHardwareSpi();

            return MISO[1];
        }

        public void GetXYZFIFO()
        {
            if(IsIntSamplingEnabled == true && fX3Connection.ReadPin(fX3Connection.DIO1) == 0x00)
            {

            }
            else
            {
                int misoLength = MaxFifoLength + InstLength;
                int numBits = 8 * misoLength;

                byte[] MOSI = new byte[]
                {
                (byte)SpiInstByte.SPI_READ | (byte)SpiInstByte.SPI_MULTIBYTE | (byte)RegisterAddress.DATAX0,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00
                };
                byte[] MISO = new byte[misoLength];

                fX3Connection.BitBangSpiConfig = new BitBangSpiConfig(true);

                MISO = fX3Connection.BitBangSpi((uint)numBits, 1, MOSI, SpiTimeout);
                Deserialize(MISO);

                fX3Connection.RestoreHardwareSpi();
            }               
        }

        public void SetPreset(Presets preset)
        {
            switch(preset)
            {
                case Presets.BYPASS:
                    WriteRegister(RegisterAddress.FIFO_CTL, 0x00);
                    WriteRegister(RegisterAddress.INT_ENABLE, 0x00);
                    IsIntSamplingEnabled = false;
                    break;
                case Presets.FIFO_WITH_INT1:
                    WriteRegister(RegisterAddress.FIFO_CTL, 0x47);
                    WriteRegister(RegisterAddress.INT_ENABLE, 0x80);
                    IsIntSamplingEnabled = true;
                    break;
                default:
                    WriteRegister(RegisterAddress.FIFO_CTL, 0x00);
                    WriteRegister(RegisterAddress.INT_ENABLE, 0x00);
                    IsIntSamplingEnabled = false;
                    break;

            }
                   
        }

        public void GetXYZFIFO_Interrupt(bool isInit)
        {

            if (isInit == false)
            {
                WriteRegister(RegisterAddress.FIFO_CTL, 0x47);
                WriteRegister(RegisterAddress.INT_ENABLE, 0x80);
            }

            // check interrupt
            if (fX3Connection.ReadPin(fX3Connection.DIO1) != 0x00)
            {
                GetXYZFIFO();
            }          
        }
    }
}

