using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sina.Api;

namespace Sina.SampleApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SinaApiService api = new SinaApiService();

                //Session["oauth_token"] = "f0b815f8578b3c7495d4ed0fbxe2df09";
                //Session["oauth_token_secret"] = "07892fx58c9c00a3f730ec52a3bf4ea3";

                if (Request["oauth_verifier"] != null)
                {
                    api.Token = Session["oauth_token"].ToString();
                    api.TokenSecret = Session["oauth_token_secret"].ToString();
                    api.oAuthWeb(Request["oauth_token"].ToString(), Request["oauth_verifier"].ToString());
                    Session["oauth_token"] = api.Token;
                    Session["oauth_token_secret"] = api.TokenSecret;
                    Response.Redirect("/");
                }
                else if (Session["oauth_token"] != null)
                {
                    api.Token = Session["oauth_token"].ToString();
                    api.TokenSecret = Session["oauth_token_secret"].ToString();

                    
                    //string y = api.user_timeline();
                    Api.User u = api.account_verify_credentials();
                    if (u != null)
                    { Label1.Text = u.name; }

                    //api.statuses_update("test ~~");

                    //string path = Server.MapPath("~/1.jpg");
                    //api.statuses_upload("美女哦！", path);

                    //api.followers_ids(1752237874);
                    
                }
            }         
        }

        protected void BtnSina_Click(object sender, EventArgs e)
        {
            SinaApiService api = new SinaApiService();
            string url = api.AuthorizationGet();
            //保存Token和TokenSecret供下一步调用接口用
            Session["oauth_token"] = api.Token;
            Session["oauth_token_secret"] = api.TokenSecret;
            Response.Redirect(url + "&oauth_callback=http://localhost:12345/");
        }
    }
}
