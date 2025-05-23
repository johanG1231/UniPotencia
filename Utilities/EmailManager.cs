using System.Net.Mail;
using System.Net;

namespace Backend_PotenciaPC.Utilities
{
    public class EmailManager
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "potenciapc2025@gmail.com";
        private string Password = "xdrrgspjxuyxbobt";
        private bool EnabledSSL = true;
        public EmailManager()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            email = new MailMessage(User, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);
            email.Dispose();
        }
    }
}
