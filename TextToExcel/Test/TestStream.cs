using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TextToExcel.Commons.Filter;

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
                DateTime dt1 = DateTime.Now;
                while (null != (str = reader.ReadLine()))
                {
                    //Console.WriteLine(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(str)));
                    FilterChain chain = new FilterChain().AddFilter(new NameAndIdCardFilter()).AddFilter(new KeywordFilter());
                    if (chain.DoFilter(str, out outStr))
                    {
                        Console.WriteLine(outStr);
                    }
                }
                Console.WriteLine((DateTime.Now - dt1));
            }

            //Build();
        }

        public static void Build()
        {
            if (File.Exists(@"F:\test.txt"))
            {
                File.Delete(@"F:\test.txt");
            }
            FileStream file = File.OpenWrite(@"F:\test.txt");
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            for (int i = 0; i < 100; i++)
            {
                writer.WriteLine("2016/6/23 14:07:27  史专扬_231002198004171038.jpg  0.61  通过  正例");
            }
            writer.Flush();
            writer.Close();
            file.Close();
        }
    }
}
