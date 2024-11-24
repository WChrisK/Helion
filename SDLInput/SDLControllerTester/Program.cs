namespace SDLControllerTester
{
    using SDLControllerWrapper;
    using System;
    using System.Timers;

    public static class Program
    {
        private static SDLControllerWrapper? _wrapper;
        private static int _mode;

        private const float ANALOGTHRESHOLD = .1f;

        public static void Main()
        {
            using (_wrapper = new SDLControllerWrapper(HandleJoystickChangeEvent))
            {
                ConsoleKeyInfo key = default;
                while (key.Key != ConsoleKey.G && key.Key != ConsoleKey.A)
                {
                    Console.WriteLine("G for Gamepad mode, or A to test accelerometers/gyros on a controller");
                    key = Console.ReadKey();
                }

                switch (key.Key)
                {
                    case ConsoleKey.G:
                        _mode = 1;
                        break;
                    case ConsoleKey.A:
                        _mode = 2;
                        break;
                }


                Timer timer = new Timer()
                {
                    Interval = 50,
                    AutoReset = true,
                };
                timer.Elapsed += TimerElapsed;
                timer.Start();

                Console.Clear();
                Console.WriteLine("Press Q to quit");

                while (Console.ReadKey().Key != ConsoleKey.Q)
                {
                }
            }
        }

        private static void HandleJoystickChangeEvent(object? sender, ConfigurationEvent evt)
        {
            Console.WriteLine($"Configuration change: {evt.JoystickIndex} {evt.ChangeType}");
        }


        private static void PollControllers()
        {
            _wrapper!.Poll();

            if (!(_wrapper!.Controllers.Count > 0))
            {
                return;
            }

            Controller controller = _wrapper.Controllers[0];


            for (int i = 0; i < controller.CurrentAxisValues.Length; i++)
            {
                float axisDelta = Math.Abs(controller.CurrentAxisValues[i] - controller.PreviousAxisValues[i]);
                if (axisDelta > ANALOGTHRESHOLD)
                {
                    Console.WriteLine($"{(Axis)i}: {controller.CurrentAxisValues[i]}");
                }
            }

            for (int i = 0; i < controller.CurrentButtonValues.Length; i++)
            {
                if (controller.CurrentButtonValues[i] != controller.PreviousButtonValues[i])
                {
                    Console.WriteLine($"{(Button)i}: {controller.CurrentButtonValues[i]}");
                    if (i < (int)Button.DPad_Up || i > (int)Button.Dpad_Right)
                    {
                        controller.Rumble(0, 0xFFFF, 100);
                    }
                }
            }

            if (controller.CurrentDPadValue != controller.PreviousDPadValue)
            {
                Console.WriteLine($"DPad: {controller.CurrentDPadValue}");
            }
        }

        private static void PollAccelAndGyro()
        {
            _wrapper!.Poll();

            if (!(_wrapper!.Controllers.Count > 0))
            {
                return;
            }

            Controller controller = _wrapper.Controllers[0];

            Console.SetCursorPosition(0, 1);

            for (int i = 0; i < controller.CurrentAccelValues.Length; i++)
            {
                Console.WriteLine($"Accelerometer {(AccelAxis)i}: {controller.CurrentAccelValues[i]}");
            }

            for (int i = 0; i < controller.CurrentGyroValues.Length; i++)
            {
                Console.WriteLine($"Gyro {(GyroAxis)i}: {controller.CurrentGyroValues[i]}");
            }
        }

        private static void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            switch (_mode)
            {
                case 1:
                    PollControllers();
                    break;
                case 2:
                    PollAccelAndGyro();
                    break;
            }
        }
    }
}
