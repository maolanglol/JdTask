//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using Newtonsoft.Json;
//using Jlion.BrushClient.Application;

//using Jlion.BrushClient.Application.Model;
//namespace Jlion.BrushClient.Application
//{
//    public class MapperConfig
//    {

//        public static void Init()
//        {
//            #region input

//            Mapper.CreateMap<PointConfigModel, PointConfig>();
//            Mapper.CreateMap<PointItem, Size>()
//                .ForMember(r => r.Width, opt => opt.MapFrom(x => x.X))
//                .ForMember(r => r.Height, opt => opt.MapFrom(x => x.Y));
//            Mapper.CreateMap<OcrConfigModel, ScreenConfig>()
//                .ForMember(r => r.ZoomLevel, opt => opt.MapFrom(x => x.ImageProcess.ZoomLevel))
//                .ForMember(r => r.ErodeLevel, opt => opt.MapFrom(x => x.ImageProcess.ErodeLevel))
//                .ForMember(r => r.Algorithm, opt => opt.MapFrom(x => x.ImageProcess.Algorithm));

//            Mapper.CreateMap<VirSerialConfigModel, VirSerialPortConfig>();

//            Mapper.CreateMap<SerialConfigModel, SerialPortConfig>()
//                .ForMember(r => r.OutputPortName, opt => opt.MapFrom(x => x.PortName))
//                .ForMember(r => r.BaudRate, opt => opt.MapFrom(x => x.BaudRate));
//            #endregion

//            #region output
//            Mapper.CreateMap<PointConfig, PointConfigModel>();
//            Mapper.CreateMap<Size, PointItem>()
//                .ForMember(r => r.X, opt => opt.MapFrom(x => x.Width))
//                .ForMember(r => r.Y, opt => opt.MapFrom(x => x.Height));
//            Mapper.CreateMap<ScreenConfig, OcrConfigModel>();

//            Mapper.CreateMap<VirSerialPortConfig, VirSerialConfigModel>();

//            #endregion

//        }
//    }
//}
