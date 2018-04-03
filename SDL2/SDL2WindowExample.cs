#region License
/* SDL2 Window for System.Windows.Forms Example
 * Written by Ethan "flibitijibibo" Lee
 * http://www.flibitijibibo.com/
 *
 * Released under public domain.
 * No warranty implied; use at your own risk.
 */
#endregion

#region Using Statements
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using SDL2;
#endregion

public class SDL2WindowExample : Form
{
    #region Private SDL2 Window/Control Variables

    // These are the variables you care about.
    private Panel gamePanel;
    private IntPtr gameWindow; // For FNA, this is Game.Window.Handle

    #endregion

    #region Private GL Variables

    // IGNORE MEEEEE
    private Random random;
    private IntPtr glContext;
    private delegate void Viewport(int x, int y, int width, int height);
    private delegate void ClearColor(float r, float g, float b, float a);
    private delegate void Clear(uint flags);
    private Viewport glViewport;
    private ClearColor glClearColor;
    private Clear glClear;

    #endregion

    #region WinAPI Entry Points

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowPos(
        IntPtr handle,
        IntPtr handleAfter,
        int x,
        int y,
        int cx,
        int cy,
        uint flags
    );
    [DllImport("user32.dll")]
    private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);
    [DllImport("user32.dll")]
    private static extern IntPtr ShowWindow(IntPtr handle, int command);

    #endregion

    #region Form Constructor

    public SDL2WindowExample() {
        // This is what we're going to attach the SDL2 window to
        gamePanel = new Panel();
        gamePanel.Size = new Size(640, 480);
        gamePanel.Location = new Point(80, 10);
        
        // Make the WinForms window
        Size = new Size(800, 600);
        FormClosing += new FormClosingEventHandler(WindowClosing);
        Button button = new Button();
        button.Text = "Whatever";
        button.Location = new Point(
            (Size.Width / 2) - (button.Size.Width / 2),
            gamePanel.Location.Y + gamePanel.Size.Height + 10
        );
        button.Click += new EventHandler(ClickedButton);
        Controls.Add(button);
        Controls.Add(gamePanel);

        // IGNORE MEEEEE
        random = new Random();
        SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
        gameWindow = SDL.SDL_CreateWindow(
            String.Empty,
            0,
            0,
            gamePanel.Size.Width,
            gamePanel.Size.Height,
            SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL
        );
        glContext = SDL.SDL_GL_CreateContext(gameWindow);
        glViewport = (Viewport)Marshal.GetDelegateForFunctionPointer(
            SDL.SDL_GL_GetProcAddress("glViewport"),
            typeof(Viewport)
        );
        glClearColor = (ClearColor)Marshal.GetDelegateForFunctionPointer(
            SDL.SDL_GL_GetProcAddress("glClearColor"),
            typeof(ClearColor)
        );
        glClear = (Clear)Marshal.GetDelegateForFunctionPointer(
            SDL.SDL_GL_GetProcAddress("glClear"),
            typeof(Clear)
        );
        glViewport(0, 0, gamePanel.Size.Width, gamePanel.Size.Height);
        glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        glClear(0x4000);
        SDL.SDL_GL_SwapWindow(gameWindow);

        // Get the Win32 HWND from the SDL2 window
        SDL.SDL_SysWMinfo info = new SDL.SDL_SysWMinfo();
        SDL.SDL_GetWindowWMInfo(gameWindow, ref info);
        IntPtr winHandle = info.info.win.window;

        // Move the SDL2 window to 0, 0
        SetWindowPos(
            winHandle,
            Handle,
            0,
            0,
            0,
            0,
            0x0401 // NOSIZE | SHOWWINDOW
        );

        // Attach the SDL2 window to the panel
        SetParent(winHandle, gamePanel.Handle);
        ShowWindow(winHandle, 1); // SHOWNORMAL
    }

    #endregion

    #region Button Event Method

    private void ClickedButton(object sender, EventArgs e) {
        glClearColor(
            (float)random.NextDouble(),
            (float)random.NextDouble(),
            (float)random.NextDouble(),
            1.0f
        );
        glClear(0x4000); // GL_COLOR_BUFFER_BIT
        SDL.SDL_GL_SwapWindow(gameWindow);
    }

    #endregion

    #region Window Close Method

    private void WindowClosing(object sender, FormClosingEventArgs e) {
        glClear = null;
        glClearColor = null;
        glViewport = null;
        SDL.SDL_GL_DeleteContext(glContext);
        glContext = IntPtr.Zero;
        SDL.SDL_DestroyWindow(gameWindow);
        gameWindow = IntPtr.Zero;
        SDL.SDL_Quit();
    }

    #endregion

   
}