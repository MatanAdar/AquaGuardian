using UnityEngine;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public enum InputType
{
    EmulationMode,
    Amadeo,
}

public class AmadeoClient : MonoBehaviour
{
    /*public static AmadeoClient Instance { get; private set; }*/

    [SerializeField] InputType inputType = InputType.Amadeo;

    [SerializeField, Tooltip("Port should be 4444 for Amadeo connection"), Range(1024, 49151)]
    private int _port = 4444;

    [SerializeField] private int _zeroFBuffer = 10;

    private CancellationTokenSource _cancellationTokenSource;
    private bool _isReceiving = false;
    private UdpClient _udpClient;
    private const string EmulationDataFile = "Assets/AmadeoRecords/force_data.txt";

    private const int DefaultPortNumber = 4444;

    private IPEndPoint _remoteEndPoint;

    private float[] _forces = new float[5];
    private readonly float[] _zeroForces = new float[5]; // init [0,0,0,0,0]
    private bool _isLeftHand = false;

    public event Action<float[]> OnForcesUpdated;

    public GameObject Panel;

    /*private void Awake()
    {
        Debug.Log("AmadeoClient Awake called");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }*/

    private void Start()
    {
        try
        {
            _udpClient = new UdpClient(_port);
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // open connetction with the amadeo socket (4444)
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize UdpClient: {ex.Message}");
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        Debug.Log("amendoClient started");
        //StartReceiveData();
        
    }

    public void StartZeroF()
    {

            SetZeroF(_cancellationTokenSource.Token);
            Debug.Log("StartReceiveData :: Starting zeroing forces.");
            return;
    }

    // forces [1,1,3,4,1]

    public void StartReceiveData()
    {
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
        _isReceiving = true;
        if (inputType == InputType.EmulationMode)
        {
            Debug.Log("StartReceiveData :: Emulation mode is true. Starting emulation data.");
            HandleIncomingDataEmu(_cancellationTokenSource.Token);
        }
        else
        {
            Debug.Log("StartReceiveData :: Amadeo mode is true. Starting Amadeo data.");

            ReceiveDataAmadeo(_cancellationTokenSource.Token);
        }
    }

    public void StopReceiveData()
    {
        _isReceiving = false;
    }

    private async void ReceiveDataAmadeo(CancellationToken cancellationToken)
    {
        while (_isReceiving && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result = await _udpClient.ReceiveAsync();
                string receivedData = Encoding.ASCII.GetString(result.Buffer);
               // Debug.Log("ReceiveDataAmadeo :: Data: " +  receivedData);
                HandleReceivedData(ParseDataFromAmadeo(receivedData));

            }
            catch (OperationCanceledException)
            {
                Debug.Log("Data reception was canceled.");
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception in ReceiveData: {ex.Message}");
                break;
            }
        }
    }

    private async void HandleIncomingDataEmu(CancellationToken cancellationToken)
    {
        try
        {
            string[] lines = await File.ReadAllLinesAsync(EmulationDataFile, cancellationToken);

            int index = 0;

            while (_isReceiving)
            {
                string line = lines[index];
                if (!string.IsNullOrWhiteSpace(line))
                {
                    HandleReceivedData(ParseDataFromAmadeo(line));
                }

                index = (index + 1) % lines.Length;
                await Task.Delay(10, cancellationToken);
            }

            Debug.Log("HandleIncomingDataEmu: Stopped receiving data.");
        }
        catch (TaskCanceledException e)
        {
            Debug.Log($"Task was canceled: {e.Message}");
            throw;
        }
    }

    private void HandleReceivedData(string data)
    {
        string[] strForces = data.Split('\t');
        if (strForces.Length != 11)
        {
            Debug.Log("Received data does not contain exactly 11 values. Ignoring...");
            return;
        }

        strForces.Select(str =>
                float.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture))
            .Skip(strForces.Length - 5)
            .ToArray()
            .CopyTo(_forces, 0);

        _forces = _forces.Select((force, i) => force - _zeroForces[i]).ToArray();

        if (!_isLeftHand)
        {
            _forces = _forces.Reverse().ToArray();
        }

        Debug.Log("Forces after processing: " + string.Join(", ", _forces));
        OnForcesUpdated?.Invoke(_forces);
    }

    private static string ParseDataFromAmadeo(string data)
    {
        return data.Replace("<Amadeo>", "").Replace("</Amadeo>", "");
    }

    private void OnDestroy()
    {
        Debug.Log("Called OnDestroy() in AmadeoClient...");
        StopClientConnection();
    }


    private void OnApplicationQuit()
    {
        StopClientConnection();
    }

    private void StopClientConnection()
    {
        _isReceiving = false;
        if (_udpClient != null)
        {
            _udpClient.Close();
            _udpClient.Dispose();
            _udpClient = null;
        }

        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private async void SetZeroF(CancellationToken cancellationToken)
    {
        var index = 0;
        int numOfLinesToRead = _zeroFBuffer;
        string[] lines = new string[numOfLinesToRead];
        try
        {
            if (inputType is InputType.EmulationMode)
            {
                using (StreamReader reader = new StreamReader(EmulationDataFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && index < numOfLinesToRead)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            lines[index] = line;
                            index++;
                        }
                    }
                }

                if (lines.Length == 0)
                {
                    Debug.LogError("No data found in the emulation file.");
                    return;
                }
            }
            else
            {
                int i = 0;
                while (i < numOfLinesToRead && !cancellationToken.IsCancellationRequested)
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    string receivedData = Encoding.ASCII.GetString(result.Buffer);
                    Debug.Log(receivedData);

                    HandleReceivedData(ParseDataFromAmadeo(receivedData));

                    lines[i] = receivedData;
                    i++;
                }

                if (i < numOfLinesToRead)
                {
                    Debug.LogError("Not enough data received from Amadeo device.");
                }
            }

            string[] parsedData = lines.Select(ParseDataFromAmadeo).ToArray();
            CalculateZeroingForces(parsedData);

        }
        catch (OperationCanceledException)
        {
            Debug.Log("Data reception was canceled.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in ReceiveData: {ex.Message}");
        }
    }

    private void CalculateZeroingForces(string[] lines)
    {
        float[] sums = new float[5];
        int count = 0;

        String[][] allLines = lines
            .Select(line => line.Replace(",", ".").Split('\t'))
            .ToArray();
        string[][] relevantForces = allLines.Select(line => line.Skip(line.Length - 5).ToArray()).ToArray();

        foreach (var line in relevantForces)
        {
            if (line.Length == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!float.TryParse(line[i], NumberStyles.Float, CultureInfo.InvariantCulture,
                            out float value)) continue;
                    sums[i] += value;
                }

                count++;
            }
        }

        for (int i = 0; i < sums.Length; i++)
        {
            _zeroForces[i] = sums[i] / count;
        }
    }
}
