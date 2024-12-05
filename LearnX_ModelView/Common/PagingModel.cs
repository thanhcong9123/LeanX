using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Common
{
    public class PagingModel
    {
        public int TotalItems { get; private set; }

        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public PagingModel()
        {

        }
        public PagingModel(int totalItems, int page, int pageSize = 10)
        {
            //số trang đưa chia thành
            int totalPage = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            //đánh dấu trang hiện tại 
            int currentPage = page;
            //các trang hiện thị trong khoảng trang được đánh dấu
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;
            // nếu trang bắt đầu bé hơn hoặc bằng 0 thì khoảng cách số
            // trang từ trang đánh dấu đến trang cuối cùng sẽ được nới rộng
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            //ngược lại
            if (endPage > totalPage)
            {
                endPage = totalPage;
                //nếu trang cuối cùng bé hơn 10 tức là tổng số tang nỏ hơn 10 thì ta gán trang đầu là 1
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPage = totalPage;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}