using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.ServiceModel.Channels;

namespace WishCordService {
    [ServiceContract]
    public interface IChat {
        [OperationContract]
        string WhichServiceAmI();
        [OperationContract]
        void NewMessage(string message);
        [OperationContract]
        void NewClient(string clientUrl);
    }
    [ServiceContract]
    public interface IChatDistribute {
        [OperationContract]
        void NewMessage(string mesage);
    }
    public class Chat : IChat {
        List<IChatDistribute> channels = new List<IChatDistribute>();
        public void NewClient(string clientUrl) {
            channels.Add((new ChannelFactory<IChatDistribute>(new WSHttpBinding(SecurityMode.None), clientUrl)).CreateChannel());
        }

        public void NewMessage(string message) {
            foreach(var channel in channels) {
                channel.NewMessage(message);
            }
        }

        public string WhichServiceAmI() {
            return "WishCord";
        }
    }
    internal class Program {
        static void Main(string[] args) {

            string URI = "http://localhost:2310/WishCord";

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
