using OpenCvSharp;

namespace DA_N6.Services
{
    public static class FaceRecognitionService
    {
        private static CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

        public static bool ValidateFace(Mat frame)
        {
            var gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
            var faces = faceCascade.DetectMultiScale(gray, 1.1, 6);
            return faces.Length > 0;
        }
    }
}
