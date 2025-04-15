using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace BookMGMT.Pages
{
    public class BookListModel : PageModel
    {

        string connString = "Data Source=HER\\NEWINSTANCE;Initial Catalog=BookMgt;Integrated Security=True;TrustServerCertificate=True";
        public string message = "";
        public List<Book> myBookList = new List<Book>();
        public void OnGet()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ISBN, TITLE, AUTHOR, SUMMARY FROM BOOK", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = new Book();

                                book.Isbn = reader.GetString(0);
                                book.Title = reader.GetString(1);
                                book.Author = reader.GetString(2);
                                book.Summary = reader.GetString(3);

                                myBookList.Add(book);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
            }
        }
    }
}
