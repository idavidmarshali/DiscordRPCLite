using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordRPC;
using DiscordRPC.Helper;
using DiscordRPC.Message;
using Newtonsoft.Json;

//  MIT License
//
//  Copyright (c) 2021 DavidMarshal
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//      of this software and associated documentation files (the "Software"), to deal
//      in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//      copies or substantial portions of the Software.
//  
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//      IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//      FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//      AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//      LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

namespace DiscordRpcLite
{
    internal class Program
    {
        private static DiscordRpcClient _client; // Main Client Object

        private static Dictionary<string, string> _rpcAtters;
        // Attributes Like ClientID, etc.. are stored in the dict above ^ 

        private static bool _isComplete;
        
        public static void Main(string[] args)
        {
            _isComplete = false;
            start: // Start point, set for Config Validation
            Console.Title = "DiscordRPCLite v1.0.0";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("DiscordRPCLite by DavidMarshal - ver1.0.0");
            Console.ResetColor();
            Console.Write("choose an option :\n 1- Load config from file  |  2- Enter config manually");
            Console.ForegroundColor = ConsoleColor.Yellow;
            AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler;
            config: // Config point, set for Invalid Input
            Write();
            switch (Console.ReadKey().Key) // Checking Input key
            {
                case ConsoleKey.D1: 
                    Write("Loading Config...", clear: true);
                    try { LoadConfig(@"./config.json"); }
                    catch (Exception e) 
                    {
                        Error(e.Message);
                        Error("Failed To Load Config File, Has it been Removed?");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nchoose an option :\n 1- Load config from file  |  2- Enter config manually");
                        Console.ResetColor();
                        goto config;
                    }
                    break;
                case ConsoleKey.D2: 
                    ManualConfig();
                    break;
                
                default:
                    Error("Invalid Key Pressed! Try Again");
                    goto config; 
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nConfig Loaded Successfully!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n--> Please Verify the config:");
            foreach (var item  in _rpcAtters) { Write(item.Value, item.Key); }
            Write(prompt: "1 - Accept and Run,  2- Deny and Reconfig\n ");
            configvalid: // Config Validation Point, set for Invalid Input
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Run();
                    break;
                
                case ConsoleKey.D2:
                    Console.Clear();
                    goto start;
                    
                default:
                    Error("Invalid Key Pressed! Try Again");
                    goto configvalid;
            }
            while (true)
            {
                // Infinite Loop that registers any command input after run
                if (_isComplete == false) {continue;}
                Write();
                ProcessCommand(Console.ReadLine());
            }
        }
        
        /// <summary>
        /// Writes the given message and prompt in console in the following way :
        /// \n{prompt}>> {message}
        /// if clear is set  to true, it will clear the console before writing.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="prompt"></param>
        /// <param name="clear"></param>
        private static void Write(string message = "" , string prompt = "", bool clear = false)
        {
            if (clear) Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n" + prompt + ">> ");
            Console.ResetColor();
            Console.Write(message);
        }
        
        /// <summary>
        /// Fancy way of Writing Errors into Console
        /// </summary>
        /// <param name="message"></param>
        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n[E] ");
            Console.ResetColor();
            Console.Write(message);
        }
        
        /// <summary>
        /// Fancy way of Writing Logs
        /// </summary>
        /// <param name="message"></param>
        private static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n[L] ");
            Console.ResetColor();
            Console.Write(message);
        }
        
        /// <summary>
        /// Global Exception Handler, catches any Unhandled Exceptions
        /// and writes the exception message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            _client.Dispose();
            Exception error = (Exception) args.ExceptionObject;
            Console.Error.WriteLine($"\n----> Unhandled Error Catched\nSource : {error.Source}" +
                                    $" \nData: {error.Data}\nMessage: {error.Message}");
            if (args.IsTerminating)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("This error is going to Terminate the program");
                Console.ResetColor();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// Loads the config from the relative file path given to it into _rpcAtters as a Dict of String:String
        /// </summary>
        /// <param name="filePath"></param>
        private static void LoadConfig(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonText = sr.ReadToEnd();
                _rpcAtters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
            }
        }

        /// <summary>
        /// Sets an RPC attribute in the _rpcatter dict
        /// if key already exsists, it will overwrite it
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SetRpcAtter(string key, string value)
        {
            if (_rpcAtters.ContainsKey(key))
            {
                _rpcAtters[key] = value;
            }
            else _rpcAtters.Add(key, value);
        }
        
        /// <summary>
        /// Function That handles Manual Config Input
        /// </summary>
        private static void ManualConfig()
        {
            _rpcAtters = new Dictionary<string, string>();
            string[] atters =
            {
                "ClientID", "State", "Details", "LargeImageKey", "LargeImageText",
                "SmallImageKey", "SmallImageText", "Button1Text", "Button1URL",
                "Button2Text", "Button2URL"
            };
            foreach (string atter in atters)
            {
                Write(prompt:$"Enter {atter}");
                SetRpcAtter(atter, Console.ReadLine());
            }
            Write(prompt:"Do you want Elapsed Time to be shown?\n 1- Yes,  2- No\n");
            SetRpcAtter("ShowTime", Console.ReadKey().Key == ConsoleKey.D1 ? "Yes" : "No");
        }

        /// <summary>
        /// Main Function that inits the Rpc Client, connects it to discord
        /// and sets the presence based on loaded configs
        /// </summary>
        private static void Run()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("DiscordRPCLite by David Marshal, v1.0.0\n" +
                          "--| Use >>Help to see all the commands!");
            Console.ResetColor();
            Log("Trying to connect to discord...");
            _client = new DiscordRpcClient(_rpcAtters["ClientID"], autoEvents: true);
            AddEvents();
            _client.Initialize();
            UpdatePresence();
        }
        
        /// <summary>
        /// Adds event handlers to the client
        /// </summary>
        private static void AddEvents()
        {
            _client.OnReady += OnReady;
            _client.OnConnectionEstablished += OnConnectionEstablished;
            _client.OnError += OnError;
            _client.OnConnectionFailed += OnConnectionFail;
            _client.OnPresenceUpdate += OnPresenceUpdate;
        }
        
        /// <summary>
        /// Updates the presence from current values of _rpcAtters
        /// </summary>
        private static void UpdatePresence()
        {
            RichPresence presence = new RichPresence
            {
                State = _rpcAtters["State"],
                Details = _rpcAtters["Details"],
                Assets = new Assets()
                {
                    LargeImageKey = _rpcAtters["LargeImageKey"],
                    LargeImageText = _rpcAtters["LargeImageText"],
                    SmallImageKey = _rpcAtters["SmallImageKey"],
                    SmallImageText = _rpcAtters["SmallImageText"]
                },
                Timestamps = _rpcAtters["ShowTime"].ToCamelCase() == "Yes"
                    ? new Timestamps(DateTime.UtcNow)
                    : new Timestamps()
            };
            if (_rpcAtters["Button1Text"] != "" && _rpcAtters["Button2Text"] != "")
            {
                presence.Buttons = new[] {new Button() {
                    Label = _rpcAtters["Button1Text"],
                    Url = _rpcAtters["Button1URL"]}, 
                    new Button()
                    {Label = _rpcAtters["Button2Text"],
                        Url = _rpcAtters["Button2URL"]}
                };
            }else if (_rpcAtters["Button1Text"] != "" || _rpcAtters["Button2Text"] != "")
            {
                int butt = _rpcAtters["Button1Text"] == "" ? 2 : 1;
                presence.Buttons = new[] {new Button() {Label = _rpcAtters[$"Button{butt}Text"], Url = _rpcAtters[$"Button{butt}URL"]}};
            }
            _client.SetPresence(presence);
        }
        
        private static void OnPresenceUpdate(object sender, PresenceMessage message)
        {
            Log($"Your Presence has been update To : {message.Name}");
            if (_isComplete == false){ _isComplete = true;}
        }
        
        private static void OnConnectionFail(object sender, ConnectionFailedMessage message)
        {
            Console.Clear();
            Error("Connection to discord Failed! is discord Open??");
            Write(prompt:"1- Retry Connection,  2- Exit\n");
            errorprompt:
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    Run();
                    break;
                case ConsoleKey.D2:
                    Environment.Exit(1);
                    break;
                default:
                    Error("Invalid Key Pressed! Try Again"); 
                    goto errorprompt;
            }
        }
        
        private static void OnError(object sender, ErrorMessage message)
        {
            Error($"Code : {message.Code}, Message : {message.Message}");
        }
        
        private static void OnConnectionEstablished(object sender, ConnectionEstablishedMessage message)
        {
            Log("Connection Established!");
        }
        
        private static void OnReady(object sender, ReadyMessage message)
        {
            Log($"Client Ready : [ID : {message.User.ID}, Username: {message.User.Username}]");
        }
        
        /// <summary>
        /// Processes the cmd string given to it to see
        /// if any command should be invoked
        /// </summary>
        /// <param name="cmd"></param>
        private static void ProcessCommand(string cmd)
        {
            switch (cmd.ToLower())
            {
                case "help":
                    PrintHelp();
                    break;
                case "set":
                    Set();
                    break;
                case "exit":
                    Exit();
                    break;
                case "restart":
                    Restart();
                    break;
            }
        }
    
        /// <summary>
        /// Disconnects the client and Exits the program
        /// </summary>
        private static void Exit()
        {
            Log("Exiting...");
            _client.Dispose();
            Environment.Exit(0);
        }
        
        /// <summary>
        /// Restarts the client by Deinitializing and ReInitializing
        /// the client connection, then calling a Presence update
        /// </summary>
        private static void Restart()
        {
            Log("Restarting Client..");
            _client.Deinitialize();
            _client.Dispose();
            _client = new DiscordRpcClient(_rpcAtters["ClientID"]);
            AddEvents();
            _client.Initialize();
            UpdatePresence();
            Log("Restarted.. Wait Until Presence Update Message!");
        }
        
        /// <summary>
        /// Set command that changes an atter to a given value
        /// then calles Update presence.
        /// </summary>
        private static void Set()
        {
            Dictionary<string, string> atters = new Dictionary<string, string>();
            foreach (var value in _rpcAtters.Keys.Select((item, index) => new {item, index}))
            {
                atters.Add(value.index.ToString(), value.item);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\n {value.index} - ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(value.item);
            }
            Write(prompt:"Select The one you wish to change or type Exit\n");
            setprompt:
            string input = Console.ReadLine();
            if (input != null && atters.ContainsKey(input))
            {
                Write(prompt:"Enter New Value");
                _rpcAtters[atters[input]] = Console.ReadLine();
                UpdatePresence();
            }else if (input == "Exit") return;
            else {Error("Invalid Input! Try again"); goto setprompt;}
            
        }
        
        /// <summary>
        /// Help Command, Prints the help text
        /// </summary>
        private static void PrintHelp()
        {
            string text = "-- help (Shows This Message)\n" +
                          "-- set (Opens the set menu for changing values!)\n" +
                          "-- restart (restarts the program with the selected configs)\n" +
                          "-- exit (Exit the program)";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nDiscordRPCLite Commands\n");
            Console.ResetColor();
            Console.Write(text);
        }
        
    }
    
}