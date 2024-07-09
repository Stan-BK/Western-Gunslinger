using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using SocketNet.Message;
using ProtoBuf;
using Google.Protobuf;

namespace Network
{
    class NetClient : Singleton<NetClient>
    {

        const int DEF_POLL_INTERVAL_MILLISECONDS = 100; //default network thread hold on interval
        const int DEF_TRY_CONNECT_TIMES = 3;            //default the number of retries the server
        const int DEF_RECV_BUFFER_SIZE = 64 * 1024;     //default initial buffer size of recvStream
        const int DEF_PACKAGE_HEADER_LENGTH = 4;        //default package header size
        const int DEF_SEND_PING_INTERVAL = 30;          //default send ping package interval
        const int NetConnectTimeout = 10000;    //default connect wait milliseconds
        const int DEF_LOAD_WHEEL_MILLISECONDS = 1000;   //default wait some milliseconds then show load wheel
        const int NetReconnectPeriod = 10;              //default reconnect seconds

        public const int NET_ERROR_UNKNOW_PROTOCOL = 2;           //协议错误
        public const int NET_ERROR_SEND_EXCEPTION = 1000;       //发送异常
        public const int NET_ERROR_ILLEGAL_PACKAGE = 1001;      //接受到错误数据包
        public const int NET_ERROR_ZERO_BYTE = 1002;            //收发0字节
        public const int NET_ERROR_PACKAGE_TIMEOUT = 1003;      //收包超时
        public const int NET_ERROR_PROXY_TIMEOUT = 1004;        //proxy超时
        public const int NET_ERROR_FAIL_TO_CONNECT = 1005;      //3次连接不上
        public const int NET_ERROR_PROXY_ERROR = 1006;          //proxy重启
        public const int NET_ERROR_ON_DESTROY = 1007;           //结束的时候，关闭网络连接
        public const int NET_ERROR_ON_KICKOUT = 25;           //被踢了

        public delegate void ConnectEventHandler(int result, string reason);
        public delegate void ExpectPackageEventHandler();

        public event ConnectEventHandler OnConnect;
        public event ConnectEventHandler OnDisconnect;
        public event ExpectPackageEventHandler OnExpectPackageTimeout;
        public event ExpectPackageEventHandler OnExpectPackageResume;

        //socket instance
        private IPEndPoint address;
        private Socket clientSocket;

        private bool connecting = false;

        private int retryTimes = 0;
        private int retryTimesTotal = DEF_TRY_CONNECT_TIMES;
        private float lastSendTime = 0;
        private int sendOffset = 0;

        public bool running { get; set; }

        protected override void Awake()
        {
            base.Awake();
            running = true;
        }
        protected virtual void RaiseConnected(int result, string reason)
        {
            ConnectEventHandler handler = OnConnect;
            if (handler != null)
            {
                handler(result, reason);
            }
        }

        public virtual void RaiseDisonnected(int result, string reason = "")
        {
            ConnectEventHandler handler = OnDisconnect;
            if (handler != null)
            {
                handler(result, reason);
            }
        }

        protected virtual void RaiseExpectPackageTimeout()
        {
            ExpectPackageEventHandler handler = OnExpectPackageTimeout;
            if (handler != null)
            {
                handler();
            }
        }
        protected virtual void RaiseExpectPackageResume()
        {
            ExpectPackageEventHandler handler = OnExpectPackageResume;
            if (handler != null)
            {
                handler();
            }
        }

        public bool Connected
        {
            get
            {
                return (clientSocket != default(Socket)) ? clientSocket.Connected : false;
            }
        }

        public NetClient()
        {
        }

        public void Reset()
        {

            this.sendOffset = 0;

            this.connecting = false;

            this.retryTimes = 0;
            this.lastSendTime = 0;

            this.OnConnect = null;
            this.OnDisconnect = null;
            this.OnExpectPackageTimeout = null;
            this.OnExpectPackageResume = null;
        }

        public void Init(string serverIP, int port)
        {
            this.address = new IPEndPoint(IPAddress.Parse(serverIP), port);
        }

        /// <summary>
        /// Connect
        /// asynchronous connect.
        /// Please use OnConnect handle connect event 
        /// </summary>
        /// <param name="retryTimes"></param>
        /// <returns></returns>
        public void Connect(int times = DEF_TRY_CONNECT_TIMES)
        {
            if (this.connecting)
            {
                return;
            }

            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
            }
            if (this.address == default(IPEndPoint))
            {
                throw new Exception("Please Init first.");
            }
            Debug.Log("DoConnect");
            this.connecting = true;
            this.lastSendTime = 0;

            this.DoConnect();
        }

        public void OnDestroy()
        {
            Debug.Log("OnDestroy NetworkManager.");
            this.CloseConnection(NET_ERROR_ON_DESTROY);
        }

        public void CloseConnection(int errCode)
        {
            Debug.LogWarning("CloseConnection(), errorCode: " + errCode.ToString());
            this.connecting = false;
            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
            }

        }

        //send a Protobuf message
        public void SendMessage()
        {
            MyMessage myMessage = new MyMessage();
            myMessage.Query = "Hello";
            byte[] array = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                myMessage.WriteTo(memoryStream);
                //Serializer.Serialize(memoryStream, (MyMessage)myMessage);
                array = new byte[memoryStream.Length + 4];
                Buffer.BlockCopy(BitConverter.GetBytes(memoryStream.Length), 0, array, 0, 4);
                Buffer.BlockCopy(memoryStream.GetBuffer(), 0, array, 4, (int)memoryStream.Length);
                
            }
            this.clientSocket.Send(array);
        }

        void DoConnect()
        {
            Debug.Log("NetClient.DoConnect on " + this.address.ToString());
            try
            {
                if (this.clientSocket != null)
                {
                    this.clientSocket.Close();
                }


                this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.clientSocket.Blocking = true;

                Debug.Log(string.Format("Connect[{0}] to server {1}", this.retryTimes, this.address) + "\n");
                IAsyncResult result = this.clientSocket.BeginConnect(this.address, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(NetConnectTimeout);
                if (success)
                {
                    Debug.Log(123);
                    this.SendMessage();
                    this.clientSocket.EndConnect(result);
                }
            }
            catch(SocketException ex)
            {
                if(ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    this.CloseConnection(NET_ERROR_FAIL_TO_CONNECT);
                }
                Debug.LogErrorFormat("DoConnect SocketException:[{0},{1},{2}]{3} ", ex.ErrorCode,ex.SocketErrorCode,ex.NativeErrorCode, ex.ToString()); 
            }
            catch (Exception e)
            {
                Debug.Log("DoConnect Exception:" + e.ToString() + "\n");
            }

            if (this.clientSocket.Connected)
            {
                this.clientSocket.Blocking = false;
                this.RaiseConnected(0, "Success");
            }
            else
            {
                this.retryTimes++;
                if (this.retryTimes >= this.retryTimesTotal)
                {
                    this.RaiseConnected(1, "Cannot connect to server");
                }
            }
            this.connecting = false;
        }

        bool KeepConnect()
        {
            if (this.connecting)
            {
                return false;
            }
            if (this.address == null)
                return false;

            if (this.Connected)
            {
                return true;
            }

            if (this.retryTimes < this.retryTimesTotal)
            {
                this.Connect();
            }
            return false;
        }

        bool ProcessRecv()
        {
            bool ret = false;
            try
            {
                if (this.clientSocket.Blocking)
                {
                    Debug.Log("this.clientSocket.Blocking = true\n");
                }
                bool error = this.clientSocket.Poll(0, SelectMode.SelectError);
                if (error)
                {
                    Debug.Log("ProcessRecv Poll SelectError\n");
                    this.CloseConnection(NET_ERROR_SEND_EXCEPTION);
                    return false;
                }

                ret = this.clientSocket.Poll(0, SelectMode.SelectRead);
                if (ret)
                {
                    this.clientSocket.Receive(new byte[0]);
                    
                }
            }
            catch (Exception e)
            {
                Debug.Log("ProcessReceive exception:" + e.ToString() + "\n");
                this.CloseConnection(NET_ERROR_ILLEGAL_PACKAGE);
                return false;
            }
            return true;
        }

        bool ProcessSend()
        {
            bool ret = false;
            try
            {
                if (this.clientSocket.Blocking)
                {
                    Debug.Log("this.clientSocket.Blocking = true\n");
                }
                bool error = this.clientSocket.Poll(0, SelectMode.SelectError);
                if (error)
                {
                    Debug.Log("ProcessSend Poll SelectError\n");
                    this.CloseConnection(NET_ERROR_SEND_EXCEPTION);
                    return false;
                }
                ret = this.clientSocket.Poll(0, SelectMode.SelectWrite);
            }
            catch (Exception e)
            {
                Debug.Log("ProcessSend exception:" + e.ToString() + "\n");
                this.CloseConnection(NET_ERROR_SEND_EXCEPTION);
                return false;
            }

            return true;
        }

        //Update need called once per frame
        public void Update()
        {
            if (!running)
            {
                return;
            }

            if (this.KeepConnect())
            {
                //if (this.ProcessRecv())
                //{
                //    if (this.Connected)
                //    {
                //        this.ProcessSend();
                //    }
                //}
            }
        }
    }
}
