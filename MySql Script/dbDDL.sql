/*Dhaval Bulsara (1032083)*/
CREATE DATABASE IF NOT EXISTS UBBIDDER;

USE UBBIDDER;

CREATE TABLE IF NOT EXISTS USERMASTER(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    TYPE VARCHAR(10),
    FNAME VARCHAR(20) NOT NULL,
    MNAME VARCHAR(20),
    LNAME VARCHAR(20) NOT NULL,
    SEX CHAR NOT NULL,
    EMAIL VARCHAR(50) NOT NULL,
    PHONE VARCHAR(11) NOT NULL,
    ADDRESS VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS USERLOGIN(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    USER_ID INT NOT NULL,
    USERNAME VARCHAR(20) NOT NULL UNIQUE,
    PASSWORD VARCHAR(20) NOT NULL,
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)
);

CREATE TABLE IF NOT EXISTS USERLOGINLOG(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    USER_ID INT NOT NULL,
    TIMESTAMP TIMESTAMP,
    IP VARCHAR(15),
    ACTION VARCHAR(15),
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)
);

CREATE TABLE IF NOT EXISTS USERPAYMENTMASTER(
    ID INT PRIMARY KEY AUTO_INCREMENT,
    USER_ID INT NOT NULL,
    CARDNO VARCHAR(16) NOT NULL,
    CARDEXP VARCHAR(5) NOT NULL,
    CARDCVV VARCHAR(6) NOT NULL,
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)
);

CREATE TABLE IF NOT EXISTS PROPERTYMASTER(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    POSTINGDATE DATETIME NOT NULL,
    USER_ID INT NOT NULL,
    ADDRESS VARCHAR(100) NOT NULL,
    MIN_PRICE INT,
    TYPE VARCHAR(10),
    STATUS VARCHAR(20) NOT NULL,
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)
);

CREATE TABLE IF NOT EXISTS PROPERTYIMAGES(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    PROPERTYID INT,
    URLONE VARCHAR(100),
    URLTWO VARCHAR(100),
    URLTHREE VARCHAR(100),
    FOREIGN KEY (PROPERTYID) REFERENCES PROPERTYMASTER(ID)
);

CREATE TABLE IF NOT EXISTS BIDDERMASTER(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    BIDDATE DATETIME NOT NULL,
    PROPERTYID INT NOT NULL,
    USER_ID INT NOT NULL,
    BIDAMOUNT INT NOT NULL,
    FOREIGN KEY (PROPERTYID) REFERENCES PROPERTYMASTER(ID),
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)    
);

CREATE TABLE IF NOT EXISTS SOLDMASTER(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    SOLDDATE DATETIME NOT NULL,
    PROPERTYID INT NOT NULL,
    USER_ID INT NOT NULL,
    AMOUNT INT NOT NULL,
    FOREIGN KEY (PROPERTYID) REFERENCES PROPERTYMASTER(ID),
    FOREIGN KEY (USER_ID) REFERENCES USERMASTER(ID)    
);

CREATE TABLE IF NOT EXISTS PORTALMESSAGE(
    ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    DATE DATETIME,
    FROMUSER INT,
    TOUSER INT,
    BODY VARCHAR(5000),
    FOREIGN KEY (FROMUSER) REFERENCES USERMASTER(ID),
    FOREIGN KEY (TOUSER) REFERENCES USERMASTER(ID)
);

DROP VIEW IF EXISTS VIEW_USERDETAILS;
CREATE VIEW VIEW_USERDETAILS AS
SELECT A.TYPE, CONCAT(A.FNAME, ' ', A.LNAME) AS NAME, 
B.USERNAME, B.PASSWORD,  A.SEX, A.EMAIL, A.PHONE  
FROM USERMASTER A INNER JOIN USERLOGIN B 
ON A.ID = B.USER_ID;

DROP VIEW IF EXISTS VIEW_SOLDPROPERTYDETAIL;
CREATE VIEW VIEW_SOLDPROPERTYDETAIL AS
SELECT B.SOLDDATE, A.ADDRESS, B.AMOUNT, CONCAT(C.FNAME, ' ', C.LNAME) AS BUYER FROM PROPERTYMASTER A INNER JOIN SOLDMASTER B INNER JOIN USERMASTER C 
ON A.ID=B.PROPERTYID AND B.USER_ID=C.ID WHERE A.STATUS='SOLD';

DROP PROCEDURE IF EXISTS SP_CREATEUSER;
DELIMITER //

CREATE PROCEDURE SP_CREATEUSER (
    IN PARAM_TYPE VARCHAR(10),
    IN PARAM_FNAME VARCHAR(20),
    IN PARAM_MNAME VARCHAR(20),  
    IN PARAM_LNAME VARCHAR(20), 
    IN PARAM_SEX CHAR(1),
    IN PARAM_EMAIL VARCHAR(50),
    IN PARAM_PHONE VARCHAR(11),
    IN PARAM_ADDRESS VARCHAR(100),
    IN PARAM_USERNAME VARCHAR(20),
    IN PARAM_PASSWORD VARCHAR(20)
)

BEGIN

DECLARE LAST_ID INT;

INSERT INTO USERMASTER VALUES('0', PARAM_TYPE, PARAM_FNAME, PARAM_MNAME, PARAM_LNAME, 
PARAM_SEX, LCASE(PARAM_EMAIL), PARAM_PHONE, PARAM_ADDRESS);

SET LAST_ID = LAST_INSERT_ID();

INSERT INTO USERLOGIN VALUES('0', LAST_ID, PARAM_USERNAME, PARAM_PASSWORD);

SELECT "User Created";

END //

DELIMITER ; //

/* 
CALL CREATEUSER('CONDO', 'DHAVAL','SHAILEN','BULSARA','M','DHAVALBULSARA1994@GMAIL.COM', '4752257594','558 GREGORY ST',
'DHAVALBULSARA','DHAVAL');
*/


DROP PROCEDURE IF EXISTS SP_LOGINCHECK;
DELIMITER //

CREATE PROCEDURE SP_LOGINCHECK (
    IN PARAM_USERNAME VARCHAR(20),
    IN PARAM_PASSWORD VARCHAR(20)
)

BEGIN

IF(SELECT COUNT(*) FROM USERLOGIN A INNER JOIN USERMASTER B ON A.USER_ID = B.ID WHERE A.USERNAME=PARAM_USERNAME AND BINARY PASSWORD=PARAM_PASSWORD)
THEN

BEGIN
SELECT * FROM USERLOGIN A INNER JOIN USERMASTER B ON A.USER_ID = B.ID WHERE A.USERNAME=PARAM_USERNAME AND BINARY PASSWORD=PARAM_PASSWORD;
END;
ELSE
BEGIN
SELECT '0' AS USERNAME, '0' AS PASSWORD, '0' as ID;
END;
END IF;
END//
DELIMITER ; //


DROP PROCEDURE IF EXISTS SP_DEFAULTBID;
DELIMITER //

CREATE PROCEDURE SP_DEFAULTBID ()

BEGIN
insert into BIDDERMASTER select '0', current_timestamp, ID, USER_ID, MIN_PRICE from PROPERTYMASTER where status = 'OPEN' and not EXISTS( select * from BIDDERMASTER where PROPERTYMASTER.ID = BIDDERMASTER.PROPERTYID);
END//
DELIMITER ; //