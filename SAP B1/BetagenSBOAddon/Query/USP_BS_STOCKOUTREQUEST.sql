CREATE PROCEDURE "USP_BS_STOCKOUTREQUEST"
  IN StockType char,
  IN FromDate DATE,
  IN ToDate DATE,
  IN User INTEGER
)

AS
BEGIN
    SELECT DISTINCT
      FALSE AS "Choo",
      "StockNo",
      "StockDate",
      "StockType",
      t0."FromWhsCode",
      t0."FromWhsName",
      t0."ToWhsCode",
      "ToWhsName",
      "CustCode",
      "CustName",
      "TruckNo",
      "Note",
      "StatusSAP",
      (CASE WHEN t0."StatusSAP" = 0 
      		THEN 'No' 
      		ELSE 'Yes' 
      	END) AS "ApplySAP",
      "DateCreate",
      "DateUpdate",
      t0."OrderType",
      t1."Name",
      t0."AbsID",
      t0."BpCode",
      t2."BpName" AS "BPName",
      "AbsEntry",
      "BinCode",
      "AbsEntry1",
      "BinCode1",
      t0."UserID",
      t3."U_NAME" AS "UserName",
      t4."DocNum" AS "TransferReqNo",
      t5."DocNum" AS "TransferNo",
      t0."Canceled",
      CASE WHEN IFNULL(t5."U_DeliveryStatus", '') = '01' THEN n'?ang giao h�ng' 
           WHEN IFNULL(t5."U_DeliveryStatus", '') = '02' THEN n'?� giao xong' 
           ELSE '' 
       END AS "DeliveryStatus",
      CASE WHEN t4."U_Confirm" = '02' THEN 'Confirmed' 
      	   ELSE 'Not Yet Confirm' 
       END AS "Confirm",
      0 AS "TotalWeight",
      "ApplyStatus",
      IFNULL(t0."ApplySAPRemark", '') AS "ApplySAPRemark"
    FROM "BS_STOCKOUTREQUEST" t0
    LEFT JOIN "@BS_ORDERTYPE" t1 ON t0."OrderType" = t1."Code"
    LEFT JOIN OOAT t2 ON t0."AbsID" = t2."AbsID"
    LEFT JOIN OUSR t3 ON t0."UserID" = t3.USERID
    LEFT JOIN OWTQ t4 ON t0."StockNo" = t4."U_SoPhieu" AND (t4."DocStatus" = 'O' OR (t4."U_Confirm" = '02' AND t4."DocStatus" = 'C'))
    LEFT JOIN OWTR t5 ON t0."StockNo" = t5."U_SoPhieu"
    WHERE (:StockType = '' OR "StockType" = :StockType)
      AND "StockDate" BETWEEN :fromDate AND :ToDate
     ORDER BY "DateCreate" DESC;
  
END;