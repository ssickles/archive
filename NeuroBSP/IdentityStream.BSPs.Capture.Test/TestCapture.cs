using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityStream.BioAPI;
using System.Threading;

namespace IdentityStream.BSPs.Capture.Test
{
    public class TestCapture: ICapture
    {
        private bool _isCapturing = false;
        private Timer _timer1;
        private Timer _timer2;
        private Timer _timer3;

        public TestCapture()
        {
            _timer1 = new Timer(new TimerCallback(_timer1_Elapsed), null, 0, 5000);
            _timer2 = new Timer(new TimerCallback(_timer2_Elapsed), null, 5100, 5000);
            _timer3 = new Timer(new TimerCallback(_timer3_Elapsed), null, 5200, 5000);
        }

        private void _timer3_Elapsed(object input)
        {
            if (_isCapturing)
                OnSourceRemoved();
        }

        private void _timer2_Elapsed(object input)
        {
            if (_isCapturing)
                OnTemplateCaptured(new TemplateCapturedEventArgs(new Template(new byte[16], 20), TemplateCapturedStatus.Success));
        }

        private void _timer1_Elapsed(object input)
        {
            if (_isCapturing)
                OnSourcePlaced();
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
            _isCapturing = true;
            return StartCapturingStatus.Success;
        }

        public void StopCapturing()
        {
            _isCapturing = false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
