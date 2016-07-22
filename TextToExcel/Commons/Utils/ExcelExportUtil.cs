using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace TextToExcel.Commons.Utils
{   
    /// <summary>
    /// Excel导出工具类
    /// 作者:李文禾
    /// </summary>
    class ExcelExportUtil
    {
        /// <summary>
        ///  Excel导出方法
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="data">数据</param>
        public static void Export(string path, List<string[]> data)
        {
            Export(path, null, data);
        }

        /// <summary>
        ///  Excel导出方法
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="filename">文件名称</param>
        /// <param name="data">数据</param>
        public static void Export(string path, string filename, List<string[]> data)
        {
            Export(path, filename, data, null);
        }

        /// <summary>
        ///  Excel导出方法
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="filename">文件名称</param>
        /// <param name="data">数据</param>
        /// <param name="template">模板文件</param>
        public static void Export(string path, string filename, List<string[]> data, Stream template)
        {
            IWorkbook workbook;
            if (null == template)
            {
                workbook = new HSSFWorkbook();
                workbook.CreateSheet("sheet1");
            }
            else
            {
                workbook = new HSSFWorkbook(template);
            }

            // 注入数据
            ISheet sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            if (null != row)
            {
                int numbersOfCol = row.Cells.Count;
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.CreateRow(i + 1).CreateCell(0).SetCellValue(i + 1);
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        if ((j + 1) == numbersOfCol)
                        {
                            break;
                        }
                        sheet.GetRow(i + 1).CreateCell(j + 1).SetCellValue(data[i][j]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.CreateRow(i + 1).CreateCell(0).SetCellValue(i + 1);
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        sheet.GetRow(i + 1).CreateCell(j + 1).SetCellValue(data[i][j]);
                    }
                }
            }

            // 处理文件名与路径
            filename = (null != filename) ? filename : DateTime.Now.ToString("yyyyMMddHHmmss");
            string filepath = (-1 != path.IndexOf(@"/")) ? path + @"/" + filename : path + @"\" + filename;

            // 输出文件
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            workbook.Write(fs);

            fs.Close();
            workbook.Close();
            template.Close();
        }
    }
}
