using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tools
{
    public class PageHandle<T>
    {
        T[] items = null;
        int maxPage = 0;
        int nowPage = 0;
        int pageSize = 0;
        /// <summary>
        /// 分页类构造
        /// </summary>
        /// <param name="itmes_">被分页的对象集合</param>
        /// <param name="pageSize">页大小</param>
        public PageHandle(T[] itmes_, int pageSize)
        {
            this.pageSize = pageSize;
            items = itmes_;
            maxPage = itmes_.Length / pageSize;
            if (itmes_.Length % pageSize != 0)
                maxPage++;
        }
        /// <summary>
        /// 获取第一页的对象
        /// </summary>
        /// <param name="isStop">是不是尾页</param>
        /// <returns>该页的对象</returns>
        public T[] getFirstPage(out bool isStop)
        {
            isStop = false;
            if (maxPage == 1)
                isStop = true;
            nowPage = 0;
            List<T> objs = new List<T>();
            for (int i = 0; i < pageSize && i < items.Length; i++)
                objs.Add(items[i]);
            return objs.ToArray();
        }
        /// <summary>
        /// 获取上一页的对象
        /// </summary>
        /// <param name="isStop">是否是首页</param>
        /// <returns></returns>
        public T[] getLastPage(out bool isStop)
        {
            isStop = false;
            nowPage--;
            if (nowPage < 0)
                nowPage = 0;
            if (nowPage == 0)
            {
                isStop = true;
                bool temp = false;
                return getFirstPage(out temp);
            }
            
            int start = nowPage * pageSize;
            int end = start + pageSize;
            if (end > items.Length)
                end = items.Length;
            List<T> objs = new List<T>();
            for (int i = start; i < end; i++)
                objs.Add(items[i]);
            return objs.ToArray();
        }
        /// <summary>
        /// 获取下一页的对象
        /// </summary>
        /// <param name="isStop">是否是尾页</param>
        /// <returns></returns>
        public T[] getNextPage(out bool isStop)
        {
            isStop = false;
            if (nowPage < maxPage - 1)
                nowPage++;
            if(nowPage == maxPage - 1)
                isStop = true;

            int start = nowPage * pageSize;
            int end = start + pageSize;
            if (end > items.Length)
                end = items.Length;
            List<T> objs = new List<T>();
            for (int i = start; i < end; i++)
                objs.Add(items[i]);
            return objs.ToArray();
        }
        /// <summary>
        /// 获取最大页码
        /// </summary>
        /// <returns></returns>
        public int getPageNum()
        {
            return maxPage;
        }
        /// <summary>
        /// 获取当前页码
        /// </summary>
        /// <returns></returns>
        public int getNowPage()
        {
            return nowPage + 1;
        }
    }
}
