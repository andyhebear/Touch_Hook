using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ICTest
{
    public abstract class BaseHandler : IDisposable, IInteractionHandler
    {
        IntPtr _context;
        int _lastFrameID = -1;

        HashSet<int> _activePointers = new HashSet<int>();

        public BaseHandler()
        {
            _context = Win32.CreateInteractionContext(this, SynchronizationContext.Current);

            //Win32.SetPropertyInteractionContext(Context, Win32.INTERACTION_CONTEXT_PROPERTY.FILTER_POINTERS, Win32.ICP_FILTER_POINTERS_ON);
        }

        public void Dispose()
        {
            Win32.DisposeInteractionContext(_context);
            _context = IntPtr.Zero;
        }

        public IntPtr Context
        {
            get { return _context; }
        }

        void IInteractionHandler.ProcessInteractionEvent(InteractionOutput output)
        {
            ProcessEvent(output);
        }

        internal abstract void ProcessEvent(InteractionOutput output);


        public void ProcessPointerFrames(int pointerID, int frameID)
        {
            if (_lastFrameID != frameID)
            {
                _lastFrameID = frameID;
                int entriesCount = 0;
                int pointerCount = 0;
                if (!Win32.GetPointerFrameInfoHistory(pointerID, ref entriesCount, ref pointerCount, IntPtr.Zero))
                {
                    Win32.CheckLastError();
                }
                Win32.POINTER_INFO[] piArr = new Win32.POINTER_INFO[entriesCount * pointerCount];
                if (!Win32.GetPointerFrameInfoHistory(pointerID, ref entriesCount, ref pointerCount, piArr))
                {
                    Win32.CheckLastError();
                }
                //System.Windows.Forms.MessageBox.Show(string.Format("ec:{0} pc:{1} piArr:{2}",entriesCount,pointerCount,piArr.Length));
                IntPtr hr = Win32.ProcessPointerFramesInteractionContext(_context, (UInt32)entriesCount, (UInt32)pointerCount, piArr);
                if (Win32.FAILED(hr))
                {
                    Debug.WriteLine("ProcessPointerFrames failed: " + Win32.GetMessageForHR(hr));
                }
            }
        }

        public HashSet<int> ActivePointers
        {
            get { return _activePointers; }
        }

        public void AddPointer(int pointerID)
        {
            Win32.AddPointerInteractionContext(_context, (UInt32)pointerID);
            _activePointers.Add(pointerID);
        }

        public void RemovePointer(int pointerID)
        {
            Win32.RemovePointerInteractionContext(_context, pointerID);
            _activePointers.Remove(pointerID);
        }

        public void StopProcessing()
        {
            foreach (int pointerID in _activePointers)
            {
                Win32.RemovePointerInteractionContext(_context, pointerID);
            }
            _activePointers.Clear();
            Win32.StopInteractionContext(_context);
        }
    }
}
