using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailApi.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        // Defina suas credenciais fixas aqui
        private const string Username = "coti-informatica";
        private const string Password = "Coti2024!";

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            // Verificar a autenticação
            if (!AuthenticateRequest(Request))
            {
                return Unauthorized(new { Message = "Autenticação falhou. Credenciais inválidas." });
            }

            try
            {
                // Configuração do cliente SMTP para enviar e-mail
                using (SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587))
                {
                    client.Credentials = new NetworkCredential("sergiojavaarq@outlook.com", "@Admin12345");
                    client.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("sergiojavaarq@outlook.com"),
                        Subject = request.Subject,
                        Body = request.Body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(request.To);

                    // Enviar e-mail
                    client.Send(mailMessage);
                }

                return Ok(new { Message = "Email enviado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro ao enviar e-mail: {ex.Message}" });
            }
        }

        // Método para autenticar a requisição com Basic Authentication
        private bool AuthenticateRequest(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authHeader = request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                var username = decodedUsernamePassword.Split(':')[0];
                var password = decodedUsernamePassword.Split(':')[1];

                return username == Username && password == Password;
            }

            return false;
        }
    }

    // Classe para o modelo de request
    public class EmailRequest
    {
        public string Subject { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }
}
