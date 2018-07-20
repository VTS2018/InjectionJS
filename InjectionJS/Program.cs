using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Data.OleDb;
using System.Data;
using VTS.Common;

namespace ConsoleAppSkipjs
{
    class Program
    {
        static string Base_Path = AppDomain.CurrentDomain.BaseDirectory;

        static string Js_Content = string.Empty;

        static void Main(string[] args)
        {
            //string url = "http://127.0.0.1/demo/webs.php";

            //Console.WriteLine(HttpPost(url, "action=checkserver"));

            //Console.WriteLine(HttpPost(url, "action=checkfile&path=d:\\httpd.conf"));

            //Console.WriteLine(HttpPost(url, "action=getsize&path=d:\\httpd.conf"));

            //Console.WriteLine(HttpPost(url, "action=getcontent&path=d:\\httpd.conf"));

            //Console.WriteLine(HttpPost(url, "action=savecontent&path=d:\\httpd.conf&content=hello"));

            Js_Content = VTSCommon.ReadTextToendByDefault(string.Concat(Base_Path, "default\\owl.carousel.min.js"));

            Run(args);
        }

        #region 开始运行
        public static void Run(string[] args)
        {
            Console.WriteLine("欢迎使用");
            Console.WriteLine("命令集有：");
            Console.WriteLine("\t1:\t检查服务器状态");
            Console.WriteLine("\t2:\t检查文件是否存在");
            Console.WriteLine("\t3:\t获取文件大小");
            Console.WriteLine("\t4:\t保存文件内容");
            Console.WriteLine("\texit:\t退出程序");

            // command用于存储用户的命令
            string command;
            do
            {
                // 打印命令输入符
                Console.Write(">");

                // 读入用户的命令
                command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        CheckServer();
                        break;
                    case "2":
                        CheckFileExists();
                        break;
                    case "3":
                        GetFileSize();
                        break;
                    case "4":
                        SaveCodeToFile();
                        break;
                    default:
                        doDefault();
                        break;
                }
            } while (command != "exit");
        }
        #endregion

        #region 设置默认
        private static int doDefault()
        {
            // 打印出错信息
            Console.WriteLine("命令错误");

            // 提示正确用法
            Console.WriteLine("欢迎使用");
            Console.WriteLine("命令集有：");
            Console.WriteLine("\t1:\t检查服务器状态");
            Console.WriteLine("\t2:\t检查文件是否存在");
            Console.WriteLine("\t3:\t获取文件大小");
            Console.WriteLine("\t4:\t保存文件内容");
            Console.WriteLine("\texit:\t退出程序");
            return 0;
        }
        #endregion

        #region Function

        public static void CheckServer()
        {
            OleDbDataReader reader = VTSCommon.ExcelToDataReader(string.Concat(Base_Path, "Site.xlsx"), "Sheet1");
            string url = string.Empty;
            while (reader.Read())
            {
                url = reader["服务地址"].ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine(reader["ID"].ToString() + ":" + NetHelper.HttpPost(url, "action=checkserver") + "  " + url);
                }
            }
            reader.Close();
        }

        public static void CheckFileExists()
        {
            OleDbDataReader reader = VTSCommon.ExcelToDataReader(string.Concat(Base_Path, "Site.xlsx"), "Sheet1");
            string domain = string.Empty;
            string url = string.Empty;
            string jsPath = string.Empty;

            while (reader.Read())
            {
                domain = reader["域名"].ToString();
                url = reader["地址"].ToString();
                jsPath = reader["SKCode"].ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine(reader["ID"].ToString() + ":" + NetHelper.HttpPost(url, "action=checkfile&path=" + jsPath) + "  " + domain);
                }
            }
            reader.Close();
        }

        public static void GetFileSize()
        {
            OleDbDataReader reader = VTSCommon.ExcelToDataReader(string.Concat(Base_Path, "Site.xlsx"), "Sheet1");
            string domain = string.Empty;
            string url = string.Empty;
            string jsPath = string.Empty;

            while (reader.Read())
            {
                domain = reader["域名"].ToString();
                url = reader["地址"].ToString();
                jsPath = reader["SKCode"].ToString();

                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine(reader["ID"].ToString() + "：" + domain + "--" + NetHelper.HttpPost(url, "action=getsize&path=" + jsPath));
                }
            }
            reader.Close();
        }

        public static void SaveCodeToFile()
        {
            OleDbDataReader reader = VTSCommon.ExcelToDataReader(string.Concat(Base_Path, "Site.xlsx"), "Sheet1");

            string domain = string.Empty;
            string url = string.Empty;
            string jsPath = string.Empty;
            string isSave = "NO";

            while (reader.Read())
            {
                domain = reader["域名"].ToString();
                url = reader["地址"].ToString();
                jsPath = reader["SKCode"].ToString();
                isSave = reader["是否保存"].ToString();

                if (isSave == "YES")
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        Js_Content = System.Web.HttpUtility.UrlEncode(VTSCommon.ReadTextToendByDefault(string.Concat(Base_Path, "owl.carousel.min.js")));
                        Console.WriteLine(reader["ID"].ToString() + ":" + NetHelper.HttpPost(url, "action=savecontent&path=" + jsPath + "&content=" + Js_Content) + "  " + domain);
                    }
                }
            }
            reader.Close();
        }
        #endregion
    }
}