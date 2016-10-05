using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Diagnostics;
using System.Windows;

namespace WpfApplication1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        Socket socket;
        bool init = false;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (!init)
            {
                SocketIO();
            }
        }
        private string tempValue ="";
        public string TempValue {
            get { return tempValue; }
            set {
                tempValue = value;
                RaisePropertyChanged("TempValue");
            } }
        public void SocketIO()
        {

            socket = IO.Socket("http://93.89.117.144/");
            socket.Connect();

            socket.On("tempUpdate", data =>
            {
                JObject json = JObject.Parse(data.ToString());
                var value = (string)json["value"];
                //MessageBox.Show("Value: " + value + "   At time: " + DateTime.Now.ToLongTimeString());
                Debug.WriteLine("Value: " + value + "   At time: " + DateTime.Now.ToLongTimeString());
                TempValue = "Value: " + value + "   At time: " + DateTime.Now.ToLongTimeString();
            });

            socket.On("connection", data => {
                // get the json data from the server message
                var jobject = data as JToken;
                Debug.WriteLine("Connected");

            });

            socket.On(Socket.EVENT_CONNECT_ERROR, () => {
                MessageBox.Show("Lost connection");
                Debug.WriteLine("Lost connection");
            });

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                MessageBox.Show("Connected");
            });
        }
    }
}