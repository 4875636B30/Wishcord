using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Drawing;
using Utility;

namespace WishCordClient {
    #region ChatInterfaces

    [ServiceContract]
    public interface IChat {
        [OperationContract]
        string WhichServiceAmI();
        [OperationContract]
        void NewClient(string clientUrl);
        [OperationContract]
        void NewMessage(string message);
    }

    [ServiceContract]
    public interface IBetterChat : IChat {
        [OperationContract]
        bool NewClient(string clientUrl, string username, System.Drawing.Color userColor, Bitmap profilePicture);
        [OperationContract]
        void ClientLeave(IBetterChatDistribute channel);
        [OperationContract]
        void NewMessage(Message message);

    }

    [ServiceContract]
    public interface IChatDistribute {
        [OperationContract]
        void NewMessage(string mesage);
    }
    [ServiceContract]
    public interface IBetterChatDistribute : IChatDistribute {
        [OperationContract]
        void NewMessage(Message message);
    }
    #endregion

    #region ChatClasses

    public class Message {
        private string messageText;
        private string username;
        private DateTime timestamp;

        private string MessageText { get; set; }
        private string Username { get; set; }
        private DateTime Timestamp { get; set; }

        Message() {
            MessageText = "null";
            Username = "null";
            Timestamp = DateTime.Now;
        }

        Message(string messageText, string username, DateTime timeStamp) {
            MessageText = messageText;
            Username = username;
            Timestamp = timeStamp;
        }

    }
    #endregion

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
    }
}
