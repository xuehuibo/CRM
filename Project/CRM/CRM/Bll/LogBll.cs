using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class LogBll
    {
        public static void Write(IDal dal, CLog log)
        {
            try
            {
                int i;
                dal.Execute(
                    "INSERT INTO dbo.tXtLog( LogDate, LogContent, LogType )VALUES  ( GETDATE(),@LogContent,@LogType)",
                    out i,
                    dal.CreateParameter("@LogContent", log.LogContent),
                    dal.CreateParameter("@LogType", log.LogType));
            }
            catch (Exception ex)
            {
                Write(log);
            }
        }

        /// <summary>
        /// 数据库无法写日志，写本地文件
        /// </summary>
        /// <param name="log"></param>
        public static void Write(CLog log)
        {
            var filename =string.Format("{0}/Logs/log{1}.txt", HttpContext.Current.Server.MapPath("/"), DateTime.Now.ToString("yyMMdd"));
            if (!Directory.Exists(string.Format("{0}/Logs", HttpContext.Current.Server.MapPath("/"))))
            {
                Directory.CreateDirectory(string.Format("{0}/Logs", HttpContext.Current.Server.MapPath("/")));
            }
            var fs = !File.Exists(filename) ? new FileStream(filename, FileMode.CreateNew) : new FileStream(filename,FileMode.Append);
            var logText = Encoding.Default.GetBytes(string.Format("时间：{0}#操作人：{1}#结果：{2}#内容：{3}#\r\n",
               log.LogDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                log.LogUser,
                log.LogType.ToString(),
                log.LogContent)
            );
            fs.Write(logText,0,logText.Length);
            fs.Close();
        }

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static CLog[] List(IDal dal, int page)
        {
            int i;
            const int length = 50;
            var startIndex = (page-1)*length + 1;
            var dt = dal.Select("SELECT * FROM tXtLog ORDER BY SerialNo", startIndex, length, out i);
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CLog
                {
                    Id = Convert.ToInt32(row["Id"]),
                    LogContent = Convert.ToString(row["LogContent"]),
                    LogDate = Convert.ToDateTime(row["LogDate"]),
                    LogType = (LogType)Convert.ToInt16(row["LogType"]),
                    LogUser = Convert.ToString(row["LogUser"])
                }).ToArray();
        }
    }
}