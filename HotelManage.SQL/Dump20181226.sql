CREATE DATABASE  IF NOT EXISTS `hotelmanage` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;
USE `hotelmanage`;
-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: hotelmanage
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `guest`
--

DROP TABLE IF EXISTS `guest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `guest` (
  `ID` bigint(20) NOT NULL,
  `CheckId` bigint(20) NOT NULL COMMENT 'roomcheckID',
  `Name` varchar(45) DEFAULT NULL COMMENT '姓名',
  `CertType` varchar(45) DEFAULT NULL COMMENT '证件类型',
  `CertId` varchar(45) DEFAULT NULL COMMENT '证件号码',
  `Mobile` varchar(45) DEFAULT NULL COMMENT '手机号',
  `IsDel` bit(1) DEFAULT b'0',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='入住人';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guest`
--

LOCK TABLES `guest` WRITE;
/*!40000 ALTER TABLE `guest` DISABLE KEYS */;
/*!40000 ALTER TABLE `guest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hotel`
--

DROP TABLE IF EXISTS `hotel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `hotel` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增ID',
  `Name` varchar(50) NOT NULL COMMENT '名称',
  `HotelPassword` varchar(50) DEFAULT NULL,
  `Region` varchar(50) DEFAULT NULL COMMENT '地区',
  `Address` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '地址',
  `IsDel` bit(1) DEFAULT b'0' COMMENT '是否删除',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间',
  `Salt` varchar(50) DEFAULT NULL COMMENT '密码盐',
  `Remark` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='宾馆';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hotel`
--

LOCK TABLES `hotel` WRITE;
/*!40000 ALTER TABLE `hotel` DISABLE KEYS */;
INSERT INTO `hotel` VALUES (1,'第一家',NULL,'灌云县','灌云县','\0','2018-08-29 18:06:47',NULL,NULL,NULL),(2,'第一家',NULL,'灌云县','灌云县','\0','2018-08-29 18:22:24',NULL,NULL,NULL),(3,'第二家',NULL,'灌云县','灌云县','\0','2018-08-29 18:25:22','2018-08-29 18:25:22',NULL,NULL),(4,'宾馆B',NULL,'lianyungang','lianyungang','','2018-08-30 00:00:00','2018-08-30 18:27:54',NULL,NULL),(5,'宾馆C',NULL,'suzhou','suzhou','\0','2018-08-30 20:28:28','2018-08-30 20:28:28',NULL,NULL),(22,'康乐宾馆','oe4di+9lSvp4EzQ+Vq18szCsuMvNRm9KfA2e1hittRQ=','山西省,忻州市,五台县','台怀镇','\0','2018-11-14 18:58:05','2018-11-14 19:07:31','4cc39392-0c4b-4c57-8430-5e9b2a04f7c5','干净卫生');
/*!40000 ALTER TABLE `hotel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hotelenum`
--

DROP TABLE IF EXISTS `hotelenum`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `hotelenum` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FullKey` varchar(20) NOT NULL COMMENT 'Key值',
  `Name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `EnumClass` varchar(20) DEFAULT NULL COMMENT '枚举类别',
  `IsDel` bit(1) DEFAULT b'0',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='宾馆相关枚举项';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hotelenum`
--

LOCK TABLES `hotelenum` WRITE;
/*!40000 ALTER TABLE `hotelenum` DISABLE KEYS */;
INSERT INTO `hotelenum` VALUES (1,'RoomType/Bigbed','大床房','RoomType','\0','2018-09-03 16:30:30','2018-09-03 16:30:30'),(2,'RoomType/Standard','标准间','RoomType','\0','2018-09-03 16:30:30','2018-09-03 16:30:30'),(3,'RoomType/Suite','套房','RoomType','\0','2018-09-03 16:30:30','2018-09-03 16:30:30'),(4,'RoomType/Single','单人间','RoomType','\0','2018-09-03 16:30:30','2018-09-03 16:30:30'),(5,'CertType/IdCard','身份证','CertType','\0','2018-12-10 20:37:39','2018-12-10 20:37:39'),(6,'CertType/Passport','护照','CertType','\0','2018-12-10 20:37:39','2018-12-10 20:37:39');
/*!40000 ALTER TABLE `hotelenum` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hotelmanager`
--

DROP TABLE IF EXISTS `hotelmanager`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `hotelmanager` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增ID',
  `HotelID` int(11) NOT NULL COMMENT '宾馆ID',
  `WxOpenID` varchar(45) NOT NULL COMMENT '''微信openid''',
  `WxUnionID` varchar(45) DEFAULT NULL COMMENT '''微信unionid''',
  `IsDel` bit(1) DEFAULT b'0' COMMENT '是否删除',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  `Role` int(4) NOT NULL COMMENT '管理角色：0-第一管理者：1-协助管理者',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='宾馆管理者';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hotelmanager`
--

LOCK TABLES `hotelmanager` WRITE;
/*!40000 ALTER TABLE `hotelmanager` DISABLE KEYS */;
INSERT INTO `hotelmanager` VALUES (16,22,'oSrrW5ZskKgloPglX_-298NeEs1s',NULL,'','2018-11-14 18:58:05','2018-11-14 18:58:05',0),(17,22,'oSrrW5ZskKgloPglX_-298NeEs1s',NULL,'\0','2018-11-22 21:14:15','2018-11-22 21:14:15',1);
/*!40000 ALTER TABLE `hotelmanager` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operationlog`
--

DROP TABLE IF EXISTS `operationlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `operationlog` (
  `ID` bigint(16) NOT NULL AUTO_INCREMENT,
  `Type` int(11) NOT NULL COMMENT '类型：1-新增，2-修改，3-删除',
  `TableName` varchar(20) NOT NULL,
  `ForeignKey` varchar(50) NOT NULL COMMENT '修改的值',
  `FieldName` varchar(20) DEFAULT NULL COMMENT '修改的字段',
  `OldValue` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '旧值',
  `NewValue` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '新值',
  `IsDel` bit(1) DEFAULT b'0' COMMENT '是否删除',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='操作日志记录表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operationlog`
--

LOCK TABLES `operationlog` WRITE;
/*!40000 ALTER TABLE `operationlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `operationlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `room` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增ID',
  `HotelID` int(11) NOT NULL COMMENT '宾馆ID',
  `RoomNo` varchar(10) NOT NULL COMMENT '房间编号',
  `RoomType` varchar(20) DEFAULT NULL COMMENT '房间类型',
  `IsDel` bit(1) DEFAULT b'0' COMMENT '是否删除',
  `Remark` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='房间';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `room`
--

LOCK TABLES `room` WRITE;
/*!40000 ALTER TABLE `room` DISABLE KEYS */;
INSERT INTO `room` VALUES (1,22,'101','RoomType/Bigbed','\0','101','2018-12-03 20:38:46','2018-12-03 20:38:46'),(2,22,'102','RoomType/Standard','\0','102','2018-12-03 20:48:37','2018-12-05 21:08:52'),(3,22,'103','RoomType/Suite','','','2018-12-03 20:50:09','2018-12-04 22:11:31'),(4,22,'105','RoomType/Single','','1055','2018-12-03 20:50:52','2018-12-04 22:10:11'),(5,22,'103','RoomType/Standard','\0','new','2018-12-05 22:42:52','2018-12-05 22:42:52'),(6,22,'104','RoomType/Standard','','new','2018-12-05 22:48:44','2018-12-05 22:49:25'),(7,22,'201','RoomType/Bigbed','\0','','2018-12-05 22:50:36','2018-12-05 22:50:36');
/*!40000 ALTER TABLE `room` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roomcheck`
--

DROP TABLE IF EXISTS `roomcheck`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `roomcheck` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `RoomID` int(11) NOT NULL,
  `Status` int(4) NOT NULL,
  `ReserveTime` datetime DEFAULT NULL,
  `PlanedCheckinTime` datetime DEFAULT NULL,
  `CheckinTime` datetime DEFAULT NULL,
  `PlanedCheckoutTime` datetime DEFAULT NULL,
  `CheckoutTime` datetime DEFAULT NULL,
  `Prices` decimal(8,2) DEFAULT NULL,
  `Deposit` decimal(8,2) DEFAULT NULL COMMENT '押金',
  `Remark` varchar(100) DEFAULT NULL,
  `IsDel` bit(1) DEFAULT b'0',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdateTime` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='房间入住情况';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roomcheck`
--

LOCK TABLES `roomcheck` WRITE;
/*!40000 ALTER TABLE `roomcheck` DISABLE KEYS */;
/*!40000 ALTER TABLE `roomcheck` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-12-26 11:01:24
