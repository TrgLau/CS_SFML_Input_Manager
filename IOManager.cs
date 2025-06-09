using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class MouseManager
{
    private RenderWindow _window; 

    public Vector2f Position { get; private set; } // position actuelle

    /// <summary>
    /// Bools pour gestion des clics
    /// </summary>
    public bool LeftClicked { get; private set; }
    public bool LeftDown { get; private set; }
    public bool RightClicked { get; private set; }
    public bool RightDown { get; private set; }
    public bool MiddleClicked { get; private set; }
    public bool MiddleDown { get; private set; }
    public bool XButton1Clicked { get; private set; }
    public bool XButton1Down { get; private set; }
    public bool XButton2CLicked { get; private set; }
    public bool XButton2Down { get; private set; }
    /*///////////////////////////////////////////// */


    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="window"> RenderWindow de la fenetre </param>

    public MouseManager(RenderWindow window) {
        
    
        _window = window;

        _window.MouseMoved += OnMouseMoved; // pour gerer la position des que ça bouge
        _window.MouseButtonPressed += OnMouseButtonPressed; // gestion clic
        _window.MouseButtonReleased += OnMouseButtonReleased;   // gestion declic
    }
    /// <summary>
    ///  méthode gestion du mouvement sourie
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMouseMoved(object sender, MouseMoveEventArgs e)
    {
        Position = new Vector2f(e.X, e.Y); // on update la nouvelle position // FIXME peu être gerer un List pour historique ??
    }
    /// <summary>
    ///  gestion du clic sur un bouton sourie
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
    {
        if (e.Button == Mouse.Button.Left)
        {
            LeftDown = true;
            LeftClicked = true;
        }
        else if (e.Button == Mouse.Button.Right)
        {
            RightDown = true;
            RightClicked = true;
        }
        else if (e.Button == Mouse.Button.Middle)
        {
            MiddleDown = true;
            MiddleClicked = true;
        }
        else if (e.Button == Mouse.Button.XButton1)
        {
            XButton1Down = true;
            XButton2CLicked = true;
        }
        else if (e.Button == Mouse.Button.XButton2)
        {
            XButton2Down = true;
            XButton2CLicked = true;
        }
    }
    /// <summary>
    ///  méthode de gestion du release des boutons de la sourie
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
    {
        if (e.Button == Mouse.Button.Left)
        {
            LeftDown = false;
        }
        else if (e.Button == Mouse.Button.Right)
        {
            RightDown = false;
        }
        else if (e.Button == Mouse.Button.Middle)
        {
            MiddleDown = false;
        }
        else if (e.Button == Mouse.Button.XButton1)
        {
            XButton1Down = false;
        }
        else if (e.Button == Mouse.Button.XButton2)
        {
            XButton2Down = false;
        }
    }

    /// <summary>
    /// pour réinitialiser les clics
    /// </summary>
    public void EndFrame()
    {
        LeftClicked = false;
        RightClicked = false;
        MiddleClicked = false;
        XButton1Clicked = false;
        XButton2CLicked = false;
    }
}

public class KeyboardManager
{
    
    private RenderWindow _window;
    private bool[] _keyDown = new bool[256]; 
    public bool[] KeysPressed { get; private set; } 

    public KeyboardManager(RenderWindow window)
    {
        _window = window;
        ResetKeys();
        SubscribeToEvents();
    }


    public void EndFrame()
    {
        KeysPressed = new bool[256];
    }


    // evenements clavier
    private void SubscribeToEvents()
    {
        _window.KeyPressed += OnKeyPressed;
        _window.KeyReleased += OnKeyReleased;
    }

    /// <summary>
    /// Gestion pression sur une touche
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnKeyPressed(object sender, KeyEventArgs e)
    {
        if (e.Code >= 0 )
        {
            _keyDown[(int)e.Code] = true;
            KeysPressed[(int)e.Code] = true;
        }
    }

    /// <summary>
    /// Gestion du relachement touche
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnKeyReleased(object sender, KeyEventArgs e)
    {
        if (e.Code >= 0 )
        {
            _keyDown[(int)e.Code] = false;
        }
    }

    // Réinit
    private void ResetKeys()
    {
        for (int i = 0; i < _keyDown.Length; i++)
        {
            _keyDown[i] = false;
        }
        KeysPressed = new bool[256];
    }
}

public class JoystickManager
{
    public uint MaxJoysticks = Joystick.Count;

    public class JoystickState
    {
        public bool IsConnected;
        public float AxisX, AxisY, AxisZ, AxisR, AxisU, AxisV;
        public bool[] ButtonsDown;
        public bool[] ButtonsPressed;

        public JoystickState()
        {
            ButtonsDown = new bool[Joystick.ButtonCount];
            ButtonsPressed = new bool[Joystick.ButtonCount];
        }
    }

    public Dictionary<uint, JoystickState> Joysticks { get; private set; }

    public JoystickManager()
    {
        Joysticks = new Dictionary<uint, JoystickState>();

        for (uint id = 0; id < MaxJoysticks; id++)
        {
            Joysticks[id] = new JoystickState();
        }
    }

    public void Update()
    {
        for (uint id = 0; id < MaxJoysticks; id++)
        {
            var state = Joysticks[id];
            state.IsConnected = Joystick.IsConnected(id);

            if (!state.IsConnected)
                continue;

            // Axes
            state.AxisX = Joystick.GetAxisPosition(id, Joystick.Axis.X);
            state.AxisY = Joystick.GetAxisPosition(id, Joystick.Axis.Y);
            state.AxisZ = Joystick.GetAxisPosition(id, Joystick.Axis.Z);
            state.AxisR = Joystick.GetAxisPosition(id, Joystick.Axis.R);
            state.AxisU = Joystick.GetAxisPosition(id, Joystick.Axis.U);
            state.AxisV = Joystick.GetAxisPosition(id, Joystick.Axis.V);

            // Boutons
            for (uint b = 0; b < Joystick.ButtonCount; b++)
            {
                bool isPressed = Joystick.IsButtonPressed(id, b);
                state.ButtonsPressed[b] = isPressed && !state.ButtonsDown[b];
                state.ButtonsDown[b] = isPressed;
            }
        }
    }

    public void EndFrame()
    {
        foreach (var state in Joysticks.Values)
        {
            if (!state.IsConnected) continue;
            for (int i = 0; i < Joystick.ButtonCount; i++)
            {
                state.ButtonsPressed[i] = false;
            }
        }
    }
}

