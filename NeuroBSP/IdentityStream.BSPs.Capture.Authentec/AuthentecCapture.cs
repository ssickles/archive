using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IdentityStream.BioAPI;

namespace IdentityStream.BSPs.Capture.Authentec
{
    public class AuthentecCapture: ICapture
    {
        #region DllImport
        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_BSPLoad(
            BioAPI_UUID BSPUuid, 
            BioSPI_EventHandler BioAPINotifyCallback, 
            BioSPI_BFP_ENUMERATION_HANDLER BFPEnumerationHandler,
            BioSPI_MEMORY_FREE_HANDLER MemoryFreeHandler);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_BSPUnLoad(
            BioAPI_UUID BSPUuid);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_BSPCapture(
            BioAPI_HANDLE BSPHandle, 
            BioAPI_BIR_PURPOSE Purpose, 
            BioAPI_BIR_SUBTYPE Subtype, 
            BioAPI_BIR_BIOMETRIC_DATA_FORMAT OutputFormat,
            BioAPI_BIR_HANDLE CapturedBIR,
            int Timeout,
            BioAPI_BIR_HANDLE AuditData);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_GetBIRFromHandle(
            BioAPI_HANDLE BSPHandle,
            BioAPI_BIR_HANDLE Handle,
            BioAPI_BIR BIR);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_Free(
            object Ptr);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_SetGUICallbacks(
            BioAPI_HANDLE BSPHandle,
            BioAPI_GUI_STREAMING_CALLBACK GuiStreamingCallback,
            object GuiStreamingCallbackCtx,
            BioAPI_GUI_STATE_CALLBACK GuiStateCallback,
            object GuiStateCallbackCtx);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_Cancel(
            BioAPI_HANDLE BSPHandle);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_QueryUnits(
            BioAPI_UUID BSPUuid,
            BioAPI_UNIT_SCHEMA UnitSchemaArray,
            int NumberOfElements);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_BSPAttach(
            BioAPI_UUID BSPUuid, 
            BioAPI_VERSION version, 
            BioAPI_UNIT_LIST_ELEMENT UnitList, 
            int NumUnits, 
            BioAPI_HANDLE BSPHandle);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_BSPDetach(
            BioAPI_HANDLE BSPHandle);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_FreeBIRHandle(
            BioAPI_HANDLE BSPHandle,
            BioAPI_BIR_HANDLE Handle);

        [DllImport("AuthentecBSP20.dll")]
        private extern static BioAPI_RETURN BioSPI_EnableEvents(
            BioAPI_HANDLE BSPHandle,
            BioAPI_EVENT_MASK Events);
        #endregion

        private BioAPI_HANDLE _handle;

        public AuthentecCapture()
        {

        }

        #region ICapture Members

        public event EventHandler SourcePlaced;

        private void OnSourcePlaced()
        {
            EventHandler obj = SourcePlaced;
            if (obj != null)
            {
                obj(this, EventArgs.Empty);
            }
        }

        public event EventHandler SourceRemoved;

        private void OnSourceRemoved()
        {
            EventHandler obj = SourceRemoved;
            if (obj != null)
            {
                obj(this, EventArgs.Empty);
            }
        }

        public event EventHandler<TemplateCapturedEventArgs> TemplateCaptured;

        private void OnTemplateCaptured(TemplateCapturedEventArgs e)
        {
            EventHandler<TemplateCapturedEventArgs> obj = TemplateCaptured;
            if (obj != null)
            {
                obj(this, e);
            }
        }

        public StartCapturingStatus StartCapturing()
        {
            //call BioSPI_BSPCapture on a different thread so that we can use the Cancel method and receive Gui callbacks
            //BioSPI_BSPCapture(_handle, BioAPI_BIR_PURPOSE, BioAPI_BIR_SUBTYPE, BioAPI_BIR_BIOMETRIC_DATA_FORMAT, BioAPI_BIR_HANDLE, Timeout, BioAPI_BIR_HANDLE);
            return StartCapturingStatus.Success;
        }

        public void StopCapturing()
        {
            //BioSPI_Cancel(_handle);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public enum BioAPI_RETURN
    {
        BioAPI_OK,
        BioAPIERR_INCOMPATIBLE_VERSION,
        BioAPIERR_BSP_LOAD_FAIL,
        BioAPIERR_INVALID_UUID,
        BioAPIERR_BSP_NOT_LOADED,
        BioAPIERR_INVALID_UNIT_ID,
        BioAPIERR_UNIT_IN_USE,
        BioAPIERR_INVALID_CATEGORY
    }
    public struct BioAPI_BSP_SCHEMA
    {
        public BioAPI_UUID BSPUuid;
    }
    public interface BioSPI_EventHandler
    {

    }
    public struct BioAPI_UUID
    {

    }
    public struct BioAPI_HANDLE
    {

    }
    public struct BioAPI_UNIT_LIST_ELEMENT
    {

    }
    public struct BioAPI_BIR_HANDLE
    {

    }
    public enum BioAPI_BIR_PURPOSE
    {

    }
    public enum BioAPI_BIR_SUBTYPE
    {

    }
    public struct BioAPI_BIR_BIOMETRIC_DATA_FORMAT
    {

    }
    public struct BioSPI_BFP_ENUMERATION_HANDLER
    {

    }
    public struct BioSPI_MEMORY_FREE_HANDLER
    {

    }
    public struct BioAPI_BIR
    {

    }
    public struct BioAPI_GUI_STREAMING_CALLBACK
    {

    }
    public struct BioAPI_GUI_STATE_CALLBACK
    {

    }
    public struct BioAPI_UNIT_SCHEMA
    {

    }
    public struct BioAPI_VERSION
    {

    }
    public struct BioAPI_EVENT_MASK
    {

    }
}
