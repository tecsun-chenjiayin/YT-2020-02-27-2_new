using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using YTH.Controls;
using YTH.Functions;
using YTH.Functions.ReadCarAndSQCode;

namespace YTH.BuKa
{
    class WeiWang
    {
        public static List<Dictionary<string, string>> getZKData(string persionid,string name,out string error)
        {
            error = null;
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                //string result = ws.allDsjk(xml);
                Log.AddLog("WeiWang", "开始获取制卡数据");
                string result = ws.getData(Config.dic("userid"), Config.dic("password"), persionid,name, Config.dic("cityCode"));
               
                Log.AddLog("WeiWang", "返回：" + result);
                List<Dictionary<string, string>> ret = AnalyzeXML(result);
                return ret;
            }
            catch(Exception e)
            {
                error = "获取制卡数据异常：" + e.ToString();
                return null;
            }
        }
        public static string lossCard(Functions.AnalyzeXML xml)
        {
            string error = null;
            Log.AddLog("WeiWang", "lossCard:");
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.setCard(
                    Config.dic("userid"), 
                    Config.dic("password"), 
                    "02",
                    xml["ROW"][0]["社保卡号"][0].ToString(),
                    xml["ROW"][0]["社会保障号"][0].ToString(),
                    xml["ROW"][0]["姓名"][0].ToString(), 
                    xml["ROW"][0]["服务银行"][0].ToString().Split('_')[0], 
                    "",//xml["ROW"][0]["AAE010"][0].ToString(), 
                    "",//xml["ROW"][0]["AAE010B"][0].ToString(), 
                    Config.dic("cityCode"));
                Log.AddLog("WeiWang", "lossCard返回：" + result);
                if (result.IndexOf("00") == 0 || result.IndexOf("01") == 0)
                    return null;
                else if (result.IndexOf("02") == 0)
                    return result + "-" + "请到到银行同步办理金融帐户的相应业务";
                else
                    return result;
            }
            catch (Exception e)
            {
                error = "挂失异常：" + e.ToString();
                return null;
            }
        }
        public static Functions.AnalyzeXML getSSSE(string persionid, string name,out string error)
        {
            error = null;      
            string xml = string.Format("<操作*>网上社保卡查询</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><所属城市>{2}</所属城市><社会保障号*>{3}</社会保障号*>", 
                Config.dic("userid"), 
                Config.dic("password"),
                Config.dic("cityCode"),
                persionid);
            Log.AddLog("WeiWang", "getSSSE 入参：" + xml);
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.allDsjk(xml);
                Log.AddLog("WeiWang", "返回：" + result);
                Functions.AnalyzeXML xml2 = new AnalyzeXML(result);
                if (xml2["ERR"][0].ToString() == "00")
                    return xml2;
                else
                {
                    error = xml2["ERR"][0].ToString();
                    return null;
                }
            }
            catch (Exception e)
            {
                error = "获取社保数据异常：" + e.ToString();
                return null;
            }
        }
        public static bool allDsjk(string persionid, string name, out string error)
        {
            error = null;
            string xml = string.Format("<操作*>一人一卡查询</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号*>{2}</社会保障号*>",
                Config.dic("userid"),
                Config.dic("password"),  
                persionid);
            Log.AddLog("WeiWang", "allDsjk 入参：" + xml);
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.allDsjk(xml);
                Log.AddLog("WeiWang", "返回：" + result);
                Functions.AnalyzeXML xml2 = new AnalyzeXML(result);
                if (xml2["ERR"][0].ToString() == "00")
                    return true;
                else
                { 
                    error = xml2["ERR"][0].ToString();
                    Log.AddLog("WeiWang", "allDsjk 出参：" + error);
                    return false;
                }
            }
            catch (Exception e)
            {
                error = "获取一人一卡查询异常：" + e.ToString();
                Log.AddLog("WeiWang", "allDsjk 出参：" + error);
                return false;
            }
        }

        public static string getSSSE2(string persionid, string name, out string error)
        {
            error = null;
            string xml = string.Format("<操作*>网上社保卡查询</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><所属城市>{2}</所属城市><社会保障号*>{3}</社会保障号*>",
                Config.dic("userid"),
                Config.dic("password"),
                Config.dic("cityCode"),
                persionid);
            Log.AddLog("WeiWang", "getSSSE 入参：" + xml);
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.allDsjk(xml);
                Log.AddLog("WeiWang", "返回：" + result);
                Functions.AnalyzeXML xml2 = new AnalyzeXML(result);
                if (xml2["ERR"][0].ToString() == "00")
                    return result;
                else
                {
                    error = xml2["ERR"][0].ToString();
                    return null;
                }
            }
            catch (Exception e)
            {
                error = "获取社保数据异常：" + e.ToString();
                return null;
            }
        }
        public static string getAC01(string persionid, string name)
        {
            WebReference.CardServiceService ws = new WebReference.CardServiceService();
            ws.Url = Config.dic("WIP");
            string result = ws.getAC01(Config.dic("userid"), Config.dic("password"), persionid, name, Config.dic("cityCode"));
            return result;
        }

        public static List<Dictionary<string, string>> getAZ03(string persionid, string name, out string error)
        {
            error = null;
            const string log = "制卡进度查询";
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
            Log.AddLog(log, "开始获取制卡进度查询");
            ws.Url = Config.dic("WIP");
            string result = ws.getAZ03(Config.dic("userid"), Config.dic("password"), persionid, name, Config.dic("cityCode"));
            Log.AddLog("WeiWang", "返回：" + result);
            List<Dictionary<string, string>> ret = AnalyzeXML(result);
                return ret;
            }
            catch (Exception e)
            {
                error = "获取制卡进度查询：" + e.ToString();
                return null;
            }
          
        }

        public static string backTOSKG(string keyID, string sfzh, string xm, string hzd)
        {
            WebReferenceSKG.JszkServiceImplService wj = new WebReferenceSKG.JszkServiceImplService();
            //Key  ：认证id
            //sfzh ：身份证号
            //xm   : 姓名
            //hzd  ：相片回执号码（补换卡或无需使用地市照片的请返回 - 1 ）    
            Log.AddLog("WeiWang", "backTOSKG入参:" + sfzh+" "+xm);
            string result = wj.insertData(keyID, sfzh, xm, hzd);

            Log.AddLog("WeiWang", "backTOSKG返回:" + result);
            return result;
        }


        public static string applyDataBuKa(Functions.AnalyzeXML xml, string cardStyle)
        {
            try
            {
                string xml_in =
@"<用户名*>" + Config.dic("userid") + @"</用户名*>" +
@"<密码*>" + Config.dic("password") + @"</密码*>" +
@"<数据归属机构*>" + Config.dic("area") + @"</数据归属机构*>" +
@"<申领表编码></申领表编码>" +
@"<卡类别*>"+ cardStyle + "</卡类别*>" +
@"<社会保障号*>" + xml["ROW"][0]["社会保障号"][0].ToString() + @"</社会保障号*>" +
@"<姓名*>" + xml["ROW"][0]["姓名"][0].ToString() + @"</姓名*>" +
@"<生僻字描述></生僻字描述>" +
@"<国家地区*>CHN</国家地区*>" +
@"<证件类型*>01</证件类型*> " +
@"<证件号码*>" + ReadIDCar.persionid + @"</证件号码*>" +
@"<证件有效期*>" + ReadIDCar.endDate + @"</证件有效期*>" +
@"<性别*>" + ReadIDCar.sexCode + @"</性别*>" +
@"<民族*>" + xml["ROW"][0]["民族"][0].ToString().Split('_')[0] + @"</民族*>" +
@"<出生日期*>" + ReadIDCar.birthday + @"</出生日期*>" +
@"<人员状态>" + xml["ROW"][0]["人员状态"][0].ToString().Split('_')[0] + @"</人员状态>" +
@"<户口区县>" + xml["ROW"][0]["户口区县"][0].ToString().Split('_')[0] + @"</户口区县>" +
@"<户口性质>" + xml["ROW"][0]["户口性质"][0].ToString().Split('_')[0] + @"</户口性质>" +
@"<户口地址>" + xml["ROW"][0]["户口地址"][0].ToString() + @"</户口地址>" +
@"<联系手机*>" + InputPhoneNum.phone + @"</联系手机*>" +
@"<联系电话>" + xml["ROW"][0]["联系电话"][0].ToString() + @"</联系电话>" +
@"<通讯地址>" + xml["ROW"][0]["通讯地址"][0].ToString() + @"</通讯地址>" +
@"<邮政编码>" + xml["ROW"][0]["邮政编码"][0].ToString() + @"</邮政编码>" +
@"<电子邮箱>" + xml["ROW"][0]["电子邮箱"][0].ToString() + @"</电子邮箱>" +
@"<个人编号>" + xml["ROW"][0]["个人编号"][0].ToString() + @"</个人编号>" +
@"<单位编号>" + xml["ROW"][0]["单位编号"][0].ToString() + @"</单位编号> " +
@"<单位名称>" + xml["ROW"][0]["单位名称"][0].ToString() + @"</单位名称>" +
@"<职业工种>" + xml["ROW"][0]["职业工种"][0].ToString().Split('_')[0] + @"</职业工种>" +
@"<监护人证件类型></监护人证件类型>" +
@"<监护人证件号码></监护人证件号码>" +
@"<监护人姓名></监护人姓名>" +
@"<服务银行*>" + Config.dic("bankCode") + @"</服务银行*>" +
@"<收表网点>" + Config.dic("wd2") + @"</收表网点> " +
@"<申请类型*>5</申请类型*>" +
@"<补换原因>遗失补卡</补换原因>" +
@"<社保卡号></社保卡号>" +
@"<收件地址></收件地址>" +
@"<即时制卡>即时制卡/空</即时制卡>" +
@"<证件图片1></证件图片1>" +
@"<证件图片2></证件图片2>" +
@"<相片*>" + xml["ROW"][0]["相片"][0].ToString() + @"</相片*>";
                Log.AddLog("WeiWang", "applyDataBuKa入参：" + xml_in);
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.insert(xml_in);
                Log.AddLog("WeiWang", "applyDataBuKa返回：" + result);
                if (result == "OK")
                    return null;
                else
                    return result;
            }
            catch (Exception e)
            {
                return "申请社保卡异常：" + e.ToString();
            }
            //insert
        }
        public static string applyDataXinBanKa(string cardStyle, string areaCode)
        {
            try
            {

                string AreaCode = areaCode;
                if (Config.dic("IsUseTestAreaCode") == "1")
                    AreaCode = Config.dic("AreaCode");
                string xml_in =
@"<用户名*>" + Config.dic("userid") + @"</用户名*>" +
@"<密码*>" + Config.dic("password") + @"</密码*>" +
@"<数据归属机构*>" + AreaCode + @"</数据归属机构*>" + //@"<数据归属机构*>" + areaCode + @"</数据归属机构*>" +   //城市代码+两位数的编号
@"<申领表编码></申领表编码>" +
@"<卡类别*>"+ cardStyle + "</卡类别*>" +
@"<社会保障号*>" + ReadIDCar.persionid + @"</社会保障号*>" +
@"<姓名*>" + ReadIDCar.name + @"</姓名*>" +
@"<生僻字描述></生僻字描述>" +
@"<国家地区*>CHN</国家地区*>" +
@"<证件类型*>01</证件类型*> " +
@"<证件号码*>" + ReadIDCar.persionid + @"</证件号码*>" +
@"<证件有效期*>" + ReadIDCar.endDate + @"</证件有效期*>" +
@"<性别*>" + ReadIDCar.sexCode + @"</性别*>" +
@"<民族*>" + ReadIDCar.nationCode + @"</民族*>" +
@"<出生日期*>" + ReadIDCar.birthday + @"</出生日期*>" +
@"<人员状态></人员状态>" +
@"<户口区县></户口区县>" +
@"<户口性质></户口性质>" +
@"<户口地址>" + ReadIDCar.address + @"</户口地址>" +
@"<联系手机*>" + InputPhoneNum.phone + @"</联系手机*>" +
@"<联系电话></联系电话>" +
@"<通讯地址></通讯地址>" +
@"<邮政编码></邮政编码>" +
@"<电子邮箱></电子邮箱>" +
@"<个人编号></个人编号>" +
@"<单位编号></单位编号> " +
@"<单位名称></单位名称>" +
@"<职业工种></职业工种>" +
@"<监护人证件类型></监护人证件类型>" +
@"<监护人证件号码></监护人证件号码>" +
@"<监护人姓名></监护人姓名>" +
@"<服务银行*>" + Config.dic("bankCode") + @"</服务银行*>" +
@"<收表网点></收表网点> " +
@"<申请类型*>1</申请类型*>" +
@"<补换原因></补换原因>" +
@"<社保卡号></社保卡号>" +
@"<收件地址></收件地址>" +
@"<即时制卡>即时制卡/空</即时制卡>" +
@"<证件图片1></证件图片1>" +
@"<证件图片2></证件图片2>" +
@"<相片*>" + ReadIDCar.base64 + @"</相片*>";
                Log.AddLog("WeiWang", "入参：" + xml_in);
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                string result = ws.insert(xml_in);
                Log.AddLog("WeiWang", "返回：" + result);
                if (result == "OK")
                    return null;
                else
                    return result;
            }
            catch (Exception e)
            {
                return "申请社保卡异常：" + e.ToString();
            }
            //insert
        }
        public static string setStart(string ssid)
        {
            string error = null;
            string result = null;
            try
            {
                WebReference.CardServiceService ws = new WebReference.CardServiceService();
                ws.Url = Config.dic("WIP");
                result = ws.setStart(Config.dic("userid"), Config.dic("password"), ssid, ReadIDCar.persionid, ReadIDCar.name, Config.dic("cityCode"));
                Log.AddLog("WeiWang", "返回：" + result);
                if (result.IndexOf("00") == 0 || result.IndexOf("01") == 0)
                    error = null;
                else if (result.IndexOf("02") == 0)
                    error = "社保卡激活成功，但银行协同失败，请到银行同步办理金融帐户的相应业务";
                else
                    error = "社保卡激活失败：" + result;
            }
            catch (Exception e)
            {
                error = "卡激活异常：" + e.ToString();
            }
            finally
            {
                Log.AddLog("领卡", ssid);
                Log.AddLog("领卡", result);
                if(error != null)
                 Log.AddLog("领卡", error);
            }
            return error;
        }
        public static string iWrite(out string error)
        {
            error = null;
            string ip = Config.dic("WIP");
            string user = Config.dic("userid");
            string cityCode = Config.dic("cityCode");
            string password = Config.dic("password");

            //清远
            string name = Config.dic("pName");
            string pPersion = Config.dic("pPersionID");
            string pDate = Config.dic("pDate");
            string pPicture = Config.dic("pPicture");
            string pSsse = Config.dic("pSsse");
            string prts = string.Format("{0},{1},{2},{3},{4}", name, pPersion, pDate,"", pPicture);

            //肇庆
            //string name = Config.dic("pName");
            //string pPersion = Config.dic("pPersionID");
            //string pDate = Config.dic("pDate");
            //string pPicture = Config.dic("pPicture");
            //string pSsse = Config.dic("pSsse");
            //string prts = string.Format("{0},{1},{2},{3},{4}", name, pPersion, pDate, "", pPicture);
            NHMakeCard.MAKECARD makecar = new NHMakeCard.MAKECARD();
            //prts = "宋体|8.5|320|180,宋体|8.5|320|230,宋体|8.5|320|280,宋体|8.5|320|330,46|127|234|300";
            
            string result = makecar.iWrite(ip, user, password, "", cityCode, ReadIDCar.persionid, prts);
            Log.AddLog("WeiWang", "result:" + result);
            if (result.IndexOf("0,") == 0)
            {
                return result;
            }
            else
            {
                //if (ip == null)
                //    return "ip null";
                //if (user == null)
                //    return "user null";
                //if (password == null)
                //    return "password null";
                //if (cityCode == null)
                //    return "cityCode null";
                //if (ReadIDCar.persionid == null)
                //    return "persionid null";
                //if (prts == null)
                //    return "prts null";
                //return result + "----" + ip +"----"+ user + "----" + password + "----" + "" + "----" + cityCode + "----" + ReadIDCar.persionid + "----" + prts;
                error = result;
                return null;
            }
        }
        /*
         7.15	申办社保卡
insert(String xml)
申办社保卡接口，接收入来自于数据采集、网上服务、服务银行、业务系统等的申办社保卡数据，申请类型上区分1个人申请、2网上申请、3批量申请、5遗失补卡、6普通换卡、7质保换卡。参数xml为申请信息串，申请成功返回“OK”，申请失败时返回出错信息。
xml申请信息串内容如下：
<用户名*></用户名*>
<密码*></密码*>
<数据归属机构*>44990000</数据归属机构*>
<申领表编码>001</申领表编码>
<卡类别*>1</卡类别*>
<社会保障号*>111111198101011110</社会保障号*>
<姓名*>姓名1</姓名*>
<生僻字描述></生僻字描述>
<国家地区*>CHN</国家地区*>
<证件类型*>01</证件类型*>
<证件号码*>111111198101011110</证件号码*>
<证件有效期*>20300101</证件有效期*>
<性别*>1</性别*>
<民族*>01</民族*>
<出生日期*>19800101</出生日期*>
<人员状态>1</人员状态>
<户口区县>440200</户口区县>
<户口性质>11</户口性质>
<户口地址>户口地址1</户口地址>
<联系手机*>13945678901</联系手机*>
<联系电话>电话1</联系电话>
<通讯地址>通讯地址1</通讯地址>
<邮政编码>123456</邮政编码>
<电子邮箱>1@12333.cn</电子邮箱>
<个人编号>个人号1</个人编号>
<单位编号>单位编号1</单位编号>
<单位名称>单位名称1</单位名称>
<职业工种></职业工种>
<监护人证件类型>01</监护人证件类型>
<监护人证件号码>监护人证号1</监护人证件号码>
<监护人姓名>监护人姓名1</监护人姓名>
<服务银行*>95566</服务银行*>
<收表网点>955660001</收表网点>
<申请类型*>3</申请类型*>
<补换原因></补换原因>
<社保卡号></社保卡号>
<收件地址></收件地址>
<即时制卡>即时制卡/空</即时制卡>
<证件图片1></证件图片1>
<证件图片2></证件图片2>
<相片*></相片*>

*/


        //===============old=====================
        public static string iWrite(string aac002, string prts)
        {
            string ip = Config.dic("WIP");
            string user = Config.dic("userid");
            string cityCode = Config.dic("cityCode");
            string password = Config.dic("password");
            Log.AddLog("写卡", "IP:" + ip);
            Log.AddLog("写卡", "user:" + user);
            Log.AddLog("写卡", "citiCode:" + cityCode);
            Log.AddLog("写卡", "社会保障号:" + aac002);
            Log.AddLog("写卡", "打印参数:" + prts);
            //string result = iWrite(ip, user, Config.dic("password"), "", cityCode, aac002, prts);
            NHMakeCard.MAKECARD makecar = new NHMakeCard.MAKECARD();
            //prts = "宋体|8.5|320|180,宋体|8.5|320|230,宋体|8.5|320|280,宋体|8.5|320|330,46|127|234|300";
            string result = makecar.iWrite(ip, user, password, "", cityCode, aac002, prts);
            Log.AddLog("写卡", "result:" + result);
            return result;
        }
        //             0        1      2    3       4
        //persionDatas:卡管地址\用户名\密码\城市代码\身份证
        public static string iWrite(string[] persionDatas, string prts)
        {
            string ip = persionDatas[0];// Config.dic("WIP");
            string user = persionDatas[1];//Config.dic("userid");
            string cityCode = persionDatas[3];// Config.dic("cityCode");
            string password = persionDatas[2];//Config.dic("password");
            Log.AddLog("制卡", "IP:" + ip);
            Log.AddLog("制卡", "user:" + user);
            Log.AddLog("制卡", "citiCode:" + cityCode);
            Log.AddLog("制卡", "社会保障号:" + persionDatas[4]);
            Log.AddLog("制卡", "打印参数:" + prts);
            //string result = iWrite(ip, user, Config.dic("password"), "", cityCode, aac002, prts);
            NHMakeCard.MAKECARD makecar = new NHMakeCard.MAKECARD();
            //prts = "宋体|8.5|320|180,宋体|8.5|320|230,宋体|8.5|320|280,宋体|8.5|320|330,46|127|234|300";
            string result = makecar.iWrite(ip, user, password, "", cityCode, persionDatas[4], prts);
      
            Log.AddLog("制卡", "result:" + result);
            return result;
        }
        public static List<Dictionary<string, string>> AnalyzeXML(string src)
        {
            int index = 0;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<Dictionary<string, string>> dec = new List<Dictionary<string, string>>();
            while (index < src.Length)
            {
                if (src[index] == '<')
                {
                    StringBuilder name = new StringBuilder();
                    StringBuilder value = new StringBuilder();
                    index++;
                    while (src[index] != '>')
                        name.Append(src[index++]);
                    index++;
                    while (src[index] != '<')
                        value.Append(src[index++]);
                    index++;
                    string Name = name.ToString();
                    string Value = value.ToString();
                    dic.Add(Name, Value);
                }
                else
                {
                    if (src[index] == '\n')
                    {
                        dec.Add(dic);
                        dic = new Dictionary<string, string>();
                    }
                    index++;
                }
            }
            if(dec.Count == 0)
                dec.Add(dic);
            return dec;
        }
    }
}
