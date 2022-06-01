CREATE DATABASE  IF NOT EXISTS `pimmorpg` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `pimmorpg`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: pimmorpg
-- ------------------------------------------------------
-- Server version	5.7.19-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `character_items`
--

DROP TABLE IF EXISTS `character_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_items` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Character` int(10) unsigned DEFAULT NULL,
  `Info` int(10) unsigned DEFAULT NULL,
  `Slot` int(10) unsigned DEFAULT NULL,
  `Quantity` int(10) unsigned DEFAULT NULL,
  `HotbarSlot` int(11) DEFAULT NULL,
  `Equipped` bit(1) DEFAULT NULL,
  `Serial` char(36) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_items`
--

LOCK TABLES `character_items` WRITE;
/*!40000 ALTER TABLE `character_items` DISABLE KEYS */;
INSERT INTO `character_items` VALUES (1,1,2,1,1,-1,'\0','122a1125-4641-442f-8b62-24a3eee35ef6'),(2,1,3,2,1,-1,'\0','68e1724d-43b7-4bd6-a6fb-bb2eed284ffe'),(3,1,4,3,1,-1,'\0','8015db23-f895-49fd-b599-4359dda5285f'),(4,1,5,6,1,-1,'','a135aad4-c0aa-4ae7-bdec-7483f7477d90'),(5,1,6,2,1,-1,'\0','b725eff4-8e64-4212-b285-652bb7932049'),(6,1,7,3,1,-1,'\0','09cfd550-2583-4409-b8fe-7bd37018f4f0'),(7,1,8,11,1,-1,'\0','e62200a3-6e13-4ee3-bb44-6738b96d6c3f'),(8,1,9,5,1,-1,'\0','326b214a-8e77-4bd7-a6ce-5d0874e2957e'),(9,1,10,7,1,-1,'\0','e554c36b-e654-436f-8566-c929c22e4cb7'),(11,1,12,1,1,-1,'\0','24ab221e-799a-4b7f-bfd2-e27747157276'),(12,1,13,2,1,-1,'\0','20e20ff2-a025-43e5-86bf-441da915433d'),(13,1,14,13,1,-1,'\0','2b722987-3919-48c0-b54f-d4c95c0009fb'),(14,1,15,14,1,-1,'\0','09bb74cf-5a61-4833-a1c7-69efbe92a2db'),(15,1,16,15,1,-1,'\0','0cedd37c-6cef-488f-9ac0-174209f3759a'),(16,1,17,16,1,-1,'\0','531460f4-d626-48a6-8d1f-12508d7f029d'),(17,1,18,17,1,-1,'\0','3ad7e551-a45c-41a0-8757-5e005821d40c'),(18,1,19,18,1,-1,'\0','44ebe5e9-f26f-4e6a-b688-3a0f7bdd105f'),(19,2,2,1,1,-1,'\0','744a0ab8-5987-47c6-9a7c-5ba4a0b8cf1a'),(20,2,3,2,1,-1,'\0','b7ed2227-3546-4a9f-a45d-0ace378c076c'),(21,2,4,3,1,-1,'\0','b7867968-605a-4018-b68b-d60e3d4b2b97'),(22,2,5,4,1,-1,'\0','ac384b9d-c7b6-4d46-ac6c-425f4170cabf'),(23,2,6,5,1,-1,'\0','d0aae239-cd60-4495-8ed8-190b7c05f230'),(24,2,7,6,1,-1,'\0','16899cf7-f52c-44d3-9357-1a7b8f3ba2ec'),(25,2,8,7,1,-1,'\0','f79ba2e7-22df-43e1-a35b-d6fe1a2eca77'),(26,2,9,8,1,-1,'\0','f9328d39-c618-49a0-9e80-eb72ac275bb7'),(27,2,10,9,1,-1,'\0','d69b648a-a0de-4f96-bbea-13cbc628fa98'),(28,2,11,10,1,-1,'\0','b0095453-2ac7-403f-b2bf-927f44e128ad'),(29,2,12,11,1,-1,'\0','f89f058d-612c-4f36-bf47-7a5a7db0ffe0'),(30,2,13,12,1,-1,'\0','c8179e1e-88a0-4d31-acee-9688f20d2a56'),(31,2,14,13,1,-1,'\0','83f4584a-0096-494f-ba82-d93b4e023fb2'),(32,2,15,14,1,-1,'\0','d25f8614-25ba-4e31-82c4-a1afd591cf28'),(33,2,16,15,1,-1,'\0','58cc62a5-794d-41d7-9f2e-684e113bda6d'),(34,2,17,16,1,-1,'\0','fc322670-b255-42cc-984d-c0e900fb5cd5'),(35,2,18,17,1,-1,'\0','21d4825f-c062-4a7e-bdfa-3ae9893ff5e6'),(36,2,19,18,1,-1,'\0','bc15a761-7c0f-48f4-aa8b-d91079acdced'),(37,1,6,23,1,-1,'\0','b1037a37-bf6b-4ab0-a42a-1069f58ee9a5'),(38,1,3,21,1,-1,'\0','5e5374e0-1e47-4614-9e60-d0706181b833'),(39,1,9,1,1,-1,'','4cd2aae0-1134-4adf-8859-5cc1d673c75e'),(41,1,2,21,19,-1,'\0','d8fbd642-0aa3-42bc-ba2d-ae7f2b6958aa'),(42,1,2,22,1,-1,'\0','3293315e-a936-46e1-b1ec-de82832177eb'),(43,1,11,5,1,-1,'\0','f552a444-63f6-48de-ae94-9a8ffd4c9876'),(44,1,4,9,1,-1,'','20e72165-d41a-4e1b-b3ab-0b650dd27d7c'),(45,1,1,0,99,-1,'\0','103ffc04-a131-4a0c-b7f0-276af78e1740'),(46,3,2,0,1,-1,'\0','9bb7adf8-8189-4506-8292-e87c08a9b9e7'),(47,3,3,1,1,-1,'\0','121be2a8-db65-4bf3-a605-b8f8ae42b159'),(48,3,4,2,1,-1,'\0','24fb7226-5dfe-459f-96a7-31d436a478bf'),(49,3,5,3,1,-1,'\0','311c8ecc-472e-4760-9f4a-73fa4ec05a71'),(50,3,6,4,1,-1,'\0','39003705-70a0-445d-b833-299f71a2794b'),(51,3,7,5,1,-1,'\0','092981fe-4cad-454b-8458-e72c116d9da8'),(52,3,8,6,1,-1,'\0','1acda0e8-f16f-432d-a07d-765031c4c3d6'),(53,3,9,7,1,-1,'\0','7ec4aa62-351c-4b9a-a0ef-6f5770d0758d'),(54,3,10,8,1,-1,'\0','318fb49f-84ec-42aa-97c1-d80f18c2709a'),(55,3,11,9,1,-1,'\0','18fca322-e98d-4823-bf3f-4c9385ce1c4e'),(56,3,12,10,1,-1,'\0','4fd2fb5f-0457-4ff5-a62f-0a47c399b451'),(57,3,13,11,1,-1,'\0','a6bb84af-7627-446f-bf4b-aabdf8c43128'),(58,3,14,12,1,-1,'\0','05fc45e3-ddd0-4dcd-8fea-5c077877e61f'),(59,3,15,13,1,-1,'\0','f34a0930-2ab0-4849-adec-4b92b7f2dfa5'),(60,3,16,14,1,-1,'\0','f3d3393e-49ac-407c-9f04-89e168baf9e3'),(61,3,17,15,1,-1,'\0','d786261f-293f-4832-ad25-2efb2af2e01b'),(62,3,18,16,1,-1,'\0','32b8ae5c-ec0c-450a-9b60-31543d8c8aea'),(63,3,19,17,1,-1,'\0','0c0d842d-1dad-49f4-b3c7-f49716c062aa');
/*!40000 ALTER TABLE `character_items` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-11-23 13:52:02
