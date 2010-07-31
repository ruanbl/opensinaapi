using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sina.Api
{
    public class User
    {
        //<id>88888888</id> 
        //<screen_name>领客康健网</screen_name> 
        //<name>领客康健网</name> 
        //<province>50</province> 
        //<city>1000</city> 
        //<location>重庆</location> 
        //<description></description> 
        //<url>http://www.ilinkee.com</url> 
        //<profile_image_url></profile_image_url> 
        //<domain>ilinkee</domain> 
        //<gender>f</gender> 
        //<followers_count>46</followers_count> 
        //<friends_count>28</friends_count> 
        //<statuses_count>93</statuses_count> 
        //<favourites_count>0</favourites_count> 
        //<created_at>Fri Jan 08 00:00:00 +0800 2010</created_at> 
        //<following>false</following> 
        //<verified>false</verified> 
        //<allow_all_act_msg>false</allow_all_act_msg> 
        //<geo_enabled>false</geo_enabled> 
        

        //id: 用户UID
        //screen_name: 微博昵称
        //name: 友好显示名称，如Tim Yang(此特性暂不支持)
        //province:省份编码（参考省份编码表）
        //city: 城市编码（参考城市编码表）
        //location：地址
        //description: 个人描叙
        //url: 用户博客地址
        //profile_image_url: 自定义图像
        //domain: 用户个性化域名
        //gender: 性别,m--男，f--女,n--未知
        //followers_count: 粉丝数
        //friends_count: 关注数
        //statuses_count: 微博数
        //favourites_count: 收藏数
        //created_at: 创建时间
        //following: 是否已关注(此特性暂不支持)
        //verified: 加V标示

        public int id { get; set; }
        public string  screen_name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string profile_image_url { get; set; }
        public string domain { get; set; }
        public string gender { get; set; }
        public int followers_count { get; set; }
        public int friends_count { get; set; }
        public int statuses_count { get; set; }
        public int favourites_count { get; set; }
        public bool following { get; set; }
    }

    public class Status
    { }

    public class Comment
    { }
}
