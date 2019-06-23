using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public class QueueManager
    {
        internal static readonly QueueManager instance = new QueueManager();

        private System.Collections.Queue messages = new System.Collections.Queue();

        public static QueueManager Instance { get { return instance; } }


        public void PushToQueue(Message message)
        {
            messages.Enqueue( message);
        }

        public Message PopFromQueue()
        {
            return messages.Dequeue() as Message;
        }
       

    }
}
