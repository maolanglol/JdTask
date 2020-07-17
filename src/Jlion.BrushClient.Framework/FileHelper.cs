using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sodao.Juketool.WeChat.EleClient.Framework.Model;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.WeChat.EleClient.Framework.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 盗链接口
        /// </summary>
        public const string WXHotlink = "http://img01.store.sogou.com/net/a/04/link?appid=100520029&url=";
        /// <summary>
        /// 文件地址中的关键词
        /// </summary>
        public const string FileKeyName = "filekey";


        /// <summary>
        /// 执行 上传
        /// </summary>
        /// <param name="host"></param>
        /// <param name="file"></param>
        /// <param name="fileName">上传的文件名称</param>
        /// <returns></returns>
        public static async Task<string> Execute(string host, string file, string fileName = "")
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(file))
                return string.Empty;

            var fileInfo = new FileInfo(file);
            using (var fs = fileInfo.OpenRead())
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = fileInfo.Name;
                return await Execute(host, fs, fileName);
            }
        }

        /// <summary>
        /// 执行 上传
        /// </summary>
        /// <param name="host"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> Execute(string host, Stream stream, string fileName)
        {
            if (string.IsNullOrEmpty(host) || stream == null)
                return string.Empty;

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                fileContent.Headers.ContentDisposition.FileName = fileName;
                content.Add(fileContent);

                try
                {
                    var result = await client.PostAsync(host, content);
                    if (result.StatusCode.ToString() != "OK")
                        return string.Empty;

                    var json = await result.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<JObject>(json);
                    if (obj?["Url"] == null)
                        return string.Empty;

                    return obj["Url"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 执行视频 上传
        /// </summary>
        /// <param name="host"></param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<VideoFileModel> ExecuteVideo(string host, string file, string fileName = "")
        {
            var resp = new VideoFileModel();
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(file))
                return null;

            var fileInfo = new FileInfo(file);
            using (var fs = fileInfo.OpenRead())
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = fileInfo.Name;
                return await ExecuteVideo(host, fs, fileName);
            }
        }

        /// <summary>
        /// 视频文件上传
        /// </summary>
        /// <param name="host"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<VideoFileModel> ExecuteVideo(string host, Stream stream, string fileName)
        {
            var response = new VideoFileModel();
            if (string.IsNullOrEmpty(host) || stream == null)
                return null;

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                fileContent.Headers.ContentDisposition.FileName = fileName;
                content.Add(fileContent);

                try
                {
                    var result = await client.PostAsync(host, content);
                    if (result.StatusCode.ToString() != "OK")
                        return null;

                    var json = await result.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<JObject>(json);
                    if (string.IsNullOrEmpty(obj?["Url"].ToString()))
                        return null;

                    if (!string.IsNullOrEmpty(obj["Url"]?.ToString()))
                    {
                        response.Url = obj["Url"]?.ToString();
                    }
                    if (!string.IsNullOrEmpty(obj["Height"]?.ToString()))
                    {
                        response.Height = Convert.ToInt32(obj["Height"]?.ToString());
                    }
                    if (!string.IsNullOrEmpty(obj["Width"]?.ToString()))
                    {
                        response.Width = Convert.ToInt32(obj["Width"].ToString());
                    }
                    if (!string.IsNullOrEmpty(obj["PicUrl"]?.ToString()))
                    {
                        response.Thumb = obj["PicUrl"].ToString();
                    }

                    return response;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        public static void Download(string url, string filePath, Action whenException = null)
        {
            try
            {
                var client = new WebClient();
                client.DownloadFile(url, filePath);
            }
            catch
            {
                whenException?.Invoke();
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFile(string userKey, string url)
        {
            try
            {
                var last = url.LastIndexOf("/");
                var fileName = url.Substring(last + 1);
                var filePath = GetFilePath(userKey, fileName);
                if (File.Exists(filePath))
                    return filePath;

                Download(url, filePath, () => filePath = string.Empty);
                return filePath;
            }
            catch
            {
                return url;
            }
        }

        /// <summary>
        /// 获取excel文件
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExcelFile(string userKey, string fileName)
        {
            return GetFilePath(userKey, fileName);
        }

        /// <summary>
        /// 获取语音转换后的文件
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetVoiceFile(string userKey, string url)
        {
            try
            {
                var last = url.LastIndexOf("/");
                var fileName = url.Substring(last + 1);
                fileName = fileName.Replace(".msg", "");
                var filePath = GetFilePath(userKey, fileName);
                // 不存在下载
                if (!File.Exists(filePath))
                    Download(url, filePath, () => filePath = string.Empty);

                var transedFile = AmrToMP3Helper.DoTrans(filePath);
                return transedFile;
            }
            catch
            {
                return url;
            }
        }

        /// <summary>
        /// 获取微信的图片
        /// 0、http://wxim.juketool.com/uploadedfiles/0b669fb0-8053-42e4-acab-bc8828fdd399.jpg
        /// 1、http://emoji.qpic.cn/wx_emoji/CCdNpyiaEOnDq3iasJVyzeibWP8pXuVvxudE4FEgI4cNKeF6bNU1FBIuRenzKawdNdr/
        /// 2、http://mmbiz.qpic.cn/mmemoticon/ajNVdqHZLLAszSAQ4gJlh6wsAv7iaia37RPEqiclQHyIySyf6nQL4LC5G9s5DRhbNiaU/0
        /// 3、http://shmmsns.qpic.cn/mmsns/OxUBpiaYgpHjWeeNciczib7DtSgesDxn7ZHeBFicicPese6m3wjlWDeJevV8ZdA9scYdzI2UTD7dD3aE/150
        /// 4、http://vweixinthumb.tc.qq.com/150/20250/snsvideodownload?filekey=30340201010420301e02020096040253480410b900fc7318a395fa51cac95f9e54c47c02021a77040d00000004627466730000000131&hy=SH&storeid=32303138303531383036313435343030303263363461353534343637356531623730333730613030303030303936&bizid=1023
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="url"></param>
        /// <param name="waitDownload">是否等待下载</param>
        /// <returns></returns>
        public static string GetImageFile(string userKey, string url)
        {
            try
            {
                if (!IsHttp(url))
                    return url;

                var filePath = string.Empty;
                if (url.EndsWith("/0") || url.EndsWith("/150") || url.EndsWith("/"))
                {
                    #region 尺寸
                    var size = string.Empty;
                    if (url.EndsWith("/0") || url.EndsWith("/150"))
                        size = url.Substring(url.LastIndexOf("/") + 1);
                    if (!string.IsNullOrEmpty(size))
                        size = $"_{size}";
                    #endregion

                    #region 文件名称
                    var fixUrl = url.Replace("/0", "").Replace("/150", "").TrimEnd('/');
                    var fileName = $"{fixUrl.Substring(fixUrl.LastIndexOf("/") + 1)}{size}.jpeg";
                    #endregion

                    // 微信有些图片有问题 无法下载
                    //if (url.IndexOf("shmmsns.qpic.cn/mmsns") > -1 && fileName.StartsWith("g9"))
                    //    return string.Empty;

                    filePath = GetFilePath(userKey, fileName);
                    if (File.Exists(filePath))
                        return filePath;

                    url = $"{WXHotlink}{url}";
                    Download(url, filePath, () => filePath = string.Empty);
                    return filePath;
                }
                else
                {
                    var fileName = string.Empty;
                    if (url.Contains(FileKeyName))
                        fileName = GetQueryString(FileKeyName, url) + ".jpeg";
                    else
                    {
                        // 自己服务器的图片
                        fileName = url.Substring(url.LastIndexOf("/") + 1).Replace("/", "");
                        if (fileName.IndexOf(".") == -1)
                            fileName += ".jpeg";
                    }

                    filePath = GetFilePath(userKey, fileName);
                    if (File.Exists(filePath))
                        return filePath;
                }

                // 让它自己异步下载，先用url地址
                Task.Factory.StartNew(() => Download(url, filePath, () => filePath = string.Empty));
                return url;
            }
            catch
            {
                return url;
            }
        }

        /// <summary>
        /// 获取视频内容
        /// 0、http://wxim.juketool.com/uploadedfiles/887f5386-4b4a-4393-be5b-e1baa3f8be08.mp4
        /// 1、http://shzjwxsns.video.qq.com/102/20202/snsvideodownload?filekey=30340201010420301e0201660402534804102cf336efbeb14c93a024a09aac4d68a402030b6d61040d00000004627466730000000131&hy=SH&storeid=32303138303531383035353632353030306337303034353534343637356565643730333730613030303030303636&dotrans=1&ef=1_0&bizid=1023
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetVideoFile(string userKey, string url)
        {
            try
            {
                if (!IsHttp(url))
                    return url;

                var fileName = string.Empty;
                if (url.Contains(FileKeyName))
                    fileName = GetQueryString(FileKeyName, url) + ".mp4";
                else
                {
                    // 自己服务器的视频
                    var last = url.LastIndexOf("/");
                    fileName = url.Substring(last + 1).Replace("/", "");
                    if (fileName.IndexOf(".") == -1)
                        fileName += ".mp4";
                }

                var filePath = GetFilePath(userKey, fileName);
                if (!File.Exists(filePath))
                    Download(url, filePath, () => filePath = string.Empty);

                if (!string.IsNullOrEmpty(filePath) && !File.Exists(filePath))
                    return string.Empty;

                return filePath;
            }
            catch
            {
                return url;
            }
        }

        /// <summary>
        /// 移动到本地目录中
        /// 网络图片直接返回结果
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SaveImageFile(string userKey, string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            if (IsHttp(content))
                return content;
            try
            {
                byte[] bts;
                if (content.IndexOf("data:image") > -1 && content.IndexOf(";base64,") > -1)
                {
                    var tmpArr = content.Split(',');
                    bts = Convert.FromBase64String(tmpArr[1]);
                    using (var ms = new MemoryStream(bts))
                    {
                        var bmp = new Bitmap(ms);
                        var filePath = GetFilePath(userKey, $"{Guid.NewGuid()}.png");
                        bmp.Save(filePath);
                        return filePath;
                    }
                }
                else
                {
                    if (!File.Exists(content))
                        return string.Empty;

                    var file = new FileInfo(content);
                    var filePath = GetFilePath(userKey, $"{Guid.NewGuid()}{file.Extension}");
                    File.Copy(content, filePath);
                    return filePath;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取图片宽高
        /// </summary>
        /// <param name="urlOrPath"></param>
        public static (int, int) GetImageWH(string urlOrPath)
        {
            var w_h = (0, 0);
            if (string.IsNullOrEmpty(urlOrPath))
                return w_h;

            try
            {
                w_h = ServiceIniWhHelper.ReadImage(urlOrPath);
                if (w_h.Item2 == 0)
                {
                    #region 网络图片
                    if (urlOrPath.StartsWith("http"))
                    {
                        var client = new WebClient();
                        var stream = client.OpenReadTaskAsync(urlOrPath).Result;
                        var networkImg = Image.FromStream(stream);
                        w_h = (networkImg.Width, networkImg.Height);
                    }
                    #endregion

                    #region 本地图片
                    else
                    {
                        var localImg = Image.FromFile(urlOrPath);
                        w_h = (localImg.Width, localImg.Height);
                    }
                    #endregion
                }

                ServiceIniWhHelper.WriteImage(urlOrPath, w_h.Item1, w_h.Item2);
                return w_h;
            }
            catch
            {
                return w_h;
            }
        }

        /// <summary>
        /// 获取视频的宽高
        /// </summary>
        /// <param name="urlOrPath"></param>
        /// <returns></returns>
        public static (int, int) GetVideoWH(string filePath)
        {
            var w_h = (0, 0);
            if (string.IsNullOrEmpty(filePath))
                return w_h;
            if (!File.Exists(filePath))
                return w_h;

            try
            {
                w_h = ServiceIniWhHelper.ReadVideo(filePath);
                if (w_h.Item2 <= 0)
                {
                    w_h = FFPobeHelper.GetVideoInfo(filePath);
                    ServiceIniWhHelper.WriteVideo(filePath, w_h.Item1, w_h.Item2);
                }
                return w_h;
            }
            catch
            {
                return w_h;
            }
        }

        #region 本地文件
        private static string tempFilePath = Path.Combine(ConstHelper.DocumentPath, "revfiles");
        private static string GetFilePath(string userKey, string fileName)
        {
            userKey = string.IsNullOrEmpty(userKey) ? "alluser" : userKey;
            var dir = Path.Combine(tempFilePath, userKey);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var filePath = Path.Combine(dir, fileName);
            return filePath;
        }

        /// <summary>  
        /// 获取url字符串参数，返回参数值字符串  
        /// </summary>  
        /// <param name="name">参数名称</param>  
        /// <param name="url">url字符串</param>  
        /// <returns></returns>  
        private static string GetQueryString(string name, string url)
        {
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", System.Text.RegularExpressions.RegexOptions.Compiled);
            System.Text.RegularExpressions.MatchCollection mc = re.Matches(url);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                if (m.Result("$2").Equals(name))
                {
                    return m.Result("$3");
                }
            }
            return "";
        }
        #endregion

        #region 本地表情包
        private static readonly string emoticonFoldName = "emoticons";
        public static string GetEmoticonBasePath(string accountKey)
        {
            return Path.Combine(ConstHelper.DocumentPath, emoticonFoldName, accountKey);
        }
        /// <summary>
        /// 相对路径
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetEmoticonRelationPath(string accountKey, string fileName)
        {
            return $"{emoticonFoldName}\\{accountKey}\\{fileName}";
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="fileRelPath"></param>
        /// <returns></returns>
        public static string GetEmoticonFilePath(string fileRelPath)
        {
            return Path.Combine(ConstHelper.DocumentPath, fileRelPath);
        }

        /// <summary>
        /// 绝对路径 包含创建文件夹
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetEmoticonFilePath(string accountKey, string fileName)
        {
            var relPath = GetEmoticonRelationPath(accountKey, fileName);
            var fullPath = GetEmoticonFilePath(relPath);
            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            return fullPath;
        }

        /// <summary>
        /// 创建表情文件
        /// 返回一个相对文档目录的路径
        /// </summary>
        /// <param name="picBytes">图片字节</param>
        /// <param name="accountKey">账号key</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string CreateEmoticonFile(byte[] picBytes, string accountKey)
        {
            if (picBytes == null || picBytes.Length == 0 || string.IsNullOrEmpty(accountKey))
                return string.Empty;

            var fileExt = FileHelper.GetPicExt(picBytes);
            if (string.IsNullOrEmpty(fileExt))
                return string.Empty;
            var fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}{fileExt}";
            var savePath = GetEmoticonFilePath(accountKey, fileName);

            File.WriteAllBytes(savePath, picBytes);

            return GetEmoticonRelationPath(accountKey, fileName);
        }

        /// <summary>
        /// 删除表情文件
        /// </summary>
        /// <param name="fileRelPath">文件相对路径</param>
        public static void DeleteEmoticonFile(string fileRelPath)
        {
            if (string.IsNullOrEmpty(fileRelPath))
                return;
            var fullPath = GetEmoticonFilePath(fileRelPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch { }
            }
        }
        /// <summary>
        /// 获取相对地址从表情完整路径中
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="filePath">完整路径</param>
        /// <returns></returns>
        public static string GetEmoticonRelPathFromFullPath(string accountKey, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            var basePath = GetEmoticonBasePath(accountKey);
            if (filePath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
                return filePath.Replace(ConstHelper.DocumentPath + "\\", "");
            return null;
        }
        #endregion

        #region 图片文件相关

        static readonly Dictionary<string, string> PicTypes = new Dictionary<string, string> { { "7173", ".gif" }, { "255216", ".jpg" }, { "13780", ".png" }, { "6677", ".bmp" } };

        /// <summary>
        /// 获取图片真实后缀
        /// </summary>
        public static string GetPicExt(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2)
            {
                return null;
            }
            var extBytes = bytes[0].ToString() + bytes[1].ToString();

            if (!PicTypes.ContainsKey(extBytes))
            {
                return null;
            }

            return PicTypes[extBytes];
        }
        /// <summary>
        /// 是否jpg文件
        /// </summary>
        public static bool IsJpgPic(byte[] bytes)
        {
            var ext = GetPicExt(bytes);

            return ".jpg" == ext;
        }
        /// <summary>
        /// 获取bytes
        /// </summary>
        /// <param name="base64String">base64string</param>
        /// <returns></returns>
        public static byte[] GetBytesFromBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;
            var tmpArr = base64String.Split(',');
            if (tmpArr.Length < 2)
                return null;
            try { return Convert.FromBase64String(tmpArr[1]); } catch { }

            return null;
        }

        #endregion

        #region 是否是网络图片
        private static bool IsHttp(string content)
        {
            return content.StartsWith("http://") || content.StartsWith("https://");
        }
        #endregion

        #region 特殊字符处理

        public static readonly Dictionary<string, string> SpecialChat = new Dictionary<string, string>() { { "#", "%23" } };

        public static string GetFilePath(string filePath)
        {
            foreach (var item in SpecialChat)
            {
                filePath = filePath.Replace(item.Key, item.Value);
            }
            return filePath;
        }
        #endregion
    }
}
