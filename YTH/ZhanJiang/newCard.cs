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

namespace YTH.ZhanJiang.BuKa
{
    class newCard
    {
        static newCard self = null;
        public static newCard GetObject()
        {
            if (self == null)
                self = new newCard();
            return self;
        }

        I_GetSSCar_old getSSCar = null;
        const string log = "新办卡";
        string timeTag = "";
        int index = 0;
        //节点信息
        string[] selfNames = new string[] {
            "读身份证",
            "数据收集",
            "数据处理",
            "社保制卡",
            "卡口取卡"
        };
        //读身份证
        public void Goin()
        {
            index = 0;
            Business.Init(Goin, "社保卡新办卡", selfNames);
            A_ReadIDCar readIDCar = A_ReadIDCar.getObject();
            CD.business1.setBusinessValue(readIDCar);
            updateTitle();
            readIDCar.Goin(inputPhone);
        }
        //数据收集：输入手机号
        private void inputPhone()
        {
            updateTitle();
            InputPhoneNum.GetObject().Goin(selectStyle);
        }
        //数据收集：选择参保身份
        private void selectStyle()
        {
            SelectCardStyle.GetObject().Goin(handlePersionData);
        }
        //挂失和获取数据
        async private void handlePersionData(string style)
        {
            updateTitle();
            CD.business1.hidenBackAndExitBtn();
            CD.business1.stop();
            Loading.show1("正在申请制卡数据");
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
                zkData = WeiWang.getZKData(ReadIDCar.persionid, ReadIDCar.name, out error);
                if(error != null)
                {
                    error = null;
                    //一人一卡查询 00 返回
                   if( WeiWang.allDsjk(ReadIDCar.persionid, ReadIDCar.name, out error))
                    {
                        ShowTip.show(false, BackExit.Exit, "您已有卡，不能重复制卡");
                        return;
                    }
                    error = null;
                    error = WeiWang.applyDataXinBanKa(style, Config.dic("AreaCode"));
                    //获取制卡数据
                    if (error == null)
                        zkData = WeiWang.getZKData(ReadIDCar.persionid, ReadIDCar.name, out error);
                }
              
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
            cpd.Goin(zkData[0]["AAC002"], zkData[0]["AAC003"], zkData[0]["PHOTO"], WriteCar);

            //给ReadIDCar赋值
           // ReadIDCar.persionid = zkData[0]["AAC002"];
        }
        //社保制卡 、签名留底
        string ssid = null;
        string bankcarNum = "";
        MakeJson mj3 = null;
        async private void WriteCar()
        {
            updateTitle();
            CD.business1.hidenBackAndExitBtn();
            Loading.show1("制卡中,请稍候...");
            string error = null;
            string atr2 = null;

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


            })).ConfigureAwait(true);
            if (error != null)
            {
                ShowTip.show(false, BackExit.Exit, error);
                return;
            }

            updateTitle();
            // Autograph.GetObject().Goin(sign);
            putCardOut();
        }
        string serialNumber = "";//流水号
       
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
                //List<string> printDatas = new List<string>();
                //printDatas.Add("        肇庆市社会保障卡业务回执单");
                //printDatas.Add("业务类型：自助申领");
                //printDatas.Add("交易流水号：" + serialNumber);
                //printDatas.Add("终端名称：" + deviceMsg["data"]["deviceName"].ToString());
                //printDatas.Add("终端编号：" + deviceMsg["data"]["deviceId"].ToString());
                //printDatas.Add("所属区域：" + deviceMsg["data"]["areaName"].ToString());
                //printDatas.Add("所属网点：" + deviceMsg["data"]["branch"].ToString());
                //printDatas.Add("网点编号：" + deviceMsg["data"]["orgCode"].ToString());
                //printDatas.Add("交易时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
                //printDatas.Add("交易结果：申领成功");
                //printDatas.Add("卡号：" + CD.hidenBankNum(bankcarNum));
                //printDatas.Add("领卡人：" + CD.hidenName(ReadIDCar.name));
                //Print.print(printDatas);
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

