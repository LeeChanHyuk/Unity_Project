using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;
using OpenCvSharp;

public class NewBehaviourScript : MonoBehaviour
{
    Pipeline pipe;
    // Start is called before the first frame update
    void Start()
    {
        pipe = new Pipeline();
        pipe.Start();
    }

    // Update is called once per frame
    void Update()
    {
        using (var frames = pipe.WaitForFrames())
        using (var depth = frames.DepthFrame)
        {
            print("The camera is pointing at an object " +
                depth.GetDistance(depth.Width / 2, depth.Height / 2) + " meters away\t");
            Mat image = new Mat(360, 640, MatType.CV_8UC3, frames.ColorFrame.Data);
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2RGB);
            Cv2.ImShow("image", image);

        }
    }
}
