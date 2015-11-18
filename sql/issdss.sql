----------------------------------------------------------  alternative  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Delete')
DROP PROCEDURE issdss_alternative_Delete
GO
CREATE PROCEDURE issdss_alternative_Delete
    @AlternativeID int
AS
BEGIN
    DELETE FROM crit_value
    WHERE alternative_id = @AlternativeID
    DELETE FROM person_alternative
    WHERE alternative_id = @AlternativeID
    DELETE FROM alternative
    WHERE id = @AlternativeID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Insert')
DROP PROCEDURE issdss_alternative_Insert
GO
CREATE PROCEDURE issdss_alternative_Insert
    @TaskID int,
    @Name varchar(255),
    @UserID int
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from alternative),0) + 1
    INSERT INTO dbo.alternative(id, task_id, name)
    VALUES (@ID, @TaskID, @Name)
    
    DECLARE @IDPA int
    SET @IDPA = isnull((select max(id) from person_alternative),0) + 1
    INSERT INTO dbo.person_alternative(id, alternative_id, person_id)
    VALUES (@IDPA, @ID, @UserID)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Read')
DROP PROCEDURE issdss_alternative_Read
GO
CREATE PROCEDURE issdss_alternative_Read
    @AlternativeID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id, name
    FROM dbo.alternative
    WHERE id = @AlternativeID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Read_All')
DROP PROCEDURE issdss_alternative_Read_All
GO
CREATE PROCEDURE issdss_alternative_Read_All
    @TaskID int = NULL
AS
BEGIN
	SELECT a.id, a.name, a.rank,
			--replace((select j.name as 'data()' from job j where j.alternative_id = a.id for xml path('')),' ',', ') as jobs
			replace(replace(replace((select j.name as q from job j where j.alternative_id = a.id order by j.ord for xml path('')),'</q><q>',', '),'<q>',''),'</q>','') as jobs
	FROM alternative a
	WHERE a.task_id = @TaskID
	ORDER by a.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_Update')
DROP PROCEDURE issdss_alternative_Update
GO
CREATE PROCEDURE issdss_alternative_Update
    @AlternativeID int,
    @Name varchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.alternative
        SET name = @Name
    WHERE id = @AlternativeID
END
GO



-------------------------------------------------  alternative_person  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_person_Read')
DROP PROCEDURE issdss_alternative_person_Read
GO
CREATE PROCEDURE issdss_alternative_person_Read
    @TaskID int = NULL,
    @AlternativeID int = NULL
AS
BEGIN
    SELECT 0 AS id, (SELECT name FROM alternative WHERE id = @AlternativeID) AS name, 0 AS isChecked, 0 AS part
    UNION
    SELECT  p.id AS id,
            p.name AS name,
            case
                when exists(select 1
                            from person_alternative
                            where alternative_id = @AlternativeID AND person_id = p.id) then 1
                else 0
            end AS isChecked,
            1 AS part
    FROM person p
    WHERE EXISTS(select 1 from person_task where task_id = @TaskID AND person_id = p.id)
    ORDER by part, name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_alternative_person_Update')
DROP PROCEDURE issdss_alternative_person_Update
GO
CREATE PROCEDURE issdss_alternative_person_Update
    @AlternativeID int,
    @Person_ids varchar(max)
AS
BEGIN
        DECLARE @max_id int
       
        DELETE person_alternative WHERE alternative_id = @AlternativeID

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_alternative),0)
       
        INSERT INTO person_alternative
        ( id, person_id, alternative_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY p.id) + @max_id,
                        p.id,
                        @AlternativeID
        FROM
                        person p
        WHERE
                        @Person_ids like '%,' + CONVERT(varchar(10), p.id) + ',%'                        
END
GO



----------------------------------------------------------  crit_scale  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Delete')
DROP PROCEDURE issdss_crit_scale_Delete
GO
CREATE PROCEDURE issdss_crit_scale_Delete
    @CriteriaID int,
    @ScaleID int
AS
BEGIN
    DELETE FROM dbo.crit_scale
    WHERE id = @ScaleID
    EXEC issdss_crit_scale_Read @CriteriaID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Insert')
DROP PROCEDURE issdss_crit_scale_Insert
GO
CREATE PROCEDURE issdss_crit_scale_Insert
    @CriteriaID int,
    @Name varchar(255) = NULL,
    @Rank decimal (18,6) = NULL,
    @Ord int = NULL
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from crit_scale),0) + 1
    INSERT INTO dbo.crit_scale(id, criteria_id, name, rank, ord)
    VALUES (@ID, @CriteriaID, @Name, @Rank, @Ord)
    EXEC issdss_crit_scale_Read @CriteriaID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Read')
DROP PROCEDURE issdss_crit_scale_Read
GO
CREATE PROCEDURE issdss_crit_scale_Read
     @CriteriaID int = NULL
AS
BEGIN
     SELECT id, criteria_id, name, ord, rank
     FROM crit_scale
     WHERE criteria_id = @CriteriaID
     ORDER BY ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Read_All')
DROP PROCEDURE issdss_crit_scale_Read_All
GO
CREATE PROCEDURE issdss_crit_scale_Read_All
    @TaskID int = NULL,
    @PersonID int = NULL,
    @AlternativeID int = NULL
AS
BEGIN
    SELECT        c.id    AS c_id,
                c.name  AS c_name,
                --cs.name AS value_for_view,
                case
                    when exists(select 1 from resource r where r.criteria_id = c.id) then CONVERT(varchar(30), (select SUM(jr.value) from job j left join job_resource jr on j.id = jr.job_id left join resource res on jr.resource_id = res.id where j.alternative_id = @AlternativeID and res.criteria_id = c.id))
					else cs.name
				end AS value_for_view,
                case
                    when exists(select 1 from resource r where r.criteria_id = c.id) then 1
					else 0
				end AS is_resourse,
				cs.id   AS scale_id,
                cs.ord    AS cs_ord,
                cv.crit_scale_id AS current_scale_id
    FROM crit_scale AS cs INNER JOIN
            criteria AS c ON cs.criteria_id = c.id LEFT OUTER JOIN
            crit_value AS cv ON cv.criteria_id = c.id AND cv.person_id = @PersonID AND cv.alternative_id = @AlternativeID
    WHERE (c.task_id = @TaskID)AND
            (NOT EXISTS (SELECT 1 AS Expr1
                            FROM criteria
                            WHERE (parent_crit_id = c.id)))
    UNION
    SELECT        c.id AS c_id,
                c.name AS c_name,
                --CONVERT(varchar(30), cv.value) AS value_for_view,
                case
                    when exists(select 1 from resource r where r.criteria_id = c.id) then CONVERT(varchar(30), (select SUM(jr.value) from job j left join job_resource jr on j.id = jr.job_id left join resource res on jr.resource_id = res.id where j.alternative_id = @AlternativeID and res.criteria_id = c.id))
					else CONVERT(varchar(30), cv.value)
				end AS value_for_view,
                case
                    when exists(select 1 from resource r where r.criteria_id = c.id) then 1
					else 0
				end AS is_resourse,
                NULL AS scale_id,
                NULL AS cs_ord,
                NULL AS current_scale_id
    FROM criteria AS c LEFT OUTER JOIN
            crit_value AS cv ON cv.criteria_id = c.id AND cv.person_id = @PersonID AND cv.alternative_id = @AlternativeID
    WHERE (c.task_id = @TaskID) AND
            (NOT EXISTS (SELECT 1 AS Expr1
                            FROM crit_scale AS cs
                            WHERE (criteria_id = c.id))) AND
            (NOT EXISTS (SELECT 1 AS Expr1
                            FROM criteria
                            WHERE (parent_crit_id = c.id)))
    UNION
    SELECT        0 AS c_id,
                NULL AS c_name,
                a.name AS value_for_view,
                0 AS is_resourse,
                NULL AS scale_id,
                NULL AS cs_ord,
                NULL AS current_scale_id
    FROM alternative AS a
    WHERE (a.id = @AlternativeID)
    ORDER BY c_id, cs_ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Read_by_ID')
DROP PROCEDURE issdss_crit_scale_Read_by_ID
GO
CREATE PROCEDURE issdss_crit_scale_Read_by_ID
     @ScaleID int = NULL
AS
BEGIN
     SELECT id, criteria_id, name, ord, rank
     FROM crit_scale
     WHERE id = @ScaleID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_scale_Update')
DROP PROCEDURE issdss_crit_scale_Update
GO
CREATE PROCEDURE issdss_crit_scale_Update
    @CriteriaID int,
    @ID int,
    @Name varchar(255) = NULL,
    @Rank decimal (18,6) = NULL,
    @Ord int = NULL
AS
BEGIN
    UPDATE dbo.crit_scale
        SET name = @Name,
        rank = @Rank,
        ord = @Ord
    WHERE id = @ID
    EXEC issdss_crit_scale_Read @CriteriaID
END
GO



----------------------------------------------------------  crit_value  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_value_Read_All')
DROP PROCEDURE issdss_crit_value_Read_All
GO
CREATE PROCEDURE issdss_crit_value_Read_All
    @TaskID int = NULL,
    @PersonID int = NULL
AS
BEGIN
    select
            a.id                            a_id,  
            a.name                          a_name,
            cv.person_id                    person_id,
            (select name from person where id = cv.person_id) p_name,
            c.id                            c_id,
            c.name                          c_name,
            cv.value                        cv_value,
            cv.crit_scale_id        crit_scale_id,
            case
				when exists(select 1 from crit_scale cs where cs.criteria_id=c.id) then 1
                else 0
			end								is_scale,
            case
                when exists(select 1 from resource r where r.criteria_id = c.id) then CONVERT(varchar(30), (select SUM(jr.value) from job j left join job_resource jr on j.id = jr.job_id left join resource res on jr.resource_id = res.id where j.alternative_id = a.id and res.criteria_id = c.id))
				when exists(select 1 from crit_scale cs where cs.criteria_id=c.id) then (select name from crit_scale cs where cs.id = cv.crit_scale_id)
                else convert(varchar(30),cv.value)
			end								value_for_view
    from
            alternative a
            join criteria c on a.task_id = c.task_id
            left outer join crit_value cv on cv.alternative_id = a.id and c.id = cv.criteria_id and cv.person_id = @PersonID
    where
            a.task_id = @TaskID AND not exists( select 1 from criteria where parent_crit_id=c.id)
   and (@PersonID IS null or exists(select 1 from person_alternative where person_id = @PersonID and alternative_id = a.id) )
    order by a.name--, c.name
    --order by a.name, p_name, c.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_value_Read_Ranks')
DROP PROCEDURE issdss_crit_value_Read_Ranks
GO
CREATE PROCEDURE issdss_crit_value_Read_Ranks
    @TaskID int = NULL
AS
BEGIN
    select
            a.id                            a_id, 
            a.name                          a_name,
            c.id                            c_id,
            c.name                          c_name,
            (select value
                from    crit_value
                where    person_id IS NULL and
                        criteria_id = c.id and
                        alternative_id = a.id)
                                            value_for_view,
            ISNULL(a.rank,0)                rank
    from
            alternative a
            join criteria c on a.task_id = c.task_id 
            left outer join crit_value cv on cv.alternative_id = a.id and c.id = cv.criteria_id
    where 
            a.task_id = @TaskID
    group by a.id ,  
            a.name,
            c.id,
            c.name,
            a.rank
    order by a.rank desc, a.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_crit_value_Update')
DROP PROCEDURE issdss_crit_value_Update
GO
CREATE procedure issdss_crit_value_Update
    @d xml,
    @alternative_id int,
    @person_id int
as
begin
        DECLARE @max_id int
       
        create table #t( criteria_id int, value decimal(18,4))
       
        delete crit_value where person_id = @person_id and alternative_id = @alternative_id

        insert into #t (criteria_id, value)
        SELECT
                        d.r.value('criteria_id[1]', 'int') criteria_id,
                        d.r.value('value[1]', 'decimal(18,4)') value
        FROM
                        @d.nodes('/crit_value/row') d(r)  

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM crit_value),0)
       
        INSERT INTO crit_value
        ( id, person_id, criteria_id, alternative_id, value, crit_scale_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY t.criteria_id) + @max_id,
                        @person_id,
                        t.criteria_id,
                        @alternative_id,
                        case when s.id is null then t.value
                        else null
                        end,
                        s.id
        from
                        #t t
                        left outer join crit_scale s on s.criteria_id = t.criteria_id and s.id =  t.value

        drop table #t
end
GO



----------------------------------------------------------  criteria   ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_ChooseParent')
DROP PROCEDURE issdss_criteria_ChooseParent
GO
CREATE PROCEDURE issdss_criteria_ChooseParent
    @id int = NULL
AS
BEGIN
    DECLARE    @name varchar(max)
    SET @name = (SELECT name FROM criteria WHERE id=@id)
    DECLARE    @task_id int
    SET @task_id = (SELECT task_id FROM criteria WHERE id=@id)

    DECLARE @lev int
    CREATE TABLE #t( id int, lev int, name varchar(max) )
    SELECT @lev = 0
    INSERT INTO #t VALUES( @id, @lev, @name )
    WHILE EXISTS( SELECT 1 FROM criteria
                  WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = @lev ))
          AND @lev < 10000 -- защита от зацикливания
    BEGIN
            SELECT @lev = @lev + 1
            INSERT INTO #t
            SELECT id, @lev, name FROM criteria
            WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = (@lev-1) )
    END
    
    --SELECT * FROM #t

    SELECT id, name, task_id FROM criteria WHERE id NOT IN (SELECT id FROM #t) AND task_id = @task_id ORDER by name
    DROP TABLE #t
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Delete')
DROP PROCEDURE issdss_criteria_Delete
GO
CREATE PROCEDURE issdss_criteria_Delete
    @id int = 8
AS
BEGIN
        DECLARE @lev int
        CREATE TABLE #t( id int, lev int)
        SELECT @lev = 0
        INSERT INTO #t VALUES( @id, @lev )
        WHILE EXISTS( SELECT 1 FROM criteria
                      WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = @lev ))
              AND @lev < 10000 -- защита от зацикливания
        BEGIN
                SELECT @lev = @lev + 1
                INSERT INTO #t
                SELECT id, @lev FROM criteria
                WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = (@lev-1) )
        END
        WHILE @lev >=0
        BEGIN
                DELETE crit_value FROM #t WHERE #t.id = crit_value.criteria_id and #t.lev = @lev
                DELETE crit_scale FROM #t WHERE #t.id = crit_scale.criteria_id and #t.lev = @lev
                DELETE pair_crit_comp FROM #t WHERE #t.id IN (
                 pair_crit_comp.criteria1_id, pair_crit_comp.criteria2_id
                 ) and #t.lev = @lev
                DELETE preference FROM #t WHERE #t.id = preference.criteria_id and #t.lev = @lev
                DELETE criteria FROM #t WHERE #t.id = criteria.id and #t.lev = @lev
                SELECT @lev = @lev - 1
        END
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Insert')
DROP PROCEDURE issdss_criteria_Insert
GO
CREATE PROCEDURE issdss_criteria_Insert
    @TaskID int,
    @Name varchar(255) = NULL,
    @Description varchar(MAX) = NULL,
    @Parent_ID int = NULL,
    @IsMin int = 1,
    @IdealValue decimal (18,6) = NULL,
    @Method_ID int = NULL,
    @IsNumber bit = NULL,
    @Ord int = NULL
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from criteria),0) + 1
    INSERT INTO dbo.criteria(id, task_id, name, description, parent_crit_id, rank, ismin, idealvalue, method_id, is_number, ord)
    VALUES (@ID, @TaskID, @Name, @Description, @Parent_ID, NULL, @IsMin, @IdealValue, @Method_ID, @IsNumber, @Ord)
    SELECT @ID as id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Insert_Main')
DROP PROCEDURE issdss_criteria_Insert_Main
GO
CREATE PROCEDURE issdss_criteria_Insert_Main
    @TaskID int,
    @Name varchar(255),
    @Rank decimal(18,6) = NULL,
    @IsMin int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ID int
    SET @ID = isnull((select max(id) from criteria),0) + 1
    INSERT INTO dbo.criteria(id, [task_id], [name], [parent_crit_id], [rank], [ismin])
    VALUES (@ID, @TaskID, @Name, NULL, @Rank, @IsMin)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read')
DROP PROCEDURE issdss_criteria_Read
GO
CREATE PROCEDURE [dbo].[issdss_criteria_Read]
    @ParentID int = NULL
AS
BEGIN
    SELECT id, name, parent_crit_id, rank
    FROM dbo.criteria
    WHERE @ParentID=parent_crit_id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_All')
DROP PROCEDURE issdss_criteria_Read_All
GO
CREATE PROCEDURE issdss_criteria_Read_All
    @TaskID int = NULL
AS
BEGIN
    SELECT id, name, parent_crit_id, description
    FROM dbo.criteria
    WHERE task_id = @TaskID
    ORDER by name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_AllNames')
DROP PROCEDURE issdss_criteria_Read_AllNames
GO
CREATE PROCEDURE issdss_criteria_Read_AllNames
    @TaskID int = NULL
AS
BEGIN
--SELECT     0 AS c_id, 'Ранг' AS c_name, 'Результирующий ранг оценки альтернатив' as description, NULL AS parent, 0 AS rank
--UNION
SELECT     id AS c_id, name AS c_name, description as description, parent_crit_id AS parent, 2 AS rank
FROM         criteria AS c
WHERE     (task_id = @TaskID)
UNION
SELECT     id AS c_id, name AS c_name, description as description, id AS parent, 1 AS rank
FROM         criteria AS c
WHERE     (task_id = @TaskID) AND EXISTS
                          (SELECT     1
                            FROM          criteria
                            WHERE      (parent_crit_id = c.id))
ORDER BY rank, c_name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_by_id')
DROP PROCEDURE issdss_criteria_Read_by_id
GO
CREATE PROCEDURE issdss_criteria_Read_by_id
    @ID int = NULL
AS      
BEGIN
    SELECT task_id, name, description, rank, ismin, parent_crit_id, idealvalue, method_id, is_number, ord,
        case when exists(select 1 from criteria where parent_crit_id = @ID) then 1 else 0 end is_parent
    FROM dbo.criteria
 WHERE @ID=id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_by_Parent')
DROP PROCEDURE issdss_criteria_Read_by_Parent
GO
CREATE PROCEDURE issdss_criteria_Read_by_Parent
@ParentID int
AS
BEGIN
    SELECT id, name, parent_crit_id
    FROM dbo.criteria
    WHERE @ParentID=parent_crit_id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_Names')
DROP PROCEDURE issdss_criteria_Read_Names
GO
CREATE PROCEDURE issdss_criteria_Read_Names
    @TaskID int = NULL
AS
BEGIN
    SELECT id, name, parent_crit_id, description
    FROM dbo.criteria c
    WHERE task_id = @TaskID
    ORDER by ord, name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Read_Names_by_Order')
DROP PROCEDURE issdss_criteria_Read_Names_by_Order
GO
CREATE PROCEDURE issdss_criteria_Read_Names_by_Order
    @TaskID int = NULL
AS
BEGIN
	with t ( lev, id, name, ord, is_parent, description ) as
	(
	select	5 lev,
			c.id id,
			c.name name,
			cast(c.ord as varchar(255)) ord,
			case
				when exists(select 1 from criteria where parent_crit_id = c.id) then 1
				else 0
			end is_parent,
			c.description
	from criteria c  where c.task_id = @TaskID and c.parent_crit_id is null

	union all

	select	t.lev+20,
			c.id,
			cast(replicate('', t.lev) + c.name as  varchar(255)),
			cast( (t.ord + '|' + cast(c.ord as varchar(255))) as varchar(255)),
			case
				when exists(select 1 from criteria where parent_crit_id = c.id) then 1
				else 0
			end is_parent,
			c.description
	from t join criteria c on c.parent_crit_id = t.id
	)
	select * from t order by ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Update')
DROP PROCEDURE issdss_criteria_Update
GO
CREATE PROCEDURE issdss_criteria_Update
    @ID int,
    @Name varchar(255) = NULL,
    @Description varchar(MAX) = NULL,
    @Parent_ID int = NULL,
    @IsMin int = 1,
    @IdealValue decimal (18,6) = NULL,
    @Method_ID int = NULL,
    @IsNumber bit = NULL,
    @Ord int = NULL
AS
BEGIN
    UPDATE dbo.criteria
        SET name = @Name,
        description = @Description,
        parent_crit_id = @Parent_ID,
        ismin = @IsMin,
        idealvalue = @IdealValue,
        method_id = @Method_ID,
        is_number = @IsNumber,
        ord = @Ord
    WHERE id = @ID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Update_Method')
DROP PROCEDURE issdss_criteria_Update_Method
GO
CREATE PROCEDURE issdss_criteria_Update_Method
    @ID int,
    @method_id int
AS
BEGIN
    UPDATE dbo.criteria
        SET method_id = @method_id
    WHERE id = @ID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Update_Parent')
DROP PROCEDURE issdss_criteria_Update_Parent
GO
CREATE PROCEDURE issdss_criteria_Update_Parent
    @ID int,
    @ParentID int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.criteria
        SET parent_crit_id = @ParentID
    WHERE id = @ID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_Update_Rank')
DROP PROCEDURE issdss_criteria_Update_Rank
GO
CREATE PROCEDURE issdss_criteria_Update_Rank
    @CriteriaID int,
    @Rank decimal(18, 6)
AS
BEGIN
    UPDATE dbo.criteria
        SET rank = @Rank
    WHERE id = @CriteriaID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_criteria_read_for_parent')
DROP PROCEDURE issdss_criteria_read_for_parent
GO
create procedure issdss_criteria_read_for_parent (
	@task_id int,     -- нужно передать обязатаельно если критерий не создан пока
	@criteria_id int  -- если критерий уже есть, то какая задача и так понятно
)
as
begin
	if @criteria_id is null 
	begin
		select id, name from criteria where task_id = @task_id
	end
	else
	begin
		select @task_id = task_id from criteria where id = @criteria_id;
		WITH criteria_child (id, child_id, level)
		AS
		(
			SELECT c.id id, c.id child_id, 0 AS level
			FROM criteria c
			WHERE c.id = @criteria_id
			UNION ALL
			SELECT a.id id, cc.id child_id, a.level + 1
			FROM criteria_child a join criteria cc on cc.parent_crit_id = a.child_id
		)
		select id, name from criteria where task_id = @task_id and
		id not in (select child_id from criteria_child)		
	end
	
	return 1
end
GO



----------------------------------------------------------  job  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_Delete')
DROP PROCEDURE issdss_job_Delete
GO
CREATE PROCEDURE issdss_job_Delete
    @AlternativeID int,
    @JobID int
AS
BEGIN
    DELETE FROM job_loop
    WHERE parent_job_id = @JobID OR child_job_id = @JobID
    DELETE FROM job_resource
    WHERE job_id = @JobID
    DELETE FROM plan_job
    WHERE job_id = @JobID
    DELETE FROM job
    WHERE id = @JobID
    EXEC issdss_job_Read_All @AlternativeID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_for_Gantt')
DROP PROCEDURE issdss_job_for_Gantt
GO
CREATE PROCEDURE issdss_job_for_Gantt
    @PlanID int = NULL
AS
BEGIN
	select '' as a_id, 'plan' as type, 0 as j_id, 0 as j_ord, name, '' as start_date, '' as end_date from plandss where id = @PlanID
	
	union

	select a.name as a_id, 'alternative' as type, 0 as j_id, 0 as j_ord, a.name, '' as start_date, '' as end_date from alternative a where task_id = (select task_id from plandss where id=@PlanID)

	union

	select	(select name from alternative where id = j.alternative_id) as a_id,
			'job' as type,
			j.id as j_id,
			j.ord as j_ord,
			j.name,
			(select begin_date from plan_job where plan_id = @PlanID AND job_id = j.id) as start_date,
			(select end_date from plan_job where plan_id = @PlanID AND job_id = j.id) as end_date
	from job j
	where alternative_id in (select id from alternative where task_id = (select task_id from plandss where id=@PlanID) )

	order by 1, 4
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_Insert')
DROP PROCEDURE issdss_job_Insert
GO
CREATE PROCEDURE issdss_job_Insert
    @AlternativeID int,
    @Name varchar(255),
    @Duration decimal(18,6),
    @MeasureID int,
    @Ord int = NULL
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from job),0) + 1
    INSERT INTO job(id, name, alternative_id, duration, measure_id, ord)
    VALUES (@ID, @Name, @AlternativeID, @Duration, @MeasureID, @Ord)
    EXEC issdss_job_Read_All @AlternativeID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_Read')
DROP PROCEDURE issdss_job_Read
GO
CREATE PROCEDURE issdss_job_Read
     @JobID int = NULL
AS
BEGIN
     SELECT id, name, duration, measure_id, ord
     FROM job
     WHERE id = @JobID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_Read_All')
DROP PROCEDURE issdss_job_Read_All
GO
CREATE PROCEDURE issdss_job_Read_All
    @AlternativeID int = NULL
AS
BEGIN
	SELECT j.id, j.name, j.duration, j.measure_id, m.name m_name, j.ord,
		   --replace((select job.name as 'data()' from job_loop jl LEFT JOIN job ON jl.parent_job_id = job.id where jl.child_job_id = j.id for xml path('')),' ',', ') as parent_jobs
		   replace(replace(replace((select job.name as q from job_loop jl LEFT JOIN job ON jl.parent_job_id = job.id where jl.child_job_id = j.id for xml path('')),'</q><q>',', '),'<q>',''),'</q>','') as parent_jobs
	FROM job j LEFT JOIN measure m ON j.measure_id = m.id
	WHERE alternative_id = @AlternativeID
	ORDER by ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_Update')
DROP PROCEDURE issdss_job_Update
GO
CREATE PROCEDURE issdss_job_Update
    @AlternativeID int,
    @JobID int,
    @Name varchar(255),
    @Duration decimal(18,6),
	@MeasureID int,
    @Ord int = NULL
AS
BEGIN
    UPDATE job
        SET name = @Name,
        duration = @Duration,
        measure_id = @MeasureID,
        ord = @Ord
    WHERE id = @JobID
    EXEC issdss_job_Read_All @AlternativeID
END
GO



----------------------------------------------------------  job_loop  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_loop_Delete')
DROP PROCEDURE issdss_job_loop_Delete
GO
CREATE PROCEDURE issdss_job_loop_Delete
    @JobID int,
    @ParentJobID int
AS
BEGIN
    DELETE FROM job_loop
    WHERE parent_job_id = @ParentJobID AND child_job_id = @JobID
    EXEC issdss_job_loop_Read @JobID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_loop_Insert')
DROP PROCEDURE issdss_job_loop_Insert
GO
CREATE PROCEDURE issdss_job_loop_Insert
    @JobID int,
    @ParentJobID int
AS
BEGIN
    INSERT INTO job_loop(parent_job_id, child_job_id)
    VALUES (@ParentJobID, @JobID)
    EXEC issdss_job_loop_Read @JobID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_loop_Read')
DROP PROCEDURE issdss_job_loop_Read
GO
CREATE PROCEDURE issdss_job_loop_Read
    @JobID int
AS
BEGIN
	SELECT jl.child_job_id, jl.parent_job_id as id, j.name
	FROM job_loop jl LEFT JOIN job j ON jl.parent_job_id = j.id
	WHERE jl.child_job_id = @JobID
	ORDER by j.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_loop_Read_All')
DROP PROCEDURE issdss_job_loop_Read_All
GO
CREATE PROCEDURE issdss_job_loop_Read_All
    @TaskID int,
    @JobID int
AS
BEGIN
	SELECT	j.id, j.name
	FROM	task t LEFT JOIN
			alternative a ON t.id = a.task_id LEFT JOIN
			job j ON a.id = j.alternative_id
	WHERE	t.id = @TaskID AND
			EXISTS(SELECT id FROM job WHERE id = j.id) AND
			j.id != @JobID AND
			NOT EXISTS(SELECT 1 FROM job_loop jl WHERE jl.child_job_id = @JobID AND jl.parent_job_id = j.id)
	ORDER by j.name
END
GO



----------------------------------------------------------  job_resource  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_resource_Read')
DROP PROCEDURE issdss_job_resource_Read
GO
CREATE PROCEDURE issdss_job_resource_Read
    @TaskID int,
    @PersonID int,
    @JobID int
AS
BEGIN

    IF (EXISTS (SELECT 1
				FROM person_role pr LEFT JOIN role r ON pr.role_id = r.id LEFT JOIN role_permission rp ON r.id = rp.role_id
				WHERE pr.person_id = @PersonID AND r.task_id = @TaskID AND rp.permission_id = 13))
		BEGIN
			SELECT  r.id, r.name, jr.value, '1' as is_enabled
			FROM	resource r LEFT JOIN
					job_resource jr ON r.id = jr.resource_id AND (jr.job_id = @JobID OR jr.job_id IS NULL)
			WHERE r.task_id = @TaskID
			ORDER by r.ord
		END
	ELSE
		BEGIN
			SELECT  r.id, r.name, jr.value, '0' as is_enabled
			FROM	resource r LEFT JOIN
					job_resource jr ON r.id = jr.resource_id AND (jr.job_id = @JobID OR jr.job_id IS NULL)
			WHERE r.task_id = @TaskID
			ORDER by r.ord
		END

END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_job_resource_Update')
DROP PROCEDURE issdss_job_resource_Update
GO
CREATE PROCEDURE issdss_job_resource_Update
    @d xml,
    @JobID int
AS
BEGIN
    DECLARE @max_id int
   
    CREATE TABLE #t( resource_id int, value decimal(18,6))
   
    DELETE job_resource
    WHERE job_id = @JobID
           
    INSERT INTO #t (resource_id, value)
    SELECT
                    d.r.value('resource_id[1]', 'int') resource_id,
                    d.r.value('value[1]', 'decimal(18,6)') value
    FROM
                    @d.nodes('/job_resource/row') d(r)  

    SELECT @max_id = ISNULL(( SELECT MAX(id) FROM job_resource),0)
   
    INSERT INTO job_resource (id, resource_id, job_id, value )
    SELECT
                    ROW_NUMBER() OVER( ORDER BY t.resource_id) + @max_id,
                    t.resource_id,
                    @JobID,
                    t.value
    FROM
                    #t t

    DROP TABLE #t
END
GO



----------------------------------------------------------  measure  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_measure_Read_All')
DROP PROCEDURE issdss_measure_Read_All
GO
CREATE PROCEDURE issdss_measure_Read_All
AS
BEGIN
    SELECT id, name, code
    FROM dbo.measure
END
GO



------------------------------------------------------  method  ------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_method_Read_All')
DROP PROCEDURE issdss_method_Read_All
GO
CREATE PROCEDURE issdss_method_Read_All
AS
BEGIN
    SELECT id, name, url FROM method ORDER BY id
END
GO



------------------------------------------------------  pair_crit_comp  ------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_pair_crit_comp_Read')
DROP PROCEDURE issdss_pair_crit_comp_Read
GO
CREATE PROCEDURE issdss_pair_crit_comp_Read
    @parent_crit_id int = NULL
AS
BEGIN
    SELECT    ROW_NUMBER()OVER( ORDER BY pcc.id) as    number,
        c1.id                    criteria1_id,
        c1.name                                 criteria1_name,
        c2.id                    criteria2_id,
        c2.name                                 criteria2_name,
        pcc.rank                rank
    FROM    criteria c1
            join criteria c2 on c2.parent_crit_id = @parent_crit_id
            left join pair_crit_comp pcc on pcc.criteria1_id = c1.id and pcc.criteria2_id = c2.id
    WHERE    c1.parent_crit_id = @parent_crit_id and c1.id < c2.id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_pair_crit_comp_Update')
DROP PROCEDURE issdss_pair_crit_comp_Update
GO
CREATE PROCEDURE issdss_pair_crit_comp_Update
    @d xml,
    @parent_crit_id int = NULL
AS
BEGIN
        DECLARE @max_id int
       
        create table #t( criteria1_id int, value decimal(18,4), criteria2_id int)
       
        DELETE pair_crit_comp
        WHERE EXISTS (
                SELECT    c.id
                FROM    criteria c
                WHERE    c.parent_crit_id = @parent_crit_id AND (
                        c.id = pair_crit_comp.criteria1_id OR
                        c.id = pair_crit_comp.criteria2_id) )
               
        insert into #t (criteria1_id, value, criteria2_id)
        SELECT
                        d.r.value('criteria1_id[1]', 'int') criteria1_id,
                        d.r.value('value[1]', 'decimal(18,4)') value,
                        d.r.value('criteria2_id[1]', 'int') criteria2_id
        FROM
                        @d.nodes('/crit_value/row') d(r)  

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM pair_crit_comp),0)
       
        INSERT INTO pair_crit_comp
        ( id, criteria1_id, criteria2_id, rank )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY t.criteria1_id) + @max_id,
                        t.criteria1_id,
                        t.criteria2_id,
                        t.value
        from
                        #t t

        drop table #t
END
GO



--------------------------------------------------------  permission  -----------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_permission_Read')
DROP PROCEDURE issdss_permission_Read
GO
CREATE PROCEDURE issdss_permission_Read
	@TaskID int = NULL,
	@PersonID int = NULL
AS
BEGIN
	SELECT p.name AS name, NULL AS task, 0 AS permission_id, 0 AS part
	FROM person p
    WHERE p.id = @PersonID
    UNION
	SELECT	NULL AS name,
			case
				when exists(select 1 from task where id = @TaskID) then (select name from task where id = @TaskID)
				else 'Задача не назначена'
			end AS task,
			0 AS permission_id,
			1 AS part
    UNION
    SELECT NULL AS name, NULL AS task, rp.permission_id AS permission_id, 2 AS part
    FROM person_role pr LEFT JOIN role r ON pr.role_id = r.id LEFT JOIN role_permission rp ON r.id = rp.role_id    
    WHERE pr.person_id = @PersonID AND r.task_id = @TaskID AND rp.permission_id IS NOT NULL
    ORDER BY part
END
GO



----------------------------------------------------------  person  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_Delete')
DROP PROCEDURE issdss_person_Delete
GO
CREATE PROCEDURE issdss_person_Delete
    @PersonID int
AS
BEGIN
    DELETE FROM dbo.person_role
    WHERE person_id = @PersonID
    DELETE FROM dbo.person_alternative
    WHERE person_id = @PersonID
    DELETE FROM dbo.person_task
    WHERE person_id = @PersonID
    DELETE FROM dbo.person
    WHERE id = @PersonID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_Insert')
DROP PROCEDURE issdss_person_Insert
GO
CREATE PROCEDURE issdss_person_Insert
   @Name varchar(255),
   @Login varchar(30),
   @Password varchar(255)
AS
BEGIN
   SET NOCOUNT ON;
   DECLARE @ID int
   SET @ID = isnull((select max(id) from person),0) + 1
   INSERT INTO dbo.person(id, [name], [login], [password])
   VALUES (@ID, @Name, @Login, @Password)
   SELECT @ID as ID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_Read')
DROP PROCEDURE issdss_person_Read
GO
CREATE PROCEDURE issdss_person_Read
    @Login varchar(30) = NULL,
    @Password varchar(255) = NULL
AS
BEGIN
    SELECT p.id, p.name, p.login, pt.task_id
    FROM person p LEFT JOIN person_task pt ON p.id = pt.person_id
    WHERE p.login = @Login AND p.password = @Password
    ORDER BY pt.task_id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_Read_All')
DROP PROCEDURE issdss_person_Read_All
GO
CREATE PROCEDURE issdss_person_Read_All
    @TaskID int = NULL
AS
BEGIN
	SELECT p.id, p.name, p.login,
		replace(replace(replace((select t.name as q from person_task pt LEFT JOIN task t on pt.task_id = t.id where pt.person_id = p.id order by t.name for xml path('')),'</q><q>',', '),'<q>',''),'</q>','') as tasks,
		replace(replace(replace((select r.name as q from person_role pr LEFT JOIN role r on pr.role_id = r.id where pr.person_id = p.id AND r.task_id = @TaskID order by r.name for xml path('')),'</q><q>',', '),'<q>',''),'</q>','') as roles
	FROM person p
	ORDER by name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_personDDL_Read_All')
DROP PROCEDURE issdss_personDDL_Read_All
GO
CREATE PROCEDURE issdss_personDDL_Read_All
    @TaskID int = NULL
AS
BEGIN
    SELECT p.id, p.name
    FROM person_task pt LEFT JOIN person p ON pt.person_id = p.id
    WHERE pt.task_id = @TaskID
    ORDER by name
END
GO



----------------------------------------------------  person_alternative  -------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_alternative_Read')
DROP PROCEDURE issdss_person_alternative_Read
GO
CREATE PROCEDURE issdss_person_alternative_Read
    @TaskID int = NULL,
    @PersonID int = NULL
AS
BEGIN
    SELECT 0 AS id, (SELECT name FROM dbo.person WHERE id = @PersonID) AS name, 0 AS isChecked, 0 AS part
    UNION
    SELECT  a.id AS id,
            a.name AS name,
            case
                when exists(select 1
                            from person_alternative
                            where person_id = @PersonID AND alternative_id = a.id) then 1
                else 0
            end AS isChecked,
            1 AS part
    FROM alternative a
    WHERE task_id = @TaskID
    ORDER by part, name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_alternative_Update')
DROP PROCEDURE issdss_person_alternative_Update
GO
CREATE PROCEDURE issdss_person_alternative_Update
    @TaskID int = NULL,
    @PersonID int,
    @Alter_ids varchar(max)
AS
BEGIN
        DECLARE @max_id int
       
        DELETE    person_alternative FROM alternative a
        WHERE    person_alternative.alternative_id = a.id AND
                person_alternative.person_id = @PersonID AND
                a.task_id = @TaskID
                
        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_alternative),0)
       
        INSERT INTO person_alternative
        ( id, person_id, alternative_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY a.id) + @max_id,
                        @PersonID,
                        a.id
        FROM
                        alternative a
        WHERE
                        --a.task_id = @TaskID and
                        @Alter_ids like '%,' + CONVERT(varchar(10), a.id) + ',%'                        

        --DELETE crit_value WHERE
        --person_id = @PersonID AND
        --alternative_id not in (SELECT alternative_id FROM person_alternative WHERE person_id = @PersonID)
END
GO



----------------------------------------------------  person_role  -------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_role_Read')
DROP PROCEDURE issdss_person_role_Read
GO
CREATE PROCEDURE issdss_person_role_Read
    @TaskID int = NULL,
    @PersonID int = NULL
AS
BEGIN
    SELECT 0 AS id, (SELECT name FROM dbo.person WHERE id = @PersonID) AS name, 0 AS isChecked, 0 AS part
    UNION
    SELECT  r.id AS id,
            r.name AS name,
            case
                when exists(select 1
                            from person_role
                            where person_id = @PersonID AND role_id = r.id) then 1
                else 0
            end AS isChecked,
            1 AS part
    FROM role r
    WHERE task_id = @TaskID
    ORDER by part, name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_role_Update')
DROP PROCEDURE issdss_person_role_Update
GO
CREATE PROCEDURE issdss_person_role_Update
    @TaskID int = NULL,
    @PersonID int,
    @Role_ids varchar(max)
AS
BEGIN
        DECLARE @max_id int
       
        DELETE    person_role FROM role r
        WHERE    person_role.role_id = r.id AND
                person_role.person_id = @PersonID AND
                r.task_id = @TaskID

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_role),0)
       
        INSERT INTO person_role
        ( id, person_id, role_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY r.id) + @max_id,
                        @PersonID,
                        r.id
        FROM
                        role r
        WHERE
                        @Role_ids like '%,' + CONVERT(varchar(10), r.id) + ',%'                        
END
GO



----------------------------------------------------  person_task  -------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_task_Read')
DROP PROCEDURE issdss_person_task_Read
GO
CREATE PROCEDURE issdss_person_task_Read
    @UserID int = NULL,
    @TaskID int = NULL,
    @PersonID int = NULL
AS
BEGIN
    IF EXISTS(SELECT 1 FROM person_role pr LEFT JOIN
                            role_permission rp ON pr.role_id = rp.role_id LEFT JOIN
                            role r ON pr.role_id = r.id
                        WHERE    pr.person_id = @UserID AND
                                rp.permission_id = 71 AND         
                                r.task_id = @TaskID)
    BEGIN
        SELECT 0 AS id, (SELECT name FROM person WHERE id = @PersonID) AS name, 0 AS isChecked, 0 AS part
        UNION
        SELECT  t.id AS id,
                t.name AS name,
                case
                    when exists(select 1
                                from person_task
                                where person_id = @PersonID AND task_id = t.id) then 1
                    else 0
                end AS isChecked,
                1 AS part
        FROM task t
        ORDER by part, id
    END
    ELSE
    BEGIN
        SELECT 0 AS id, (SELECT name FROM person WHERE id = @PersonID) AS name, 0 AS isChecked, 0 AS part
        UNION
        SELECT  t.id AS id,
                t.name AS name,
                case
                    when exists(select 1
                                from person_task
                                where person_id = @PersonID AND task_id = t.id) then 1
                    else 0
                end AS isChecked,
                1 AS part
        FROM task t
        WHERE EXISTS(SELECT 1 FROM person_task WHERE person_id = @PersonID AND task_id = t.id)
        ORDER by part, id
    END
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_person_task_Update')
DROP PROCEDURE issdss_person_task_Update
GO
CREATE PROCEDURE issdss_person_task_Update
    @PersonID int,
    @Task_ids varchar(max)
AS
BEGIN
    DECLARE @max_id int
   
    DELETE person_task WHERE person_id = @PersonID

    SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_task),0)
   
    INSERT INTO person_task
        ( id, person_id, task_id )
    SELECT
                    ROW_NUMBER() OVER( ORDER BY t.id) + @max_id,
                    @PersonID,
                    t.id
    FROM
                    task t
    WHERE
                    @Task_ids like '%,' + CONVERT(varchar(10), t.id) + ',%'                        

    SELECT pt.task_id
    FROM person_task pt
    WHERE pt.person_id = @PersonID
                        
END
GO



----------------------------------------------------------  plan_job  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_job_Read')
DROP PROCEDURE issdss_plan_job_Read
GO
CREATE PROCEDURE issdss_plan_job_Read
    @JobID int,
    @PlanID int
AS
BEGIN
	IF exists(select 1 from plan_job where job_id = @JobID AND plan_id = @PlanID)
	begin
		SELECT p.id, p.plan_id, p.job_id, p.begin_date, p.end_date, j.duration, j.measure_id
		FROM job j LEFT JOIN plan_job p ON p.job_id = j.id
		WHERE j.id = @JobID AND p.plan_id = @PlanID
	end
	else
	begin
		SELECT null as id, null as plan_id, j.id as job_id, null as begin_date, null as end_date, j.duration, j.measure_id
		FROM job j
		WHERE j.id = @JobID
	end
    --SELECT p.id, p.plan_id, p.job_id, p.begin_date, p.end_date, j.duration, j.measure_id
    --FROM job j LEFT JOIN plan_job p ON p.job_id = j.id
    --WHERE plan_id = @PlanID AND job_id = @JobID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_job_Update')
DROP PROCEDURE issdss_plan_job_Update
GO
CREATE PROCEDURE issdss_plan_job_Update
    @JobID int = NULL,
    @PlanID int = NULL,
    @StartDate datetime = NULL,
    @EndDate datetime = NULL
AS
BEGIN
    DELETE FROM plan_job
    WHERE plan_id = @PlanID AND job_id = @JobID
    
    DECLARE @ID int
    SET @ID = isnull((select max(id) from plan_job),0) + 1
    INSERT INTO plan_job(id, plan_id, job_id, begin_date, end_date)
    VALUES (@ID, @PlanID, @JobID, @StartDate, @EndDate)
END
GO



----------------------------------------------------------  plan_loop  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_loop_Read')
DROP PROCEDURE issdss_plan_loop_Read
GO
CREATE PROCEDURE issdss_plan_loop_Read
    @PlanID int
AS
BEGIN
	SELECT pl.child_plan_id, pl.parent_plan_id as id, p.name
	FROM plan_loop pl LEFT JOIN plandss p ON pl.parent_plan_id = p.id
	WHERE pl.child_plan_id = @PlanID
	ORDER by p.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_loop_Delete')
DROP PROCEDURE issdss_plan_loop_Delete
GO
CREATE PROCEDURE issdss_plan_loop_Delete
    @PlanID int,
    @ParentPlanID int
AS
BEGIN
    DELETE FROM plan_loop
    WHERE parent_plan_id = @ParentPlanID AND child_plan_id = @PlanID
    EXEC issdss_plan_loop_Read @PlanID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_loop_Insert')
DROP PROCEDURE issdss_plan_loop_Insert
GO
CREATE PROCEDURE issdss_plan_loop_Insert
    @PlanID int,
    @ParentPlanID int
AS
BEGIN
    INSERT INTO plan_loop(parent_plan_id, child_plan_id)
    VALUES (@ParentPlanID, @PlanID)
    EXEC issdss_plan_loop_Read @PlanID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_loop_Read_All')
DROP PROCEDURE issdss_plan_loop_Read_All
GO
CREATE PROCEDURE issdss_plan_loop_Read_All
    @TaskID int,
    @PlanID int
AS
BEGIN
	SELECT	p.id, p.name
	FROM	plandss p
	WHERE	p.task_id = @TaskID AND
			p.id != @PlanID AND
			NOT EXISTS(SELECT 1 FROM plan_loop pl WHERE pl.child_plan_id = @PlanID AND pl.parent_plan_id = p.id)
	ORDER by p.name
END
GO



----------------------------------------------------------  plan_method  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plan_method_Read_All')
DROP PROCEDURE issdss_plan_method_Read_All
GO
CREATE PROCEDURE issdss_plan_method_Read_All
AS
BEGIN
    SELECT id, name
    FROM plan_method
    ORDER BY id
END
GO



----------------------------------------------------------  plandss  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plandss_Delete')
DROP PROCEDURE issdss_plandss_Delete
GO
CREATE PROCEDURE issdss_plandss_Delete
    @PlanID int
AS
BEGIN
    DELETE FROM plandss
    WHERE id = @PlanID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plandss_Insert')
DROP PROCEDURE issdss_plandss_Insert
GO
CREATE PROCEDURE issdss_plandss_Insert
    @TaskID int,
    @Name varchar(255),
    @MethodID int = NULL,
    @BeginDate datetime,
    @EndDate datetime,
    @MeasureID int = NULL,
    @IsReady decimal(18,6),
    @Alfa decimal(18,6),
    --@FuncValue decimal(18,6),
    @ReservPercent decimal(18,4)
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from plandss),0) + 1
    INSERT INTO plandss(id, name, task_id, plan_method_id, begin_date, end_date, measure_id, isready, alfa, reserv_percent)
    VALUES (@ID, @Name, @TaskID, @MethodID, @BeginDate, @EndDate, @MeasureID, @IsReady, @Alfa, @ReservPercent)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plandss_Read')
DROP PROCEDURE issdss_plandss_Read
GO
CREATE PROCEDURE issdss_plandss_Read
    @PlanID int
AS
BEGIN
    SELECT id, name, plan_method_id, begin_date, end_date, measure_id, isready, alfa, func_value, reserv_percent
    FROM plandss
    WHERE id = @PlanID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plandss_Read_All')
DROP PROCEDURE issdss_plandss_Read_All
GO
CREATE PROCEDURE issdss_plandss_Read_All
    @TaskID int = NULL
AS
BEGIN
	SELECT  p.id, p.name, pm.id as method_id, pm.name as method, p.begin_date, p.end_date, m.id as measure_id, m.name as measure, p.isready, p.alfa, p.func_value, p.reserv_percent
	FROM	plandss p LEFT JOIN 
			measure m ON p.measure_id = m.id LEFT JOIN
			plan_method pm ON p.plan_method_id = pm.id
	WHERE p.task_id = @TaskID
	ORDER by p.ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_plandss_Update')
DROP PROCEDURE issdss_plandss_Update
GO
CREATE PROCEDURE issdss_plandss_Update
    @PlanID int,
    @Name varchar(255),
    @MethodID int = NULL,
    @BeginDate datetime,
    @EndDate datetime,
    @MeasureID int = NULL,
    @IsReady decimal(18,6),
    @Alfa decimal(18,6),
    --@FuncValue decimal(18,6),
    @ReservPercent decimal(18,4)
AS
BEGIN
    UPDATE plandss
        SET name = @Name,
			plan_method_id = @MethodID,
			begin_date = @BeginDate,
			end_date = @EndDate,
			measure_id = @MeasureID,
			isready = @IsReady,
			alfa = @Alfa,
			--func_value = @FuncValue,
			reserv_percent = @ReservPercent
    WHERE id = @PlanID
END
GO



----------------------------------------------------------  resourse  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_resource_Delete')
DROP PROCEDURE issdss_resource_Delete
GO
CREATE PROCEDURE issdss_resource_Delete
    @ResourceID int
AS
BEGIN
    DELETE FROM job_resource
    WHERE resource_id = @ResourceID
    DELETE FROM resource
    WHERE id = @ResourceID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_resource_Insert')
DROP PROCEDURE issdss_resource_Insert
GO
CREATE PROCEDURE issdss_resource_Insert
    @Name varchar(255),
    @TaskID int,
    @Description varchar(2000),
    @Value decimal(18,6),
    @Period int,
    @MeasureID int = NULL,
    @CriteriaID int = NULL
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from resource),0) + 1
    INSERT INTO resource(id, name, task_id, description, value, period, measure_id, criteria_id)
    VALUES (@ID, @Name, @TaskID, @Description, @Value, @Period, @MeasureID, @CriteriaID)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_resource_Read')
DROP PROCEDURE issdss_resource_Read
GO
CREATE PROCEDURE issdss_resource_Read
    @ResourceID int
AS
BEGIN
    SELECT id, name, description, value, period, measure_id, criteria_id
    FROM resource
    WHERE id = @ResourceID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_resource_Read_All')
DROP PROCEDURE issdss_resource_Read_All
GO
CREATE PROCEDURE issdss_resource_Read_All
    @TaskID int = NULL
AS
BEGIN
	SELECT r.id, r.name, r.description, r.value, r.period, r.measure_id, m.name m_name, r.criteria_id, c.name c_name
	FROM	resource r LEFT JOIN 
			measure m ON r.measure_id = m.id LEFT JOIN
			criteria c ON r.criteria_id = c.id
	WHERE r.task_id = @TaskID
	ORDER by r.ord
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_resource_Update')
DROP PROCEDURE issdss_resource_Update
GO
CREATE PROCEDURE issdss_resource_Update
    @ResourceID int,
    @Name varchar(255),
    @Description varchar(2000),
    @Value decimal(18,6),
    @Period int,
    @MeasureID int = NULL,
    @CriteriaID int = NULL
AS
BEGIN
    UPDATE resource
        SET name = @Name,
			description = @Description,
			value = @Value,
			period = @Period,
			measure_id = @MeasureID,
			criteria_id = @CriteriaID
    WHERE id = @ResourceID
END
GO



----------------------------------------------------------  role  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_Delete')
DROP PROCEDURE issdss_role_Delete
GO
CREATE PROCEDURE issdss_role_Delete
    @RoleID int
AS
BEGIN
    DELETE FROM role
    WHERE id = @RoleID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_Insert')
DROP PROCEDURE issdss_role_Insert
GO
CREATE PROCEDURE issdss_role_Insert
    @TaskID int,
    @Name varchar(255)
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from role),0) + 1
    INSERT INTO role(id, task_id, name)
    VALUES (@ID, @TaskID, @Name)
END
GO
 
 

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_Read')
DROP PROCEDURE issdss_role_Read
GO
CREATE PROCEDURE issdss_role_Read
    @RoleID int
AS
BEGIN
    SELECT id, name
    FROM role
    WHERE id = @RoleID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_Read_All')
DROP PROCEDURE issdss_role_Read_All
GO
CREATE PROCEDURE issdss_role_Read_All
    @TaskID int = NULL
AS
BEGIN
	SELECT r.id, r.name
	FROM role r
	WHERE r.task_id = @TaskID
	ORDER by r.name
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_Update')
DROP PROCEDURE issdss_role_Update
GO
CREATE PROCEDURE issdss_role_Update
    @RoleID int,
    @Name varchar(255)
AS
BEGIN
    UPDATE role
        SET name = @Name
    WHERE id = @RoleID
END
GO



----------------------------------------------------  role_permission  -------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_permission_Read')
DROP PROCEDURE issdss_role_permission_Read
GO
CREATE PROCEDURE issdss_role_permission_Read
    @RoleID int = NULL
AS
BEGIN
    SELECT 0 AS id, (SELECT name FROM role WHERE id = @RoleID) AS name, 0 AS isChecked, 0 AS part
    UNION
    SELECT  p.id AS id,
            p.name AS name,
            case
                when exists(select 1
                            from role_permission
                            where role_id = @RoleID AND permission_id = p.id) then 1
                else 0
            end AS isChecked,
            1 AS part
    FROM permission p
    ORDER by part, id
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_permission_Update')
DROP PROCEDURE issdss_role_permission_Update
GO
CREATE PROCEDURE issdss_role_permission_Update
    @RoleID int,
    @Permission_ids varchar(max)
AS
BEGIN
        DECLARE @max_id int
       
        DELETE role_permission WHERE  role_id = @RoleID

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM role_permission),0)
       
        INSERT INTO role_permission
        ( id, role_id, permission_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY p.id) + @max_id,
                        @RoleID,
                        p.id
        FROM
                        permission p
        WHERE
                        @Permission_ids like '%,' + CONVERT(varchar(10), p.id) + ',%'                        
END
GO



----------------------------------------------------  role_person  -------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_person_Read')
DROP PROCEDURE issdss_role_person_Read
GO
CREATE PROCEDURE issdss_role_person_Read
    @TaskID int = NULL,
    @RoleID int = NULL
AS
BEGIN
    SELECT 0 AS id, (SELECT name FROM role WHERE id = @RoleID) AS name, 0 AS isChecked, 0 AS part
    UNION
    SELECT  p.id AS id,
            p.name AS name,
            case
                when exists(select 1
                            from person_role
                            where role_id = @RoleID AND person_id = p.id) then 1
                else 0
            end AS isChecked,
            1 AS part
    FROM person p
    WHERE EXISTS(select 1 from person_task where task_id = @TaskID AND person_id = p.id)
    ORDER by part, name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_role_person_Update')
DROP PROCEDURE issdss_role_person_Update
GO
CREATE PROCEDURE issdss_role_person_Update
    @RoleID int,
    @Person_ids varchar(max)
AS
BEGIN
        DECLARE @max_id int
       
        DELETE person_role WHERE  role_id = @RoleID

        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_role),0)
       
        INSERT INTO person_role
        ( id, person_id, role_id )
        SELECT
                        ROW_NUMBER() OVER( ORDER BY p.id) + @max_id,
                        p.id,
                        @RoleID
        FROM
                        person p
        WHERE
                        @Person_ids like '%,' + CONVERT(varchar(10), p.id) + ',%'                        
END
GO



----------------------------------------------------------  task  ----------------------------------------------------------



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_Delete')
DROP PROCEDURE issdss_task_Delete
GO
CREATE PROCEDURE issdss_task_Delete
    @TaskID int
AS
BEGIN
    DELETE FROM person_task WHERE task_id = @TaskID
    DELETE FROM dbo.task WHERE id = @TaskID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_Insert')
DROP PROCEDURE issdss_task_Insert
GO
CREATE PROCEDURE issdss_task_Insert
    @Name varchar(255),
    @UserID int
AS
BEGIN
    DECLARE @ID int
    SET @ID = isnull((select max(id) from task),0) + 1
    INSERT INTO dbo.task(id, [name])
    VALUES (@ID, @Name)
    
    DECLARE @IDPT int
    SET @IDPT = isnull((select max(id) from person_task),0) + 1
    INSERT INTO dbo.person_task(id, task_id, person_id)
    VALUES (@IDPT, @ID, @UserID)
    
    DECLARE @IDR int
    SET @IDR = isnull((select max(id) from role),0) + 1
    INSERT INTO dbo.role(id, task_id, name)
    VALUES (@IDR, @ID, 'Администратор')

    DECLARE @max_id int
    SELECT @max_id = ISNULL(( SELECT MAX(id) FROM role_permission),0)
    INSERT INTO role_permission ( id, role_id, permission_id )
    SELECT
        ROW_NUMBER() OVER( ORDER BY p.id) + @max_id,
        @IDR,
        p.id
    FROM
        permission p
    
    DECLARE @IDPR int
    SET @IDPR = isnull((select max(id) from person_role),0) + 1
    INSERT INTO dbo.person_role(id, role_id, person_id)
    VALUES (@IDPR, @IDR, @UserID)
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_Read')
DROP PROCEDURE issdss_task_Read
GO
CREATE PROCEDURE issdss_task_Read
    @TaskID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id, name
    FROM dbo.task
    WHERE id = @TaskID
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_Read_All')
DROP PROCEDURE issdss_task_Read_All
GO
CREATE PROCEDURE issdss_task_Read_All
    @UserID int = NULL,
    @TaskID int = NULL
AS
BEGIN
	SELECT t.id, t.name
	FROM person_task pt LEFT JOIN task t ON pt.task_id = t.id
	WHERE pt.person_id = @UserID
	ORDER by t.name
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_Update')
DROP PROCEDURE issdss_task_Update
GO
CREATE PROCEDURE issdss_task_Update
    @TaskID int,
    @Name varchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.task
        SET name = @Name
        WHERE id = @TaskID
END
GO



----------------------------------------------------  task_person  -------------------------------------------------
 
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_person_Read')
DROP PROCEDURE issdss_task_person_Read
GO
CREATE PROCEDURE issdss_task_person_Read
    @UserID int = NULL,
    @TaskID int = NULL,
    @CurrentTaskID int = NULL
AS
BEGIN
    IF EXISTS(SELECT 1 FROM person_role pr LEFT JOIN
                            role_permission rp ON pr.role_id = rp.role_id LEFT JOIN
                            role r ON pr.role_id = r.id
                        WHERE    pr.person_id = @UserID AND
                                rp.permission_id = 81 AND         
                                r.task_id = @TaskID)
    BEGIN
        SELECT 0 AS id, (SELECT name FROM task WHERE id = @CurrentTaskID) AS name, 0 AS isChecked, 0 AS part
        UNION
        SELECT  p.id AS id,
                p.name AS name,
                case
                    when exists(select 1
                                from person_task
                                where task_id = @CurrentTaskID AND person_id = p.id) then 1
                    else 0
                end AS isChecked,
                1 AS part
        FROM person p
        ORDER by part, id
    END
    ELSE
    BEGIN
        SELECT 0 AS id, (SELECT name FROM task WHERE id = @CurrentTaskID) AS name, 0 AS isChecked, 0 AS part
        UNION
        SELECT  p.id AS id,
                p.name AS name,
                case
                    when exists(select 1
                                from person_task
                                where task_id = @CurrentTaskID AND person_id = p.id) then 1
                    else 0
                end AS isChecked,
                1 AS part
        FROM person p
        WHERE EXISTS(SELECT 1 FROM person_task WHERE task_id = @CurrentTaskID AND person_id = p.id)
        ORDER by part, id
    END
END
GO
 
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'issdss_task_person_Update')
DROP PROCEDURE issdss_task_person_Update
GO
CREATE PROCEDURE issdss_task_person_Update
    @TaskID int,
    @PersonID int = NULL,
    @Person_ids varchar(max)
AS
BEGIN
    DECLARE @max_id int
   
    DELETE person_task WHERE task_id = @TaskID

    SELECT @max_id = ISNULL(( SELECT MAX(id) FROM person_task),0)
   
    INSERT INTO person_task
        ( id, person_id, task_id )
    SELECT
                    ROW_NUMBER() OVER( ORDER BY p.id) + @max_id,
                    p.id,
                    @TaskID
    FROM
                    person p
    WHERE
                    @Person_ids like '%,' + CONVERT(varchar(10), p.id) + ',%'                        

    SELECT pt.task_id
    FROM person_task pt
    WHERE pt.person_id = @PersonID
                        
END
GO
 

alter FUNCTION [dbo].[GetFuzzyValueEstimation]
(
	@critId int, 
	@value float
)
RETURNS float
AS
BEGIN
	DECLARE @result float;
	WITH CTE AS (
		SELECT X = CAST([name] AS float),
			   Y = [rank]
		FROM [issdss].[dbo].[crit_scale]
		WHERE criteria_id = @critId
	),
	CTE2 as (
		SELECT CTE.*, LeftDiff = case when X <= @value then @value - X else NULL end, 
		              RightDiff = case when X >= @value then  X - @value else NULL end
		FROM CTE
	),
	CTE3 as (
		SELECT X, Y, row_number () over (order by (select X)) RN
		FROM CTE2
		WHERE LeftDiff = (SELECT MIN(LeftDiff) FROM CTE2)
			  OR RightDiff = (SELECT MIN(RightDiff) FROM CTE2)
	),
	CTE4 AS (
		SELECT 
		  MAX(case when RN = 1 then X end) as X1,
		  MAX(case when RN = 2 then X end) as X2,
		  MAX(case when RN = 1 then Y end) as Y1,
		  MAX(case when RN = 2 then Y end) as Y2
		FROM CTE3
	)
	SELECT @result = MAX([dbo].[InterpolateLine](CTE4.X1, CTE4.Y1, CTE4.X2, CTE4.Y2, @value))
	FROM CTE4	
	RETURN @result
END

GO

alter FUNCTION [dbo].[InterpolateLine]
(
	@x1 float,
	@y1 float,
	@x2 float,
	@y2 float,
	@x float
)
RETURNS float
AS
BEGIN
	IF (@y1 is null)
		RETURN @y2;
	IF (@y2 is null)
		RETURN @y1;
	RETURN @y1 + ((@y2 - @y1)/(@x2 - @x1))*(@x - @x1)
END

GO

 
 
---------------------------------------------  ГЛАВНАЯ ПРОЦЕДУРА РАНЖИРОВАНИЯ  ------------------------------------------



IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'prc_calculate_task')
DROP PROCEDURE prc_calculate_task
GO
create PROCEDURE [dbo].[prc_calculate_task]
    @task_id int = NULL
AS
BEGIN
        DECLARE @lev    int -- текущий уровень
        DECLARE @max_id int
       
        CREATE TABLE #cr( id int, rank decimal(18,6)) -- МПС
        CREATE TABLE #t(                                                          -- Критерии
                id int,
                lev int,
                method_id int,
                max_value decimal(18,6),
                min_value decimal(18,6),
                primary key(id)
        )                        

        UPDATE
                pair_crit_comp
        SET
                rank = rank + 0.1
        WHERE
                rank = 0
               
        UPDATE
                pair_crit_comp
        SET
                rank = rank - 0.1
        WHERE
                rank = 100
                               
        INSERT INTO #cr( id, rank)
        SELECT
                criteria1_id,
                rank/(100-rank)
        FROM pair_crit_comp
        UNION ALL
        SELECT
                criteria2_id,
                (100-rank)/rank
        FROM pair_crit_comp
       
        -- Корень n-й степени из произведения
        UPDATE
                criteria
        SET
                rank = exp((SELECT      sum(log(#cr.rank))/count(*)
                                FROM    #cr
                                WHERE   #cr.id = criteria.id                    
                                ))
        WHERE
                task_id = @task_id and
                EXISTS( SELECT 1 FROM #cr WHERE #cr.id = criteria.id)  
       
        -- Допущение о равной важности
        UPDATE
                criteria
        SET
                rank = 1
        WHERE
                task_id = @task_id and
                rank is NULL
       
        -- Нормировка
        UPDATE
                criteria
        SET
                rank = rank / ISNULL((SELECT SUM(i.rank)
                                                FROM criteria i
                                                WHERE ISNULL(i.parent_crit_id,0) =
                                                      ISNULL(criteria.parent_crit_id,0) AND
                                                      i.rank > 0
                                        ),1)
        WHERE
                task_id = @task_id
               

        -- Удалить все расчетные значения
        DELETE
                crit_value
        FROM
                criteria c
        WHERE
                crit_value.criteria_id = c.id AND
                c.task_id = @task_id AND
                crit_value.person_id IS NULL
       
        SELECT @max_id = ISNULL(( SELECT MAX(id) FROM crit_value),0)

        INSERT INTO crit_value( id, criteria_id, alternative_id )
        SELECT
                ROW_NUMBER() OVER( ORDER BY a.id, c.id) + @max_id,
                c.id,
                a.id
        FROM
                criteria c,
                alternative a
        WHERE
                a.task_id = @task_id AND
                c.task_id = @task_id
                       
        SELECT @lev = 0
        -- Корни
        INSERT INTO #t ( id, method_id, lev )
        SELECT id, method_id, @lev
        FROM criteria
        WHERE task_id = @task_id AND parent_crit_id IS NULL
               
        WHILE EXISTS( SELECT 1 FROM criteria
                      WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = @lev ))
              AND @lev < 10000 -- защита от зацикливания
        BEGIN
                SELECT @lev = @lev + 1
                INSERT INTO #t ( id, method_id, lev)
                SELECT id, method_id, @lev FROM criteria
                WHERE parent_crit_id IN (SELECT id FROM #t WHERE lev = (@lev-1) )
        END
       
        -- От листвы к корням  
        WHILE @lev >=0
        BEGIN
                       
                UPDATE
                        crit_value
                SET
                        value = (
                                SELECT
                                        SUM((cv.value-tt.min_value)*c.rank
                                             /(tt.max_value - tt.min_value)
                                           )
                                FROM
                                        criteria c
                                        join crit_value cv on c.id = cv.criteria_id
                                        join #t tt on tt.id = c.id
                                WHERE
                                        c.parent_crit_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        cv.person_id IS NULL
                        )
                FROM
                        #t
                WHERE
                        #t.id  = crit_value.criteria_id AND
						#t.method_id in ( 1, 2) AND       -- взвешенная сумма
                        #t.lev = @lev AND
                        EXISTS(SELECT 1 FROM criteria c    -- Только если обощенный!
                               WHERE c.parent_crit_id = #t.id)

				-- минимум

				UPDATE
                        crit_value
                SET
                        value = (
                                SELECT
                                        MIN(cv.value)
                                FROM
                                        criteria c
                                        join crit_value cv on c.id = cv.criteria_id
                                        join #t tt on tt.id = c.id
                                WHERE
                                        c.parent_crit_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        cv.person_id IS NULL
                        )
                FROM
                        #t
                WHERE
                        #t.id  = crit_value.criteria_id AND
						#t.method_id = 8 AND
                        #t.lev = @lev

				-------

				
				-- нечеткий метод с функцией принадлежности

				UPDATE
                        crit_value
                SET
                        value = (
                                SELECT TOP 1
                                        [dbo].[GetFuzzyValueEstimation](c.parent_crit_id, cv.value)
                                FROM
                                        criteria c
                                        join crit_value cv on c.id = cv.criteria_id
                                        join #t tt on tt.id = c.id
                                WHERE
                                        c.parent_crit_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        cv.person_id IS NULL
                        )
                FROM
                        #t
                WHERE
                        #t.id  = crit_value.criteria_id AND
						#t.method_id = 7 AND
                        #t.lev = @lev

				-------

                UPDATE
                        crit_value
                SET 
                        value = case when 
                        exists(
                                SELECT
                                        1
                                FROM
                                        criteria c
                                        join crit_value cv on c.id = cv.criteria_id
                                        join #t tt on tt.id = c.id
                                WHERE
										cv.value = 0 AND
                                        c.parent_crit_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        cv.person_id IS NULL
                        )
                         then 0
                        else
                        exp((
                                SELECT
                                        SUM(log(cv.value)*c.rank)
                                FROM
                                        criteria c
                                        join crit_value cv on c.id = cv.criteria_id
                                        join #t tt on tt.id = c.id
                                WHERE
										cv.value <> 0 AND
                                        c.parent_crit_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        cv.person_id IS NULL
                        ))
                        end
                FROM
                        #t
                WHERE
                        #t.id  = crit_value.criteria_id AND
						#t.method_id = 6 AND
                        #t.lev = @lev AND
                        EXISTS(SELECT 1 FROM criteria c    -- Только если обощенный!
                               WHERE c.parent_crit_id = #t.id)
                               
                UPDATE
                        crit_value
                SET
                        value = ISNULL((
                                SELECT
                                        AVG(ISNULL(cs.rank,ISNULL(cv.value,0)))
                                FROM
                                        crit_value cv
                                        left outer join crit_scale cs on cs.id = cv.crit_scale_id
                                WHERE
                                        cv.criteria_id = #t.id AND
                                        cv.alternative_id = crit_value.alternative_id AND
                                        NOT (cv.person_id IS NULL)
                        ),0)
                FROM
                        #t
                WHERE
                        #t.id  = crit_value.criteria_id AND
                        #t.lev = @lev AND
                        crit_value.person_id IS NULL AND
                        NOT EXISTS(SELECT 1 FROM criteria c    -- Только если исходный
                                   WHERE c.parent_crit_id = #t.id)
                                   
                UPDATE
                        #t
                SET
                        max_value = (
                                SELECT
                                        max(ISNULL(cv.value,0))
                                FROM
                                        crit_value cv
                                WHERE
                                        cv.criteria_id = #t.id AND
                                        cv.person_id IS NULL
                        ),
                        min_value = (
                                SELECT
                                        min(ISNULL(cv.value,0))
                                FROM
                                        crit_value cv
                                WHERE
                                        cv.criteria_id = #t.id AND
                                        cv.person_id IS NULL
                        )                      
                WHERE
                        #t.lev = @lev
                       
                UPDATE
                        #t
                SET
                        max_value = max_value + 1  -- Шаман однако
                WHERE
                        (max_value - min_value) < 1
                                                       
                SELECT @lev = @lev - 1
        END
       
        UPDATE
                alternative
        SET
                rank = ( SELECT
                                                AVG(ISNULL(cv.value,0))
                                   FROM
                                                crit_value cv
                                                join #t on #t.id = cv.criteria_id
                                               
                                  WHERE
                                                #t.lev = 0 AND
                                                cv.alternative_id = alternative.id AND
                                                cv.person_id IS NULL
                                )
        WHERE
                task_id = @task_id

        DROP TABLE #cr
        RETURN 1
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'prc_criteria_copy')
DROP PROCEDURE prc_criteria_copy
GO

create procedure prc_criteria_copy( @src_task int, @target_task int)
as
begin
declare @id int

CREATE TABLE #t(
	new_id int,
	src_id int,
	p_id   int
)

select @id = isnull((select max(id) from criteria),0) + 1

insert into #t (new_id, src_id, p_id)
select ROW_NUMBER() OVER( ORDER BY id) + @id, id, parent_crit_id
from criteria
where task_id = @src_task

insert into criteria (id,task_id,name,ismin,idealvalue,method_id,descr)
select #t.new_id, @target_task, c.name, c.ismin, c.idealvalue, c.method_id, c.descr
from #t join criteria c on c.id = #t.src_id

update 
		criteria 
set
		parent_crit_id = tt.new_id
from #t t join #t tt on tt.src_id = t.p_id 
where t.new_id = id

select @id = isnull((select max(id) from crit_scale),0) + 1

insert into crit_scale (id, criteria_id, name,rank,ord)
select 
	ROW_NUMBER() OVER( ORDER BY id) + @id,
	t.new_id,
	c.name,
	c.rank,
	c.ord
from
	crit_scale c join #t t on t.src_id = c.criteria_id            

select @id = isnull((select max(id) from alternative),0) + 1

insert into alternative(id, task_id, name)
select 
	ROW_NUMBER() OVER( ORDER BY id) + @id,
	@target_task,
	name
from
	alternative
where 
	task_id = @src_task
	
drop table #t

end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'get_days')
DROP function get_days
GO
create function dbo.get_days(@measure_id int)
returns int
as
begin
declare @i int

select @i = case @measure_id when 1 then  365
when 2 then  30*31*30
when 3 then  30
else 1
end

return @i
end
go

IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'prc_plandss')
DROP PROCEDURE prc_plandss
GO


create procedure prc_plandss ( @plan_id int = 1)
as
begin
declare @task_id    int
declare @begin_date datetime
declare @end_date   datetime
declare @t          int
declare @max_t      int
declare @min_t      int
declare @job_id     int
declare @duration   int
declare @id         int

create table #job(
	id int,
	t int,
	duration int,
	rank decimal(18,6),
	nores int default 0
)

create table #t(
	t int
)

create table #res(
	id int,
	t1 int,
	t2 int,
	v decimal(18,6)
)

create table #jobres(
	job_id int,
	res_id int,
	v decimal(18,6)
)


select @task_id=task_id, @begin_date=begin_date, @end_date=end_date  
from plandss where id = @plan_id

insert into #job ( id, duration, rank)
select j.id, j.duration, a.rank
from job j
join alternative a on j.alternative_id = a.id
where a.task_id = @task_id

insert into #jobres( job_id, res_id, v)
select jr.job_id, jr.resource_id, case when r.period is null then jr.value * j.duration else jr.value end  
from 
job_resource jr
join resource r on r.id = jr.resource_id
join job j on jr.job_id = j.id
join alternative a on j.alternative_id = a.id
where a.task_id = @task_id


-- все дни
select @max_t = datediff(day,@begin_date, @end_date)+1
set @t = 0
while @t <= @max_t
begin
	insert into #t(t) values (@t)
	set @t = @t +1
end

-- мощностные
insert into #res(id, t1, t2, v)
select r.id, #t.t, #t.t, r.value
from resource r, #t 
where r.task_id = @task_id and isnull(r.period,0) = 0

--складируемые
insert into #res(id, t1, t2, v)
select r.id, #t.t, #t.t + r.period*dbo.get_days(r.measure_id) - 1, r.value
from resource r
     join #t on (#t.t % (r.period*dbo.get_days(r.measure_id) ))=0  
where r.task_id = @task_id and r.period > 0

-- хвост
update
	#res
set
	v = v*(@max_t-t1+1)/(t2-t1+1)
where 
    t2 > @max_t
   

while (1=1)
begin
	set @job_id = (select top 1 j.id 
	               from #job j 
	               where j.t is null and j.nores = 0 and 
	                     not exists( select 1 from #job pj join job_loop jl on jl.parent_job_id = pj.id where jl.child_job_id = j.id and pj.t is null)             
	               order by j.rank DESC, j.id ASC
	              )
	              
	if (@job_id is null) break
	
	select @duration = duration from #job where id = @job_id
	
	set @min_t = (select min(t) 
	              from #t                               -- от t до t+@duration-1 
	              where ( select count(*)  
	                       from #jobres jr join #res on #res.id = jr.res_id   
	                        where jr.job_id = @job_id and #res.t1 <= (#t.t + @duration - 1 ) and #res.t2 >= #t.t 
	                    ) =
	                    ( select count(*)  
	                       from #jobres jr join #res on #res.id = jr.res_id   
	                        where jr.job_id = @job_id and #res.t1 <= (#t.t + @duration - 1 ) and #res.t2 >= #t.t and
	                              #res.v >= case when #res.t1 <= #t.t then
	                                                  case when #res.t2 >= (#t.t + @duration - 1 ) then
	                                                  		1
	                                                  else
	                                                  		convert(decimal(18,6),(#res.t2 - #t.t + 1)) / @duration
	                                                  end
	                                             else
	                                             	  case when #res.t2 >= (#t.t + @duration - 1 ) then
	                                                  		convert(decimal(18,6),((#t.t + @duration - 1 ) - #res.t1 + 1)) / @duration
	                                                  else
	                                                  		convert(decimal(18,6),(#res.t2 - #res.t1 + 1)) / @duration
	                                                  end
	                                        end * jr.v 
	                    ) and
	                    (#t.t + @duration - 1 ) < @max_t 
	             )       
	
    update
    		#job
    set 
    		t = @min_t,
    		nores = case when @min_t is null then 1
    		             else 0
    		        end
    where 
    		id = @job_id
   
   		
   if (@min_t is null) continue
   
   update
   		#res
   set 
   		v = #res.v - case when #res.t1 <= @min_t then
	                           case when #res.t2 >= (@min_t + @duration - 1 ) then
	                                 1
	                           else
	                                 convert(decimal(18,6),(#res.t2 - @min_t + 1)) / @duration
	                           end
	                  else
	                           case when #res.t2 >= (@min_t + @duration - 1 ) then
	                                 convert(decimal(18,6),((@min_t + @duration - 1 ) - #res.t1 + 1)) / @duration
	                           else
	                                 convert(decimal(18,6),(#res.t2 - #res.t1 + 1)) / @duration
	                           end
	            end * jr.v 
   from #jobres jr
   where #res.id = jr.res_id and 
         jr.job_id = @job_id and 
         #res.t1 <= (@min_t + @duration - 1 ) and #res.t2 >= @min_t 
 
end
   
delete plan_job where plan_id = @plan_id

select @id = ISNULL(( select max(id) from plan_job),0)

insert into plan_job( id, plan_id, job_id, begin_date, end_date )
select
                row_number() over( order by id, t) + @id,
                @plan_id,
                id,
                dateadd(day,t,@begin_date),
                dateadd(day,t+duration-1,@begin_date)
from #job
where nores = 0 and t is not null


drop table #res                
drop table #job
drop table #jobres
drop table #t
end
go


