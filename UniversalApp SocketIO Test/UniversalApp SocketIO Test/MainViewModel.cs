using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Diagnostics;

namespace UniversalApp_SocketIO_Test
{
    public class MainViewModel : ViewModelBase
    {
        Socket socket;
        public MainViewModel()
        {

                SocketIO();
            
        }
        private string tempValue = "";
        public string TempValue
        {
            get { return tempValue; }
            set
            {
                tempValue = value;
                RaisePropertyChanged("TempValue");
            }
        }

        public void SocketIO()
        {

            socket = IO.Socket("http://93.89.117.144/");
            socket.Connect();

            socket.On("tempUpdate", data =>
            {
                JObject json = JObject.Parse(data.ToString());
                var value = (string)json["value"];
                //MessageBox.Show("Value: " + value + "   At time: " + DateTime.Now.ToLongTimeString());
                Debug.WriteLine("Value: " + value + "   At time: " + DateTime.Now.ToLocalTime());
                TempValue = "Value: " + value + "   At time: " + DateTime.Now.ToLocalTime();
            });

            socket.On("connection", data => {
                // get the json data from the server message
                var jobject = data as JToken;
                Debug.WriteLine("Connected");

            });

            socket.On(Socket.EVENT_CONNECT_ERROR, () => {

                Debug.WriteLine("Lost connection");
            });

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Debug.WriteLine("Connected");
            });
        }
    }
}
