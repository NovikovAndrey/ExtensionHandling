using System.Web;
using System.Linq;
namespace TestFileHandlerLibrary
{
    public class TestFileHandler : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            string filePath = context.Request.PhysicalPath;
            using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                System.Xml.Linq.XDocument xDocument = System.Xml.Linq.XDocument.Load(stream);
                var allUsers = from user in xDocument.Descendants("User")
                               select new
                               {
                                   FirstName = user.Element("FirstName").Value,
                                   LastName = user.Element("LastName").Value
                               };
                context.Response.Write("<html><body><table border='1'>");
                foreach (var user in allUsers)
                {
                    context.Response.Write("<tr>");
                    context.Response.Write("<td>"+user.FirstName+"</td>");
                    context.Response.Write("<td>"+user.LastName+"</td>");
                    context.Response.Write("</tr>");
                }
                context.Response.Write("</table></body></html>");
            }
        }
    }
}
