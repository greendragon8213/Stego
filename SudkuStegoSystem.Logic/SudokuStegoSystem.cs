using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SudkuStegoSystem.Logic.Helpers;
using SudkuStegoSystem.Logic.Models;
using System.IO;

namespace SudkuStegoSystem.Logic
{
    /// <summary>
    /// Just encrypts and decrypts data
    /// </summary>
    public class SudokuStegoSystem
    {
        private const int ExpectedSudokuSize = 256;
        
        public Image Encrypt(Image container, SecretFile secretFile, SudokuMatrix sudokuKey)
        {
            if(sudokuKey.SudokuSize != ExpectedSudokuSize)
            {
                throw new InvalidOperationException($"This steganography method works only with matrix {ExpectedSudokuSize}x{ExpectedSudokuSize}.");
            }

            Tuple<byte[], BitmapData> cover = container.GetByteArrayByImageFile(ImageLockMode.ReadWrite);
            byte[] coverBytes = cover.Item1;
            BitmapData coverBitmap = cover.Item2;
            byte[] secretData = GetSecretBytesToEncode(secretFile);

            #region Embedding secret data into the container

            int i = 0;
            int k = 0;
            while (k + 2 < secretData.Length && i < coverBytes.Length / 2)//ToDo check borders!
            {
                for (int j = 0; j < 3; j++)
                {
                    byte currentByte = secretData[k + j];
                    SudokoCoordinates initialCoordinates = new SudokoCoordinates(coverBytes[i + j], coverBytes[i + j + 3]);

                    //ToDo hardcode!
                    SudokoCoordinates nearestCoordinates = sudokuKey.FindNearestCoordinates(currentByte, initialCoordinates);

                    coverBytes[i + j] = nearestCoordinates.X;
                    coverBytes[i + j + 3] = nearestCoordinates.Y;
                }
                k += 3;
                i += 6;
            }

            Marshal.Copy(coverBytes, 0, coverBitmap.Scan0, coverBytes.Length);
            container.UnlockBits(coverBitmap);
            //container.Save(stegocontainerFilePath, ImageFormat.Bmp);
            //container.Save(stegocontainerFilePath);

            return container;

            #endregion
        }

        public SecretFile Decrypt(Image stegocontainer, SudokuMatrix sudokuKey)
        {
            if (sudokuKey.SudokuSize != ExpectedSudokuSize)
            {
                throw new InvalidOperationException($"This steganography method works only with matrix {ExpectedSudokuSize}x{ExpectedSudokuSize}.");
            }

            Tuple<byte[], BitmapData> stego = stegocontainer.GetByteArrayByImageFile(ImageLockMode.ReadOnly);
            byte[] stegoBytes = stego.Item1;
            BitmapData stegoBitmap = stego.Item2;

            var fileLengthValueInByteArray = new byte[] {
                sudokuKey[stegoBytes[0], stegoBytes[3]],
                sudokuKey[stegoBytes[1], stegoBytes[4]],
                sudokuKey[stegoBytes[2], stegoBytes[5]],
                sudokuKey[stegoBytes[6], stegoBytes[9]]
            };
            int secretFilePayloadLength = BitConverter.ToInt32(fileLengthValueInByteArray, 0);

            // decode secret file name length (stored in a 1 byte)
            int secretFileNameLength = sudokuKey[stegoBytes[7], stegoBytes[10]];

            // decode secret file name
            int stegoContainerOffset = 8;
            int offsetInsidePixels = 0;

            byte[] fileNameBytes = new byte[secretFileNameLength];
            int iterationToReadSecretFileName = 0;
            while (iterationToReadSecretFileName < secretFileNameLength && stegoContainerOffset < stegoBytes.Length)
            {
                offsetInsidePixels = stegoContainerOffset % 3;
                while (iterationToReadSecretFileName < secretFileNameLength && offsetInsidePixels < 3)
                {
                    fileNameBytes[iterationToReadSecretFileName] = sudokuKey[stegoBytes[stegoContainerOffset], stegoBytes[stegoContainerOffset + 3]];
                    iterationToReadSecretFileName++;
                    stegoContainerOffset++;
                    offsetInsidePixels++;
                }
                if (offsetInsidePixels == 3)
                {
                    stegoContainerOffset += 3;
                }
            }
            string secretFileName = Encoding.ASCII.GetString(fileNameBytes, 0, fileNameBytes.Length);

            // decode secret file payload
            byte[] secretFilePayloadBytes = new byte[secretFilePayloadLength];
            int iterationToReadSecretFiePayload = 0;
            while (iterationToReadSecretFiePayload < secretFilePayloadLength && stegoContainerOffset < stegoBytes.Length)
            {
                offsetInsidePixels = stegoContainerOffset % 3;
                while (iterationToReadSecretFiePayload < secretFilePayloadLength && stegoContainerOffset < stegoBytes.Length && offsetInsidePixels < 3)
                {
                    secretFilePayloadBytes[iterationToReadSecretFiePayload] =
                        sudokuKey[stegoBytes[stegoContainerOffset], stegoBytes[stegoContainerOffset + 3]];
                    iterationToReadSecretFiePayload++;
                    stegoContainerOffset++;
                    offsetInsidePixels++;
                }
                if (offsetInsidePixels == 3)
                {
                    stegoContainerOffset += 3;
                }
            }

            stegocontainer.UnlockBits(stegoBitmap);
            return new SecretFile(secretFileName, secretFilePayloadBytes);            
        }

        /// <summary>
        /// Gets bytes to encode by file. FL = 4byte, FNL = 1byte, FN = computed, Payload = computed 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private byte[] GetSecretBytesToEncode(SecretFile file)
        {            
            byte[] fileLength = BitConverter.GetBytes(file.Payload.Length);//4 bytes

            byte[] fileNameLength = new byte[1] { BitConverter.GetBytes(file.FileName.Length).First() };//1 byte should be enought
            byte[] fileName = Encoding.ASCII.GetBytes(file.FileName);

            byte[] resultBytes = new byte[4 + 1 + fileName.Length + file.Payload.Length];
            Buffer.BlockCopy(fileLength, 0, resultBytes, 0, fileLength.Length);
            Buffer.BlockCopy(fileNameLength, 0, resultBytes, fileLength.Length, fileNameLength.Length);
            Buffer.BlockCopy(fileName, 0, resultBytes, fileLength.Length + fileNameLength.Length, fileName.Length);
            Buffer.BlockCopy(file.Payload, 0, resultBytes, fileLength.Length + fileNameLength.Length + fileName.Length, file.Payload.Length);

            //Marshal.Copy(ditherBmpData.Scan0, ditherBytes, 0, ditherBytes.Length);

            return resultBytes;
        }
    }
}
