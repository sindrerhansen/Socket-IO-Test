using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;

namespace Socket_IO_Console_test
{
    class Program
    {
        private static bool life=true;

        static void Main(string[] args)
        {
            // connect to a Socket.IO server
            var socket = IO.Socket("http://93.89.117.144/");
            socket.Connect();
            socket.On("chat message", data => {
                var jobject = data as JToken;
                Console.WriteLine("Message received");
                Console.WriteLine(data);
            });
            socket.On("tempUpdate", data =>
            {
                JObject json = JObject.Parse(data.ToString());
                var value= (string)json["value"];
                Console.WriteLine("Value: " + value + "   At time: " + DateTime.Now.ToLongTimeString());
            });
            socket.On("connection", data => {
                // get the json data from the server message
                var jobject = data as JToken;

            });
            socket.On(Socket.EVENT_CONNECT_ERROR, () => {
                Console.WriteLine("Lost connection");
            });

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected");
            });

            while (life)
            {
                Console.WriteLine("Write message:");
                var message = Console.ReadLine();
                if (message == "q")
                    life = false;
                else
                {
                    socket.Emit("chat message", message);
                }
            }
            
        }
    }
}
