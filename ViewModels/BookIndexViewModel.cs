using System.Collections.Generic;
using bookmanagement.Models;

namespace bookmanagement.ViewModels
{
    public class BookIndexViewModel
    {
        public List<Book> Books { get; set; }
        public string SearchTerm { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}
