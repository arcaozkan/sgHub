using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DatabaseAccess : MonoBehaviour
{



    MongoClient client = new MongoClient("mongodb+srv://arcaozkan:passWord125@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database, database2,database3,database4,database5;
    IMongoCollection<Therapist_Patient> collection2;
    IMongoCollection<PatientGameInfo> collection;
    IMongoCollection<TherapistInfo> collection3;
    IMongoCollection<Games> collection4;
    IMongoCollection<Parameters> collection5;
    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("PatientGameDB");
        collection = database.GetCollection<PatientGameInfo>("PatientGameCollection");

        database2 = client.GetDatabase("Therapist_PatientsDB");
        collection2 = database2.GetCollection<Therapist_Patient>("Therapist_PatientsCollection");

        database3 = client.GetDatabase("TherapistInfoDB");
        collection3 = database3.GetCollection<TherapistInfo>("TherapistInfoCollection");

        database4 = client.GetDatabase("GamesDB");
        collection4 = database4.GetCollection<Games>("GamesCollection");

        database5 = client.GetDatabase("PatientGameProcessedDB");
        collection5 = database5.GetCollection<Parameters>("PatientGameProcessedCollection");

    }

    public async void SaveScoreToDataBase(int userID, string userName, int score, double[,] fMissedAlien)
    {
        await collection.InsertOneAsync(new PatientGameInfo { userID = userID, UserName = userName, Score = score, FirstHeatMap = fMissedAlien });
    }

    public async void SaveParametersToDatabase(int userID, double best_param1, double best_param2, double best_param3,string gamename)
    {
        await collection5.InsertOneAsync(new Parameters{ userID = userID, best_param1 = best_param1 , best_param2 = best_param2, best_param3 = best_param3, game =gamename });
    }

    public async void SavePatientToDataBase(string therapist_username, int patient_id, string patient_name, int age, string gender, 
                                            double height, double weight, string special_need, string therapy_type, string therapy_start_date,string additional_notes)
    {
        await collection2.InsertOneAsync(new Therapist_Patient { therapist_username = therapist_username, patient_id = patient_id, patient_name = patient_name,age=age,gender=gender,
                                   height=height,weight=weight,special_need=special_need,therapy_type=therapy_type, therapy_start_date= therapy_start_date,dateint=Convert.ToInt32(therapy_start_date.Split('.')[2]),
            additional_notes= additional_notes});
    }

    public async Task<List<Therapist_Patient>> GetPatientsFromDatabase()
    {
        var builder = Builders<Therapist_Patient>.Filter; //Burda filterı therapist username'ine göre yaparsak sadece o terapistin hastaları çıkar
        var filter = builder.Eq(f => f.therapist_username, LogInTherapist.username_verified);
        var allPatientsTask = collection2.Find(filter).ToList();

    List<Therapist_Patient> patientInfos = new List<Therapist_Patient>();
        foreach (var patient in allPatientsTask)
        {
            
            patientInfos.Add(patient);
        }
        return patientInfos;
    }

    public async Task<List<TherapistInfo>> GetTherapistFromDatabase(string username,string password)
    {
        var builder = Builders<TherapistInfo>.Filter; 
        var filter= builder.Eq(f => f.therapist_username, username) & builder.Eq(f => f.password, password);
        var allTherapistsTask = collection3.Find(filter).ToList();
        List<TherapistInfo> TherapistInfos = new List<TherapistInfo>();

        foreach (var therapist in allTherapistsTask)
        {
            TherapistInfos.Add(therapist);
        }
        return TherapistInfos;

    }


    public async Task<List<Games>> GetGamesFromDatabase()
    {
        var builder = Builders<Games>.Filter;
        var allGamesTask = collection4.FindAsync(new BsonDocument());
        var gamesAwaited = await allGamesTask;
        List<Games> GameInfos = new List<Games>();

        foreach (var game in gamesAwaited.ToList())
        {
            GameInfos.Add(game);
        }
        return GameInfos;

    }

    public async Task<List<Parameters>> GetParametersFromDatabase(string gamename)
    {
        var builder = Builders<Parameters>.Filter;
        var filter = builder.Eq(f => f.userID, ChosenPatient.patientID) & builder.Eq(f => f.game, gamename);
        var allParametersTask = collection5.Find(filter).ToList();
        List<Parameters> ParameterInfos = new List<Parameters>();

        foreach (var parameter in allParametersTask)
        {
            ParameterInfos.Add(parameter);
            Debug.Log(parameter);
        }
        return ParameterInfos;

    }


    public async Task<List<PatientGameInfo>> GetScoresFromDatabase()
    {


        var builder = Builders<PatientGameInfo>.Filter;
        var filter = builder.Eq(f => f.userID, ChosenPatient.patientID);
        var allPatientsTask = collection.Find(filter).ToList();
        List<PatientGameInfo> patientInfos = new List<PatientGameInfo>();
        foreach (var score in allPatientsTask)
        {
            patientInfos.Add(score);
        }
        return patientInfos;
    }


    public async Task<List<PatientGameInfo>> Query1(string game,bool lessmore, int score)
    {
        var builder = Builders<PatientGameInfo>.Filter;

        var filter = builder.Eq(f => f.gamePlayed, game) & builder.Gte(f => f.Score, score);
        if (lessmore)
            filter = builder.Eq(f => f.gamePlayed, game) & builder.Lt(f => f.Score, score);

        var allPatientsTask = collection.Find(filter).ToList();
        List<PatientGameInfo> PatientGameInfos = new List<PatientGameInfo>();

        foreach (var patient in allPatientsTask)
        {
            PatientGameInfos.Add(patient);
        }
        return PatientGameInfos;

    }

    public async Task<List<Therapist_Patient>> Query3(int age, string greater, bool lessmore, int year)
    {
        var builder = Builders<Therapist_Patient>.Filter;
        int thisyear = System.DateTime.Now.Year;
        var filter = builder.Gte(f => f.age, age) & builder.Lte(f => f.dateint, thisyear-year);
        if (lessmore) {
           filter = builder.Gte(f => f.age, age) & builder.Gte(f => f.dateint, thisyear-year);
        }
        if (greater == "küçük")
        {
            filter = builder.Lt(f => f.age, age) & builder.Lte(f => f.dateint, thisyear - year);
            if (lessmore)
                filter = builder.Lt(f => f.age, age) & builder.Gte(f => f.dateint, thisyear - year);
        }

        var allPatientsTask = collection2.Find(filter).ToList();
        List<Therapist_Patient> Therapist_Patient_Infos = new List<Therapist_Patient>();

        foreach (var patient in allPatientsTask)
        {
            Therapist_Patient_Infos.Add(patient);
        }
        return Therapist_Patient_Infos;

    }
    /*public async Task<List<Therapist_Patient>> Query4(int age, string greater, bool lessmore, int year)
    {
        var builder = Builders<Therapist_Patient>.Filter;
        int thisyear = System.DateTime.Now.Year;
        var filter = builder.Gte(f => f.age, age) & builder.Lte(f => f.dateint, thisyear - year);
        if (lessmore)
        {
            filter = builder.Gte(f => f.age, age) & builder.Gte(f => f.dateint, thisyear - year);
        }

        var allPatientsTask = collection2.Find(filter).ToList();
        List<Therapist_Patient> Therapist_Patient_Infos = new List<Therapist_Patient>();

        foreach (var patient in allPatientsTask)
        {
            Therapist_Patient_Infos.Add(patient);
        }
        return Therapist_Patient_Infos;

    }*/


}
[BsonIgnoreExtraElements]
public class PatientGameInfo
{
    public int userID { get; set; }
    public string UserName { get; set; }
    public int Score { get; set; }
    public double[,] FirstHeatMap { get; set; }
    public int[,] FirstCaughtHeatMap { get; set; }
    public float[,,] FirstCaughtAlienStats { get; set; }
    public int[,] FirstMissedHeatMap { get; set; }
    public double[,] SecondHeatMap { get; set; }
    public double[] ThirdHeatMap { get; set; }
    public float smoothness { get; set; }
    public float completeness { get; set; }
    public float duration { get; set; }
    public float steadiness { get; set; }
    public string Date { get; set; }
    public string gamePlayed { get; set; }
    public double param1 { get; set; }
    public double param2 { get; set; }
    public double param3 { get; set; }
    public double time { get; set; }
}

[BsonIgnoreExtraElements]
public class Games
{
    public string name { get; set; }

    public string parameters { get; set; }
}

[BsonIgnoreExtraElements]
public class Parameters
{
    public int userID { get; set; }
    public double best_param1 { get; set; }
    public double best_param2  { get; set; }
    public double best_param3 { get; set; }
    public string game { get; set; }
}


[BsonIgnoreExtraElements]
public class Therapist_Patient
{
    public string therapist_username { get; set; }
    public int patient_id { get; set; }
    public string patient_name { get; set; }
    public int age { get; set; }
    public string gender { get; set; }
    public double height { get; set; }
    public double weight { get; set; }
    public string special_need { get; set; }
    public string therapy_type { get; set; }
    public string therapy_start_date { get; set; }
    public int dateint { get; set; }
    public string additional_notes { get; set; }

}
[BsonIgnoreExtraElements]
public class TherapistInfo
{
    public string therapist_username { get; set; }
    public string password { get; set; }
}