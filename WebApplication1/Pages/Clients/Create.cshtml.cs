using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static WebApplication1.Pages.Clients.IndexModel;

namespace WebApplication1.Pages.Clients
{
    public class CreateModel : PageModel    
    {
        public ClientInfo clientinfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        { 
            clientinfo.name= Request.Form["name"];
            clientinfo.email = Request.Form["email"];
            clientinfo.phone = Request.Form["phone"];
            clientinfo.address = Request.Form["address"];

            if(clientinfo.name.Length == 0 || clientinfo.email.Length == 0 || 
               clientinfo.phone.Length == 0 || clientinfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            //save  the new client into  the database

            try
            {
                String connectionString = @"Data Source=.\sqlexpress;Initial Catalog=project;Integrated Security=True;Encrypt=False";
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients (name,email,phone,address) VALUES(@name, @email, @phone, @address);";

                    using(SqlCommand command= new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientinfo.name);
                        command.Parameters.AddWithValue("@email", clientinfo.email);
                        command.Parameters.AddWithValue("@phone", clientinfo.phone);
                        command.Parameters.AddWithValue("@address", clientinfo.address);

                        command.ExecuteNonQuery();
                    }
                    {
                            
                    }
                }

            }catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientinfo.name = "";
            clientinfo.email = "";
            clientinfo.phone = "";
            clientinfo.address = "";

            successMessage = "New Client Added Successfully";

            Response.Redirect("/Clients/Index");
        }
    }
}
