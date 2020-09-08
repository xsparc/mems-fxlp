using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CyUSB;

namespace mems_fx3lp.API
{
    partial class FX3Peripheral
    {
        // CyUSB object for the FX3Device 
        private CyUSB.CyFX3Device m_ActiveFX3 = null;

        // Status check if a FX3Device is successfully connected 
        private bool m_FX3Connected;

        // CyUSB control endpoint object
        private CyUSB.CyControlEndPoint FX3ControlEndPt;

        // Mutex for control endpoint
        private Mutex m_ControlMutex;
    }
}
