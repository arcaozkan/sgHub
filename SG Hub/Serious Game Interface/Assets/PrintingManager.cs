using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Sfs2X.Entities.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

public class PrintingManager : MonoBehaviour
{
    string path = null;
    List<float> smoothnessList = new List<float>() { };
    List<float> completenessList = new List<float>() { };
    List<float> durationList = new List<float>() { };
    List<float> steadinessList = new List<float>() { };
    private DatabaseAccess databaseAccess;
    public Dropdown graphDropdown;
    public Dropdown heatmapDropdown;

    public GameObject WindowGraph;
    string[] ssname = new string[3];

    public async void getScores(List<float> smoothnessList, List<float> completenessList, List<float> durationList, List<float> steadinessList)
    {
        
        var task = databaseAccess.GetScoresFromDatabase();
        var result = await task;

        foreach (var patient in result)
        {
            if (patient.gamePlayed == "Uzay Macerası:Meteor Yolculuğu" && patient.userID==ChosenPatient.patientID)
            {
                smoothnessList.Add(patient.smoothness);
                completenessList.Add(patient.completeness);
                durationList.Add(patient.duration);
                steadinessList.Add(patient.steadiness);
                if (smoothnessList.Count > 5)
                {
                    smoothnessList.RemoveAt(0);
                    completenessList.RemoveAt(0);
                    durationList.RemoveAt(0);
                    steadinessList.RemoveAt(0);
                }
            }
        }
        //ShowGraph(valueList, (int _i) => "Seans " + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        path = Application.dataPath + "/Rapor.pdf";
        getScores(smoothnessList,completenessList,durationList,steadinessList);
    }

    public void GenerateFile() {
        if (File.Exists(path))
            File.Delete(path);
        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            graphDropdown.value = 2;
            heatmapDropdown.value = 1;
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.NewPage();
            
            //TO TAKE SCREENSHOT
            string folderPath = "Assets/Screenshots/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 48f); //DOESNT WORK
            Paragraph p = new Paragraph(string.Format("UZAY MACERASI SON 5 SEANS RAPORU", font)); //iSFSObject.GetUtfString("TICKET_ID"
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            var screenshotName ="Screenshot_1.png";
            //if (File.Exists(folderPath + screenshotName))
                //File.Delete(folderPath + screenshotName);
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName);
            //END

            string imageURL = Application.dataPath + "/Screenshots/Screenshot_1.png";

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need

            jpg.ScaleToFit(420f, 360f);

            //Give space before image

            jpg.SpacingBefore = 10f;

            //Give some space after the image

            jpg.SpacingAfter = 1f;

            jpg.Alignment = Element.ALIGN_CENTER;

            document.Add(jpg);

            while (smoothnessList.Count < 5)
                smoothnessList.Add(0);
            p = new Paragraph(string.Format("\n\nSon 5 seansin smoothness skorlari : {0} - {1} - {2} - {3} - {4} \n Gelisme: %{5}", smoothnessList[0],
           smoothnessList[1], smoothnessList[2], smoothnessList[3], smoothnessList[4], LineFitter.smoothnessA)); //iSFSObject.GetUtfString("TICKET_ID"
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            graphDropdown.value = 3;
            heatmapDropdown.value = 2;
            //SS 2
            screenshotName = "Screenshot_2.png";
            //if (File.Exists(folderPath + screenshotName))
                //File.Delete(folderPath + screenshotName);
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName);
            //END

            imageURL = Application.dataPath + "/Screenshots/Screenshot_2.png";

            jpg = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need

            jpg.ScaleToFit(420f, 360f);

            //Give space before image

            jpg.SpacingBefore = 10f;

            //Give some space after the image

            jpg.SpacingAfter = 1f;

            jpg.Alignment = Element.ALIGN_CENTER;

            document.Add(jpg);

            while (completenessList.Count < 5)
                completenessList.Add(0);
            p = new Paragraph(string.Format("\n\nSon 5 seansin completeness skorlari : {0} - {1} - {2} - {3} - {4} \n Gelisme: %{5}", completenessList[0],
completenessList[1], completenessList[2], completenessList[3], completenessList[4], LineFitter.completenessA)); //iSFSObject.GetUtfString("TICKET_ID"
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            //SS 3
            graphDropdown.value = 4;
            heatmapDropdown.value = 3;
            screenshotName = "Screenshot_3.png";
            //if (File.Exists(folderPath + screenshotName))
                //File.Delete(folderPath + screenshotName);
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName);
            //END

            imageURL = Application.dataPath + "/Screenshots/Screenshot_3.png";

            jpg = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need

            jpg.ScaleToFit(420f, 360f);

            //Give space before image

            jpg.SpacingBefore = 10f;

            //Give some space after the image

            jpg.SpacingAfter = 1f;

            jpg.Alignment = Element.ALIGN_CENTER;

            document.Add(jpg);

            while (durationList.Count < 5)
                durationList.Add(0);
            p = new Paragraph(string.Format("\n\nSon 5 seansin hiz skorlari : {0} - {1} - {2} - {3} - {4} \n Gelisme: %{5}", durationList[0],
durationList[1], durationList[2], durationList[3], durationList[4], LineFitter.durationA)); //iSFSObject.GetUtfString("TICKET_ID"
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            //SS 4
            graphDropdown.value = 5;
            heatmapDropdown.value = 4;
            screenshotName = "Screenshot_4.png";
            //if (File.Exists(folderPath + screenshotName))
                //File.Delete(folderPath + screenshotName);
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName);
            //END

            imageURL = Application.dataPath + "/Screenshots/Screenshot_4.png";

            jpg = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need

            jpg.ScaleToFit(420f, 360f);

            //Give space before image

            jpg.SpacingBefore = 10f;

            //Give some space after the image

            jpg.SpacingAfter = 1f;

            jpg.Alignment = Element.ALIGN_CENTER;

            document.Add(jpg);

            while (steadinessList.Count < 5)
                steadinessList.Add(0);
            p = new Paragraph(string.Format("\n\nSon 5 seansin steadiness skorlari : {0} - {1} - {2} - {3} - {4} \n Gelisme: %{5}", steadinessList[0],
steadinessList[1], steadinessList[2], steadinessList[3], steadinessList[4], LineFitter.steadinessA*100)); //iSFSObject.GetUtfString("TICKET_ID"
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            
            for (int i = 0; i < 3; i++)
            {
                //SS 5
                if(ssname[i]==null)
                    ssname[i] = WindowGraph.GetComponent<Window_Graph>().regionName;
                screenshotName = "Screenshot_" + (5+i).ToString() + ".png";
                //if (File.Exists(folderPath + screenshotName))
                //File.Delete(folderPath + screenshotName);
                ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
                Debug.Log(folderPath + screenshotName);
                //END

                imageURL = Application.dataPath + "/Screenshots/" + screenshotName;

                jpg = iTextSharp.text.Image.GetInstance(imageURL);

                //Resize image depend upon your need

                jpg.ScaleToFit(420f, 360f);

                //Give space before image

                jpg.SpacingBefore = 10f;

                //Give some space after the image

                jpg.SpacingAfter = 1f;

                jpg.Alignment = Element.ALIGN_CENTER;

                document.Add(jpg);



                p = new Paragraph(string.Format("\n\n" + ssname[i] + "bölgesindeki gelisme grafigi\n")); //iSFSObject.GetUtfString("TICKET_ID"
                p.Alignment = Element.ALIGN_CENTER;
                document.Add(p);
            }
            

            string text="";
            if (LineFitter.smoothnessA > 0.5)
                text += "Oyuncu hareketlerin yumusakligi konusunda büyük gelisme kaydetmistir.\n";
            if (LineFitter.smoothnessA > 0.2)
                text += "Oyuncu hareketlerin yumusakligi konusunda hafif gelisme kaydetmistir.\n";
            if (LineFitter.smoothnessA < 0.2 && LineFitter.smoothnessA > -0.2)
                text += "Oyuncu hareketlerin yumusakligi konusunda ne gelisme ne de gerileme kaydetmistir.\n";
            if (LineFitter.smoothnessA < -0.2)
                text += "Oyuncu hareketlerin yumusakligi konusunda gerileme yasamistir.\n";

            if (LineFitter.completenessA > 0.5)
                text += "Oyuncu hareketlerin tamamlanması konusunda büyük gelisme kaydetmistir.\n";
            if (LineFitter.completenessA > 0.2)
                text += "Oyuncu hareketlerin tamamlanması konusunda hafif gelisme kaydetmistir.\n";
            if (LineFitter.completenessA < 0.2 && LineFitter.completenessA > -0.2)
                text += "Oyuncu hareketlerin tamamlanması konusunda ne gelisme ne de gerileme kaydetmistir.\n";
            if (LineFitter.completenessA < -0.2)
                text += "Oyuncu hareketlerin tamamlanması konusunda gerileme yasamistir.\n";

            if (LineFitter.durationA > 0.5)
                text += "Oyuncu hareketlerin hızı konusunda büyük gelisme kaydetmistir.\n";
            if (LineFitter.durationA > 0.2)
                text += "Oyuncu hareketlerin hızı konusunda hafif gelisme kaydetmistir.\n";
            if (LineFitter.durationA < 0.2 && LineFitter.durationA > -0.2)
                text += "Oyuncu hareketlerin hızı konusunda ne gelisme ne de gerileme kaydetmistir.\n";
            if (LineFitter.durationA < -0.2)
                text += "Oyuncu hareketlerin hızı konusunda gerileme yasamistir.\n";

            if (LineFitter.steadinessA > 0.5)
                text += "Oyuncu hareketlerin stabilliği konusunda büyük gelisme kaydetmistir.\n";
            if (LineFitter.steadinessA > 0.2)
                text += "Oyuncu hareketlerin stabilliği konusunda hafif gelisme kaydetmistir.\n";
            if (LineFitter.steadinessA < 0.2 && LineFitter.steadinessA > -0.2)
                text += "Oyuncu hareketlerin stabilliği konusunda ne gelisme ne de gerileme kaydetmistir.\n";
            if (LineFitter.steadinessA < -0.2)
                text += "Oyuncu hareketlerin stabilliği konusunda gerileme yasamistir.\n";



            p = new Paragraph(string.Format("\n\n\n"+text));
                p.Alignment = Element.ALIGN_CENTER;
                document.Add(p);
        

            document.Close();
            writer.Close();
        }



        //StreamWriter writer = new StreamWriter(path, false);
        //writer.WriteLine(string.Format("Ticket Id : {0}",iSFSObject.GetUtfString("TICKET_ID")));
        //var betting = iSFSObject.GetSFSArray("BET_DETAILS");
        //for (int i = 0; i< betting.Count;i++)
        //    writer.WriteLine(string.Format("Bet Number : {0}     BetAmount : {1}", betting.GetSFSObject(i).GetUtfString("BET_NUM"), betting.GetSFSObject(i).GetDouble("BET_AMOUNT")));
        //writer.Close();
        
        PrintFiles();
    }

    void PrintFiles()
    {
        Debug.Log(path);
        if (path == null)
            return;

        if (File.Exists(path))
        {
            Debug.Log("file found");
            //var startInfo = new System.Diagnostics.ProcessStartInfo(path);
            //int i = 0;
            //foreach (string verb in startInfo.Verbs)
            //{
            //    // Display the possible verbs.
            //    Debug.Log(string.Format("  {0}. {1}", i.ToString(), verb));
            //    i++;
            //}
        }
        else
        {
            Debug.Log("file not found");
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = path;
        //process.StartInfo.Verb = "print";

        process.Start();
        //process.WaitForExit();

    }


}
