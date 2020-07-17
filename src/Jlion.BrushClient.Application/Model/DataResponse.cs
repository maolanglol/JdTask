using Jlion.BrushClient.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class DataResponse<T>
    {
        public T Data { set; get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 失败Code
        /// </summary>
        public ApiCodeEnums Code { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        public static DataResponse<T> AsSuccess(T Data, string message = "") => new DataResponse<T>() { Data = Data, Success = true, Code = ApiCodeEnums.SUCCESS, Msg = message };

        public static DataResponse<T> AsFail(ApiCodeEnums Code = ApiCodeEnums.SYSTEM_ERROR, string message = "") => new DataResponse<T>() { Code = Code, Msg = message };

    }
}
