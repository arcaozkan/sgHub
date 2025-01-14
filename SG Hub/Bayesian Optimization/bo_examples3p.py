from bo_library import *


from pymongo import MongoClient
client = MongoClient("mongodb+srv://arcaozkan:MUuudblkb148@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
client2 = MongoClient("mongodb+srv://arcaozkan:MUuudblkb148@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
db2 = client2["PatientGameDB"] #uzaymacerası
col2 = db2["PatientGameCollection"] #uzaymacerası
db3 = client2["ens_game"] #zırhlıküçük
col3 = db3["patient"] #zırhlıküçük

"""
#BURASI ZIRHLI KÜÇÜK İÇİN 
print("\n\n1D Sampling Example Started..\n"+"-*"*20)
# Gaussian Process Example for measured scores
game_results = col3.find()
Scores=[]
x_meas=[]
for result in game_results:
    timeint=int(result["time"][0:2])*60+int(result["time"][5:7]) #how many seconds
    score=int(result["status"])/timeint
    Scores.append(score)
    if result["difficulty"] == "Düşük Yer Çekimi": #Şu anda düşük yer çekimini 0, yükseği 1 alıyor, daha continous bişey mi yapsak
        x_meas.append(0)
    elif result["difficulty"]  == "Yüksek Yer Çekimi":
        x_meas.append(1) 
#Scores=[1.40,1.62,1.69,1.74]

print(Scores)
print(x_meas)
theta=10
x_arr=np.linspace(0,1,100)
mu_post,var_post=Gaussian_Process(x_arr,x_meas,Scores,10,noise=0.05)

new_sample,_=acquisitionFunct(x_arr,Scores,mu_post,var_post)
plt.fill_between(x_arr, mu_post + var_post, mu_post - var_post, alpha=0.5)
plt.plot(x_arr, mu_post)
plt.plot(x_meas,Scores, 'o', alpha=0.4)
plt.show()

print("Best parameter to use is:",x_arr[np.argmax(mu_post)])
print("\n1D Sampling Example Ended..\n"+"-*"*20)

# Creating database  
db = client["PatientGameProcessedDB"]
col = db["PatientGameProcessedCollection"]
user = {"_id": "103",  #Bu id her seferinde değişmeli
"best_speed_param": x_arr[np.argmax(mu_post)],  

}  


col.insert_one(user)

client.close();
"""



 #BURASI UZAY MACERASI İÇİN SONUNA KADAR UNCOMMENTLE
print("\n\n3D Sampling Example Started..\n"+"-*"*20)


# GP 2 Dim. Example

theta=10

gamename="Uzay Macerası:Meteor Yolculuğu"
patientID=3
game_results = col2.find({"gamePlayed": gamename,"userID":patientID})
Scores=[]
x_meas=[]
for result in game_results:
    score=result["smoothness"]*25+result["completeness"]*25+result["duration"]*25+result["steadiness"]*25
    Scores.append(score) 
    x_meas.append([result["param1"],result["param2"],result["param3"]])
    print(result["param1"],result["param2"],result["param3"])
    print(result["Score"])
    
#x_meas=np.array([[0.3,0.3],[0.4,1.3],[1.,1.],[1.8,1.4],[0.3,0.3],[0.3,0.3],[0.3,0.3],[0.3,0.3]]) # parameter sets used in earlier iterations
#Scores=[0.2,0.5,1,0.4,0.3,0.5,0.8,0.1] # measured scores from earlier iterations
x_meas=np.array(x_meas)
x1_size=50
x2_size=50
x3_size=50
# Discrete search space for parameter sets
x1 = np.linspace(0, 1,x1_size )
x2 = np.linspace(0, 1, x2_size)
x3 = np.linspace(0, 1, x3_size)
X1, X2,X3 = np.meshgrid(x1, x2,x3)

mean_arr=[]
var_arr=[]
x_arr=[]
for k in range(x3_size):
    for i in range(x2_size):
        for j in range(x1_size):
            mean_ss, var_ss = Gaussian_Process([[x1[j], x2[i],x3[k]]], x_meas, Scores,theta)

            if i ==0 and j==0 and k==0:
                mean_arr=np.array([mean_ss])
                var_arr=np.array([var_ss])
                x_arr = np.array([[x1[j], x2[i],x3[k]]])

            else:
                mean_arr=np.vstack((mean_arr,mean_ss))
                var_arr =np.vstack((var_arr,var_ss))
                x_arr=np.vstack((x_arr,[x1[j], x2[i],x3[k]]))


new_sample,_=acquisitionFunct(x_arr,Scores,mean_arr,var_arr)
print(f'Parameters for sampling in next iterations: {new_sample}') #BU SATIR PARAMETERS TO USE

M=mean_arr.reshape(x2_size,-1)
V=var_arr.reshape(x2_size,-1)

fig, ax = plt.subplots(1,2,subplot_kw={'projection': '3d'})

ax[0].plot_surface(X1, X2, M, rstride=1, cstride=1,
                cmap='winter', edgecolor='none',alpha=0.5)
ax[0].scatter(x_meas[:,0],x_meas[:,1],Scores,c='orange',alpha=1)

ax[0].set_title('Mean Plot')
ax[0].set_xlabel('x1')
ax[0].set_ylabel('x2')
ax[0].set_zlabel('y (E(y_*))')

ax[1].plot_surface(X1, X2, V, rstride=1, cstride=1,
                cmap='winter', edgecolor='none',alpha=0.5)
ax[1].set_title('Variance Plot')
ax[1].set_xlabel('x1')
ax[1].set_ylabel('x2')
ax[1].set_zlabel('y (Var(y_*))')

plt.show()

db = client["PatientGameProcessedDB"]
col = db["PatientGameProcessedCollection"]
#BURAYA IF USER ALREADY EXİSTS, CHANGE PARAMS KOYALIM
user = {"userID": patientID,
"best_param1": new_sample[0],
"best_param2": new_sample[1],
"best_param3": new_sample[2],
"game": gamename

}  


col.insert_one(user)

client.close();

print("\n2D Sampling Example Ended..\n"+"-*"*20)

