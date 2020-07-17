using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WP.Device.Plugins.Kernel.Enums.OcrOptionType;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class OcrProcessResponse
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }


        /// <summary>
        /// 放大级别,默认1.5倍放大
        /// </summary>
        public double ZoomLevel
        {
            set; get;
        }

        /// <summary>
        /// 腐蚀程度
        /// </summary>
        public double ErodeLevel
        {
            set; get;
        }

        /// <summary>
        /// 算法集合多个用逗号隔开
        /// </summary>
        public string Algorithm
        {
            set; get;
        }

        /// <summary>
        /// 二值化类型
        /// </summary>
        public int ThresholdType
        {
            set; get;
        }

        /// <summary>
        /// OCR 识别模式
        /// </summary>
        public OcrModeEnums OcrModeEnums { set; get; }
    }
}
