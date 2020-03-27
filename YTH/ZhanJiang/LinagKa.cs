using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTH.BuKa;
using YTH.Controls;
using YTH.Functions;
using YTH.Functions.MSDLL;
using YTH.Functions.ReadCarAndSQCode;
using YTH.GetCar;
using YTH.GetCar.Self;
using YTH.LingKa;
using YTH.NHNet;

namespace YTH.ZhanJiang
{
    class LinagKa
    {
        static LinagKa self = null;
        public static LinagKa GetObject()
        {
            if (self == null)
                self = new LinagKa();
            return self;
        }

        //节点信息
        string[] selfNames = new string[] {
            "读身份证",
            "选择卡片",
            "信息校验",
            "授权取卡",
            "卡片启用",
            "卡口取卡"
        };
        MakeJson mjBack = null;
        const string log = "领卡";
        string atr1 = "";
        string timeTag = "";
        NetStepUI nsu = null;
        static int index = 0;
        SelectCars sc = null;
        tools.AnalyzeJson deviceInfo = null;
        tools.AnalyzeJson cpCardInfo = null;
        List<int> ds = null;
        tools.AnalyzeJson tellHadGetCard = null;
        string ssid = null;
        //1.入口-读身份证信息
        public void Goin()
        {
            ds = null;
            index = 0;
            timeTag = CD.timeTag.updateTag();

            Business.Init(Goin, "办理领卡", selfNames);
            updateTitle();
            A_ReadIDCar readIDCar = A_ReadIDCar.getObject();
            CD.business1.setBusinessValue(readIDCar);
            readIDCar.Goin(selectCards);
        }
        //2.选择卡片
        async private void selectCards()
        {
            index = 1;
            string error = null;
            if (CD.timeTag.equal(timeTag) == false)
                return;
            Loading.show1("正在查询个人数据...");
            await TaskMore.Run(new Action(() =>
            {
                MakeJson mj_normal = new MakeJson();
                deviceInfo = Network3.getJson(mj_normal, "deviceLogin");
                error = deviceInfo.error;
                if (error != null)
                    return;

                MakeJson mj = new MakeJson();
                mj.add("sfzh", ReadIDCar.persionid, DataStyle.STR);
                mj.add("xm", ReadIDCar.name, DataStyle.STR);
                cpCardInfo = Network3.getJson(mj, "getCPCardInfo");
                error = cpCardInfo.error;
            })).ConfigureAwait(true);
            if (error != null)
            {
                ShowTip.show(false, BackExit.Exit, error);
                return;
            }
            try
            {
                List<string[]> datas = new List<string[]>();
                int count = cpCardInfo["data"]["data"].getArrayCount();
                //"序号,姓名,身份证号,待领卡类型,银行卡号,社保卡号"
                int k = 1;
                for (int i = 0; i < count; i++)
                {
                    if (cpCardInfo["data"]["data"][i]["status"].ToString() == "0")
                    {
                        string name = cpCardInfo["data"]["data"][i]["xm"].ToString();
                        string sfzh = cpCardInfo["data"]["data"][i]["sfzh"].ToString();
                        string yhkh = cpCardInfo["data"]["data"][i]["yhkh"].ToString();

                        name = "*" + name.Substring(1, name.Length - 1);
                        sfzh = sfzh.Substring(0, 6) + "**********" + sfzh.Substring(16, 2);
                        yhkh = "**********************".Substring(0, yhkh.Length - 4) + yhkh.Substring(yhkh.Length - 4, 4);

                        datas.Add(new string[] {
                        (k++).ToString(),
                        name,
                        sfzh,
                        cpCardInfo["data"]["data"][i]["klbName"].ToString(),
                        yhkh,
                        cpCardInfo["data"]["data"][i]["kh"].ToString()});
                    }
                }

                if (sc == null)
                {
                    sc = new SelectCars();
                    sc.setSelectOK(goToLingKa);
                }
                updateTitle();
                sc.setTable(datas);
                CD.business1.setBusinessValue(sc);
            }
            catch (Exception e)
            {
                ShowTip.show(false, BackExit.Exit, "数据解析异常：" + e.ToString());
                return;
            }
        }
        //3.信息校验
        async private void goToLingKa()
        {
            bool carHasOut = false;
            tellHadGetCard = null;
            string error = null;
            string bankcarnum = null;
            if (ds == null)
                ds = sc.getSelectItems();
            if (ds == null || ds.Count == 0)
                return;

            updateTitle();
            CD.business1.hidenBackAndExitBtn();
            CD.business1.stop();
            int getIndex = ds[0];
            ds.RemoveAt(0);

            Loading.show1("正在出卡，请稍候....");
            string carMsg = cpCardInfo["data"]["data"][getIndex]["klbName"].ToString() + "-" +
                    cpCardInfo["data"]["data"][getIndex]["yhkh"].ToString() + "-" +
                    cpCardInfo["data"]["data"][getIndex]["kh"].ToString();
            string msg = cpCardInfo["data"]["data"][getIndex]["cardId"].ToString();
            MakeJson mj2 = new MakeJson();
            mj2.add("orgCode", deviceInfo["data"]["orgCode"], DataStyle.STR);
            mj2.add("devSeq", deviceInfo["data"]["devSeq"], DataStyle.STR);
            mj2.add("sfzh", cpCardInfo["data"]["data"][getIndex]["sfzh"], DataStyle.STR);
            mj2.add("xm", cpCardInfo["data"]["data"][getIndex]["xm"], DataStyle.STR);
            mj2.add("cardId", int.Parse(cpCardInfo["data"]["data"][getIndex]["cardId"].ToString()), DataStyle.INT);
            mj2.add("orgId", deviceInfo["data"]["orgId"], DataStyle.STR);
            mj2.add("applytype", "0", DataStyle.STR);
            string atr_json = cpCardInfo["data"]["data"][getIndex]["atr"].ToString();
            int kc = int.Parse(cpCardInfo["data"]["data"][getIndex]["slotno"].ToString());

            await TaskMore.Run(new Action(() =>
            {
                string atr_raed = null;
                string[] carDatas = null;
                error = MS2.PutCardToIC(atr_json, kc);
                if(error == null)
                    atr_raed = MS2.GetATR(out error);
                if (error == null)
                    carDatas = MS2.GetBaseMsg(out error);
                if (error == null)
                    bankcarnum = MS2.ReadBankNum(out error);
                if (error != null)
                {
                    ShowTip.show(false, BackExit.Exit, error);
                    return;
                }
                ssid = carDatas[6];
                    //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
                    //0         1       2         3               4         5         6     7             8     9     10    11      12
                if (atr_raed != cpCardInfo["data"]["data"][getIndex]["atr"].ToString() ||
                    carDatas[7] != cpCardInfo["data"]["data"][getIndex]["sfzh"].ToString() ||
                    carDatas[8] != cpCardInfo["data"]["data"][getIndex]["xm"].ToString() ||
                    carDatas[6] != cpCardInfo["data"]["data"][getIndex]["kh"].ToString() ||
                    bankcarnum != cpCardInfo["data"]["data"][getIndex]["yhkh"].ToString()
                    )
                {
                    error = "信息校验失败！";
                    MS2.PutCardToReject();
                }

                if (error != null)
                {
                    mj2.add("status", "2", DataStyle.STR);
                    mj2.add("description", "卡信息校验失败", DataStyle.STR);
                }
                else
                {
                    mj2.add("status", "1", DataStyle.STR);
                    mj2.add("description", "成功", DataStyle.STR);
                }
                tellHadGetCard = Network3.getJson(mj2, "uploadFKRecord");
            })).ConfigureAwait(true);
            if (error != null)
            {
                Log.AddLog("领卡", carMsg + "  error:" +error);
                if (carHasOut)
                {
                    MS2.PutCardToReject();
                }
                ShowTip.show(false, BackExit.Exit, error);
            }
            else if (tellHadGetCard.error != null)
            {
                MS2.PutCardToReject();

                ShowTip.show(false, BackExit.Exit, tellHadGetCard.error);
            }
            else
            {
                checkPsw();
            }

        }
        //4.管理员输入密码
        private void checkPsw()
        {
            updateTitle();
            CheckPassword3.GetObject().Goin(PutCardOut, true, "卡片出库成功请呼叫工作人员授权取卡");
        }
        //5.取卡
        async private void PutCardOut()
        {
            string error = null;
            updateTitle();
            Loading.show1("正在启用社保卡，请稍候....");
            await TaskMore.Run(new Action(() =>
            {
                //激活
                error = WeiWang.setStart(ssid);
                MS2.PutCardOut();
            })).ConfigureAwait(true);
            new Task(new Action(() => {
                //打印凭条
                List<string> printDatas = new List<string>();
                printDatas.Add("        肇庆市社会保障卡业务回执单");
                printDatas.Add("业务类型：自助领卡");
                printDatas.Add("交易流水号：" + tellHadGetCard["data"]["transNo"].ToString());
                printDatas.Add("终端名称：" + deviceInfo["data"]["deviceName"].ToString());
                printDatas.Add("终端编号：" + deviceInfo["data"]["deviceId"].ToString());
                printDatas.Add("所属区域：" + deviceInfo["data"]["areaName"].ToString());
                printDatas.Add("所属网点：" + deviceInfo["data"]["branch"].ToString());
                printDatas.Add("网点编号：" + deviceInfo["data"]["orgCode"].ToString());
                printDatas.Add("交易时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
                printDatas.Add("交易结果：领卡成功");
                printDatas.Add("卡号：" + CD.hidenBankNum(tellHadGetCard["data"]["yhkh"].ToString()));
                printDatas.Add("领卡人：" + CD.hidenName(tellHadGetCard["data"]["xm"].ToString()));
                Print.print(printDatas);
            })).Start();

            if (error != null)
                ShowTip.show(false, null, error);
            if (ds.Count > 0)
                I_GetSSCar_old.getObject().Goin(goToLingKa);
            else
            {
                CD.business1.showBackAndExitBtn();
                I_GetSSCar_old.getObject().Goin();
            }
            BackExit.LetNextClickToMain();
            CD.business1.showBackAndExitBtn();
        }
        private void updateTitle()
        { 
            CD.business1.setTitle(selfNames[index]);
            index++;
            CD.business1.setIndex(index);
        }
        ////5.出卡
        //MakeJson mj2 = null;
        //string carMsg = null;
        //int getIndex = 0;
        //private void putCarOut()
        //{
        //    updateTitle();

        //    if (ds.Count == 0 || getIndex >= ds.Count)
        //    {
        //        BackExit.Exit();
        //        return;
        //    }
        //    TH.addOnceUI(CD.countDownTime.stop);
        //    Loading.show("正在出卡，请稍候....");
        //    int index = ds[getIndex]; //dic[ds[0]];
        //    getIndex++;
        //    //ds.RemoveAt(0);
        //    carMsg = cpCardInfo["data"]["data"][index]["klbName"].ToString() + "-" +
        //                cpCardInfo["data"]["data"][index]["yhkh"].ToString() + "-" +
        //                cpCardInfo["data"]["data"][index]["kh"].ToString();

        //    //检查是否可以领卡
        //    MakeJson mj5 = new MakeJson();
        //    mj5.add("bankNo", cpCardInfo["data"]["data"][index]["yhkh"], DataStyle.STR);
        //    mj5.add("orgCode", deviceInfo["data"]["orgCode"], DataStyle.STR);
        //    mj5.add("devSeq", deviceInfo["data"]["devSeq"], DataStyle.STR);
        //    string error5 = null;
        //    AnalyzeJson aj5 = Network3.getJson(mj5, "canGetCard", out error5);
        //    if (error5 != null)
        //    {
        //        ShowTip.show(false, BackExit.Exit, "网络异常(3)");
        //        //ShowError.show1("网络异常");
        //        return;
        //    }
        //    else if (aj5["message"].ToString() != "可以领卡")
        //    {
        //        NetStepUI.show(putCarOut, "卡片:" + carMsg + "不可领取", getIndex == ds.Count);
        //        return;
        //    }

        //    //固定部分
        //    mj2 = new MakeJson();
        //    mj2.add("orgCode", deviceInfo["data"]["orgCode"], DataStyle.STR);
        //    mj2.add("devSeq", deviceInfo["data"]["devSeq"], DataStyle.STR);
        //    mj2.add("sfzh", cpCardInfo["data"]["data"][index]["sfzh"], DataStyle.STR);
        //    mj2.add("xm", cpCardInfo["data"]["data"][index]["xm"], DataStyle.STR);
        //    mj2.add("cardId", int.Parse(cpCardInfo["data"]["data"][index]["cardId"].ToString()), DataStyle.INT);
        //    mj2.add("orgId", deviceInfo["data"]["orgId"], DataStyle.STR);
        //    mj2.add("applytype", "0", DataStyle.STR);

        //    //卡库出卡 slotno
        //    string atr = null;
        //    string tag = cpCardInfo["data"]["data"][index]["atr"].ToString();
        //    atr1 = tag;
        //    int kc = int.Parse(cpCardInfo["data"]["data"][index]["slotno"].ToString());
        //    bool carHasOut = false;
        //    string error = MS2.putCarToIC_Old2(tag, kc, out atr, out carHasOut);
        //    if (error != null && carHasOut == false)
        //    {
        //        ShowSubTip.Show(CD.business1, TIPSTYLE.MachineError, "设备异常");
        //        //ShowError.show1("设备异常");
        //        return;
        //    }
        //    else if (error != null && carHasOut)
        //    {
        //        mj2.add("status", "2", DataStyle.STR);
        //        mj2.add("description", "读卡异常", DataStyle.STR);
        //        sendDataToNet("读卡异常");
        //        return;
        //    }


        //    string carData = null;
        //    string bankcarnum = null;
        //    if (cpCardInfo["data"]["data"][index]["klbName"].ToString() == "社保卡")
        //        carData = MS2.readCar(out error, out atr);
        //    if (error == null)
        //        bankcarnum = MS2.readBankCarNum(out error);
        //    if (error != null)
        //    {
        //        mj2.add("status", "2", DataStyle.STR);
        //        mj2.add("description", "读卡异常", DataStyle.STR);
        //        sendDataToNet("读卡异常");
        //        return;
        //    }
        //    string[] carDatas = null;
        //    bool checkSuccess = true;
        //    //校验信息
        //    if (cpCardInfo["data"]["data"][index]["klbName"].ToString() == "社保卡")
        //    {
        //        //卡识别码、卡类别、规范版本、初始化机构编号、发卡日期、卡有效期、卡号、社会保障号码、姓名、性别、民族、出生地、出生日期
        //        //0         1       2         3               4         5         6     7             8     9     10    11      12
        //        carDatas = carData.Split('|');
        //        if (atr != cpCardInfo["data"]["data"][index]["atr"].ToString() ||
        //            carDatas[7] != cpCardInfo["data"]["data"][index]["sfzh"].ToString() ||
        //            carDatas[8] != cpCardInfo["data"]["data"][index]["xm"].ToString() ||
        //            carDatas[6] != cpCardInfo["data"]["data"][index]["kh"].ToString() ||
        //            bankcarnum != cpCardInfo["data"]["data"][index]["yhkh"].ToString()
        //            )
        //        {
        //            checkSuccess = false;
        //        }
        //    }
        //    else
        //    {
        //        if (atr != cpCardInfo["data"]["data"][index]["atr"].ToString() ||
        //            bankcarnum != cpCardInfo["data"]["data"][index]["yhkh"].ToString()
        //            )
        //        {
        //            checkSuccess = false;
        //        }
        //    }

        //    //////////////////////预备卡回收的json内容/////////////////////////
        //    mjBack = new MakeJson();
        //    mjBack.add("orgCode", deviceInfo["data"]["orgCode"], DataStyle.STR);
        //    mjBack.add("devSeq", deviceInfo["data"]["devSeq"], DataStyle.STR);
        //    mjBack.add("orgId", deviceInfo["data"]["orgId"], DataStyle.STR);
        //    mjBack.add("atr", tag, DataStyle.STR);
        //    mjBack.add("yhkh", bankcarnum, DataStyle.STR);
        //    mjBack.add("boxno", 1, DataStyle.INT);
        //    //(唯一变的是)mjBack.add("slotno", stlo, DataStyle.INT);
        //    mjBack.add("ksbm", carDatas[0], DataStyle.STR);
        //    mjBack.add("shbzh", carDatas[7], DataStyle.STR);
        //    mjBack.add("kh", carDatas[6], DataStyle.STR);
        //    mjBack.add("sfzh", carDatas[7], DataStyle.STR);
        //    mjBack.add("xm", carDatas[8], DataStyle.STR);
        //    mjBack.add("klb", "01", DataStyle.STR);
        //    mjBack.add("gfbb", carDatas[2], DataStyle.STR);
        //    mjBack.add("jgbm", carDatas[3], DataStyle.STR);
        //    mjBack.add("fkrq", carDatas[4], DataStyle.STR);
        //    mjBack.add("kyxq", carDatas[5], DataStyle.STR);
        //    mjBack.add("sex", carDatas[9], DataStyle.STR);
        //    mjBack.add("nation", carDatas[10], DataStyle.STR);
        //    mjBack.add("csrq", carDatas[12], DataStyle.STR);

        //    atr = tag;
        //    ///////////////////////////////////////////////////////////////////

        //    if (checkSuccess == false)
        //    {
        //        mj2.add("status", "2", DataStyle.STR);
        //        mj2.add("description", "卡信息校验失败", DataStyle.STR);
        //        sendDataToNet("卡信息校验失败");
        //    }
        //    else
        //    {
        //        mj2.add("status", "1", DataStyle.STR);
        //        mj2.add("description", "成功", DataStyle.STR);
        //        sendDataToNet(null);
        //    }


        //}

        //private void sendDataToNet(string error)
        //{
        //    string error2 = null;
        //    AnalyzeJson aj2 = Network3.getJson(mj2, "uploadFKRecord", out error2);
        //    string error3 = null;
        //    if (error != null && error2 == null)
        //    {
        //        error3 = error;
        //    }
        //    if (error == null && error2 != null)
        //    {
        //        error3 = "网络异常";
        //    }
        //    if (error != null && error2 != null)
        //    {
        //        error3 = error + ",网络异常";
        //    }
        //    if (error3 != null)
        //    {
        //        error3 = carMsg + ":" + error3;
        //        MS2.PutCardToReject();
        //        if (ds.Count == 0)
        //            ShowError.show1(error3);
        //        else
        //            NetStepUI.show(putCarOut, error3, getIndex == ds.Count);
        //    }
        //    else if (aj2["statusCode"].ToString() != "200")
        //    {
        //        error3 = carMsg + ":" + aj2["message"].ToString();
        //        MS2.PutCardToReject();
        //        if (ds.Count == 0)
        //            ShowError.show1(error3);
        //        else
        //            NetStepUI.show(putCarOut, error3, getIndex == ds.Count);
        //    }
        //    else
        //    {
        //        List<string> printDatas = new List<string>();
        //        printDatas.Add("业务类型：" + aj2["data"]["busType"].ToString());
        //        printDatas.Add("交易流水号：" + aj2["data"]["transNo"].ToString());
        //        printDatas.Add("交易终端名称：" + aj2["data"]["transDevName"].ToString());
        //        printDatas.Add("交易终端编号：" + aj2["data"]["transDevId"].ToString());
        //        printDatas.Add("交易时间：" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
        //        printDatas.Add("交易结果：" + aj2["data"]["transResult"].ToString());
        //        printDatas.Add("领卡人卡片信息：");
        //        printDatas.Add("卡号：" + aj2["data"]["yhkh"].ToString());
        //        printDatas.Add("领卡人：" + aj2["data"]["xm"].ToString());
        //        Print.print(printDatas);
        //        MS2.PutCardOut();

        //        getCar(putCarOut);
        //    }
        //}

        ////尝试下一张
        //private void TryToNext()
        //{
        //    Loading.show("正在出卡，请稍候....");
        //    TH.addOnceData(putCarOut);
        //}

        ////5.取卡
        //private void getCar(Action nextStep)
        //{
        //    if (CD.timeTag.equal(timeTag) == false)
        //        return;
        //    TH.addOnceUI(new Action(() => {
        //        CD.business1.setIndex(4);

        //        if (ds.Count - index <= 0)
        //            nextStep = null;
        //        GetCar.I_GetSSCar.getObject().Goin(nextStep, getBackCar);
        //    }));
        //}

        

        ////6.卡片回收
        //public void getBackCar()
        //{
        //    TH.addOnceUI(CD.business1.hidenBackAndExitBtn);
        //    Loading.show("正在回收卡片，请勿操作...");

        //    int slot = 0;
        //    int ret = MS2.letCarBackToKC(atr1, ref slot);
        //    if (ret == 1)
        //        ShowSubTip.Show(CD.business1, TIPSTYLE.PutCarOutFailed, "卡回收失败，卡片已移到回收箱");
        //    else if (ret == 2)
        //    {
        //        ShowSubTip.Show(CD.business1, TIPSTYLE.MachineError, "设备故障，请联系管理员处理");
        //        return;
        //    }
        //    else
        //    {
        //        mjBack.add("slotno", slot, DataStyle.INT);
        //        string netError2 = null;
        //        AnalyzeJson aj2 = Network3.getJson(mjBack, "uploadCPCardInfo", out netError2);
        //        if (netError2 != null || (aj2 != null && aj2["statusCode"].ToString() != "200"))
        //        {
        //            if (MS2.putCarToReject(atr1, slot) != 0)
        //            {
        //                ShowSubTip.Show(CD.business1, TIPSTYLE.MachineError, "设备故障(006)，请联系管理员处理");
        //                return;
        //            }
        //            else
        //            {
        //                ShowSubTip.Show(CD.business1, TIPSTYLE.PutCarOutFailed, "卡回收失败-后台入库失败，卡片已移到回收箱");
        //            }
        //        }
        //        else
        //            TH.addOnceUI(BackExit.Exit);
        //    }
        //    Status.isWorking = true;
        //}
        //public void getBackCar2()
        //{
        //    Loading.show("正在回收卡片，请勿操作...");
        //    string error = "";
        //    TaskMore.Run(new Action(() => {
        //        int slot = 0;
        //        int ret = MS2.letCarBackToKC(atr1, ref slot);
        //        if (ret == 1)
        //            error = "卡回收失败，卡片已移到回收箱";
        //        else if (ret == 2)
        //        {
        //            error = "设备故障，卡回收失败，请联系管理员处理";
        //        }
        //        else
        //        {
        //            mjBack.add("slotno", slot, DataStyle.INT);
        //            string netError2 = null;
        //            AnalyzeJson aj2 = Network3.getJson(mjBack, "uploadCPCardInfo", out netError2);
        //            if (netError2 != null || (aj2 != null && aj2["statusCode"].ToString() != "200"))
        //            {
        //                if (MS2.putCarToReject(atr1, slot) != 0)
        //                {
        //                    error = "设备故障2，卡回收失败，请联系管理员处理";
        //                }
        //                else
        //                {
        //                    error = "卡回收失败-后台入库失败，卡片已移到回收箱";
        //                }
        //            }
        //            else
        //                error = "卡片已经存放回卡库";
        //        }
        //    })).ConfigureAwait(true);
        //    ShowTip.show(false, BackExit.Exit, error);
        //}
    }
}
