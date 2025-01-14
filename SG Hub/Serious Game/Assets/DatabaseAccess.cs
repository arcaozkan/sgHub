using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System;

using MongoDB.Bson.Serialization.Attributes;
public class DatabaseAccess : MonoBehaviour
{



    MongoClient client = new MongoClient("mongodb+srv://arcaozkan:passWord125@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database, database2,database3,database4;

    IMongoCollection<TherapistInfo> collection4;
    IMongoCollection<PatientGameProcessedInfo> collection3;
    IMongoCollection<Therapist_Patient> collection2;
    IMongoCollection<PatientInfo> collection;
  
    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("PatientGameDB");
        collection = database.GetCollection<PatientInfo>("PatientGameCollection");

        database2= client.GetDatabase("Therapist_PatientsDB");
        collection2 = database2.GetCollection<Therapist_Patient>("Therapist_PatientsCollection");

        database3 = client.GetDatabase("PatientGameProcessedDB");
        collection3 = database3.GetCollection<PatientGameProcessedInfo>("PatientGameProcessedCollection");

        database4 = client.GetDatabase("TherapistInfoDB");
        collection4 = database4.GetCollection<TherapistInfo>("TherapistInfoCollection");

    }

    public async void SaveScoreToDataBase(int userID,int score,double parameter1,double parameter2, double parameter3, float timer, float smoothness, float completeness, float duration, float steadiness, float[,] fRatioAlien, int[,] fCaughtAlien,float[,,] fCaughtAlienStats, float[,,] fMissedAlienStats,
        int[,] fMissedAlien, bool liked,string date,string gameplayed)
    {
        await collection.InsertOneAsync(new PatientInfo { userID= userID,  Score=score, param1=parameter1, param2=parameter2, param3 = parameter3, //speed,size
            time = timer,
            smoothness = smoothness,
            completeness=completeness,
            duration=duration,
            steadiness=steadiness,
            FirstHeatMap = fRatioAlien,
            FirstCaughtHeatMap = fCaughtAlien,
            FirstMissedHeatMap = fMissedAlien,
            FirstCaughtAlienStats=fCaughtAlienStats,
            FirstMissedAlienStats = fMissedAlienStats,
            gameLiked =liked,Date=date,gamePlayed=gameplayed });
    }

    public async void SaveScoreToDataBaseG2(int userID,  int score, double parameter1, double parameter2,double parameter3, float timer, float[,] heatmapAlien, bool liked, string date, string gameplayed,float smoothness,float completeness, float duration, float steadiness)
    {
        await collection.InsertOneAsync(new PatientInfo
        {
            userID = userID,
            Score = score,
            param1 = parameter1,
            param2 = parameter2,
            param3 = parameter3,
            time = timer,
            SecondHeatMap = heatmapAlien,
            gameLiked = liked,
            Date = date,
            gamePlayed = gameplayed,
            smoothness = smoothness,
            completeness = completeness,
            duration = duration,
            steadiness = steadiness
        }); ;
    }

    public async void SaveScoreToDataBaseG3(int userID, int score, double parameter1, double parameter2, double parameter3, float timer, float[] calcHeatmap, bool liked, string date, string gameplayed, float smoothness, float completeness, float duration, float steadiness)
    {
        await collection.InsertOneAsync(new PatientInfo
        {
            userID = userID,
            Score = score,
            param1 = parameter1,
            param2 = parameter2,
            param3=parameter3,
            time=timer,
            ThirdHeatMap = calcHeatmap,
            gameLiked = liked,
            Date = date,
            gamePlayed = gameplayed,
            smoothness = smoothness,
            completeness = completeness,
            duration = duration,
            steadiness = steadiness
        });
    }


    public async void SaveScoreToDataBase2(string userName, int patient_id,string patient_name)
    {
        await collection2.InsertOneAsync(new Therapist_Patient { therapist_username = userName, patient_id= patient_id, patient_name = patient_name });
    }

    public async Task<List<PatientInfo>> GetScoresFromDatabase()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;

        List<PatientInfo> patientInfos = new List<PatientInfo>();
        foreach(var score in scoresAwaited.ToList())
        {
             //patientInfos.Add(Deserialize(score.ToString())); //BUNU AÇINCA HATA VERÝYOR, deserialize olayý outdated bunu da öbürleri gibi yap
        }
        return patientInfos;
    }

    public async Task<List<PatientGameProcessedInfo>> GetParameterFromDatabase()
    {
        var builder = Builders<PatientGameProcessedInfo>.Filter;
        var filter = builder.Eq(f => f.userID, ChosenPatient.patientID) & builder.Eq(f => f.game, ChosenPatient.chosenGame); //Patient ID'si seçilen patient olanlarý al
        var allParamTask = collection3.Find(filter).ToList();
        List<PatientGameProcessedInfo> ParameterInfos = new List<PatientGameProcessedInfo>();

        foreach (var param in allParamTask)
        {
            Debug.Log(ChosenPatient.patientID);
            ParameterInfos.Add(param);
        }
        return ParameterInfos;

    }

    public async Task<List<TherapistInfo>> GetTherapistFromDatabase(string username, string password)
    {
        var builder = Builders<TherapistInfo>.Filter;
        var filter = builder.Eq(f => f.therapist_username, username) & builder.Eq(f => f.password, password);
        var allTherapistsTask = collection4.Find(filter).ToList();
        List<TherapistInfo> TherapistInfos = new List<TherapistInfo>();

        foreach (var therapist in allTherapistsTask)
        {
            TherapistInfos.Add(therapist);
        }
        return TherapistInfos;

    }

    public async Task<List<Therapist_Patient>> GetPatientsFromDatabase()
    {
        var builder = Builders<Therapist_Patient>.Filter; //Burda filterý therapist username'ine göre yaparsak sadece o terapistin hastalarý çýkar
        var filter = builder.Eq(f => f.therapist_username, LogInTherapist.username_verified);
        var allPatientsTask = collection2.Find(filter).ToList();

        List<Therapist_Patient> patientInfos = new List<Therapist_Patient>();
        foreach (var patient in allPatientsTask)
        {

            patientInfos.Add(patient);
        }
        return patientInfos;
    }



    private PatientInfo Deserialize(string rawJson)
    {
        var patientInfo = new PatientInfo();
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4);
        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}") - stringWithoutID.IndexOf(":") - 3);
        //patientInfo.UserName = username;
        patientInfo.Score = Convert.ToInt32(score);
        return patientInfo;
    }
}
[BsonIgnoreExtraElements]
public class PatientInfo
{
    public int userID { get; set; }
    public int Score { get; set; }
    public double param1 { get; set; }
    public double param2 { get; set; }
    public double param3 { get; set; }
    public float time { get; set; }
    public float smoothness { get; set; }
    public float completeness { get; set; }
    public float duration { get; set; }
    public float steadiness { get; set; }
    public float[,] FirstHeatMap { get; set; }
    public int[,] FirstCaughtHeatMap { get; set; }
    public int[,] FirstMissedHeatMap { get; set; }
    public float[,] SecondHeatMap { get; set; }
    public float[] ThirdHeatMap { get; set; }
    public bool gameLiked { get; set; }
    public string Date { get; set; }
    public string gamePlayed { get; set; }
    public float[,,] FirstCaughtAlienStats { get; set; }
    public float[,,] FirstMissedAlienStats { get; set; }
}
[BsonIgnoreExtraElements]
public class Therapist_Patient
{
    public string therapist_username { get; set; }
    public int patient_id{ get; set; }
    public string patient_name { get; set; }
}

[BsonIgnoreExtraElements]
public class PatientGameProcessedInfo
{
    public int userID { get; set; }
    public double best_param1 { get; set; }
    public double best_param2 { get; set; }
    public double best_param3 { get; set; }
    public string game { get; set; }
}

[BsonIgnoreExtraElements]
public class TherapistInfo
{
    public string therapist_username { get; set; }
    public string password { get; set; }
}