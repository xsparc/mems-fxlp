using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mems_fx3lp.API
{
    partial class FX3Peripheral
    {
        /// <summary>
        /// Data transfer on the control endpoint with timeout
        /// </summary>
        /// <param name="buf">The buffer to hold the data</param>
        /// <param name="numBytes">Number of bytes to transfer</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns>Returns true if when execution time exceeds timeout value</returns>
        private bool XferControlData(ref Byte[] buf, Int32 numBytes, Int32 timeout)
        {
            Stopwatch startTime = new Stopwatch();
            Boolean validTransfer = true;

            // Get control endpoint mutex
            validTransfer = m_ControlMutex.WaitOne(timeout);

            // Abort routine when mutex can't be acquired
            if (!validTransfer)
            {
                // Indicator for debug. TODO: replace with proper error handling.
                Console.WriteLine("Could not acquire control endpoint");
                return false;
            }

            // Get pointer for the FX3 target
            FX3ControlEndPt = m_ActiveFX3.ControlEndPt;

            // Execute transfer
            startTime.Start();
            validTransfer = FX3ControlEndPt.XferData(ref buf, ref numBytes);
            startTime.Stop();

            // Check if execution goes beyond timout
            if (startTime.ElapsedMilliseconds > timeout)
            {
                validTransfer = false;
            }
            else
            {
                validTransfer = true;
            }

            // Release the mutex
            m_ControlMutex.ReleaseMutex();

            return validTransfer;
        }

    }
}
