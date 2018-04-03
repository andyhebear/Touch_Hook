using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace ICTest
{
    public class Background : BaseHandler
    {
        TouchForm _form;
        int _n = 1;

        public Background(TouchForm form)
            : base()
        {
            _form = form;

            Win32.INTERACTION_CONTEXT_CONFIGURATION[] cfg = new Win32.INTERACTION_CONTEXT_CONFIGURATION[]
            {
                new Win32.INTERACTION_CONTEXT_CONFIGURATION(Win32.INTERACTION.TAP,
                    Win32.INTERACTION_CONFIGURATION_FLAGS.TAP |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.TAP_DOUBLE),

                new Win32.INTERACTION_CONTEXT_CONFIGURATION(Win32.INTERACTION.SECONDARY_TAP,
                    Win32.INTERACTION_CONFIGURATION_FLAGS.SECONDARY_TAP),

                new Win32.INTERACTION_CONTEXT_CONFIGURATION(Win32.INTERACTION.HOLD,
                    Win32.INTERACTION_CONFIGURATION_FLAGS.HOLD)
            };

            Win32.SetInteractionConfigurationInteractionContext(Context, cfg.Length, cfg);
        }

        internal override void ProcessEvent(InteractionOutput output)
        {
            if (output.Data.Interaction == Win32.INTERACTION.TAP)
            {
                if (output.Data.Tap.Count == 2)
                {
                    foreach (Figure f in _form.Figures)
                    {
                        f.ResetPoints();
                    }
                    _form.Invalidate();
                }
            }
            else if (output.Data.Interaction == Win32.INTERACTION.SECONDARY_TAP)
            {
                _n++;
                if (_n > 3)
                {
                    _n = 1;
                }
                Color c = Color.Green;
                switch (_n)
                {
                    case 1:
                        c = Color.FromArgb(64, 64, 64);
                        break;
                    case 2:
                        c = Color.Maroon;
                        break;
                    case 3:
                        c = Color.DarkGreen;
                        break;
                }
                _form.BackColor = c;
            }
        }
    }
}
