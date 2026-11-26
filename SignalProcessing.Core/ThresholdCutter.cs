using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    internal class ThresholdCutter
    {
        private SignalChannelSignal _signal;

        
        public void SetSignal(SignalChannelSignal signal)
        {
            _signal = signal;

        }
        public SignalChannelSignal GetSignal()
        {
            return _signal;
        }
        public void CutUp(float upThr)
        {
            for(int i = 0; i < _signal.Value.Count(); i++)
            {
                if (_signal.Value[i] > upThr) {
                    _signal.Value[i] = upThr;
                }
            }
        }
        public void CutDown(float downThr)
        {
            for (int i = 0; i < _signal.Value.Count(); i++)
            {
                if (_signal.Value[i] < downThr)
                {
                    _signal.Value[i] = downThr;
                }
            }
        }
        public void CutUpAndDown(float upThr, float downThr)
        {
            for (int i = 0; i < _signal.Value.Count(); i++)
            {
                if (_signal.Value[i] > upThr)
                {
                    _signal.Value[i] = upThr;
                }
                else if (_signal.Value[i] < downThr)
                {
                    _signal.Value[i] = downThr;
                }
            }
        }
    }
}
