using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace BookMGMT.Pages
{
    public class BookAddModel : PageModel
    {
        string connString = "Data Source=HER\\NEWINSTANCE;Initial Catalog=BookMgt;Integrated Security=True;TrustServerCertificate=True";
        public string message = "";
        public Book myBook = new Book();

        public void OnGet()
        {
        }

        public void OnPost()
        {
            addBook();
        }

        public void addBook()
        {
            myBook.Isbn = Request.Form["isbn"];
            myBook.Title = Request.Form["Title"];
            myBook.Author = Request.Form["Author"];
            myBook.Summary = Request.Form["Summary"];

            try
            {
                using(SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO BOOK (ISBN, TITLE, AUTHOR, SUMMARY) VALUES(@v_isbn, @v_title, @v_author, @v_summary)", conn))
                    {
                        cmd.Parameters.AddWithValue("@v_isbn", myBook.Isbn);
                        cmd.Parameters.AddWithValue("@v_title", myBook.Title);
                        cmd.Parameters.AddWithValue("@v_author", myBook.Author);
                        cmd.Parameters.AddWithValue("@v_summary", myBook.Summary);

                        int rowsAffected = cmd.ExecuteNonQuery(); // execute and store rows affected
                        if (rowsAffected > 0)
                        {
                            message = "Book added successfully";
                            Response.Redirect("BookList");
                        } else
                        {
                            message = "Book not added";
                        }
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Book
    {
        public string? Isbn { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Summary { get; set; }
    }
}
