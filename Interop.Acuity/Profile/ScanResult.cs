using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Mathematics;

namespace Interop.Acuity.Profile
{
    public sealed class ScanResult
    {
        private readonly int _encoderPosition;
        private readonly Vector3[] _xziPoints;
        private readonly byte _firstImageNumber;
        private readonly byte _secondImageNumber;

        private const int ExpectedNumberOfXPixels = 290;
        private const int StrideOfScanData = 5;
        private const int ImageNumberOffset = 62;
        private const int ScanDataOffset = 66;
        private const int EncoderPositionOffset = 1525;

        private ScanResult(int encoderPosition, byte firstImageNumber, byte secondImageNumber, Vector3[] xziPoints)
        {
            _encoderPosition = encoderPosition;
            _firstImageNumber = firstImageNumber;
            _secondImageNumber = secondImageNumber;
            _xziPoints = xziPoints;
        }

        public static byte PeekImageNumber(byte[] buffer)
        {
            return buffer[ImageNumberOffset];
        }

        public static ScanResult FromSingleBuffer(ScannerInformation scanInfo, byte[] buffer, bool useSymmetricXCoordinates)
        {
            var numXPixels = scanInfo.NumXPixels;
            if (numXPixels != ExpectedNumberOfXPixels) throw new ArgumentException("Unsupported number of X pixels.");

            var encoderPosition = ReadSigned27BitValue(buffer, EncoderPositionOffset);
            var imageNumber = buffer[ImageNumberOffset];

            var xScale = scanInfo.XScaleFactor;
            var zScale = scanInfo.ZScaleFactor;
            var iScale = 1.0 / byte.MaxValue;

            var xOffset = useSymmetricXCoordinates ? -(scanInfo.ScanRangeEnd / 10.0) / 2 : 0;

            var points = new Vector3[numXPixels];
            for (int i = 0; i < numXPixels; i++)
            {
                var offset = ScanDataOffset + i * StrideOfScanData;

                points[i] = ReadVector(buffer, offset, xScale, zScale, iScale, xOffset);
            }

            return new ScanResult(encoderPosition, imageNumber, 0, points);
        }

        public static ScanResult FromBufferPair(ScannerInformation scanInfo, byte[] buffer1, byte[] buffer2, DroppedFrameResponse droppedFrameResponse, bool useSymmetricXCoordinates)
        {
            var numXPixels = scanInfo.NumXPixels;
            if (numXPixels != ExpectedNumberOfXPixels) throw new ArgumentException("Unsupported number of X pixels.");

            var encoderPosition = ReadSigned27BitValue(buffer1, EncoderPositionOffset);
            var imageNumber = buffer1[ImageNumberOffset];
            var secondImageNumber = buffer2[ImageNumberOffset];

            if (secondImageNumber != imageNumber + 1)
            {
                // buffer pair is not sequential, we dropped a frame
                switch (droppedFrameResponse)
                {
                    case DroppedFrameResponse.ThrowException:
                    default:
                        throw new ApplicationException("Non-sequential buffer pair.");
                    case DroppedFrameResponse.Ignore:
                    case DroppedFrameResponse.IgnoreOnce:
                        return null;
                    case DroppedFrameResponse.ReturnNonInterleaved:
                        return FromSingleBuffer(scanInfo, buffer2, useSymmetricXCoordinates);
                }
            }

            var xScale = scanInfo.XScaleFactor;
            var zScale = scanInfo.ZScaleFactor;
            var iScale = 1.0 / byte.MaxValue;

            var xOffset = useSymmetricXCoordinates ? -(scanInfo.ScanRangeEnd / 10.0) / 2 : 0;

            var points = new Vector3[2 * numXPixels];
            for (int i = 0; i < numXPixels; i++)
            {
                var offset = ScanDataOffset + i * StrideOfScanData;

                points[2 * i] = ReadVector(buffer1, offset, xScale, zScale, iScale, xOffset);
                points[2 * i + 1] = ReadVector(buffer2, offset, xScale, zScale, iScale, xOffset);
            }

            return new ScanResult(encoderPosition, imageNumber, secondImageNumber, points);
        }

        private static Vector3 ReadVector(byte[] buffer, int offset, double xScale, double zScale, double iScale, double xOffset)
        {
            var rawX = Read14BitValue(buffer, offset);
            var rawZ = Read14BitValue(buffer, offset + 2);
            var rawIntensity = buffer[offset + 4];

            var x = (rawX * xScale) + xOffset;
            var z = rawZ * zScale;
            var i = rawIntensity * iScale;

            if (rawX < 0 || rawZ < 0 || rawX > 4095 || rawZ > 4095)
            {
                x = double.NaN;
                z = double.NaN;
            }

            return new Vector3(x, z, i);
        }

        private static UInt16 Read14BitValue(byte[] buffer, int index)
        {
            // see section 7.1 of manual
            var low = buffer[index] & 0x7f;
            var high = buffer[index + 1] & 0x7f;

            var result = (high << 7) | low;

            return (UInt16)result;
        }

        private static Int32 ReadSigned27BitValue(byte[] buffer, int index)
        {
            // see section 7.2 of manual
            int result = 0;
            result |= buffer[index + 3] & 0x3f;
            result <<= 7;
            result |= buffer[index + 2] & 0x7f;
            result <<= 7;
            result |= buffer[index + 1] & 0x7f;
            result <<= 7;
            result |= buffer[index] & 0x7f;

            if ((result & 0x8000000) == 0x8000000)
            {
                // this is a negative number
                // get 1's complement
                result = (~result) & 0x7fffffff;
                // get 2's complement
                result += 1;
                // flip sign
                result *= -1;
            }

            return result;
        }

        public IEnumerable<Vector2> Points
        {
            get
            {
                var result = from point in _xziPoints
                             where !(double.IsNaN(point.X) || double.IsNaN(point.Y))
                             select new Vector2(point.X, point.Y);

                return result;
            }
        }

        public IEnumerable<Vector3> PointsWithIntensity
        {
            get
            {
                var result = from point in _xziPoints
                             where !(double.IsNaN(point.X) || double.IsNaN(point.Y))
                             select point;

                return result;
            }
        }

        public bool IsInterleaved
        {
            get
            {
                return _secondImageNumber != 0;
            }
        }

        public int EncoderPosition
        {
            get
            {
                return _encoderPosition;
            }
        }
    }
}
