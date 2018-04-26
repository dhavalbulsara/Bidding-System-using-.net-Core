/*DHAVAL BULSARA (1032083)*/

/* DISPLAY WHICH USER BOUGHT WHICH PROPERTY WITH AMOUNT */

SELECT B.SOLDDATE, A.ADDRESS, B.AMOUNT, CONCAT(C.FNAME, ' ', C.LNAME) AS BUYER FROM PROPERTYMASTER A INNER JOIN SOLDMASTER B INNER JOIN USERMASTER C ON A.ID=B.PROPERTYID AND B.USER_ID=C.ID WHERE A.STATUS='SOLD' ORDER BY BUYER;
/*+---------------------+-------------------+--------+-------------+
| SOLDDATE            | ADDRESS           | AMOUNT | BUYER       |
+---------------------+-------------------+--------+-------------+
| 2018-04-06 21:35:53 | 433, PARK AVENUE  |  40000 | AVI PATEL   |
| 2018-04-06 21:35:53 | 43, WALNUT STREET | 100000 | AVI PATEL   |
| 2018-04-06 21:35:53 | 2, RENNELL STREET | 100000 | SUNIL DUDHE |
+---------------------+-------------------+--------+-------------+*/


/* DISPLAY NUMBER OF PROPERTIES THAT ARE SOLD  BY USERS*/

SELECT B.FNAME,COUNT(A.ID) FROM PROPERTYMASTER A INNER JOIN USERMASTER B ON A.USER_ID=B.ID WHERE A.STATUS='SOLD' GROUP BY B.FNAME ;
/*+----------+-------------+
| FNAME    | COUNT(A.ID) |
+----------+-------------+
| ADITYA   |           1 |
| ABHI     |           1 |
| KRUTARTH |           1 |
+----------+-------------+*/

/* LIST OF ALL USER WHO HAVE LISTED MORE THAN ONE PROPERTY*/

SELECT B.FNAME FROM PROPERTYMASTER A INNER JOIN USERMASTER B ON A.USER_ID=B.ID  GROUP BY B.FNAME HAVING COUNT(A.USER_ID)>1 ;
/*+----------+
| FNAME    |
+----------+
| ADITYA   |
| KRUTARTH |
| ABHI     |
+----------+*/

/* LIST OF ALL USER WHO HAVE LOGGED OUT*/

SELECT A.TYPE, CONCAT(A.FNAME, ' ', A.LNAME) AS NAME, 
B.USERNAME, B.PASSWORD,  A.SEX, A.EMAIL, A.PHONE  
FROM USERMASTER A INNER JOIN USERLOGIN B 
ON A.ID = B.USER_ID WHERE A.ID IN (SELECT C.USER_ID FROM USERLOGINLOG C WHERE C.ACTION='LOGOUT');

/*+--------+-----------------+----------------+----------+-----+--------------------------+------------+
| TYPE   | NAME            | USERNAME       | PASSWORD | SEX | EMAIL                    | PHONE      |
+--------+-----------------+----------------+----------+-----+--------------------------+------------+
| CLIENT | ADITYA SHUKLA   | ADI SHUKLA     | ADITYA   | M   | ADISHUKLA@GMAIL.COM      | 4752257594 |
| CLIENT | ABHI PATEL      | ABHIPATEL      | ABHI     | M   | ABHIPATEL@GMAIL.COM      | 4752257594 |
| CLIENT | KRUTARTH SAILOR | KRUTARTHSAILOR | KRUTARTH | M   | KRUTARTHSAILOR@GMAIL.COM | 4752257594 |
+--------+-----------------+----------------+----------+-----+--------------------------+------------+*/

/* LIST OF ALL MESSAGES EXCHANGED*/

SELECT (SELECT FNAME FROM USERMASTER WHERE ID=A.FROMUSER) AS FROMUSER, 
(SELECT FNAME FROM USERMASTER WHERE ID=A.TOUSER) AS TOUSER,
BODY  FROM PORTALMESSAGE A;

/*+----------+----------+--------------+
| FROMUSER | TOUSER   | BODY         |
+----------+----------+--------------+
| ADITYA   | DHAVAL   | TEST MESSAGE |
| AMAN     | ABHI     | TEST MESSAGE |
| KRUTARTH | AVI      | TEST MESSAGE |
| SUNIL    | KRUTARTH | TEST MESSAGE |
| AVI      | SUNIL    | TEST MESSAGE |
| ABHI     | AMAN     | TEST MESSAGE |
| DHAVAL   | ADITYA   | TEST MESSAGE |
+----------+----------+--------------+*/
