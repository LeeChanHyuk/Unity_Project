using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using TensorFlow;
using System.IO;
using System.Runtime.InteropServices;
using System;
using Intel.RealSense;

public class faces : MonoBehaviour
{
    DepthFrame depth;
    Pipeline pipe;
    public GameObject pixxy;
    public Vector3 v3;
    OpenCvSharp.VideoCapture camera;
    // Start is called before the first frame update
    Mat image;
    Mat importantImage;
    double min = 0;
    double max = 0;
    Point minLoc;
    Point maxLoc;
    TFGraph graph1;
    TFGraph graph2;
    TFGraph graph3;
    TFSession session1;
    TFSession session2;
    TFSession session3;
    List<Scalar> colors;
    Mat face_position;
    void Start()
    {
        colors = new List<Scalar>();
        face_position = Cv2.ImRead("C:/Users/CheNik/Desktop/Face_position.png");
        colors.Add(new Scalar(0, 0, 100));
        colors.Add(new Scalar(0, 100, 0));
        colors.Add(new Scalar(0, 100, 100));
        colors.Add(new Scalar(100, 0, 0));
        colors.Add(new Scalar(100, 0, 100));
        colors.Add(new Scalar(100, 100, 0));
        colors.Add(new Scalar(100, 100, 100));
        colors.Add(new Scalar(0, 0, 150));
        colors.Add(new Scalar(0, 150, 0));
        colors.Add(new Scalar(0, 150, 150));
        colors.Add(new Scalar(150, 0, 0));
        colors.Add(new Scalar(150, 0, 150));
        colors.Add(new Scalar(150, 150, 0));
        colors.Add(new Scalar(150, 150, 150));
        colors.Add(new Scalar(0, 0, 180));
        colors.Add(new Scalar(0, 180, 0));
        colors.Add(new Scalar(0, 180, 180));
        pipe = new Pipeline();
        pipe.Start();
        v3 = new Vector3(0, -5, 2.4f);
        pixxy.transform.position = new Vector3(0f, 0f, 0f);
        pixxy.transform.rotation = Quaternion.Euler(0, 180, 0);
        camera = new VideoCapture(0);
        image = new Mat();
        importantImage = new Mat();
       camera.Open(0);
        camera.Set(OpenCvSharp.CaptureProperty.FrameWidth, 640);
        camera.Set(OpenCvSharp.CaptureProperty.FrameHeight, 360);
        graph1 = new TFGraph();
        graph2 = new TFGraph();
        graph3 = new TFGraph();
        graph1.Import(File.ReadAllBytes("C:/Users/CheNik/Downloads/FSA-Net-master/FSA-Net-master/pre-trained/converted-models/tf/fsanet_capsule_3_16_2_21_5.pb"));
        graph2.Import(File.ReadAllBytes("C:/Users/CheNik/Downloads/FSA-Net-master/FSA-Net-master/pre-trained/converted-models/tf/fsanet_noS_capsule_3_16_2_192_5.pb"));
        graph3.Import(File.ReadAllBytes("C:/Users/CheNik/Downloads/FSA-Net-master/FSA-Net-master/pre-trained/converted-models/tf/fsanet_var_capsule_3_16_2_21_5.pb"));
        session1 = new TFSession(graph1);
        session2 = new TFSession(graph2);
        session3 = new TFSession(graph3);
    }

    //var layerName = nets.GetLayerNames()[0];
    //prob.MinMaxLoc(out min, out max, out minLoc, out maxLoc);
    // Update is called once per frame
    void Update()
    {
        var frames = pipe.WaitForFrames();
        
        depth = frames.DepthFrame;
        print("The camera is pointing at an object " +
                depth.GetDistance(depth.Width / 2, depth.Height / 2) + " meters away\t");
        Mat depth_frame = new Mat(new Size(640, 360), MatType.CV_8UC3);

        //image=Cv2.ImRead("C:/Users/CheNik/Desktop/KakaoTalk_20191128_172510392.jpg");
        camera.Read(image);
        var image4 = image.Clone();
        var image5 = image.Clone();
        int frameWidth = image.Width;
        int frameHeight = image.Height;
        Cv2.ImShow("image", image);
        Cv2.WaitKey(1);
        importantImage = OpenCvSharp.Dnn.CvDnn.BlobFromImage(image, 1, new Size(300, 300), new Scalar(104, 117, 123), false, false);

        var nets = OpenCvSharp.Dnn.CvDnn.ReadNetFromCaffe("C:/Users/CheNik/Face/Assets/deploy.prototxt", "C:/Users/CheNik/Face/Assets/res10_300x300_ssd_iter_140000_fp16.caffemodel");
        //var nets = OpenCvSharp.Dnn.CvDnn.ReadNetFromTensorflow("C:/Users/CheNik/Face/Assets/opencv_face_detector_uint8.pb","C:/Users/CheNik/Face/Assets/opencv_face_detector.pbtxt");
        nets.SetInput(importantImage,"data");
        var prob2 = nets.Forward();
        var p = prob2.Reshape(1, prob2.Size(2));
        
        for (int i = 0; i < prob2.Size(2); i++)
        {
            var confidence = p.At<float>(i, 2);
            if (confidence > 0.7)
            {
                //get value what we need
                var idx = (int)p.At<float>(i, 1);
                var x1 = (int)(image.Width * p.At<float>(i, 3));
                var y1 = (int)(image.Height * p.At<float>(i, 4));
                var x2 = (int)(image.Width * p.At<float>(i, 5));
                var y2 = (int)(image.Height * p.At<float>(i, 6));
                var width = x2 - x1 + 1;
                var height = y2 - y1 + 1;
                //draw result
                OpenCvSharp.Rect facerect = new OpenCvSharp.Rect(x1, y1, width + 1, height + 1);
                OpenCvSharp.Point center_point = new OpenCvSharp.Point((int)(x1+(width/2))+1,(int)(y1+(height/2)+1));
                print(center_point);
                Scalar specified_position = get_position(center_point);
                print("specified_position = "+specified_position);
                //float depth_center_point = depth.GetDistance(center_point.X, center_point.Y);
                pixxy.transform.position = new Vector3(0, 0, 0);
                Mat face = image4.Clone().SubMat(facerect);
                var tensorimage = CreateTensorFromImageFileAlt(face);
                var tensored_image = int_to_float_and_div(tensorimage);



                var runner1 = session1.GetRunner();
                var runner2 = session2.GetRunner();
                var runner3 = session3.GetRunner();
                var input1 = graph1["input_1"][0];
                var input2 = graph2["input_1"][0];
                var input3 = graph3["input_1"][0];
                var pred1 = graph1["pred_pose/mul_24"][0];
                var pred2 = graph2["pred_pose/mul_24"][0];
                var pred3 = graph3["pred_pose/mul_24"][0];
                runner1.AddInput(input1, tensored_image);
                runner2.AddInput(input2, tensored_image);
                runner3.AddInput(input3, tensored_image);
                runner1.Fetch(pred1);
                runner2.Fetch(pred2);
                runner3.Fetch(pred3);
                var output1 = runner1.Run();
                var output2 = runner2.Run();
                var output3 = runner3.Run();
                TFTensor probresult1 = output1[0];
                TFTensor probresult2 = output2[0];
                TFTensor probresult3 = output3[0];
                float[,] result1 = return_value(probresult1);
                float[,] result2 = return_value(probresult2);
                float[,] result3 = return_value(probresult3);
                float[] result = new float[3];
                result[0] = result1[0, 0] + result2[0, 0] + result3[0, 0];
                result[1] = result1[0, 1] + result2[0, 1] + result3[0, 1];
                result[2] = result1[0, 2] + result2[0, 2] + result3[0, 2];
                //print("model1 result" + result1[0, 0] +" " +result1[0, 1] +" " +result1[0, 2]);
                //print("model2 result" + result2[0, 0] + " " + result2[0, 1] + " " + result2[0, 2]);
                //print("model3 result" + result3[0, 0] + " " + result3[0, 1] + " " + result3[0, 2]);
                //print(result[0]/3);
                //print(result[1]/3);
                //print(result[2]/3);
                float yaw = result[0] / 3;
                float pitch = result[1] / 3;
                float roll = result[2] / 3;
                image4=draw_axis(image4, yaw, pitch, roll);

                /*if (yaw < -30)
                    v3.y =v3.y+ 30;
                if (yaw > 30)
                    v3.y = v3.y - 30;
                if(pitch)*/
                
                float yaw2 = yaw;
                float pitch2 = pitch - 5;
                float roll2 = roll + 2.4f;
                yaw2 = rotate_Charactor(yaw2);
                pitch2 = rotate_Charactor(pitch2);
                roll2 = rotate_Charactor(roll2);
                print("yaw = " + yaw2 + " " + "pitch = " + pitch2 + " " + "roll = " + roll2);

                
                pixxy.transform.rotation = Quaternion.Euler(-pitch2, yaw2, roll2);

                Cv2.Rectangle(image4, new Point(x1, y1), new Point(x2, y2), new Scalar(255, 0, 0), 3,shift:8);
            }
        }
        Cv2.ImShow("image4", image4);
        pipe.Stop();


    }

    private unsafe TFTensor CreateTensorFromImageFileAlt(Mat inputFileName, TFDataType destinationDataType = TFDataType.Float)
    {
        Cv2.Resize(inputFileName, inputFileName, new OpenCvSharp.Size(64, 64)); // 일반 graph일 때


        var array = new byte[inputFileName.Width * inputFileName.Height * inputFileName.ElemSize()]; // 임시 array 생성
        Marshal.Copy(inputFileName.Data, array, 0, array.Length); // MS에서 제공하는 COPY Method
        var matrix = new byte[1, inputFileName.Rows, inputFileName.Width, 3];
        for (int i = 0; i < inputFileName.Rows; i++)
        {
            for (int j = 0; j < inputFileName.Cols; j++)
            {

                // 혹시라도 안되면 byte의 channel에 주목해보기!!! 난 bgr2rgb도 따로 안해서 둘다 시험해보기.
                matrix[0, i, j, 0] = array[i * inputFileName.Step() + j * 3 + 0]; // 행-렬이다 여기서도. 즉 y,x이다.
                matrix[0, i, j, 1] = array[i * inputFileName.Step() + j * 3 + 1];
                matrix[0, i, j, 2] = array[i * inputFileName.Step() + j * 3 + 2]; // c++에서 복사해보고 제대로 복사되나 보고
            }
        }
        UInt16[,,,] float_array = new UInt16[1, inputFileName.Rows, inputFileName.Cols, 3];
        for (int i = 0; i < inputFileName.Rows; i++)
        {
            for (int j = 0; j < inputFileName.Cols; j++)
            { // 안된다면 byte에 

                // 혹시라도 안되면 byte의 channel에 주목해보기!!! 난 bgr2rgb도 따로 안해서 둘다 시험해보기.
                float_array[0, i, j, 0] = (UInt16)(matrix[0, i, j, 0]);

                float_array[0, i, j, 1] = (UInt16)(matrix[0, i, j, 1]);

                float_array[0, i, j, 2] = (UInt16)(matrix[0, i, j, 2]); // c++에서 복사해보고 제대로 복사되나 보고

            }
        }

        //Buffer.BlockCopy(array, 0, float_array, 0, matrix.Length);


        TFTensor tensor = float_array; // 그대로 주게되면 제대로 들어가는지도 보고.
        return tensor;
    }
    // byte ->float나 tensor -> float도 오류가 난다.

    private TFTensor int_to_float_and_div(TFTensor input_tensor)
    {
        const float scale = 255.0f;
        TFOutput input, output;
        var graph = new TFGraph();
        input = graph.Placeholder(TFDataType.UInt16); // 여기 placeholder에서 무슨 문제가 있는지도 보고
        output = graph.Cast(input, DstT: TFDataType.Float);
        // Execute that graph to normalize this one image
        using (var session = new TFSession(graph))
        {
            var normalized = session.Run(
                inputs: new[] { input },
                inputValues: new[] { input_tensor },
                outputs: new[] { output });

            return normalized[0];
        }

    }

    private unsafe float[,] return_value(TFTensor input)
    {
        float* input_pointer = (float*)input.Data;
        float[,] array = new float[1,3];
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[i, j] = input_pointer[j];
            }
        }
        return array;
    }

    private Mat draw_axis(Mat img, float yaw, float pitch, float roll, int size = 50)
    {
        //print("yaw : " + yaw + " roll = "+ roll+ " pitch = " + pitch);
        pitch = (float)(pitch * Math.PI / 180);
        yaw = (float)-(yaw * Math.PI / 180);
        roll = (float)(roll * Math.PI / 180); // yaw is pich , pitch is yaw
        float tdx = img.Size(1)/2;
        float tdy = img.Size(0) / 2;

        // X-Axis pointing to right. drawn in red
        float x1 = size * (float)(Math.Cos(yaw) * Math.Cos(roll)) + tdx;
        float y1 = size * (float)(Math.Cos(pitch) * Math.Sin(roll) + Math.Cos(roll) * Math.Sin(pitch) * Math.Sin(yaw)) + tdy;

        // Y-Axis | drawn in green
        // v
        float x2 = size * (float)(-Math.Cos(yaw) * Math.Sin(roll)) + tdx;
        float y2 = size * (float)(Math.Cos(pitch) * Math.Cos(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll)) + tdy;

        // Z-Axis (out of the screen) drawn in blue
        float x3 = size * (float)(Math.Sin(yaw)) + tdx;
        float y3 = size * (float)(-Math.Cos(yaw) * Math.Sin(pitch)) + tdy;

        OpenCvSharp.Cv2.Line(img, (int)tdx, (int)tdy, (int)x1, (int)y1, new Scalar(0, 0, 255), 3);
        OpenCvSharp.Cv2.Line(img, (int)tdx, (int)tdy, (int)x2, (int)y2, new Scalar(0, 255, 0), 3);
        OpenCvSharp.Cv2.Line(img, (int)tdx, (int)tdy, (int)x3, (int)y3, new Scalar(255, 0, 255), 2);

        return img;
    }

    private float rotate_Charactor(float num)
    {
        float round_Num = 0;
        if (num < 0 && num > -10)
            round_Num = 0;
        else if (num < -10 && num > -20)
            round_Num = -20;
        else if (num < -20 && num > -30)
            round_Num = -45;
        else if (num < -30 && num > -40)
            round_Num = -60;
        else if (num < -40)
            round_Num = -90;
        else if (num > 0 && num < 10)
            round_Num = 0;
        else if (num > 10 && num < 20)
            round_Num = 20;
        else if (num > 20 && num < 30)
            round_Num = 45;
        else if (num > 30 && num < 40)
            round_Num = 60;
        else if (num > 40)
            round_Num = 90;
        return round_Num;
    }

    private unsafe Scalar get_position(Point center_point)
    {
        int blue = face_position.At<Vec3b>(center_point.Y, center_point.X)[0];
        int green = face_position.At<Vec3b>(center_point.Y, center_point.X)[1];
        int red = face_position.At<Vec3b>(center_point.Y, center_point.X)[2];
        Scalar color = new Scalar(blue, green, red);
        print("blue = " + blue + " green = " + green + " red = " + red);
        for(int i=0; i<colors.Count; i++)
        {
            if (colors[i] == color)
            {
                return color;
            }
        }
        return new Scalar(0, 0, 0);
    }
}
