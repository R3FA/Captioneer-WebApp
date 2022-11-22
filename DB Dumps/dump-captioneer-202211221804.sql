-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: localhost    Database: captioneer
-- ------------------------------------------------------
-- Server version	8.0.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20221109235306_InitialMigration','6.0.11'),('20221110205311_FixAdmin','6.0.11'),('20221114123807_AddTitles','6.0.11'),('20221114192045_FixTVShowYear','6.0.11'),('20221114204030_RemoveProperties','6.0.11'),('20221114205353_AddedProperties','6.0.11'),('20221114222520_NullableMapsCoordinates','6.0.11'),('20221115161440_AddIMDBID','6.0.11'),('20221115161838_ChangeYearToString','6.0.11'),('20221115163605_ChangeMovieShowTitle','6.0.11'),('20221116154444_NullableEpisodeCount','6.0.11'),('20221116154735_ChangeYearLength','6.0.11'),('20221117153110_ChangeSeason','6.0.11'),('20221117165924_EditEpisode','6.0.11'),('20221117172539_AddEpisodeNumber','6.0.11'),('20221117183102_AddEpisodeNumber','6.0.11'),('20221122170016_AddUsersLanguages','6.0.11');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actormovies`
--

DROP TABLE IF EXISTS `actormovies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actormovies` (
  `ActorID` int NOT NULL,
  `MovieID` int NOT NULL,
  PRIMARY KEY (`ActorID`,`MovieID`),
  KEY `IX_ActorMovies_MovieID` (`MovieID`),
  CONSTRAINT `FK_ActorMovies_Actors_ActorID` FOREIGN KEY (`ActorID`) REFERENCES `actors` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActorMovies_Movies_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `movies` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actormovies`
--

LOCK TABLES `actormovies` WRITE;
/*!40000 ALTER TABLE `actormovies` DISABLE KEYS */;
INSERT INTO `actormovies` VALUES (25,16),(26,16),(27,16),(25,17),(33,17),(34,17),(30,18),(31,18),(32,18),(10,19),(11,19),(12,19),(10,20),(11,20),(35,20),(36,21),(37,21),(10,22),(11,22),(35,22),(38,23),(39,23),(40,23),(41,24),(42,24),(43,24),(27,25),(65,25),(66,25),(67,26),(13,27),(68,27),(69,27);
/*!40000 ALTER TABLE `actormovies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actors`
--

DROP TABLE IF EXISTS `actors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actors` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Surname` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actors`
--

LOCK TABLES `actors` WRITE;
/*!40000 ALTER TABLE `actors` DISABLE KEYS */;
INSERT INTO `actors` VALUES (4,'Michael','Keaton'),(5,'Jack','Nicholson'),(6,'Kim','Basinger'),(10,'Elijah','Wood'),(11,'Ian','McKellen'),(12,'Orlando','Bloom'),(13,'Keanu','Reeves'),(14,'Michael','Nyqvist'),(15,'Alfie','Allen'),(16,'Angel','Dennis-Serhan'),(17,'Teddy','Dennis-Serhan'),(18,'Summer','Dodd'),(19,'Dennis','Impink'),(20,'Lisa','Emmerik'),(21,'Junaid','Chundrigar'),(22,'Mark','Hamill'),(23,'Harrison','Ford'),(24,'Carrie','Fisher'),(25,'Christian','Bale'),(26,'Michael','Caine'),(27,'Ken','Watanabe'),(28,'Eva','Green'),(29,'Liam','Neeson'),(30,'Marlon','Brando'),(31,'Al','Pacino'),(32,'James','Caan'),(33,'Heath','Ledger'),(34,'Aaron','Eckhart'),(35,'Viggo','Mortensen'),(36,'Brad','Stapleton'),(37,'Jeff','Weinkam'),(38,'Daniel','Kovac'),(39,'Amra','Latific'),(40,'Marina','Markovic'),(41,'Joseph','Gordon-Levitt'),(42,'Bruce','Willis'),(43,'Emily','Blunt'),(50,'Jack','Bannon'),(51,'Ben','Aldridge'),(52,'Emma','Paetz'),(53,'Emilia','Clarke'),(54,'Peter','Dinklage'),(55,'Kit','Harington'),(56,'Rhys','Ifans'),(57,'Fabien','Frankel'),(58,'Matt','Smith'),(59,'Bryan','Cranston'),(60,'Aaron','Paul'),(61,'Anna','Gunn'),(62,'Diego','Luna'),(63,'Stellan','Skarsgård'),(64,'Alex','Ferns'),(65,'Tom','Cruise'),(66,'Billy','Connolly'),(67,'Chris','Harvey'),(68,'Laurence','Fishburne'),(69,'Carrie-Anne','Moss'),(70,'Damon','Wayans'),(71,'Keesha','Sharp'),(72,'Kevin','Rahm');
/*!40000 ALTER TABLE `actors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actortvshows`
--

DROP TABLE IF EXISTS `actortvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actortvshows` (
  `ActorID` int NOT NULL,
  `TVShowID` int NOT NULL,
  PRIMARY KEY (`ActorID`,`TVShowID`),
  KEY `IX_ActorTVShows_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_ActorTVShows_Actors_ActorID` FOREIGN KEY (`ActorID`) REFERENCES `actors` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actortvshows`
--

LOCK TABLES `actortvshows` WRITE;
/*!40000 ALTER TABLE `actortvshows` DISABLE KEYS */;
INSERT INTO `actortvshows` VALUES (50,24),(51,24),(52,24),(56,25),(57,25),(58,25),(59,26),(60,26),(61,26),(53,27),(54,27),(55,27),(70,28),(71,28),(72,28);
/*!40000 ALTER TABLE `actortvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admins` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Admins_UserID` (`UserID`),
  CONSTRAINT `FK_Admins_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `SubtitleMovieID` int NOT NULL,
  `SubtitleTVShowID` int NOT NULL,
  `Content` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AdminID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Comments_AdminID` (`AdminID`),
  KEY `IX_Comments_SubtitleMovieID` (`SubtitleMovieID`),
  KEY `IX_Comments_SubtitleTVShowID` (`SubtitleTVShowID`),
  KEY `IX_Comments_UserID` (`UserID`),
  CONSTRAINT `FK_Comments_Admins_AdminID` FOREIGN KEY (`AdminID`) REFERENCES `admins` (`ID`),
  CONSTRAINT `FK_Comments_SubtitleMovies_SubtitleMovieID` FOREIGN KEY (`SubtitleMovieID`) REFERENCES `subtitlemovies` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_SubtitleTVShows_SubtitleTVShowID` FOREIGN KEY (`SubtitleTVShowID`) REFERENCES `subtitletvshows` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creators`
--

DROP TABLE IF EXISTS `creators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creators` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Surname` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=78 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creators`
--

LOCK TABLES `creators` WRITE;
/*!40000 ALTER TABLE `creators` DISABLE KEYS */;
INSERT INTO `creators` VALUES (47,'Bob','Kane'),(48,'David','S.'),(49,'Christopher','Nolan'),(50,'Jonathan','Nolan'),(51,'Mario','Puzo'),(52,'Francis','Ford'),(53,'J.R.R.','Tolkien'),(54,'Fran','Walsh'),(55,'Philippa','Boyens'),(56,'Peter','Jackson'),(57,'Jeff','Weinkam'),(58,'Tyler','Meyer'),(59,'Milutin','Petrovic'),(60,'Sasa','Radojevic'),(61,'Rian','Johnson'),(63,'Bruno','Heller'),(64,'David','Benioff'),(65,'D.B.','Weiss'),(66,'Ryan','J.'),(67,'George','R.R.'),(68,'Vince','Gilligan'),(69,'Tony','Gilroy'),(70,'John','Logan'),(71,'Edward','Zwick'),(72,'Marshall','Herskovitz'),(73,'Sam','Reddy'),(74,'Kim','Harrington'),(75,'Lilly','Wachowski'),(76,'Lana','Wachowski'),(77,'Matthew','Miller');
/*!40000 ALTER TABLE `creators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creatorsmovie`
--

DROP TABLE IF EXISTS `creatorsmovie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creatorsmovie` (
  `CreatorID` int NOT NULL,
  `MovieID` int NOT NULL,
  `Position` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`CreatorID`,`MovieID`,`Position`),
  KEY `IX_CreatorsMovie_MovieID` (`MovieID`),
  CONSTRAINT `FK_CreatorsMovie_Creators_CreatorID` FOREIGN KEY (`CreatorID`) REFERENCES `creators` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_CreatorsMovie_Movies_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `movies` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creatorsmovie`
--

LOCK TABLES `creatorsmovie` WRITE;
/*!40000 ALTER TABLE `creatorsmovie` DISABLE KEYS */;
INSERT INTO `creatorsmovie` VALUES (47,16,'Writer'),(48,16,'Writer'),(49,16,'Director'),(49,16,'Writer'),(48,17,'Writer'),(49,17,'Director'),(49,17,'Writer'),(50,17,'Writer'),(51,18,'Writer'),(52,18,'Director'),(52,18,'Writer'),(53,19,'Writer'),(54,19,'Writer'),(55,19,'Writer'),(56,19,'Director'),(53,20,'Writer'),(54,20,'Writer'),(55,20,'Writer'),(56,20,'Director'),(57,21,'Writer'),(58,21,'Director'),(53,22,'Writer'),(54,22,'Writer'),(55,22,'Writer'),(56,22,'Director'),(59,23,'Director'),(59,23,'Writer'),(60,23,'Writer'),(61,24,'Director'),(61,24,'Writer'),(70,25,'Writer'),(71,25,'Director'),(71,25,'Writer'),(72,25,'Writer'),(73,26,'Writer'),(74,26,'Director'),(75,27,'Director'),(75,27,'Writer'),(76,27,'Director'),(76,27,'Writer');
/*!40000 ALTER TABLE `creatorsmovie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creatorstvshows`
--

DROP TABLE IF EXISTS `creatorstvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creatorstvshows` (
  `CreatorID` int NOT NULL,
  `TVShowID` int NOT NULL,
  `Position` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`CreatorID`,`TVShowID`,`Position`),
  KEY `IX_CreatorsTVShows_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_CreatorsTVShows_Creators_CreatorID` FOREIGN KEY (`CreatorID`) REFERENCES `creators` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_CreatorsTVShows_TVShows_TVShowID` FOREIGN KEY (`TVShowID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creatorstvshows`
--

LOCK TABLES `creatorstvshows` WRITE;
/*!40000 ALTER TABLE `creatorstvshows` DISABLE KEYS */;
INSERT INTO `creatorstvshows` VALUES (63,24,'Writer'),(66,25,'Writer'),(67,25,'Writer'),(68,26,'Writer'),(64,27,'Writer'),(65,27,'Writer'),(77,28,'Writer');
/*!40000 ALTER TABLE `creatorstvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `directmessages`
--

DROP TABLE IF EXISTS `directmessages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `directmessages` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `RecipientUserID` int NOT NULL,
  `Content` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Time` datetime(6) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_DirectMessages_RecipientUserID` (`RecipientUserID`),
  KEY `IX_DirectMessages_UserID` (`UserID`),
  CONSTRAINT `FK_DirectMessages_Users_RecipientUserID` FOREIGN KEY (`RecipientUserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_DirectMessages_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `directmessages`
--

LOCK TABLES `directmessages` WRITE;
/*!40000 ALTER TABLE `directmessages` DISABLE KEYS */;
/*!40000 ALTER TABLE `directmessages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `episodes`
--

DROP TABLE IF EXISTS `episodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `episodes` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SeasonID` int NOT NULL,
  `EpisodeNumber` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Episodes_SeasonID` (`SeasonID`),
  CONSTRAINT `FK_Episodes_Seasons_SeasonID` FOREIGN KEY (`SeasonID`) REFERENCES `seasons` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=601 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `episodes`
--

LOCK TABLES `episodes` WRITE;
/*!40000 ALTER TABLE `episodes` DISABLE KEYS */;
INSERT INTO `episodes` VALUES (371,'Pilot',47,1),(372,'The Landlord\'s Daughter',47,2),(373,'Martha Kane',47,3),(374,'Lady Penelope',47,4),(375,'Shirley Bassey',47,5),(376,'Cilla Black',47,6),(377,'Julie Christie',47,7),(378,'Sandie Shaw',47,8),(379,'Alma Cogan',47,9),(380,'Marianne Faithfull',47,10),(381,'The Heavy Crown',48,1),(382,'The Burning Bridge',48,2),(383,'The Belt and Welt',48,3),(384,'The Hunted Fox',48,4),(385,'The Bleeding Heart',48,5),(386,'The Rose and Thorn',48,6),(387,'The Bloody Mary',48,7),(388,'The Hangman\'s Noose',48,8),(389,'Paradise Lost',48,9),(390,'The Lion and Lamb',48,10),(391,'Well to Do',49,1),(392,'Many Clouds',49,2),(393,'Comply or Die',49,3),(394,'Silver Birch',49,4),(395,'Rhyme \'n\' Reason',49,5),(396,'Hedgehunter',49,6),(397,'Don\'t Push It',49,7),(398,'Red Marauder',49,8),(399,'Rag Trade',49,9),(400,'Highland Wedding',49,10),(401,'The Heirs of the Dragon',50,1),(402,'The Rogue Prince',50,2),(403,'Second of His Name',50,3),(404,'King of the Narrow Sea',50,4),(405,'We Light the Way',50,5),(406,'The Princess and the Queen',50,6),(407,'Driftmark',50,7),(408,'The Lord of the Tides',50,8),(409,'The Green Council',50,9),(410,'The Black Queen',50,10),(411,'Pilot',51,1),(412,'Cat\'s in the Bag...',51,2),(413,'...and the Bag\'s in the River',51,3),(414,'Cancer Man',51,4),(415,'Gray Matter',51,5),(416,'Crazy Handful of Nothin\'',51,6),(417,'A No-Rough-Stuff-Type Deal',51,7),(418,'Seven Thirty-Seven',52,1),(419,'Grilled',52,2),(420,'Bit by a Dead Bee',52,3),(421,'Down',52,4),(422,'Breakage',52,5),(423,'Peekaboo',52,6),(424,'Negro Y Azul',52,7),(425,'Better Call Saul',52,8),(426,'4 Days Out',52,9),(427,'Over',52,10),(428,'Mandala',52,11),(429,'Phoenix',52,12),(430,'ABQ',52,13),(431,'No Más',53,1),(432,'Caballo sin Nombre',53,2),(433,'I.F.T.',53,3),(434,'Green Light',53,4),(435,'Más',53,5),(436,'Sunset',53,6),(437,'One Minute',53,7),(438,'I See You',53,8),(439,'Kafkaesque',53,9),(440,'Fly',53,10),(441,'Abiquiu',53,11),(442,'Half Measures',53,12),(443,'Full Measure',53,13),(444,'Box Cutter',54,1),(445,'Thirty-Eight Snub',54,2),(446,'Open House',54,3),(447,'Bullet Points',54,4),(448,'Shotgun',54,5),(449,'Cornered',54,6),(450,'Problem Dog',54,7),(451,'Hermanos',54,8),(452,'Bug',54,9),(453,'Salud',54,10),(454,'Crawl Space',54,11),(455,'End Times',54,12),(456,'Face Off',54,13),(457,'Live Free or Die',55,1),(458,'Madrigal',55,2),(459,'Hazard Pay',55,3),(460,'Fifty-One',55,4),(461,'Dead Freight',55,5),(462,'Buyout',55,6),(463,'Say My Name',55,7),(464,'Gliding Over All',55,8),(465,'Blood Money',55,9),(466,'Buried',55,10),(467,'Confessions',55,11),(468,'Rabid Dog',55,12),(469,'To\'hajiilee',55,13),(470,'Ozymandias',55,14),(471,'Granite State',55,15),(472,'Felina',55,16),(473,'Winter is Coming',56,1),(474,'The Kingsroad',56,2),(475,'Lord Snow',56,3),(476,'Cripples, Bastards, and Broken Things',56,4),(477,'The Wolf and the Lion',56,5),(478,'A Golden Crown',56,6),(479,'You Win or You Die',56,7),(480,'The Pointy End',56,8),(481,'Baelor',56,9),(482,'Fire and Blood',56,10),(483,'The North Remembers',57,1),(484,'The Night Lands',57,2),(485,'What is Dead May Never Die',57,3),(486,'Garden of Bones',57,4),(487,'The Ghost of Harrenhal',57,5),(488,'The Old Gods and the New',57,6),(489,'A Man Without Honor',57,7),(490,'The Prince of Winterfell',57,8),(491,'Blackwater',57,9),(492,'Valar Morghulis',57,10),(493,'Valar Dohaeris',58,1),(494,'Dark Wings, Dark Words',58,2),(495,'Walk of Punishment',58,3),(496,'And Now His Watch is Ended',58,4),(497,'Kissed by Fire',58,5),(498,'The Climb',58,6),(499,'The Bear and the Maiden Fair',58,7),(500,'Second Sons',58,8),(501,'The Rains of Castamere',58,9),(502,'Mhysa',58,10),(503,'Two Swords',59,1),(504,'The Lion and the Rose',59,2),(505,'Breaker of Chains',59,3),(506,'Oathkeeper',59,4),(507,'First of His Name',59,5),(508,'The Laws of Gods and Men',59,6),(509,'Mockingbird',59,7),(510,'The Mountain and the Viper',59,8),(511,'The Watchers on the Wall',59,9),(512,'The Children',59,10),(513,'The Wars to Come',60,1),(514,'The House of Black and White',60,2),(515,'High Sparrow',60,3),(516,'Sons of the Harpy',60,4),(517,'Kill the Boy',60,5),(518,'Unbowed, Unbent, Unbroken',60,6),(519,'The Gift',60,7),(520,'Hardhome',60,8),(521,'The Dance of Dragons',60,9),(522,'Mother\'s Mercy',60,10),(523,'The Red Woman',61,1),(524,'Home',61,2),(525,'Oathbreaker',61,3),(526,'Book of the Stranger',61,4),(527,'The Door',61,5),(528,'Blood of My Blood',61,6),(529,'The Broken Man',61,7),(530,'No One',61,8),(531,'Battle of the Bastards',61,9),(532,'The Winds of Winter',61,10),(533,'Dragonstone',62,1),(534,'Stormborn',62,2),(535,'The Queen\'s Justice',62,3),(536,'The Spoils of War',62,4),(537,'Eastwatch',62,5),(538,'Beyond the Wall',62,6),(539,'The Dragon and the Wolf',62,7),(540,'Winterfell',63,1),(541,'A Knight of the Seven Kingdoms',63,2),(542,'The Long Night',63,3),(543,'The Last of the Starks',63,4),(544,'The Bells',63,5),(545,'The Iron Throne',63,6),(546,'Pilot',64,1),(547,'Surf N Turf',64,2),(548,'Best Buds',64,3),(549,'There Goes the Neighborhood',64,4),(550,'Spilt Milk',64,5),(551,'Ties That Bind',64,6),(552,'Fashion Police',64,7),(553,'Can I Get a Witness?',64,8),(554,'Jingle Bell Glock',64,9),(555,'Homebodies',64,10),(556,'Lawmen',64,11),(557,'Brotherly Love',64,12),(558,'The Seal is Broken',64,13),(559,'The Murtaugh File',64,14),(560,'As Good As It Getz',64,15),(561,'Unnecessary Roughness',64,16),(562,'A Problem Like Maria',64,17),(563,'Commencement',64,18),(564,'El Gringo Loco',65,1),(565,'Dancing in September',65,2),(566,'Born to Run',65,3),(567,'Flight Risk',65,4),(568,'Let It Ride',65,5),(569,'Gold Rush',65,6),(570,'Birdwatching',65,7),(571,'Fork-Getta-Bout It',65,8),(572,'Fools Rush In',65,9),(573,'Wreck the Halls',65,10),(574,'Funny Money',65,11),(575,'Diggin\' Up Dirt',65,12),(576,'Better Living Through Chemistry',65,13),(577,'Double Shot of Baileys',65,14),(578,'An Inconvenient Ruth',65,15),(579,'Ruthless',65,16),(580,'The Odd Couple',65,17),(581,'Frankie Comes to Hollywood',65,18),(582,'Leo Getz Hitched',65,19),(583,'Jesse\'s Girl',65,20),(584,'Family Ties',65,21),(585,'One Day More',65,22),(586,'In the Same Boat',66,1),(587,'Need to Know',66,2),(588,'A Whole Lotto Trouble',66,3),(589,'Leo Getz Justice',66,4),(590,'Get the Picture',66,5),(591,'Panama',66,6),(592,'Bali',66,7),(593,'What the Puck',66,8),(594,'Bad Santas',66,9),(595,'There Will Be Bud',66,10),(596,'Dial M for Murtaugh',66,11),(597,'The Roger and Me',66,12),(598,'Coyote Ugly',66,13),(599,'A Game of Chicken',66,14),(600,'The Spy Who Loved Me',66,15);
/*!40000 ALTER TABLE `episodes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genres`
--

DROP TABLE IF EXISTS `genres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genres` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genres`
--

LOCK TABLES `genres` WRITE;
/*!40000 ALTER TABLE `genres` DISABLE KEYS */;
INSERT INTO `genres` VALUES (3,'Action'),(4,'Adventure'),(6,'Drama'),(7,'Crime'),(8,'Thriller'),(9,'Short'),(10,'Animation'),(11,'Comedy'),(12,'Fantasy'),(13,'N/A'),(14,'Mystery'),(15,'Sci-Fi'),(16,'Documentary');
/*!40000 ALTER TABLE `genres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genresmovie`
--

DROP TABLE IF EXISTS `genresmovie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genresmovie` (
  `GenreID` int NOT NULL,
  `MovieID` int NOT NULL,
  PRIMARY KEY (`GenreID`,`MovieID`),
  KEY `IX_GenresMovie_MovieID` (`MovieID`),
  CONSTRAINT `FK_GenresMovie_Genres_GenreID` FOREIGN KEY (`GenreID`) REFERENCES `genres` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_GenresMovie_Movies_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `movies` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genresmovie`
--

LOCK TABLES `genresmovie` WRITE;
/*!40000 ALTER TABLE `genresmovie` DISABLE KEYS */;
INSERT INTO `genresmovie` VALUES (3,16),(6,16),(7,16),(3,17),(6,17),(7,17),(6,18),(7,18),(3,19),(4,19),(6,19),(3,20),(4,20),(6,20),(13,21),(3,22),(4,22),(6,22),(14,23),(3,24),(6,24),(15,24),(3,25),(6,25),(15,26),(16,26),(3,27),(15,27);
/*!40000 ALTER TABLE `genresmovie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `genrestvshows`
--

DROP TABLE IF EXISTS `genrestvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genrestvshows` (
  `GenreID` int NOT NULL,
  `TVShowID` int NOT NULL,
  PRIMARY KEY (`GenreID`,`TVShowID`),
  KEY `IX_GenresTVShows_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_GenresTVShows_Genres_GenreID` FOREIGN KEY (`GenreID`) REFERENCES `genres` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_GenresTVShows_TVShows_TVShowID` FOREIGN KEY (`TVShowID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genrestvshows`
--

LOCK TABLES `genrestvshows` WRITE;
/*!40000 ALTER TABLE `genrestvshows` DISABLE KEYS */;
INSERT INTO `genrestvshows` VALUES (3,24),(6,24),(7,24),(3,25),(4,25),(6,25),(6,26),(7,26),(8,26),(3,27),(4,27),(6,27),(3,28),(7,28),(11,28);
/*!40000 ALTER TABLE `genrestvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `languages`
--

DROP TABLE IF EXISTS `languages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `languages` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EnglishName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Flag` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `languages`
--

LOCK TABLES `languages` WRITE;
/*!40000 ALTER TABLE `languages` DISABLE KEYS */;
/*!40000 ALTER TABLE `languages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movies`
--

DROP TABLE IF EXISTS `movies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `movies` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Synopsis` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Year` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Runtime` int NOT NULL,
  `IMDBRatingValue` double NOT NULL,
  `IMDBRatingCount` int NOT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MetacriticValue` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RottenTomatoesValue` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IMDBId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies`
--

LOCK TABLES `movies` WRITE;
/*!40000 ALTER TABLE `movies` DISABLE KEYS */;
INSERT INTO `movies` VALUES (16,'After training with his mentor, Batman begins his fight to free crime-ridden Gotham City from corruption.','2005',140,8.2,1458039,'https://m.media-amazon.com/images/M/MV5BOTY4YjI2N2MtYmFlMC00ZjcyLTg3YjEtMDQyM2ZjYzQ5YWFkXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SX300.jpg','Batman Begins','70/100','84%','tt0372784'),(17,'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.','2008',152,9,2628154,'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_SX300.jpg','The Dark Knight','84/100','94%','tt0468569'),(18,'The aging patriarch of an organized crime dynasty in postwar New York City transfers control of his clandestine empire to his reluctant youngest son.','1972',175,9.2,1840332,'https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg','The Godfather','100/100','97%','tt0068646'),(19,'A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring and save Middle-earth from the Dark Lord Sauron.','2001',178,8.8,1858615,'https://m.media-amazon.com/images/M/MV5BN2EyZjM3NzUtNWUzMi00MTgxLWI0NTctMzY4M2VlOTdjZWRiXkEyXkFqcGdeQXVyNDUzOTQ5MjY@._V1_SX300.jpg','The Lord of the Rings: The Fellowship of the Ring','92/100','91%','tt0120737'),(20,'While Frodo and Sam edge closer to Mordor with the help of the shifty Gollum, the divided fellowship makes a stand against Sauron\'s new ally, Saruman, and his hordes of Isengard.','2002',179,8.8,1653467,'https://m.media-amazon.com/images/M/MV5BZGMxZTdjZmYtMmE2Ni00ZTdkLWI5NTgtNjlmMjBiNzU2MmI5XkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg','The Lord of the Rings: The Two Towers','87/100','95%','tt0167261'),(21,'','2006',95,0,0,'https://m.media-amazon.com/images/M/MV5BMTIwNDk5ODQtMWZlNi00MTM5LTgwYWUtZmUxMTlhYTZlN2U2XkEyXkFqcGdeQXVyMTk3OTg1OA@@._V1_SX300.jpg','Return of the King',NULL,NULL,'tt2125599'),(22,'Gandalf and Aragorn lead the World of Men against Sauron\'s army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.','2003',201,9,1830463,'https://m.media-amazon.com/images/M/MV5BNzA5ZDNlZWMtM2NhNS00NDJjLTk4NDItYTRmY2EwMWZlMTY3XkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg','The Lord of the Rings: The Return of the King','94/100','93%','tt0167260'),(23,'This is an encounter of Hollywood and European underground in the feature of Milutin Petrovic. The film is dedicated to the work of Ljubomir Simunic, author of avantgarde films and photographs. The Loop is a visual exploration of ...','2015',74,7.1,21,'https://m.media-amazon.com/images/M/MV5BMGI1M2FiNDEtYzFjOS00MDRjLThkYTYtZjg3NjBhMjM4YWMzXkEyXkFqcGdeQXVyMjM5MTg4MjQ@._V1_SX300.jpg','The Loop',NULL,NULL,'tt3920498'),(24,'In 2074, when the mob wants to get rid of someone, the target is sent into the past, where a hired gun awaits - someone like Joe - who one day learns the mob wants to \'close the loop\' by sending back Joe\'s future self for assassin...','2012',119,7.4,575322,'https://m.media-amazon.com/images/M/MV5BMTg5NTA3NTg4NF5BMl5BanBnXkFtZTcwNTA0NDYzOA@@._V1_SX300.jpg','Looper','84/100','93%','tt1276104'),(25,'An American military advisor embraces the Samurai culture he was hired to destroy after he is captured in battle.','2003',154,7.8,440996,'https://m.media-amazon.com/images/M/MV5BMzkyNzQ1Mzc0NV5BMl5BanBnXkFtZTcwODg3MzUzMw@@._V1_SX300.jpg','The Last Samurai','55/100','66%','tt0325710'),(26,'Welcome to the matrix, is it illusion or is it reality? Find out in this powerful new documentary that explores the ultimate questions of life: where do we come from and what is the nature of reality itself. Plug into the Matrix: ...','2020',42,7,75,'https://m.media-amazon.com/images/M/MV5BNjQyNWZiM2UtOWYyYS00MmVmLWFhZGUtY2ExMDQxNDc5YTE0XkEyXkFqcGdeQXVyMTEzMjQzMDM1._V1_SX300.jpg','Matrix',NULL,NULL,'tt11749868'),(27,'When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.','1999',136,8.7,1895851,'https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg','The Matrix','73/100','88%','tt0133093');
/*!40000 ALTER TABLE `movies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `seasons`
--

DROP TABLE IF EXISTS `seasons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `seasons` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `TVShowID` int NOT NULL,
  `EpisodeCount` int DEFAULT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SeasonNumber` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `IX_Seasons_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_Seasons_TVShows_TVShowID` FOREIGN KEY (`TVShowID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=67 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seasons`
--

LOCK TABLES `seasons` WRITE;
/*!40000 ALTER TABLE `seasons` DISABLE KEYS */;
INSERT INTO `seasons` VALUES (47,24,10,NULL,1),(48,24,10,NULL,2),(49,24,10,NULL,3),(50,25,10,NULL,1),(51,26,7,NULL,1),(52,26,13,NULL,2),(53,26,13,NULL,3),(54,26,13,NULL,4),(55,26,16,NULL,5),(56,27,10,NULL,1),(57,27,10,NULL,2),(58,27,10,NULL,3),(59,27,10,NULL,4),(60,27,10,NULL,5),(61,27,10,NULL,6),(62,27,7,NULL,7),(63,27,6,NULL,8),(64,28,18,NULL,1),(65,28,22,NULL,2),(66,28,15,NULL,3);
/*!40000 ALTER TABLE `seasons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shootingplaces`
--

DROP TABLE IF EXISTS `shootingplaces`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shootingplaces` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Country` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `City` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MapsCoordinates` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shootingplaces`
--

LOCK TABLES `shootingplaces` WRITE;
/*!40000 ALTER TABLE `shootingplaces` DISABLE KEYS */;
INSERT INTO `shootingplaces` VALUES (3,'United States','',''),(4,'United Kingdom','',''),(5,'New Zealand','',''),(6,'China','',''),(7,'Australia','',''),(8,'Netherlands','',''),(9,'Germany','',''),(10,'Spain','',''),(11,'Morocco','',''),(12,'Italy','',''),(13,'France','',''),(14,'USA','',''),(15,'Serbia','',''),(16,'Japan','','');
/*!40000 ALTER TABLE `shootingplaces` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shootingplacesmovie`
--

DROP TABLE IF EXISTS `shootingplacesmovie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shootingplacesmovie` (
  `ShootingPlaceID` int NOT NULL,
  `MovieID` int NOT NULL,
  PRIMARY KEY (`ShootingPlaceID`,`MovieID`),
  KEY `IX_ShootingPlacesMovie_MovieID` (`MovieID`),
  CONSTRAINT `FK_ShootingPlacesMovie_Movies_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `movies` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ShootingPlacesMovie_ShootingPlaces_ShootingPlaceID` FOREIGN KEY (`ShootingPlaceID`) REFERENCES `shootingplaces` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shootingplacesmovie`
--

LOCK TABLES `shootingplacesmovie` WRITE;
/*!40000 ALTER TABLE `shootingplacesmovie` DISABLE KEYS */;
INSERT INTO `shootingplacesmovie` VALUES (3,16),(4,16),(3,17),(4,17),(3,18),(3,19),(5,19),(3,20),(5,20),(14,21),(3,22),(5,22),(15,23),(3,24),(6,24),(3,25),(5,25),(16,25),(3,26),(3,27),(7,27);
/*!40000 ALTER TABLE `shootingplacesmovie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shootingplacestvshows`
--

DROP TABLE IF EXISTS `shootingplacestvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shootingplacestvshows` (
  `ShootingPlaceID` int NOT NULL,
  `TVShowID` int NOT NULL,
  PRIMARY KEY (`ShootingPlaceID`,`TVShowID`),
  KEY `IX_ShootingPlacesTVShows_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_ShootingPlacesTVShows_ShootingPlaces_ShootingPlaceID` FOREIGN KEY (`ShootingPlaceID`) REFERENCES `shootingplaces` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ShootingPlacesTVShows_TVShows_TVShowID` FOREIGN KEY (`TVShowID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shootingplacestvshows`
--

LOCK TABLES `shootingplacestvshows` WRITE;
/*!40000 ALTER TABLE `shootingplacestvshows` DISABLE KEYS */;
INSERT INTO `shootingplacestvshows` VALUES (3,24),(3,25),(3,26),(3,27),(4,27),(3,28);
/*!40000 ALTER TABLE `shootingplacestvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subtitlemovies`
--

DROP TABLE IF EXISTS `subtitlemovies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subtitlemovies` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `MovieID` int NOT NULL,
  `LanguageID` int NOT NULL,
  `DownloadCount` int NOT NULL,
  `SubtitlePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  `AdminID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_SubtitleMovies_AdminID` (`AdminID`),
  KEY `IX_SubtitleMovies_LanguageID` (`LanguageID`),
  KEY `IX_SubtitleMovies_MovieID` (`MovieID`),
  CONSTRAINT `FK_SubtitleMovies_Admins_AdminID` FOREIGN KEY (`AdminID`) REFERENCES `admins` (`ID`),
  CONSTRAINT `FK_SubtitleMovies_Languages_LanguageID` FOREIGN KEY (`LanguageID`) REFERENCES `languages` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_SubtitleMovies_Movies_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `movies` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subtitlemovies`
--

LOCK TABLES `subtitlemovies` WRITE;
/*!40000 ALTER TABLE `subtitlemovies` DISABLE KEYS */;
/*!40000 ALTER TABLE `subtitlemovies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subtitletvshows`
--

DROP TABLE IF EXISTS `subtitletvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subtitletvshows` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `EpisodeID` int NOT NULL,
  `LanguageID` int NOT NULL,
  `DownloadCount` int NOT NULL,
  `SubtitlePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  `AdminID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_SubtitleTVShows_AdminID` (`AdminID`),
  KEY `IX_SubtitleTVShows_EpisodeID` (`EpisodeID`),
  KEY `IX_SubtitleTVShows_LanguageID` (`LanguageID`),
  CONSTRAINT `FK_SubtitleTVShows_Admins_AdminID` FOREIGN KEY (`AdminID`) REFERENCES `admins` (`ID`),
  CONSTRAINT `FK_SubtitleTVShows_Episodes_EpisodeID` FOREIGN KEY (`EpisodeID`) REFERENCES `episodes` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_SubtitleTVShows_Languages_LanguageID` FOREIGN KEY (`LanguageID`) REFERENCES `languages` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subtitletvshows`
--

LOCK TABLES `subtitletvshows` WRITE;
/*!40000 ALTER TABLE `subtitletvshows` DISABLE KEYS */;
/*!40000 ALTER TABLE `subtitletvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subtitleusers`
--

DROP TABLE IF EXISTS `subtitleusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subtitleusers` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `SubtitleMovieID` int DEFAULT NULL,
  `SubtitleTVShowID` int DEFAULT NULL,
  `TranslationID` int DEFAULT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_SubtitleUsers_SubtitleMovieID` (`SubtitleMovieID`),
  KEY `IX_SubtitleUsers_SubtitleTVShowID` (`SubtitleTVShowID`),
  KEY `IX_SubtitleUsers_TranslationID` (`TranslationID`),
  KEY `IX_SubtitleUsers_UserID` (`UserID`),
  CONSTRAINT `FK_SubtitleUsers_SubtitleMovies_SubtitleMovieID` FOREIGN KEY (`SubtitleMovieID`) REFERENCES `subtitlemovies` (`ID`),
  CONSTRAINT `FK_SubtitleUsers_SubtitleTVShows_SubtitleTVShowID` FOREIGN KEY (`SubtitleTVShowID`) REFERENCES `subtitletvshows` (`ID`),
  CONSTRAINT `FK_SubtitleUsers_Translations_TranslationID` FOREIGN KEY (`TranslationID`) REFERENCES `translations` (`ID`),
  CONSTRAINT `FK_SubtitleUsers_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subtitleusers`
--

LOCK TABLES `subtitleusers` WRITE;
/*!40000 ALTER TABLE `subtitleusers` DISABLE KEYS */;
/*!40000 ALTER TABLE `subtitleusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `translations`
--

DROP TABLE IF EXISTS `translations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `translations` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `LanguageID` int NOT NULL,
  `SubtitlePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Translations_LanguageID` (`LanguageID`),
  CONSTRAINT `FK_Translations_Languages_LanguageID` FOREIGN KEY (`LanguageID`) REFERENCES `languages` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `translations`
--

LOCK TABLES `translations` WRITE;
/*!40000 ALTER TABLE `translations` DISABLE KEYS */;
/*!40000 ALTER TABLE `translations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tvshows`
--

DROP TABLE IF EXISTS `tvshows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tvshows` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Synopsis` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Year` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SeasonCount` int NOT NULL,
  `EpisodeCount` int DEFAULT NULL,
  `IMDBRatingValue` double NOT NULL,
  `IMDBRatingCount` int NOT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MetacriticValue` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `RottenTomatoesValue` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IMDBId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tvshows`
--

LOCK TABLES `tvshows` WRITE;
/*!40000 ALTER TABLE `tvshows` DISABLE KEYS */;
INSERT INTO `tvshows` VALUES (24,'The origin story of Alfred Pennyworth, a former special-forces soldier living in London and how he came to work for Bruce Wayne\'s father.','2019–',3,30,7.9,15231,'https://m.media-amazon.com/images/M/MV5BODk5ZmRlYjMtZDMzNS00M2Y4LTkzNWQtMThiODIyOGJkZGY5XkEyXkFqcGdeQXVyMDM2NDM2MQ@@._V1_SX300.jpg','Pennyworth','','','tt8425532'),(25,'An internal succession war within House Targaryen at the height of its power, 172 years before the birth of Daenerys Targaryen.','2022–',1,10,8.6,195655,'https://m.media-amazon.com/images/M/MV5BZjBiOGIyY2YtOTA3OC00YzY1LThkYjktMGRkYTNhNTExY2I2XkEyXkFqcGdeQXVyMTEyMjM2NDc2._V1_SX300.jpg','House of the Dragon','','','tt11198330'),(26,'A high school chemistry teacher diagnosed with inoperable lung cancer turns to manufacturing and selling methamphetamine in order to secure his family\'s future.','2008–2013',5,62,9.5,1847129,'https://m.media-amazon.com/images/M/MV5BNDkyZThhNmMtZDBjYS00NDBmLTlkMjgtNWM2ZWYzZDQxZWU1XkEyXkFqcGdeQXVyMTMzNDExODE5._V1_SX300.jpg','Breaking Bad','','96%','tt0903747'),(27,'Nine noble families fight for control over the lands of Westeros, while an ancient enemy returns after being dormant for millennia.','2011–2019',8,73,9.2,2083397,'https://m.media-amazon.com/images/M/MV5BYTRiNDQwYzAtMzVlZS00NTI5LWJjYjUtMzkwNTUzMWMxZTllXkEyXkFqcGdeQXVyNDIzMzcwNjc@._V1_SX300.jpg','Game of Thrones','','','tt0944947'),(28,'A slightly unhinged cop is partnered with a veteran detective trying to maintain a low stress level in his life.','2016–2019',3,55,7.8,38417,'https://m.media-amazon.com/images/M/MV5BMzMyNzM1MTczM15BMl5BanBnXkFtZTgwNDg4MzA0NjM@._V1_SX300.jpg','Lethal Weapon','','','tt5164196');
/*!40000 ALTER TABLE `tvshows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProfileImage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AdminID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Users_AdminID` (`AdminID`),
  CONSTRAINT `FK_Users_Admins_AdminID` FOREIGN KEY (`AdminID`) REFERENCES `admins` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userslanguages`
--

DROP TABLE IF EXISTS `userslanguages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userslanguages` (
  `UserID` int NOT NULL,
  `LanguageID` int NOT NULL,
  PRIMARY KEY (`UserID`,`LanguageID`),
  KEY `IX_UsersLanguages_LanguageID` (`LanguageID`),
  CONSTRAINT `FK_UsersLanguages_Languages_LanguageID` FOREIGN KEY (`LanguageID`) REFERENCES `languages` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_UsersLanguages_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `users` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userslanguages`
--

LOCK TABLES `userslanguages` WRITE;
/*!40000 ALTER TABLE `userslanguages` DISABLE KEYS */;
/*!40000 ALTER TABLE `userslanguages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'captioneer'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-11-22 18:04:23
