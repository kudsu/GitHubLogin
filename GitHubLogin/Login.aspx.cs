using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GitHubLogin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = Request.QueryString["code"] == null || Request.QueryString["code"].ToString() == "" ? "" : Request.QueryString["code"].ToString();
                if (code != "")
                {
                    //第二步，获取token
                    string tokenJson = LoadURLString("https://github.com/login/oauth/access_token?client_id=xxxxxxxxxxxxxxxxxxxx&client_secret=xxxxxxxxxxxxxxxxxxxxxx&code=" + code + "&redirect_uri=http://www.kudsu.xyz/", "post");
                    JObject jo = (JObject)JsonConvert.DeserializeObject(tokenJson);
                    tokenJson = jo["access_token"].ToString();
                    //第三步，获取GitHub用户信息
                    string userJson = LoadURLString("https://api.github.com/user?access_token=" + tokenJson, "get");
                    ////把GitHub用户信息输出到页面上
                    Response.Write(userJson);
                }
            }
        }
        /// <summary>
        /// 请求url
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="GetPost">post、get</param>
        /// <returns></returns>
        private string LoadURLString(string url, string GetPost)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
            request1.Method = GetPost;
            request1.ContentType = "application/json";
            request1.Accept = "application/json";
            request1.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            request1.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";
            return new StreamReader(((HttpWebResponse)request1.GetResponse()).GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
    }
}