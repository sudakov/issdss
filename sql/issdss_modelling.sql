----------------------------------------------------------  model  ----------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_Read')
DROP PROCEDURE issdss_model_Read
GO
CREATE PROCEDURE issdss_model_Read
    @ModelName varchar(255)
AS
BEGIN
    SELECT id
    FROM dbo.model
    WHERE name = @ModelName
END
GO



IF EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_Read_All')
DROP PROCEDURE issdss_model_Read_All
GO
CREATE PROCEDURE issdss_model_Read_All
	@UserID int = NULL,
	@TaskID int = NULL
AS
BEGIN
	SELECT name, description FROM model
	ORDER BY name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_read_id')
DROP PROCEDURE issdss_model_read_id
GO
CREATE PROCEDURE issdss_model_read_id
    @ThisAlternativeId int
AS
BEGIN
    SELECT model_id
    FROM dbo.alternative
    WHERE id = @ThisAlternativeId
END



----------------------------------------------------------  alternative  ----------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Update_Trace')
DROP PROCEDURE issdss_alternative_Update_Trace
GO
CREATE PROCEDURE issdss_alternative_Update_Trace
    @TraceText varchar(MAX),
    @AlternativeID int
AS
BEGIN
    UPDATE dbo.alternative
		SET trace_text = @TraceText
    WHERE id = @AlternativeID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_model_Read')
DROP PROCEDURE issdss_alternative_model_Read
GO
CREATE PROCEDURE issdss_alternative_model_Read
    @ThisAlternativeId int
AS
BEGIN
    SELECT name
    FROM dbo.model
    WHERE model.id IN 
		(
		 SELECT model_id 
		 FROM dbo.alternative 
		 WHERE alternative.id = @ThisAlternativeId
		)
END
GO

----------------------------------------------------------  model_crit  ----------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_crit_values')
DROP PROCEDURE issdss_model_crit_values
GO
CREATE PROCEDURE issdss_model_crit_values
    @ThisAlternativeId int
AS
BEGIN
    SELECT value 
    FROM issdss.dbo.crit_value 
    WHERE crit_value.criteria_id IN
		(
		 SELECT id 
		 FROM issdss.dbo.criteria
		 LEFT JOIN issdss.dbo.model_crit ON criteria.id = model_crit.id_crit
		 WHERE model_crit.is_param = 'true' 
		 AND crit_value.alternative_id = @ThisAlternativeId
		)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_crit_Read_Code_Params')
DROP PROCEDURE issdss_model_crit_Read_Code_Params
GO
CREATE PROCEDURE issdss_model_crit_Read_Code_Params
	@ThisAlternativeID int
AS
BEGIN
    SELECT code 
    FROM dbo.model_crit 
    WHERE id_crit IN
		(
		 SELECT criteria_id 
		 FROM dbo.crit_value 
		 WHERE alternative_id = @ThisAlternativeID
		)	   AND is_param = 'true'
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_model_crit_Read_korm')
DROP PROCEDURE issdss_model_crit_Read_korm
GO
CREATE PROCEDURE issdss_model_crit_Read_korm
	@ThisAlternativeID int
AS
BEGIN
    SELECT id_crit 
    FROM issdss.dbo.model_crit 
    WHERE id_crit IN
					(
					 SELECT criteria_id 
					 FROM issdss.dbo.crit_value 
					 WHERE alternative_id = @ThisAlternativeID
					) 
				  AND is_param = 'false'
END
GO
----------------------------------------------------------  crit_value  ----------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_value_Insert_KORM')
DROP PROCEDURE issdss_crit_value_Insert_KORM
GO
CREATE PROCEDURE issdss_crit_value_Insert_KORM
    @Value decimal(18,6),
    @Code varchar(255)
AS
BEGIN
    UPDATE dbo.crit_value
	SET value = @Value 
	WHERE crit_value.criteria_id = (SELECT id_crit 
									FROM dbo.model_crit 
									WHERE code = @Code)

END
GO