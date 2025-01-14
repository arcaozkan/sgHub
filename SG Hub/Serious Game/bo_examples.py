from bo_library import *


from pymongo import MongoClient
client = MongoClient("mongodb+srv://arcaozkan:MUuudblkb148@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
client2 = MongoClient("mongodb+srv://arcaozkan:MUuudblkb148@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");
db2 = client2["PatientGameDB"]
col2 = db2["PatientGameCollection"]

print("\n\n1D Sampling Example Started..\n"+"-*"*20)
# Gaussian Process Example for measured scores
game_results = col2.find()
Scores=[]
x_meas=[]
for result in game_results:
    Scores.append(result["Score"]) #Şu anda databasede 23 skor var ve hiç parametre yok, parametreleri de database'e yükleyip öyle çekmek lazım burayı değiştirmeye gerek yok aslında
    
#Scores=[1.40,1.62,1.69,1.74]
x_meas=[0.2,0.6,1,0.8,1,0.8,0.3,0.2,0.6,1,0.8,1,0.8,0.3,0.2,0.6,1,0.8,1,0.8,0.3,1,0]#Bunun yerine user'ın bütün önceki parametrelerinin listesi

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
user = {"_id": "102",  #Bu id her seferinde değişmeli
"best_param": x_arr[np.argmax(mu_post)],  

}  


col.insert_one(user)

# Fetching data  
#pprint.pprint(employees.find_one())

client.close();



print("\n\n2D Sampling Example Started..\n"+"-*"*20)


# GP 2 Dim. Example

theta=5

# x=[x1,x2]

x_meas=np.array([[0.3,0.3],[0.4,1.3],[1.,1.],[1.8,1.4]]) # parameter sets used in earlier iterations
Scores=[0.2,0.5,1,-0.4] # measured scores from earlier iterations

x1_size=30
x2_size=30

# Discrete search space for parameter sets
x1 = np.linspace(0, 2,x1_size )
x2 = np.linspace(0, 2, x2_size)
X1, X2 = np.meshgrid(x1, x2)

mean_arr=[]
var_arr=[]
x_arr=[]

for i in range(x2_size):
    for j in range(x1_size):
        mean_ss, var_ss = Gaussian_Process([[x1[j], x2[i]]], x_meas, Scores,theta)

        if i ==0 and j==0:
            mean_arr=np.array([mean_ss])
            var_arr=np.array([var_ss])
            x_arr = np.array([[x1[j], x2[i]]])

        else:
            mean_arr=np.vstack((mean_arr,mean_ss))
            var_arr =np.vstack((var_arr,var_ss))
            x_arr=np.vstack((x_arr,[x1[j], x2[i]]))


new_sample,_=acquisitionFunct(x_arr,Scores,mean_arr,var_arr)
print(f'Parameters for sampling in next iterations: {new_sample}')

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
print("\n2D Sampling Example Ended..\n"+"-*"*20)
