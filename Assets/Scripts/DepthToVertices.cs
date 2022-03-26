using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthToVertices : MonoBehaviour
{
    public DepthViewMode ViewMode = DepthViewMode.SeparateSourceReaders;
    public MeasureDepth mMeasureDepth;
    public GameObject ColorSourceManager;
    public GameObject DepthSourceManager;    
    private KinectSensor _Sensor;
    private Vector3[] _Vertices;
    private const double _DepthScale = 0.1f;
    private ColorSourceManager _ColorManager;
    private DepthSourceManager _DepthManager;

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            // Downsample to lower resolution
            Create_Vertices(frameDesc.Width , frameDesc.Height );
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    //init the vertices array
    void Create_Vertices(int width, int height)
    {
        _Vertices = new Vector3[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                _Vertices[index] = new Vector3(x, y, 0);
            }
        }
    }
    
    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.TextField(new Rect(Screen.width - 250 , 10, 250, 20), "DepthMode: " + ViewMode.ToString());
        GUI.EndGroup();
    }

    void Update()
    {
        if (_Sensor == null)
        {
            return;
        }
        Check_managers();
        RefreshData(_DepthManager.GetData());
    }

    //check the links to all the required managers.
    private void Check_managers(){
        if (ColorSourceManager == null)
            {
                return;
            }
            
        _ColorManager = ColorSourceManager.GetComponent<ColorSourceManager>();
        if (_ColorManager == null)
            {
                return;
            }
            
        if (DepthSourceManager == null)
            {
                return;
            }
            
        _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
        if (_DepthManager == null)
            {
                return;
            }
    }
    
    //calculates all the points distances and sends the array of points to update the displayed texture. 
    private void RefreshData(ushort[] depthData)
    {
        var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
        for (int y = 0; y < frameDesc.Height; y ++)
        {
            for (int x = 0; x < frameDesc.Width; x ++)
            {
                int smallIndex = (y *frameDesc.Width + x);
                float dis = depthData[(y * frameDesc.Width) + x];
                dis = dis * (float)_DepthScale;
                _Vertices[smallIndex].z = dis;
            }
        }
        mMeasureDepth.UpdateTexture(_Vertices);
    }
}
