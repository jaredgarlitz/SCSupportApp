using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCSupportApp.Controllers;

namespace SCSupportApp
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Page.IsPostBack)
            {
                string toEmail = email.Text;
                submitEmail.Click += new EventHandler(this.submitEmail_Click);
            }
        }
        protected void submitEmail_Click(object sender, EventArgs e)
        {
            string toEmail = email.Text;
            string worked = "Success";
            try
            {
                emailSend.sendMail(toEmail);
            }
            catch
            {
                throw new ApplicationException("Please enter a valid email");
            }
        }
    }
}