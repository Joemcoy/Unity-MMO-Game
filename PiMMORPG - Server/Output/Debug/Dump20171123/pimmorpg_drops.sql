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
-- Table structure for table `drops`
--

DROP TABLE IF EXISTS `drops`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `drops` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Serial` char(36) DEFAULT NULL,
  `Map` int(10) unsigned DEFAULT NULL,
  `InventoryID` int(10) unsigned DEFAULT NULL,
  `Quantity` int(10) unsigned DEFAULT NULL,
  `PositionX` float DEFAULT NULL,
  `PositionY` float DEFAULT NULL,
  `PositionZ` float DEFAULT NULL,
  `RotationX` float DEFAULT NULL,
  `RotationY` float DEFAULT NULL,
  `RotationZ` float DEFAULT NULL,
  `RotationW` float DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `drops`
--

LOCK TABLES `drops` WRITE;
/*!40000 ALTER TABLE `drops` DISABLE KEYS */;
INSERT INTO `drops` VALUES (1,'aa923ea0-ea80-4be8-b49a-1ade51f6e570',1,12,1,-171.403,75.2876,-298.089,-4.2039e-45,-0.677255,4.2039e-45,0.735749),(4,'f552a444-63f6-48de-ae94-9a8ffd4c9876',1,10,1,-175.308,74.7644,-298.597,0,0,0,1),(5,'b6e0e67f-4ddf-41f0-8823-4d5950073d3c',1,6,1,-173.124,74.7332,-301.745,-4.2039e-45,-0.262574,4.2039e-45,-0.964912),(6,'44ba6dd7-67ae-4d6a-8cb3-0cc17ecca414',1,1,99,-171.192,74.6603,-304.43,-4.2039e-45,-0.379794,4.2039e-45,0.925071);
/*!40000 ALTER TABLE `drops` ENABLE KEYS */;
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
