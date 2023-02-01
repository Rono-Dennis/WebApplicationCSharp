using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static WebApplication1.Pages.Clients.IndexModel;

namespace WebApplication1.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientinfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id  = Request.Query["id"];

            try
            {
                String connectionString = @"Data Source=.\sqlexpress;Initial Catalog=project;Integrated Security=True;Encrypt=False";
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //String sql = "UPDATE clients SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                clientinfo.id = "" + reader.GetInt32(0);
                                clientinfo.name = reader.GetString(1);
                                clientinfo.email = reader.GetString(2);
                                clientinfo.phone = reader.GetString(3);
                                clientinfo.address = reader.GetString(4);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        public void OnPost()
        {
            clientinfo.id = Request.Form["id"];
            clientinfo.name = Request.Form["name"];
            clientinfo.email = Request.Form["email"];
            clientinfo.phone = Request.Form["phone"];
            clientinfo.address = Request.Form["address"];

            if (clientinfo.id.Length == 0 || clientinfo.name.Length == 0 ||
                clientinfo.email.Length == 0 || clientinfo.phone.Length == 0 ||
                clientinfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            } 
            else
            {
                try
                {
                    String connectionString = @"Data Source=.\sqlexpress;Initial Catalog=project;Integrated Security=True;Encrypt=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "UPDATE clients SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", clientinfo.name);
                            command.Parameters.AddWithValue("@email", clientinfo.email);
                            command.Parameters.AddWithValue("@phone", clientinfo.phone);
                            command.Parameters.AddWithValue("@address", clientinfo.address);
                            command.Parameters.AddWithValue("@id", clientinfo.id);

                            command.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }

                Response.Redirect("/Clients/Index");
            }
        }
    }
}
