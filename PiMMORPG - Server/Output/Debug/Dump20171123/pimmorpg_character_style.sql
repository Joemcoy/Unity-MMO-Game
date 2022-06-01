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
-- Table structure for table `character_style`
--

DROP TABLE IF EXISTS `character_style`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_style` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Hair` smallint(6) DEFAULT NULL,
  `HairColor` smallint(6) DEFAULT NULL,
  `CraniumSlope` float DEFAULT NULL,
  `EarlobesAttached` float DEFAULT NULL,
  `EarsDepth` float DEFAULT NULL,
  `EarsElf` float DEFAULT NULL,
  `EarsHeight` float DEFAULT NULL,
  `JawOutIn` float DEFAULT NULL,
  `LipBottomInOut` float DEFAULT NULL,
  `LipBottomInOutL` float DEFAULT NULL,
  `LipBottomInOutR` float DEFAULT NULL,
  `LipBottomUpDown` float DEFAULT NULL,
  `LipBottomUpDownL` float DEFAULT NULL,
  `LipBottomUpDownR` float DEFAULT NULL,
  `JawCurve` float DEFAULT NULL,
  `JawHeight` float DEFAULT NULL,
  `JawlineDepth` float DEFAULT NULL,
  `lHornFrontHigh` float DEFAULT NULL,
  `lHornFrontLarge` float DEFAULT NULL,
  `lHornFrontSmall` float DEFAULT NULL,
  `lHornSide` float DEFAULT NULL,
  `rHornFrontHigh` float DEFAULT NULL,
  `rHornFrontLarge` float DEFAULT NULL,
  `rHornFrontSmall` float DEFAULT NULL,
  `rHornSide` float DEFAULT NULL,
  `EyelashesLength` float DEFAULT NULL,
  `EyelidSize` float DEFAULT NULL,
  `EyelidsFoldDown` float DEFAULT NULL,
  `EyesAlmondInner` float DEFAULT NULL,
  `EyesAlmondOuter` float DEFAULT NULL,
  `EyesAngledInner` float DEFAULT NULL,
  `EyesAngledOuter` float DEFAULT NULL,
  `EyesHeight` float DEFAULT NULL,
  `EyesIrisSize` float DEFAULT NULL,
  `EyesSize` float DEFAULT NULL,
  `EyesWidth` float DEFAULT NULL,
  `EyelidsLowerUpDown` float DEFAULT NULL,
  `EyelidsLowerUpDownL` float DEFAULT NULL,
  `EyelidsLowerUpDownR` float DEFAULT NULL,
  `EyelidsUpperDownUp` float DEFAULT NULL,
  `EyelidsUpperDownUpL` float DEFAULT NULL,
  `EyelidsUpperDownUpR` float DEFAULT NULL,
  `EyesSquint` float DEFAULT NULL,
  `EyesSquintL` float DEFAULT NULL,
  `EyesSquintR` float DEFAULT NULL,
  `EyeSagTop` float DEFAULT NULL,
  `FaceCenterDepth` float DEFAULT NULL,
  `FaceFlat` float DEFAULT NULL,
  `FaceHeart` float DEFAULT NULL,
  `FaceRound` float DEFAULT NULL,
  `FaceSquare` float DEFAULT NULL,
  `BrowInnerUpDown` float DEFAULT NULL,
  `BrowInnerUpDownL` float DEFAULT NULL,
  `BrowInnerUpDownR` float DEFAULT NULL,
  `BrowOuterUpDown` float DEFAULT NULL,
  `BrowOuterUpDownL` float DEFAULT NULL,
  `BrowOuterUpDownR` float DEFAULT NULL,
  `BrowUpDown` float DEFAULT NULL,
  `BrowUpDownL` float DEFAULT NULL,
  `BrowUpDownR` float DEFAULT NULL,
  `CheekbonesDepressionHD` float DEFAULT NULL,
  `CheeksDepth` float DEFAULT NULL,
  `ChinCleftHD` float DEFAULT NULL,
  `ChinWidth` float DEFAULT NULL,
  `LipsPart` float DEFAULT NULL,
  `LipsPartCenter` float DEFAULT NULL,
  `LipsPucker` float DEFAULT NULL,
  `LipTopUpDown` float DEFAULT NULL,
  `LipTopUpDownL` float DEFAULT NULL,
  `LipTopUpDownR` float DEFAULT NULL,
  `MouthCornerUpDown` float DEFAULT NULL,
  `MouthFrown` float DEFAULT NULL,
  `MouthNarrow` float DEFAULT NULL,
  `MouthNarrowL` float DEFAULT NULL,
  `MouthNarrowR` float DEFAULT NULL,
  `MouthCurves` float DEFAULT NULL,
  `MouthHeight` float DEFAULT NULL,
  `MouthSize` float DEFAULT NULL,
  `MouthWidth` float DEFAULT NULL,
  `LipDepthLower` float DEFAULT NULL,
  `LipsHeart` float DEFAULT NULL,
  `LipsSquare` float DEFAULT NULL,
  `LipUpperSize` float DEFAULT NULL,
  `NoseAge` float DEFAULT NULL,
  `NoseBridgeDepth` float DEFAULT NULL,
  `NoseBridgeHeight` float DEFAULT NULL,
  `NoseBridgeSkew` float DEFAULT NULL,
  `NoseBridgeSlope` float DEFAULT NULL,
  `NoseBump` float DEFAULT NULL,
  `NoseDepth` float DEFAULT NULL,
  `NoseHeight` float DEFAULT NULL,
  `NosePinch` float DEFAULT NULL,
  `NoseRidgeWidth` float DEFAULT NULL,
  `NoseSize` float DEFAULT NULL,
  `NoseTipHeight` float DEFAULT NULL,
  `NoseTipRound` float DEFAULT NULL,
  `NoseWidth` float DEFAULT NULL,
  `NostrilWingSize` float DEFAULT NULL,
  `NostrilWingWidth` float DEFAULT NULL,
  `UpperArmsSize` float DEFAULT NULL,
  `ArmsLength` float DEFAULT NULL,
  `FingersLength` float DEFAULT NULL,
  `FingersLengthL` float DEFAULT NULL,
  `FingersLengthR` float DEFAULT NULL,
  `FingersWidth` float DEFAULT NULL,
  `FingersWidthL` float DEFAULT NULL,
  `FingersWidthR` float DEFAULT NULL,
  `ForearmsSize` float DEFAULT NULL,
  `PalmScale` float DEFAULT NULL,
  `PalmScaleL` float DEFAULT NULL,
  `PalmScaleR` float DEFAULT NULL,
  `BasicWeight1` float DEFAULT NULL,
  `BodybuilderSize` float DEFAULT NULL,
  `BodybuilderDetails` float DEFAULT NULL,
  `FitnessSize` float DEFAULT NULL,
  `FitnessDetails` float DEFAULT NULL,
  `NeckWeight` float DEFAULT NULL,
  `NeckLength` float DEFAULT NULL,
  `NeckSize` float DEFAULT NULL,
  `ChestDepth` float DEFAULT NULL,
  `ChestWidth` float DEFAULT NULL,
  `ShldrsScale` float DEFAULT NULL,
  `ShldrsWidth` float DEFAULT NULL,
  `ShoulderDrop` float DEFAULT NULL,
  `ShouldersSize` float DEFAULT NULL,
  `PectoralsCleavage` float DEFAULT NULL,
  `PectoralsDiameter` float DEFAULT NULL,
  `PectoralsHeavy` float DEFAULT NULL,
  `PectoralsSag` float DEFAULT NULL,
  `RibcageSize` float DEFAULT NULL,
  `SternumDepth` float DEFAULT NULL,
  `Belly` float DEFAULT NULL,
  `StomachLowerDepth` float DEFAULT NULL,
  `ThighsSize` float DEFAULT NULL,
  `CalvesSize` float DEFAULT NULL,
  `GlutesSize` float DEFAULT NULL,
  `LegsLength` float DEFAULT NULL,
  `ShinsSize` float DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_style`
--

LOCK TABLES `character_style` WRITE;
/*!40000 ALTER TABLE `character_style` DISABLE KEYS */;
INSERT INTO `character_style` VALUES (1,0,3,-0.0668526,0,0,0,0,0,0,0,0,0,50,50,0,0,0,0,0,0,0,0,0,0,0,97.2444,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,33.5221,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,28.3832,20.4887,92.4332,0,0,0,0,0,74.6333,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,82.8555,0,100,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),(2,10,3,0,0,0,0,0,0,0,0,0,0,28.1342,50,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,0,0,0,0,65.6376,72.349,0,0,0,0,0,0,0,0,0,85.7718,54.8993,0,0,0,0,0,0,0,0,0,0,0),(3,1,3,-0.0668526,0,0,0,0,0,0,0,0,0,47.3423,50,0,0,0,0,0,0,0,0,0,0,0,97.2444,0,0,0,0,0,0,0,0,25,50,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,100,0,0,0,0,0,0,0,0,0,43.0201,100,0,0,0,0,0,0,0,28.3832,20.4887,92.4332,0,0,0,0,20.5369,63.4899,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,51.4094,90.3356,69.5302,53.4228,70.8725,46.0403,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `character_style` ENABLE KEYS */;
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
