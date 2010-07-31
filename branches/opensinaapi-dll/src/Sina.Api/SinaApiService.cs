using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Web;

namespace Sina.Api
{
    public class SinaApiService : oAuthSina//,ISinaApiService
    {
        //public string Token { get; set; }
        //public string TokenSecret { get; set; }
        //private oAuthSina oauth = new oAuthSina();

        public SinaApiService()
        {
            //default format
            Format = "xml";
        }

        public void CheckToken()
        {

        }

        //从新浪跳转回来，换取Access Token
        public bool oAuthWeb(string oauth_token, string oauth_verifier)
        {
            Verifier = oauth_verifier;
            AccessTokenGet(oauth_token);
            return true;
        }


        /// <summary>
        /// 这个是供桌面端应用调用的方法，需要用户提供用户名和密码
        /// </summary>
        /// <returns></returns>
        public bool oAuthDesktop(string userid, string passwd)
        {
            try
            {
                string authLink = AuthorizationGet();
                authLink += "&userId=" + userid + "&passwd=" + passwd + "&action=submit&oauth_callback=none";
                string html = WebRequest(Method.POST, authLink, null);
                string pin = ParseHtml(html);
                Verifier = pin;
                AccessTokenGet(Token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**********************************************************************************************
         *************************************下行数据获取*********************************************
         **********************************************************************************************
         **********************************************************************************************/

        /*最新公共微博*/
        public List<User> public_timeline()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/public_timeline." + Format;
                string response = oAuthWebRequest(Method.GET, url, String.Empty);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                List<User> users = new List<User>();
                XmlNodeList nodes = xmlDoc.GetElementsByTagName("user");
                for (int i = 0; i < nodes.Count; i++)
                {
                    User u = new User();
                    u.id = Convert.ToInt32(nodes[i].ChildNodes[0].InnerText);
                    u.followers_count = Convert.ToInt32(nodes[i].ChildNodes[11].InnerText);
                    u.friends_count = Convert.ToInt32(nodes[i].ChildNodes[12].InnerText);
                    u.following = Convert.ToBoolean(nodes[i].ChildNodes[16].InnerText);
                    users.Add(u);
                }
                return users;
            }
            catch
            { return null; }
        }
        /*最新关注人微博*/
        public string friend_timeline()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/friends_timeline." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch { return null; }
        }
        /*用户发表微薄列表*/
        public string user_timeline()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/user_timeline." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*最新n条@我的微博*/
        public string mentions()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/mentions." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*最新评论*/
        public string comments_timeline()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/comments_timeline." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*发出的评论*/
        public string comments_by_me()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/comments_by_me." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /* 单条评论列表*/
        public string comments(string id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/comments." + Format + "?id=" + id;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*批量获取一组微博的评论数及转发数*/
        public string counts(string ids)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/counts." + Format + "?ids=" + ids;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /**********************************************************************************************
         *************************************微博访问接口*********************************************
         **********************************************************************************************
         **********************************************************************************************/
        /*获取单条ID的微博信息*/
        public string statuses_show(string id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/show/" + id + "." + Format;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*获取单条ID的微博信息*/
        public string statuses_id(string id, string uid)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/" + uid + "/statuses/" + id;
                return oAuthWebRequest(Method.GET, url, String.Empty);
            }
            catch
            { return null; }
        }
        /*发布一条微博信息*/
        public string statuses_update(string status)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/update." + Format + "?";
                return oAuthWebRequest(Method.POST, url, "status=" + HttpUtility.UrlEncode(status));
            }
            catch
            { return null; }
        }
        /*上传图片并发布一条微博信息 */
        public string statuses_upload(string status, string pic)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/upload." + Format + "?";
                return oAuthWebRequest(Method.POST, url, "status=" + HttpUtility.UrlEncode(status) + "&pic=" + HttpUtility.UrlEncode(pic));
            }
            catch
            { return null; }
        }
        /*验证当前用户身份是否合法*/
        public User account_verify_credentials()
        {
            try
            {
                string url = "http://api.t.sina.com.cn/account/verify_credentials." + Format;
                string response = oAuthWebRequest(Method.GET, url, String.Empty);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                User user = new User();
                user.id = Convert.ToInt32(xmlDoc.GetElementsByTagName("id")[0].InnerText);
                user.screen_name = xmlDoc.GetElementsByTagName("screen_name")[0].InnerText;
                user.name = xmlDoc.GetElementsByTagName("name")[0].InnerText;
                user.province = xmlDoc.GetElementsByTagName("province")[0].InnerText;
                user.city = xmlDoc.GetElementsByTagName("city")[0].InnerText;
                user.location = xmlDoc.GetElementsByTagName("location")[0].InnerText;
                user.description = xmlDoc.GetElementsByTagName("description")[0].InnerText;
                user.url = xmlDoc.GetElementsByTagName("url")[0].InnerText;
                user.profile_image_url = xmlDoc.GetElementsByTagName("profile_image_url")[0].InnerText;
                user.domain = xmlDoc.GetElementsByTagName("domain")[0].InnerText;
                user.gender = xmlDoc.GetElementsByTagName("gender")[0].InnerText;
                user.statuses_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("statuses_count")[0].InnerText);
                user.friends_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("friends_count")[0].InnerText);
                user.followers_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("followers_count")[0].InnerText);
                return user;
            }
            catch
            { return null; }
        }

        /*关注某用户*/
        //领客康健网官方微博帐号ID:1679214941
        public string friendships_create(int user_id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/friendships/create." + Format + "?";
                return oAuthWebRequest(Method.POST, url, "user_id=" + user_id);
            }
            catch
            { return null; }
        }

        /*取消关注 */
        public string friendships_destroy(int user_id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/friendships/destroy." + Format + "?";
                return oAuthWebRequest(Method.POST, url, "user_id=" + user_id);
            }
            catch
            { return null; }
        }

        /*是否关注某用户 ,user_a关注user_b返回true*/
        public bool friendships_exists(int user_a, int user_b)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/friendships/exists." + Format + "?";
                string response = oAuthWebRequest(Method.POST, url, "user_a=" + user_a + "&user_b=" + user_b);
                if (response.ToLower().Contains("true"))
                { return true; }
                else
                { return false; }
            }
            catch
            { return false; }
        }

        /*获取当前用户关注对象列表及最新一条微博信息  */
        public List<User> statuses_friends(int count)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/statuses/friends." + Format + "?";
                string response = oAuthWebRequest(Method.GET, url, "cursor=1000&count=" + count);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                List<User> users = new List<User>();
                XmlNodeList nodes = xmlDoc.GetElementsByTagName("user");
                for (int i = 0; i < nodes.Count; i++)
                {
                    User u = new User();
                    u.id = Convert.ToInt32(nodes[i].ChildNodes[0].InnerText);
                    u.followers_count = Convert.ToInt32(nodes[i].ChildNodes[11].InnerText);
                    u.friends_count = Convert.ToInt32(nodes[i].ChildNodes[12].InnerText);
                    u.following = Convert.ToBoolean(nodes[i].ChildNodes[16].InnerText);
                    users.Add(u);
                }
                return users;
            }
            catch
            { return null; }
        }

        /*获取用户关注对象uid列表 */
        public List<int> friends_ids(int user_id, int count)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/friends/ids." + Format + "?";
                string response = oAuthWebRequest(Method.GET, url, "count=" + count);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                List<int> ids = new List<int>();
                XmlNodeList nodes = xmlDoc.GetElementsByTagName("id");
                for (int i = 0; i < nodes.Count; i++)
                {
                    ids.Add(Convert.ToInt32(nodes[i].InnerText));
                }
                return ids;
            }
            catch
            { return null; }
        }

        /*获取用户粉丝对象uid列表 */
        public List<int> followers_ids(int user_id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/followers/ids." + Format + "?";
                string response = oAuthWebRequest(Method.GET, url, "user_id=" + user_id);

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                List<int> ids = new List<int>();
                XmlNodeList nodes = xmlDoc.GetElementsByTagName("id");
                for (int i = 0; i < nodes.Count; i++)
                {
                    ids.Add(Convert.ToInt32(nodes[i].InnerText));
                }
                return ids;
            }
            catch
            { return null; }
        }

        //发送一条私信
        //成功后返回私信ID，供删除用
        public int direct_messages_new(int user_id, string text)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/direct_messages/new." + Format + "?";
                string response = oAuthWebRequest(Method.POST, url, "user_id=" + user_id + "&text=" + HttpUtility.UrlEncode(text));

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                int id = Convert.ToInt32(xmlDoc.GetElementsByTagName("id")[0].InnerText);
                return id;
            }
            catch
            { return 0; }
        }

        //删除一条私信
        public string direct_messages_destroy(int id)
        {
            try
            {
                string url = "http://api.t.sina.com.cn/direct_messages/destroy/" + id + "." + Format;
                return oAuthWebRequest(Method.POST, url, String.Empty);
            }
            catch
            { return null; }
        }
    }
}