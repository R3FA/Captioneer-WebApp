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
INSERT INTO `__efmigrationshistory` VALUES ('20221109235306_InitialMigration','6.0.11'),('20221110205311_FixAdmin','6.0.11');
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
  `Role` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
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
  `Portrait` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actors`
--

LOCK TABLES `actors` WRITE;
/*!40000 ALTER TABLE `actors` DISABLE KEYS */;
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
  `EpisodeCount` int NOT NULL,
  `Role` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MovieID` int NOT NULL,
  PRIMARY KEY (`ActorID`,`TVShowID`),
  KEY `IX_ActorTVShows_MovieID` (`MovieID`),
  CONSTRAINT `FK_ActorTVShows_Actors_ActorID` FOREIGN KEY (`ActorID`) REFERENCES `actors` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActorTVShows_TVShows_MovieID` FOREIGN KEY (`MovieID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actortvshows`
--

LOCK TABLES `actortvshows` WRITE;
/*!40000 ALTER TABLE `actortvshows` DISABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creators`
--

LOCK TABLES `creators` WRITE;
/*!40000 ALTER TABLE `creators` DISABLE KEYS */;
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
  `Runtime` int NOT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Episodes_SeasonID` (`SeasonID`),
  CONSTRAINT `FK_Episodes_Seasons_SeasonID` FOREIGN KEY (`SeasonID`) REFERENCES `seasons` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `episodes`
--

LOCK TABLES `episodes` WRITE;
/*!40000 ALTER TABLE `episodes` DISABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `genres`
--

LOCK TABLES `genres` WRITE;
/*!40000 ALTER TABLE `genres` DISABLE KEYS */;
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
  `Year` int NOT NULL,
  `Runtime` int NOT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movies`
--

LOCK TABLES `movies` WRITE;
/*!40000 ALTER TABLE `movies` DISABLE KEYS */;
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
  `EpisodeCount` int NOT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`),
  KEY `IX_Seasons_TVShowID` (`TVShowID`),
  CONSTRAINT `FK_Seasons_TVShows_TVShowID` FOREIGN KEY (`TVShowID`) REFERENCES `tvshows` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `seasons`
--

LOCK TABLES `seasons` WRITE;
/*!40000 ALTER TABLE `seasons` DISABLE KEYS */;
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
  `MapsCoordinates` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shootingplaces`
--

LOCK TABLES `shootingplaces` WRITE;
/*!40000 ALTER TABLE `shootingplaces` DISABLE KEYS */;
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
  `Year` int NOT NULL,
  `SeasonCount` int NOT NULL,
  `EpisodeCount` int NOT NULL,
  `RatingValue` double NOT NULL,
  `RatingCount` int NOT NULL,
  `CoverArt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tvshows`
--

LOCK TABLES `tvshows` WRITE;
/*!40000 ALTER TABLE `tvshows` DISABLE KEYS */;
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
  `Username` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProfileImage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AdminID` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `IX_Users_AdminID` (`AdminID`),
  CONSTRAINT `FK_Users_Admins_AdminID` FOREIGN KEY (`AdminID`) REFERENCES `admins` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
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

-- Dump completed on 2022-11-10 21:58:46
