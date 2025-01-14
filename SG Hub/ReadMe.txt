Files in SG Hub:

-Bayesian Optimization folder contains 2 parameter and 3 parameter codes of Bayesian Optimization. 

	Database connection has to be established using MongoDb Client in the beginning with username and password.
	Example:
		client = MongoClient("mongodb+srv://arcaozkan:passWord125@cluster0.nkqth.mongodb.net/?retryWrites=true&w=majority");

	The "gamename" and "patientID" parameters have to be changed according to the game.

	The parameter and evaluation metric names and calculations can also be changed according to the game.

	bo_library file contains relevant functions developed by Harun Tolasa.

-Example Databases folder contains the exported version of some of my databases that was used for generating a knowledge graph.

	More detailed information can be found in my thesis, A Hybrid AI System for the Education of Children with Cerebral Palsy.

-Saves folder contains private information of the child saved after a game session of Uzay Macerası is finished. 

	This information is mapped with the anonymized patients database using the userID's.

	The information stored can be adjusted from PatientsInDropdown.cs file in Uzay Macerası.

-Serious Game folder contains Uzay Macerası Game Suite consisting of 3 games.
	
	DatabaseAccess.cs file has to be adjusted according to your database for the games to work.
	
	After logging in, you can add a user and choose them from the dropdown to start a game for them. 

	Each game contains several adjustable parameters, these parameters are set to the optimal values provided by the 
	Bayesian optimization algorithm, stored in PatientGameProcessedDB.

	The evaluation metrics are all calculated in PlayerController.cs for Meteor Yolculuğu, and in DragObject2.cs and 3 for the other games.
	
	The data that will be saved to the database at the end of the session are stored in FirstPartData.cs, SecondPartData.cs and ThirdPartData.cs

-Serious Game Interface folder contains SG Hub Interface.

	Database connection has to be established in the same way described above.

	Game Recommendation feature is not implemented.

	Query Answering feature is incomplete, and the files contains some bits about connecting to knowledge graphs,
	but some of the queries work by retrieving data only from the databases.

	Game Setup feature enables the therapist to change default value of the parameters and stores the data in PatientGameProcessedDB.

	Reports feature contains a scatter graph for each game. A line can be fit to this graph to display an improvement score
	using linear regression algorithm found in LineFitter.cs.

	There is also a heatmap for each game, found in Heatmap.cs, Heatmap2.cs and Heatmap3.cs. The heatmap for Meteor Yolculuğu, heatmap1, can show
	individual session results along with improvements at each region for the chosen metric.

	If a new game were to be integrated to the system, the heatmap has to be adjusted to display the information needed.

	The patient interface has to be connected to the game suite for the games to be played from this interface. They can still be played through the
	project in the serious game folder.

-Hand Detection.py

	Contains the hand detection algorithm used for Uzay Macerası games.

	Has to be run manually before starting the game as of now.

	More information about the algorithm can be found in the commented youtube link in beginning of the file.
	

Feel free to contact me at +90 539 360 40 44 or aarcaa148@gmail.com if you have any questions.

-Arca Özkan
	

	



	
	

