using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Condemned_DDS_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = string.Join(" ", args);
            if(File.Exists(fileName))
            {
                if(!fileName.ToLower().EndsWith(".dds"))
                {
                    Console.WriteLine("Program is only meant to be used on DDS files!");
                    Console.ReadKey();
                    return;
                }

                byte[] array = File.ReadAllBytes(string.Join(" ", args));
                if(array.Length == 0x0)
                {
                    Console.WriteLine("File is empty. Skipping!");
                    return;
                }

                string header = "";
                int i = 0;
                while (array[i] != 0x0)
                {
                    header += (char)array[i];
                    i++;
                }

                if (header.StartsWith("TEXR"))
                {
                    Console.WriteLine("Detected MonolithDDS file. Converting to normal DDS file.");
                    int indexOfNormalHeader = FindIndexOfString(array, "DDS |");
                    var newArray = array.ToList();
                    newArray.RemoveRange(0, indexOfNormalHeader);
                    File.WriteAllBytes(fileName, newArray.ToArray());
                }
                else
                {
                    Console.WriteLine("Detected normal DDS file. Converting to MonolithDDS.");
                    var newArray = array.ToList();
                    newArray.InsertRange(0, new byte[] { 0x54, 0x45, 0x58, 0x52, 0x01, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x00 }); //TEXR with some stuff behind... hopefully not important what stuff.
                    File.WriteAllBytes(fileName, newArray.ToArray());
                }

                Console.WriteLine("Done!");
                return;
            }

            Console.WriteLine("File doesn't exists?!");
            Console.ReadKey();
        }

        private static int FindIndexOfString(byte[] array, string v)
        {
            byte[] stringAsArray = Encoding.GetEncoding("UTF-8").GetBytes(v.ToCharArray());
            for(int i=0; i<array.Length-v.Length; i++)
            {
                if (CompareByteSequences(array, i, stringAsArray))
                    return i;
            }
            return -1;
        }

        private static bool CompareByteSequences(byte[] array1, int array1Index, byte[] array2)
        {
            for(int i=0; i+array1Index<array1.Length && i<array2.Length; i++)
            {
                if (array1[i+array1Index] != array2[i])
                    return false;
            }
            return true;
        }
    }
}
