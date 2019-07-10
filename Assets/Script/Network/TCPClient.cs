using UnityEngine;
using System.Text;
using System.Collections.Generic;


using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class TCPClient : Singleton<TCPClient>
{
    [Tooltip("The IPv4 Address of the machine running the Unity editor.")]
    public string ServerIP;

    [Tooltip("The connection port on the machine to use.")]
    public int ConnectionPort = 11000;

    // ManualResetEvent instances signal completion.

    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);

    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    
    /// <summary>
    /// Tracks if we are currently sending a mesh.
    /// </summary>
    private bool Sending = false;

    /// <summary>
    /// Temporary buffer for the data we are sending.
    /// </summary>
    private byte[] nextDataBufferToSend;

    /// <summary>
    /// A queue of data buffers to send.
    /// </summary>
    private Queue<byte[]> dataQueue = new Queue<byte[]>();

    /// <summary>
    /// If we cannot connect to the server, we will wait before trying to reconnect.
    /// </summary>
    private float deferTime = 0.0f;

    /// <summary>
    /// If we cannot connect to the server, this is how long we will wait before retrying.
    /// </summary>
    private float timeToDeferFailedConnections = 10.0f;

    public void Update()
    {
        // Check to see if deferTime has been set.
        // DeferUpdates will set the Sending flag to true for
        // deferTime seconds.
        if (deferTime > 0.0f)
        {
            DeferUpdates(deferTime);
            deferTime = 0.0f;
        }

        // If we aren't sending a mesh, but we have a mesh to send, send it.
        if (!Sending && dataQueue.Count > 0)
        {
            byte[] nextPacket = dataQueue.Dequeue();
            Debug.Log("Trying to Send");
            SendDataOverNetwork(nextPacket);
        }
    }

    /// <summary>
    /// Handles waiting for some amount of time before trying to reconnect.
    /// </summary>
    /// <param name="timeout">Time in seconds to wait.</param>
    private void DeferUpdates(float timeout)
    {
        Sending = true;
        Invoke("EnableUpdates", timeout);
    }

    /// <summary>
    /// Stops waiting to reconnect.
    /// </summary>
    private void EnableUpdates()
    {
        Sending = false;
    }

    /// <summary>
    /// Queues up a data buffer to send over the network.
    /// </summary>
    /// <param name="dataBufferToSend">The data buffer to send.</param>
    public void SendString(string msg)
    {
        byte[] dataBufferToSend = Encoding.ASCII.GetBytes(msg);
        dataQueue.Enqueue(dataBufferToSend);
        Debug.Log("Queued packet to Send");
    }

    /// <summary>
    /// Sends the data over the network.
    /// </summary>
    /// <param name="dataBufferToSend">The data buffer to send.</param>
    private void SendDataOverNetwork(byte[] dataBufferToSend)
    {
        if (Sending)
        {
            // This shouldn't happen, but just in case.
            Debug.Log("one at a time please");
            return;
        }

        // Track that we are sending a data buffer.
        Sending = true;

        // Set the next buffer to send when the connection is made.
        nextDataBufferToSend = dataBufferToSend;

        // Setup a connection to the server.
        try
        {
            IPAddress ipAddress = IPAddress.Parse(ServerIP);

            // Convert the string data to byte data using ASCII encoding.

            TcpClient client = new TcpClient();

            client.BeginConnect(ipAddress, ConnectionPort,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne(100);

            NetworkStream netStream = client.GetStream();
            // Write how much data we are sending.
            netStream.WriteByte((byte)dataBufferToSend.Length);

            // Then write the data.
            netStream.Write(dataBufferToSend, 0, dataBufferToSend.Length);

            Sending = false;
            // Always disconnect here since we will reconnect when sending the next message
            client.Close();
        }
        catch (Exception e)
        {
            Sending = false;
            Debug.Log(e.ToString());
        }
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            //Socket client = (Socket)ar.AsyncState;

            //// Complete sending the data to the remote device.
            //int bytesSent = client.EndSend(ar);
            //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            //// Signal that all bytes have been sent.
            //sendDone.Set();
            //Sending = false;
            //// Always disconnect here since we will reconnect when sending the next message
            //client.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}