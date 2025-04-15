using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace BookMGMT.Pages
{
    public class BookEditModel : PageModel
    {
        string connString = "Data Source=HER\\NEWINSTANCE;Initial Catalog=BookMgt;Integrated Security=True;TrustServerCertificate=True";
        public string message = "";

        public Book book = new Book();
        public void OnGet()
        {
            string isbn = Request.Query["isbn"];

            //retrieve existing informations
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT ISBN, TITLE, AUTHOR, SUMMARY FROM BOOK WHERE ISBN=@v_isbn";
                    using (SqlCommand cmd = new SqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@v_isbn", isbn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                book.Isbn = reader.GetString(0);
                                book.Title = reader.GetString(1);
                                book.Author = reader.GetString(2);
                                book.Summary = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnPost() 
        {

        }

        public void UpdateBook()
        {
            book.Isbn = Request.Form["isbn"];
            book.Title = Request.Form["Title"];
            book.Author = Request.Form["Author"];
            book.Summary = Request.Form["Summary"];

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE BOOK TITLE = v_title , AUTHOR = v_author, SUMMARY = v_summary) where ISBN = v_isbn", conn))
                    {
                        cmd.Parameters.AddWithValue("@v_isbn", book.Isbn);
                        cmd.Parameters.AddWithValue("@v_title", book.Title);
                        cmd.Parameters.AddWithValue("@v_author", book.Author);
                        cmd.Parameters.AddWithValue("@v_summary", book.Summary);

                        int rowsAffected = cmd.ExecuteNonQuery(); // execute and store rows affected
                        if (rowsAffected > 0)
                        {
                            message = "Book added successfully";
                            Response.Redirect("BookList");
                        }
                        else
                        {
                            message = "Book not added";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
