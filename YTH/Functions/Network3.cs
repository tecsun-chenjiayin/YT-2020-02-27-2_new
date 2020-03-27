using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using YTH.Functions;

namespace YTH.NHNet
{
    class Network3
    {
        const string logName = "Net3";
        const string channelcode = "SelfService";//CardDispenser    SelfService
        static string channelcodeMD5 = null;
        static string token = null;




        //接口测试
        public static void test()
        {
            //自检
            {
                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
checkState	String	是	自检状态 1通过，0不通过
cardBox	String	是	卡盒
wheelDisc	String	是	轮盘
filpMachine	String	是	翻转机
eleCar	String	是	电动小车
icReader	String	是	IC读卡器
cardReader	String	是	二代证读卡器
a4printer	String	是	A4打印机
voucherPrinter	String	是	凭条打印机
camera	String	是	摄像头
qrCode	String	是	二维码扫描仪
mj.add("","", DataStyle.STR);
       */
                MakeJson mj = new MakeJson();
                mj.add("checkState", "1", DataStyle.STR);
                mj.add("cardBox", "正常", DataStyle.STR);
                mj.add("wheelDisc", "正常", DataStyle.STR);
                mj.add("filpMachine", "正常", DataStyle.STR);
                mj.add("eleCar", "正常", DataStyle.STR);
                mj.add("icReader", "正常", DataStyle.STR);
                mj.add("cardReader", "正常", DataStyle.STR);
                mj.add("a4printer", "正常", DataStyle.STR);
                mj.add("voucherPrinter", "正常", DataStyle.STR);
                mj.add("camera", "正常", DataStyle.STR);
                mj.add("qrCode", "正常", DataStyle.STR);

                string error = null;
                //AnalyzeJson aj = getJson(mj, "DevCheck", out error);

            }
            //保存色带信息
            {
                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
ribState	String	是	色带使用状态 2余量充足(可制卡量>30张)、1即将耗尽(<=30张)、 0已耗尽(色带剩余可制卡量<=3）

                 */
                MakeJson mj = new MakeJson();
                mj.add("ribState", "2", DataStyle.STR);
                string error = null;
                //AnalyzeJson aj = getJson(mj, "saveDevRibbon", out error);
            }
            //保存凭条打印机信息
            {
                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
priState	String	是	打印机使用状态 1正常、0缺纸
                 */
                MakeJson mj = new MakeJson();
                mj.add("priState", "1", DataStyle.STR);
                string error = null;
                //AnalyzeJson aj = getJson(mj, "saveDevPrinter", out error);
            }
            //保存设备在线信息saveDevOnline
            {
                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
onlineState	String	是	状态1在线，0不在线
                 */
                MakeJson mj = new MakeJson();
                mj.add("onlineState", "1", DataStyle.STR);
                string error = null;
                //AnalyzeJson aj = getJson(mj, "saveDevOnline", out error);
            }
            //设备登录deviceLogin + 预制卡入库管理putYZCardInStock
            {
                /*
deviceid	String	是	设备编号
deviceIP	String	否	设备IP地址
                 */
                MakeJson mj = new MakeJson();
                //mj.add("deviceid", GetMacAddress(), DataStyle.STR);
                string error = null;
                tools.AnalyzeJson aj = getJson(mj, "deviceLogin", out error);
                /*
返回字段
1	deviceId	设备编号
2	address	设备所在地址
3	status	状态 Y可用,S删除,T停用
4	deviceName	设备名称
5	areaCode	区域编码
6	branch	网点信息
7	deviceIp	设备ip
8	deviceMac	设备mac
9	orgId	网点id
10	orgCode	网点编码
11	devSeq	设备序列号

                 */


                /*
channelcode	String	是	渠道编码
allNum	int	是	卡总数
orgId	String	是	网点id（2.5接口返回的orgId）
operator	String	是	操作人员（姓名）
              */
                MakeJson mj1 = new MakeJson();
                mj1.add("allNum", 150, DataStyle.INT);
                mj1.add("orgId", aj["data"]["orgId"].ToString(), DataStyle.STR);
                mj1.add("operator", "测试姓名", DataStyle.STR);

                string error1 = null;
                //AnalyzeJson aj1 = getJson(mj1, "putYZCardInStock", out error1);
            }
            //制卡->回盘->查询卡信息->是否可以领卡->领卡   回盘getBackData + 上传领卡信息uploadFKRecord
            if(true)
            {
                MakeJson mj = new MakeJson();
                string error = null;
                tools.AnalyzeJson aj = getJson(mj, "deviceLogin", out error);

                /*
                 channelcode	String	是	渠道编码
orgCode	String	是	网点编码(接口2.5返回orgCode)
devSeq	String	是	设备序号(接口2.5返回devSeq)
atr	String	是	ATR
ksbm	String	是	卡识别码
sfzh	String	是	身份证
xm	String	是	姓名
status	String	是	制卡状态 1:成功 2:失败
orgId	String	是	网点id（2.5接口返回的orgId）
kh	String	是	社保卡号
yhkh	String	是	银行卡号
backStatus	String	是	回盘状态 1:回盘成功，2:回盘失败
backmsg	String	否	回盘信息
slotno	String	否	槽号
batchno	String	否	批次号
description	String	否	制卡成功或失败原因描述
ksdm	String	否	卡商代码
gfbb	String	否	规范版本
jgbm	String	否	机构编码
yhbm	String	否	银行编码
kyxq	String	否	卡有效期
sex	String	否	性别
nation	String	否	民族

                    "channelcode": "CardDispenser", 
    "atr": "3B6D0000008154443686605328A8888888", 
    "ksbm": "3B6D0011111111111111113388888888", 
    "sfzh": "413024197410137053", 
    "xm": "张四", 
    "status": "1", 
    "batchno": "PC1000008", 
    "orgId": 3, 
    "yhkh": "6688888888888888888888", 
    "kh": "A88888888", 
    "orgCode": "4400", 
    "devSeq": "1", 
    "backStatus": "2"

                 */
                MakeJson mj3 = new MakeJson();
                mj3.add("orgCode", aj["data"]["orgCode"], DataStyle.STR);
                mj3.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                mj3.add("atr", "3B6D0000008154443686605328A8888881", DataStyle.STR);
                mj3.add("ksbm", "3B6D0011111111111111113388888881", DataStyle.STR);
                mj3.add("sfzh", "413024197410137051", DataStyle.STR);
                mj3.add("xm", "张无", DataStyle.STR);
                mj3.add("status", "1", DataStyle.STR);
                mj3.add("orgId", aj["data"]["orgId"], DataStyle.STR);
                mj3.add("kh", "A123456781", DataStyle.STR);
                mj3.add("yhkh", "6688888888888888888881", DataStyle.STR);
                mj3.add("backStatus", "1", DataStyle.STR);
                mj3.add("description", "制卡成功", DataStyle.STR);


                string error3 = null;
                tools.AnalyzeJson aj3 = getJson(mj3, "getBackData", out error3);



                /*
                 channelcode	String	是	渠道编码
sfzh	String	是	身份证号
xm	String	是	姓名
pageno	int	否	页码
pagesize	int	否	页数
JSON数据格式： 
{
    "channelcode": "CardDispenser", 
    "sfzh": "413024197410137053", 
    "xm": "张四"
}
                 */
                //getCPCardInfo=/iface/machine/getCPCardInfo	#查询成品卡数据
                MakeJson mj4 = new MakeJson();
                mj4.add("sfzh", "413024197410137051", DataStyle.STR);
                mj4.add("xm", "张无", DataStyle.STR);
                string error4 = null;
                tools.AnalyzeJson aj4 = getJson(mj4, "getCPCardInfo", out error4);


                /*    查询领卡授权信息canGetCard
                 channelcode	String	是	渠道编码
bankNo	String	是	银行卡号
orgCode	String	是	网点编码(接口2.5返回orgCode)
devSeq	String	是	设备序号(接口2.5返回devSeq)
JSON数据格式： 
{
    "channelcode": "CardDispenser", 
    "bankNo": "66666668888888", 
    "orgCode": "4400", 
    "devSeq": "1"
}

                yhkh
                 */
                MakeJson mj5 = new MakeJson();
                mj5.add("bankNo", aj4["data"]["data"][0]["yhkh"], DataStyle.STR);
                mj5.add("orgCode", aj["data"]["orgCode"], DataStyle.STR);
                mj5.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                string error5 = null;
                tools.AnalyzeJson aj5 = getJson(mj5, "canGetCard", out error5);



                /*
   channelcode	String	是	渠道编码
   orgCode	String	是	网点编码(接口2.5返回orgCode)
   devSeq	String	是	设备序号(接口2.5返回devSeq)
   sfzh	String	是	身份证
   xm	String	是	姓名
   status	String	是	发卡状态 1:成功 2:失败
   description	String	是	成功或失败原因
   cardId	long	是	卡片id (成品卡：2.11接口返回的cardId)
   applytype	String	是	申请类型（0自领或1代领）
   orgId	String	是	网点id（2.5接口返回的orgId）
   szqmId	long	否	数字签名照片id (2.15接口返回的picId)
   rlzpId	long	否	领卡人照片id (2.15接口返回的picId)

                    */
                MakeJson mj2 = new MakeJson();
                mj2.add("orgCode",aj["data"]["orgCode"] , DataStyle.STR);
                mj2.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                mj2.add("sfzh", "413024197410137053", DataStyle.STR);
                mj2.add("xm", "张四", DataStyle.STR);
                mj2.add("status","1" , DataStyle.STR);
                mj2.add("description","成功" , DataStyle.STR);
                mj2.add("cardId", int.Parse(aj4["data"]["data"][0]["cardId"].ToString()), DataStyle.INT);
                mj2.add("applytype","0" , DataStyle.STR);
                mj2.add("orgId", aj["data"]["orgId"], DataStyle.STR);
                string error2 = null;
                tools.AnalyzeJson aj2 = getJson(mj2, "uploadFKRecord", out error2);



            }
            //
            {
                //2.2	校验软件版本信息checkUpdate
                /*
                 cpu	String	是	CPU编码
model	String	否	型号
hardwareVersion	String	否	设备硬件版本号
androidVersion	String	否	设备安卓版本号
appType	String	是	App类型（参考系统字典设置值）
softwareVersion	String	是	设备软件版本号（整数）
keyboardCode	String	否	密码键盘机器号
encryptionString	String	否	加密字符串
moduleId	String	否	3G模块ID

                 */
                MakeJson mj = new MakeJson();
                mj.add("cpu", GetMacAddress(), DataStyle.STR);
                mj.add("appType", "SelfService_Card_NH", DataStyle.STR);
                mj.add("softwareVersion", "1", DataStyle.STR);
                string error = null;
                tools.AnalyzeJson aj = getJson(mj, "checkUpdate", out error);



                //2.3	下载新版软件文件downloadVersion接口
                /*
                 cpu	String	是	CPU编码
model	String	否	型号
hardwareVersion	String	否	设备硬件版本号
androidVersion	String	否	设备安卓版本号
appType	String	是	App类型（参考系统字典设置值）
softwareVersion	String	否	设备软件版本号（整数）
keyboardCode	String	否	密码键盘机器号
encryptionString	String	否	加密字符串
moduleId	String	否	3G模块ID
                 */

                MakeJson mj2 = new MakeJson();
                mj2.add("cpu", GetMacAddress(), DataStyle.STR);
                mj2.add("appType", "SelfService_Card_NH", DataStyle.STR);
                string error2 = null;
                tools.AnalyzeJson aj2 = getJson(mj2, "downloadVersion", out error2);

            }
        }
        public static void test_add_card()
        {
            try
            {
                MakeJson mj = new MakeJson();
                string netError = null;
                tools.AnalyzeJson aj = getJson(mj, "deviceLogin", out netError);

                //6、接口入库
                /*
                 channelcode	String	是	渠道编码
orgCode	String	是	网点编码(接口2.5返回orgCode)
devSeq	String	是	设备序号(接口2.5返回devSeq)
atr	String	是	ATR
ksbm	String	是	卡识别码（社保卡时必填）
yhkh	String	是	银行卡号
shbzh	String	是	社会保障号（社保卡时必填）
sfzh	String	是	身份证（社保卡时必填）
xm	String	是	姓名（社保卡时必填）
slotno	int	是	槽号
orgId	long	是	网点id（2.5接口返回的orgId）
klb	String	是	卡类别 01:社保卡 02:借记卡 03:信用卡 
gfbb	String	否	规范版本
jgbm	String	否	机构编码
fkrq	String	否	发卡日期yyyyMMdd
kyxq	String	否	卡有效期
kh	String	否	卡号
sex	String	否	性别
nation	String	否	民族
csrq	String	否	出生日期

                 */
                string carDatas1 = "111|1|1|111111|20200101|20200101|01234567891|44178994455745255|李三|1|01|出生地|20200101";
                string[] carDatas = carDatas1.Split('|');
                MakeJson mj2 = new MakeJson();
                mj2.add("orgCode", aj["data"]["orgCode"], DataStyle.STR);
                mj2.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                mj2.add("orgId", int.Parse(aj["data"]["orgId"].ToString()), DataStyle.INT);
                mj2.add("atr", "111111111111111111", DataStyle.STR);
                mj2.add("yhkh", "66666666666666", DataStyle.STR);
                mj2.add("slotno", "1", DataStyle.INT);
                mj2.add("boxno", 1, DataStyle.INT);

                //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                //0         1       2         3               4         5         6     7             8     9     10    11      12

                //mj2.add("ksbm", carDatas[0], DataStyle.STR);
                //mj2.add("shbzh", carDatas[7], DataStyle.STR);
                //mj2.add("kh", carDatas[6], DataStyle.STR);
                //mj2.add("sfzh", carDatas[7], DataStyle.STR);
                //mj2.add("xm", carDatas[8], DataStyle.STR);
                //mj2.add("klb", "01", DataStyle.STR);
                //mj2.add("gfbb", carDatas[2], DataStyle.STR);
                //mj2.add("jgbm", carDatas[3], DataStyle.STR);
                //mj2.add("fkrq", carDatas[4], DataStyle.STR);
                //mj2.add("kyxq", carDatas[5], DataStyle.STR);
                //mj2.add("sex", carDatas[9], DataStyle.STR);
                //mj2.add("nation", carDatas[10], DataStyle.STR);
                //mj2.add("csrq", carDatas[12], DataStyle.STR);

                //string netError2 = null;
                //AnalyzeJson aj2 = Network3.getJson(mj2, "uploadYZCardInfo", out netError2);


                mj2.add("ksbm", carDatas[0], DataStyle.STR);
                mj2.add("shbzh", carDatas[7], DataStyle.STR);
                mj2.add("kh", carDatas[6], DataStyle.STR);
                mj2.add("sfzh", carDatas[7], DataStyle.STR);
                mj2.add("xm", carDatas[8], DataStyle.STR);
                mj2.add("klb", "01", DataStyle.STR);
                mj2.add("gfbb", carDatas[2], DataStyle.STR);
                mj2.add("jgbm", carDatas[3], DataStyle.STR);
                mj2.add("fkrq", carDatas[4], DataStyle.STR);
                mj2.add("kyxq", carDatas[5], DataStyle.STR);
                mj2.add("sex", carDatas[9], DataStyle.STR);
                mj2.add("nation", carDatas[10], DataStyle.STR);
                mj2.add("csrq", carDatas[12], DataStyle.STR);

                string netError2 = null;
                tools.AnalyzeJson aj2 = Network3.getJson(mj2, "uploadCPCardInfo", out netError2);

            }
            catch (Exception e)
            {

            }
            finally
            {
                Status.isWorking = false;
            }
        }

        public static void test_lingka()
        {
            MakeJson mj1 = new MakeJson();
            string error1 = null;
            tools.AnalyzeJson device = Network3.getJson(mj1, "deviceLogin", out error1);

            MakeJson mj = new MakeJson();
            mj.add("sfzh", "44178994455745255", DataStyle.STR);
            mj.add("xm", "李三", DataStyle.STR);
            string error = null;
            tools.AnalyzeJson aj = Network3.getJson(mj, "getCPCardInfo", out error);

            int index = 0;
            //固定部分
            MakeJson mj2 = new MakeJson();
            mj2.add("orgCode", device["data"]["orgCode"], DataStyle.STR);
            mj2.add("devSeq", device["data"]["devSeq"], DataStyle.STR);
            mj2.add("sfzh", aj["data"]["data"][index]["sfzh"], DataStyle.STR);
            mj2.add("xm", aj["data"]["data"][index]["xm"], DataStyle.STR);
            mj2.add("cardId", int.Parse(aj["data"]["data"][index]["cardId"].ToString()), DataStyle.INT);
            mj2.add("orgId", device["data"]["orgId"], DataStyle.STR);
            mj2.add("applytype", "0", DataStyle.STR);
            mj2.add("status", "1", DataStyle.STR);
            mj2.add("description", "成功", DataStyle.STR);

            string error2 = null;
            tools.AnalyzeJson aj2 = Network3.getJson(mj2, "uploadFKRecord", out error2);
        }

        public static tools.AnalyzeJson getJson(MakeJson json, string urlKey, out string error)
        {
            error = login();
            if (error != null)
                return null;
            if(urlKey != "checkUpdate" && urlKey != "downloadVersion")
            {
                json.add("deviceid", GetMacAddress(), DataStyle.STR);
                json.add("channelcode", channelcode, DataStyle.STR);
                json.add("tokenid", token, DataStyle.STR);
                json.add("equipmentNo", GetMacAddress(), DataStyle.STR);
            }

            string inJson = json.ToString();

            tools.AnalyzeJson aj = getJson_private(inJson, urlKey, out error);
            if (error == null)
                return aj;
            else
                return null;
        }
        public static tools.AnalyzeJson getJson(MakeJson json, string urlKey)
        {
            try
            {
                string error = null;
                error = login();
                if (error != null)
                    return new tools.AnalyzeJson(error, false);
                if (urlKey != "checkUpdate" && urlKey != "downloadVersion")
                {
                    json.add("deviceid", GetMacAddress(), DataStyle.STR);
                    json.add("channelcode", channelcode, DataStyle.STR);
                    json.add("tokenid", token, DataStyle.STR);
                    json.add("equipmentNo", GetMacAddress(), DataStyle.STR);
                }

                string inJson = json.ToString();

                tools.AnalyzeJson aj = getJson_private(inJson, urlKey, out error);
                error = getError(aj, error);
                if (error != null)
                    aj.error = error;
                return aj;
            }
            catch (Exception e)
            {
                return new tools.AnalyzeJson(e.ToString(), false);
            }
        }

        public static string login()
        {
            if (channelcodeMD5 == null)
                channelcodeMD5 = MD5.GetMD5String(channelcode);
            MakeJson mj = new MakeJson();
            
            mj.add("channelcode", channelcode, DataStyle.STR);
            mj.add("username", channelcodeMD5, DataStyle.STR);
            mj.add("password", channelcodeMD5, DataStyle.STR);
            string inJson = mj.ToString();
            string loginError = null;
            tools.AnalyzeJson aj = getJson_private(inJson, "Login", out loginError);
            if (loginError != null)
                return loginError;
            if (aj["statusCode"].ToString() != "200")
                return "登录失败";
            else
            {
                token = aj["data"].ToString();
                return null;
            }
        }

        private static tools.AnalyzeJson getJson_private(string inJson, string urlKey, out string error)
        {
            Log.AddLog(logName, "接口入参:" + inJson);
            error = null;
            string url = null;
            try
            {
                url = Config.net_dic("BaseURL") + Config.net_dic(urlKey);
                if (urlKey != "Login")
                    url += string.Format("?deviceid={0}&tokenid={1}", GetMacAddress(), token);
            }
            catch(Exception e0)
            {
                Log.AddLog(logName, "读取接口" + urlKey + "失败:" + e0.ToString());
                error = "参数配置异常";
                return null;
            }
            Log.AddLog(logName, "接口地址:" + url);
            Uri uri = new Uri(url);
            Common c = new Common();
            string jsonStr = null;
            try
            {
                jsonStr = c.RetrunJSONValueByHttps(uri, inJson, ref error);
                if(error != null)
                    jsonStr = c.RetrunJSONValueByHttps(uri, inJson, ref error);
                Log.AddLog(logName, "接口出参:" + (jsonStr == null ? "" : jsonStr));
                Log.AddLog(logName, "错误信息:" + (error == null ? "" : error));
                if(error != null)
                    error = "网络异常";
            }
            catch (Exception e1)
            {
                error = "网络异常";
                Log.AddLog(logName, "调用接口失败:" + e1.ToString());
                return null;
            }
            if (error != null)
                return null;

            try
            {
                tools.AnalyzeJson aj = new tools.AnalyzeJson(jsonStr);
                return aj;
            }
            catch (Exception e2)
            {
                Log.AddLog(logName, "数据解析异常:" + e2.ToString());
                error = "数据解析异常";
                return null;
            }
        }

        public static string getError(tools.AnalyzeJson json, string error)
        {
            if (error != null)
                return error;
            else if (json == null)
                return "空的内容";
            else if (json["statusCode"].ToString() != "200")
                return json["message"].ToString();
            else
                return null;
        }

        static string mac = null;
        public static string GetMacAddress()
        {
           // return "00:ED:GD:ED:1A:16";
            //return "BFEBFBFF000306A9";
            if (mac != null) return mac;
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString().Replace(":","");
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
    }
}
