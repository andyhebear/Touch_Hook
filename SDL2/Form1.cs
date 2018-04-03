using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
//using SDL2;
namespace SDL2
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
            this.textBox1.SendToBack();
            this.textBox1.ReadOnly = true;
        }
        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            //
            int ret_init = SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            if (ret_init == -1) {
                throw new Exception("Could not initialize SDL!");
            }
            uint retii = SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO);
            _sdl_formhandle = SDL.SDL_CreateWindowFrom(this.panel1.Handle);

            _sdl_initSDL2 = true;
            SDL.SDL_version ver;
            SDL.SDL_VERSION(out ver);
            this.textBox1.AppendText(string.Format("\r\nSDL {0}.{1}.{2}",
    ver.major,
    ver.minor,
    ver.patch
));
            SDL.SDL_RenderPresent(_sdl_formhandle); //刷新屏幕
            this.Focus();
            this.timer1.Start();
        }
        bool _sdl_initSDL2 = false;
        IntPtr _sdl_formhandle = IntPtr.Zero;
        //IntPtr glContext = IntPtr.Zero;
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
        }
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            SDL.SDL_RenderPresent(_sdl_formhandle); //刷新屏幕
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            this.timer1.Stop();
            this.timer1.Dispose();
            //SDL.SDL_GL_DeleteContext(glContext);
            //glContext = IntPtr.Zero;
            SDL.SDL_SetHintWithPriority(
                       SDL.SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS,
                       "0",
                       SDL.SDL_HintPriority.SDL_HINT_OVERRIDE
                   );
            SDL.SDL_DestroyWindow(this._sdl_formhandle);
            _sdl_formhandle = IntPtr.Zero;
            SDL.SDL_Quit();
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (!_sdl_initSDL2) return;
            SDL.SDL_Event _event;
            while (SDL.SDL_PollEvent(out _event) != 0) {
                HasTouch();
                switch (_event.type) {
                    case SDL.SDL_EventType.SDL_FINGERDOWN://触碰按下                       
                        this.textBox1.AppendText("\r\ntouchdown:" + _event.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_FINGERUP://触碰放开
                        this.textBox1.AppendText("\r\ntouchup:" + _event.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_FINGERMOTION://触碰移动
                        this.textBox1.AppendText("\r\ntouchmove:" + _event.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        this.textBox1.AppendText("\r\nkeydown:" + _event.key.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        this.textBox1.AppendText("\r\nkeyup:" + _event.key.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_LASTEVENT:
                        this.textBox1.AppendText("\r\nkeypress:" + _event.key.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        this.textBox1.AppendText("\r\nmousedown:" + _event.button.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                        this.textBox1.AppendText("\r\nmouseup:" + _event.button.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        this.textBox1.AppendText("\r\nmousemove:" + _event.motion.ToString());
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                        this.textBox1.AppendText("\r\nmouseWheel:"+_event.wheel.ToString());
                        break;

                    default: break;
                }

            }
            //SDL.SDL_RenderPresent(_sdl_formhandle); //刷新屏幕
        }

        public bool HasTouch() {
            int tc = SDL.SDL_GetNumTouchDevices();
            this.textBox1.AppendText("\r\n触碰:" + tc.ToString());
            return tc > 0;
        }

    }
}
