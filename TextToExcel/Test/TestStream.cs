using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using TextToExcel.Commons.Filter;
using TextToExcel.Commons.Utils;

namespace TextToExcel.Test
{
    class TestStream
    {
        public static void Test()
        {
            Console.WriteLine("Hello World");
            FileStream fileStream = File.OpenRead(@"F:\test.txt");

            if (fileStream.CanRead)
            {
                int i = 0;
                int buffer = 32;

                byte[] allBytes = new byte[fileStream.Length];
                byte[] tempBytes = new byte[buffer];

                while ((i = fileStream.Read(tempBytes, 0, buffer)) > 0)
                {
                    long aviLength = fileStream.Length - fileStream.Position;
                    for (long j = 0; j < buffer; j++)
                    {
                        allBytes[fileStream.Position - buffer + j] = tempBytes[j];
                    }
                    buffer = buffer < aviLength ? buffer : (int)aviLength;
                }

                MemoryStream memoeryStream = new MemoryStream(allBytes);
                StreamReader reader = new StreamReader(memoeryStream, Encoding.Default);

                string str;
                string outStr;
                List<string[]> data = new List<string[]>();
                DateTime dt1 = DateTime.Now;
                //FileStream xlsStream = File.Open(TextToExcel.Properties.Resources.ExcelXlsTemplate, FileMode.Open, FileAccess.Read);
                using (MemoryStream ms = new MemoryStream(TextToExcel.Properties.Resources.Template_xlsx, false))
                {
                    while (null != (str = reader.ReadLine()))
                    {
                        //Console.WriteLine(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(str)));
                        FilterChain chain = new FilterChain().AddFilter(new NameAndIdCardFilter()).AddFilter(new KeywordFilter());
                        if (chain.DoFilter(str, out outStr))
                        {
                            string[] strArr = Regex.Split(outStr, " ");
                            string[] tempStrArr = new string[strArr.Length - 1];
                            for (int ix = 0; ix < tempStrArr.Length; ix++)
                            {
                                if (ix == 0)
                                {
                                    tempStrArr[ix] = strArr[0] + " " + strArr[1];
                                }
                                else
                                {
                                    tempStrArr[ix] = strArr[ix + 1];
                                }
                            }
                            data.Add(tempStrArr);
                        }
                    }
                    ExcelExportUtil.Export(@"F:\", "222.xls", data, ms);
                    Console.WriteLine((DateTime.Now - dt1));
                }
            }

            //Build();
        }

        public static void Build()
        {
            if (File.Exists(@"F:\test.txt"))
            {
                File.Delete(@"F:\test.txt");
            }
            FileStream file = File.OpenWrite(@"F:\VerifyResult_2016-06-27.txt");
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            for (int i = 0; i < 500000; i++)
            {
                writer.WriteLine("2016/6/27 14:07:27  史专扬_231002198004171038.jpg  0.61  通过  正例");
            }
            writer.Flush();
            writer.Close();
            file.Close();
        }

        public static void main(string[] args)
        {
            Console.WriteLine("hello world1");
        }

        public static void TestResources()
        {
            WritePrivateProfileString("Test", "name", "liwenhe", @"F:\Conf.ini");
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            int nSize, string lpFileName);
    }
}
