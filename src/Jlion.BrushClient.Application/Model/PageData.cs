using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Application.Model
{
	public class PageData<T>
	{

		public PageData(IEnumerable<T> items, int totalCount = 0)
		{
			Data = items;
			TotalCount = totalCount;
		}

		/// <summary>
		/// 信息实体
		/// </summary>
		public IEnumerable<T> Data { set; get; }

		/// <summary>
		/// 总数量
		/// </summary>
		public int TotalCount { set; get; }

		public static PageData<T> Get(IEnumerable<T> items, int totalCount = 0)
		{
			return new PageData<T>(items, totalCount);
		}
	}
}
