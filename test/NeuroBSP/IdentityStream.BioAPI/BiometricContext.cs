using System;
using System.Collections.Generic;
using System.Threading;

namespace IdentityStream.BioAPI
{
    /// <summary>
    /// The BiometricContext class is responsible for providing a wrapper for BSPs
    /// It will keep a singleton instance of itself using the Current property
    /// The CaptureBSPs method will return all the BSP classes available
    /// In order to work with the BSPs, make a call to CreateCaptureSession and pass in the BSPs you would like to use
    /// 
    /// Be sure to clean up the sessions by calling EndCaptureSession
    /// In this first implementation, only one session will be allowed at a time
    /// </summary>
    public class BiometricContext
    {
        private static Mutex mutex = new Mutex(false, "IdentityStream");
        private CaptureSession _session = null;

        static BiometricContext()
        {
            CaptureBSPs = new List<Type>() { Type.GetType("IdentityStream.BSPs.Capture.Neuro.NeuroCapture,IdentityStream.BSPs.Capture.Neuro") };
            BiometricContext.Current = new BiometricContext();
        }

        public BiometricContext()
        {

        }

        public static BiometricContext Current { get; private set; }

        public static List<Type> CaptureBSPs { get; set; }

        private CaptureSession CreateCaptureSession(List<Type> BSPs)
        {
            return new CaptureSession(BSPs);
        }

        /// <summary>
        /// This method establishes a biometric session.
        /// Only one session may be created at once. Call BiometricContext.EndSession in order to end the session.
        /// </summary>
        /// <returns>IdentityStream.BioAPI.CaptureSession</returns>
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="IdentityStream.BioAPI.BspLoadException" />
        /// <exception cref="IdentityStream.BioAPI.LicenseNotAvailableException" />
        /// <exception cref="IdentityStream.BioAPI.LicensingException" />
        /// <exception cref="IdentityStream.BioAPI.SessionInProgressException" />
        public CaptureSession CreateCaptureSession()
        {
            if (mutex.WaitOne(TimeSpan.FromSeconds(1), false))
            {
                _session = CreateCaptureSession(CaptureBSPs);
                return _session;
            }
            throw new SessionInProgressException("Unable to create a new session. A session is still in progress.");
        }

        public void EndCaptureSession()
        {
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
                mutex.ReleaseMutex();
            }
        }
    }

    public class SessionInProgressException : Exception
    {
        public SessionInProgressException(string Message)
            : base(Message)
        {

        }
    }
}
