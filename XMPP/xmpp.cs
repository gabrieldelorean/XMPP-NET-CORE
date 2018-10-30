using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Almawave.xmpp.XMPP
{
   public class xmpp
    {
        private readonly ManualResetEvent allDone = new ManualResetEvent(false);
        private Socket m_Listener;
        private bool m_Listening;


        private static void SetLicense()
        {
            // request demo license here:
            // http://www.ag-software.net/matrix-xmpp-sdk/request-demo-license/
            const string LIC = @"YOUR LICENSE";
            Matrix.License.LicenseManager.SetLicense(LIC);

            // when something is wrong with your license you can find the error here
            Console.WriteLine(Matrix.License.LicenseManager.LicenseError);
        }

        public void cmdStart_Click()
        {
            var myThreadDelegate = new ThreadStart(Listen);
            var myThread = new Thread(myThreadDelegate);
            myThread.Start();
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            m_Listening = false;
            allDone.Set();
        }

        private void Listen()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, 5222);

            // Create a TCP/IP socket.
            m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                m_Listener.Bind(localEndPoint);
                m_Listener.Listen(10);

                m_Listening = true;

                while (m_Listening)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    m_Listener.BeginAccept(AcceptCallback, null);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            // Get the socket that handles the client request.
            Socket sock = m_Listener.EndAccept(ar);

            var con = new XmppSeverConnection(sock);
        }

    }
}
