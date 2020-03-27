using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using YTH.Functions;
using YTH.Functions.MSDLL;
using YTH.NHNet;

namespace YTH
{
    class CheckSelf
    {
        public static void checkSelf()
        {

            //while(true)
            //{
            //    Network3.login();
            //    int a = 0;
            //}
            return;
            try
            {
                Status.isWorking = true;
                Thread.Sleep(15000);
               
                //MS2.setLogFileName_Old("设备自检");
                string printStatus = "";
                bool zkj = false;
                bool dyj = false;
                int canPrintNum = 0;
                string icError = MS2.checkIC();
                string idError = MS2.checkSFZ();
                string qrError = MS2.checkScanner();
                string cameraError = MS2.checkSam();
                string printError = Print.checkPrint(ref printStatus);
                string zkjError = MS2.ResetAllAndGetStatus(out zkj, out dyj, ref canPrintNum);


                if (zkjError == null)
                    zkjError = "正常";
                if (icError == null)
                    icError = "正常";
                if (idError == null)
                    idError = "正常";
                if (qrError == null)
                    qrError = "正常";
                if (cameraError == null)
                    cameraError = "正常";
                if (printError == null)
                    printError = "正常";
                string result = "1";
                if (zkjError != "正常"
                    || icError != "正常"
                    || idError != "正常"
                    || qrError != "正常"
                    || printError != "正常"
                    || cameraError != "正常")
                    result = "0";

                #region
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
                #endregion
                MakeJson mj = new MakeJson();
                mj.add("checkState", result, DataStyle.STR);
                mj.add("cardBox", zkjError, DataStyle.STR);
                mj.add("wheelDisc", zkjError, DataStyle.STR);
                mj.add("filpMachine", zkjError, DataStyle.STR);
                mj.add("eleCar", zkjError, DataStyle.STR);
                mj.add("icReader", icError, DataStyle.STR);
                mj.add("cardReader", idError, DataStyle.STR);
                mj.add("a4printer", "正常", DataStyle.STR);
                mj.add("voucherPrinter", printError, DataStyle.STR);
                mj.add("camera", cameraError, DataStyle.STR);
                mj.add("qrCode", qrError, DataStyle.STR);

                string error = null;
                tools.AnalyzeJson aj = Network3.getJson(mj, "DevCheck", out error);

                //保存色带信息
                #region

                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
ribState	String	是	色带使用状态 2余量充足(可制卡量>30张)、1即将耗尽(<=30张)、 0已耗尽(色带剩余可制卡量<=3）

                 */
                #endregion
                string have = "0";
                if (canPrintNum > 30)
                    have = "2";
                else if (canPrintNum <= 30 && canPrintNum > 0)
                    have = "1";
                MakeJson mj2 = new MakeJson();
                mj2.add("ribState", have, DataStyle.STR);
                string error2 = null;
                tools.AnalyzeJson aj2 = Network3.getJson(mj2, "saveDevRibbon", out error2);


                //保存凭条打印机信息
                #region
                /*
channelcode	String	是	渠道编码(参考3.1渠道类型编码说明 channelCode)
deviceid	String	是	设备编码
tokenid	String	是	权限验证码，前端主页面第一次进入会请求返回的key
equipmentNo	String	是	设备id/编码
priState	String	是	打印机使用状态 1正常、0缺纸
                 */
                #endregion
                MakeJson mj3 = new MakeJson();
                //mj3.add("priState", ((printStatus != null && printStatus.Replace(" ", "") == "") ? "1" : "0"), DataStyle.STR);
                mj3.add("priState",  "1",DataStyle.STR);//Test
                string error3 = null;
                tools.AnalyzeJson aj3 = Network3.getJson(mj3, "saveDevPrinter", out error3);


            }
            catch (Exception e)
            {
                Log.AddLog("设备自检", e.ToString());
            }
            finally
            {
                Status.isWorking = false;
            }
        }

    }
}
