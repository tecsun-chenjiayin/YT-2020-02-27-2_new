using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTH.BuKa;
using YTH.Controls;
using YTH.Functions;
using YTH.Functions.MSDLL;
using YTH.Functions.ReadCarAndSQCode;
using YTH.GetCar;
using YTH.GetCar.Self;
using YTH.NHNet;
namespace YTH.ZhanJiang
{
    class huanka
    {
        static huanka self = null;
        public static huanka GetObject()
        {
            if (self == null)
                self = new huanka();
            return self;
        }

        const string log = "补卡";
        int index = 0;
        //节点信息
        string[] selfNames = new string[] {
            "读身份证",
            "数据处理",
            "社保制卡",
            "签名留底",
            "授权取卡",
            "卡口取卡"
        };

        //读身份证
        public void Goin()
        {
            index = 0;
            Business.Init(Goin, "社保卡补卡", selfNames);
            A_ReadIDCar readIDCar = A_ReadIDCar.getObject();
            CD.business1.setBusinessValue(readIDCar);
            updateTitle();
            readIDCar.Goin(handlePersionData);
        }
        //数据处理
        tools.AnalyzeJson deviceMsg = null;
        async private void handlePersionData()
        {
            Log(ReadIDCar.pOutInfo.ToString());
            updateTitle();
            CD.business1.hidenBackAndExitBtn();
            CD.business1.stop();
            Loading.show1("正在挂失");
            List<Dictionary<string, string>> zkData = null;
            string error = null;
            await TaskMore.Run(new Action(() =>
            {
                int box = int.Parse(Config.dic("yzkBoxs"));
                int ret = MS2.getLetfCardNum(box, out error);
                if (error == null && ret == 0)
                    error = "预制卡已用完，请联系管理员加卡";
                else if (error == null && ret == -1)
                    error = "料盒状态异常，请联系管理员处理！";
                if (error != null) return;
                MakeJson mj = new MakeJson();
                deviceMsg = Network3.getJson(mj, "deviceLogin");
                error = deviceMsg.error;
                if (error != null) return;
                //获取制卡数据
              
                if (error == null)
                    zkData = WeiWang.getZKData(ReadIDCar.persionid, ReadIDCar.name, out error);
            })).ConfigureAwait(true);
            if (error != null)
            {
                ShowTip.show(false, BackExit.Exit, error);
                return;
            }
            check(zkData);
        }
        //数据校验
        CheckPersionData cpd = null;
        private void check(List<Dictionary<string, string>> zkData)
        {
            if (cpd == null)
                cpd = new CheckPersionData();
            cpd.Goin(zkData[0]["社会保障号码"], zkData[0]["姓名"], zkData[0]["印刷相片"], WriteCar);
        }
        //社保制卡 、签名留底        
        string ssid = "";
        string bankcarNum = "";
        MakeJson mj3 = null;
        async private void WriteCar()
        {
            updateTitle();
            CD.business1.hidenBackAndExitBtn();
            Loading.show1("制卡中,请稍候...");
            string error = null;
            await TaskMore.Run(new Action(() =>
            {
                //==制卡
                string result = WeiWang.iWrite(out error);
                if (error != null)
                {
                    if (MS2.PutCardToReject() != null)
                        error += "-卡回收失败";
                    return;
                }

                /*
                //==读卡信息
                string[] datas = null;
                string atr2 = MS2.GetATR(out error);
                if (error != null)
                    return;
                bankcarNum = MS2.ReadBankNum(out error);
                if (error != null)
                    return;
                //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                //0         1       2         3               4         5         6     7             8     9     10    11      12
                datas = MS2.GetBaseMsg(out error);
                if (error != null)
                    return;
                ssid = datas[6];
                */

                //0,6217281914006994119,441800  ,441225198703040437,R47708862,441800D1560000053030737878EC1A84,杨建辉,0087CF20018649618B00930612,2.00    ,20200226,20300226
                //0,1                  ,2       ,3                 ,4        ,5                               ,6     ,7                         ,8       ,9       ,10
                //0,银行卡号           ,发卡地区,社会保障号码      ,卡号     ,卡识别码                        ,姓名  ,卡复位信息                ,规范版本,发卡日期,卡有效期
                string[] results = result.Split(',');

                MakeJson mj = new MakeJson();
                tools.AnalyzeJson aj = Network3.getJson(mj, "deviceLogin", out error);
                mj3 = new MakeJson();
                mj3.add("yhkh", results[1], DataStyle.STR);
                mj3.add("orgCode", aj["data"]["orgCode"], DataStyle.STR);
                mj3.add("devSeq", aj["data"]["devSeq"], DataStyle.STR);
                mj3.add("orgId", aj["data"]["orgId"], DataStyle.STR);
                mj3.add("atr", results[7], DataStyle.STR);
                mj3.add("ksbm", results[5], DataStyle.STR);
                mj3.add("sfzh", results[3], DataStyle.STR);
                mj3.add("xm", results[6], DataStyle.STR);
                mj3.add("kh", results[4], DataStyle.STR);
                mj3.add("backStatus", "1", DataStyle.STR);
                mj3.add("status", "1", DataStyle.STR);
                mj3.add("description", "制卡成功", DataStyle.STR);
                ssid = results[4];
            })).ConfigureAwait(true);
            if (error != null)
            {
                ShowTip.show(false, BackExit.Exit, error);
                return;
            }

            updateTitle();
            Autograph.GetObject().Goin(sign);
        }

        string serialNumber = "";//流水号
        //合并签名信息回盘
        async private void sign(string pic)
        {
            Loading.show1("正在保存签名信息...");
            tools.AnalyzeJson aj3 = null;
            await TaskMore.Run(new Action(() =>
            {
                mj3.add("receipt", pic, DataStyle.STR);
                Log("send:" + mj3.ToString());
                aj3 = Network3.getJson(mj3, "getBackData");
                if (aj3.error != null)
                {
                    if (MS2.PutCardToReject() != null)
                        aj3.error += "-卡回收失败";
                }
                else
                    serialNumber = aj3["data"].ToString();
            })).ConfigureAwait(true);
            if (aj3.error != null)
                ShowTip.show(false, BackExit.Exit, aj3.error);
            else
                manageCheck();
        }
        //授权
        private void manageCheck()
        {
            updateTitle();
            CheckPassword3.GetObject().Goin(putCardOut, true, "制卡成功，请呼叫工作人员授权取卡!");
        }
        //出卡
        async private void putCardOut()
        {
            updateTitle();
            string error = null;
            Loading.show1("正在激活社保卡，请稍候...");

            await TaskMore.Run(new Action(() => {
                //激活
                error = WeiWang.setStart(ssid);
                //打印凭条
                List<string> printDatas = new List<string>();
                printDatas.Add("        肇庆市社会保障卡业务回执单");
                printDatas.Add("业务类型：自助换卡");
                printDatas.Add("交易流水号：" + serialNumber);
                printDatas.Add("终端名称：" + deviceMsg["data"]["deviceName"].ToString());
                printDatas.Add("终端编号：" + deviceMsg["data"]["deviceId"].ToString());
                printDatas.Add("所属区域：" + deviceMsg["data"]["areaName"].ToString());
                printDatas.Add("所属网点：" + deviceMsg["data"]["branch"].ToString());
                printDatas.Add("网点编号：" + deviceMsg["data"]["orgCode"].ToString());
                printDatas.Add("交易时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
                printDatas.Add("交易结果：换卡成功");
                printDatas.Add("卡号：" + CD.hidenBankNum(bankcarNum));
                printDatas.Add("领卡人：" + CD.hidenName(ReadIDCar.name));
                Print.print(printDatas);
                //出卡
                MS2.PutCardOut();
            })).ConfigureAwait(true);
            if (error != null)
                ShowTip.show(false, null, "社保卡激活失败：" + error);

            I_GetSSCar_old getSSCar = I_GetSSCar_old.getObject();
            getSSCar.Goin();
            BackExit.LetNextClickToMain();
            CD.business1.showBackAndExitBtn();
        }


        private void updateTitle()
        {
            CD.business1.setTitle(selfNames[index]);
            index++;
            CD.business1.setIndex(index);
        }
        private void Log(string msg)
        {
            YTH.Functions.Log.AddLog(MS2.log, msg);
        }
    }
}
