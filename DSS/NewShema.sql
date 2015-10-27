USE [issdss]
GO
/****** Object:  UserDefinedFunction [dbo].[GetFuzzyValueEstimation]    Script Date: 27.10.2015 9:47:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetFuzzyValueEstimation]
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
		WHERE criteria_id = 128
	),
	CTE2 as (
		SELECT CTE.*, LeftDiff = IIF(X <= @value, @value - X, NULL), RightDiff = IIF(X >= @value, X - @value, NULL)
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
/****** Object:  UserDefinedFunction [dbo].[InterpolateLine]    Script Date: 27.10.2015 9:47:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[InterpolateLine]
(
	-- Add the parameters for the function here
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
/****** Object:  StoredProcedure [dbo].[prc_calculate_task]    Script Date: 27.10.2015 9:47:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- select * from task
-- exec prc_calculate_task 3

--select * from crit_value, criteria c
 --       WHERE
  --              crit_value.criteria_id = c.id and task_id = 3


CREATE PROCEDURE [dbo].[prc_calculate_task] @task_id int = 3
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

				-- нечеткий минимум

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
						#t.method_id = 7 AND
                        #t.lev = @lev
						AND (SELECT TOP 1 is_number FROM criteria WHERE crit_value.criteria_id = criteria.id) = 0

				-------

				
				-- нечеткий метод с функцией принадлежности

				UPDATE
                        crit_value
                SET
                        value = (
                                SELECT TOP 1
                                        [dbo].[GetFuzzyValueEstimation](c.id, cv.value)
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
						AND (SELECT TOP 1 is_number FROM criteria WHERE crit_value.criteria_id = criteria.id) = 1

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

				select * from #t
				select * from #cr

        DROP TABLE #cr
        RETURN 1
END

GO
