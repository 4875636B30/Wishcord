using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Drawing;
using System.ServiceModel.Channels;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Net;
using Utility;
using System.Net.Http;

namespace WishCordService {
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
    public interface IBetterChat : IChat{
        [OperationContract]
        bool NewClient(string clientUrl, string username, Color userColor, Bitmap profilePicture);
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
    public interface IBetterChatDistribute : IChatDistribute{
        [OperationContract]
        void NewMessage(Message message);
    }
    #endregion

    #region ChatClasses
    public class Chat : IBetterChat {
        static LinkedList<IBetterChatDistribute> channels = new LinkedList<IBetterChatDistribute>();

        public void ClientLeave(IBetterChatDistribute channel) {
            channels.Remove(channel);
        }

        public void NewClient(string clientUrl) {
            channels.AddLast((new ChannelFactory<IBetterChatDistribute>(new WSHttpBinding(SecurityMode.None), clientUrl)).CreateChannel());
        }

        public bool NewClient(string clientUrl, string username, Color userColor, Bitmap profilePicture) {
            channels.AddLast((new ChannelFactory<IBetterChatDistribute>(new WSHttpBinding(SecurityMode.None), clientUrl)).CreateChannel());
            return true;
        }

        public void NewMessage(string message) {
            foreach(var channel in channels) {
                channel.NewMessage(message);
            }
        }

        public void NewMessage(Message message) {
            foreach(var channel in channels) {
                channel.NewMessage(message);
            }
        }

        public string WhichServiceAmI() {
            return "WishCord";
        }
    }
    public class Client {
        private string username;
        private Bitmap profilePicture;
        private Color userColor;

        private string Username { get; set; }
        private Bitmap ProfilePicture { get; set; }
        private Color UserColor { get; set; }

        Client() {
            Username = string.Empty;
            ProfilePicture = null;
            UserColor = Color.Empty;
        }

        Client(string username, Bitmap profilePicture, Color userColor) {
            Username = username;
            ProfilePicture = profilePicture;
            UserColor = userColor;
            Username = username;
            ProfilePicture = profilePicture;
            UserColor = userColor;
        }
    }
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

    internal class Program {
        
        static void Main(string[] args) {

            
            string URI = "http://"+IPAdress.GetIPv4Adress()+":2310/WishCord";
            Console.WriteLine(URI);
            ServiceHost chatService = new ServiceHost(typeof(Chat));

            chatService.AddServiceEndpoint(typeof(IChat), new WSHttpBinding(SecurityMode.None), URI);

            chatService.Open();

            Console.WriteLine("Service wartet!");
            Console.WriteLine();
            Console.WriteLine("Press ENTER to QUIT ...");
            Console.ReadLine();

            chatService.Close();
        }
    }
}
